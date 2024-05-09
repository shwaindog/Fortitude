#region

using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.OSWrapper.MemoryMappedFiles;

public class MemoryMappedPageBuffer { }

public unsafe class TwoContiguousPagedFileChunks : IDisposable
{
    private static readonly int PageSize = Environment.SystemPageSize;
    private static readonly EndOfFileEmptyChunk BeyondEndOfFileChunk = new();
    private readonly bool closePagedMemoryMappedFileOnDispose;
    private readonly IOSMemoryAllocation osMemoryApi;
    private readonly PagedMemoryMappedFile pagedMemoryMappedFile;
    private readonly byte* reservedTwoPageChunkMemoryRegion;
    private bool isDisposed;
    private MappedPagedFileChunk lowerContiguousChunk;
    private MappedPagedFileChunk upperContiguousChunk;

    public TwoContiguousPagedFileChunks(PagedMemoryMappedFile pagedMemoryMappedFile, bool closePagedMemoryMappedFileOnDispose
        , IOSMemoryAllocation osMemoryApi)
    {
        this.pagedMemoryMappedFile = pagedMemoryMappedFile;
        this.closePagedMemoryMappedFileOnDispose = closePagedMemoryMappedFileOnDispose;
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.IncrementUsageCount();
        this.osMemoryApi = osMemoryApi;
        var twoChunkSize = pagedMemoryMappedFile.ChunkPageSize * 2;
        reservedTwoPageChunkMemoryRegion = osMemoryApi.ReserveMemoryRangeInPages(null, twoChunkSize);
        lowerContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(0, reservedTwoPageChunkMemoryRegion)!;
        upperContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(pagedMemoryMappedFile.ChunkPageSize, reservedTwoPageChunkMemoryRegion)!;
    }

    public byte* ChunkAddress => lowerContiguousChunk.Address;

    public void Dispose()
    {
        if (isDisposed) return;
        isDisposed = true;
        pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        pagedMemoryMappedFile.FreeChunk(upperContiguousChunk);
        var twoChunkSize = pagedMemoryMappedFile.ChunkPageSize * 2;
        osMemoryApi.ReleaseReserveMemoryRangeInPages(reservedTwoPageChunkMemoryRegion, twoChunkSize);
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.DecrementUsageCount();
    }

    ~TwoContiguousPagedFileChunks()
    {
        Dispose();
    }

    public bool NextChunk(bool shouldGrow = false)
    {
        var requiredFileSize = upperContiguousChunk.FilePageNumber * PageSize + pagedMemoryMappedFile.ChunkSizeBytes;
        MappedPagedFileChunk? nextChunk;
        if (pagedMemoryMappedFile.FileStream.Length <= requiredFileSize)
        {
            if (!shouldGrow)
            {
                if (upperContiguousChunk is EndOfFileEmptyChunk) return false;
                pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
                lowerContiguousChunk = upperContiguousChunk;
                upperContiguousChunk = BeyondEndOfFileChunk;
                return true;
            }

            nextChunk = pagedMemoryMappedFile.GrowAndReturnChunk(upperContiguousChunk.FilePageNumber + pagedMemoryMappedFile.ChunkPageSize,
                upperContiguousChunk.Address + pagedMemoryMappedFile.ChunkSizeBytes);
        }
        else
        {
            nextChunk = pagedMemoryMappedFile.CreatePagedFileChunk(upperContiguousChunk.FilePageNumber + pagedMemoryMappedFile.ChunkPageSize,
                upperContiguousChunk.Address + pagedMemoryMappedFile.ChunkSizeBytes);
        }

        if (nextChunk == null) return RemapChunksOnReservedMemory(upperContiguousChunk.FilePageNumber);
        pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        lowerContiguousChunk = upperContiguousChunk;
        upperContiguousChunk = nextChunk;
        return true;
    }

    public bool RemapChunksOnReservedMemory(int lowerChunkPageNumber)
    {
        pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        pagedMemoryMappedFile.FreeChunk(upperContiguousChunk);
        var checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(lowerChunkPageNumber, reservedTwoPageChunkMemoryRegion);
        lowerContiguousChunk = checkChunk ?? throw new Exception("Unexpected mapping failed on reserved memory.  " +
                                                                 "Previous file chunks were not released properly");
        checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(lowerChunkPageNumber + pagedMemoryMappedFile.ChunkPageSize,
            lowerContiguousChunk.Address + pagedMemoryMappedFile.ChunkSizeBytes);
        upperContiguousChunk = checkChunk ?? BeyondEndOfFileChunk;
        return true;
    }

    public byte* EnsureLowerChunkContainsFileCursorOffset(long fileCursorOffset, long allowedUpperToleranceBytes = 16 * 1024, bool shouldGrow = false)
    {
        var upperChunkIsBeyondEndOfFile = upperContiguousChunk is EndOfFileEmptyChunk;
        var allowedUpperChunk = !upperChunkIsBeyondEndOfFile ? allowedUpperToleranceBytes : 0;
        if (fileCursorOffset >= lowerContiguousChunk.FilePageNumber
            && fileCursorOffset < lowerContiguousChunk.FilePageNumber + pagedMemoryMappedFile.ChunkSizeBytes + allowedUpperChunk)
            return lowerContiguousChunk.Address;
        if (fileCursorOffset >= upperContiguousChunk.FilePageNumber &&
            fileCursorOffset < upperContiguousChunk.FilePageNumber + pagedMemoryMappedFile.ChunkSizeBytes)
        {
            NextChunk(shouldGrow);
            return lowerContiguousChunk.Address;
        }

        var foundChunkPageNumber
            = pagedMemoryMappedFile.FindChunkPageNumberContainingCursor(fileCursorOffset, reservedTwoPageChunkMemoryRegion, shouldGrow);
        if (foundChunkPageNumber == null) throw new Exception("Attempted");
        RemapChunksOnReservedMemory(foundChunkPageNumber.Value);
        return lowerContiguousChunk.Address;
    }

