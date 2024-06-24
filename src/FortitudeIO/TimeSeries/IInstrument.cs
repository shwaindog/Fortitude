// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries.FileSystem;

#endregion

namespace FortitudeIO.TimeSeries;

public interface IInstrument : IEnumerable<KeyValuePair<string, string>>
{
    string InstrumentName { get; }

    string? this[string key] { get; set; }
    IEnumerable<string> RequiredInstrumentKeys { get; set; }
    IEnumerable<string> OptionalInstrumentKeys { get; set; }
    bool                HasAllRequiredKeys     { get; }

    public TimeSeriesPeriod EntryPeriod { get; set; }
    public InstrumentType   Type        { get; }
}

public class Instrument : IInstrument
{
    private static string[]? requiredKeys;
    private static string[]? optionalKeys;

    private Dictionary<string, string> instrumentDefinition = new();

    public Instrument
    (string instrumentName, InstrumentType type, TimeSeriesPeriod entryPeriod, IEnumerable<KeyValuePair<string, string>> requiredValues
      , IEnumerable<KeyValuePair<string, string>>? optionalValues = null)
    {
        InstrumentName = instrumentName;
        var optionalOrEmpty = optionalValues ?? Enumerable.Empty<KeyValuePair<string, string>>();
        instrumentDefinition = requiredValues.Concat(optionalOrEmpty).ToDictionary();
        requiredKeys         = requiredValues.Select(x => x.Key).ToArray();
        Type                 = type;
        EntryPeriod          = entryPeriod;
        optionalKeys         = optionalOrEmpty.Select(x => x.Key).ToArray();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
        instrumentDefinition.Where(kvp => kvp.Value.IsNotNullOrEmpty()).GetEnumerator();

    public string? this[string key]
    {
        get => instrumentDefinition[key];
        set => instrumentDefinition[key] = value ?? string.Empty;
    }

    public IEnumerable<string> RequiredInstrumentKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public bool HasAllRequiredKeys => instrumentDefinition.All(kvp => RequiredInstrumentKeys.Contains(kvp.Key) && kvp.Value.IsNotNullOrEmpty());
    public IEnumerable<string> OptionalInstrumentKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public string InstrumentName { get; }

    public TimeSeriesPeriod EntryPeriod { get; set; }
    public InstrumentType   Type        { get; }
}

public class InstrumentEntryRangeMatch
{
    private Dictionary<string, string> instrumentMatchValues = new();
    public InstrumentEntryRangeMatch(InstrumentType timeSeriesTypeMatch) => TimeSeriesTypeMatch = timeSeriesTypeMatch;
    public InstrumentEntryRangeMatch(TimeSeriesPeriod? entryPeriodMatchFrom) => EntryPeriodMatchFrom = entryPeriodMatchFrom;

    public InstrumentEntryRangeMatch(InstrumentType timeSeriesTypeMatch, TimeSeriesPeriod? entryPeriodMatchFrom)
        : this(entryPeriodMatchFrom) =>
        TimeSeriesTypeMatch = timeSeriesTypeMatch;

    public InstrumentEntryRangeMatch(InstrumentType timeSeriesTypeMatch, TimeSeriesPeriod? entryPeriodMatchFrom, TimeSeriesPeriod? entryPeriodMatchTo)
        : this(timeSeriesTypeMatch, entryPeriodMatchFrom) =>
        EntryPeriodMatchTo = entryPeriodMatchTo;

    public string? InstrumentNameMatch { get; set; }

    public string? this[string key]
    {
        get => instrumentMatchValues[key];
        set => instrumentMatchValues[key] = value ?? string.Empty;
    }

    public TimeSeriesPeriod? EntryPeriodMatchFrom { get; set; }
    public TimeSeriesPeriod? EntryPeriodMatchTo   { get; set; }
    public InstrumentType?   TimeSeriesTypeMatch  { get; set; }

    public bool Matches(IInstrument instrument)
    {
        var instrumentNameMatches = InstrumentNameMatch == null || instrument.InstrumentName.Contains(InstrumentNameMatch);

        var entryPeriodFromMatches = EntryPeriodMatchFrom == null || instrument.EntryPeriod >= EntryPeriodMatchFrom;
        var entryPeriodToMatches   = EntryPeriodMatchTo == null || instrument.EntryPeriod <= EntryPeriodMatchTo;
        var instrumentTypeMatches  = TimeSeriesTypeMatch == null || instrument.Type == TimeSeriesTypeMatch;

        var allMatch = instrumentNameMatches && entryPeriodFromMatches && entryPeriodToMatches && instrumentTypeMatches;
        return allMatch;
    }
}
