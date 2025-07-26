// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory;

public unsafe class DisposableVirtualMemoryRange : IVirtualMemoryAddressRange
{
    private readonly int sizeInPages;

    private IOSDirectMemoryApi osDirectMemoryApi;

    public DisposableVirtualMemoryRange(IOSDirectMemoryApi osDirectMemoryApi, byte* startAddress, int sizeInPages)
    {
        this.osDirectMemoryApi = osDirectMemoryApi;
        this.sizeInPages       = sizeInPages;
        StartAddress           = startAddress;
    }

    public void Dispose()
    {
        osDirectMemoryApi.DecommitPageMemory(StartAddress, sizeInPages);
        osDirectMemoryApi.ReleaseReserveMemoryRangeInPages(StartAddress, sizeInPages);
    }

    public void Flush() { }

    public UnmanagedByteArray CreateUnmanagedByteArrayInThisRange(long viewOffset, int length)
    {
        if (viewOffset + length > (nint)EndAddress) throw new ArgumentOutOfRangeException("UnmanagedByteArray can not be mapped onto this range");
        return new UnmanagedByteArray(this, viewOffset, length);
    }

    public byte* StartAddress    { get; }
    public long  Length          => sizeInPages * osDirectMemoryApi.MinimumRequiredPageSize;
    public byte* EndAddress      => StartAddress + Length;
    public long  DefaultGrowSize => Length;

    public IVirtualMemoryAddressRange GrowByDefaultSize() => MemoryUtils.ResizeVirtualMemory(this, Length + DefaultGrowSize);
}
