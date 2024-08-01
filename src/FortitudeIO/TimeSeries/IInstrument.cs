// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Chronometry;
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

    public DiscreetTimePeriod CoveringPeriod { get; set; }
    public InstrumentType     InstrumentType { get; }

    void Add(KeyValuePair<string, string> instrumentAttribute);
    void Add(string name, string value);
    bool Remove(string name);
}

public static class InstrumentExtensions
{
    public static string Name(this IInstrument instrument) =>
        $"{instrument.InstrumentType}_{instrument.InstrumentSource}_{instrument.InstrumentName}_{instrument.CoveringPeriod}";
}

public class Instrument : IInstrument
{
    private static string[]? requiredKeys;
    private static string[]? optionalKeys;

    private readonly Dictionary<string, string> instrumentAttributes;

    public Instrument
    (string instrumentName, string instrumentSource, InstrumentType type, DiscreetTimePeriod coveringPeriod
      , IEnumerable<KeyValuePair<string, string>> requiredValues
      , IEnumerable<KeyValuePair<string, string>>? optionalValues = null)
    {
        InstrumentName   = instrumentName;
        InstrumentSource = instrumentSource;
        var optionalOrEmpty = optionalValues ?? Enumerable.Empty<KeyValuePair<string, string>>();
        instrumentAttributes = requiredValues.Concat(optionalOrEmpty).ToDictionary();

        requiredKeys   = requiredValues.Select(x => x.Key).ToArray();
        InstrumentType = type;
        CoveringPeriod = coveringPeriod;
        optionalKeys   = optionalOrEmpty.Select(x => x.Key).ToArray();
    }

    public Instrument
    (string instrumentName, string instrumentSource, InstrumentType type, DiscreetTimePeriod coveringPeriod
      , params KeyValuePair<string, string>[] requiredValues)
        : this(instrumentName, instrumentSource, type, coveringPeriod, requiredValues, null) { }

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

    public DiscreetTimePeriod CoveringPeriod { get; set; }
    public InstrumentType     InstrumentType { get; }
}

public class InstrumentEntryRangeMatch
{
    private Dictionary<string, string> instrumentMatchValues = new();
    public InstrumentEntryRangeMatch(InstrumentType timeSeriesTypeMatch) => TimeSeriesTypeMatch = timeSeriesTypeMatch;
    public InstrumentEntryRangeMatch(DiscreetTimePeriod? coveringPeriodMatchFrom) => CoveringPeriodMatchFrom = coveringPeriodMatchFrom;

    public InstrumentEntryRangeMatch(InstrumentType timeSeriesTypeMatch, DiscreetTimePeriod? coveringPeriodMatchFrom)
        : this(coveringPeriodMatchFrom) =>
        TimeSeriesTypeMatch = timeSeriesTypeMatch;

    public InstrumentEntryRangeMatch
        (InstrumentType timeSeriesTypeMatch, DiscreetTimePeriod? coveringPeriodMatchFrom, DiscreetTimePeriod? coveringPeriodMatchTo)
        : this(timeSeriesTypeMatch, coveringPeriodMatchFrom) =>
        CoveringPeriodMatchTo = coveringPeriodMatchTo;

    public string? InstrumentNameMatch   { get; set; }
    public string? InstrumentSourceMatch { get; set; }

    public string? this[string key]
    {
        get => instrumentMatchValues[key];
        set => instrumentMatchValues[key] = value ?? string.Empty;
    }

    public DiscreetTimePeriod? CoveringPeriodMatchFrom { get; set; }
    public DiscreetTimePeriod? CoveringPeriodMatchTo   { get; set; }
    public InstrumentType?     TimeSeriesTypeMatch     { get; set; }

    public bool Matches(IInstrument instrument)
    {
        var instrumentNameMatches   = InstrumentNameMatch == null || instrument.InstrumentName.Contains(InstrumentNameMatch);
        var instrumentSourceMatches = InstrumentSourceMatch == null || instrument.InstrumentSource.Contains(InstrumentSourceMatch);

        var coveringPeriodFromMatches = CoveringPeriodMatchFrom == null ||
                                        instrument.CoveringPeriod >= (CoveringPeriodMatchFrom?.AveragePeriodTimeSpan() ?? TimeSpan.Zero);
        var coveringPeriodToMatches = CoveringPeriodMatchTo == null ||
                                      instrument.CoveringPeriod <= (CoveringPeriodMatchTo?.AveragePeriodTimeSpan() ?? TimeSpan.MaxValue);
        var instrumentTypeMatches = TimeSeriesTypeMatch == null || instrument.InstrumentType == TimeSeriesTypeMatch;

        var allMatch = instrumentNameMatches && instrumentSourceMatches && coveringPeriodFromMatches && coveringPeriodToMatches &&
                       instrumentTypeMatches;
        return allMatch;
    }
}
