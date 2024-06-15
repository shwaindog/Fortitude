// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public class PathInstrumentMatch
{
    public PathInstrumentMatch(IInstrument searchInstrument) => SearchInstrument = searchInstrument;

    public IInstrument SearchInstrument { get; }
    public IPathFile?  TimeSeriesFile   { get; set; }

    public bool HasTimeSeriesFileStructureMatch => TimeSeriesFile != null;
}

public class PathFileMatch
{
    private TimeSeriesPeriod? filePeriodMatch;

    public PathFileMatch(
        FileInfo searchFile,
        IPathDirectory deepestDirectoryMatch,
        DateTime periodStart = default)
    {
        SearchFile            = searchFile;
        DeepestDirectoryMatch = deepestDirectoryMatch;
        PeriodStart           = periodStart;
    }

    public FileInfo SearchFile { get; }

    public bool HasFilePeriodRange => PeriodStart != default;
    public bool HasInstrument =>
        InstrumentNameMatch != null && SourceNameMatch != null && EntryPeriodMatch != null
     && InstrumentTypeMatch != null && HasMarketClassificationMatch;

    public IInstrument Instrument =>
        new Instrument(InstrumentNameMatch!, SourceNameMatch!, InstrumentTypeMatch!.Value, MarketClassificationMatch!.Value, EntryPeriodMatch!.Value
                     , CategoryMatch);

    public DateTime              PeriodStart     { get; set; }
    public TimeSeriesPeriodRange FilePeriodRange => new(PeriodStart, FilePeriodMatch!.Value);

    public List<string> MatchedPath   { get; set; } = new();
    public List<string> RemainingPath { get; set; } = null!;

    public string? InstrumentNameMatch { get; set; }
    public string? SourceNameMatch     { get; set; }
    public string? CategoryMatch       { get; set; }

    public bool HasMarketClassificationMatch =>
        MarketTypeMatch != null
     && MarketProductTypeMatch != null && MarketRegionMatch != null;
    public MarketClassification? MarketClassificationMatch =>
        HasMarketClassificationMatch
            ? new MarketClassification(MarketTypeMatch!.Value, MarketProductTypeMatch!.Value, MarketRegionMatch!.Value)
            : null;
    public MarketType?   MarketTypeMatch        { get; set; }
    public ProductType?  MarketProductTypeMatch { get; set; }
    public MarketRegion? MarketRegionMatch      { get; set; }

    public InstrumentType? InstrumentTypeMatch { get; set; }
    public TimeSeriesPeriod? FilePeriodMatch
    {
        get => filePeriodMatch ?? DeepestDirectoryMatch.PathTimeSeriesPeriod;
        set => filePeriodMatch = value;
    }
    public TimeSeriesPeriod? EntryPeriodMatch { get; set; }

    public IPathDirectory DeepestDirectoryMatch { get; set; }
    public IPathFile?     TimeSeriesFile        { get; set; }
}
