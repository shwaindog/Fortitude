// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Summaries;
using FortitudeMarkets.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Summaries;

public class PQPriceStoragePeriodSummaryGenerator : PricePeriodSummaryGenerator<PQPriceStoragePeriodSummary>
{
    public PQPriceStoragePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo) : base(generatePriceSummaryInfo) { }

    public override PQPriceStoragePeriodSummary CreatePricePeriodSummary
        (ISourceTickerInfo sourceTickerInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var currMid = previousCurrentMidPriceTime.CurrentMid;
        return new PQPriceStoragePeriodSummary
        {
            TimeBoundaryPeriod = GeneratePriceSummaryInfo.SummaryPeriod
          , PeriodStartTime    = currMid.Time
        };
    }
}
