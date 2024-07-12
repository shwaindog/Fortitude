// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators.Persistence;

public static class IndicatorPersisterConstants
{
    public const string IndicatorPersisterBase = $"{IndicatorServiceConstants.IndicatorsBase}.Persistence";

    public const string IndicatorPersisterFullDrainRequest = $"{IndicatorPersisterBase}.FullDrain.{{0}}.RequestResponse";

    public static string FullDrainRequest<TEntry>() => string.Format(IndicatorPersisterFullDrainRequest, typeof(TEntry).Name);
}
