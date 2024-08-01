// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.Session;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public struct RepositoryInfo
{
    public RepositoryInfo
    (IRepositoryRootDirectory repoRootDir, IFileRepositoryLocationConfig repositoryLocationConfig
      , string[] requiredInstrumentFields, string[]? optionalInstrumentFields)
    {
        RepoRootDir              = repoRootDir;
        Proximity                = repositoryLocationConfig.Proximity;
        TimeSeriesFileExtension  = repositoryLocationConfig.TimeSeriesFileExtension;
        RequiredInstrumentFields = requiredInstrumentFields;
        OptionalInstrumentFields = optionalInstrumentFields;
    }

    public RepositoryInfo
    (IRepositoryRootDirectory repoRootDir, RepositoryProximity proximity, string fileExtension
      , string[] requiredInstrumentFields, string[]? optionalInstrumentFields)
    {
        RepoRootDir              = repoRootDir;
        Proximity                = proximity;
        TimeSeriesFileExtension  = fileExtension;
        RequiredInstrumentFields = requiredInstrumentFields;
        OptionalInstrumentFields = optionalInstrumentFields;
    }

    public IRepositoryRootDirectory RepoRootDir;
    public RepositoryProximity      Proximity;

    public string[]  RequiredInstrumentFields;
    public string[]? OptionalInstrumentFields;

    public string TimeSeriesFileExtension = ".tsf";
}

public interface ITimeSeriesRepository
{
    RepositoryProximity Proximity { get; }

    IRepositoryRootDirectory RepoRootDirectory { get; }

    IMap<IInstrument, InstrumentRepoFileSet> InstrumentFilesMap { get; }

    string[]  RequiredInstrumentFields { get; }
    string[]? OptionalInstrumentFields { get; }

    void CloseAllFilesAndSessions();

    InstrumentFileInfo      GetInstrumentFileInfo(IInstrument instrument);
    InstrumentFileEntryInfo GetInstrumentFileEntryInfo(IInstrument instrument, UnboundedTimeRange? limitEntryRange = null);

    IEnumerable<IInstrument> AvailableInstrumentsMatching
    (string instrumentName, string instrumentSource, InstrumentType? instrumentType = null, DiscreetTimePeriod? coveringPeriod = null
      , IEnumerable<KeyValuePair<string, string>>? attributes = null);

    IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, UnboundedTimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry;

    IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument) where TEntry : ITimeSeriesEntry;
}

public abstract class TimeSeriesDirectoryRepositoryBase : ITimeSeriesRepository
{
    protected readonly RepositoryInfo RepositoryInfo;

    protected TimeSeriesDirectoryRepositoryBase(RepositoryInfo repositoryInfo) => RepositoryInfo = repositoryInfo;

    public RepositoryProximity Proximity => RepositoryInfo.Proximity;

    public abstract IMap<IInstrument, InstrumentRepoFileSet> InstrumentFilesMap { get; protected set; }

    public IRepositoryRootDirectory RepoRootDirectory => RepositoryInfo.RepoRootDir;

    public string[]  RequiredInstrumentFields => RepositoryInfo.RequiredInstrumentFields;
    public string[]? OptionalInstrumentFields => RepositoryInfo.OptionalInstrumentFields;

