// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers.UnmanagedMemory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.DataStructures.MemoryPools.Buffers;

public static class MemoryUtils
{
    public const int LargeObjectHeapAllocationSize = 85_000;

    public const int CacheLineSize                 = 128;

    public static IOSDirectMemoryApi OsDirectMemoryAccess { get; set; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsDirectMemoryApi() : new LinuxDirectMemoryApi();

    public static int CeilingNextPowerOfTwo(int value)
    {
        var ceiling = 2;

        while (ceiling < value) ceiling *= 2;

        return ceiling;
    }

    public static bool IsPowerOfTwo(int value)
    {
        var ceiling = 2;

        while (ceiling < value) ceiling *= 2;

        return ceiling == value;
    }

    public static unsafe void* MemCpy(void* dest, void* src, long count) => OsDirectMemoryAccess.memcpy(dest, src, (ulong)count);

    public static unsafe DisposableVirtualMemoryRange AllocVirtualMemory(long size)
    {
        var osDirectMemoryApi = OsDirectMemoryAccess;
        var numberOfPages     = Math.Max(1, (int)(size / osDirectMemoryApi.MinimumRequiredPageSize + 1));
        var addressStart      = osDirectMemoryApi.ReserveMemoryRangeInPages(null, numberOfPages);
        osDirectMemoryApi.CommitPageMemory(addressStart, numberOfPages);
        return new DisposableVirtualMemoryRange(osDirectMemoryApi, (byte*)addressStart, numberOfPages);
    }

    public static unsafe DisposableVirtualMemoryRange ResizeVirtualMemory(DisposableVirtualMemoryRange existing, long newSize)
    {
        var newVirtualMemoryRange = AllocVirtualMemory(newSize);
        var cappedCopySize        = Math.Min(existing.Length, newVirtualMemoryRange.Length);
        MemCpy(newVirtualMemoryRange.StartAddress, existing.StartAddress, cappedCopySize);
        return newVirtualMemoryRange;
    }

    public static UnmanagedByteArray CreateUnmanagedByteArray(long size)
    {
        var virtualMemoryRange = AllocVirtualMemory(size);
        return new UnmanagedByteArray(virtualMemoryRange, 0, size, true);
    }

    public static ByteArrayMemoryStream CreateByteArrayMemoryStream(long size, bool writable = true, bool closeByteArrayOnDispose = true) =>
        new(CreateUnmanagedByteArray(size), writable, closeByteArrayOnDispose);
}
