// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public interface IGrowableUnmanagedBuffer : IBuffer, IGrowable<IGrowableUnmanagedBuffer>
{
    IVirtualMemoryAddressRange BackingMemoryAddressRange   { get; }
    int                        GrowRemainingBytesThreshold { get; set; }
}

public class GrowableUnmanagedBuffer : FixedByteArrayBuffer, IGrowableUnmanagedBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(GrowableUnmanagedBuffer));

    public GrowableUnmanagedBuffer(IVirtualMemoryAddressRange virtualMemoryAddressRange, bool shouldCloseView = true)
        : base(virtualMemoryAddressRange, shouldCloseView) { }

    public int GrowRemainingBytesThreshold { get; set; } = 1024;

    public IGrowableUnmanagedBuffer GrowByDefaultSize()
    {
        if (VirtualMemoryAddressRange != null)
        {
            if (VirtualMemoryAddressRange is ShiftableMemoryMappedFileView shiftableMemoryMappedFileView)
            {
                ReadCursor  -= shiftableMemoryMappedFileView.HalfViewSizeBytes;
                WriteCursor -= shiftableMemoryMappedFileView.HalfViewSizeBytes;
            }
            var grown = VirtualMemoryAddressRange.GrowByDefaultSize();
            if (grown != VirtualMemoryAddressRange)
            {
                VirtualMemoryAddressRange.Dispose();
                VirtualMemoryAddressRange = grown;
            }

            if (ReadCursor < 0 || WriteCursor < 0)
                throw new Exception("Error ShiftableMemoryMappedFileView moved beyond the ReadCursor or WriteCursor");
        }
        return this;
    }

    public override long DefaultGrowSize => VirtualMemoryAddressRange!.DefaultGrowSize;

    public override long WriteCursor
    {
        get => WriteCursorPos;
        set
        {
            LimitNextSerialize = null;
            WriteCursorPos     = value;
            if (BufferAccessCounter <= 1)
                if (RemainingStorage < GrowRemainingBytesThreshold)
                    GrowByDefaultSize();
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > Length && offset < Length + VirtualMemoryAddressRange!.DefaultGrowSize)
                    GrowByDefaultSize();
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
                if (proposedCursor > Length && offset < Length + VirtualMemoryAddressRange!.DefaultGrowSize)
                    GrowByDefaultSize();
                if (proposedCursor < 0 || proposedCursor > Length)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public override void SetLength(long value)
    {
        if (VirtualMemoryAddressRange != null)
        {
            while (Length < value)
            {
                if (VirtualMemoryAddressRange is ShiftableMemoryMappedFileView shiftableMemoryMappedFileView)
                {
                    ReadCursor  -= shiftableMemoryMappedFileView.HalfViewSizeBytes;
                    WriteCursor -= shiftableMemoryMappedFileView.HalfViewSizeBytes;
                }
                var grown = VirtualMemoryAddressRange.GrowByDefaultSize();
                if (grown == VirtualMemoryAddressRange) continue;
                VirtualMemoryAddressRange.Dispose();
                VirtualMemoryAddressRange = grown;
            }
            if (ReadCursor < 0 || WriteCursor < 0)
                throw new Exception("Error ShiftableMemoryMappedFileView moved beyond the ReadCursor or WriteCursor");
            ReadCursor  = Math.Min(ReadCursor, Length);
            WriteCursor = Math.Min(WriteCursor, Length);
        }
    }

    public override void TryHandleRemainingWriteBufferRunningLow()
    {
        GrowByDefaultSize();
    }
}
