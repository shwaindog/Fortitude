#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public unsafe class CircularReadWriteBuffer : IBuffer
{
    public const int LargeObjectHeapAllocationSize = 85_000;

    private long bufferShifted;
    private GCHandle? handle;
    private int pinCount;
    private bool shouldUnpin = true;

    public CircularReadWriteBuffer(byte[] buffer)
    {
        Buffer = buffer;
        if (buffer.Length >= LargeObjectHeapAllocationSize) shouldUnpin = false;
    }

    public byte[] Buffer { get; }

    public byte* ReadBuffer
    {
        get
        {
            if (pinCount < 0) pinCount = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            if (shouldUnpin) pinCount++;
            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public byte* WriteBuffer
    {
        get
        {
            if (pinCount < 0) pinCount = 0;
            if (handle is not { IsAllocated: true }) handle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            if (shouldUnpin) pinCount++;

            return (byte*)handle.Value.AddrOfPinnedObject().ToPointer();
        }
    }

    public void Dispose()
    {
        if (shouldUnpin && --pinCount <= 0 && handle is { IsAllocated: true })
        {
            handle.Value.Free();
            handle = null;
        }
    }

    public nint BufferRelativeReadCursor => (nint)(ReadCursor - bufferShifted);
    public nint BufferRelativeWriteCursor => (nint)(WriteCursor - bufferShifted);

    public long ReadCursor { get; set; }
    public long WriteCursor { get; set; }

    public bool AllRead => ReadCursor == WriteCursor;

    public long UnreadBytesRemaining => WriteCursor - ReadCursor;

    public void SetAllRead()
    {
        bufferShifted = ReadCursor = WriteCursor;
    }

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

    public long RemainingStorage => Buffer.Length - BufferRelativeWriteCursor;

    public long Size => Buffer.Length;

    public bool CanWrite(int size) => RemainingStorage > size;

    ~CircularReadWriteBuffer()
    {
        if (handle is not { IsAllocated: true }) return;
        handle.Value.Free();
        handle = null;
    }
}
