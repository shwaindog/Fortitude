// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.File.Header;

public interface IFileSubHeader
{
    void CloseFileView();
    bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None);
}
