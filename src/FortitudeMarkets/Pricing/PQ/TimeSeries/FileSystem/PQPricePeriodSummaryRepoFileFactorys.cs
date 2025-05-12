// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem;

public class PQOneWeekCandleRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeBoundaryPeriod.OneWeek) throw new Exception("Expected file period to be one week");
        var pricingInstrumentId = instrument as IPricingInstrumentId;
        if (pricingInstrumentId == null) throw new Exception("Expected instrument to be of IPricingInstrumentId");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(pricingInstrumentId, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry> OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyCandleTimeSeriesFile<
                           ICandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(Candle) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyCandleTimeSeriesFile<
                           Candle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQCandle) => (ITimeSeriesEntryFile<TEntry>)WeeklyDailyHourlyCandleTimeSeriesFile<
                           PQCandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQStorageCandle) => (ITimeSeriesEntryFile<TEntry>)
                       WeeklyDailyHourlyCandleTimeSeriesFile<PQStorageCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var coveringPeriod = instrument.CoveringPeriod;
        return instrument.InstrumentType == InstrumentType.Candle &&
               coveringPeriod.Period is >= TimeBoundaryPeriod.FifteenSeconds and <= TimeBoundaryPeriod.FiveMinutes;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyCandleTimeSeriesFile<ICandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(Candle) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyCandleTimeSeriesFile<Candle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new WeeklyDailyHourlyCandleTimeSeriesFile<PQCandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQStorageCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new WeeklyDailyHourlyCandleTimeSeriesFile<PQStorageCandle>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQOneMonthCandleRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeBoundaryPeriod.OneMonth) throw new Exception("Expected file period to be one month");
        var pricingInstrumentId = instrument as IPricingInstrumentId;
        if (pricingInstrumentId == null) throw new Exception("Expected instrument to be of IPricingInstrumentId");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(pricingInstrumentId, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry> OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyCandleTimeSeriesFile<
                           ICandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(Candle) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyCandleTimeSeriesFile<
                           Candle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQCandle) => (ITimeSeriesEntryFile<TEntry>)MonthlyDailyHourlyCandleTimeSeriesFile<
                           PQCandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQStorageCandle) => (ITimeSeriesEntryFile<TEntry>)
                       MonthlyDailyHourlyCandleTimeSeriesFile<PQStorageCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var candlePeriod = instrument.CoveringPeriod;
        return instrument.InstrumentType == InstrumentType.Candle &&
               candlePeriod.Period is >= TimeBoundaryPeriod.TenMinutes and <= TimeBoundaryPeriod.OneHour;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyCandleTimeSeriesFile<ICandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(Candle) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyCandleTimeSeriesFile<Candle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new MonthlyDailyHourlyCandleTimeSeriesFile<PQCandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQStorageCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new MonthlyDailyHourlyCandleTimeSeriesFile<PQStorageCandle>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQOneYearPriceCandleRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeBoundaryPeriod.OneYear) throw new Exception("Expected file period to be one year");
        var pricingInstrumentId = instrument as IPricingInstrumentId;
        if (pricingInstrumentId == null) throw new Exception("Expected instrument to be of IPricingInstrumentId");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(pricingInstrumentId, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry> OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyCandleTimeSeriesFile<
                           ICandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(Candle) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyCandleTimeSeriesFile<
                           Candle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQCandle) => (ITimeSeriesEntryFile<TEntry>)YearlyMonthlyWeeklyCandleTimeSeriesFile<
                           PQCandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQStorageCandle) => (ITimeSeriesEntryFile<TEntry>)
                       YearlyMonthlyWeeklyCandleTimeSeriesFile<PQStorageCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var candlePeriod = instrument.CoveringPeriod;
        return instrument.InstrumentType == InstrumentType.Candle &&
               candlePeriod == TimeBoundaryPeriod.FourHours;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyCandleTimeSeriesFile<ICandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(Candle) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyCandleTimeSeriesFile<Candle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new YearlyMonthlyWeeklyCandleTimeSeriesFile<PQCandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQStorageCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new YearlyMonthlyWeeklyCandleTimeSeriesFile<PQStorageCandle>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQDecenniallyCandleRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        if (filePeriod != TimeBoundaryPeriod.OneDecade) throw new Exception("Expected file period to be one decade");
        var pricingInstrumentId = instrument as IPricingInstrumentId;
        if (pricingInstrumentId == null) throw new Exception("Expected instrument to be of IPricingInstrumentId");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(pricingInstrumentId, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry> OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyCandleTimeSeriesFile<
                               ICandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(Candle) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyCandleTimeSeriesFile<
                               Candle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQCandle) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyCandleTimeSeriesFile<
                               PQCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQStorageCandle) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedDecenniallyYearlyCandleTimeSeriesFile<PQStorageCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var candlePeriod = instrument.CoveringPeriod;
        return instrument.InstrumentType == InstrumentType.Candle &&
               candlePeriod.Period is >= TimeBoundaryPeriod.OneWeek and <= TimeBoundaryPeriod.OneMonth;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyCandleTimeSeriesFile<ICandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(Candle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyCandleTimeSeriesFile<Candle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyCandleTimeSeriesFile<PQCandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQStorageCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedDecenniallyYearlyCandleTimeSeriesFile<PQStorageCandle>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}

