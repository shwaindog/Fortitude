// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

// A short term cache of recently last traded trades so clients resyncing can stitch together gaps in last trade updates
// or contains internal order last trades for the day or open position last trades
public interface IRecentlyTraded : ILastTradedList, IInterfacesComparable<IRecentlyTraded>, ICloneable<IRecentlyTraded>
  , IExpiringCachedPeriodUpdateHistory<ILastTrade, ILastTrade>
{
    new ILastTrade this[int index] { get; }

    ListTransmissionFlags TransferFlags { get; }

    new IRecentlyTraded Clone();
}

public interface IMutableRecentlyTraded : IRecentlyTraded, IMutableLastTradedList, ITrackableReset<IMutableRecentlyTraded>
  , IMutableExpiringCachedPeriodUpdateHistory<IMutableLastTrade, ILastTrade>, IAlertLifeCycleChanges<IMarketTradingStateEvent>
{
    new IMutableLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new ListTransmissionFlags TransferFlags { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }
    
    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<ILastTrade> updatedCollection);

    new IMutableRecentlyTraded Clone();

    new IMutableRecentlyTraded ResetWithTracking();
}
