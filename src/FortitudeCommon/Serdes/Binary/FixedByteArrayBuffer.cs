// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public interface IFixedByteArrayBuffer : IBuffer
{
    IByteArray                 BackingByteArray          { get; }
    IVirtualMemoryAddressRange BackingMemoryAddressRange { get; }
}

public unsafe class FixedByteArrayBuffer : IFixedByteArrayBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(GrowableUnmanagedBuffer));

    private readonly bool shouldCloseView;

    protected int BufferAccessCounter;

    private bool isDestroyed;
    private long ReadCursorPos;

    protected IVirtualMemoryAddressRange? VirtualMemoryAddressRange;

    protected long WriteCursorPos;

    public FixedByteArrayBuffer(IVirtualMemoryAddressRange virtualMemoryAddressRange, bool shouldCloseView = true)
    {
        VirtualMemoryAddressRange = virtualMemoryAddressRange;
        this.shouldCloseView      = shouldCloseView;
    }


    public virtual long DefaultGrowSize => 0;

    public IByteArray BackingByteArray => (UnmanagedByteArray)VirtualMemoryAddressRange!;

    public IVirtualMemoryAddressRange BackingMemoryAddressRange => VirtualMemoryAddressRange!;

    public long ReadCursor
    {
        get => ReadCursorPos;
        set
        {
            LimitNextDeserialize = null;
            ReadCursorPos        = value;
        }
    }

    public long UnreadBytesRemaining => Math.Min(LimitNextDeserialize ?? WriteCursorPos - ReadCursor, WriteCursorPos - ReadCursor);

    public virtual long WriteCursor
    {
        get => WriteCursorPos;
        set
        {
            LimitNextSerialize = null;
            WriteCursorPos     = value;
        }
    }

    public long RemainingStorage => Math.Min(LimitNextSerialize ?? Length - WriteCursorPos, Length - WriteCursorPos);

    public long? LimitNextSerialize { get; set; }

    public long? LimitNextDeserialize { get; set; }

    public void Dispose()
    {
        Interlocked.Decrement(ref BufferAccessCounter);
    }

    public void Close()
    {
        Dispose();
    }

    public void DestroyBuffer()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        if (shouldCloseView)
        {
            VirtualMemoryAddressRange?.Dispose();
            VirtualMemoryAddressRange = null;
        }
    }

    public byte* ReadBuffer
    {
        get
        {
            if (VirtualMemoryAddressRange == null) return null;
            Interlocked.Increment(ref BufferAccessCounter);
            return VirtualMemoryAddressRange.StartAddress;
        }
    }

    public byte* WriteBuffer
    {
        get
        {
            if (VirtualMemoryAddressRange == null) return null;
            Interlocked.Increment(ref BufferAccessCounter);
            return VirtualMemoryAddressRange.StartAddress;
        }
    }

    public nint BufferRelativeReadCursor => (nint)ReadCursor;

    public nint BufferRelativeWriteCursor => (nint)WriteCursor;

    public bool CanRead  => BufferRelativeReadCursor < Length;
    public bool CanSeek  => true;
    public bool CanWrite => BufferRelativeWriteCursor < Length;

    public long Position
    {
        get => WriteCursor = ReadCursor;
        set => WriteCursor = ReadCursor = value;
    }

    public long Length => (nint)VirtualMemoryAddressRange!.EndAddress - (nint)VirtualMemoryAddressRange!.StartAddress;

    public bool AllRead => WriteCursorPos == ReadCursor;

    public void Flush()
    {
        VirtualMemoryAddressRange?.Flush();
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        var remainingBytes = BufferRelativeWriteCursor - BufferRelativeReadCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        var ptr                                                      = ReadBuffer + BufferRelativeReadCursor;
        for (var i = offset; i < offset + cappedSize; i++) buffer[i] = *ptr++;
        ReadCursor = cappedSize;
        return (int)cappedSize;
    }

    public int ReadByte()
    {
        var result = ReadCursor >= WriteCursor ? -1 : *(ReadBuffer + BufferRelativeReadCursor);
        ReadCursor++;
        return result;
    }

    public virtual long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > Length || offset < 0)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = (int)offset;
                break;
            case SeekOrigin.End:
                if (offset < 0 || offset > Length)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = Length - (int)offset;
                break;
            default:
                var proposedCursor = Position + (int)offset;
                if (proposedCursor < 0 || proposedCursor > Length)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public virtual void SetLength(long value) { }

    public void Write(byte[] buffer, int offset, int count)
    {
        var remainingBytes = Length - WriteCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        var ptr = WriteBuffer + BufferRelativeWriteCursor;

        for (var i = offset; i < offset + cappedSize; i++) *ptr++ = buffer[i];
        WriteCursor += cappedSize;
    }

    public void WriteByte(byte value)
    {
        if (BufferRelativeWriteCursor >= Length) TryHandleRemainingWriteBufferRunningLow();
        if (BufferRelativeWriteCursor < Length)
        {
            var ptr = WriteBuffer + BufferRelativeWriteCursor;
            *ptr = value;
            WriteCursor++;
        }
    }

    public void SetAllRead()
    {
        ReadCursor = WriteCursorPos;
    }

    public virtual void TryHandleRemainingWriteBufferRunningLow() { }

    public bool HasStorageForBytes(int bytes) => RemainingStorage > bytes;

    ~FixedByteArrayBuffer()
    {
        if (shouldCloseView)
        {
            VirtualMemoryAddressRange?.Dispose();
            VirtualMemoryAddressRange = null;
        }
    }
}
