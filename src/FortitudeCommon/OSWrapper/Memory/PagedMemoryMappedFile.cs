#region

using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class MappedViewRegion : IDisposable
{
    private readonly UsageCountedMemoryMappedFile usageCountedMemoryMappedFile;
    public byte* Address;
    public int FileViewChunkNumber;
    public long StartFileCursorOffset;
    public int ViewSizeInPages;

    public MappedViewRegion(UsageCountedMemoryMappedFile usageCountedMemoryMappedFile, int fileViewChunkNumber, int viewSizeInPages
        , long fileCursorPosition)
    {
        this.usageCountedMemoryMappedFile = usageCountedMemoryMappedFile;
        FileViewChunkNumber = fileViewChunkNumber;
        ViewSizeInPages = viewSizeInPages;
        StartFileCursorOffset = fileCursorPosition;
    }

    public void Dispose()
    {
        usageCountedMemoryMappedFile.DecrementAndDisposeIfRefCount0();
    }
}

public unsafe class EndOfFileEmptyChunk : MappedViewRegion
{
    public EndOfFileEmptyChunk() : base(null!, 0, 0, 0) => Address = null;
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

    public PagedMemoryMappedFile(string filePath, int initialFileSizePages = 2)
    {
        IncrementUsageCount();
        osDirectMemoryApi = osDirectMemoryApi = MemoryUtils.OsDirectMemoryAccess;
        PageSize = (int)osDirectMemoryApi.MinimumRequiredPageSize;
        var initialFileSize = PageSize * Math.Max(2, initialFileSizePages);
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
    }

    public ShiftableMemoryMappedFileView CreateShiftableMemoryMappedFileView(string viewName, int halfViewPageSize = 1, int reserveRegionMultiple = 1
        , bool closePagedMemoryMappedFileOnDispose = true)
    {
        var halfViewNumberOfPages = halfViewPageSize <= 1 ? 1 : MemoryUtils.CeilingNextPowerOfTwo(halfViewPageSize);
        var halfViewSizeBytes = (int)(halfViewNumberOfPages * PageSize);
        var minimumFileSizeForView = halfViewSizeBytes * 2;
        if (minimumFileSizeForView <= 0) minimumFileSizeForView = PageSize * 2;
        var fileSize = fileStream.Length;
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
            osDirectMemoryApi.ReleaseViewOfFile(toBeFreed.Address, toBeFreed.ViewSizeInPages * PageSize);
            toBeFreed.Dispose();
            DecrementUsageCount();
        }
    }

    public bool EnsureFileCanContain(string viewName, int fileChunkNumber, int sizeInPages)
    {
        var chunkSizeBytes = sizeInPages * PageSize;
        var expectedFileSize = fileChunkNumber * chunkSizeBytes + chunkSizeBytes;
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

    public MappedViewRegion? CreatePagedFileChunk(string viewName, int fileChunkNumber, int sizeInPages, void* atAddress)
    {
        CheckDisposed();
        var chunkSizeBytes = sizeInPages * PageSize;
        if (!EnsureFileCanContain(viewName, fileChunkNumber, sizeInPages)) return null;

        var address = osDirectMemoryApi.MapPageViewOfFile(currentLargestRefCountedMemoryMappedFile.MemoryMappedFileHandle
            , fileChunkNumber * sizeInPages
            , sizeInPages
            , atAddress);
        if (address == null)
        {
            var memMapFileError = Marshal.GetLastPInvokeError();
            if (memMapFileError != 0)
            {
                if (memMapFileError == InvalidVirtualMemoryLocation)
                    Logger.Debug("Got InvalidVirtualMemoryLocation when attempting to map memory mapped file {0}-{1} " +
                                 "chunk {2} to virtual memory location {3:x}.  " +
                                 "Location probably in use will attempt to remap to a new location",
                        FileStream.Name, viewName, fileChunkNumber, (nint)atAddress);
                else
                    Logger.Warn("Got Error code {0} on map memory mapped file {1}-{2} chunk {3} to " +
                                "virtual memory location {4:x} when trying to create Memory Mapped File Mapping",
                        memMapFileError, FileStream.Name, viewName, fileChunkNumber, (nint)atAddress);
            }
            else
            {
                Logger.Info("Could not map memory mapped file {0}-{1} chunk {2} to virtual memory location {3:x}.  " +
                            "Location probably in use will attempt to remap to a new location",
                    FileStream.Name, viewName, fileChunkNumber, (nint)atAddress);
            }

            return null;
        }

        IncrementUsageCount();
        var mappedPagedFileChunk
            = new MappedViewRegion(currentLargestRefCountedMemoryMappedFile.IncrementRefCountedMemoryMappedFile(), fileChunkNumber, sizeInPages
                , fileChunkNumber * chunkSizeBytes)
            {
                Address = (byte*)address
            };
        return mappedPagedFileChunk;
    }

    public MappedViewRegion? GrowAndReturnChunk(string viewName, int fileChunkNumber, int viewSizeInPages, byte* atAddress)
    {
        CheckDisposed();
        var chunkSizeBytes = viewSizeInPages * PageSize;
        var expectedFileSize = fileChunkNumber * chunkSizeBytes + chunkSizeBytes;
        if (fileStream.Length < expectedFileSize) fileStream.SetLength(expectedFileSize);

        return CreatePagedFileChunk(viewName, fileChunkNumber, chunkSizeBytes, atAddress);
    }

    public int? FindChunkPageNumberContainingCursor(long fileCursorOffset, int viewSizeInPages, bool shouldGrow = false)
    {
        CheckDisposed();
        var chunkSizeBytes = viewSizeInPages * PageSize;
        if (fileStream.Length < fileCursorOffset && !shouldGrow) return null;
        var chunkNumber = 0;
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

    public bool Flush(MappedViewRegion chunk, long fileCursorFrom, nint bytes)
    {
        CheckDisposed();
        var bytesToFlush = bytes;
        var chunkSizeBytes = chunk.ViewSizeInPages * PageSize;
        void* addressToFlush;
        if (fileCursorFrom > chunk.StartFileCursorOffset + chunkSizeBytes || fileCursorFrom < chunk.StartFileCursorOffset) return false;
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
            var chunkOffset = (int)(fileCursorFrom - chunk.StartFileCursorOffset);
            addressToFlush = chunk.Address + chunkOffset;
            if (bytesToFlush + chunkOffset > PageSize) bytesToFlush = bytesToFlush + chunkOffset - PageSize;
        }

        if (!osDirectMemoryApi.FlushPageDataToDisk(addressToFlush, bytesToFlush))
            throw new Win32Exception();
        fileStream.Flush(true);
        return true;
    }
}
