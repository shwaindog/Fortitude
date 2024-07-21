// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Generators.Summaries;

public class PQPricePeriodSummaryGenerator : PricePeriodSummaryGenerator<PQPricePeriodSummary>
{
    public PQPricePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo) : base(generatePriceSummaryInfo) { }

    public override PQPricePeriodSummary CreatePricePeriodSummary
        (ISourceTickerInfo sourceTickerInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var currMid = previousCurrentMidPriceTime.CurrentMid;
        return new PQPricePeriodSummary
        {
            TimeSeriesPeriod = GeneratePriceSummaryInfo.SummaryPeriod
          , PeriodEndTime    = GeneratePriceSummaryInfo.SummaryPeriod.PeriodEnd(currMid.Time)
          , PeriodStartTime  = currMid.Time
        };
    }
}
