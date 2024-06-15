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
    public RepositoryInfo(IRepositoryRootDirectory repositoryRoot, RepositoryProximity proximity, string timeSeriesFileExtension)
    {
        Directory               = repositoryRoot;
        Proximity               = proximity;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public IRepositoryRootDirectory Directory;
    public RepositoryProximity      Proximity;

    public string TimeSeriesFileExtension = ".tsf";
}

public enum RepositoryProximity
{
    Local
  , Remote
}

public interface ITimeSeriesRepository
{
    public void CloseAllFilesAndSessions();

    IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, TimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry<TEntry>;

    IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument) where TEntry : ITimeSeriesEntry<TEntry>;
}

public class TimeSeriesDirectoryRepository : ITimeSeriesRepository
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesDirectoryRepository));

    private readonly RepositoryInfo repositoryInfo;

    private IMap<IInstrument, List<InstrumentRepoFile>> instrumentFileDetails = new ConcurrentMap<IInstrument, List<InstrumentRepoFile>>();
    private IMap<IInstrument, List<InstrumentRepoFile>> parsingFileDetails    = new ConcurrentMap<IInstrument, List<InstrumentRepoFile>>();

    protected TimeSeriesDirectoryRepository(RepositoryInfo repositoryInfo)
    {
        this.repositoryInfo = repositoryInfo;
        ScanRepositoryAndUpdate();
        repositoryInfo.Directory.FileAvailabilityChanged += RepositoryFileAvailabilityChanged;
    }

    public IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, TimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var instrumentFiles = instrumentFileDetails[instrument];
        if (instrumentFiles == null) return null;
        if (restrictedRange != null) instrumentFiles = instrumentFiles.Where(irf => irf.FileIntersects(restrictedRange)).ToList();
        return new RepositoryFilesReaderSession<TEntry>(instrumentFiles);
    }

    public IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument) where TEntry : ITimeSeriesEntry<TEntry>
    {
        var fileStructure = repositoryInfo.Directory.FindFileStructure(instrument);
        if (fileStructure == null) return null;
        return new InstrumentRepoWriterSession<TEntry>(instrument, fileStructure);
    }

    public void CloseAllFilesAndSessions()
    {
        foreach (var instrumentRepoFile in instrumentFileDetails.SelectMany(kvp => kvp.Value))
            if (instrumentRepoFile.TimeSeriesRepoFile is { TimeSeriesFile: not null, TimeSeriesFile.IsOpen: true })
                instrumentRepoFile.TimeSeriesRepoFile.TimeSeriesFile.Close();
    }

    private void RepositoryFileAvailabilityChanged(IRepositoryRootDirectory sender, InstrumentRepoFileUpdateEventArgs eventArgs)
    {
        var instrumentRepoFile = eventArgs.InstrumentRepoFile;
        var instrument         = instrumentRepoFile.Instrument;
        if (!instrumentFileDetails.TryGetValue(instrument, out var fileList))
        {
            fileList = new List<InstrumentRepoFile>();
            instrumentFileDetails.Add(instrument, fileList);
        }
        switch (eventArgs.PathUpdateType)
        {
            case PathUpdateType.Available when !fileList!.Contains(instrumentRepoFile):
                fileList.Add(instrumentRepoFile);
                fileList.Sort();
                break;
            case PathUpdateType.Unavailable when fileList!.Contains(instrumentRepoFile):
                fileList.Remove(instrumentRepoFile);
                fileList.Sort();
                break;
        }
    }

    public void ScanRepositoryAndUpdate()
    {
        parsingFileDetails.Clear();

        foreach (var instrumentDetails in
                 repositoryInfo.Directory.RepositoryInstrumentDetails(repositoryInfo.TimeSeriesFileExtension))
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
                {
                    instrumentDetails.FileStructure                     = existingFile.FileStructure;
                    instrumentDetails.TimeSeriesRepoFile.TimeSeriesFile = existingFile.TimeSeriesRepoFile.TimeSeriesFile;
                }
                else
                {
                    Logger.Info("{0} detected new Time Series file {1}", repositoryInfo.Proximity
                              , instrumentDetails.TimeSeriesRepoFile.RepositoryRelativePath);
                }
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
