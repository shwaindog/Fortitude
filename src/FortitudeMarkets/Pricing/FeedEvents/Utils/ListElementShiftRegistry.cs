using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using static FortitudeMarkets.Pricing.FeedEvents.ListShiftCommandType;

namespace FortitudeMarkets.Pricing.FeedEvents.Utils;

public class ListElementShiftRegistry<T, TCompare>
    (IMutableCapacityList<T> shiftedList, Func<T> newElementFactory, Func<TCompare, TCompare, bool> comparison)
    : ITrackableReset<ListElementShiftRegistry<T, TCompare>>
      , ITransferState<ListElementShiftRegistry<T, TCompare>>, ITransferState<IEnumerable<ListShiftCommand>>
    where T : class, TCompare, IReusableObject<T>, IShowsEmpty
{
    public List<ListShiftCommand> ShiftCommands = new();

    public ushort? ClearRemainingElementsAt
    {
        get =>
            ShiftCommands
                .Where(es => es.Shift == ListShiftCommand.ClearAllShiftAmount)
                .Select(es => (ushort)(es.FromIndex + 1))
                .FirstOrDefault();
        set
        {
            var prevClearAt = ShiftCommands.FindIndex(es => es.Shift == ListShiftCommand.ClearAllShiftAmount);
            if (prevClearAt >= 0)
            {
                ShiftCommands.RemoveAt(prevClearAt);
            }
            if (value == null) return;
            ShiftCommands.AppendShiftTowardFromIndex((short)value, ListShiftCommand.ClearAllShiftAmount);
            ApplyElementShift(ShiftCommands.LastShift());
        }
    }

    public bool HasRandomAccessUpdates { get; set; }
    public int  CachedMaxCount         { get; set; } = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;

    public DateTime UpdateTime { get; set; }

    public T ElementAt(int index) => shiftedList[index];

    public ListElementShiftRegistry<T, TCompare> ResetWithTracking()
    {
        ShiftCommands.Clear();
        return this;
    }

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex)
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == pinElementsFromIndex
         && Math.Sign(lastCommand.Value.Shift) == Math.Sign(byElements)
         && lastCommand.Value.ShiftCommandType == ShiftAllElementsAwayFromPinnedIndex)
        {
            ShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftAwayFromIndex((short)pinElementsFromIndex, (short)byElements);
        }
        ApplyElementShift(ShiftCommands.LastShift());
        return ShiftCommands.LastShift();
    }

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex)
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == pinElementsFromIndex
         && Math.Sign(lastCommand.Value.Shift) == Math.Sign(byElements)
         && lastCommand.Value.ShiftCommandType == ShiftAllElementsTowardPinnedIndex)
        {
            ShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftTowardFromIndex((short)pinElementsFromIndex, (short)byElements);
        }
        ApplyElementShift(ShiftCommands.LastShift());
        return ShiftCommands.LastShift();
    }

    public ListShiftCommand ApplyElementShift(ListShiftCommand shiftCommandToApply)
    {
        switch (shiftCommandToApply.ShiftCommandType)
        {
            case ShiftAllElementsAwayFromPinnedIndex: ApplyShiftAwayFromPinnedIndex(shiftCommandToApply); break;
            case ShiftAllElementsTowardPinnedIndex:   ApplyShiftTowardPinnedIndex(shiftCommandToApply); break;
            case MoveSingleElement:
            case MoveSingleElement
               | InsertElementsRange:
                ApplyMoveSingleElement(shiftCommandToApply);
                break;
            case RemoveElementsRange: ApplyDeleteElements(shiftCommandToApply); break;
            default:                  throw new ArgumentException($"Currently don't handle {shiftCommandToApply.ShiftCommandType}");
        }

        return shiftCommandToApply;
    }

    private void ApplyDeleteElements(ListShiftCommand shiftCommandToApply)
    {
        var (rangeToRemove, originIndex) = shiftCommandToApply;
        if (originIndex >= shiftedList.Count) return;
        var removeIndex = Math.Clamp(originIndex, 0, shiftedList.Count);
        for (int i = 0; i < rangeToRemove; i++)
        {
            var lastTradeAt = shiftedList[removeIndex];
            lastTradeAt.StateReset();
            shiftedList.RemoveAt(removeIndex);
            shiftedList.Add(lastTradeAt);
        }
    }


    private void ApplyMoveSingleElement(ListShiftCommand shiftCommandToApply)
    {
        var (destinationIndex, originIndex) = shiftCommandToApply;
        var lastTradeAt       = shiftedList[originIndex];
        shiftedList.RemoveAt(originIndex);
        if (destinationIndex > shiftedList.Capacity)
        {
            shiftedList.Add(lastTradeAt);
        }
        else
        {
            if (shiftCommandToApply.IsSwapWithDestination())
            {
                var adjustedDestIndex   = Math.Clamp(originIndex < destinationIndex ? destinationIndex - 1 : destinationIndex, 0, shiftedList.Capacity - 1);
                var adjustedOriginIndex = Math.Clamp(originIndex > destinationIndex ? originIndex - 1 : originIndex, 0, shiftedList.Capacity - 1);
                var destinationElement  = shiftedList[adjustedDestIndex];
                shiftedList.RemoveAt(adjustedDestIndex);
                shiftedList.Insert(adjustedOriginIndex, destinationElement);
            }
            shiftedList.Insert(destinationIndex, lastTradeAt);
        }
    }

    private void ApplyShiftTowardPinnedIndex(ListShiftCommand shiftCommandToApply)
    {
        var (byElements, pinElementsFromIndex) = shiftCommandToApply;
        if (byElements < 0)
        {
            ApplyShiftLeftTowardPinnedIndex(pinElementsFromIndex, byElements);
        }
        if (byElements > 0)
        {
            ApplyShiftRightTowardPinnedIndex(pinElementsFromIndex, byElements);
        }
    }

    private void ApplyShiftLeftTowardPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // remove entry at pinElementsFromIndex + 1 and reset and append to end.
        var removeIndex = Math.Clamp(pinElementsFromIndex + 1, 0, CachedMaxCount - 1);
        if (removeIndex >= shiftedList.Count) return;
        var insertEmptyIndex = Math.Min(shiftedList.Capacity - 1, CachedMaxCount - 1);
        var shiftAmount = byElements == short.MinValue
            ? Math.Clamp(shiftedList.Count - removeIndex, 0, shiftedList.Count)
            : Math.Clamp(Math.Abs(byElements), 0, shiftedList.Count - removeIndex);
        for (var i = 0; i < shiftAmount; i++)
        {
            var checkLastExistingElement = shiftedList[removeIndex];
            shiftedList.RemoveAt(removeIndex);
            checkLastExistingElement.StateReset();
            if (insertEmptyIndex >= shiftedList.Capacity)
            {
                shiftedList.Add(checkLastExistingElement);
            }
            else
            {
                shiftedList.Insert(insertEmptyIndex, checkLastExistingElement);
            }
        }
    }

    private void ApplyShiftRightTowardPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // insert a new entry (check end for possible spare items at pinElementsFromIndex - 1) otherwise create a new entry at 0.
        var removeIndex = Math.Clamp(pinElementsFromIndex - 1, 0, CachedMaxCount - 1);
        for (var i = byElements; i > 0; i--)
        {
            var lastTradeAt = removeIndex > shiftedList.Capacity ? newElementFactory() : shiftedList[removeIndex];
            lastTradeAt.StateReset();
            shiftedList.RemoveAt(removeIndex);
            shiftedList.Insert(0, lastTradeAt);
        }
        PostInsertCheck();
    }

    private void ApplyShiftAwayFromPinnedIndex(ListShiftCommand shiftCommandToApply)
    {
        var (byElements, pinElementsFromIndex) = shiftCommandToApply;
        if (byElements < 0)
        {
            ApplyShiftLeftAwayFromPinnedIndex(pinElementsFromIndex, byElements);
        }
        if (byElements > 0)
        {
            ApplyShiftRightAwayFromPinnedIndex(pinElementsFromIndex, byElements);
        }
    }

    private void ApplyShiftLeftAwayFromPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // remove first entry and place it at pinElementsFromIndex  - 1.
        var insertEmptyIndex = Math.Clamp(pinElementsFromIndex - 1, 0, CachedMaxCount - 1);
        var shiftAmount      = byElements == short.MinValue ? shiftedList.Count : Math.Clamp(Math.Abs(byElements), 0, CachedMaxCount - 1);
        for (var i = shiftAmount; i > 0; i--)
        {
            var lastTradeAt = shiftedList[0];
            lastTradeAt.StateReset();
            shiftedList.RemoveAt(0);
            if (insertEmptyIndex >= shiftedList.Capacity)
            {
                shiftedList.Add(lastTradeAt);
            }
            else
            {
                shiftedList.Insert(insertEmptyIndex, lastTradeAt);
            }
        }
    }

    private void ApplyShiftRightAwayFromPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // insert a new entry (check end for possible spare items at end) otherwise create a new entry at pinElementsFromIndex + 1.
        var insertEmptyIndex = Math.Clamp(pinElementsFromIndex + 1, 0, shiftedList.Capacity - 1);
        for (var i = insertEmptyIndex; i < insertEmptyIndex + byElements; i++)
        {
            var lastElementCliffDeath = i + byElements >= CachedMaxCount;
            var checkEndElementIndex  = Math.Min(shiftedList.Count, CachedMaxCount - 1); // one past populated elements or very last allowed
            if (lastElementCliffDeath)
            {
                var checkLastExistingElement = shiftedList[checkEndElementIndex];
                shiftedList.RemoveAt(checkEndElementIndex);
                checkLastExistingElement.StateReset();
                if (insertEmptyIndex >= shiftedList.Capacity)
                {
                    shiftedList.Add(checkLastExistingElement);
                }
                else
                {
                    shiftedList.Insert(insertEmptyIndex, checkLastExistingElement);
                }
            }
            else
            {
                shiftedList.Insert(insertEmptyIndex, newElementFactory());
            }
        }
        PostInsertCheck();
    }

    private readonly List<ListShiftCommand> candidateRightShiftFirstCommands = new();
    private readonly List<ListShiftCommand> candidateLeftShiftFirstCommands  = new();

    private readonly List<(short From, short To)> exitingMoves = new List<(short From, short To)>();

    public virtual void CalculateShift(DateTime asAtTime, IReadOnlyList<TCompare> updatedCollection)
    {
        UpdateTime = asAtTime;
        if (ShiftCommands.Any() && HasRandomAccessUpdates)
        {
            ShiftCommands.Clear();
            HasRandomAccessUpdates = false;
        }

        candidateRightShiftFirstCommands.Clear();
        int lastMoveFromIndex = updatedCollection.Count;
        for (var i = shiftedList.Count - 1; i >= 0; i--)
        {
            var prevItem = shiftedList[i];

            for (var j = i + 1; j < updatedCollection.Count - 1 && j < lastMoveFromIndex; j++)
            {
                var updateItem = updatedCollection[j];

                if (comparison(prevItem, updateItem))
                { // found new shift
                    var indexDiff = (short)(j - i);
                    candidateRightShiftFirstCommands.ChangeLastCommandShiftBy((short)-indexDiff);
                    candidateRightShiftFirstCommands.AppendShiftAwayFromIndex((short)(i - 1), indexDiff);
                    lastMoveFromIndex = j;
                    break;
                }
            }
        }

        candidateLeftShiftFirstCommands.Clear();
        var appliedShiftLeft = 0;
        for (var i = 0; i < shiftedList.Count; i++)
        {
            var prevItem = shiftedList[i];


            for (var j = Math.Min(i + appliedShiftLeft - 1, updatedCollection.Count - 1); j >= 0; j--)
            {
                var updateItem = updatedCollection[j];

                if (comparison(prevItem, updateItem) && (j - i) < appliedShiftLeft)
                { // found new shift
                    var indexDiff = (short)(j - i - appliedShiftLeft);
                    candidateLeftShiftFirstCommands.AppendShiftTowardFromIndex((short)(j - 1), indexDiff);
                    appliedShiftLeft += indexDiff;
                    break;
                }
            }
        }

        exitingMoves.Clear();
        var shiftRightSingleElementMoves = 0;
        var shiftRightCommandCount       = candidateRightShiftFirstCommands.Count;
        if (shiftRightCommandCount > 0)
        { // check for elements that may have been moved left
            for (var i = shiftedList.Count - 1; i >= 0; i--)
            {
                var prevItem = shiftedList[i];

                for (var j = updatedCollection.Count - 1; j >= 0; j--)
                {
                    var updateItem        = updatedCollection[j];
                    var finalShiftApplied = candidateRightShiftFirstCommands.Where(lsc => lsc.FromIndex < i).Sum(lsc => lsc.Shift);

                    if (comparison(prevItem, updateItem) && (i + finalShiftApplied) != j)
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
                        candidateRightShiftFirstCommands.AppendSwapMoveSingleElement((short)(updatedOrigin + hasLowerIndexInserts), (short)j);
                        exitingMoves.Add((From: (short)(updatedOrigin + hasLowerIndexInserts), To: (short)j));
                        break;
                    }
                }
            }
        }
        
        exitingMoves.Clear();
        var shiftLeftSingleElementMoves = 0;
        var shiftLeftCommandCount       = candidateLeftShiftFirstCommands.Count;
        if (shiftLeftCommandCount > 0)
        { // check for elements that may have been moved left
            for (var i = 0; i < shiftedList.Count; i++)
            {
                var prevItem = shiftedList[i];

                for (var j = 0; j < updatedCollection.Count; j++)
                {
                    var updateItem        = updatedCollection[j];
                    var finalShiftApplied = candidateLeftShiftFirstCommands.Where(lsc => lsc.FromIndex < j).Sum(lsc => lsc.Shift);

                    if (comparison(prevItem, updateItem) && (i + finalShiftApplied) != j)
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
                        candidateLeftShiftFirstCommands.AppendSwapMoveSingleElement((short)(updatedOrigin + hasLowerIndexInserts), (short)j);
                        exitingMoves.Add((From: (short)(updatedOrigin + hasLowerIndexInserts), To: (short)j));
                        break;
                    }
                }
            }
        }

        if (shiftLeftCommandCount > shiftRightCommandCount &&
            (shiftLeftSingleElementMoves < shiftRightSingleElementMoves || shiftRightCommandCount == 0))
        {
            ShiftCommands.AddRange(candidateLeftShiftFirstCommands);
            return;
        }
        if (shiftLeftCommandCount <= shiftRightCommandCount || shiftRightSingleElementMoves > shiftLeftSingleElementMoves ||
            candidateRightShiftFirstCommands.Any())
        {
            ShiftCommands.AddRange(candidateRightShiftFirstCommands);
            return;
        }
        if (candidateLeftShiftFirstCommands.Any())
        {
            ShiftCommands.AddRange(candidateLeftShiftFirstCommands);
        }
    }

    public ListShiftCommand InsertAtStart(T toInsertAtStart)
    {
        if (ShiftCommands.LastShiftIsShiftRightFromStart())
        {
            ShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AddShiftRightFromStart();
        }
        shiftedList.Insert(0, toInsertAtStart);
        PostInsertCheck();
        return ShiftCommands.LastShift();
    }

    private void PostInsertCheck()
    {
        while (shiftedList.Count > CachedMaxCount)
        {
            var toRecycle = shiftedList[^1];
            shiftedList.RemoveAt(shiftedList.Count - 1);
            toRecycle.DecrementRefCount();
        }
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ListElementShiftRegistry<T, TCompare>)source, copyMergeFlags);

    public ListElementShiftRegistry<T, TCompare> CopyFrom
        (ListElementShiftRegistry<T, TCompare> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ShiftCommands = source.ShiftCommands.ToList();
        return this;
    }

    public IEnumerable<ListShiftCommand> CopyFrom(IEnumerable<ListShiftCommand> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ShiftCommands.Clear();
        ShiftCommands.AddRange(source);
        return ShiftCommands;
    }

    public ListShiftCommand InsertAt(int index, T toInsertAtStart)
    {
        if (index == 0)
        {
            return InsertAtStart(toInsertAtStart);
        }
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null && lastCommand.Value.FromIndex == index - 1)
        {
            ShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftAwayFromIndex((short)(index - 1), 1);
        }
        shiftedList.Insert(index, toInsertAtStart);
        PostInsertCheck();
        return ShiftCommands.LastShift();
    }

    public ListShiftCommand DeleteAt(int index)
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == index
         && lastCommand.Value.ShiftCommandType == RemoveElementsRange)
        {
            ShiftCommands.ChangeLastCommandShiftBy(1);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendRemoveRangeFromIndex((short)index, 1);
        }
        shiftedList.RemoveAt(index);
        return ShiftCommands.LastShift();
    }

    public ListShiftCommand ClearAll()
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand is not { FromIndex: ListShiftCommand.ClearAllPinnedIndex, Shift: ListShiftCommand.ClearAllShiftAmount })
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftAwayFromIndex(ListShiftCommand.ClearAllPinnedIndex, ListShiftCommand.ClearAllShiftAmount);
        }
        foreach (var lastTrade in shiftedList)
        {
            lastTrade.StateReset();
        }
        return ShiftCommands.LastShift();
    }

    public int IndexOfExisting(T existingItem)
    {
        var indexOfExisting = -1;
        for (int i = 0; i < shiftedList.Count; i++)
        {
            var checkItem = shiftedList[i];
            if (comparison(existingItem, checkItem))
            {
                indexOfExisting = i;
                break;
            }
        }
        return indexOfExisting;
    }

    public ListShiftCommand MoveToStart(T existingItem)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveToStart(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{shiftedList.JoinToString()}]");
    }

    public ListShiftCommand MoveToStart(int indexToMoveToStart)
    {
        if (indexToMoveToStart is < 0 || indexToMoveToStart >= shiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToStart} as it is outside the existing items. shiftedList:  [{shiftedList.JoinToString()}]");
        }
        if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
        ShiftCommands.AppendMoveSingleElement((short)indexToMoveToStart, 0);
        return ApplyElementShift(ShiftCommands.LastShift());
    }

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd)
    {
        if (indexToMoveToEnd is < 0 || indexToMoveToEnd >= shiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToEnd} as it is outside the existing items. shiftedList:  [{shiftedList.JoinToString()}]");
        }
        if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
        ShiftCommands.AppendMoveSingleElement((short)indexToMoveToEnd
                                            , (short)(shiftedList.Count - 1)); // list will shrink by one when element is removed so at
        // reinsert it will be shiftListCount.Count - 2

        return ApplyElementShift(ShiftCommands.LastShift());
    }

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift)
    {
        if (indexToMoveToEnd is < 0 || indexToMoveToEnd >= shiftedList.Count)
        {
            throw new
                ArgumentException($"Can not move an item at {indexToMoveToEnd} as it is outside the existing items. shiftedList:  [{shiftedList.JoinToString()}]");
        }
        var destinationIndex = Math.Clamp(indexToMoveToEnd + shift, 0, shiftedList.Capacity - 1);
        ShiftCommands.AppendMoveSingleElement((short)indexToMoveToEnd, (short)destinationIndex);
        return ApplyElementShift(ShiftCommands.LastShift());
    }

    public ListShiftCommand MoveSingleElementBy(T existingItem, int shift)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveSingleElementBy(indexOfExisting, shift);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{shiftedList.JoinToString()}]");
    }

    public ListShiftCommand MoveToEnd(T existingItem)
    {
        var indexOfExisting = IndexOfExisting(existingItem);
        if (indexOfExisting > 0)
        {
            return MoveToEnd(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {existingItem} in list to be moved. shiftedList:  [{shiftedList.JoinToString()}]");
    }

    public bool AppendAtEnd(T toAppendAtEnd)
    {
        if (shiftedList.Count < CachedMaxCount)
        {
            shiftedList[shiftedList.Count] = toAppendAtEnd;
            return true;
        }
        return false;
    }

    public ListShiftCommand Delete(T toDelete)
    {
        var indexOfExisting = IndexOfExisting(toDelete);
        if (indexOfExisting > 0)
        {
            return DeleteAt(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {toDelete} in list to be deleted. shiftedList:  [{shiftedList.JoinToString()}]");
    }
}