    public bool FlushCursorDataToDisk(long fileCursorOffset, int sizeToFlush)
    {
        var wasFlushed = pagedMemoryMappedFile.Flush(lowerContiguousChunk, fileCursorOffset, sizeToFlush);
        wasFlushed |= pagedMemoryMappedFile.Flush(upperContiguousChunk, fileCursorOffset, sizeToFlush);
        return wasFlushed;
    }
}

public unsafe class MappedPagedFileChunk
{
    private static readonly int PageSize = Environment.SystemPageSize;
    public byte* Address;

    public int FilePageNumber;
    public int NumberOfPagesSize;
    public long StartFileCursorOffset;

    public MappedPagedFileChunk(int filePageNumber, int numberOfPages)
    {
        FilePageNumber = filePageNumber;
        NumberOfPagesSize = numberOfPages;
        StartFileCursorOffset = filePageNumber * PageSize;
    }
}

public unsafe class EndOfFileEmptyChunk : MappedPagedFileChunk
{
    public EndOfFileEmptyChunk() : base(0, 0) => Address = null;
}

public sealed unsafe class PagedMemoryMappedFile : IDisposable
{
    private static readonly int PageSize = Environment.SystemPageSize;
    private readonly MemoryMappedFile memoryMappedFile;
    private readonly IOsMemoryMappedFileApi memoryMappedFileApi;
    private readonly IOSMemoryAllocation osMemoryApi;
    private FileStream fileStream;
    private bool isDisposed;
    private int liveCount;

    public PagedMemoryMappedFile(string filePath, int requestedChunkPageSize, IOsMemoryMappedFileApi memoryMappedFileApi
        , IOSMemoryAllocation osMemoryApi)
    {
        IncrementUsageCount();
        this.memoryMappedFileApi = memoryMappedFileApi;
        this.osMemoryApi = osMemoryApi;
        if (requestedChunkPageSize <= 1)
            ChunkPageSize = 1;
        else
            ChunkPageSize = MemoryUtils.CeilingNextPowerOfTwo(requestedChunkPageSize);

        ChunkSizeBytes = ChunkPageSize * PageSize;
        var roundedUpInitialFileSize = ChunkPageSize * PageSize * 2;
        if (roundedUpInitialFileSize <= 0) roundedUpInitialFileSize = PageSize;
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
            fileStream.SetLength(roundedUpInitialFileSize);
        }

        memoryMappedFile = MemoryMappedFile.CreateFromFile(fileStream, null, fileStream.Length,
            MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
    }

    public int ChunkPageSize { get; set; }

    public int ChunkSizeBytes { get; set; }

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
        var result = new TwoContiguousPagedFileChunks(this, closePagedMemoryMappedFileOnDispose, osMemoryApi);
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
            memoryMappedFileApi.ReleaseViewOfFile(toBeFreed.Address);
            DecrementUsageCount();
        }
    }

    public MappedPagedFileChunk? CreatePagedFileChunk(int filePageNumber, byte* atAddress)
    {
        CheckDisposed();
        var expectedFileSize = filePageNumber / ChunkPageSize * PageSize + ChunkSizeBytes;
        if (fileStream.Length < expectedFileSize) return null;
        var address = memoryMappedFileApi.MapPageViewOfFile(memoryMappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle(),
            FileMapAccess.Read | FileMapAccess.Write, filePageNumber, ChunkPageSize
            , atAddress);
        if (address == null)
        {
            if (atAddress == null) throw new Exception("Could not create file chunk at any address possible out of memory.");
            return null;
        }

        IncrementUsageCount();
        var mappedPagedFileChunk = new MappedPagedFileChunk(filePageNumber, ChunkPageSize)
        {
            Address = address
        };
        return mappedPagedFileChunk;
    }

    public MappedPagedFileChunk GrowAndReturnChunk(int filePageNumber, byte* atAddress)
    {
        CheckDisposed();
        var expectedFileSize = filePageNumber / ChunkPageSize * PageSize + ChunkSizeBytes;
        fileStream.SetLength(expectedFileSize);
        return CreatePagedFileChunk(filePageNumber, atAddress) ?? CreatePagedFileChunk(filePageNumber, null)!;
    }

    public int? FindChunkPageNumberContainingCursor(long fileCursorOffset, byte* atAddress, bool shouldGrow = false)
    {
        CheckDisposed();
        if (fileStream.Length < fileCursorOffset && !shouldGrow) return null;
        var chunkPageNumber = 0;
        var chunkStartByte = 0;
        while (fileCursorOffset > chunkStartByte + ChunkSizeBytes)
        {
            chunkPageNumber += ChunkPageSize;
            chunkStartByte = chunkPageNumber * ChunkSizeBytes;
        }

        return chunkPageNumber;
    }

    private void CheckDisposed()
    {
        if (isDisposed) throw new ObjectDisposedException(GetType().Name);
    }

    public bool Flush(MappedPagedFileChunk chunk, long fileCursorFrom, nint bytes)
    {
        CheckDisposed();
        var bytesToFlush = bytes;
        byte* addressToFlush;
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

        if (!memoryMappedFileApi.FlushPageDataToDisk(addressToFlush, bytesToFlush))
            throw new Win32Exception();
        fileStream.Flush(true);
        return true;
    }
}
