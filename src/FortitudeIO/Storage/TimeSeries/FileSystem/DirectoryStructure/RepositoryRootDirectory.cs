// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;

public delegate void RepositoryInstrumentFileAvailabilityChanged(IRepositoryRootDirectory sender, InstrumentRepoFileUpdateEventArgs eventArgs);

public interface IRepositoryRootDirectory : IPathDirectory
{
    DirectoryInfo DirInfo        { get; set; }
    string?       RepositoryName { get; set; }
    string        DirPath        { get; }

    ITimeSeriesRepository Repository { get; set; }

    event RepositoryInstrumentFileAvailabilityChanged FileAvailabilityChanged;

    void OnFileInstrumentUpdated(RepositoryInstrumentFileUpdateEvent updateEvent);

    IEnumerable<InstrumentRepoFile> RepositoryInstrumentDetails(string extension);

    InstrumentRepoFile? GetInstrumentFileDetails(FileInfo timeSeriesFile);

    IPathFile? FindFileStructure(IInstrument instrument);
}

public class RepositoryRootDirectory : PathDirectory, IRepositoryRootDirectory
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(RepositoryRootDirectory));

    private DirectoryInfo rootDirectoryInfo;

    public RepositoryRootDirectory(string? repositoryName, DirectoryInfo rootDirectoryInfo) : base(Array.Empty<PathName>())
    {
        this.rootDirectoryInfo = rootDirectoryInfo;
        RepositoryName         = repositoryName;
    }

    public override IPathDirectory? ParentDirectory
    {
        get => null;
        set { }
    }

    public string? RepositoryName { get; set; }

    public string DirPath => rootDirectoryInfo.FullName;

    public event RepositoryInstrumentFileAvailabilityChanged? FileAvailabilityChanged;

    public DirectoryInfo DirInfo
    {
        get => rootDirectoryInfo;
        set => rootDirectoryInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ITimeSeriesRepository Repository { get; set; } = null!;

    public override TimeBoundaryPeriod PathTimeBoundaryPeriod => TimeBoundaryPeriod.OneDecade;

    public override string FullPath(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forPeriod) => rootDirectoryInfo.FullName;


    public IEnumerable<InstrumentRepoFile> RepositoryInstrumentDetails(string extension) =>
        DirInfo
            .RecursiveFindFiles("*" + extension)
            .Select(GetInstrumentFileDetails)
            .OfType<InstrumentRepoFile>();

    public virtual InstrumentRepoFile? GetInstrumentFileDetails(FileInfo timeSeriesFile)
    {
        var structureMatch = PathMatch(timeSeriesFile);
        if (structureMatch is { HasFilePeriodRange: true, HasInstrument: true, TimeSeriesFile: not null })
            return new InstrumentRepoFile
                (structureMatch.Instrument,
                 new TimeSeriesRepoFile(structureMatch.SearchFile, this, structureMatch.TimeSeriesFile) { }, structureMatch.FilePeriodRange);
        Logger.Warn("Could not resolve file details for {0} without opening the file.  This file will not be searchable." +
                    "  Todo open file and read details and move file to expected location", timeSeriesFile.FullName);
        return null;
    }

    public void OnFileInstrumentUpdated(RepositoryInstrumentFileUpdateEvent updateEvent)
    {
        var instrumentRepoFile =
            new InstrumentRepoFile
                (updateEvent.Instrument,
                 new TimeSeriesRepoFile(updateEvent.File, this, updateEvent.PathFile)
                 {
                     TimeSeriesFile = updateEvent.TimeSeriesFile
                 }, updateEvent.FilePeriodRange);
        FileAvailabilityChanged?.Invoke(this, new InstrumentRepoFileUpdateEventArgs(instrumentRepoFile, updateEvent.PathUpdateType));
    }

    public IPathFile? FindFileStructure(IInstrument instrument)
    {
        var structureMatch = new PathInstrumentMatch(instrument);
        foreach (var timeSeriesStructure in Children)
        {
            timeSeriesStructure.InstrumentMatch(structureMatch);
            if (structureMatch.HasTimeSeriesFileStructureMatch) break;
        }
        return structureMatch.TimeSeriesFile;
    }

    public override string PathName(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forPeriod) => "";

    public PathFileMatch PathMatch(FileInfo fileInRepo)
    {
        if (fileInRepo.FullName.Contains(rootDirectoryInfo.FullName)) throw new Exception("File is not in the Repository");
        var dirSplit = fileInRepo.FullName.Replace(rootDirectoryInfo.FullName + Path.DirectorySeparatorChar, "").Split(Path.DirectorySeparatorChar);
        if (dirSplit.Length == 1) throw new Exception("File is not contained within a date defining directory");
        var dateChildDir =
            Children
                .OfType<IPathDirectory>()
                .FirstOrDefault(cd => cd.NameParts
                                        .Any(np => np.PathPart is RepositoryPathName.Year or RepositoryPathName.Decade));
        if (dateChildDir != null)
        {
            var baseDate = dateChildDir.NameParts.FirstOrDefault(np => np.ExtractDate(dirSplit[0]) != null)?.ExtractDate(dirSplit[0]);
            if (baseDate != null)
            {
                var search = new PathFileMatch(fileInRepo, Repository.RequiredInstrumentFields, this, Repository.OptionalInstrumentFields
                                             , baseDate.Value)
                {
                    RemainingPath = dirSplit.ToList()
                };
                return dateChildDir.PathMatch(search);
            }
        }
        else
        {
            foreach (var childItem in Children)
                if (childItem.MatchesFormat(dirSplit[0]))
                {
                    var search = new PathFileMatch(fileInRepo, Repository.RequiredInstrumentFields, this, Repository.OptionalInstrumentFields)
                    {
                        RemainingPath = dirSplit.ToList()
                    };
                    return childItem.PathMatch(search);
                }
        }
        throw new Exception("File is not contained within a year defining directory");
    }
}
