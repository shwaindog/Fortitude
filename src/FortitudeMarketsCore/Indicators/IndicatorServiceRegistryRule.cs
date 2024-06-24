// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarketsCore.Indicators;

public interface IIndicatorServicesConfig
{
    IMarketConnectionConfig MarketConnectionConfig { get; set; }
}

public class IndicatorServiceRegistryRule { }
