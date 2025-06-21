// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public interface ICandleConstructionRequestDispatcher
{
    ICandleRequestResponseAttendant GetRequestResponseAttendant(HistoricalCandleResponseRequest request);

    ICandleStreamRequestAttendant GetStreamRequestAttendant(HistoricalCandleStreamRequest request);
}

public abstract class CandleConstructionRequestDispatcher : ICandleConstructionRequestDispatcher
{
    protected IHistoricalCandleResolverRule ConstructingRule;

    protected CandleConstructionRequestDispatcher(IHistoricalCandleResolverRule constructingRule) => ConstructingRule = constructingRule;

    public abstract ICandleRequestResponseAttendant GetRequestResponseAttendant(HistoricalCandleResponseRequest request);

    public abstract ICandleStreamRequestAttendant GetStreamRequestAttendant(HistoricalCandleStreamRequest request);
}

public class SubCandleConstructionRequestDispatcher : CandleConstructionRequestDispatcher
{
    private readonly TimeBoundaryPeriod subCandlesPeriod;

    public SubCandleConstructionRequestDispatcher(IHistoricalCandleResolverRule constructingRule, TimeBoundaryPeriod subCandlesPeriod)
        : base(constructingRule) =>
        this.subCandlesPeriod = subCandlesPeriod;

    public override ICandleRequestResponseAttendant GetRequestResponseAttendant(HistoricalCandleResponseRequest request) =>
        new SubCandleRequestResponseAttendant(ConstructingRule, request, subCandlesPeriod);

    public override ICandleStreamRequestAttendant GetStreamRequestAttendant(HistoricalCandleStreamRequest request) =>
        new SubCandleStreamRequestAttendant(ConstructingRule, request, subCandlesPeriod);
}

public class QuoteToCandleConstructionRequestDispatcher<TQuote> : CandleConstructionRequestDispatcher
    where TQuote : class, ITimeSeriesEntry, IPublishableLevel1Quote, new()
{
    public QuoteToCandleConstructionRequestDispatcher(IHistoricalCandleResolverRule constructingRule) : base(constructingRule) { }

    public override ICandleRequestResponseAttendant GetRequestResponseAttendant(HistoricalCandleResponseRequest request) =>
        new QuoteToCandleRequestResponseAttendant<TQuote>(ConstructingRule, request);

    public override ICandleStreamRequestAttendant GetStreamRequestAttendant(HistoricalCandleStreamRequest request) =>
        new QuoteToCandleStreamRequestAttendant<TQuote>(ConstructingRule, request);
}
