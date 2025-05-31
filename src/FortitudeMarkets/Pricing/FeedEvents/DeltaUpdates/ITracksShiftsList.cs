using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;


public interface ITracksShiftsList<out TElement, in TCompare> : IResetableCappedCapacityList<TElement> where TElement : TCompare
{
    IReadOnlyList<ListShiftCommand> ShiftCommands { get; }

    int? ClearRemainingElementsFromIndex { get; }

    bool HasUnreliableListTracking { get; }

    bool CalculateShift(DateTime asAtTime, IReadOnlyList<TCompare> updatedCollection);
}

public interface IMutableTracksShiftsList<TElement, in TCompare> : ITracksShiftsList<TElement, TCompare>, 
    ITracksResetCappedCapacityList<TElement>
    where TElement : ITrackableReset<TElement>, TCompare
{
    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new ushort MaxAllowedSize { get; set; }

    ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    void ClearShiftCommands();

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new TElement this[int index] { get; set; }

    ListShiftCommand InsertAtStart(TElement toInsertAtStart);

    bool AppendAtEnd(TElement toAppendAtEnd);

    ListShiftCommand InsertAt(int index, TElement toInsertAtStart);

    ListShiftCommand DeleteAt(int index);

    ListShiftCommand Delete(TElement toDelete);

    ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply);

    ListShiftCommand ClearAll();

    ListShiftCommand ShiftElements(int byElements);
}

public interface IMutableTracksReorderingList<TElement, in TCompare>
    : IMutableTracksShiftsList<TElement, TCompare> where TElement : ITrackableReset<TElement>, TCompare
{
    ListShiftCommand MoveToStart(TElement existingItem);

    ListShiftCommand MoveToStart(int indexToMoveToStart);

    ListShiftCommand MoveToEnd(int indexToMoveToEnd);

    ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift);

    ListShiftCommand MoveSingleElementBy(TElement existingItem, int shift);

    ListShiftCommand MoveToEnd(TElement existingItem);

    ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex);

    ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex);
}
