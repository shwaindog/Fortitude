// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

public static class HistoricalQuoteTimeSeriesRepositoryConstants
{
    public const string PricingRepoBase                         = "Markets.Pricing.Repository";
    public const string PricingRepoRetrieveTickInstantRequest   = $"{PricingRepoBase}.Retrieve.TickInstant.Request";
    public const string PricingRepoRetrieveL1QuoteRequest       = $"{PricingRepoBase}.Retrieve.Level1Quote.Request";
    public const string PricingRepoRetrieveL2QuoteRequest       = $"{PricingRepoBase}.Retrieve.Level2Quote.Request";
    public const string PricingRepoRetrieveL3QuoteRequest       = $"{PricingRepoBase}.Retrieve.Level3Quote.Request";
    public const string PricingRepoRetrievePqTickInstantRequest = $"{PricingRepoBase}.Retrieve.PQTickInstant.Request";
    public const string PricingRepoRetrievePqL1QuoteRequest     = $"{PricingRepoBase}.Retrieve.PQLevel1Quote.Request";
    public const string PricingRepoRetrievePqL2QuoteRequest     = $"{PricingRepoBase}.Retrieve.PQLevel2Quote.Request";
    public const string PricingRepoRetrievePqL3QuoteRequest     = $"{PricingRepoBase}.Retrieve.PQLevel3Quote.Request";

    public const string PricePeriodSummaryRepoStreamRequest   = $"{PricingRepoBase}.Retrieve.PeriodSummary.Stream.Request";
    public const string PricePeriodSummaryRepoRequestResponse = $"{PricingRepoBase}.Retrieve.PeriodSummary.RequestResponse";
}
