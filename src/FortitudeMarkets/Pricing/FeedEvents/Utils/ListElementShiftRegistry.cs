using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Utils;

public class ListElementShiftRegistry<T>(IList<T> shiftedList, Func<T> newElementFactory) : ITrackableReset<ListElementShiftRegistry<T>>
    where T : IInterfacesComparable<T>, ITrackableReset<T>, IShowsEmpty
{
    public List<ElementShift> ShiftCommands = new();

    public ushort? ClearRemainingElementsAt
    {
        get => ShiftCommands
               .Where( es => es.ShiftAmount == ElementShift.ClearAllShiftAmount)
               .Select( es => (ushort)(es.PinnedElementsFromIndex + 1))
               .FirstOrDefault();
        set
        {
            var prevClearAt = ShiftCommands.FindIndex(es => es.ShiftAmount == ElementShift.ClearAllShiftAmount);
            if (prevClearAt >= 0)
            {
                ShiftCommands.RemoveAt(prevClearAt);
            }
            if (value == null) return;
            ShiftCommands.AppendShiftFromIndex((short)value, ElementShift.ClearAllShiftAmount);
        }
    }

    public bool HasRandomAccessUpdates { get; set; }
    public int  CachedMaxCount         { get; set; }

    public DateTime UpdateTime { get; set; }

    public T ElementAt(int index) => shiftedList[index];

    public ListElementShiftRegistry<T> ResetWithTracking()
    {
        ShiftCommands.Clear();
        return this;
    }

    public ElementShift ShiftElements(int byElements, int pinElementsFromIndex)
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null && lastCommand.Value.PinnedElementsFromIndex == pinElementsFromIndex &&
            Math.Sign(lastCommand.Value.ShiftAmount) == Math.Sign(byElements))
        {
            ShiftCommands.ChangeLastCommandShiftBy((short)byElements);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftFromIndex((short)pinElementsFromIndex, (short)byElements);
        }
        ApplyElementShift(new ElementShift((short)byElements, (short)pinElementsFromIndex));
        return ShiftCommands.LastShift();
    }

    public ElementShift ApplyElementShift(ElementShift shiftToApply)
    {
        var (byElements, pinElementsFromIndex) = shiftToApply;
        if (byElements < 0)
        {
            var insertEmptyIndex = Math.Min(pinElementsFromIndex - 2, shiftedList.Count - 2); // after element is removed it will be minus 2;
            for (var i = Math.Abs(byElements); i > 0; i--)
            {
                var lastTradeAt = shiftedList[0];
                lastTradeAt.ResetWithTracking();
                shiftedList.RemoveAt(0);
                if (insertEmptyIndex >= shiftedList.Count)
                {
                    shiftedList.Add(lastTradeAt);
                }
                else
                {
                    shiftedList.Insert(insertEmptyIndex, lastTradeAt);
                }
            }
        }
        if (byElements > 0)
        {
            var insertEmptyIndex = Math.Max(0, pinElementsFromIndex + 1);
            for (var i = insertEmptyIndex; i < pinElementsFromIndex + byElements; i++)
            {
                var lastElementCliffDeath    = i + byElements >= CachedMaxCount;
                var checkEndElementIndex     = Math.Min(shiftedList.Count - 1, i + byElements);
                var checkLastExistingElement = shiftedList[checkEndElementIndex];
                if (checkLastExistingElement.IsEmpty || lastElementCliffDeath)
                {
                    shiftedList.RemoveAt(shiftedList.Count - 1);
                    shiftedList.Insert(insertEmptyIndex, checkLastExistingElement);
                }
                else
                {
                    shiftedList.Insert(insertEmptyIndex, newElementFactory());
                }
            }
        }
        return shiftToApply;
    }

    public virtual void CalculateShift(DateTime asAtTime, IReadOnlyList<T> updatedCollection)
    {
        UpdateTime = asAtTime;
        if (ShiftCommands.Any() && HasRandomAccessUpdates)
        {
            ShiftCommands.Clear();
            HasRandomAccessUpdates = false;
        }
        // more recent entries appear at start
        // find the shift of the first entry in previous collection in the updated collection

        for (var i = 0; i < shiftedList.Count; i++)
        {
            var prevItem = shiftedList[i];

            for (var j = i; j < updatedCollection.Count; j++)
            {
                var updateItem = updatedCollection[j];

                if (prevItem?.AreEquivalent(updateItem) ?? false)
                { // found new shift
                    ShiftCommands.AppendShiftFromIndex(ElementShift.FromStartPinnedIndex, (short)(j - i));
                    return;
                }
            }
        }
        ClearAll();
    }

    public ElementShift InsertAtStart(T toInsertAtStart)
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
        return ShiftCommands.LastShift();
    }

    public ElementShift InsertAt(int index, T toInsertAtStart)
    {
        if (index == 0)
        {
            return InsertAtStart(toInsertAtStart);
        }
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null && lastCommand.Value.PinnedElementsFromIndex == index - 1)
        {
            ShiftCommands.IncrementLastCommandShift();
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftFromIndex((short)(index - 1), 1);
        }
        shiftedList.Insert(index, toInsertAtStart);
        return ShiftCommands.LastShift();
    }

    public ElementShift DeleteAt(int index)
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand != null && lastCommand.Value.PinnedElementsFromIndex == index - 1 &&
            Math.Sign(lastCommand.Value.ShiftAmount) == -1)
        {
            ShiftCommands.ChangeLastCommandShiftBy(-1);
        }
        else
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftFromIndex((short)(index - 1), -1);
        }
        shiftedList.RemoveAt(index);
        return ShiftCommands.LastShift();
    }

    public ElementShift ClearAll()
    {
        var lastCommand = ShiftCommands.SafeLastShift();
        if (lastCommand is not { PinnedElementsFromIndex: ElementShift.ClearAllPinnedIndex, ShiftAmount: ElementShift.ClearAllShiftAmount })
        {
            if (!ShiftCommands.Any()) HasRandomAccessUpdates = false;
            ShiftCommands.AppendShiftFromIndex(ElementShift.ClearAllPinnedIndex, ElementShift.ClearAllShiftAmount);
        }
        foreach (var lastTrade in shiftedList)
        {
            lastTrade.ResetWithTracking();
        }
        return ShiftCommands.LastShift();
    }
}
