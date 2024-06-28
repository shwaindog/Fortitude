// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public static class PeriodSummaryConstants
{
    public const string PeriodSummaryBase               = "Markets.Indicators.PricePeriodSummaries";
    public const string PeriodSummaryLiveTemplate       = $"{PeriodSummaryBase}.Live.{{0}}.{{1}}.{{2}}";
    public const string PeriodSummaryCompleteTemplate   = $"{PeriodSummaryBase}.Complete.{{0}}.{{1}}.{{2}}";
    public const string PeriodSummaryHistoricalTemplate = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}";

    public static string LivePeriodSummaryAddress(this ISourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryLiveTemplate, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());

    public static string CompletePeriodSummaryAddress(this ISourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryCompleteTemplate, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());

    public static string HistoricalPeriodSummaryAddress(this ISourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryHistoricalTemplate, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());
}
