// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.Config;

#endregion

namespace FortitudeBusRules.Rules.Common.TimeSeries;

public struct TimeSeriesRepositoryParams
{
    public TimeSeriesRepositoryParams(IRepositoryBuilder repositoryBuilder) => RepositoryBuilder = repositoryBuilder;

    public TimeSeriesRepositoryParams(ITimeSeriesRepository timeSeriesRepository) => TimeSeriesRepository = timeSeriesRepository;

    public IRepositoryBuilder?    RepositoryBuilder    { get; }
    public ITimeSeriesRepository? TimeSeriesRepository { get; }
}

public class TimeSeriesRepositoryAccessRule : Rule
{
    private readonly IRepositoryBuilder? repoBuilder;

    private ITimeSeriesRepository? timeSeriesRepository;

    public TimeSeriesRepositoryAccessRule(TimeSeriesRepositoryParams repositoryParams, string ruleName) : base(ruleName)
    {
        repoBuilder          = repositoryParams.RepositoryBuilder;
        timeSeriesRepository = repositoryParams.TimeSeriesRepository;
    }

    public TimeSeriesRepositoryAccessRule(IRepositoryBuilder repoBuilder, string ruleName) : base(ruleName) => this.repoBuilder = repoBuilder;

    public TimeSeriesRepositoryAccessRule(ITimeSeriesRepository existingRepository, string ruleName) : base(ruleName) =>
        timeSeriesRepository = existingRepository;

    protected ITimeSeriesRepository TimeSeriesRepository => timeSeriesRepository!;

    public override ValueTask StartAsync()
    {
        if (timeSeriesRepository == null && repoBuilder != null) timeSeriesRepository = repoBuilder.BuildRepository();
        if (timeSeriesRepository == null) throw new InvalidOperationException("Expected TimeSeriesRepository to be set or parameters to create one");
        return base.StartAsync();
    }

    public override ValueTask StopAsync()
    {
        timeSeriesRepository?.CloseAllFilesAndSessions();
        return base.StopAsync();
    }

    protected List<InstrumentFileInfo> InstrumentFileInfos
        (string instrumentName, string instrumentSource, InstrumentType? instrumentType, DiscreetTimePeriod? coveringPeriod)
    {
        var matchingFileInfos =
            timeSeriesRepository!
                .AvailableInstrumentsMatching(instrumentName, instrumentSource, instrumentType, coveringPeriod)
                .Select(i => timeSeriesRepository.GetInstrumentFileInfo(i))
                .ToList();
        return matchingFileInfos;
    }

    protected List<InstrumentFileInfo> InstrumentFileInfos
    (string instrumentName, string instrumentSource, InstrumentType? instrumentType, DiscreetTimePeriod? coveringPeriod
      , params KeyValuePair<string, string>[] filterPairs)
    {
        var matchingFileInfos =
            timeSeriesRepository!
                .AvailableInstrumentsMatching(instrumentName, instrumentSource, instrumentType, coveringPeriod)
                .Where(i => filterPairs.All(kvp => i[kvp.Key] == kvp.Value))
                .Select(i => timeSeriesRepository.GetInstrumentFileInfo(i))
                .ToList();
        return matchingFileInfos;
    }

    protected List<InstrumentFileEntryInfo> InstrumentFileEntryInfos
        (string instrumentName, string instrumentSource, InstrumentType? instrumentType, DiscreetTimePeriod? coveringPeriod)
    {
        var matchingFileInfos =
            timeSeriesRepository!
                .AvailableInstrumentsMatching(instrumentName, instrumentSource, instrumentType, coveringPeriod)
                .Select(i => timeSeriesRepository.GetInstrumentFileEntryInfo(i))
                .ToList();
        return matchingFileInfos;
    }
}
