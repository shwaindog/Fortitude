// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public class RepositoryRootDirectoryStructure : TimeSeriesDirectoryStructure
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(RepositoryRootDirectoryStructure));

    private DirectoryInfo rootDirectoryInfo;

    public RepositoryRootDirectoryStructure(string repositoryName, DirectoryInfo rootDirectoryInfo) : base(Array.Empty<TimeSeriesPathNameFormat>())
    {
        this.rootDirectoryInfo = rootDirectoryInfo;
        RepositoryName         = repositoryName;
    }

    public override ITimeSeriesDirectoryStructure? ParentDirectory
    {
        get => null;
        set { }
    }

    public string RepositoryName { get; set; }

    public DirectoryInfo RootDirectoryInfo
    {
        get => rootDirectoryInfo;
        set => rootDirectoryInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override TimeSeriesPeriod PathTimeSeriesPeriod => TimeSeriesPeriod.OneDecade;

    public override string PathName(Instrument instrument, DateTime timeInPeriod) => "";

    public override string FullPath(Instrument instrument, DateTime timeInPeriod) => rootDirectoryInfo.FullName;

    public TimeSeriesFileStructureMatch PathMatch(FileInfo fileInRepo)
    {
        if (fileInRepo.FullName.Contains(rootDirectoryInfo.FullName)) throw new Exception("File is not in the Repository");
        var dirSplit = fileInRepo.FullName.Replace(rootDirectoryInfo.FullName + Path.DirectorySeparatorChar, "").Split(Path.DirectorySeparatorChar);
        if (dirSplit.Length == 1) throw new Exception("File is not contained within a date defining directory");
        var dateChildDir =
            Children
                .OfType<ITimeSeriesDirectoryStructure>()
                .FirstOrDefault(cd => cd.NameParts
                                        .Any(np => np.PathPart is TimeSeriesPathNameComponent.Year or TimeSeriesPathNameComponent.Decade));
        if (dateChildDir != null)
        {
            var baseDate = dateChildDir.NameParts.FirstOrDefault(np => np.ExtractDate(dirSplit[0]) != null)?.ExtractDate(dirSplit[0]);
            if (baseDate != null)
            {
                var search = new TimeSeriesFileStructureMatch(fileInRepo, this, baseDate.Value)
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
                    var search = new TimeSeriesFileStructureMatch(fileInRepo, this)
                    {
                        RemainingPath = dirSplit.ToList()
                    };
                    return childItem.PathMatch(search);
                }
        }
        throw new Exception("File is not contained within a year defining directory");
    }


    public IEnumerable<InstrumentRepoFile> RepositoryInstrumentDetails(string extension) =>
        RootDirectoryInfo
            .RecursiveFindFiles("*." + extension)
            .Select(GetInstrumentFileDetails)
            .OfType<InstrumentRepoFile>();

    public virtual InstrumentRepoFile? GetInstrumentFileDetails(FileInfo timeSeriesFile)
    {
        var structureMatch = PathMatch(timeSeriesFile);
        if (structureMatch is { HasFilePeriodRange: true, HasInstrument: true, TimeSeriesFile: not null })
            return new InstrumentRepoFile
                (structureMatch.Instrument,
                 new TimeSeriesRepoFile(structureMatch.SearchFile, this, structureMatch.TimeSeriesFile), structureMatch.FilePeriodRange);
        Logger.Warn("Could not resolve file details for {0} without opening the file.  This file will not be searchable." +
                    "  Todo open file and read details and move file to expected location", timeSeriesFile.FullName);
        return null;
    }

    public ITimeSeriesFileStructure? FindFileStructure(Instrument instrument)
    {
        var structureMatch = new TimeSeriesInstrumentStructureMatch(instrument);
        foreach (var timeSeriesStructure in Children)
        {
            timeSeriesStructure.InstrumentMatch(structureMatch);
            if (structureMatch.HasTimeSeriesFileStructureMatch) break;
        }
        return structureMatch.TimeSeriesFile;
    }
}
