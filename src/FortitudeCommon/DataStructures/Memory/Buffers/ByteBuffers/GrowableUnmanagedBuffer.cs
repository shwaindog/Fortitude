// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;

public interface IGrowableUnmanagedBuffer : IFixedByteArrayBuffer, IGrowable<IGrowableUnmanagedBuffer>, IVirtualMemoryAddressRange
{
    new long Length                      { get; }
    new long DefaultGrowSize             { get; }
    int      GrowRemainingBytesThreshold { get; set; }

    new void Flush();

    new IGrowableUnmanagedBuffer GrowByDefaultSize();
}

public class GrowableUnmanagedBuffer : FixedByteArrayBuffer, IGrowableUnmanagedBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(GrowableUnmanagedBuffer));

    public GrowableUnmanagedBuffer(IVirtualMemoryAddressRange virtualMemoryAddressRange, bool shouldCloseView = true)
        : base(virtualMemoryAddressRange, shouldCloseView) { }

    public int GrowRemainingBytesThreshold { get; set; } = 16192;

    public IGrowableUnmanagedBuffer GrowByDefaultSize()
    {
        if (VirtualMemoryAddressRange != null)
        {
            var grown = VirtualMemoryAddressRange.GrowByDefaultSize();
            if (grown != VirtualMemoryAddressRange)
            {
                VirtualMemoryAddressRange.Dispose();
                VirtualMemoryAddressRange = grown;
            }
            Length = grown.Length;

            if (ReadCursor < 0 || WriteCursor < 0)
                throw new Exception("Error ShiftableMemoryMappedFileView moved beyond the ReadCursor or WriteCursor");
        }
        return this;
    }

    public override long DefaultGrowSize => VirtualMemoryAddressRange!.DefaultGrowSize;

    IVirtualMemoryAddressRange IGrowable<IVirtualMemoryAddressRange>.GrowByDefaultSize() => GrowByDefaultSize();

    public unsafe byte* StartAddress => VirtualMemoryAddressRange!.StartAddress;
    public unsafe byte* EndAddress   => VirtualMemoryAddressRange!.EndAddress;

    public UnmanagedByteArray CreateUnmanagedByteArrayInThisRange(long fileCursorPosition, int length) =>
        VirtualMemoryAddressRange!.CreateUnmanagedByteArrayInThisRange(fileCursorPosition, length);

    public override long WriteCursor
    {
        get => WriteCursorPos;
        set
        {
            LimitNextSerialize = null;
            WriteCursorPos     = value;
            if (BufferAccessCounter <= 1)
            {
                var bufferStorage = VirtualMemoryAddressRange!.Length - WriteCursorPos - GrowRemainingBytesThreshold;
                if (bufferStorage < 0) GrowByDefaultSize();
                if (Length < GrowRemainingBytesThreshold) Length += GrowRemainingBytesThreshold;
            }
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > Length && offset < Length + VirtualMemoryAddressRange!.DefaultGrowSize) GrowByDefaultSize();
                if (offset > Length || offset < 0) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = (int)offset;
                break;
            case SeekOrigin.End:
                if (offset < 0 || offset > Length) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = Length - (int)offset;
                break;
            default:
                var proposedCursor = Position + (int)offset;
                if (proposedCursor > Length && offset < Length + VirtualMemoryAddressRange!.DefaultGrowSize) GrowByDefaultSize();
                if (proposedCursor < 0 || proposedCursor > Length) throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public override void SetLength(long value)
    {
        if (VirtualMemoryAddressRange != null)
        {
            while (VirtualMemoryAddressRange.Length < value)
            {
                var grown = VirtualMemoryAddressRange.GrowByDefaultSize();
                if (grown != VirtualMemoryAddressRange)
                {
                    VirtualMemoryAddressRange.Dispose();
                    VirtualMemoryAddressRange = grown;
                }
            }
            Length = value;
            if (ReadCursor < 0 || WriteCursor < 0)
                throw new Exception("Error ShiftableMemoryMappedFileView moved beyond the ReadCursor or WriteCursor");
            ReadCursor     = Math.Min(ReadCursor, Length);
            WriteCursorPos = Math.Min(WriteCursor, Length);
        }
    }

    public override void TryHandleRemainingWriteBufferRunningLow()
    {
        GrowByDefaultSize();
    }
}
