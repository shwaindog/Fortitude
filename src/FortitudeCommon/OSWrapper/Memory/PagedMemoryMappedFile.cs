#region

using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class ShiftableMemoryMappedFileView : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(ShiftableMemoryMappedFileView));
    private static readonly EndOfFileEmptyChunk BeyondEndOfFileChunk = new();

    private readonly bool closePagedMemoryMappedFileOnDispose;
    private readonly IOSDirectMemoryApi osDirectMemoryApi;
    private readonly PagedMemoryMappedFile pagedMemoryMappedFile;
    private readonly int twoChunkSize;
    private void* foundTwoChunkVirtualMemoryRegion;
    private bool isDisposed;
    private MappedPagedFileChunk? lowerContiguousChunk;
    private MappedPagedFileChunk? upperContiguousChunk;

    public ShiftableMemoryMappedFileView(PagedMemoryMappedFile pagedMemoryMappedFile, bool closePagedMemoryMappedFileOnDispose)
    {
        this.closePagedMemoryMappedFileOnDispose = closePagedMemoryMappedFileOnDispose;
        try
        {
            this.pagedMemoryMappedFile = pagedMemoryMappedFile;
            UpperChunkTriggerChunkShiftTolerance = pagedMemoryMappedFile.ChunkSizeBytes / 4;
            if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.IncrementUsageCount();
            osDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
            twoChunkSize = pagedMemoryMappedFile.ChunkPageSize * 2;
            foundTwoChunkVirtualMemoryRegion = FindTwoChunkFreeVirtualMemorySlot();
            lowerContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(0, foundTwoChunkVirtualMemoryRegion)!;
            if (lowerContiguousChunk == null)
            {
                RemapChunksOnReservedMemory(0, 2);
                return;
            }

            upperContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(1,
                lowerContiguousChunk.Address + pagedMemoryMappedFile.ChunkSizeBytes)!;
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception trying to create TwoContiguousPagedFileChunks on file {0}. Got {1}", pagedMemoryMappedFile.FileStream.Name
                , ex);
            Dispose();
            throw;
        }
    }

    public byte* LowerChunkAddress => lowerContiguousChunk != null ? lowerContiguousChunk.Address : null;

    public byte* EndAddress =>
        IsUpperChunkAvailableForContiguousReadWrite ? upperContiguousChunk!.Address + ChunkSizeBytes : lowerContiguousChunk!.Address + ChunkSizeBytes;

    public long EndFileCursor =>
        IsUpperChunkAvailableForContiguousReadWrite ?
            upperContiguousChunk!.StartFileCursorOffset + ChunkSizeBytes :
            lowerContiguousChunk!.StartFileCursorOffset + ChunkSizeBytes;

    public long ChunkSizeBytes => pagedMemoryMappedFile.ChunkSizeBytes;
    public long LowerChunkNumber => lowerContiguousChunk!.FileChunkNumber;

    public long UpperChunkTriggerChunkShiftTolerance { get; set; }

    public long Size => ChunkSizeBytes + (IsUpperChunkAvailableForContiguousReadWrite ? ChunkSizeBytes : 0);

    public long LowerChunkFileCursorOffset => lowerContiguousChunk?.StartFileCursorOffset ?? 0;

    public bool IsUpperChunkAvailableForContiguousReadWrite =>
        upperContiguousChunk is not null or EndOfFileEmptyChunk && lowerContiguousChunk != null
                                                                && upperContiguousChunk.Address == lowerContiguousChunk.Address +
                                                                ChunkSizeBytes;

    public void Dispose()
    {
        if (isDisposed) return;
        Logger.Debug("Close TwoContiguousPagedFileChunks to file {0}", pagedMemoryMappedFile.FileStream.Name);
        isDisposed = true;
        if (lowerContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerContiguousChunk);
        if (upperContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(upperContiguousChunk);
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.DecrementUsageCount();
    }

    private void* FindTwoChunkFreeVirtualMemorySlot()
    {
        var emptyReservedSpace = osDirectMemoryApi.ReserveMemoryRangeInPages(null, twoChunkSize);
        var wasReleased = osDirectMemoryApi.ReleaseReserveMemoryRangeInPages(emptyReservedSpace, twoChunkSize);
        // Logger.Debug("Found two chunk free slot at {0:X} to {1:X}", (nint)emptyReservedSpace, (nint)emptyReservedSpace + ChunkSizeBytes * 2);
        if (!wasReleased)
        {
            var memMapFileError = Marshal.GetLastPInvokeError();
            if (memMapFileError != 0)
                Logger.Warn("Got Error code {0} when trying to free reserved memory at {1:X}", memMapFileError, (nint)emptyReservedSpace);
            else
                Logger.Warn("Failed to release memory at {0:X}", (nint)emptyReservedSpace);
        }

        return emptyReservedSpace;
    }

    ~ShiftableMemoryMappedFileView()
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

            nextChunk = pagedMemoryMappedFile.GrowAndReturnChunk(upperContiguousChunk.FileChunkNumber + 1,
                upperContiguousChunk.Address + ChunkSizeBytes);
        }
        else
        {
            nextChunk = pagedMemoryMappedFile.CreatePagedFileChunk(upperContiguousChunk.FileChunkNumber + 1,
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

        var requiredFileSize = (lowerChunkPageNumber + 1) * ChunkSizeBytes + ChunkSizeBytes;
        if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
        {
            upperContiguousChunk = BeyondEndOfFileChunk;
            return true;
        }

        checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(lowerChunkPageNumber + 1,
            lowerContiguousChunk.Address + ChunkSizeBytes);
        if (checkChunk == null) return RemapChunksOnReservedMemory(lowerChunkPageNumber, attemptCountDown - 1);
        upperContiguousChunk = checkChunk;
        return true;
    }

    public byte* FileCursorBufferPointer(long fileCursorOffset)
    {
        EnsureLowerChunkContainsFileCursorOffset(fileCursorOffset, false);
        return LowerChunkAddress + fileCursorOffset - LowerChunkFileCursorOffset;
    }

    public bool EnsureLowerChunkContainsFileCursorOffset(long fileCursorOffset, bool shouldGrow = false)
    {
        if (fileCursorOffset > PagedMemoryMappedFile.MaxFileCursorOffset)
            throw new ArgumentOutOfRangeException(
                $"To protect file system the maximum allowed file cursor offset is limited to {PagedMemoryMappedFile.MaxFileCursorOffset}.  Requested {fileCursorOffset}");
        var upperChunkIsBeyondEndOfFile = upperContiguousChunk is EndOfFileEmptyChunk or null;
        var allowedUpperChunk = !upperChunkIsBeyondEndOfFile ? UpperChunkTriggerChunkShiftTolerance : 0;
        if (fileCursorOffset >= lowerContiguousChunk!.StartFileCursorOffset
            && fileCursorOffset < lowerContiguousChunk.StartFileCursorOffset + pagedMemoryMappedFile.ChunkSizeBytes + allowedUpperChunk)
            return false;
        if ((upperChunkIsBeyondEndOfFile && shouldGrow) || (!upperChunkIsBeyondEndOfFile &&
                                                            fileCursorOffset >= upperContiguousChunk!.StartFileCursorOffset &&
                                                            fileCursorOffset < upperContiguousChunk.StartFileCursorOffset +
                                                            pagedMemoryMappedFile.ChunkSizeBytes))
        {
            NextChunk(shouldGrow);
            return true;
        }

        var foundChunkPageNumber
            = pagedMemoryMappedFile.FindChunkPageNumberContainingCursor(fileCursorOffset, foundTwoChunkVirtualMemoryRegion, shouldGrow);
        if (foundChunkPageNumber == null) throw new Exception("Attempted to set cursor beyond end of file");
        RemapChunksOnReservedMemory(foundChunkPageNumber.Value);
        return true;
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
    private readonly UsageCountedMemoryMappedFile usageCountedMemoryMappedFile;
    public byte* Address;
    public int FileChunkNumber;
    public long StartFileCursorOffset;

    public MappedPagedFileChunk(UsageCountedMemoryMappedFile usageCountedMemoryMappedFile, int fileChunkNumber, long fileCursorPosition)
    {
        this.usageCountedMemoryMappedFile = usageCountedMemoryMappedFile;
        FileChunkNumber = fileChunkNumber;
        StartFileCursorOffset = fileCursorPosition;
    }

    public void Dispose()
    {
        usageCountedMemoryMappedFile.DecrementAndDisposeIfRefCount0();
    }
}

public unsafe class EndOfFileEmptyChunk : MappedPagedFileChunk
{
    public EndOfFileEmptyChunk() : base(null!, 0, 0) => Address = null;
}

public class UsageCountedMemoryMappedFile
{
    private readonly MemoryMappedFile memoryMappedFile;
    private int usageCount;

    public UsageCountedMemoryMappedFile(MemoryMappedFile memoryMappedFile, long streamSizeAtCreation)
    {
        this.memoryMappedFile = memoryMappedFile;
        MemoryMappedFileSize = streamSizeAtCreation;
        Interlocked.Increment(ref usageCount);
    }

    public long MemoryMappedFileSize { get; }

    public nint MemoryMappedFileHandle => memoryMappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle();

    public UsageCountedMemoryMappedFile IncrementRefCountedMemoryMappedFile()
    {
        Interlocked.Increment(ref usageCount);
        return this;
    }

    public void DecrementAndDisposeIfRefCount0()
    {
        var currentValue = Interlocked.Decrement(ref usageCount);
        if (currentValue == 0) memoryMappedFile.Dispose();
    }
}

public sealed unsafe class PagedMemoryMappedFile : IDisposable
{
    public const long MaxFileCursorOffset = (long)uint.MaxValue * 4; // ~ 16GB
    public const int InvalidVirtualMemoryLocation = 487;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PagedMemoryMappedFile));
    private readonly IOSDirectMemoryApi osDirectMemoryApi;
    public readonly int PageSize;
    private UsageCountedMemoryMappedFile? currentLargestRefCountedMemoryMappedFile;
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
        var minimumFileSizeForTwoChunks = ChunkSizeBytes * 2;
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
        Logger.Debug("Close PagedMemoryMappedFile to file {0}", FileStream.Name);
        isDisposed = true;
        fileStream.Dispose();
    }

    public ShiftableMemoryMappedFileView CreateTwoContiguousPagedFileChunks(bool closePagedMemoryMappedFileOnDispose = true)
    {
        var result = new ShiftableMemoryMappedFileView(this, closePagedMemoryMappedFileOnDispose);
        if (closePagedMemoryMappedFileOnDispose) DecrementUsageCount();
        return result;
    }

    public int IncrementUsageCount() => Interlocked.Increment(ref liveCount);

    public int DecrementUsageCount()
    {
        Interlocked.Decrement(ref liveCount);
        if (liveCount == 0)
        {
            currentLargestRefCountedMemoryMappedFile?.DecrementAndDisposeIfRefCount0();
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
        var expectedFileSize = fileChunkNumber * ChunkSizeBytes + ChunkSizeBytes;
        if (fileStream.Length < expectedFileSize) return null;
        if (currentLargestRefCountedMemoryMappedFile == null || currentLargestRefCountedMemoryMappedFile.MemoryMappedFileSize < FileStream.Length)
        {
            currentLargestRefCountedMemoryMappedFile?.DecrementAndDisposeIfRefCount0();
            var memoryMappedFile = MemoryMappedFile.CreateFromFile(fileStream, null, 0,
                MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
            currentLargestRefCountedMemoryMappedFile = new UsageCountedMemoryMappedFile(memoryMappedFile, FileStream.Length);
            Logger.Debug("Created new RefCountedMemoryMapped file for {0} at size {1}", fileStream.Name, fileStream.Length);
        }

        var address = osDirectMemoryApi.MapPageViewOfFile(currentLargestRefCountedMemoryMappedFile.MemoryMappedFileHandle
            , fileChunkNumber * ChunkPageSize
            , ChunkPageSize
            , atAddress);
        if (address == null)
        {
            var memMapFileError = Marshal.GetLastPInvokeError();
            if (memMapFileError != 0)
            {
                if (memMapFileError == InvalidVirtualMemoryLocation)
                    Logger.Debug("Could not map memory mapped file {0} chunk {1} to virtual memory location {2:x}.  " +
                                 "Location probably in use will attempt to remap to a new location", FileStream.Name, fileChunkNumber
                        , (nint)atAddress);
                else
                    Logger.Warn("Got Error code {0} when trying to create Memory Mapped File Mapping", memMapFileError);
            }

            if (atAddress == null) Logger.Info("Failed to create chunk at requested address will make more attempts.");
            return null;
        }

        IncrementUsageCount();
        var mappedPagedFileChunk
            = new MappedPagedFileChunk(currentLargestRefCountedMemoryMappedFile.IncrementRefCountedMemoryMappedFile(), fileChunkNumber
                , fileChunkNumber * ChunkSizeBytes)
            {
                Address = (byte*)address
            };
        return mappedPagedFileChunk;
    }

    public MappedPagedFileChunk? GrowAndReturnChunk(int fileChunkNumber, byte* atAddress)
    {
        CheckDisposed();
        var expectedFileSize = fileChunkNumber * ChunkSizeBytes + ChunkSizeBytes;
        if (fileStream.Length < expectedFileSize) fileStream.SetLength(expectedFileSize);

        return CreatePagedFileChunk(fileChunkNumber, atAddress);
    }

    public int? FindChunkPageNumberContainingCursor(long fileCursorOffset, void* atAddress, bool shouldGrow = false)
    {
        CheckDisposed();
        if (fileStream.Length < fileCursorOffset && !shouldGrow) return null;
        var chunkNumber = 0;
        var chunkStartByte = 0;
        while (fileCursorOffset >= chunkStartByte + ChunkSizeBytes)
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
