// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public static class MemoryUtils
{
    public const int CacheLineSize = 128;

    public static IOSDirectMemoryApi OsDirectMemoryAccess { get; set; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsDirectMemoryApi() : new LinuxDirectMemoryApi();

    public static int CeilingNextPowerOfTwo(int value)
    {
        var ceiling = 2;

        while (ceiling < value) ceiling *= 2;

        return ceiling;
    }

    public static unsafe void* MemCpy(void* dest, void* src, ulong count) => OsDirectMemoryAccess.memcpy(dest, src, count);

    public static unsafe IVirtualMemoryAddressRange AllocVirtualMemory(long size)
    {
        var osDirectMemoryApi = OsDirectMemoryAccess;
        var numberOfPages     = Math.Max(1, (int)(size / osDirectMemoryApi.MinimumRequiredPageSize + 1));
        var addressStart      = osDirectMemoryApi.ReserveMemoryRangeInPages(null, numberOfPages);
        osDirectMemoryApi.CommitPageMemory(addressStart, numberOfPages);
        return new DisposableVirtualMemoryRange(osDirectMemoryApi, (byte*)addressStart, numberOfPages);
    }

    public static IByteArray CreateUnmanagedByteArray(long size)
    {
        var virtualMemoryRange = AllocVirtualMemory(size);
        return new UnmanagedByteArray(virtualMemoryRange, 0, size);
    }

    public static ByteArrayMemoryStream CreateByteArrayMemoryStream(long size, bool writable = true, bool closeByteArrayOnDispose = true) =>
        new(CreateUnmanagedByteArray(size), writable, closeByteArrayOnDispose);
}
