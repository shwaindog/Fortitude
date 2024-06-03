// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableOrderBook : IOrderBook, ICloneable<IMutableOrderBook>, IStoreState<IOrderBook>
{
    new int Capacity { get; set; }
    new IMutablePriceVolumeLayer? this[int level] { get; set; }
    new IMutableOrderBook Clone();
    int                   AppendEntryAtEnd();
}
