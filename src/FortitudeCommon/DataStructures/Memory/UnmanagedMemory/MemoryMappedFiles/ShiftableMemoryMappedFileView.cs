// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;

public unsafe class ShiftableMemoryMappedFileView : IVirtualMemoryAddressRange
{
    private const long MaxViewSizeBytes                = PagedMemoryMappedFile.MaxFileCursorOffset; // 16Gb;
    public const  long DefaultMinimumShiftableViewSize = ushort.MaxValue * 2;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(ShiftableMemoryMappedFileView));

    private static readonly EndOfFileEmptyChunk BeyondEndOfFileChunk = new();

    private readonly IOSDirectMemoryApi    osDirectMemoryApi;
    private readonly PagedMemoryMappedFile pagedMemoryMappedFile;

    private readonly int reserveRegionViewMultiple;

    private bool  closePagedMemoryMappedFileOnDispose;
    private void* foundTwoChunkVirtualMemoryRegion;
    private long  halfViewSizeBytes;
    private bool  isDisposed;

    private MappedViewRegion? lowerViewContiguousChunk;
    private MappedViewRegion? upperViewContiguousChunk;

    public ShiftableMemoryMappedFileView
    (string viewName, PagedMemoryMappedFile pagedMemoryMappedFile, int startChunkNumber, int halfViewNumberOfPages
      , int reserveRegionViewMultiple
      , bool closePagedMemoryMappedFileOnDispose)
    {
        ViewName = viewName;

        this.reserveRegionViewMultiple = reserveRegionViewMultiple;

        this.closePagedMemoryMappedFileOnDispose = closePagedMemoryMappedFileOnDispose;

        HalfViewPageSize  = halfViewNumberOfPages;
        HalfViewSizeBytes = halfViewNumberOfPages * PagedMemoryMappedFile.PageSize;
        try
        {
            this.pagedMemoryMappedFile          = pagedMemoryMappedFile;
            UpperViewTriggerChunkShiftTolerance = HalfViewSizeBytes / 4;
            if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.IncrementUsageCount();
            osDirectMemoryApi                = MemoryUtils.OsDirectMemoryAccess;
            foundTwoChunkVirtualMemoryRegion = FindFreeVirtualMemoryForView();
            lowerViewContiguousChunk
                = pagedMemoryMappedFile.CreatePagedFileChunk(viewName, startChunkNumber, HalfViewPageSize, foundTwoChunkVirtualMemoryRegion)!;
            if (lowerViewContiguousChunk == null)
            {
                AttemptRemapViewOnContiguousVirtualMemoryRegion(0, 2);
                return;
            }

            upperViewContiguousChunk = pagedMemoryMappedFile.CreatePagedFileChunk
                (viewName, startChunkNumber + 1, HalfViewPageSize, lowerViewContiguousChunk.EndAddress)!;
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

    public long FileSize => pagedMemoryMappedFile.Length;

    public long HighestFileCursor =>
        IsUpperViewAvailableForContiguousReadWrite
            ? upperViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes
            : lowerViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes;

    public int HalfViewPageSize { get; private set; }

    public long HalfViewSizeBytes
    {
        get => halfViewSizeBytes;
        private set
        {
            if (value > MaxViewSizeBytes / 2) throw new ArgumentException($"Can not create a view greater than {MaxViewSizeBytes:###,##0} bytes");
            halfViewSizeBytes = value;
        }
    }
    public long LowerViewFileChunkNumber => lowerViewContiguousChunk!.FilePageOrChunkNumber;

    public long UpperViewTriggerChunkShiftTolerance { get; set; }

    public long LowerViewFileCursorOffset => lowerViewContiguousChunk?.StartFileCursorOffset ?? 0;

    public bool IsUpperViewAvailableForContiguousReadWrite =>
        upperViewContiguousChunk is not (null or EndOfFileEmptyChunk) && lowerViewContiguousChunk != null
                                                                      && upperViewContiguousChunk.StartAddress ==
                                                                         lowerViewContiguousChunk.EndAddress;

    public long DefaultGrowSize => HalfViewPageSize;

    public IVirtualMemoryAddressRange GrowByDefaultSize()
    {
        ShiftFileUpByHalfOfViewSize(true);
        return this;
    }

    public UnmanagedByteArray CreateUnmanagedByteArrayInThisRange(long viewOffset, int length)
    {
        if (viewOffset < 0 || viewOffset + length > Length)
            throw new ArgumentOutOfRangeException("UnmanagedByteArray can not be mapped onto this range");
        return new UnmanagedByteArray(this, viewOffset, length);
    }

    public byte* StartAddress => lowerViewContiguousChunk != null ? lowerViewContiguousChunk.StartAddress : null;

    public byte* EndAddress =>
        IsUpperViewAvailableForContiguousReadWrite
            ? upperViewContiguousChunk!.StartAddress + HalfViewSizeBytes
            : lowerViewContiguousChunk!.StartAddress + HalfViewSizeBytes;

    public long Length => HalfViewSizeBytes + (IsUpperViewAvailableForContiguousReadWrite ? HalfViewSizeBytes : 0);

    public void Dispose()
    {
        if (isDisposed) return;
        Logger.Debug("Close TwoContiguousPagedFileChunks to file {0}-{1}.", pagedMemoryMappedFile.FileStream.Name, ViewName);
        isDisposed = true;
        lowerViewContiguousChunk?.Dispose();
        upperViewContiguousChunk?.Dispose();
        if (closePagedMemoryMappedFileOnDispose) pagedMemoryMappedFile.DecrementUsageCount();
    }

    public void Flush()
    {
        if (lowerViewContiguousChunk != null)
            pagedMemoryMappedFile.Flush(lowerViewContiguousChunk, lowerViewContiguousChunk.StartFileCursorOffset, lowerViewContiguousChunk.Length);
        if (upperViewContiguousChunk != null)
            pagedMemoryMappedFile.Flush(upperViewContiguousChunk, upperViewContiguousChunk.StartFileCursorOffset, upperViewContiguousChunk.Length);
    }

    public ShiftableMemoryMappedFileView CreateEphemeralViewThatCanCover(string viewName, long fileCursorOffset, long sizeBytes) =>
        pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView(viewName, fileCursorOffset, sizeBytes, 1, false);

    public ShiftableMemoryMappedFileView ReplaceViewThatCanCover(string viewName, long fileCursorOffset, long sizeBytes)
    {
        var newReplacedView
            = pagedMemoryMappedFile.CreateShiftableMemoryMappedFileView(viewName, fileCursorOffset, sizeBytes, reserveRegionViewMultiple
                                                                      , closePagedMemoryMappedFileOnDispose);
        closePagedMemoryMappedFileOnDispose = false;
        Dispose();
        return newReplacedView;
    }

    private void* FindFreeVirtualMemoryForView()
    {
        var maxPageRangeCount              = (int)(MaxViewSizeBytes / PagedMemoryMappedFile.PageSize);
        var checkReserveRegionViewMultiple = Math.Min(maxPageRangeCount, Math.Max(1, reserveRegionViewMultiple)); // limit to 65 GB on windows
        var freeRegionPageSize             = HalfViewPageSize * 2 * checkReserveRegionViewMultiple;
        var emptyReservedSpace             = osDirectMemoryApi.ReserveMemoryRangeInPages(null, freeRegionPageSize);
        var wasReleased                    = osDirectMemoryApi.ReleaseReserveMemoryRangeInPages(emptyReservedSpace, freeRegionPageSize);
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
        var requiredFileSize = (upperViewContiguousChunk.FilePageOrChunkNumber + 1) * HalfViewSizeBytes + HalfViewSizeBytes;

        MappedViewRegion? nextChunk;
        if (pagedMemoryMappedFile.FileStream.Length < requiredFileSize)
        {
            if (!shouldGrow)
            {
                if (upperViewContiguousChunk is EndOfFileEmptyChunk) return false;
                lowerViewContiguousChunk?.Dispose();
                lowerViewContiguousChunk = upperViewContiguousChunk;
                upperViewContiguousChunk = BeyondEndOfFileChunk;
                return true;
            }

            nextChunk = pagedMemoryMappedFile.GrowAndReturnChunk
                (ViewName, upperViewContiguousChunk.FilePageOrChunkNumber + 1, HalfViewPageSize, upperViewContiguousChunk.EndAddress);
        }
        else
        {
            nextChunk = pagedMemoryMappedFile.CreatePagedFileChunk
                (ViewName, upperViewContiguousChunk.FilePageOrChunkNumber + 1, HalfViewPageSize, upperViewContiguousChunk.EndAddress);
        }

        if (nextChunk == null) return AttemptRemapViewOnContiguousVirtualMemoryRegion(upperViewContiguousChunk.FilePageOrChunkNumber);
        lowerViewContiguousChunk?.Dispose();
        lowerViewContiguousChunk = upperViewContiguousChunk;
        upperViewContiguousChunk = nextChunk;
        return true;
    }

    public bool AttemptRemapViewOnContiguousVirtualMemoryRegion(int lowerChunkPageNumber, int attemptCountDown = 3)
    {
        if (attemptCountDown < 0)
            throw new Exception($"Unexpected mapping failed on reserved memory for file {FileName}-{ViewName}.  " +
                                "Previous file chunks were not released properly");

        lowerViewContiguousChunk?.Dispose();
        lowerViewContiguousChunk = null;
        upperViewContiguousChunk?.Dispose();
        upperViewContiguousChunk = null;
        var allocationAddress = attemptCountDown == 3 ? foundTwoChunkVirtualMemoryRegion : null;
        if (attemptCountDown < 3)
        {
            foundTwoChunkVirtualMemoryRegion = FindFreeVirtualMemoryForView();
            allocationAddress                = attemptCountDown != 0 ? foundTwoChunkVirtualMemoryRegion : null;
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
                                                                lowerViewContiguousChunk.EndAddress);
        if (checkChunk == null) return AttemptRemapViewOnContiguousVirtualMemoryRegion(lowerChunkPageNumber, attemptCountDown - 1);
        upperViewContiguousChunk = checkChunk;
        return true;
    }

    public byte* FixedFileCursorBufferPointer(long fileCursorOffset, long maxUpperChunkTolerance = ushort.MaxValue, bool shouldGrow = false) =>
        StartAddress + fileCursorOffset - LowerViewFileCursorOffset;

    public byte* FileCursorBufferPointer(long fileCursorOffset, long maxUpperChunkTolerance = ushort.MaxValue, bool shouldGrow = false)
    {
        EnsureLowerViewContainsFileCursorOffset(fileCursorOffset, maxUpperChunkTolerance, shouldGrow);
        return StartAddress + fileCursorOffset - LowerViewFileCursorOffset;
    }

    public bool EnsureViewCoversFileCursorOffsetAndSize(long fileCursorOffset, long minimumViewSize, bool shouldGrow = false)
    {
        if (fileCursorOffset > PagedMemoryMappedFile.MaxFileCursorOffset)
            throw new ArgumentOutOfRangeException($"To protect file system the maximum allowed file cursor offset is limited to " +
                                                  $"{PagedMemoryMappedFile.MaxFileCursorOffset}.  Requested {fileCursorOffset}");

        if (fileCursorOffset >= lowerViewContiguousChunk!.StartFileCursorOffset
         && fileCursorOffset + minimumViewSize < HighestFileCursor)
            return false;

        if ((upperViewContiguousChunk is not (EndOfFileEmptyChunk or null) || shouldGrow)
         && fileCursorOffset >= upperViewContiguousChunk!.StartFileCursorOffset
         && fileCursorOffset + minimumViewSize < HighestFileCursor + HalfViewPageSize)
        {
            ShiftFileUpByHalfOfViewSize(shouldGrow);
            return true;
        }

        var pageSize = PagedMemoryMappedFile.PageSize;

        var interimHalfMinimumViewSize = minimumViewSize / 2;
        var safeHalfMinimumViewPages =
            MemoryUtils.CeilingNextPowerOfTwo
                (Math.Max(HalfViewPageSize, (int)(interimHalfMinimumViewSize /
                              pageSize + (interimHalfMinimumViewSize % pageSize == 0 ? 0 : 1))));

        var fileCursorDeduction = fileCursorOffset % (safeHalfMinimumViewPages * pageSize);
        while (2 * safeHalfMinimumViewPages * pageSize - fileCursorDeduction < minimumViewSize)
        {
            safeHalfMinimumViewPages++;
            safeHalfMinimumViewPages = MemoryUtils.CeilingNextPowerOfTwo(safeHalfMinimumViewPages);
            fileCursorDeduction      = fileCursorOffset % (safeHalfMinimumViewPages * pageSize);
        }
        var halfViewNumberOfPages = MemoryUtils.CeilingNextPowerOfTwo(safeHalfMinimumViewPages);
        if (HalfViewPageSize < halfViewNumberOfPages)
        {
            HalfViewPageSize  = halfViewNumberOfPages;
            HalfViewSizeBytes = halfViewNumberOfPages * pageSize;
        }

        var fileCursorStartChunkNumber = (int)(fileCursorOffset / HalfViewSizeBytes);
        if (shouldGrow) pagedMemoryMappedFile.GrowFileToEncompass(fileCursorStartChunkNumber + 1, halfViewNumberOfPages);

        foundTwoChunkVirtualMemoryRegion = FindFreeVirtualMemoryForView();
        AttemptRemapViewOnContiguousVirtualMemoryRegion(fileCursorStartChunkNumber);
        return true;
    }

    public bool EnsureLowerViewContainsFileCursorOffset(long fileCursorOffset, long maxUpperChunkTolerance = ushort.MaxValue, bool shouldGrow = false)
    {
        if (fileCursorOffset > PagedMemoryMappedFile.MaxFileCursorOffset)
            throw new ArgumentOutOfRangeException(
                                                  $"To protect file system the maximum allowed file cursor offset is limited to {PagedMemoryMappedFile.MaxFileCursorOffset}.  Requested {fileCursorOffset}");
        var upperChunkIsBeyondEndOfFile = upperViewContiguousChunk is EndOfFileEmptyChunk or null;
        var allowedUpperChunk           = !upperChunkIsBeyondEndOfFile ? Math.Min(UpperViewTriggerChunkShiftTolerance, maxUpperChunkTolerance) : 0;
        if (fileCursorOffset >= lowerViewContiguousChunk!.StartFileCursorOffset
         && fileCursorOffset < lowerViewContiguousChunk.StartFileCursorOffset + HalfViewSizeBytes + allowedUpperChunk)
            return false;
        switch (upperChunkIsBeyondEndOfFile)
        {
            case true when shouldGrow && fileCursorOffset >= lowerViewContiguousChunk!.StartFileCursorOffset + HalfViewSizeBytes
                                      && fileCursorOffset < lowerViewContiguousChunk.StartFileCursorOffset + 2 * HalfViewSizeBytes:
                pagedMemoryMappedFile.GrowFileToEncompass(lowerViewContiguousChunk.FilePageOrChunkNumber + 1, HalfViewPageSize);
                pagedMemoryMappedFile.EnsureFileCanContain(ViewName, lowerViewContiguousChunk.FilePageOrChunkNumber + 1, HalfViewPageSize);
                AttemptRemapViewOnContiguousVirtualMemoryRegion(lowerViewContiguousChunk.FilePageOrChunkNumber + 1);
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
        var fileCursorOffset = (byte*)pointerAddress - StartAddress + LowerViewFileCursorOffset;
        var wasFlushed = lowerViewContiguousChunk != null && pagedMemoryMappedFile.Flush(lowerViewContiguousChunk, fileCursorOffset, sizeToFlush);
        wasFlushed |= upperViewContiguousChunk != null && pagedMemoryMappedFile.Flush(upperViewContiguousChunk, fileCursorOffset, sizeToFlush);
        return wasFlushed;
    }
}