    public abstract IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, UnboundedTimeRange? restrictedRange = null)
        where TEntry : ITimeSeriesEntry;

    public abstract IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument) where TEntry : ITimeSeriesEntry;

    public void CloseAllFilesAndSessions()
    {
        foreach (var instrumentRepoFile in InstrumentFilesMap.SelectMany(kvp => kvp.Value))
            if (instrumentRepoFile.TimeSeriesRepoFile is { TimeSeriesFile: not null, TimeSeriesFile.IsOpen: true })
                instrumentRepoFile.TimeSeriesRepoFile.TimeSeriesFile.Close();
    }

    public InstrumentFileInfo GetInstrumentFileInfo(IInstrument instrument)
    {
        if (!InstrumentFilesMap.TryGetValue(instrument, out var instrumentFiles))
        {
            var fileStructure       = RepositoryInfo.RepoRootDir.FindFileStructure(instrument);
            var fileStructurePeriod = fileStructure?.PathTimeBoundaryPeriod ?? TimeBoundaryPeriod.None;
            return new InstrumentFileInfo(instrument, fileStructurePeriod);
        }
        if (instrumentFiles == null || !instrumentFiles.Any())
        {
            var fileStructure       = RepositoryInfo.RepoRootDir.FindFileStructure(instrument);
            var fileStructurePeriod = fileStructure?.PathTimeBoundaryPeriod ?? TimeBoundaryPeriod.None;
            return new InstrumentFileInfo(instrument, fileStructurePeriod);
        }
        var earliestFile           = instrumentFiles.First();
        var earliestTimeSeriesFile = earliestFile.TimeSeriesFile;
        var earliestEntryTime      = earliestTimeSeriesFile.Header.EarliestEntryTime;
        var latestFile             = instrumentFiles.Last();
        var latestTimeSeriesFile   = latestFile.TimeSeriesFile;
        var latestEntryTime        = latestTimeSeriesFile.Header.LatestEntryTime;
        var allFileStartTimes      = instrumentFiles.Select(irf => irf.FilePeriodRange.PeriodStartTime).ToList();
        var filePeriod             = earliestFile.FilePeriodRange.TimeBoundaryPeriod;
        return new InstrumentFileInfo(instrument, filePeriod, earliestEntryTime, latestEntryTime, allFileStartTimes);
    }

    public InstrumentFileEntryInfo GetInstrumentFileEntryInfo(IInstrument instrument, UnboundedTimeRange? limitEntryRange = null)
    {
        if (!InstrumentFilesMap.TryGetValue(instrument, out var instrumentFiles))
        {
            var fileStructure       = RepositoryInfo.RepoRootDir.FindFileStructure(instrument);
            var fileStructurePeriod = fileStructure?.PathTimeBoundaryPeriod ?? TimeBoundaryPeriod.None;
            return new InstrumentFileEntryInfo(instrument, fileStructurePeriod);
        }
        if (instrumentFiles == null || !instrumentFiles.Any())
        {
            var fileStructure       = RepositoryInfo.RepoRootDir.FindFileStructure(instrument);
            var fileStructurePeriod = fileStructure?.PathTimeBoundaryPeriod ?? TimeBoundaryPeriod.None;
            return new InstrumentFileEntryInfo(instrument, fileStructurePeriod);
        }
        var totalCount  = 0L;
        var fileDetails = new List<FileEntryInfo>(instrumentFiles.Count);
        foreach (var instrumentFile in instrumentFiles.Where(irfs => irfs.FilePeriodRange.Intersects(limitEntryRange)))
        {
            var timeSeriesFile = instrumentFile.TimeSeriesFile;
            var fileEntries    = timeSeriesFile.Header.TotalEntries;
            fileDetails.Add(new FileEntryInfo(instrumentFile.FilePeriodRange.PeriodStartTime, instrumentFile.TimeSeriesRepoFile.Proximity
                                            , fileEntries));
            totalCount += fileEntries;
        }
        var filePeriod = instrumentFiles[0].FilePeriodRange.TimeBoundaryPeriod;
        return new InstrumentFileEntryInfo(instrument, filePeriod, fileDetails, totalCount);
    }

    public IEnumerable<IInstrument> AvailableInstrumentsMatching
    (string instrumentName, string instrumentSource, InstrumentType? instrumentType = null, DiscreetTimePeriod? coveringPeriod = null
      , IEnumerable<KeyValuePair<string, string>>? attributes = null)
    {
        foreach (var instrument in InstrumentFilesMap.Keys)
        {
            var isMatch = instrument.InstrumentName == instrumentName && instrument.InstrumentSource == instrumentSource;
            if (!isMatch) continue;
            if (instrumentType != null && instrumentType != instrument.InstrumentType) continue;
            if (coveringPeriod != null && coveringPeriod != instrument.CoveringPeriod) continue;
            if (attributes != null && attributes.Any(kvp => instrument[kvp.Key] != kvp.Value)) continue;
            yield return instrument;
        }
    }
}

