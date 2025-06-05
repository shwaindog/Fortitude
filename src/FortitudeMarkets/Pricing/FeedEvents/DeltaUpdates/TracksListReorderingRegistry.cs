using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using static FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates.ListShiftCommandType;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

public class TracksListReorderingRegistry<TElement, TCompare>
    (IMutableTracksShiftsList<TElement, TCompare> shiftedList, Func<TElement> newElementFactory, Func<TCompare, TCompare, bool> comparison)
    : TrackListShiftsRegistry<TElement, TCompare>(shiftedList, newElementFactory, comparison), IMutableTracksReorderingList<TElement, TCompare>
    where TElement : class, TCompare, ITrackableReset<TElement>, IRecyclableObject, IShowsEmpty
{
    public DateTime UpdateTime { get; set; }

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex)
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == pinElementsFromIndex
         && Math.Sign(lastCommand.Value.Shift) == Math.Sign(byElements)
         && lastCommand.Value.ShiftCommandType == ShiftAllElementsAwayFromPinnedIndex)
        {
            RegisteredShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex((short)pinElementsFromIndex, (short)byElements);
        }
        ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
        return RegisteredShiftCommands.LastShift();
    }

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex)
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == pinElementsFromIndex
         && Math.Sign(lastCommand.Value.Shift) == Math.Sign(byElements)
         && lastCommand.Value.ShiftCommandType == ShiftAllElementsTowardPinnedIndex)
        {
            RegisteredShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendShiftTowardFromIndex((short)pinElementsFromIndex, (short)byElements);
        }
        ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
        return RegisteredShiftCommands.LastShift();
    }

    private readonly List<(short From, short To)> exitingMoves = new();

    public override bool CalculateShift(DateTime asAtTime, IReadOnlyList<TCompare> updatedCollection)
    {
        UpdateTime = asAtTime;

        var wasRecalculated = base.CalculateShift(asAtTime, updatedCollection);
        if (!wasRecalculated)
        {
            return !wasRecalculated;
        }
        RegisteredShiftCommands.Clear();

        exitingMoves.Clear();
        var shiftRightSingleElementMoves = 0;
        var shiftRightCommandCount       = CandidateRightShiftFirstCommands.Count;
        if (shiftRightCommandCount > 0)
        { // check for elements that may have been moved left
            for (var i = ShiftedList.Count - 1; i >= 0; i--)
            {
                var prevItem = ShiftedList[i];

                for (var j = updatedCollection.Count - 1; j >= 0; j--)
                {
                    var updateItem = updatedCollection[j];
                    var finalShiftApplied = CandidateRightShiftFirstCommands
                                            .Where(lsc => lsc.FromIndex < i
                                                       && lsc.ShiftCommandType == ShiftAllElementsAwayFromPinnedIndex)
                                            .Sum(lsc => lsc.Shift);

                    if (Comparison(prevItem, updateItem) && (i + finalShiftApplied) != j)
                    { // found new shift
                        shiftRightSingleElementMoves++;
                        var  updatedOrigin = exitingMoves.Any(pi => pi.To == i) ? exitingMoves.First(pi => pi.To == i).From : i;
                        bool wasMoved      = updatedOrigin != i;

                        var foundMoveOriginal = !wasMoved;
                        var hasLowerIndexInserts =
                            exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                        .Count(pi => pi.From > updatedOrigin && pi.To <= i) -
                            exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                        .Count(pi => pi.From <= updatedOrigin && pi.To > i);
                        if (updatedOrigin + hasLowerIndexInserts != j)
                        {
                            CandidateRightShiftFirstCommands.AppendSwapMoveSingleElement((short)(updatedOrigin + hasLowerIndexInserts), (short)j);
                            exitingMoves.Add((From: (short)(updatedOrigin + hasLowerIndexInserts), To: (short)j));
                        }
                        break;
                    }
                }
            }
        }

        exitingMoves.Clear();
        var shiftLeftSingleElementMoves = 0;
        var shiftLeftCommandCount       = CandidateLeftShiftFirstCommands.Count;
        if (shiftLeftCommandCount > 0)
        { // check for elements that may have been moved left
            for (var i = 0; i < ShiftedList.Count; i++)
            {
                var prevItem = ShiftedList[i];

                for (var j = 0; j < updatedCollection.Count; j++)
                {
                    var updateItem = updatedCollection[j];
                    var finalShiftApplied = CandidateLeftShiftFirstCommands
                                            .Where(lsc => lsc.FromIndex < j
                                                       && lsc.ShiftCommandType == ShiftAllElementsTowardPinnedIndex)
                                            .Sum(lsc => lsc.Shift);

                    if (Comparison(prevItem, updateItem) && (i + finalShiftApplied) != j)
                    { // found new shift
                        shiftLeftSingleElementMoves++;
                        var  updatedOrigin = exitingMoves.Any(pi => pi.To == i) ? exitingMoves.First(pi => pi.To == i).From : i;
                        bool wasMoved      = updatedOrigin != i;

                        var foundMoveOriginal = !wasMoved;
                        var hasLowerIndexInserts =
                            exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                        .Count(pi => pi.From > updatedOrigin && pi.To <= i) -
                            exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                        .Count(pi => pi.From <= updatedOrigin && pi.To > i);
                        if (updatedOrigin + hasLowerIndexInserts != j)
                        {
                            CandidateLeftShiftFirstCommands.AppendSwapMoveSingleElement((short)(updatedOrigin + hasLowerIndexInserts), (short)j);
                            exitingMoves.Add((From: (short)(updatedOrigin + hasLowerIndexInserts), To: (short)j));
                        }
                        break;
                    }
                }
            }
        }

        if (shiftLeftCommandCount > shiftRightCommandCount && CountLeftShiftFirstUnmoved < CountLeftShiftFirstMovedRescued &&
            (shiftLeftSingleElementMoves <= shiftRightSingleElementMoves || shiftRightSingleElementMoves == 0))
        {
            RegisteredShiftCommands.AddRange(CandidateLeftShiftFirstCommands);
            return true;
        }
        if (shiftLeftCommandCount <= shiftRightCommandCount && CountRightShiftFirstUnmoved < CountRightShiftFirstMovedRescued)
        {
            RegisteredShiftCommands.AddRange(CandidateRightShiftFirstCommands);
            return true;
        }

        // See if there are any single moves that without any shifts applied that rescue moved elements
        CandidateLeftShiftFirstCommands.Clear();
        exitingMoves.Clear();
        shiftLeftSingleElementMoves = 0;
        for (var i = 0; i < ShiftedList.Count; i++)
        {
            var prevItem = ShiftedList[i];

            for (var j = 0; j < updatedCollection.Count; j++)
            {
                var updateItem = updatedCollection[j];

                if (Comparison(prevItem, updateItem) && i != j)
                { // found new shift
                    shiftLeftSingleElementMoves++;
                    var  updatedOrigin = exitingMoves.Any(pi => pi.To == i) ? exitingMoves.First(pi => pi.To == i).From : i;
                    bool wasMoved      = updatedOrigin != i;

                    var foundMoveOriginal = !wasMoved;
                    var hasLowerIndexInserts =
                        exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                    .Count(pi => pi.From > updatedOrigin && pi.To <= i) -
                        exitingMoves.TakeWhile(pi => (foundMoveOriginal |= pi.To != i) && pi.To != i)
                                    .Count(pi => pi.From <= updatedOrigin && pi.To > i);
                    if (updatedOrigin + hasLowerIndexInserts != j)
                    {
                        CandidateLeftShiftFirstCommands.AppendSwapMoveSingleElement((short)(updatedOrigin + hasLowerIndexInserts), (short)j);
                        exitingMoves.Add((From: (short)(updatedOrigin + hasLowerIndexInserts), To: (short)j));
                    }
                    break;
                }
            }
        }
        if (CandidateLeftShiftFirstCommands.Any())
        {
            RegisteredShiftCommands.AddRange(CandidateLeftShiftFirstCommands);
            return true;
        }
        return true;
    }

    public ListShiftCommand MoveToStart(TElement existingItem)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveToStart(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{ShiftedList.JoinToString()}]");
    }

    public ListShiftCommand MoveToStart(int indexToMoveToStart)
    {
        if (indexToMoveToStart is < 0 || indexToMoveToStart >= ShiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToStart} as it is outside the existing items. shiftedList:  [{ShiftedList.JoinToString()}]");
        }
        if (!ShiftCommands.Any()) HasUnreliableListTracking = false;
        RegisteredShiftCommands.AppendMoveSingleElement((short)indexToMoveToStart, 0);
        return ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
    }

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd)
    {
        if (indexToMoveToEnd is < 0 || indexToMoveToEnd >= ShiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToEnd} as it is outside the existing items. shiftedList:  [{ShiftedList.JoinToString()}]");
        }
        if (!ShiftCommands.Any()) HasUnreliableListTracking = false;
        RegisteredShiftCommands.AppendMoveSingleElement((short)indexToMoveToEnd
                                                      , (short)(ShiftedList.Count - 1)); // list will shrink by one when element is removed so at
        // reinsert it will be shiftListCount.Count - 2

        return ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
    }

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift)
    {
        if (indexToMoveToEnd is < 0 || indexToMoveToEnd >= ShiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToEnd} as it is outside the existing items. shiftedList:  [{ShiftedList.JoinToString()}]");
        }
        var destinationIndex = Math.Clamp(indexToMoveToEnd + shift, 0, ShiftedList.Capacity - 1);
        RegisteredShiftCommands.AppendMoveSingleElement((short)indexToMoveToEnd, (short)destinationIndex);
        return ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
    }

    public ListShiftCommand MoveSingleElementBy(TElement existingItem, int shift)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveSingleElementBy(indexOfExisting, shift);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{ShiftedList.JoinToString()}]");
    }

    public ListShiftCommand MoveToEnd(TElement existingItem)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveToEnd(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{ShiftedList.JoinToString()}]");
    }
}
