#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public class MemoryMappedFileBuffer : IBuffer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MemoryMappedFileBuffer));
    private readonly ShiftableMemoryMappedFileView contiguousPagedFile;
    private int bufferAccessCounter;
    private nint originalBufferRelativeWriteCursor;
    private nint readCursor;
    private nint writeCursor;

    public MemoryMappedFileBuffer(ShiftableMemoryMappedFileView contiguousPagedFile) => this.contiguousPagedFile = contiguousPagedFile;

    public void Dispose()
    {
        Interlocked.Decrement(ref bufferAccessCounter);
    }

    public unsafe byte* ReadBuffer
    {
        get
        {
            Interlocked.Increment(ref bufferAccessCounter);
            return contiguousPagedFile.LowerChunkAddress;
        }
    }

    public unsafe byte* WriteBuffer
    {
        get
        {
            Interlocked.Increment(ref bufferAccessCounter);
            return contiguousPagedFile.LowerChunkAddress;
        }
    }

    public nint BufferRelativeReadCursor => readCursor - (nint)contiguousPagedFile.LowerChunkFileCursorOffset;

    public nint BufferRelativeWriteCursor
    {
        get
        {
            originalBufferRelativeWriteCursor = writeCursor - (nint)contiguousPagedFile.LowerChunkFileCursorOffset;
            return originalBufferRelativeWriteCursor;
        }
    }

    public long Size => PagedMemoryMappedFile.MaxFileCursorOffset;

    public nint ReadCursor
    {
        get => readCursor;
        set
        {
            readCursor = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = contiguousPagedFile.EnsureLowerChunkContainsFileCursorOffset(readCursor, true);
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public bool AllRead => writeCursor == readCursor;
    public nint UnreadBytesRemaining => writeCursor - readCursor;

    public nint WriteCursor
    {
        get => writeCursor;
        set
        {
            if (value > writeCursor) contiguousPagedFile.FlushCursorDataToDisk(originalBufferRelativeWriteCursor, (int)(value - writeCursor));
            writeCursor = value;
            if (bufferAccessCounter <= 1)
            {
                var moved = contiguousPagedFile.EnsureLowerChunkContainsFileCursorOffset(writeCursor, true);
                if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
            }
        }
    }

    public nint RemainingStorage => (nint)contiguousPagedFile.EndFileCursor - writeCursor;

    public void SetAllRead()
    {
        readCursor = writeCursor;
        var moved = contiguousPagedFile.EnsureLowerChunkContainsFileCursorOffset(readCursor, true);
        if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
    }

    public void TryHandleRemainingWriteBufferRunningLow()
    {
        var moved = contiguousPagedFile.EnsureLowerChunkContainsFileCursorOffset(writeCursor, true);
        if (moved) Logger.Debug("Memory Mapped File Chunk Shifted LowerChunkFileCursorOffset");
    }

    public bool HasStorageForBytes(int bytes) => RemainingStorage > bytes;

    ~MemoryMappedFileBuffer()
    {
        contiguousPagedFile.Dispose();
    }
}
