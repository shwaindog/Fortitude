#region

using System.Diagnostics;
using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public unsafe interface IOSMemoryAllocation
{
    byte* ReserveMemoryRangeInPages(nint? preferredAddress, int numberOfPages);
    byte* CommitPageMemory(nint? preferredAddress, int numberOfPages);
    bool DecommitPageMemory(nint preferredAddress, int numberOfPages);
    bool ReleaseReserveMemoryRangeInPages(byte* previouslyReservedAddress, int numberOfPages);
}

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

[Flags]
public enum FreeType
{
    Decommit = 0x4000
    , Release = 0x8000
}

public unsafe class DirectWindowsOSMemoryAllocation : IOSMemoryAllocation
{
    private static readonly int PageSize = Environment.SystemPageSize;

    public unsafe byte* ReserveMemoryRangeInPages(IntPtr? preferredAddress, int numberOfPages)
    {
        var size = (uint)(numberOfPages * PageSize);
        return (byte*)VirtualAlloc(preferredAddress, size, AllocationType.Reserve, MemoryProtection.ReadWrite).ToPointer();
    }

    public unsafe byte* CommitPageMemory(IntPtr? preferredAddress, int numberOfPages)
    {
        var size = (uint)(numberOfPages * PageSize);
        return (byte*)VirtualAlloc(preferredAddress, size, AllocationType.Commit, MemoryProtection.ReadWrite).ToPointer();
    }

    public bool DecommitPageMemory(IntPtr preferredAddress, int numberOfPages)
    {
        var size = numberOfPages * PageSize;
        return VirtualFreeEx(Process.GetCurrentProcess().Handle, preferredAddress, size, FreeType.Decommit);
    }

    public bool ReleaseReserveMemoryRangeInPages(byte* previouslyReservedAddress, int numberOfPages)
    {
        var size = numberOfPages * PageSize;
        return VirtualFreeEx(Process.GetCurrentProcess().Handle, previouslyReservedAddress, size, FreeType.Release);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAlloc(IntPtr? lpAddress, UIntPtr dwSize, AllocationType lAllocationType, MemoryProtection flProtect);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, FreeType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern unsafe bool VirtualFreeEx(
        IntPtr hProcess, byte* pAddress,
        int size, FreeType freeType);
}
