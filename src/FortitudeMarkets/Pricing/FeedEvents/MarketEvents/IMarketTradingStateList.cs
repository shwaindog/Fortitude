using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IMarketTradingStateList : IReusableObject<IMarketTradingStateList>, IInterfacesComparable<IMarketTradingStateList>
  , ICachedRecentCountHistory<IMarketTradingStateEvent, IMarketTradingStateEvent>
{
    IReadOnlyList<IMarketTradingStateEvent> TradingStateEvents { get; }
}

public interface IMutableMarketTradingStateList : IMarketTradingStateList, ITrackableReset<IMutableMarketTradingStateList>
  , IMutableCachedRecentCountHistory<IMutableMarketTradingStateEvent, IMarketTradingStateEvent>, IDiscreetUpdatable
{
    new int Capacity { get; set; }

    new IMutableMarketTradingStateEvent this[int i] { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int Count { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new void Clear();

    new bool IsReadOnly { get; }

    new void RemoveAt(int index);
    
    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IMarketTradingStateEvent> updatedCollection);

    new IMutableMarketTradingStateList Clone();

    new IEnumerator<IMutableMarketTradingStateEvent> GetEnumerator();

    new IMutableMarketTradingStateList ResetWithTracking();

    string EachTradingStateByIndexOnNewLines();

    int AppendEntryAtEnd();
}