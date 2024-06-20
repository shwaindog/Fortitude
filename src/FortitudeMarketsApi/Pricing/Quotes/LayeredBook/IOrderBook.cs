// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

public enum BookSide
{
    Unknown
  , BidBook
  , AskBook
}

public interface IOrderBook : IEnumerable<IPriceVolumeLayer>, IReusableObject<IOrderBook>,
    IInterfacesComparable<IOrderBook>
{
    LayerType  LayersOfType             { get; }
    LayerFlags LayersSupportsLayerFlags { get; }

    bool     IsLadder { get; }
    int      Capacity { get; }
    int      Count    { get; }
    BookSide BookSide { get; }
    IPriceVolumeLayer? this[int level] { get; }
}

public interface IMutableOrderBook : IOrderBook, ICloneable<IMutableOrderBook>
{
    new int Capacity { get; set; }
    new IMutablePriceVolumeLayer? this[int level] { get; set; }
    new IMutableOrderBook Clone();

    int AppendEntryAtEnd();
}
