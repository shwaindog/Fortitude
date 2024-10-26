// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Summaries;
using FortitudeMarkets.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Summaries;

public class PQPricePeriodSummaryGenerator : PricePeriodSummaryGenerator<PQPricePeriodSummary>
{
    public PQPricePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo) : base(generatePriceSummaryInfo) { }

    public override PQPricePeriodSummary CreatePricePeriodSummary
        (ISourceTickerInfo sourceTickerInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var currMid = previousCurrentMidPriceTime.CurrentMid;
        return new PQPricePeriodSummary
        {
            TimeBoundaryPeriod = GeneratePriceSummaryInfo.SummaryPeriod
          , PeriodEndTime      = GeneratePriceSummaryInfo.SummaryPeriod.PeriodEnd(currMid.Time)
          , PeriodStartTime    = currMid.Time
        };
    }
}
