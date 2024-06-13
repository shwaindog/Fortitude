// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public class TimeSeriesInstrumentStructureMatch
{
    public TimeSeriesInstrumentStructureMatch(Instrument searchInstrument) => SearchInstrument = searchInstrument;

    public Instrument                SearchInstrument { get; }
    public ITimeSeriesFileStructure? TimeSeriesFile   { get; set; }

    public bool HasTimeSeriesFileStructureMatch => TimeSeriesFile != null;
}

public class TimeSeriesFileStructureMatch
{
    private TimeSeriesPeriod? filePeriodMatch;

    public TimeSeriesFileStructureMatch(
        FileInfo searchFile,
        ITimeSeriesDirectoryStructure deepestDirectoryStructureMatch,
        DateTime periodStart = default)
    {
        SearchFile                     = searchFile;
        DeepestDirectoryStructureMatch = deepestDirectoryStructureMatch;
        PeriodStart                    = periodStart;
    }

    public FileInfo SearchFile { get; }

    public bool HasFilePeriodRange => PeriodStart != default;
    public bool HasInstrument => InstrumentNameMatch != null && SourceNameMatch != null && EntryPeriodMatch != null && InstrumentTypeMatch != null;

    public Instrument Instrument => new(InstrumentNameMatch!, SourceNameMatch!, InstrumentTypeMatch!.Value, EntryPeriodMatch!.Value, CategoryMatch);

    public DateTime              PeriodStart     { get; set; }
    public TimeSeriesPeriodRange FilePeriodRange => new(PeriodStart, FilePeriodMatch!.Value);

    public List<string> MatchedPath   { get; set; } = new();
    public List<string> RemainingPath { get; set; } = null!;

    public string? InstrumentNameMatch { get; set; }
    public string? SourceNameMatch     { get; set; }
    public string? CategoryMatch       { get; set; }

    public InstrumentType? InstrumentTypeMatch { get; set; }
    public TimeSeriesPeriod? FilePeriodMatch
    {
        get => filePeriodMatch ?? DeepestDirectoryStructureMatch.PathTimeSeriesPeriod;
        set => filePeriodMatch = value;
    }
    public TimeSeriesPeriod? EntryPeriodMatch { get; set; }

    public ITimeSeriesDirectoryStructure DeepestDirectoryStructureMatch { get; set; }
    public ITimeSeriesFileStructure?     TimeSeriesFile                 { get; set; }
}