public class TimeSeriesDirectoryRepository : TimeSeriesDirectoryRepositoryBase, ITimeSeriesRepository
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesDirectoryRepository));

    private IMap<IInstrument, InstrumentRepoFileSet> parsingFileDetails = new ConcurrentMap<IInstrument, InstrumentRepoFileSet>();

    protected TimeSeriesDirectoryRepository(ISingleRepositoryBuilderConfig singleRepositoryBuilderConfig, string repositoryName)
        : base(singleRepositoryBuilderConfig.BuildRepositoryInfo(repositoryName))
    {
        RepositoryInfo.RepoRootDir.Repository = this;

        ScanRepositoryAndUpdate();
        RepositoryInfo.RepoRootDir.FileAvailabilityChanged += RepositoryFileAvailabilityChanged;
    }

    protected TimeSeriesDirectoryRepository(RepositoryInfo repositoryInfo) : base(repositoryInfo)
    {
        RepositoryInfo.RepoRootDir.Repository = this;

        ScanRepositoryAndUpdate();
        RepositoryInfo.RepoRootDir.FileAvailabilityChanged += RepositoryFileAvailabilityChanged;
    }

    public override IWriterSession<TEntry>? GetWriterSession<TEntry>(IInstrument instrument)
    {
        var fileStructure = RepositoryInfo.RepoRootDir.FindFileStructure(instrument);
        if (fileStructure == null) return null;
        return new InstrumentRepoWriterSession<TEntry>(instrument, fileStructure);
    }

    public override IMap<IInstrument, InstrumentRepoFileSet> InstrumentFilesMap { get; protected set; }
        = new ConcurrentMap<IInstrument, InstrumentRepoFileSet>();

    public override IReaderSession<TEntry>? GetReaderSession<TEntry>(IInstrument instrument, UnboundedTimeRange? restrictedRange = null)
    {
        var instrumentFiles = InstrumentFilesMap[instrument];
        if (instrumentFiles == null) return null;
        if (restrictedRange != null) instrumentFiles = instrumentFiles.Where(irf => irf.FileIntersects(restrictedRange)).ToInstrumentRepoFileSet();
        return new RepositoryFilesReaderSession<TEntry>(instrumentFiles);
    }

    private void RepositoryFileAvailabilityChanged(IRepositoryRootDirectory sender, InstrumentRepoFileUpdateEventArgs eventArgs)
    {
        var instrumentRepoFile = eventArgs.InstrumentRepoFile;
        var instrument         = instrumentRepoFile.Instrument;
        if (!InstrumentFilesMap.TryGetValue(instrument, out var fileList))
        {
            fileList = new InstrumentRepoFileSet();
            InstrumentFilesMap.Add(instrument, fileList);
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
                 RepositoryInfo.RepoRootDir.RepositoryInstrumentDetails(RepositoryInfo.TimeSeriesFileExtension))
        {
            if (!parsingFileDetails.TryGetValue(instrumentDetails.Instrument, out var fileList))
            {
                fileList = new InstrumentRepoFileSet();
                parsingFileDetails.Add(instrumentDetails.Instrument, fileList);
            }
            fileList!.Add(instrumentDetails);

            if (InstrumentFilesMap.TryGetValue(instrumentDetails.Instrument, out var existingFileList))
            {
                var existingFile = existingFileList!.FirstOrDefault(ifd => Equals(ifd, instrumentDetails));
                if (existingFile != null)
                    instrumentDetails.TimeSeriesRepoFile.TimeSeriesFile = existingFile.TimeSeriesRepoFile.TimeSeriesFile;
                else
                    Logger.Info("{0} detected new Time Series file {1}", RepositoryInfo.Proximity
                              , instrumentDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
            else
            {
                Logger.Info("{0} detected new Time Series file {1}", RepositoryInfo.Proximity
                          , instrumentDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
        }
        foreach (var existingFileDetails in InstrumentFilesMap.Values.SelectMany(ifdl => ifdl))
            if (parsingFileDetails.TryGetValue(existingFileDetails.Instrument, out var existingFileList))
            {
                var justParsed = existingFileList!.FirstOrDefault(ifd => Equals(ifd, existingFileDetails));
                if (justParsed == null)
                    Logger.Warn("{0} removed Time Series file {1} ", RepositoryInfo.Proximity
                              , existingFileDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
            else
            {
                Logger.Warn("{0} removed Time Series file {1}", RepositoryInfo.Proximity
                          , existingFileDetails.TimeSeriesRepoFile.RepositoryRelativePath);
            }
        (InstrumentFilesMap, parsingFileDetails) = (parsingFileDetails, InstrumentFilesMap);
    }
}
