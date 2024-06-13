// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.Session;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public struct RepositoryInfo
{
    public RepositoryInfo(RepositoryRootDirectoryStructure repositoryRoot, RepositoryProximity proximity)
    {
        DirectoryStructureStructure = repositoryRoot;
        Proximity                   = proximity;
    }

    public RepositoryRootDirectoryStructure DirectoryStructureStructure;
    public RepositoryProximity              Proximity;

    public string TimeSeriesFileExtension = "tsf";
}

public enum RepositoryProximity
{
    Local
  , Remote
}

public interface ITimeSeriesRepository
{
    IReaderSession<TEntry>? GetReaderSession<TEntry>(Instrument instrument, TimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry<TEntry>;

    IWriterSession<TEntry>? GetWriterSession<TEntry>(Instrument instrument) where TEntry : ITimeSeriesEntry<TEntry>;
}

public class TimeSeriesDirectoryRepository : ITimeSeriesRepository
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesDirectoryRepository));

    private readonly RepositoryInfo repositoryInfo;

    private IMap<Instrument, List<InstrumentRepoFile>> instrumentFileDetails = new ConcurrentMap<Instrument, List<InstrumentRepoFile>>();
    private IMap<Instrument, List<InstrumentRepoFile>> parsingFileDetails    = new ConcurrentMap<Instrument, List<InstrumentRepoFile>>();

    protected TimeSeriesDirectoryRepository(RepositoryInfo repositoryInfo)
    {
        this.repositoryInfo = repositoryInfo;
        ScanRepositoryAndUpdate();
    }

    public IReaderSession<TEntry>? GetReaderSession<TEntry>(Instrument instrument, TimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var instrumentFiles = instrumentFileDetails[instrument];
        if (instrumentFiles == null) return null;
        if (restrictedRange != null) instrumentFiles = instrumentFiles.Where(irf => irf.FileIntersects(restrictedRange)).ToList();
        return new RepositoryFilesReaderSession<TEntry>(instrumentFiles);
    }

    public IWriterSession<TEntry>? GetWriterSession<TEntry>(Instrument instrument) where TEntry : ITimeSeriesEntry<TEntry>
    {
        var fileStructure = repositoryInfo.DirectoryStructureStructure.FindFileStructure(instrument);
        if (fileStructure == null) return null;
        return new InstrumentRepoWriterSession<TEntry>(instrument, fileStructure);
    }

    public void ScanRepositoryAndUpdate()
    {
        parsingFileDetails.Clear();

        foreach (var instrumentDetails in
                 repositoryInfo.DirectoryStructureStructure.RepositoryInstrumentDetails(repositoryInfo.TimeSeriesFileExtension))
        {
            if (!parsingFileDetails.TryGetValue(instrumentDetails.Instrument, out var fileList))
            {
                fileList = new List<InstrumentRepoFile>();
                parsingFileDetails.Add(instrumentDetails.Instrument, fileList);
            }
            fileList!.Add(instrumentDetails);

            if (instrumentFileDetails.TryGetValue(instrumentDetails.Instrument, out var existingFileList))
            {
                var existingFile = existingFileList!.FirstOrDefault(ifd => Equals(ifd, instrumentDetails));
                if (existingFile != null)
                    instrumentDetails.FileStructure = existingFile.FileStructure;
                else
                    Logger.Info("{0} detected new Time Series file {1}", repositoryInfo.Proximity
                              , instrumentDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
            else
            {
                Logger.Info("{0} detected new Time Series file {1}", repositoryInfo.Proximity
                          , instrumentDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
        }
        foreach (var existingFileDetails in instrumentFileDetails.Values.SelectMany(ifdl => ifdl))
            if (parsingFileDetails.TryGetValue(existingFileDetails.Instrument, out var existingFileList))
            {
                var justParsed = existingFileList!.FirstOrDefault(ifd => Equals(ifd, existingFileDetails));
                if (justParsed == null)
                    Logger.Warn("{0} removed Time Series file {1} ", repositoryInfo.Proximity
                              , existingFileDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
            else
            {
                Logger.Warn("{0} removed Time Series file {1}", repositoryInfo.Proximity
                          , existingFileDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
        (instrumentFileDetails, parsingFileDetails) = (parsingFileDetails, instrumentFileDetails);
    }
}
