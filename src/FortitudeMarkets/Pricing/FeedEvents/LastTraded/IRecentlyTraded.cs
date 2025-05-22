// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IRecentlyTraded : ILastTradedList, IInterfacesComparable<IRecentlyTraded>, ICloneable<IRecentlyTraded>
  , IExpiringCachedPeriodUpdateHistory<IRecentlyTraded, ILastTrade>
{
    new ILastTrade this[int index] { get; }
    new IRecentlyTraded Clone();
}

public interface IMutableRecentlyTraded : IRecentlyTraded, IMutableLastTradedList, ITrackableReset<IMutableRecentlyTraded>
  , IMutableExpiringCachedPeriodUpdateHistory<IMutableRecentlyTraded, IMutableLastTrade>
{
    new IMutableLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ElementShift> ElementShifts { get; set; }

    new ushort? ClearedElementsAfterIndex { get; set; }
    new bool    HasRandomAccessUpdates    { get; set; }

    new int CachedMaxCount { get; set; }

    new TimeBoundaryPeriod     DuringPeriod { get; set; }
    new IMutableRecentlyTraded Clone();

    new IMutableRecentlyTraded ResetWithTracking();
}
