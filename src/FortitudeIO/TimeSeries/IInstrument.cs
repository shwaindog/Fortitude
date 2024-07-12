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
    string InstrumentName   { get; }
    string InstrumentSource { get; }

    string? this[string key] { get; set; }
    IEnumerable<string> RequiredAttributeKeys { get; set; }
    IEnumerable<string> OptionalAttributeKeys { get; set; }

    bool HasAllRequiredKeys { get; }

    IEnumerable<KeyValuePair<string, string>> AllAttributes      { get; }
    IEnumerable<KeyValuePair<string, string>> RequiredAttributes { get; }
    IEnumerable<KeyValuePair<string, string>> OptionalAttributes { get; }

    public TimeSeriesPeriod EntryPeriod    { get; set; }
    public InstrumentType   InstrumentType { get; }

    void Add(KeyValuePair<string, string> instrumentAttribute);
    void Add(string name, string value);
    bool Remove(string name);
}

public static class InstrumentExtensions
{
    public static string Name(this IInstrument instrument) =>
        $"{instrument.InstrumentType}_{instrument.InstrumentSource}_{instrument.InstrumentName}_{instrument.EntryPeriod}";
}

public class Instrument : IInstrument
{
    private static string[]? requiredKeys;
    private static string[]? optionalKeys;

    private readonly Dictionary<string, string> instrumentAttributes;

    public Instrument
    (string instrumentName, string instrumentSource, InstrumentType type, TimeSeriesPeriod entryPeriod
      , IEnumerable<KeyValuePair<string, string>> requiredValues
      , IEnumerable<KeyValuePair<string, string>>? optionalValues = null)
    {
        InstrumentName   = instrumentName;
        InstrumentSource = instrumentSource;
        var optionalOrEmpty = optionalValues ?? Enumerable.Empty<KeyValuePair<string, string>>();
        instrumentAttributes = requiredValues.Concat(optionalOrEmpty).ToDictionary();
        requiredKeys         = requiredValues.Select(x => x.Key).ToArray();
        InstrumentType       = type;
        EntryPeriod          = entryPeriod;
        optionalKeys         = optionalOrEmpty.Select(x => x.Key).ToArray();
    }

    public Instrument
    (string instrumentName, string instrumentSource, InstrumentType type, TimeSeriesPeriod entryPeriod
      , params KeyValuePair<string, string>[] requiredValues)
        : this(instrumentName, instrumentSource, type, entryPeriod, requiredValues, null) { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
        instrumentAttributes.Where(kvp => kvp.Value.IsNotNullOrEmpty()).GetEnumerator();

    public string? this[string key]
    {
        get => instrumentAttributes[key];
        set => instrumentAttributes[key] = value ?? string.Empty;
    }

    public void Add(KeyValuePair<string, string> instrumentAttribute)
    {
        instrumentAttributes.Add(instrumentAttribute.Key, instrumentAttribute.Value);
    }

    public void Add(string name, string value)
    {
        instrumentAttributes.Add(name, value);
    }

    public bool Remove(string name) => instrumentAttributes.Remove(name);

    public IEnumerable<KeyValuePair<string, string>> AllAttributes => instrumentAttributes;

    public IEnumerable<KeyValuePair<string, string>> RequiredAttributes => instrumentAttributes.Where(kvp => RequiredAttributeKeys.Contains(kvp.Key));
    public IEnumerable<KeyValuePair<string, string>> OptionalAttributes => instrumentAttributes.Where(kvp => OptionalAttributeKeys.Contains(kvp.Key));

    public IEnumerable<string> RequiredAttributeKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public bool HasAllRequiredKeys => instrumentAttributes.All(kvp => RequiredAttributeKeys.Contains(kvp.Key) && kvp.Value.IsNotNullOrEmpty());
    public IEnumerable<string> OptionalAttributeKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public string InstrumentName   { get; }
    public string InstrumentSource { get; }

    public TimeSeriesPeriod EntryPeriod    { get; set; }
    public InstrumentType   InstrumentType { get; }
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

    public string? InstrumentNameMatch   { get; set; }
    public string? InstrumentSourceMatch { get; set; }

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
        var instrumentNameMatches   = InstrumentNameMatch == null || instrument.InstrumentName.Contains(InstrumentNameMatch);
        var instrumentSourceMatches = InstrumentSourceMatch == null || instrument.InstrumentSource.Contains(InstrumentSourceMatch);

        var entryPeriodFromMatches = EntryPeriodMatchFrom == null || instrument.EntryPeriod >= EntryPeriodMatchFrom;
        var entryPeriodToMatches   = EntryPeriodMatchTo == null || instrument.EntryPeriod <= EntryPeriodMatchTo;
        var instrumentTypeMatches  = TimeSeriesTypeMatch == null || instrument.InstrumentType == TimeSeriesTypeMatch;

        var allMatch = instrumentNameMatches && instrumentSourceMatches && entryPeriodFromMatches && entryPeriodToMatches && instrumentTypeMatches;
        return allMatch;
    }
}
