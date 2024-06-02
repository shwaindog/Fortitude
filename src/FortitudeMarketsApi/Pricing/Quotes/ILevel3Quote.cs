// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel3Quote : ILevel2Quote, ICloneable<ILevel3Quote>, ITimeSeriesEntry<ILevel3Quote>
{
    IRecentlyTraded? RecentlyTraded       { get; }
    uint             BatchId              { get; }
    uint             SourceQuoteReference { get; }
    DateTime         ValueDate            { get; }
    new ILevel3Quote Clone();
}
