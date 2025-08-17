// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory;

public unsafe interface IVirtualMemoryAddressRange : IDisposable, IGrowable<IVirtualMemoryAddressRange>
{
    byte*              StartAddress { get; }
    long               Length       { get; }
    byte*              EndAddress   { get; }
    UnmanagedByteArray CreateUnmanagedByteArrayInThisRange(long fileCursorPosition, int length);
    void               Flush();
}
