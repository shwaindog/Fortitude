#region

using System.Runtime.InteropServices;
using System.Security;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class LinuxDirectMemoryApi : IOSDirectMemoryApi
{
    [Flags]
    public enum MemoryMap
    {
        Reserve = 0x00_00
        , MapShared = 0x00_01
        , MapPrivate = 0x00_10
        , MapFixed = 0x00_10
        , FailIfAddrNotAvailable = 0x08_00
        , MapAnon = 0x10_00
        , MapStack = 0x40_00
        , MapConceal = 0x80_00
    }

    [Flags]
    public enum MemoryProtection
    {
        NoPermission = 0x00
        , Read = 0x01
        , Write = 0x02
        , Execute = 0x04
    }

    [Flags]
    public enum MemorySyncFlags
    {
        Async = 0x01
        , Sync = 0x02
        , Invalidate = 0x04
    }

    private static readonly int PageSize = Environment.SystemPageSize;

    public LinuxDirectMemoryApi()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException("This class should only be instantiated on .NET Core on Linux");
    }

    public IntPtr MinimumRequiredPageSize => PageSize;

    public void* ReserveMemoryRangeInPages(void* preferredAddress, int numberOfPages)
    {
        var anyAddressIsFine = preferredAddress == null;
        var mapFlags = anyAddressIsFine ? MemoryMap.Reserve : MemoryMap.FailIfAddrNotAvailable;
        var size = (nint)(numberOfPages * PageSize);
        return (void*)mmap(null, size, MemoryProtection.Read | MemoryProtection.Write, mapFlags, -1, 0);
    }

    public void* CommitPageMemory(void* preferredAddress, int numberOfPages)
    {
        var anyAddressIsFine = preferredAddress == null;
        var mapFlags = MemoryMap.MapAnon | (anyAddressIsFine ? MemoryMap.Reserve : MemoryMap.FailIfAddrNotAvailable);
        var size = (nint)(numberOfPages * PageSize);
        return (void*)mmap(preferredAddress, size, MemoryProtection.Read | MemoryProtection.Write, mapFlags, -1, 0);
    }

    public bool DecommitPageMemory(void* previouslyCommittedAddress, int numberOfPages)
    {
        var size = (nint)(numberOfPages * PageSize);
        return munmap(previouslyCommittedAddress, size) == 0;
    }

    public bool ReleaseReserveMemoryRangeInPages(void* previouslyReservedAddress, int numberOfPages)
    {
        var size = (nint)(numberOfPages * PageSize);
        return munmap(previouslyReservedAddress, size) == 0;
    }

    public void* MapPageViewOfFile(IntPtr memoryMappedFileHandle, int filePageNumber, int numberOfPages, void* desiredAddress)
    {
        var anyAddressIsFine = desiredAddress == null;
        var mapFlags = MemoryMap.MapShared | (anyAddressIsFine ? MemoryMap.Reserve : MemoryMap.FailIfAddrNotAvailable);
        var size = (nint)numberOfPages * PageSize;
        var toFileOffset = (nint)filePageNumber * PageSize;
        return (void*)mmap(desiredAddress, size, MemoryProtection.Read | MemoryProtection.Write,
            mapFlags, memoryMappedFileHandle, toFileOffset);
    }

    public bool ReleaseViewOfFile(void* addressToRelease, int numberOfPages)
    {
        var size = (nint)numberOfPages * PageSize;
        return munmap(addressToRelease, size) == 0;
    }

    public bool FlushPageDataToDisk(void* startAddressToFlush, nint bytesToFlush) =>
        msync(startAddressToFlush, bytesToFlush, MemorySyncFlags.Async) == 0;

    void* IOSDirectMemoryApi.memcpy(void* dest, void* src, ulong count) => memcpy(dest, src, count);


    [DllImport("libc", EntryPoint = "mmap", CallingConvention = CallingConvention.Cdecl,
        SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    private static extern int msync([In] void* dest, [In] nint size, [In] MemorySyncFlags flags);

    [DllImport("libc", EntryPoint = "mmap", CallingConvention = CallingConvention.Cdecl,
        SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    private static extern void* mmap([In] void* dest, [In] nint size, [In] MemoryProtection memoryProtection,
        [In] MemoryMap flags, [In] nint fileDescriptor, [In] nint offset);

    [DllImport("libc", EntryPoint = "mmap", CallingConvention = CallingConvention.Cdecl,
        SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    private static extern int munmap([In] void* dest, [In] nint size);

    [DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
        SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    private static extern void* memcpy([In] void* dest, [In] void* src, [In] ulong count);
}
