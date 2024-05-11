#region

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe class WindowsDirectMemoryApi : IOSDirectMemoryApi
{
    [Flags]
    public enum AllocationType
    {
        Commit = 0x1000
        , Reserve = 0x2000
        , Decommit = 0x4000
        , Release = 0x8000
        , Reset = 0x80000
        , Physical = 0x400000
        , TopDown = 0x100000
        , WriteWatch = 0x200000
        , LargePages = 0x20000000
    }

    [Flags]
    public enum FileMapAccess : uint
    {
        Copy = 0x01
        , Write = 0x02
        , Read = 0x04
        , AllAccess = 0x08
        , Execute = 0x20
    }

    [Flags]
    public enum FreeType
    {
        Decommit = 0x4000
        , Release = 0x8000
    }

    [Flags]
    public enum MemoryMapAllocationType
    {
        None = 0x00
    }

    [Flags]
    public enum MemoryProtection
    {
        Execute = 0x10
        , ExecuteRead = 0x20
        , ExecuteReadWrite = 0x40
        , ExecuteWriteCopy = 0x80
        , NoAccess = 0x01
        , ReadOnly = 0x02
        , ReadWrite = 0x04
        , WriteCopy = 0x08
        , GuardModifierflag = 0x100
        , NoCacheModifierflag = 0x200
        , WriteCombineModifierflag = 0x400
    }

    private const nint MinimumPageSize = ushort.MaxValue + 1;
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WindowsDirectMemoryApi));

    public WindowsDirectMemoryApi()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("This class should only be instantiated on .NET on Windows");
    }

    public nint MinimumRequiredPageSize => MinimumPageSize;

    public void* ReserveMemoryRangeInPages(void* preferredAddress, int numberOfPages)
    {
        var size = (nint)(numberOfPages * MinimumRequiredPageSize);
        return (byte*)VirtualAlloc(preferredAddress, size, AllocationType.Reserve, MemoryProtection.ReadWrite).ToPointer();
    }

    public void* CommitPageMemory(void* preferredAddress, int numberOfPages)
    {
        var size = (nint)(numberOfPages * MinimumRequiredPageSize);
        return VirtualAlloc(preferredAddress, size, AllocationType.Commit, MemoryProtection.ReadWrite).ToPointer();
    }

    public bool DecommitPageMemory(void* previouslyCommittedAddress, int numberOfPages)
    {
        var size = numberOfPages * MinimumRequiredPageSize;
        return VirtualFreeEx(Process.GetCurrentProcess().Handle, previouslyCommittedAddress, size, FreeType.Decommit);
    }

    public bool ReleaseReserveMemoryRangeInPages(void* previouslyReservedAddress, int numberOfPages) =>
        VirtualFreeEx(Process.GetCurrentProcess().Handle, previouslyReservedAddress, 0, FreeType.Release);

    void* IOSDirectMemoryApi.memcpy(void* dest, void* src, ulong count) => memcpy(dest, src, count);

    public void* MapPageViewOfFile(nint memoryMappedFileHandle, int filePageNumber, int numberOfPages, void* desiredAddress)
    {
        var fromFileOffset = (long)filePageNumber * MinimumRequiredPageSize;
        var size = (uint)(numberOfPages * MinimumRequiredPageSize);

        var fileOffSetUpperBits = (uint)(fromFileOffset >> 32);
        var fileOffSetLowerBits = (uint)(0xFFFF_FFFF & fromFileOffset);
        var mapAtAddress = (nint)desiredAddress;
        // Logger.Info("Attempting to map view from {0} to {1} at address {2:X}", fromFileOffset, fromFileOffset + size, mapAtAddress);

        return MapViewOfFileEx(memoryMappedFileHandle, FileMapAccess.Write | FileMapAccess.Read,
            fileOffSetUpperBits, fileOffSetLowerBits, size, mapAtAddress);
    }

    public bool ReleaseViewOfFile(void* addressToRelease, int numberOfPages)
    {
        var releaseAt = (nint)addressToRelease;
        // Logger.Info("Releasing view of mapped file at {0:X}", releaseAt);
        var returnVal = UnmapViewOfFile(releaseAt);

        var memMapFileError = Marshal.GetLastPInvokeError();
        if (memMapFileError != 0) { }

        return returnVal;
    }

    public bool FlushPageDataToDisk(void* flushBase, nint bytesToFlush) => FlushViewOfFile(flushBase, bytesToFlush);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint VirtualAlloc(void* lpAddress, nint dwSize, AllocationType lAllocationType, MemoryProtection flProtect);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool VirtualFreeEx(nint hProcess, void* lpAddress, nint dwSize, FreeType dwFreeType);

    [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
        SetLastError = false)]
    [SuppressUnmanagedCodeSecurity]
    private static extern void* memcpy([In] void* dest, [In] void* src, [In] ulong count);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern byte* MapViewOfFileEx(nint mappingHandle,
        FileMapAccess memoryAccess,
        uint offsetUpperFileCursor,
        uint offsetLowerFileCursor,
        UIntPtr size,
        IntPtr desiredAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool UnmapViewOfFile(IntPtr address);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlushViewOfFile(void* address, nint bytesToFlush);
}
