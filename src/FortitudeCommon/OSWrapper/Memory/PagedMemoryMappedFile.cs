#region

using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class TwoContiguousPagedFileChunks : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TwoContiguousPagedFileChunks));
    private static readonly EndOfFileEmptyChunk BeyondEndOfFileChunk = new();
    private readonly bool closePagedMemoryMappedFileOnDispose;
    private readonly IOSDirectMemoryApi osDirectMemoryApi;
    private readonly PagedMemoryMappedFile pagedMemoryMappedFile;
    private void* foundTwoChunkVirtualMemoryRegion;
    private bool isDisposed;
    private MappedPagedFileChunk? lowerContiguousChunk;
    private int twoChunkSize;
    private MappedPagedFileChunk? upperContiguousChunk;

    public TwoContiguousPagedFileChunks(PagedMemoryMappedFile pagedMemoryMappedFile, bool closePagedMemoryMappedFileOnDispose)
    {
        this.pagedMemoryMappedFile = pagedMemoryMappedFile;
        this.closePagedMemoryMappedFileOnDispose = closePagedMemoryMappedFileOnDispose;
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.IncrementUsageCount();
        osDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
        twoChunkSize = pagedMemoryMappedFile.ChunkPageSize * 2;
        foundTwoChunkVirtualMemoryRegion = FindTwoChunkFreeVirtualMemorySlot();
        lowerContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(0, foundTwoChunkVirtualMemoryRegion)!;
        upperContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(pagedMemoryMappedFile.ChunkPageSize,
            lowerContiguousChunk.Address + pagedMemoryMappedFile.ChunkSizeBytes)!;
    }


    public byte* ChunkAddress => lowerContiguousChunk != null ? lowerContiguousChunk.Address : null;

    public long ChunkSizeBytes => pagedMemoryMappedFile.ChunkSizeBytes;

    public long UpperChunkTriggerChunkShiftTolerance { get; set; }

    public bool IsUpperChunkAvailableForContiguousReadWrite =>
        upperContiguousChunk is not null or EndOfFileEmptyChunk && lowerContiguousChunk != null
                                                                && upperContiguousChunk.Address == lowerContiguousChunk.Address +
                                                                ChunkSizeBytes;

    public void Dispose()
    {
        if (isDisposed) return;
        isDisposed = true;
        if (lowerContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        if (upperContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(upperContiguousChunk);
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.DecrementUsageCount();
    }

    private void* FindTwoChunkFreeVirtualMemorySlot()
    {
        var emptyReservedSpace = osDirectMemoryApi.ReserveMemoryRangeInPages(null, twoChunkSize);
        osDirectMemoryApi.ReleaseReserveMemoryRangeInPages(emptyReservedSpace, twoChunkSize);
        return emptyReservedSpace;
    }

    ~TwoContiguousPagedFileChunks()
    {
        Dispose();
    }

    public bool NextChunk(bool shouldGrow = false)
    {
        if (upperContiguousChunk is null or EndOfFileEmptyChunk) return false;
        var requiredFileSize = (upperContiguousChunk.FileChunkNumber + 1) * ChunkSizeBytes + ChunkSizeBytes;
        MappedPagedFileChunk? nextChunk;
        if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
        {
            if (!shouldGrow)
            {
                if (upperContiguousChunk is EndOfFileEmptyChunk) return false;
                if (lowerContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
                lowerContiguousChunk = upperContiguousChunk;
                upperContiguousChunk = BeyondEndOfFileChunk;
                return true;
            }

            nextChunk = pagedMemoryMappedFile.GrowAndReturnChunk(upperContiguousChunk.FileChunkNumber + pagedMemoryMappedFile.ChunkPageSize,
                upperContiguousChunk.Address + ChunkSizeBytes);
            if (nextChunk == null) return RemapChunksOnReservedMemory(upperContiguousChunk.FileChunkNumber);
        }
        else
        {
            nextChunk = pagedMemoryMappedFile.CreatePagedFileChunk(upperContiguousChunk.FileChunkNumber + pagedMemoryMappedFile.ChunkPageSize,
                upperContiguousChunk.Address + ChunkSizeBytes);
        }

        if (nextChunk == null) return RemapChunksOnReservedMemory(upperContiguousChunk.FileChunkNumber);
        if (lowerContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        lowerContiguousChunk = upperContiguousChunk;
        upperContiguousChunk = nextChunk;
        return true;
    }

    public bool RemapChunksOnReservedMemory(int lowerChunkPageNumber, int attemptCountDown = 3)
    {
        if (attemptCountDown < 0)
            throw new Exception("Unexpected mapping failed on reserved memory.  " +
                                "Previous file chunks were not released properly");

        if (lowerContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        lowerContiguousChunk = null;
        if (upperContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(upperContiguousChunk);
        upperContiguousChunk = null;
        var allocationAddress = attemptCountDown == 3 ? foundTwoChunkVirtualMemoryRegion : null;
        if (attemptCountDown < 3)
        {
            foundTwoChunkVirtualMemoryRegion = FindTwoChunkFreeVirtualMemorySlot();
            allocationAddress = attemptCountDown != 0 ? foundTwoChunkVirtualMemoryRegion : null;
        }

        var checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(lowerChunkPageNumber, allocationAddress);
        if (checkChunk == null) return RemapChunksOnReservedMemory(lowerChunkPageNumber, attemptCountDown - 1);
        lowerContiguousChunk = checkChunk;
        checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(lowerChunkPageNumber + pagedMemoryMappedFile.ChunkPageSize,
            lowerContiguousChunk.Address + ChunkSizeBytes);
        if (checkChunk == null)
        {
            var requiredFileSize = (lowerChunkPageNumber + ChunkSizeBytes) * pagedMemoryMappedFile.PageSize +
                                   pagedMemoryMappedFile.ChunkSizeBytes;
            if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
                checkChunk = BeyondEndOfFileChunk;
            else
                return RemapChunksOnReservedMemory(lowerChunkPageNumber, attemptCountDown - 1);
        }

        upperContiguousChunk = checkChunk;
        return true;
    }

    public void* EnsureLowerChunkContainsFileCursorOffset(long fileCursorOffset, bool shouldGrow = false)
    {
        var upperChunkIsBeyondEndOfFile = upperContiguousChunk is EndOfFileEmptyChunk or null;
        var allowedUpperChunk = !upperChunkIsBeyondEndOfFile ? UpperChunkTriggerChunkShiftTolerance : 0;
        if (fileCursorOffset >= lowerContiguousChunk!.FileChunkNumber
            && fileCursorOffset < lowerContiguousChunk.FileChunkNumber + pagedMemoryMappedFile.ChunkSizeBytes + allowedUpperChunk)
            return lowerContiguousChunk.Address;
        if ((upperChunkIsBeyondEndOfFile && shouldGrow) || (fileCursorOffset >= upperContiguousChunk!.FileChunkNumber &&
                                                            fileCursorOffset < upperContiguousChunk.FileChunkNumber +
                                                            pagedMemoryMappedFile.ChunkSizeBytes))
        {
            NextChunk(shouldGrow);
            return lowerContiguousChunk.Address;
        }

        var foundChunkPageNumber
            = pagedMemoryMappedFile.FindChunkPageNumberContainingCursor(fileCursorOffset, foundTwoChunkVirtualMemoryRegion, shouldGrow);
        if (foundChunkPageNumber == null) throw new Exception("Attempted");
        RemapChunksOnReservedMemory(foundChunkPageNumber.Value);
        return lowerContiguousChunk.Address;
    }

    public bool FlushCursorDataToDisk(long fileCursorOffset, int sizeToFlush)
    {
        var wasFlushed = lowerContiguousChunk != null && pagedMemoryMappedFile.Flush(lowerContiguousChunk, fileCursorOffset, sizeToFlush);
        wasFlushed |= upperContiguousChunk != null && pagedMemoryMappedFile.Flush(upperContiguousChunk, fileCursorOffset, sizeToFlush);
        return wasFlushed;
    }
}

public unsafe class MappedPagedFileChunk : IDisposable
{
    private readonly MemoryMappedFile memoryMappedFile;
    public byte* Address;
    public int FileChunkNumber;
    public long StartFileCursorOffset;

    public MappedPagedFileChunk(MemoryMappedFile memoryMappedFile, int fileChunkNumber, long fileCursorPosition)
    {
        this.memoryMappedFile = memoryMappedFile;
        FileChunkNumber = fileChunkNumber;
        StartFileCursorOffset = fileCursorPosition;
    }

    public void Dispose()
    {
        memoryMappedFile.Dispose();
    }
}

public unsafe class EndOfFileEmptyChunk : MappedPagedFileChunk
{
    public EndOfFileEmptyChunk() : base(null!, 0, 0) => Address = null;
}

public sealed unsafe class PagedMemoryMappedFile : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PagedMemoryMappedFile));

    private readonly IOSDirectMemoryApi osDirectMemoryApi;
    public readonly int PageSize;
    private FileStream fileStream;
    private bool isDisposed;
    private int liveCount;

    public PagedMemoryMappedFile(string filePath, int requestedChunkPageSize)
    {
        IncrementUsageCount();
        osDirectMemoryApi = osDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
        PageSize = (int)osDirectMemoryApi.MinimumRequiredPageSize;
        ChunkPageSize = requestedChunkPageSize <= 1 ? 1 : MemoryUtils.CeilingNextPowerOfTwo(requestedChunkPageSize);
        ChunkSizeBytes = (int)(ChunkPageSize * PageSize);
        var minimumFileSizeForTwoChunks = ChunkPageSize * PageSize * 2;
        if (minimumFileSizeForTwoChunks <= 0) minimumFileSizeForTwoChunks = PageSize * 2;
        var existingFile = File.Exists(filePath);
        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        if (existingFile)
        {
            var fileSize = fileStream.Length;
            if (fileStream.Length % ChunkSizeBytes != 0)
            {
                var growFileSize = fileSize % ChunkSizeBytes + ChunkSizeBytes;
                fileStream.SetLength(growFileSize);
            }
        }
        else
        {
            fileStream.SetLength(minimumFileSizeForTwoChunks);
        }
    }

    public int ChunkPageSize { get; }

    public int ChunkSizeBytes { get; }

    public FileStream FileStream
    {
        get => fileStream;
        set => fileStream = value ?? throw new ArgumentNullException(nameof(value));
    }

    public long Length
    {
        get
        {
            CheckDisposed();
            return fileStream.Length;
        }
    }

    public void Dispose()
    {
        if (liveCount > 0 || isDisposed) return;
        isDisposed = true;
        fileStream.Dispose();
    }

    public TwoContiguousPagedFileChunks CreateTwoContiguousPagedFileChunks(bool closePagedMemoryMappedFileOnDispose = true)
    {
        var result = new TwoContiguousPagedFileChunks(this, closePagedMemoryMappedFileOnDispose);
        if (closePagedMemoryMappedFileOnDispose) DecrementUsageCount();
        return result;
    }

    public int IncrementUsageCount() => Interlocked.Increment(ref liveCount);

    public int DecrementUsageCount()
    {
        Interlocked.Decrement(ref liveCount);
        if (liveCount == 0)
        {
            Dispose();
            return 0;
        }

        return liveCount;
    }

    public void FreeChunk(MappedPagedFileChunk toBeFreed)
    {
        if (toBeFreed is not EndOfFileEmptyChunk)
        {
            osDirectMemoryApi.ReleaseViewOfFile(toBeFreed.Address, ChunkSizeBytes);
            toBeFreed.Dispose();
            DecrementUsageCount();
        }
    }

    public MappedPagedFileChunk? CreatePagedFileChunk(int fileChunkNumber, void* atAddress)
    {
        CheckDisposed();
        var expectedFileSize = fileChunkNumber * ChunkPageSize * PageSize + ChunkSizeBytes;
        if (fileStream.Length < expectedFileSize) return null;

        var memoryMappedFile = MemoryMappedFile.CreateFromFile(fileStream, null, 0,
            MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
        var address = osDirectMemoryApi.MapPageViewOfFile(memoryMappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle()
            , fileChunkNumber * ChunkPageSize
            , ChunkPageSize
            , atAddress);
        var memMapFileError = Marshal.GetLastPInvokeError();
        if (memMapFileError != 0)
            Logger.Warn("Got Error code {0} when trying to create Memory Mapped File Mapping", memMapFileError);
        if (address == null)
        {
            if (atAddress == null) Logger.Info("Failed to create chunk at requested address will make more attempts.");
            return null;
        }

        IncrementUsageCount();
        var mappedPagedFileChunk = new MappedPagedFileChunk(memoryMappedFile, fileChunkNumber, fileChunkNumber * ChunkPageSize * PageSize)
        {
            Address = (byte*)address
        };
        return mappedPagedFileChunk;
    }

    public MappedPagedFileChunk? GrowAndReturnChunk(int fileChunkNumber, byte* atAddress)
    {
        CheckDisposed();
        var expectedFileSize = fileChunkNumber * ChunkPageSize * PageSize + ChunkSizeBytes;
        if (fileStream.Length < expectedFileSize) fileStream.SetLength(expectedFileSize);

        return CreatePagedFileChunk(fileChunkNumber, atAddress);
    }

    public int? FindChunkPageNumberContainingCursor(long fileCursorOffset, void* atAddress, bool shouldGrow = false)
    {
        CheckDisposed();
        if (fileStream.Length < fileCursorOffset && !shouldGrow) return null;
        var chunkNumber = 0;
        var chunkStartByte = 0;
        while (fileCursorOffset > chunkStartByte + ChunkSizeBytes)
        {
            chunkNumber++;
            chunkStartByte = chunkNumber * ChunkSizeBytes;
        }

        return chunkNumber;
    }

    private void CheckDisposed()
    {
        if (isDisposed) throw new ObjectDisposedException(GetType().Name);
    }

    public bool Flush(MappedPagedFileChunk chunk, long fileCursorFrom, nint bytes)
    {
        CheckDisposed();
        var bytesToFlush = bytes;
        void* addressToFlush;
        if (fileCursorFrom > chunk.StartFileCursorOffset + ChunkSizeBytes) return false;
        if (fileCursorFrom < chunk.StartFileCursorOffset)
        {
            if (fileCursorFrom + bytes < chunk.StartFileCursorOffset)
            {
                var bytesToGetToStartOfChunk = chunk.StartFileCursorOffset - fileCursorFrom;
                bytesToFlush = (nint)(bytes - bytesToGetToStartOfChunk);
                addressToFlush = chunk.Address;
            }
            else
            {
                return false;
            }
        }
        else
        {
            var chunkOffset = fileCursorFrom - chunk.StartFileCursorOffset;
            addressToFlush = chunk.Address + chunkOffset;
        }

        if (!osDirectMemoryApi.FlushPageDataToDisk(addressToFlush, bytesToFlush))
            throw new Win32Exception();
        fileStream.Flush(true);
        return true;
    }
}
