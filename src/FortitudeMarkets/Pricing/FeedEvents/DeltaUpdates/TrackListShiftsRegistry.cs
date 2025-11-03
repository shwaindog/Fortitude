using System.Collections;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using static FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates.ListShiftCommandType;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

public class TrackListShiftsRegistry<TElement, TCompare> : ITrackableReset<TrackListShiftsRegistry<TElement, TCompare>>
  , ITransferState<IEnumerable<ListShiftCommand>>, IMutableTracksShiftsList<TElement, TCompare>
    where TElement : class, TCompare, ITrackableReset<TElement>, IRecyclableObject, IShowsEmpty
{
    protected readonly IMutableTracksShiftsList<TElement, TCompare> ShiftedList;

    protected readonly Func<TElement> NewElementFactory;

    protected readonly Func<TCompare, TCompare, bool> Comparison;

    protected List<ListShiftCommand> RegisteredShiftCommands = new();

    private int? previousCopyHashCode;

    protected readonly List<ListShiftCommand> CandidateRightShiftFirstCommands = new();
    protected readonly List<ListShiftCommand> CandidateLeftShiftFirstCommands  = new();

    public TrackListShiftsRegistry
        (IMutableTracksShiftsList<TElement, TCompare> shiftedList, Func<TElement> newElementFactory, Func<TCompare, TCompare, bool> comparison)
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
            RegisteredShiftCommands.AppendClearRangeFromIndex((short)value, (short)Math.Max(0, ShiftedList.Count - value.Value));
            ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
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

    public bool IsEmpty
    {
        get => !RegisteredShiftCommands.Any();
        set
        {
            if (value) RegisteredShiftCommands.Clear();
        }
    }

    public bool HasUnreliableListTracking { get; set; }

    public ushort MaxAllowedSize
    {
        get => ShiftedList.MaxAllowedSize;
        set => ShiftedList.MaxAllowedSize = value;
    }

    public TElement ElementAt(int index) => ShiftedList[index];

    public TElement this[int index]
    {
        get => ShiftedList[index];
        set
        {
            if(ReferenceEquals(value, ShiftedList[index] )) return;
            var prevHasUnreliableTracking = HasUnreliableListTracking;
            ShiftedList[index]     = value;
            HasUnreliableListTracking = prevHasUnreliableTracking;
        }
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
            if (!ShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex(pinElementsFromIndex, (short)byElements);
        }
        ApplyListShiftCommand(RegisteredShiftCommands.LastShift());
        return RegisteredShiftCommands.LastShift();
    }

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply)
    {
        var hasUnreliableListTracking = HasUnreliableListTracking;
        switch (shiftCommandToApply.ShiftCommandType)
        {
            case ShiftAllElementsAwayFromPinnedIndex: ApplyShiftAwayFromPinnedIndex(shiftCommandToApply); break;
            case ShiftAllElementsTowardPinnedIndex:   ApplyShiftTowardPinnedIndex(shiftCommandToApply); break;
            case MoveSingleElement:
            case MoveSingleElement
               | InsertElementsRange:
                ApplyMoveSingleElement(shiftCommandToApply);
                break;
            case RemoveElementsRange: 
            case RemoveElementsRange
                | InsertElementsRange: ApplyClearOrDeleteElements(shiftCommandToApply); break;
            default:                  throw new ArgumentException($"Currently don't handle {shiftCommandToApply.ShiftCommandType}");
        }
        HasUnreliableListTracking = hasUnreliableListTracking;

        return shiftCommandToApply;
    }

    private void ApplyClearOrDeleteElements(ListShiftCommand shiftCommandToApply)
    {
        var (rangeToRemove, originIndex) = shiftCommandToApply;
        if (originIndex >= ShiftedList.Count) return;
        var removeIndex = Math.Clamp(originIndex, 0, ShiftedList.Count);
        for (int i = removeIndex; i < removeIndex + rangeToRemove; i++)
        {
            var elementToDelete = ShiftedList[i];
            elementToDelete.StateReset();
            if (!shiftCommandToApply.IsJustClearNotRemove())
            {
                RemoveAt(removeIndex);
                Add(elementToDelete);
                i--;
                rangeToRemove--;
            }
        }
    }

    private void ApplyMoveSingleElement(ListShiftCommand shiftCommandToApply)
    {
        var (destinationIndex, originIndex) = shiftCommandToApply;
        var shiftListCount = ShiftedList.Count;
        if (destinationIndex >= shiftListCount && originIndex >= shiftListCount) return;
        var lastTradeAt    = ShiftedList[originIndex];
        RemoveAt(originIndex);
        if (destinationIndex > ShiftedList.Capacity)
        {
            Add(lastTradeAt);
        }
        else
        {
            if (shiftCommandToApply.IsSwapWithDestination())
            {
                var adjustedDestIndex = Math.Clamp(originIndex < destinationIndex ? destinationIndex - 1 : destinationIndex, 0
                                                 , shiftListCount - 1);
                var adjustedOriginIndex = Math.Clamp(originIndex > destinationIndex ? originIndex - 1 : originIndex, 0, ShiftedList.Capacity - 1);
                var destinationElement  = ShiftedList[adjustedDestIndex];
                RemoveAt(adjustedDestIndex);
                if (adjustedDestIndex < ShiftedList.Capacity)
                {
                    Insert(adjustedOriginIndex, destinationElement);
                }
                else
                {
                    Add(destinationElement);
                }
            }
            Insert(destinationIndex, lastTradeAt);
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
            RemoveAt(removeIndex);
            checkLastExistingElement.StateReset();
            if (insertEmptyIndex >= ShiftedList.Capacity)
            {
                Add(checkLastExistingElement);
            }
            else
            {
                Insert(insertEmptyIndex, checkLastExistingElement);
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
            RemoveAt(removeIndex);
            Insert(0, lastTradeAt);
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
            RemoveAt(0);
            if (insertEmptyIndex >= ShiftedList.Capacity)
            {
                Add(lastTradeAt);
            }
            else
            {
                Insert(insertEmptyIndex, lastTradeAt);
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
                RemoveAt(checkEndElementIndex);
                checkLastExistingElement.StateReset();
                if (insertEmptyIndex >= ShiftedList.Capacity)
                {
                    Add(checkLastExistingElement);
                }
                else
                {
                    Insert(insertEmptyIndex, checkLastExistingElement);
                }
            }
            else
            {
                Insert(insertEmptyIndex, NewElementFactory());
            }
        }
        PostInsertCheck();
    }

    public virtual bool CalculateShift(DateTime asAtTime, IReadOnlyList<TCompare> updatedCollection)
    {
        if (ShiftCommands.Any() && !HasUnreliableListTracking)
        {
            return false;
        }
        HasUnreliableListTracking = false;
        RegisteredShiftCommands.Clear();
        CandidateRunRightShiftFirstCommands(updatedCollection);

        CandidateRunLeftShiftFirstCommands(updatedCollection);

        var shiftRightCommandCount = CandidateRightShiftFirstCommands.Count;
        var shiftLeftCommandCount  = CandidateLeftShiftFirstCommands.Count;


        if (shiftLeftCommandCount > shiftRightCommandCount && CountLeftShiftFirstUnmoved < CountLeftShiftFirstMovedRescued)
        {
            RegisteredShiftCommands.AddRange(CandidateLeftShiftFirstCommands);
            return true;
        }
        if (shiftLeftCommandCount <= shiftRightCommandCount && CountRightShiftFirstUnmoved < CountRightShiftFirstMovedRescued)
        {
            RegisteredShiftCommands.AddRange(CandidateRightShiftFirstCommands);
            return true;
        }
        return true;
    }
    
    protected int CountLeftShiftFirstUnmoved;
    protected int CountLeftShiftFirstMovedRescued;

    protected void CandidateRunLeftShiftFirstCommands(IReadOnlyList<TCompare> updatedCollection)
    {
        CandidateLeftShiftFirstCommands.Clear();
        CountLeftShiftFirstUnmoved      = 0;
        CountLeftShiftFirstMovedRescued = 0;
        var isFirstShift           = true;
        var appliedShiftLeft       = 0;
        int highestMatchedPrevious = 0;
        for (var i = 0; i < ShiftedList.Count; i++)
        {
            var prevItem = ShiftedList[i];
            for (var j = Math.Min(i, updatedCollection.Count - 1); j >= 0; j--)
            {
                var updateItem = updatedCollection[j];
                if (Comparison(prevItem, updateItem))
                {
                    if (j == i) CountLeftShiftFirstUnmoved++;
                    if (isFirstShift && j > 0 && i > 0)
                    {
                        CandidateLeftShiftFirstCommands.AppendClearRangeFromIndex(0, (short)i);
                    }
                    isFirstShift     =  false;
                    var indexDiff = (short)(j - i - appliedShiftLeft);
                    if (indexDiff <= 0 && j != i) CountLeftShiftFirstMovedRescued++;
                    if (indexDiff < 0)
                    {
                        CandidateLeftShiftFirstCommands.AppendShiftTowardFromIndex((short)(j - 1), indexDiff);
                        appliedShiftLeft += indexDiff;
                    }
                    highestMatchedPrevious =  i;
                    break;
                }
            }
        }
        if (!isFirstShift && highestMatchedPrevious + 1 < ShiftedList.Count)
        {
            int currentRunLength = 0;
            for (int i = highestMatchedPrevious + 1; i < ShiftedList.Count; i++)
            {
                var checkElement = ShiftedList[i];
                if(updatedCollection.All( uce => !Comparison(checkElement, uce)))
                {
                    currentRunLength++;
                }
                else
                {
                    if (currentRunLength > 0)
                    {
                        CandidateLeftShiftFirstCommands.AppendClearRangeFromIndex((short)(i + appliedShiftLeft - currentRunLength), (short)(currentRunLength));
                    }
                    currentRunLength = 0;
                }
            }
            if (currentRunLength > 0)
            {
                CandidateLeftShiftFirstCommands.AppendClearRangeFromIndex((short)(ShiftedList.Count + appliedShiftLeft - currentRunLength), (short)(currentRunLength));
            }
        }

        // Only RightShiftsFirstCommands can have potential readjusted 0 shifts
    }

    protected int CountRightShiftFirstUnmoved;
    protected int CountRightShiftFirstMovedRescued;

    protected void CandidateRunRightShiftFirstCommands(IReadOnlyList<TCompare> updatedCollection)
    {
        CandidateRightShiftFirstCommands.Clear();
        CountRightShiftFirstUnmoved      = 0;
        CountRightShiftFirstMovedRescued = 0;
        var isFirstShift          = true;
        int lastMoveFromIndex     = updatedCollection.Count;
        int lowestMatchedPrevious = updatedCollection.Count;
        for (var i = ShiftedList.Count - 1; i >= 0; i--)
        {
            var prevItem = ShiftedList[i];

            for (var j = i; j < updatedCollection.Count && j < lastMoveFromIndex; j++)
            {
                var updateItem = updatedCollection[j];

                if (Comparison(prevItem, updateItem))
                { // found new shift
                    if (j == i) CountRightShiftFirstUnmoved++;
                    var indexDiff = (short)(j - i);
                    if (isFirstShift && i + 1 < ShiftedList.Count)
                    {
                        CandidateRightShiftFirstCommands.AppendClearRangeFromIndex((short)(i + 1), (short)(ShiftedList.Count - i - 1));
                    }
                    isFirstShift      = false;
                    if (indexDiff > 0)
                    {
                        CountRightShiftFirstMovedRescued++;
                        CandidateRightShiftFirstCommands.ChangeLastCommandShiftBy((short)-indexDiff);
                        CandidateRightShiftFirstCommands.AppendShiftAwayFromIndex((short)(i - 1), indexDiff);

                        lastMoveFromIndex = j;
                    }
                    lowestMatchedPrevious = i;
                    break;
                }
            }
        }
        if (!isFirstShift && lowestMatchedPrevious > 0)
        {
            int currentRunLength = 0;
            for (int i = 0; i < lowestMatchedPrevious; i++)
            {
                var checkElement = ShiftedList[i];
                if(updatedCollection.All( uce => !Comparison(checkElement, uce)))
                {
                    currentRunLength++;
                }
                else
                {
                    if (currentRunLength > 0)
                    {
                        CandidateRightShiftFirstCommands.AppendClearRangeFromIndex((short)(i - currentRunLength), (short)(currentRunLength));
                    }
                    currentRunLength = 0;
                }
            }
        }
        for (var i = 0; i < CandidateRightShiftFirstCommands.Count; i++)
        {
            var rightShift = CandidateRightShiftFirstCommands[i];
            if (rightShift.IsShiftLeftOrRightCommand() && rightShift.Shift <= 0)
            {
                CandidateRightShiftFirstCommands.RemoveAt(i);
                i--;
            }
        }
    }

    public ListShiftCommand InsertAtStart(TElement toInsertAtStart)
    {
        if (RegisteredShiftCommands.LastShiftIsShiftRightFromStart())
        {
            RegisteredShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!RegisteredShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AddShiftRightFromStart();
        }
        Insert(0, toInsertAtStart);
        PostInsertCheck();
        return RegisteredShiftCommands.LastShift();
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
            if (!RegisteredShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex((short)(index - 1), 1);
        }
        Insert(index, toInsertAtStart);
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
            if (!RegisteredShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendRemoveRangeFromIndex((short)index, 1);
        }
        RemoveAt(index);
        return RegisteredShiftCommands.LastShift();
    }

    public ListShiftCommand ClearAll()
    {
        var lastCommand = RegisteredShiftCommands.SafeLastShift();
        if (lastCommand is not { FromIndex: ListShiftCommand.ClearAllPinnedIndex, Shift: ListShiftCommand.ClearAllShiftAmount })
        {
            if (!RegisteredShiftCommands.Any()) HasUnreliableListTracking = false;
            RegisteredShiftCommands.AppendShiftAwayFromIndex(ListShiftCommand.ClearAllPinnedIndex, ListShiftCommand.ClearAllShiftAmount);
        }
        Clear();
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
        if (Count < MaxAllowedSize)
        {
            if (ShiftedList.Count < ShiftedList.Capacity)
            {
                this[Count] = toAppendAtEnd;
            }
            else
            {
                var prevHasUnreliableListTracking = HasUnreliableListTracking;
                ShiftedList.Add(toAppendAtEnd);
                HasUnreliableListTracking = prevHasUnreliableListTracking;
            }
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

    public void Add(TElement item)
    {
        var prevHasRandomAccessUpdates = HasUnreliableListTracking;
        ShiftedList.Add(item);
        HasUnreliableListTracking = prevHasRandomAccessUpdates;
    }

    public void Clear()
    {
        ShiftedList.Clear();
        HasUnreliableListTracking = false;
    }

    bool ICollection<TElement>.Contains(TElement item) => ShiftedList.Contains(item);

    void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
    {
        for (int i = 0; i < ShiftedList.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = ShiftedList[i];
        }
    }

    public bool Remove(TElement item)
    {
        var prevHasRandomAccessUpdates = HasUnreliableListTracking;
        var result = ShiftedList.Remove(item);
        HasUnreliableListTracking = prevHasRandomAccessUpdates;
        return result;
    }

    bool ICollection<TElement>.IsReadOnly => ShiftedList.IsReadOnly;

    int IList<TElement>.IndexOf(TElement item) => ShiftedList.IndexOf(item);

    public void Insert(int index, TElement item)
    {
        var prevHasRandomAccessUpdates = HasUnreliableListTracking;
        ShiftedList.Insert(index, item);
        HasUnreliableListTracking = prevHasRandomAccessUpdates;
    }

    public void RemoveAt(int index)
    {
        var prevHasRandomAccessUpdates = HasUnreliableListTracking;
        ShiftedList.RemoveAt(index);
        HasUnreliableListTracking = prevHasRandomAccessUpdates;
    }

    IEnumerator IEnumerable.GetEnumerator() => ShiftedList.GetEnumerator();

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator() => ShiftedList.GetEnumerator();

    protected void PostInsertCheck()
    {
        while (ShiftedList.Count > MaxAllowedSize)
        {
            var toRecycle = ShiftedList[^1];
            RemoveAt(ShiftedList.Count - 1);
            toRecycle.DecrementRefCount();
        }
    }

    public virtual IMutableTracksShiftsList<TElement, TCompare> CopyFrom
        (IReadOnlyList<TCompare> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var isFullReplace = copyMergeFlags.HasFullReplace();
        var wasEmpty      = ShiftedList.All(e => e.IsEmpty);
        if (source is IMutableTracksShiftsList<TElement, TCompare> sourceTracksShifts)
        {
            if (isFullReplace) ClearShiftCommands();
            var currentHashCode = ShiftedList.GetHashCode();
            if (previousCopyHashCode != null)
            {
                HasUnreliableListTracking = previousCopyHashCode.Value != currentHashCode;
            }
            previousCopyHashCode = currentHashCode;
            if (sourceTracksShifts.ShiftCommands.Any() && (isFullReplace || !HasUnreliableListTracking && !sourceTracksShifts.HasUnreliableListTracking))
            {
                HasUnreliableListTracking = sourceTracksShifts.HasUnreliableListTracking;
                CopyFrom(sourceTracksShifts.ShiftCommands, CopyMergeFlags.KeepCachedItems);
                if (!wasEmpty)
                {
                    foreach (var applyShifts in sourceTracksShifts.ShiftCommands)
                    {
                        ApplyListShiftCommand(applyShifts);
                    }
                }
            }
            else
            {
                if (!wasEmpty)
                {
                    CalculateShift(TimeContext.UtcNow, sourceTracksShifts);
                    foreach (var applyShifts in ShiftCommands)
                    {
                        ApplyListShiftCommand(applyShifts);
                    }
                }
                else
                {
                    ClearShiftCommands(); // just in case
                }
            }
        }
        return ShiftedList;
    }

    ITransferState ITransferState.CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IReadOnlyList<TCompare> compareList)
        {
            CopyFrom(compareList, copyMergeFlags);
        } else if (source is IEnumerable<ListShiftCommand> listCommandEnum)
        {
            CopyFrom(listCommandEnum, copyMergeFlags);
        }
        return this;
    }

    public IEnumerable<ListShiftCommand> CopyFrom(IEnumerable<ListShiftCommand> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!copyMergeFlags.HasKeepCachedItems()) RegisteredShiftCommands.Clear();
        RegisteredShiftCommands.AddRange(source);
        return RegisteredShiftCommands;
    }

    ITracksResetCappedCapacityList<TElement> ITrackableReset<ITracksResetCappedCapacityList<TElement>>.ResetWithTracking() => ResetWithTracking();

    public TrackListShiftsRegistry<TElement, TCompare> ResetWithTracking()
    {
        RegisteredShiftCommands.Clear();
        HasUnreliableListTracking = false;
        return this;
    }

    public void StateReset()
    {
        RegisteredShiftCommands.Clear();
        HasUnreliableListTracking = false;
    }
}
