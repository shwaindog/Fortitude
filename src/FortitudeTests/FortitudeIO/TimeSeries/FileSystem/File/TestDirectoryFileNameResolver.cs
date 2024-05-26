#region

using FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

public class TestDirectoryFileNameResolver : IDirectoryFileNameResolver
{
    public FileInfo TestTimeSeriesFile { get; set; }
    public DirectoryInfo RootDirPath { get; set; }
    public FileInfo FilePath(CreateFileParameters createFileParameters) => TestTimeSeriesFile;
    public string FileName(CreateFileParameters createFileParameters) => TestTimeSeriesFile.Name;
}
