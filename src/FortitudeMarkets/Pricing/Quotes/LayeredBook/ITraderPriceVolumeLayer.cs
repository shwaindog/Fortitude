// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ITraderPriceVolumeLayer : IPriceVolumeLayer,
    ICloneable<ITraderPriceVolumeLayer>
{
    int  Count             { get; }
    bool IsTraderCountOnly { get; }

    IList<ITraderLayerInfo?> TraderDetails { get; }
    ITraderLayerInfo? this[int i] { get; }
    new ITraderPriceVolumeLayer Clone();
}

public interface IMutableTraderPriceVolumeLayer : ITraderPriceVolumeLayer, IMutablePriceVolumeLayer
{
    new IMutableTraderLayerInfo? this[int i] { get; set; }

    new IList<IMutableTraderLayerInfo?> TraderDetails { get; }

    void Add(string traderName, decimal size);
    void SetTradersCountOnly(int numberOfTraders);
    bool RemoveAt(int index);

    new IMutableTraderPriceVolumeLayer Clone();
}
