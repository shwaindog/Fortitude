// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem;

#endregion

namespace FortitudeBusRules.Rules.Common.TimeSeries;

public class TimeSeriesRepositoryRetrievalRule : Rule
{
    private readonly IRepositoryBuilder?    repoBuilder;
    protected        ITimeSeriesRepository? TimeSeriesRepository;

    public TimeSeriesRepositoryRetrievalRule(IRepositoryBuilder repoBuilder, string ruleName) : base(ruleName) => this.repoBuilder = repoBuilder;

    public TimeSeriesRepositoryRetrievalRule(ITimeSeriesRepository existingRepository, string ruleName) : base(ruleName) =>
        TimeSeriesRepository = existingRepository;

    public override ValueTask StartAsync()
    {
        if (TimeSeriesRepository == null && repoBuilder != null) TimeSeriesRepository = repoBuilder.BuildRepository();
        if (TimeSeriesRepository == null) throw new InvalidOperationException("Expected TimeSeriesRepository to be set or parameters to create one");
        return base.StartAsync();
    }

    public override ValueTask StopAsync()
    {
        TimeSeriesRepository?.CloseAllFilesAndSessions();
        return base.StopAsync();
    }
}
