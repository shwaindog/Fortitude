// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel2Quote : ILevel1Quote, ICloneable<ILevel2Quote>, ITimeSeriesEntry<ILevel2Quote>
{
    IOrderBook       BidBook          { get; }
    bool             IsBidBookChanged { get; }
    IOrderBook       AskBook          { get; }
    bool             IsAskBookChanged { get; }
    new ILevel2Quote Clone();
}
