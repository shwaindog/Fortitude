// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators;

public static class IndicatorServiceConstants
{
    public const string IndicatorsBase = "Markets.Indicators";

    public const string IndicatorsServiceRegistryBase = $"{IndicatorsBase}.Service.Registry";
    public const string IndicatorsServiceStartRequest = $"{IndicatorsServiceRegistryBase}.Start.Request";

    public const string IndicatorsServiceStatusUpdate = $"{IndicatorsServiceStartRequest}.Service.Status.Update";
}
