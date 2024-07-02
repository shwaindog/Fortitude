// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

public static class TimeSeriesBusRulesConstants
{
    public const string PricingRepoBase                = "Markets.Pricing.Repository";
    public const string PricingRepoRetrieveL0Request   = $"{PricingRepoBase}.Retrieve.Level0.Request";
    public const string PricingRepoRetrieveL1Request   = $"{PricingRepoBase}.Retrieve.Level1.Request";
    public const string PricingRepoRetrieveL2Request   = $"{PricingRepoBase}.Retrieve.Level2.Request";
    public const string PricingRepoRetrieveL3Request   = $"{PricingRepoBase}.Retrieve.Level3.Request";
    public const string PricingRepoRetrievePqL0Request = $"{PricingRepoBase}.Retrieve.PQLevel0.Request";
    public const string PricingRepoRetrievePqL1Request = $"{PricingRepoBase}.Retrieve.PQLevel1.Request";
    public const string PricingRepoRetrievePqL2Request = $"{PricingRepoBase}.Retrieve.PQLevel2.Request";
    public const string PricingRepoRetrievePqL3Request = $"{PricingRepoBase}.Retrieve.PQLevel3.Request";

    public const string PricePeriodSummaryRepoStreamRequest   = $"{PricingRepoBase}.Retrieve.PeriodSummary.Stream.Request";
    public const string PricePeriodSummaryRepoRequestResponse = $"{PricingRepoBase}.Retrieve.PeriodSummary.RequestResponse";
}