public class PQUnlimitedCandleRepoFileFactory<TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle

{
    protected override TimeSeriesFileParameters CreateTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, 7, FileFlags.WriterOpened);
    }

    protected virtual PriceTimeSeriesFileParameters CreatePriceQuoteTimeSeriesFileParameters
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var pricingInstrumentId = instrument as IPricingInstrumentId;
        if (pricingInstrumentId == null) throw new Exception("Expected instrument to be of IPricingInstrumentId");

        var timeSeriesFileParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);

        return new PriceTimeSeriesFileParameters(pricingInstrumentId, timeSeriesFileParams);
    }

    public override ITimeSeriesEntryFile<TEntry> OpenExistingEntryFile(FileInfo fileInfo)
    {
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) => (ITimeSeriesEntryFile<TEntry>)UnlimitedCandleTimeSeriesFile<
                           ICandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(Candle) => (ITimeSeriesEntryFile<TEntry>)UnlimitedCandleTimeSeriesFile<
                           Candle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQCandle) => (ITimeSeriesEntryFile<TEntry>)UnlimitedCandleTimeSeriesFile<
                           PQCandle>
                       .OpenExistingTimeSeriesFile(fileInfo)
                 , _ when EntryType == typeof(PQStorageCandle) => (ITimeSeriesEntryFile<TEntry>)
                       UnlimitedCandleTimeSeriesFile<PQStorageCandle>
                           .OpenExistingTimeSeriesFile(fileInfo)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }

    public override ITimeSeriesFile OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);

    public override bool IsBestFactoryFor(IInstrument instrument)
    {
        var coveringPeriod = instrument.CoveringPeriod;
        return instrument.InstrumentType == InstrumentType.Candle &&
               coveringPeriod.Period is >= TimeBoundaryPeriod.OneWeek and <= TimeBoundaryPeriod.OneMonth;
    }

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var priceQuoteFileParams = CreatePriceQuoteTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return EntryType switch
               {
                   _ when EntryType == typeof(ICandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedCandleTimeSeriesFile<ICandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(Candle) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedCandleTimeSeriesFile<Candle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)new UnlimitedCandleTimeSeriesFile<PQCandle>(priceQuoteFileParams)
                 , _ when EntryType == typeof(PQStorageCandle) =>
                       (ITimeSeriesEntryFile<TEntry>)
                       new UnlimitedCandleTimeSeriesFile<PQStorageCandle>(priceQuoteFileParams)
                 , _ => throw new Exception("Expected entry type to be ILevel#Quote type")
               };
    }
}
