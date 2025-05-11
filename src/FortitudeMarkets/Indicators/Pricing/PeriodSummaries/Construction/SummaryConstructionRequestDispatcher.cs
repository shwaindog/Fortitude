// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.PeriodSummaries.Construction;

public interface ISummaryConstructionRequestDispatcher
{
    ISummaryRequestResponseAttendant GetRequestResponseAttendant(HistoricalPeriodResponseRequest request);

    ISummaryStreamRequestAttendant GetStreamRequestAttendant(HistoricalPeriodStreamRequest request);
}

public abstract class SummaryConstructionRequestDispatcher : ISummaryConstructionRequestDispatcher
{
    protected IHistoricalPricePeriodSummaryResolverRule ConstructingRule;

    protected SummaryConstructionRequestDispatcher(IHistoricalPricePeriodSummaryResolverRule constructingRule) => ConstructingRule = constructingRule;

    public abstract ISummaryRequestResponseAttendant GetRequestResponseAttendant(HistoricalPeriodResponseRequest request);

    public abstract ISummaryStreamRequestAttendant GetStreamRequestAttendant(HistoricalPeriodStreamRequest request);
}

public class SubSummaryConstructionRequestDispatcher : SummaryConstructionRequestDispatcher
{
    private readonly TimeBoundaryPeriod subSummaryPeriod;

    public SubSummaryConstructionRequestDispatcher(IHistoricalPricePeriodSummaryResolverRule constructingRule, TimeBoundaryPeriod subSummaryPeriod)
        : base(constructingRule) =>
        this.subSummaryPeriod = subSummaryPeriod;

    public override ISummaryRequestResponseAttendant GetRequestResponseAttendant(HistoricalPeriodResponseRequest request) =>
        new SubSummaryRequestResponseAttendant(ConstructingRule, request, subSummaryPeriod);

    public override ISummaryStreamRequestAttendant GetStreamRequestAttendant(HistoricalPeriodStreamRequest request) =>
        new SubSummaryStreamRequestAttendant(ConstructingRule, request, subSummaryPeriod);
}

public class QuoteToSummaryConstructionRequestDispatcher<TQuote> : SummaryConstructionRequestDispatcher
    where TQuote : class, ITimeSeriesEntry, IPublishableLevel1Quote, new()
{
    public QuoteToSummaryConstructionRequestDispatcher(IHistoricalPricePeriodSummaryResolverRule constructingRule) : base(constructingRule) { }

    public override ISummaryRequestResponseAttendant GetRequestResponseAttendant(HistoricalPeriodResponseRequest request) =>
        new QuoteToSummaryRequestResponseAttendant<TQuote>(ConstructingRule, request);

    public override ISummaryStreamRequestAttendant GetStreamRequestAttendant(HistoricalPeriodStreamRequest request) =>
        new QuoteToSummaryStreamRequestAttendant<TQuote>(ConstructingRule, request);
}
