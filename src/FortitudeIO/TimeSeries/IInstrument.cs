// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries;

public interface IInstrument
{
    public string               InstrumentName       { get; }
    public string               SourceName           { get; }
    public MarketClassification MarketClassification { get; }
    public string?              Category             { get; set; }

    public TimeSeriesPeriod EntryPeriod { get; set; }
    public InstrumentType   Type        { get; }
}

public class Instrument : IInstrument
{
    public Instrument(string instrumentName, string sourceName, InstrumentType type,
        MarketClassification marketClassification, TimeSeriesPeriod entryPeriod, string? category = null)
    {
        InstrumentName       = instrumentName;
        SourceName           = sourceName;
        MarketClassification = marketClassification;
        Type                 = type;
        EntryPeriod          = entryPeriod;
        Category             = category;
    }

    public string               InstrumentName       { get; }
    public string               SourceName           { get; }
    public MarketClassification MarketClassification { get; }
    public string?              Category             { get; set; }

    public TimeSeriesPeriod EntryPeriod { get; set; }
    public InstrumentType   Type        { get; }
}

public class InstrumentMatch
{
    public InstrumentMatch(InstrumentType timeSeriesTypeMatch) => TimeSeriesTypeMatch = timeSeriesTypeMatch;
    public InstrumentMatch(TimeSeriesPeriod? entryPeriodMatchFrom) => EntryPeriodMatchFrom = entryPeriodMatchFrom;

    public InstrumentMatch(InstrumentType timeSeriesTypeMatch, TimeSeriesPeriod? entryPeriodMatchFrom)
        : this(entryPeriodMatchFrom) =>
        TimeSeriesTypeMatch = timeSeriesTypeMatch;

    public InstrumentMatch(InstrumentType timeSeriesTypeMatch, TimeSeriesPeriod? entryPeriodMatchFrom, TimeSeriesPeriod? entryPeriodMatchTo)
        : this(timeSeriesTypeMatch, entryPeriodMatchFrom) =>
        EntryPeriodMatchTo = entryPeriodMatchTo;

    public string?               InstrumentNameMatch       { get; set; }
    public string?               SourceNameMatch           { get; set; }
    public string?               CategoryMatch             { get; set; }
    public MarketClassification? MarketClassificationMatch { get; set; }

    public TimeSeriesPeriod? EntryPeriodMatchFrom { get; set; }
    public TimeSeriesPeriod? EntryPeriodMatchTo   { get; set; }
    public InstrumentType?   TimeSeriesTypeMatch  { get; set; }

    public bool Matches(IInstrument instrument)
    {
        var instrumentNameMatches  = InstrumentNameMatch == null || instrument.InstrumentName.Contains(InstrumentNameMatch);
        var sourceNameMatches      = SourceNameMatch == null || instrument.SourceName.Contains(SourceNameMatch);
        var categoryMatches        = CategoryMatch == null || instrument.Category == null || instrument.Category.Contains(CategoryMatch);
        var entryPeriodFromMatches = EntryPeriodMatchFrom == null || instrument.EntryPeriod >= EntryPeriodMatchFrom;
        var entryPeriodToMatches   = EntryPeriodMatchTo == null || instrument.EntryPeriod <= EntryPeriodMatchTo;
        var instrumentTypeMatches  = TimeSeriesTypeMatch == null || instrument.Type == TimeSeriesTypeMatch;

        var allMatch = instrumentNameMatches && sourceNameMatches && categoryMatches
                    && entryPeriodFromMatches && entryPeriodToMatches && instrumentTypeMatches;
        return allMatch;
    }
}
