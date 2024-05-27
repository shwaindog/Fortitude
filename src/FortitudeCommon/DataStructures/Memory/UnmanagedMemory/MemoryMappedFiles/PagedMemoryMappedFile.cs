// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;

public unsafe class MappedViewRegion : IVirtualMemoryAddressRange
{
    public int FilePageOrChunkNumber;

    private PagedMemoryMappedFile?        originPagedMemoryMappedFile;
    public  long                          StartFileCursorOffset;
    private UsageCountedMemoryMappedFile? usageCountedMemoryMappedFile;
    public  int                           ViewSizeInPages;

    public MappedViewRegion(UsageCountedMemoryMappedFile? usageCountedMemoryMappedFile, PagedMemoryMappedFile? originPagedMemoryMappedFile,
        byte* startAddress, int filePageOrChunkNumber, int viewSizeInPages, long fileCursorPosition)
    {
        this.usageCountedMemoryMappedFile = usageCountedMemoryMappedFile;
        this.originPagedMemoryMappedFile  = originPagedMemoryMappedFile;
        StartAddress                      = startAddress;
        FilePageOrChunkNumber             = filePageOrChunkNumber;
        ViewSizeInPages                   = viewSizeInPages;
        StartFileCursorOffset             = fileCursorPosition;
    }

    public byte* StartAddress { get; }
    public byte* EndAddress   => StartAddress + SizeBytes;
    public long  SizeBytes    => ViewSizeInPages * PagedMemoryMappedFile.PageSize;

    public void Flush()
    {
        originPagedMemoryMappedFile?.Flush(this, StartFileCursorOffset, SizeBytes);
    }

    public void Dispose()
    {
        originPagedMemoryMappedFile?.FreeChunk(this);
        originPagedMemoryMappedFile = null;
        usageCountedMemoryMappedFile?.DecrementAndDisposeIfRefCount0();
        usageCountedMemoryMappedFile = null;
    }
}

public unsafe class EndOfFileEmptyChunk : MappedViewRegion
{
    public EndOfFileEmptyChunk() : base(null, null, null,
                                        0, 0, 0) { }
}

public class UsageCountedMemoryMappedFile
{
    private readonly MemoryMappedFile memoryMappedFile;
    private          int              usageCount;

