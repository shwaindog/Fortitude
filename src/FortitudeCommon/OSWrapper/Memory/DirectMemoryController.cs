using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;

namespace FortitudeCommon.OSWrapper.Memory;

public interface IDirectMemoryController
{
    IPagedMemoryMappedFile CreateScratchMemoryMappedFile(int sizeInPages = 8);
    
    IMappedViewRegion CreateScratchMemory(int sizeInBytes = 1024 * 1024);

    IVirtualMemoryAddressRange AllocVirtualMemory(long size);

    IVirtualMemoryAddressRange ResizeVirtualMemory(IVirtualMemoryAddressRange existing, long newSize);

    IUnmanagedByteArray CreateUnmanagedByteArray(long size);

    IAcceptsByteArrayStream CreateByteArrayMemoryStream(long size, bool writable = true, bool closeByteArrayOnDispose = true);
}

public class DirectMemoryController : IDirectMemoryController
{
    private static IDirectMemoryController? instance;

    public static IDirectMemoryController Instance
    {
        get => instance ??= new DirectMemoryController();
        set => instance = value;
    }

    public IPagedMemoryMappedFile CreateScratchMemoryMappedFile(int sizeInPages = 8) =>
        PagedMemoryMappedFile.CreateScratchMemoryMappedFile(sizeInPages);

    public IMappedViewRegion CreateScratchMemory(int sizeInBytes = 1048576) => PagedMemoryMappedFile.CreateScratchMemory(sizeInBytes);

    public IVirtualMemoryAddressRange AllocVirtualMemory(long size) => MemoryUtils.AllocVirtualMemory(size);

    public IVirtualMemoryAddressRange ResizeVirtualMemory(IVirtualMemoryAddressRange existing, long newSize) => 
        MemoryUtils.ResizeVirtualMemory((DisposableVirtualMemoryRange)existing, newSize);

    public IUnmanagedByteArray CreateUnmanagedByteArray(long size) => MemoryUtils.CreateUnmanagedByteArray(size);

    public IAcceptsByteArrayStream CreateByteArrayMemoryStream(long size, bool writable = true, bool closeByteArrayOnDispose = true) =>
        MemoryUtils.CreateByteArrayMemoryStream(size, writable, closeByteArrayOnDispose);
}
