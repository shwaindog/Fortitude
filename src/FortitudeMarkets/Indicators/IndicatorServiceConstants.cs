// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Indicators;

public static class IndicatorServiceConstants
{
    public const string IndicatorsBase = "Markets.Indicators";

    public const string IndicatorsServiceRegistryBase        = $"{IndicatorsBase}.Service.Registry";
    public const string PricingIndicatorsServiceStartRequest = $"{IndicatorsServiceRegistryBase}.Start.CandleService.Request";
    public const string GlobalIndicatorsServiceStartRequest  = $"{IndicatorsServiceRegistryBase}.Start.GlobalService.Request";

    public const string IndicatorsServiceStatusUpdate = $"{PricingIndicatorsServiceStartRequest}.Service.Status.Update";
}
