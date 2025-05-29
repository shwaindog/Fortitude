// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

// A short term cache of recently last traded trades so clients resyncing can stitch together gaps in last trade updates
// or contains internal order last trades for the day or open position last trades
public interface IRecentlyTraded : ILastTradedList, IInterfacesComparable<IRecentlyTraded>, ICloneable<IRecentlyTraded>
  , IExpiringCachedPeriodUpdateHistory<IRecentlyTraded, ILastTrade>, IShowsEmpty
{
    new ILastTrade this[int index] { get; }

    LastTradedTransmissionFlags TransferFlags { get; }

    new IRecentlyTraded Clone();
}

public interface IMutableRecentlyTraded : IRecentlyTraded, IMutableLastTradedList, ITrackableReset<IMutableRecentlyTraded>
  , IMutableExpiringCachedPeriodUpdateHistory<IMutableRecentlyTraded, IMutableLastTrade>, IEmptyable
{
    new IMutableLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ListShiftCommand> ElementShifts { get; set; }

    new int? ClearedElementsAfterIndex { get; set; }

    new bool HasRandomAccessUpdates { get; set; }

    new int CachedMaxCount { get; set; }

    new LastTradedTransmissionFlags TransferFlags { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }

    new IMutableRecentlyTraded Clone();

    new IMutableRecentlyTraded ResetWithTracking();
}
