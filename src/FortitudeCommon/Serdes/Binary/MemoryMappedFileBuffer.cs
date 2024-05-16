#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public class MemoryMappedFileBuffer : IBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MemoryMappedFileBuffer));
    private readonly bool shouldCloseView;
    private int bufferAccessCounter;
    private ShiftableMemoryMappedFileView? mappedFileShiftableView;
    private nint originalBufferRelativeWriteCursor;
    private long readCursor;
    private long writeCursor;

    public MemoryMappedFileBuffer(ShiftableMemoryMappedFileView fileShiftableView, bool shouldCloseView = true)
    {
        mappedFileShiftableView = fileShiftableView;
        this.shouldCloseView = shouldCloseView;
    }

    public long ReadCursor
    {
        get => readCursor;
        set
        {
            readCursor = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(readCursor, true) ?? false;
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public long UnreadBytesRemaining => writeCursor - readCursor;

    public long WriteCursor
    {
        get => writeCursor;
        set
        {
            if (value > writeCursor) mappedFileShiftableView?.FlushCursorDataToDisk(originalBufferRelativeWriteCursor, (int)(value - writeCursor));
            writeCursor = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(writeCursor, true) ?? false;
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public long RemainingStorage => mappedFileShiftableView?.HighestFileCursor - writeCursor ?? 0;

    public void Dispose()
    {
        Interlocked.Decrement(ref bufferAccessCounter);
    }

    public unsafe byte* ReadBuffer
    {
        get
        {
            if (mappedFileShiftableView == null) return null;
            Interlocked.Increment(ref bufferAccessCounter);
            return mappedFileShiftableView.LowerHalfViewVirtualMemoryAddress;
        }
    }

    public unsafe byte* WriteBuffer
    {
        get
        {
            if (mappedFileShiftableView == null) return null;
            Interlocked.Increment(ref bufferAccessCounter);
            return mappedFileShiftableView.LowerHalfViewVirtualMemoryAddress;
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

    public long Size => PagedMemoryMappedFile.MaxFileCursorOffset;

    public bool AllRead => writeCursor == readCursor;

    public void SetAllRead()
    {
        readCursor = writeCursor;
        var moved = mappedFileShiftableView?.EnsureLowerViewContainsFileCursorOffset(readCursor, true) ?? false;
        if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
    }

    public void TryHandleRemainingWriteBufferRunningLow()
    {
        var moved = mappedFileShiftableView!.EnsureLowerViewContainsFileCursorOffset(writeCursor, true);
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

    ~MemoryMappedFileBuffer()
    {
        if (shouldCloseView) mappedFileShiftableView?.Dispose();
    }
}
