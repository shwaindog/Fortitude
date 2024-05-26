#region

using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Header;

public interface IFileSubHeader
{
    void CloseFileView();
    bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None);
}
