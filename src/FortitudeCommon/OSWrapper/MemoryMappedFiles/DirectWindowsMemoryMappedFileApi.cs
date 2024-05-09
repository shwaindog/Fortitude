#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.OSWrapper.MemoryMappedFiles;

[Flags]
public enum FileMapAccess : uint
{
    Copy = 0x01
    , Write = 0x02
    , Read = 0x04
    , AllAccess = 0x08
    , Execute = 0x20
}

public unsafe interface IOsMemoryMappedFileApi
{
    byte* MapPageViewOfFile(IntPtr memoryMappedFileHandle, FileMapAccess access, int filePageNumber, int numberOfPages, byte* desiredAddress);
    bool ReleaseViewOfFile(byte* addressToRelease);
    bool FlushPageDataToDisk(byte* startAddressToFlush, nint bytesToFlush);
}

public unsafe class DirectWindowsMemoryMappedFileApi : IOsMemoryMappedFileApi
{
    private static readonly uint SystemPageFileSize = (uint)Environment.SystemPageSize;

    public byte* MapPageViewOfFile(IntPtr memoryMappedFileHandle, FileMapAccess access, int filePageNumber, int numberOfPages, byte* desiredAddress)
    {
        var fromFileOffset = (uint)filePageNumber * SystemPageFileSize;
        var toFileOffset = (uint)(filePageNumber + numberOfPages) * SystemPageFileSize;

        return MapViewOfFileEx(memoryMappedFileHandle, access, toFileOffset, fromFileOffset, SystemPageFileSize, desiredAddress);
    }

    public bool ReleaseViewOfFile(byte* addressToRelease) => UnmapViewOfFile(addressToRelease);

    public bool FlushPageDataToDisk(byte* flushBase, nint bytesToFlush) => FlushViewOfFile(flushBase, bytesToFlush);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern byte* MapViewOfFileEx(nint mappingHandle,
        FileMapAccess access,
        uint offsetHigh,
        uint offsetLow,
        UIntPtr bytesToMap,
        byte* desiredAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool UnmapViewOfFile(byte* address);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlushViewOfFile(byte* address, IntPtr bytesToFlush);
}
