// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
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

    private TimeBoundaryPeriod? filePeriodMatch;

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
        InstrumentNameMatch != null && InstrumentSourceMatch != null && HasValuesForKeys(RequiredFields) && CoveringPeriodMatch != null
     && InstrumentTypeMatch != null;

    public IInstrument Instrument =>
        new Instrument(InstrumentNameMatch!, InstrumentSourceMatch!, InstrumentTypeMatch!.Value, CoveringPeriodMatch!.Value
                     , ExtractKeyValuePairs(RequiredFields), ExtractKeyValuePairs(OptionalFields));

    public DateTime                PeriodStart     { get; set; }
    public TimeBoundaryPeriodRange FilePeriodRange => new(PeriodStart, FilePeriodMatch!.Value);

    public List<string> MatchedPath   { get; set; } = new();
    public List<string> RemainingPath { get; set; } = null!;

    public string? InstrumentNameMatch   { get; set; }
    public string? InstrumentSourceMatch { get; set; }

    public string? this[string key]
    {
        get => instrumentPathValues[key];
        set => instrumentPathValues[key] = value;
    }

    public InstrumentType? InstrumentTypeMatch { get; set; }
    public TimeBoundaryPeriod? FilePeriodMatch
    {
        get => filePeriodMatch ?? DeepestDirectoryMatch.PathTimeBoundaryPeriod;
        set => filePeriodMatch = value;
    }
    public DiscreetTimePeriod? CoveringPeriodMatch { get; set; }

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
