// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

public interface ITraderPriceVolumeLayer : IPriceVolumeLayer, IEnumerable<ITraderLayerInfo>,
    ICloneable<ITraderPriceVolumeLayer>
{
    int  Count             { get; }
    bool IsTraderCountOnly { get; }
    ITraderLayerInfo? this[int i] { get; }
    new ITraderPriceVolumeLayer Clone();
}

public interface IMutableTraderPriceVolumeLayer : ITraderPriceVolumeLayer, IMutablePriceVolumeLayer,
    IEnumerable<IMutableTraderLayerInfo>
{
    new IMutableTraderLayerInfo? this[int i] { get; set; }
    void Add(string traderName, decimal size);
    void SetTradersCountOnly(int numberOfTraders);
    bool RemoveAt(int index);

    new IMutableTraderPriceVolumeLayer       Clone();
    new IEnumerator<IMutableTraderLayerInfo> GetEnumerator();
}
