#region

using FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

public class TestDirectoryFileNameResolver : IDirectoryFileNameResolver
{
    public FileInfo TestTimeSeriesFile { get; set; } = null!;
    public DirectoryInfo RootDirPath { get; set; } = null!;
    public FileInfo FilePath(CreateFileParameters createFileParameters) => TestTimeSeriesFile;
    public string FileName(CreateFileParameters createFileParameters) => TestTimeSeriesFile.Name;
}
