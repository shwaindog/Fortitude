// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;

#endregion

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
    private readonly Dictionary<string, string?> instrumentPathValues = new();

    private TimeSeriesPeriod? filePeriodMatch;

    public PathFileMatch
    (
        FileInfo searchFile,
        string[] requiredFields,
        IPathDirectory deepestDirectoryMatch,
        string[]? optionalFields = null,
        DateTime periodStart = default)
    {
        SearchFile            = searchFile;
        RequiredFields        = requiredFields;
        DeepestDirectoryMatch = deepestDirectoryMatch;
        OptionalFields        = optionalFields ?? Array.Empty<string>();
        PeriodStart           = periodStart;
    }

    public FileInfo SearchFile { get; }

    public bool HasFilePeriodRange => PeriodStart != default;

    public string[] RequiredFields { get; set; }
    public string[] OptionalFields { get; set; }

    public bool HasInstrument =>
        InstrumentNameMatch != null && HasValuesForKeys(RequiredFields) && EntryPeriodMatch != null
     && InstrumentTypeMatch != null;

    public IInstrument Instrument =>
        new Instrument(InstrumentNameMatch!, InstrumentTypeMatch!.Value, EntryPeriodMatch!.Value
                     , ExtractKeyValuePairs(RequiredFields), ExtractKeyValuePairs(OptionalFields));

    public DateTime              PeriodStart     { get; set; }
    public TimeSeriesPeriodRange FilePeriodRange => new(PeriodStart, FilePeriodMatch!.Value);

    public List<string> MatchedPath   { get; set; } = new();
    public List<string> RemainingPath { get; set; } = null!;

    public string? InstrumentNameMatch { get; set; }

    public string? this[string key]
    {
        get => instrumentPathValues[key];
        set => instrumentPathValues[key] = value;
    }

    public InstrumentType? InstrumentTypeMatch { get; set; }
    public TimeSeriesPeriod? FilePeriodMatch
    {
        get => filePeriodMatch ?? DeepestDirectoryMatch.PathTimeSeriesPeriod;
        set => filePeriodMatch = value;
    }
    public TimeSeriesPeriod? EntryPeriodMatch { get; set; }

    public IPathDirectory DeepestDirectoryMatch { get; set; }
    public IPathFile?     TimeSeriesFile        { get; set; }

    public bool HasValuesForKeys(string[] requiredKeys)
    {
        return requiredKeys.Select(key => instrumentPathValues[key]).All(checkValue => checkValue.IsNotNullOrEmpty());
    }

    public IEnumerable<KeyValuePair<string, string>> ExtractKeyValuePairs(string[] keys)
    {
        return instrumentPathValues.Where(kvp => keys.Contains(kvp.Key))!;
    }
}
