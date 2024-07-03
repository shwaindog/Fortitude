// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators;

public static class IndicatorServiceConstants
{
    public const string IndicatorsBase = "Markets.Indicators";

    public const string IndicatorsServiceRegistryBase            = $"{IndicatorsBase}.Service.Registry";
    public const string PricePeriodIndicatorsServiceStartRequest = $"{IndicatorsServiceRegistryBase}.Start.PricePeriodService.Request";
    public const string GlobalIndicatorsServiceStartRequest      = $"{IndicatorsServiceRegistryBase}.Start.GlobalService.Request";

    public const string IndicatorsServiceStatusUpdate = $"{PricePeriodIndicatorsServiceStartRequest}.Service.Status.Update";
}
