// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem;

public class PQOneWeekPricePeriodSummaryRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyPriceSummaryTimeSeriesFile<
                           IPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyPriceSummaryTimeSeriesFile<
                           PricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyPriceSummaryTimeSeriesFile<
                           PQPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       WeeklyDailyHourlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var entryPeriod = instrument.EntryPeriod;
        return instrument.Type == InstrumentType.PriceSummaryPeriod &&
               entryPeriod is >= TimeSeriesPeriod.FifteenSeconds and <= TimeSeriesPeriod.FiveMinutes;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyPriceSummaryTimeSeriesFile<PricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new WeeklyDailyHourlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQOneMonthPricePeriodSummaryRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyPriceSummaryTimeSeriesFile<
                           IPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyPriceSummaryTimeSeriesFile<
                           PricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyPriceSummaryTimeSeriesFile<
                           PQPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       MonthlyDailyHourlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var entryPeriod = instrument.EntryPeriod;
        return instrument.Type == InstrumentType.PriceSummaryPeriod &&
               entryPeriod is >= TimeSeriesPeriod.TenMinutes and <= TimeSeriesPeriod.OneHour;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyPriceSummaryTimeSeriesFile<PricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new MonthlyDailyHourlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQOneYearPricePeriodSummaryRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<
                           IPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<
                           PricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<
                           PQPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var entryPeriod = instrument.EntryPeriod;
        return instrument.Type == InstrumentType.PriceSummaryPeriod &&
               entryPeriod == TimeSeriesPeriod.FourHours;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<PricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new YearlyMonthlyWeeklyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQDecenniallyPricePeriodSummaryRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<
                               IPricePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<
                               PricePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<
                               PQPricePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var entryPeriod = instrument.EntryPeriod;
        return instrument.Type == InstrumentType.PriceSummaryPeriod &&
               entryPeriod is >= TimeSeriesPeriod.OneWeek and <= TimeSeriesPeriod.OneMonth;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<IPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<PricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQUnlimitedPricePeriodSummaryRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeSeriesPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var sourceTickerQuoteInfo = instrument as ISourceTickerQuoteInfo;
        if (sourceTickerQuoteInfo == null) throw new Exception("Expected instrument to be of ISourceTickerQuoteInfo");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(sourceTickerQuoteInfo, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)UnlimitedPriceSummaryTimeSeriesFile<
                           IPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)UnlimitedPriceSummaryTimeSeriesFile<
                           PricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPricePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)UnlimitedPriceSummaryTimeSeriesFile<
                           PQPricePeriodSummary>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var entryPeriod = instrument.EntryPeriod;
        return instrument.Type == InstrumentType.PriceSummaryPeriod &&
               entryPeriod is >= TimeSeriesPeriod.OneWeek and <= TimeSeriesPeriod.OneMonth;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(IPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedPriceSummaryTimeSeriesFile<IPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedPriceSummaryTimeSeriesFile<PricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPricePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedPriceSummaryTimeSeriesFile<PQPricePeriodSummary>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQPriceStoragePeriodSummary) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedPriceSummaryTimeSeriesFile<PQPriceStoragePeriodSummary>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}
