#region

using FortitudeIO.TimeSeries.FileSystem.File.Header;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public enum TimeSeriesFileStatus
{
    Unknown
    , New
    , CompleteHealthy
    , IncompleteAppending
    , WriterInterrupted
    , Corrupt
}

public interface ITimerSeriesFile
{
    ushort FileVersion { get; }
    ITimeSeriesFileHeader Header { get; }
    TimeSeriesPeriod FileTimeSeriesPeriod { get; }
    string FileName { get; }
    bool IsWritable { get; }
}

public class TimeSeriesFile { }
