#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public unsafe class CircularReadWriteBuffer : IBuffer
{
    private nint bufferShifted;
    private GCHandle? handle;
    private int pinCount;

    public CircularReadWriteBuffer(byte[] buffer) => Buffer = buffer;

    public byte[] Buffer { get; }

    public byte* ReadBuffer
    {
        get
        {
            if (pinCount < 0) pinCount = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer);
            pinCount++;
            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public byte* WriteBuffer
    {
        get
        {
            if (pinCount < 0) pinCount = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer);
            pinCount++;
            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public void Dispose()
    {
        if (--pinCount <= 0 && handle is { IsAllocated: true })
        {
            handle.Value.Free();
            handle = null;
        }
    }

    public nint BufferRelativeReadCursor => ReadCursor - bufferShifted;
    public nint BufferRelativeWriteCursor => WriteCursor - bufferShifted;

    public nint ReadCursor { get; set; }
    public nint WriteCursor { get; set; }

    public bool AllRead => ReadCursor == WriteCursor;

    public nint UnreadBytesRemaining => WriteCursor - ReadCursor;

    public void SetAllRead()
    {
        bufferShifted = ReadCursor = WriteCursor;
    }

    public bool CanWrite(int size) => RemainingStorage > size;

    public void TryHandleRemainingWriteBufferRunningLow()
    {
        if (BufferRelativeReadCursor > 0 && BufferRelativeWriteCursor > 0)
        {
            System.Buffer.BlockCopy(Buffer, (int)BufferRelativeReadCursor, Buffer, 0,
                (int)(BufferRelativeWriteCursor - BufferRelativeReadCursor));
            bufferShifted = ReadCursor;
        }
    }

    public bool HasStorageForBytes(int bytes) => BufferRelativeWriteCursor + bytes <= Buffer.Length;

    public nint RemainingStorage => Buffer.Length - BufferRelativeWriteCursor;

    public int Size => Buffer.Length;

    ~CircularReadWriteBuffer()
    {
        if (handle is not { IsAllocated: true }) return;
        handle.Value.Free();
        handle = null;
    }
}
