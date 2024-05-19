#region

using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class ShiftableMemoryMappedFileView : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(ShiftableMemoryMappedFileView));
    private static readonly EndOfFileEmptyChunk BeyondEndOfFileChunk = new();
    private readonly int freeRegionPageSize;
    private readonly IOSDirectMemoryApi osDirectMemoryApi;
    private readonly PagedMemoryMappedFile pagedMemoryMappedFile;

    private readonly int reserveRegionViewMultiple;
    private bool closePagedMemoryMappedFileOnDispose;
    private void* foundTwoChunkVirtualMemoryRegion;
    private bool isDisposed;
    private MappedViewRegion? lowerViewContiguousChunk;
    private MappedViewRegion? upperViewContiguousChunk;

    public ShiftableMemoryMappedFileView(string viewName, PagedMemoryMappedFile pagedMemoryMappedFile, int halfViewNumberOfPages
        , int reserveRegionViewMultiple
        , bool closePagedMemoryMappedFileOnDispose)
    {
        ViewName = viewName;
        this.reserveRegionViewMultiple = reserveRegionViewMultiple;
        this.closePagedMemoryMappedFileOnDispose = closePagedMemoryMappedFileOnDispose;
        HalfViewPageSize = halfViewNumberOfPages;
        HalfViewSizeBytes = halfViewNumberOfPages * pagedMemoryMappedFile.PageSize;
        try
        {
            this.pagedMemoryMappedFile = pagedMemoryMappedFile;
            UpperViewTriggerChunkShiftTolerance = HalfViewSizeBytes / 4;
            if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.IncrementUsageCount();
            osDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
            var checkReserveRegionViewMultiple = Math.Min(60, Math.Max(1, reserveRegionViewMultiple));
            freeRegionPageSize = HalfViewPageSize * 2 * checkReserveRegionViewMultiple;
            foundTwoChunkVirtualMemoryRegion = FindFreeVirtualMemoryForView();
            lowerViewContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(viewName, 0, HalfViewPageSize, foundTwoChunkVirtualMemoryRegion)!;
            if (lowerViewContiguousChunk == null)
            {
                AttemptRemapViewOnContiguousVirtualMemoryRegion(0, 2);
                return;
            }

            upperViewContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk(viewName, 1, HalfViewPageSize,
                lowerViewContiguousChunk.Address + HalfViewSizeBytes)!;
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception trying to create ShiftableMemoryMappedFileView on file {0}-{1}. Got {2}",
                pagedMemoryMappedFile.FileStream.Name, ViewName, ex);
            Dispose();
            throw;
        }
    }

    public string ViewName { get; set; }

    public string FileName => pagedMemoryMappedFile.FileStream.Name;

    public byte* LowerHalfViewVirtualMemoryAddress => lowerViewContiguousChunk != null ? lowerViewContiguousChunk.Address : null;

    public byte* HighestViewAddress =>
        IsUpperViewAvailableForContiguousReadWrite ?
            upperViewContiguousChunk!.Address + HalfViewSizeBytes :
            lowerViewContiguousChunk!.Address + HalfViewSizeBytes;

    public long HighestFileCursor =>
        IsUpperViewAvailableForContiguousReadWrite ?
            upperViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes :
            lowerViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes;


    public int HalfViewPageSize { get; }

    public long HalfViewSizeBytes { get; }
    public long LowerViewFileChunkNumber => lowerViewContiguousChunk!.FileViewChunkNumber;

    public long UpperViewTriggerChunkShiftTolerance { get; set; }

    public long ViewSizeBytes => HalfViewSizeBytes + (IsUpperViewAvailableForContiguousReadWrite ? HalfViewSizeBytes : 0);

    public long LowerViewFileCursorOffset => lowerViewContiguousChunk?.StartFileCursorOffset ?? 0;

    public bool IsUpperViewAvailableForContiguousReadWrite =>
        upperViewContiguousChunk is not null or EndOfFileEmptyChunk && lowerViewContiguousChunk != null
                                                                    && upperViewContiguousChunk.Address == lowerViewContiguousChunk.Address +
                                                                    HalfViewSizeBytes;

    public void Dispose()
    {
        if (isDisposed) return;
        Logger.Debug("Close TwoContiguousPagedFileChunks to file {0}-{1}", pagedMemoryMappedFile.FileStream.Name, ViewName);
        isDisposed = true;
        if (lowerViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerViewContiguousChunk);
        if (upperViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(upperViewContiguousChunk);
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.DecrementUsageCount();
    }

    public ShiftableMemoryMappedFileView CreateEphemeralViewThatCanCover(string viewName, int sizeBytes)
    {
        var numberOfPages = sizeBytes / pagedMemoryMappedFile.PageSize;
        numberOfPages += sizeBytes % pagedMemoryMappedFile.PageSize != 0 ? 1 : 0;
        var halfViewPages = numberOfPages / 2;
        halfViewPages += numberOfPages % 2 != 0 ? numberOfPages % 2 : 0;
        return pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView(viewName, halfViewPages, 1, false);
    }

    public ShiftableMemoryMappedFileView ReplaceViewThatCanCover(string viewName, int sizeBytes)
    {
        var numberOfPages = sizeBytes / pagedMemoryMappedFile.PageSize;
        numberOfPages += sizeBytes % pagedMemoryMappedFile.PageSize != 0 ? 1 : 0;
        var halfViewPages = numberOfPages / 2;
        halfViewPages += numberOfPages % 2 != 0 ? numberOfPages % 2 : 0;
        if (halfViewPages <= HalfViewPageSize) return this;
        var newReplacedView
            = pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView(viewName, halfViewPages, reserveRegionViewMultiple
                , closePagedMemoryMappedFileOnDispose);
        closePagedMemoryMappedFileOnDispose = false;
        Dispose();
        return newReplacedView;
    }

    private void* FindFreeVirtualMemoryForView()
    {
        var emptyReservedSpace = osDirectMemoryApi.ReserveMemoryRangeInPages(null, freeRegionPageSize);
        var wasReleased = osDirectMemoryApi.ReleaseReserveMemoryRangeInPages(emptyReservedSpace, freeRegionPageSize);
        // Logger.Debug("Found two chunk free slot at {0:X} to {1:X}", (nint)emptyReservedSpace, (nint)emptyReservedSpace + ChunkSizeBytes * 2);
        if (!wasReleased)
        {
            var memMapFileError = Marshal.GetLastPInvokeError();
            if (memMapFileError != 0)
                Logger.Warn("Got Error code {0} when trying to free reserved memory at {1:X} for {2}-{3}",
                    memMapFileError, (nint)emptyReservedSpace, FileName, ViewName);
            else
                Logger.Warn("Failed to release memory at {0:X} for {1}-{2}", (nint)emptyReservedSpace, FileName, ViewName);
        }

        return emptyReservedSpace;
    }

    ~ShiftableMemoryMappedFileView()
    {
        Dispose();
    }

    public bool ShiftFileUpByHalfOfViewSize(bool shouldGrow = false)
    {
        if (upperViewContiguousChunk is null or EndOfFileEmptyChunk) return false;
        var requiredFileSize = (upperViewContiguousChunk.FileViewChunkNumber + 1) * HalfViewSizeBytes + HalfViewSizeBytes;
        MappedViewRegion? nextChunk;
        if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
        {
            if (!shouldGrow)
            {
                if (upperViewContiguousChunk is EndOfFileEmptyChunk) return false;
                if (lowerViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerViewContiguousChunk);
                lowerViewContiguousChunk = upperViewContiguousChunk;
                upperViewContiguousChunk = BeyondEndOfFileChunk;
                return true;
            }

            nextChunk = pagedMemoryMappedFile.GrowAndReturnChunk(ViewName, upperViewContiguousChunk.FileViewChunkNumber + 1, HalfViewPageSize,
                upperViewContiguousChunk.Address + HalfViewSizeBytes);
        }
        else
        {
            nextChunk = pagedMemoryMappedFile.CreatePagedFileChunk(ViewName, upperViewContiguousChunk.FileViewChunkNumber + 1,
                HalfViewPageSize, upperViewContiguousChunk.Address + HalfViewSizeBytes);
        }

        if (nextChunk == null) return AttemptRemapViewOnContiguousVirtualMemoryRegion(upperViewContiguousChunk.FileViewChunkNumber);
        if (lowerViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerViewContiguousChunk);
        lowerViewContiguousChunk = upperViewContiguousChunk;
        upperViewContiguousChunk = nextChunk;
        return true;
    }

    public bool AttemptRemapViewOnContiguousVirtualMemoryRegion(int lowerChunkPageNumber, int attemptCountDown = 3)
    {
        if (attemptCountDown < 0)
            throw new Exception($"Unexpected mapping failed on reserved memory for file {FileName}-{ViewName}.  " +
                                "Previous file chunks were not released properly");

        if (lowerViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(lowerViewContiguousChunk);
        lowerViewContiguousChunk = null;
        if (upperViewContiguousChunk != null) pagedMemoryMappedFile.FreeChunk(upperViewContiguousChunk);
        upperViewContiguousChunk = null;
        var allocationAddress = attemptCountDown == 3 ? foundTwoChunkVirtualMemoryRegion : null;
        if (attemptCountDown < 3)
        {
            foundTwoChunkVirtualMemoryRegion = FindFreeVirtualMemoryForView();
            allocationAddress = attemptCountDown != 0 ? foundTwoChunkVirtualMemoryRegion : null;
        }

        var checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(ViewName, lowerChunkPageNumber, HalfViewPageSize, allocationAddress);
        if (checkChunk == null) return AttemptRemapViewOnContiguousVirtualMemoryRegion(lowerChunkPageNumber, attemptCountDown - 1);
        lowerViewContiguousChunk = checkChunk;

        var requiredFileSize = (lowerChunkPageNumber + 1) * HalfViewSizeBytes + HalfViewSizeBytes;
        if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
        {
            upperViewContiguousChunk = BeyondEndOfFileChunk;
            return true;
        }

        checkChunk = pagedMemoryMappedFile.CreatePagedFileChunk(ViewName, lowerChunkPageNumber + 1, HalfViewPageSize,
            lowerViewContiguousChunk.Address + HalfViewSizeBytes);
        if (checkChunk == null) return AttemptRemapViewOnContiguousVirtualMemoryRegion(lowerChunkPageNumber, attemptCountDown - 1);
        upperViewContiguousChunk = checkChunk;
        return true;
    }

    public byte* FileCursorBufferPointer(long fileCursorOffset, long maxUpperChunkTolerance = ushort.MaxValue, bool shouldGrow = false)
    {
        EnsureLowerViewContainsFileCursorOffset(fileCursorOffset, maxUpperChunkTolerance, shouldGrow);
        return LowerHalfViewVirtualMemoryAddress + fileCursorOffset - LowerViewFileCursorOffset;
    }

    public bool EnsureLowerViewContainsFileCursorOffset(long fileCursorOffset, long maxUpperChunkTolerance = ushort.MaxValue, bool shouldGrow = false)
    {
        if (fileCursorOffset > PagedMemoryMappedFile.MaxFileCursorOffset)
            throw new ArgumentOutOfRangeException(
                $"To protect file system the maximum allowed file cursor offset is limited to {PagedMemoryMappedFile.MaxFileCursorOffset}.  Requested {fileCursorOffset}");
        var upperChunkIsBeyondEndOfFile = upperViewContiguousChunk is EndOfFileEmptyChunk or null;
        var allowedUpperChunk = !upperChunkIsBeyondEndOfFile ? Math.Min(UpperViewTriggerChunkShiftTolerance, maxUpperChunkTolerance) : 0;
        if (fileCursorOffset >= lowerViewContiguousChunk!.StartFileCursorOffset
            && fileCursorOffset < lowerViewContiguousChunk.StartFileCursorOffset + HalfViewSizeBytes + allowedUpperChunk)
            return false;
        switch (upperChunkIsBeyondEndOfFile)
        {
            case true when shouldGrow && fileCursorOffset >= lowerViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes
                                      && fileCursorOffset < lowerViewContiguousChunk.StartFileCursorOffset + 2 * HalfViewSizeBytes:
                pagedMemoryMappedFile.EnsureFileCanContain(ViewName, lowerViewContiguousChunk.FileViewChunkNumber + 1, HalfViewPageSize);
                AttemptRemapViewOnContiguousVirtualMemoryRegion(lowerViewContiguousChunk.FileViewChunkNumber + 1);
                return true;
            case false when fileCursorOffset >= upperViewContiguousChunk!.StartFileCursorOffset &&
                            fileCursorOffset < upperViewContiguousChunk.StartFileCursorOffset + HalfViewSizeBytes:
                ShiftFileUpByHalfOfViewSize(shouldGrow);
                return true;
        }

        var foundChunkPageNumber = pagedMemoryMappedFile.FindChunkPageNumberContainingCursor(fileCursorOffset, HalfViewPageSize, shouldGrow);
        if (foundChunkPageNumber == null) throw new Exception("Attempted to set cursor beyond end of file");
        AttemptRemapViewOnContiguousVirtualMemoryRegion(foundChunkPageNumber.Value);
        return true;
    }

    public bool FlushCursorDataToDisk(long fileCursorOffset, int sizeToFlush)
    {
        var wasFlushed = lowerViewContiguousChunk != null && pagedMemoryMappedFile.Flush(lowerViewContiguousChunk, fileCursorOffset, sizeToFlush);
        wasFlushed |= upperViewContiguousChunk != null && pagedMemoryMappedFile.Flush(upperViewContiguousChunk, fileCursorOffset, sizeToFlush);
        return wasFlushed;
    }

    public bool FlushPtrDataToDisk(void* pointerAddress, int sizeToFlush)
    {
        var fileCursorOffset = (byte*)pointerAddress - LowerHalfViewVirtualMemoryAddress + LowerViewFileCursorOffset;
        var wasFlushed = lowerViewContiguousChunk != null && pagedMemoryMappedFile.Flush(lowerViewContiguousChunk, fileCursorOffset, sizeToFlush);
        wasFlushed |= upperViewContiguousChunk != null && pagedMemoryMappedFile.Flush(upperViewContiguousChunk, fileCursorOffset, sizeToFlush);
        return wasFlushed;
    }
}
