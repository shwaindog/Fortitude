// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Generators.Summaries;

public class PQPriceStoragePeriodSummaryGenerator : PricePeriodSummaryGenerator<PQPriceStoragePeriodSummary>
{
    public PQPriceStoragePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo) : base(generatePriceSummaryInfo) { }

    public override PQPriceStoragePeriodSummary CreatePricePeriodSummary
        (ISourceTickerQuoteInfo sourceTickerQuoteInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var currMid = previousCurrentMidPriceTime.CurrentMid;
        return new PQPriceStoragePeriodSummary(new PQSourceTickerQuoteInfo(sourceTickerQuoteInfo))
        {
            SummaryPeriod    = GeneratePriceSummaryInfo.SummaryPeriod
          , SummaryStartTime = currMid.Time
        };
    }
}
