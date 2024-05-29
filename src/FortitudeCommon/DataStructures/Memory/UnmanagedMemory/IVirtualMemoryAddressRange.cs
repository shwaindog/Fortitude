// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory;

public unsafe interface IVirtualMemoryAddressRange : IDisposable
{
    byte*              StartAddress { get; }
    long               SizeBytes    { get; }
    byte*              EndAddress   { get; }
    UnmanagedByteArray CreateUnmanagedByteArrayInThisView(long fileCursorPosition, int length);
    void               Flush();
}
