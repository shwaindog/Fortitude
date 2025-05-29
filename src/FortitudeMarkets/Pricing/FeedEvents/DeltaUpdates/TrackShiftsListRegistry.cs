using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using static FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates.ListShiftCommandType;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

public class TrackShiftsListRegistry<TElement, TCompare> : ITrackableReset<TrackShiftsListRegistry<TElement, TCompare>>
  , ITransferState<TrackShiftsListRegistry<TElement, TCompare>>, ITransferState<IEnumerable<ListShiftCommand>>
  , IMutableTracksShiftsList<TElement, TCompare>
    where TElement : class, TCompare, ITrackableReset<TElement>, IReusableObject<TElement>, IShowsEmpty
{
    protected readonly IMutableCapacityList<TElement> ShiftedList;

    protected readonly Func<TElement> NewElementFactory;

    protected readonly Func<TCompare, TCompare, bool> Comparison;

    protected List<ListShiftCommand> RegisteredShiftCommands = new();

    protected readonly List<ListShiftCommand> CandidateRightShiftFirstCommands = new();
    protected readonly List<ListShiftCommand> CandidateLeftShiftFirstCommands  = new();
    private            bool                   isReadOnly;

    public TrackShiftsListRegistry
        (IMutableCapacityList<TElement> shiftedList, Func<TElement> newElementFactory, Func<TCompare, TCompare, bool> comparison)
    {
        ShiftedList       = shiftedList;
        NewElementFactory = newElementFactory;
        Comparison        = comparison;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get =>
            ShiftCommands
                .Where(es => es.Shift == ListShiftCommand.ClearAllShiftAmount)
                .Select(es => (ushort)(es.FromIndex + 1))
                .FirstOrDefault();
        set
        {
            var prevClearAt = RegisteredShiftCommands.FindIndex(es => es.Shift == ListShiftCommand.ClearAllShiftAmount);
            if (prevClearAt >= 0)
            {
                RegisteredShiftCommands.RemoveAt(prevClearAt);
            }
            if (value == null) return;
            RegisteredShiftCommands.AppendShiftTowardFromIndex((short)value, ListShiftCommand.ClearAllShiftAmount);
            ApplyElementShift(RegisteredShiftCommands.LastShift());
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => RegisteredShiftCommands;
        set => RegisteredShiftCommands = (List<ListShiftCommand>)value;
    }

    public ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd)
    {
        RegisteredShiftCommands.Append(toAppendAtEnd);
        return toAppendAtEnd;
    }

    public void ClearShiftCommands()
    {
        RegisteredShiftCommands.Clear();
    }

    public int Count
    {
        get => ShiftedList.Count;
        set => ShiftedList.Count = value;
    }

    public int Capacity
    {
        get => ShiftedList.Capacity;
        set => ShiftedList.Capacity = value;
    }

    public bool HasRandomAccessUpdates { get; set; }

    public ushort MaxAllowedSize { get; set; } = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;

    public TElement ElementAt(int index) => ShiftedList[index];

    public TElement this[int index]
    {
        get => ShiftedList[index];
        set => ShiftedList[index] = value;
    }

    public TrackShiftsListRegistry<TElement, TCompare> ResetWithTracking()
    {
        RegisteredShiftCommands.Clear();
        return this;
    }

    public ListShiftCommand ShiftElements(int byElements)
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        var pinElementsFromIndex = byElements > 0
            ? ListShiftCommand.ShiftRightFromStartAwayFromPinnedIndex
            : ListShiftCommand.ShiftLeftFromEndAwayFromPinnedIndex;
        if (lastCommand != null
         && lastCommand.Value.FromIndex == pinElementsFromIndex
         && Math.Sign(lastCommand.Value.Shift) == Math.Sign(byElements)
         && lastCommand.Value.ShiftCommandType == ShiftAllElementsAwayFromPinnedIndex)
        {
            RegisteredShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex(pinElementsFromIndex, (short)byElements);
        }
        ApplyElementShift(RegisteredShiftCommands.LastShift());
        return RegisteredShiftCommands.LastShift();
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
        if (originIndex >= ShiftedList.Count) return;
        var removeIndex = Math.Clamp(originIndex, 0, ShiftedList.Count);
        for (int i = 0; i < rangeToRemove; i++)
        {
            var lastTradeAt = ShiftedList[removeIndex];
            lastTradeAt.StateReset();
            ShiftedList.RemoveAt(removeIndex);
            ShiftedList.Add(lastTradeAt);
        }
    }

    private void ApplyMoveSingleElement(ListShiftCommand shiftCommandToApply)
    {
        var (destinationIndex, originIndex) = shiftCommandToApply;
        var lastTradeAt = ShiftedList[originIndex];
        ShiftedList.RemoveAt(originIndex);
        if (destinationIndex > ShiftedList.Capacity)
        {
            ShiftedList.Add(lastTradeAt);
        }
        else
        {
            if (shiftCommandToApply.IsSwapWithDestination())
            {
                var adjustedDestIndex = Math.Clamp(originIndex < destinationIndex ? destinationIndex - 1 : destinationIndex, 0
                                                 , ShiftedList.Capacity - 1);
                var adjustedOriginIndex = Math.Clamp(originIndex > destinationIndex ? originIndex - 1 : originIndex, 0, ShiftedList.Capacity - 1);
                var destinationElement  = ShiftedList[adjustedDestIndex];
                ShiftedList.RemoveAt(adjustedDestIndex);
                ShiftedList.Insert(adjustedOriginIndex, destinationElement);
            }
            ShiftedList.Insert(destinationIndex, lastTradeAt);
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
        var removeIndex = Math.Clamp(pinElementsFromIndex + 1, 0, MaxAllowedSize - 1);
        if (removeIndex >= ShiftedList.Count) return;
        var insertEmptyIndex = Math.Min(ShiftedList.Capacity - 1, MaxAllowedSize - 1);
        var shiftAmount = byElements == short.MinValue
            ? Math.Clamp(ShiftedList.Count - removeIndex, 0, ShiftedList.Count)
            : Math.Clamp(Math.Abs(byElements), 0, ShiftedList.Count - removeIndex);
        for (var i = 0; i < shiftAmount; i++)
        {
            var checkLastExistingElement = ShiftedList[removeIndex];
            ShiftedList.RemoveAt(removeIndex);
            checkLastExistingElement.StateReset();
            if (insertEmptyIndex >= ShiftedList.Capacity)
            {
                ShiftedList.Add(checkLastExistingElement);
            }
            else
            {
                ShiftedList.Insert(insertEmptyIndex, checkLastExistingElement);
            }
        }
    }

    private void ApplyShiftRightTowardPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // insert a new entry (check end for possible spare items at pinElementsFromIndex - 1) otherwise create a new entry at 0.
        var removeIndex = Math.Clamp(pinElementsFromIndex - 1, 0, MaxAllowedSize - 1);
        for (var i = byElements; i > 0; i--)
        {
            var lastTradeAt = removeIndex > ShiftedList.Capacity ? NewElementFactory() : ShiftedList[removeIndex];
            lastTradeAt.StateReset();
            ShiftedList.RemoveAt(removeIndex);
            ShiftedList.Insert(0, lastTradeAt);
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
        var insertEmptyIndex = Math.Clamp(pinElementsFromIndex - 1, 0, MaxAllowedSize - 1);
        var shiftAmount      = byElements == short.MinValue ? ShiftedList.Count : Math.Clamp(Math.Abs(byElements), 0, MaxAllowedSize - 1);
        for (var i = shiftAmount; i > 0; i--)
        {
            var lastTradeAt = ShiftedList[0];
            lastTradeAt.StateReset();
            ShiftedList.RemoveAt(0);
            if (insertEmptyIndex >= ShiftedList.Capacity)
            {
                ShiftedList.Add(lastTradeAt);
            }
            else
            {
                ShiftedList.Insert(insertEmptyIndex, lastTradeAt);
            }
        }
    }

    private void ApplyShiftRightAwayFromPinnedIndex(short pinElementsFromIndex, short byElements)
    {
        // insert a new entry (check end for possible spare items at end) otherwise create a new entry at pinElementsFromIndex + 1.
        var insertEmptyIndex = Math.Clamp(pinElementsFromIndex + 1, 0, ShiftedList.Capacity - 1);
        for (var i = insertEmptyIndex; i < insertEmptyIndex + byElements; i++)
        {
            var lastElementCliffDeath = i + byElements >= MaxAllowedSize;
            var checkEndElementIndex  = Math.Min(ShiftedList.Count, MaxAllowedSize - 1); // one past populated elements or very last allowed
            if (lastElementCliffDeath)
            {
                var checkLastExistingElement = ShiftedList[checkEndElementIndex];
                ShiftedList.RemoveAt(checkEndElementIndex);
                checkLastExistingElement.StateReset();
                if (insertEmptyIndex >= ShiftedList.Capacity)
                {
                    ShiftedList.Add(checkLastExistingElement);
                }
                else
                {
                    ShiftedList.Insert(insertEmptyIndex, checkLastExistingElement);
                }
            }
            else
            {
                ShiftedList.Insert(insertEmptyIndex, NewElementFactory());
            }
        }
        PostInsertCheck();
    }

    public virtual bool CalculateShift(DateTime asAtTime, IReadOnlyList<TCompare> updatedCollection)
    {
        if (ShiftCommands.Any() && !HasRandomAccessUpdates)
        {
            return false;
        }

        CandidateRightShiftFirstCommands.Clear();
        int lastMoveFromIndex = updatedCollection.Count;
        for (var i = ShiftedList.Count - 1; i >= 0; i--)
        {
            var prevItem = ShiftedList[i];

            for (var j = i + 1; j < updatedCollection.Count - 1 && j < lastMoveFromIndex; j++)
            {
                var updateItem = updatedCollection[j];

                if (Comparison(prevItem, updateItem))
                { // found new shift
                    var indexDiff = (short)(j - i);
                    CandidateRightShiftFirstCommands.ChangeLastCommandShiftBy((short)-indexDiff);
                    CandidateRightShiftFirstCommands.AppendShiftAwayFromIndex((short)(i - 1), indexDiff);
                    lastMoveFromIndex = j;
                    break;
                }
            }
        }

        CandidateLeftShiftFirstCommands.Clear();
        var appliedShiftLeft = 0;
        for (var i = 0; i < ShiftedList.Count; i++)
        {
            var prevItem = ShiftedList[i];


            for (var j = Math.Min(i + appliedShiftLeft - 1, updatedCollection.Count - 1); j >= 0; j--)
            {
                var updateItem = updatedCollection[j];

                if (Comparison(prevItem, updateItem) && (j - i) < appliedShiftLeft)
                { // found new shift
                    var indexDiff = (short)(j - i - appliedShiftLeft);
                    CandidateLeftShiftFirstCommands.AppendShiftTowardFromIndex((short)(j - 1), indexDiff);
                    appliedShiftLeft += indexDiff;
                    break;
                }
            }
        }

        var shiftRightCommandCount = CandidateRightShiftFirstCommands.Count;
        var shiftLeftCommandCount  = CandidateLeftShiftFirstCommands.Count;


        if (shiftLeftCommandCount > shiftRightCommandCount || shiftRightCommandCount == 0)
        {
            RegisteredShiftCommands.AddRange(CandidateLeftShiftFirstCommands);
            return true;
        }
        if (shiftLeftCommandCount <= shiftRightCommandCount || CandidateRightShiftFirstCommands.Any())
        {
            RegisteredShiftCommands.AddRange(CandidateRightShiftFirstCommands);
            return true;
        }
        if (CandidateLeftShiftFirstCommands.Any())
        {
            RegisteredShiftCommands.AddRange(CandidateLeftShiftFirstCommands);
        }
        return true;
    }

    public ListShiftCommand InsertAtStart(TElement toInsertAtStart)
    {
        if (RegisteredShiftCommands.LastShiftIsShiftRightFromStart())
        {
            RegisteredShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!RegisteredShiftCommands.Any()) HasRandomAccessUpdates = false;
            RegisteredShiftCommands.AddShiftRightFromStart();
        }
        ShiftedList.Insert(0, toInsertAtStart);
        PostInsertCheck();
        return RegisteredShiftCommands.LastShift();
    }

    protected void PostInsertCheck()
    {
        while (ShiftedList.Count > MaxAllowedSize)
        {
            var toRecycle = ShiftedList[^1];
            ShiftedList.RemoveAt(ShiftedList.Count - 1);
            toRecycle.DecrementRefCount();
        }
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((TrackShiftsListRegistry<TElement, TCompare>)source, copyMergeFlags);

    public virtual TrackShiftsListRegistry<TElement, TCompare> CopyFrom
        (TrackShiftsListRegistry<TElement, TCompare> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ShiftCommands = source.ShiftCommands.ToList();
        return this;
    }

    public IEnumerable<ListShiftCommand> CopyFrom(IEnumerable<ListShiftCommand> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        RegisteredShiftCommands.Clear();
        RegisteredShiftCommands.AddRange(source);
        return RegisteredShiftCommands;
    }

    public ListShiftCommand InsertAt(int index, TElement toInsertAtStart)
    {
        if (index == 0)
        {
            return InsertAtStart(toInsertAtStart);
        }
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand != null && lastCommand.Value.FromIndex == index - 1)
        {
            RegisteredShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!RegisteredShiftCommands.Any()) HasRandomAccessUpdates = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex((short)(index - 1), 1);
        }
        ShiftedList.Insert(index, toInsertAtStart);
        PostInsertCheck();
        return RegisteredShiftCommands.LastShift();
    }

    public ListShiftCommand DeleteAt(int index)
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand != null
         && lastCommand.Value.FromIndex == index
         && lastCommand.Value.ShiftCommandType == RemoveElementsRange)
        {
            RegisteredShiftCommands.ChangeLastCommandShiftBy(1);
        }
        else
        {
            if (!RegisteredShiftCommands.Any()) HasRandomAccessUpdates = false;
            RegisteredShiftCommands.AppendRemoveRangeFromIndex((short)index, 1);
        }
        ShiftedList.RemoveAt(index);
        return RegisteredShiftCommands.LastShift();
    }

    public ListShiftCommand ClearAll()
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand is not { FromIndex: ListShiftCommand.ClearAllPinnedIndex, Shift: ListShiftCommand.ClearAllShiftAmount })
        {
            if (!RegisteredShiftCommands.Any()) HasRandomAccessUpdates = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex(ListShiftCommand.ClearAllPinnedIndex, ListShiftCommand.ClearAllShiftAmount);
        }
        foreach (var lastTrade in ShiftedList)
        {
            lastTrade.StateReset();
        }
        return RegisteredShiftCommands.LastShift();
    }

    public int IndexOfExisting(TElement existingItem)
    {
        var indexOfExisting = -1;
        for (int i = 0; i < ShiftedList.Count; i++)
        {
            var checkItem = ShiftedList[i];
            if (Comparison(existingItem, checkItem))
            {
                indexOfExisting = i;
                break;
            }
        }
        return indexOfExisting;
    }

    public bool AppendAtEnd(TElement toAppendAtEnd)
    {
        if (ShiftedList.Count < MaxAllowedSize)
        {
            ShiftedList[ShiftedList.Count] = toAppendAtEnd;
            return true;
        }
        return false;
    }

    public ListShiftCommand Delete(TElement toDelete)
    {
        var indexOfExisting = IndexOfExisting(toDelete);
        if (indexOfExisting > 0)
        {
            return DeleteAt(indexOfExisting);
        }
        throw new ArgumentException($"Could not find item {toDelete} in list to be deleted. shiftedList:  [{ShiftedList.JoinToString()}]");
    }

    void ICollection<TElement>.Add(TElement item) => ShiftedList.Add(item);

    void ICollection<TElement>.Clear()                 => ShiftedList.Clear();

    bool ICollection<TElement>.Contains(TElement item) => ShiftedList.Contains(item);

    void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
    {
        for (int i = 0; i < ShiftedList.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = ShiftedList[i];
        }
    }

    bool ICollection<TElement>.Remove(TElement item) => ShiftedList.Remove(item);

    bool ICollection<TElement>.IsReadOnly => ShiftedList.IsReadOnly;

    int IList<TElement>. IndexOf(TElement item) => ShiftedList.IndexOf(item);

    void IList<TElement>.Insert(int index, TElement item) => ShiftedList.Insert(index, item);

    void IList<TElement>.RemoveAt(int index) => ShiftedList.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => ShiftedList.GetEnumerator();

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator() => ShiftedList.GetEnumerator();
}
