// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public interface IMemoryMappedFileBuffer : IMessageQueueBuffer, IGrowable<IBuffer>
{
    void SetFileWriterAt(ShiftableMemoryMappedFileView fileShiftableView, long fileCursorOffset);
    void ClearFileView();
}

public class MemoryMappedFileBuffer : IMemoryMappedFileBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MemoryMappedFileBuffer));

    private readonly bool shouldCloseView;

    private int  bufferAccessCounter;
    private bool isDestroyed;

    private ShiftableMemoryMappedFileView? mappedFileShiftableView;

    private nint originalBufferRelativeWriteCursor;
    private long readCursor;
    private long writeCursor;

    private List<MessageBufferEntry> messageQueue = new();

    public MemoryMappedFileBuffer(ShiftableMemoryMappedFileView fileShiftableView, bool shouldCloseView = true)
    {
        mappedFileShiftableView = fileShiftableView;
        this.shouldCloseView    = shouldCloseView;
    }

    public long DefaultGrowSize => mappedFileShiftableView!.DefaultGrowSize;

    public IBuffer GrowByDefaultSize()
    {
        mappedFileShiftableView!.GrowByDefaultSize();
        return this;
    }

    public long ReadCursor
    {
        get => readCursor;
        set
        {
            LimitNextDeserialize = null;
            readCursor           = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(readCursor, shouldGrow: true) ?? false;
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public long UnreadBytesRemaining => Math.Min(LimitNextDeserialize ?? writeCursor - readCursor, writeCursor - readCursor);

    public long WriteCursor
    {
        get => writeCursor;
        set
        {
            LimitNextSerialize = null;
            if (value > writeCursor) mappedFileShiftableView?.FlushCursorDataToDisk(originalBufferRelativeWriteCursor, (int)(value - writeCursor));
            writeCursor = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(writeCursor, shouldGrow: true) ?? false;
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public long RemainingStorage =>
        Math.Min(LimitNextSerialize ?? mappedFileShiftableView?.HighestFileCursor - writeCursor ?? 0
               , mappedFileShiftableView?.HighestFileCursor - writeCursor ?? 0);

    public long? LimitNextSerialize { get; set; }

    public long? LimitNextDeserialize { get; set; }

    public void Dispose()
    {
        Interlocked.Decrement(ref bufferAccessCounter);
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
            mappedFileShiftableView?.Dispose();
            mappedFileShiftableView = null;
        }
    }

    public unsafe byte* ReadBuffer
    {
        get
        {
            if (mappedFileShiftableView == null) return null;
            Interlocked.Increment(ref bufferAccessCounter);
            return mappedFileShiftableView.StartAddress;
        }
    }

    public unsafe byte* WriteBuffer
    {
        get
        {
            if (mappedFileShiftableView == null) return null;
            Interlocked.Increment(ref bufferAccessCounter);
            return mappedFileShiftableView.StartAddress;
        }
    }

    public nint BufferRelativeReadCursor =>
        mappedFileShiftableView == null ? 0 : (nint)(readCursor - mappedFileShiftableView.LowerViewFileCursorOffset);

    public nint BufferRelativeWriteCursor
    {
        get
        {
            if (mappedFileShiftableView == null) return 0;
            originalBufferRelativeWriteCursor = (nint)(writeCursor - mappedFileShiftableView.LowerViewFileCursorOffset);
            return originalBufferRelativeWriteCursor;
        }
    }

    public bool CanRead  => BufferRelativeReadCursor < (mappedFileShiftableView?.HighestFileCursor ?? 0);
    public bool CanSeek  => true;
    public bool CanWrite => BufferRelativeWriteCursor < (mappedFileShiftableView?.HighestFileCursor ?? 0);

    public long Position
    {
        get => WriteCursor = ReadCursor;
        set
        {
            if (value < mappedFileShiftableView!.LowerViewFileCursorOffset
             || value > mappedFileShiftableView.LowerViewFileCursorOffset + mappedFileShiftableView.HalfViewSizeBytes)
                mappedFileShiftableView.EnsureLowerViewContainsFileCursorOffset(value, shouldGrow: true);
            WriteCursor = ReadCursor = value;
        }
    }

    public long Length => mappedFileShiftableView?.FileSize ?? 0;

    public bool AllRead => writeCursor == readCursor;

    public void Flush()
    {
        mappedFileShiftableView?.Flush();
    }

    public unsafe int Read(byte[] buffer, int offset, int count)
    {
        var remainingBytes = BufferRelativeWriteCursor - BufferRelativeReadCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        var ptr                                                      = ReadBuffer + BufferRelativeReadCursor;
        for (var i = offset; i < offset + cappedSize; i++) buffer[i] = *ptr++;
        ReadCursor = cappedSize;
        return (int)cappedSize;
    }

    public unsafe int ReadByte()
    {
        var result = ReadCursor >= WriteCursor ? -1 : *(ReadBuffer + BufferRelativeReadCursor);
        ReadCursor++;
        return result;
    }

    public long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset > Length)
                    mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(offset, shouldGrow: true);
                if (offset < 0)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = (int)offset;
                break;
            case SeekOrigin.End:
                if (offset > Length || offset < 0)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = Length - (int)offset;
                break;
            default:
                var proposedCursor = Position + (int)offset;
                if (proposedCursor > Length && proposedCursor < Length + mappedFileShiftableView!.HalfViewPageSize)
                    mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(proposedCursor, shouldGrow: true);
                if (proposedCursor < 0 || proposedCursor > Length)
                    throw new Exception("Attempted to seek beyond the end of the Buffer");
                Position = proposedCursor;
                break;
        }
        return Position;
    }

    public void SetLength(long value)
    {
        if (mappedFileShiftableView != null)
        {
            var originalFileOffset = mappedFileShiftableView.LowerViewFileCursorOffset;
            mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(value, shouldGrow: true);
            mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(originalFileOffset);
        }
    }

    public unsafe void Write(byte[] buffer, int offset, int count)
    {
        var remainingBytes = Length - WriteCursor;
        var cappedSize     = Math.Min(Math.Min(count, remainingBytes), buffer.Length - offset);

        var ptr = WriteBuffer + BufferRelativeWriteCursor;

        for (var i = offset; i < offset + cappedSize; i++) *ptr++ = buffer[i];
        WriteCursor += cappedSize;
    }

    public unsafe void WriteByte(byte value)
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
        readCursor = writeCursor;
        var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(readCursor, shouldGrow: true) ?? false;
        if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
    }

    public void TryHandleRemainingWriteBufferRunningLow()
    {
        var moved = mappedFileShiftableView!.EnsureLowerViewContainsFileCursorOffset(writeCursor, shouldGrow: true);
        if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
    }

    public bool HasStorageForBytes(int bytes) => RemainingStorage > bytes;


    public void SetFileWriterAt(ShiftableMemoryMappedFileView fileShiftableView, long fileCursorOffset)
    {
        mappedFileShiftableView = fileShiftableView;
        mappedFileShiftableView.EnsureLowerViewContainsFileCursorOffset(fileCursorOffset);
        readCursor = writeCursor = fileCursorOffset;
    }

    public void ClearFileView()
    {
        mappedFileShiftableView = null;
    }

    public bool EnforceCappedMessageSize { get; set; }

    public long MaximumMessageSize { get; set; }

    public bool HasAnotherMessage => messageQueue.Count > 0;

    public MessageBufferEntry PopNextMessage()
    {
        var next = messageQueue[0];
        messageQueue.RemoveAt(0);
        return next;
    }

    public void QueueMessage(MessageBufferEntry messageEntry)
    {
        messageQueue.Add(messageEntry);
    }

    ~MemoryMappedFileBuffer()
    {
        if (shouldCloseView)
        {
            mappedFileShiftableView?.Dispose();
            mappedFileShiftableView = null;
        }
    }
}