    public UsageCountedMemoryMappedFile(MemoryMappedFile memoryMappedFile, long streamSizeAtCreation)
    {
        this.memoryMappedFile = memoryMappedFile;
        MemoryMappedFileSize  = streamSizeAtCreation;
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
    public const long MaxFileCursorOffset          = (long)uint.MaxValue * 4; // ~ 16GB
    public const int  InvalidVirtualMemoryLocation = 487;

    public static           IOSDirectMemoryApi OSDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
    public static           int                PageSize          = (int)MemoryUtils.OsDirectMemoryAccess.MinimumRequiredPageSize;
    private static readonly IFLogger           Logger            = FLoggerFactory.Instance.GetLogger(typeof(PagedMemoryMappedFile));

    private UsageCountedMemoryMappedFile? currentLargestRefCountedMemoryMappedFile;
    private bool                          deleteOnClose;
    private FileStream                    fileStream;

    private bool isDisposed;
    private int  liveCount;

    public PagedMemoryMappedFile(string filePath, int initialFileSizePages = 2)
    {
        IncrementUsageCount();
        var initialFileSize                       = PageSize * Math.Max(2, initialFileSizePages);
        if (initialFileSize <= 0) initialFileSize = PageSize * 2;

        var existingFile = File.Exists(filePath);
        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        if (existingFile)
        {
            var fileSize = fileStream.Length;
            if (fileStream.Length < initialFileSize || fileStream.Length % initialFileSize != 0)
            {
                var growFileSize = fileSize % initialFileSize + fileSize;
                fileStream.SetLength(growFileSize);
            }
        }
        else
        {
            fileStream.SetLength(initialFileSize);
        }
    }

    public static bool AutoDeleteScratchFiles { get; set; } = true;

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

    public bool IsOpen => !isDisposed;

    public void Dispose()
    {
        if (liveCount > 0 || isDisposed) return;
        Logger.Debug("Close PagedMemoryMappedFile to file {0}", FileStream.Name);
        isDisposed = true;
        fileStream.Dispose();
        if (deleteOnClose) File.Delete(fileStream.Name);
    }

    public static PagedMemoryMappedFile CreateScratchMemoryMappedFile(int sizeInPages = 8)
    {
        var pageMemoryMappedFile = new PagedMemoryMappedFile(Path.GetTempFileName(), sizeInPages)
        {
            deleteOnClose = AutoDeleteScratchFiles
        };
        pageMemoryMappedFile.DecrementUsageCount();
        return pageMemoryMappedFile;
    }

    public static MappedViewRegion CreateScratchMemory(int sizeInBytes = 1024 * 1024)
    {
        var numberOfPages = Math.Max(2, sizeInBytes / (int)MemoryUtils.OsDirectMemoryAccess.MinimumRequiredPageSize);
        var pagedMemoryMappedFile = new PagedMemoryMappedFile(Path.GetTempFileName(), numberOfPages)
        {
            deleteOnClose = AutoDeleteScratchFiles
        };
        pagedMemoryMappedFile.DecrementUsageCount();
        return pagedMemoryMappedFile.CreateMappedViewRegion("ScratchView", 0, numberOfPages, null)!;
    }

    public ShiftableMemoryMappedFileView CreateShiftableMemoryMappedFileView(string viewName, int halfViewPageSize = 1, int reserveRegionMultiple = 1
      , bool closePagedMemoryMappedFileOnDispose = true)
    {
        CheckDisposed();
        var halfViewNumberOfPages                               = halfViewPageSize <= 1 ? 1 : MemoryUtils.CeilingNextPowerOfTwo(halfViewPageSize);
        var halfViewSizeBytes                                   = halfViewNumberOfPages * PageSize;
        var minimumFileSizeForView                              = halfViewSizeBytes * 2;
        if (minimumFileSizeForView <= 0) minimumFileSizeForView = PageSize * 2;
        var fileSize                                            = fileStream.Length;
        if (fileStream.Length % minimumFileSizeForView != 0)
        {
            var growFileSize = fileSize % minimumFileSizeForView + fileSize;
            fileStream.SetLength(growFileSize);
        }

        var result = new ShiftableMemoryMappedFileView(viewName, this, halfViewNumberOfPages, reserveRegionMultiple
                                                     , closePagedMemoryMappedFileOnDispose);
        if (closePagedMemoryMappedFileOnDispose) DecrementUsageCount();
        return result;
    }

    public UnmanagedByteArray CreateVirtualMemoryMappedByteArray(long fileCursorPosition, int length)
    {
        CheckDisposed();
        var pageStart   = (int)(fileCursorPosition / PageSize);
        var pageEnd     = (int)(fileCursorPosition + length) / PageSize;
        var minimumSize = (pageEnd + 1) * PageSize;
        if (fileStream.Length < minimumSize) fileStream.SetLength(minimumSize);
        var numberOfPages = Math.Max(1, pageStart - pageEnd);
        var viewRegion    = CreateMappedViewRegion("VirtualMemoryMappedByteArray", pageStart, numberOfPages, null);
        if (viewRegion!.StartFileCursorOffset > fileCursorPosition)
            throw new Exception("Memory mapped file view does not match expected file position");
        var viewOffset       = fileCursorPosition - viewRegion.StartFileCursorOffset;
        var virtualByteArray = new UnmanagedByteArray(viewRegion, viewOffset, length);
        return virtualByteArray;
    }

    public MemoryMappedFileBuffer CreateMemoryMappedBuffer(long fileCursorPosition, string viewName, int halfViewPageSize = 1
      , int reserveRegionMultiple = 1
      , bool closePagedMemoryMappedFileOnDispose = true)
    {
        CheckDisposed();
        var shiftableView
            = CreateShiftableMemoryMappedFileView(viewName, halfViewPageSize, reserveRegionMultiple, closePagedMemoryMappedFileOnDispose);
        shiftableView.EnsureLowerViewContainsFileCursorOffset(fileCursorPosition);
        var memoryMappedBuffer = new MemoryMappedFileBuffer(shiftableView);
        memoryMappedBuffer.ReadCursor  = fileCursorPosition;
        memoryMappedBuffer.WriteCursor = fileCursorPosition;
        return memoryMappedBuffer;
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

    public void FreeChunk(MappedViewRegion toBeFreed)
    {
        if (toBeFreed is not EndOfFileEmptyChunk)
        {
            // Flush(toBeFreed, toBeFreed.StartFileCursorOffset, toBeFreed.ViewSizeInPages * PageSize);
            OSDirectMemoryApi.ReleaseViewOfFile(toBeFreed.StartAddress, toBeFreed.ViewSizeInPages * PageSize);
            DecrementUsageCount();
        }
    }

    public bool EnsureFileCanContain(string viewName, int pageNumber, int sizeInPages)
    {
        var chunkSizeBytes   = sizeInPages * PageSize;
        var startBytes       = pageNumber * PageSize;
        var expectedFileSize = startBytes + chunkSizeBytes;
        if (fileStream.Length < expectedFileSize) return false;
        if (currentLargestRefCountedMemoryMappedFile == null || currentLargestRefCountedMemoryMappedFile.MemoryMappedFileSize < FileStream.Length)
        {
            currentLargestRefCountedMemoryMappedFile?.DecrementAndDisposeIfRefCount0();
            var memoryMappedFile = MemoryMappedFile.CreateFromFile(fileStream, null, 0,
                                                                   MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
            currentLargestRefCountedMemoryMappedFile = new UsageCountedMemoryMappedFile(memoryMappedFile, FileStream.Length);
            Logger.Debug("Created new RefCountedMemoryMapped file for {0}-{1} at size {2}", fileStream.Name, viewName, fileStream.Length);
        }

        return true;
    }

    public MappedViewRegion? CreateMappedViewRegion(string viewName, int pageStart, int sizeInPages, void* atAddress)
    {
        CheckDisposed();
        if (!EnsureFileCanContain(viewName, pageStart, sizeInPages)) return null;

        var address = OSDirectMemoryApi.MapPageViewOfFile(currentLargestRefCountedMemoryMappedFile!.MemoryMappedFileHandle
                                                        , pageStart
                                                        , sizeInPages
                                                        , atAddress);
        if (address == null)
        {
            var memMapFileError = Marshal.GetLastPInvokeError();
            if (memMapFileError != 0)
            {
                if (memMapFileError == InvalidVirtualMemoryLocation)
                    Logger.Debug("Got InvalidVirtualMemoryLocation when attempting to map memory mapped file {0}-{1} " +
                                 "page number {2} to virtual memory location {3:x}.  " +
                                 "Location probably in use will attempt to remap to a new location",
                                 FileStream.Name, viewName, pageStart, (nint)atAddress);
                else
                    Logger.Warn("Got Error code {0} on map memory mapped file {1}-{2} page number {3} to " +
                                "virtual memory location {4:x} when trying to create Memory Mapped File Mapping",
                                memMapFileError, FileStream.Name, viewName, pageStart, (nint)atAddress);
            }
            else
            {
                Logger.Info("Could not map memory mapped file {0}-{1} page number {2} to virtual memory location {3:x}.  " +
                            "Location probably in use will attempt to remap to a new location",
                            FileStream.Name, viewName, pageStart, (nint)atAddress);
            }

            return null;
        }

        IncrementUsageCount();
        var startPos = pageStart * PageSize;
        var mappedPagedFileChunk
            = new MappedViewRegion(currentLargestRefCountedMemoryMappedFile.IncrementRefCountedMemoryMappedFile(), this,
                                   (byte*)address, pageStart, sizeInPages, startPos);
        return mappedPagedFileChunk;
    }

    public MappedViewRegion? CreatePagedFileChunk(string viewName, int fileChunkNumber, int sizeInPages, void* atAddress)
    {
        CheckDisposed();
        var pageNumber                                                       = sizeInPages * fileChunkNumber;
        var mappedRegionView                                                 = CreateMappedViewRegion(viewName, pageNumber, sizeInPages, atAddress);
        if (mappedRegionView != null) mappedRegionView.FilePageOrChunkNumber = fileChunkNumber;
        return mappedRegionView;
    }

    public MappedViewRegion? GrowAndReturnChunk(string viewName, int fileChunkNumber, int viewSizeInPages, byte* atAddress)
    {
        CheckDisposed();
        var chunkSizeBytes   = viewSizeInPages * PageSize;
        var expectedFileSize = fileChunkNumber * chunkSizeBytes + chunkSizeBytes;
        if (fileStream.Length < expectedFileSize) fileStream.SetLength(expectedFileSize);

        return CreatePagedFileChunk(viewName, fileChunkNumber, chunkSizeBytes, atAddress);
    }

    public int? FindChunkPageNumberContainingCursor(long fileCursorOffset, int viewSizeInPages, bool shouldGrow = false)
    {
        CheckDisposed();
        var chunkSizeBytes = viewSizeInPages * PageSize;
        if (fileStream.Length < fileCursorOffset && !shouldGrow) return null;
        var chunkNumber    = 0;
        var chunkStartByte = 0;
        while (fileCursorOffset >= chunkStartByte + chunkSizeBytes)
        {
            chunkNumber++;
            chunkStartByte = chunkNumber * chunkSizeBytes;
        }

        return chunkNumber;
    }

    private void CheckDisposed()
    {
        if (isDisposed) throw new ObjectDisposedException(GetType().Name);
    }

    public bool Flush(MappedViewRegion chunk, long fileCursorFrom, long bytes)
    {
        CheckDisposed();
        var   bytesToFlush   = bytes;
        var   chunkSizeBytes = chunk.ViewSizeInPages * PageSize;
        void* addressToFlush;
        if (fileCursorFrom > chunk.StartFileCursorOffset + chunkSizeBytes || fileCursorFrom < chunk.StartFileCursorOffset) return false;
        if (fileCursorFrom < chunk.StartFileCursorOffset)
        {
            if (fileCursorFrom + bytes < chunk.StartFileCursorOffset)
            {
                var bytesToGetToStartOfChunk = chunk.StartFileCursorOffset - fileCursorFrom;
                bytesToFlush   = (nint)(bytes - bytesToGetToStartOfChunk);
                addressToFlush = chunk.StartAddress;
            }
            else
            {
                return false;
            }
        }
        else
        {
            var chunkOffset = (int)(fileCursorFrom - chunk.StartFileCursorOffset);
            addressToFlush = chunk.StartAddress + chunkOffset;
            if (bytesToFlush + chunkOffset > PageSize) bytesToFlush = bytesToFlush + chunkOffset - PageSize;
        }

        if (!OSDirectMemoryApi.FlushPageDataToDisk(addressToFlush, (nint)bytesToFlush))
            throw new Win32Exception();
        fileStream.Flush(true);
        return true;
    }
}
