using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents;

public readonly struct ElementShift(short shiftAmount, short pinnedElementsFromIndex = 0)
{
    public const short FromStartPinnedIndex = -1;
    public const short FromEndPinnedIndex   = short.MaxValue;
    public const short ClearAllPinnedIndex  = short.MaxValue;
    public const short ClearAllShiftAmount  = short.MinValue;

    public short ShiftAmount             { get; } = shiftAmount;
    public short PinnedElementsFromIndex { get; } = pinnedElementsFromIndex;


    public void Deconstruct(out short shiftAmount, out short pinnedElementFromIndex)
    {
        shiftAmount            = ShiftAmount;
        pinnedElementFromIndex = PinnedElementsFromIndex;
    }
}

public static class ElementShiftExtensions
{
    public static bool LastShiftIsShiftRightFromStart(this IList<ElementShift> shiftCommands)
    {
        var lastCommand = shiftCommands.LastShift();
        if (lastCommand is { PinnedElementsFromIndex: ElementShift.FromStartPinnedIndex, ShiftAmount: > 0 }) // last command was shift right
        {
            return true;
        }
        return false;
    }

    public static bool HasLastShift(this IList<ElementShift> shiftCommands) => shiftCommands.Any();

    public static ElementShift? SafeLastShift(this IList<ElementShift> shiftCommand)
    {
        if (shiftCommand.HasLastShift())
        {
            var lastAddedCommand = shiftCommand[^1];
            return lastAddedCommand;
        }
        return null;
    }

    public static ElementShift LastShift(this IList<ElementShift> shiftCommand)
    {
        if (shiftCommand.HasLastShift())
        {
            var lastAddedCommand = shiftCommand[^1];
            return lastAddedCommand;
        }
        throw new ArgumentException("No last shift commands exist");
    }

    public static bool ReplaceLastShift(this IList<ElementShift> shiftCommands, ElementShift replaceWith)
    {
        if (shiftCommands.HasLastShift())
        {
            shiftCommands[^1] = replaceWith;
            return true;
        }
        return false;
    }

    public static bool AddShiftRightFromStart(this IList<ElementShift> shiftCommands, short amount = 1)
    {
        shiftCommands.Add(new ElementShift(amount, -1));
        return false;
    }

    public static bool AppendShiftFromIndex(this IList<ElementShift> shiftCommands, short fromIndex, short amount)
    {
        shiftCommands.Add(new ElementShift(amount, fromIndex));
        return false;
    }

    public static bool IncrementLastCommandShift(this IList<ElementShift> shiftCommands)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().IncrementShift());
    }

    public static bool DecrementLastCommandShift(this IList<ElementShift> shiftCommands)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().DecrementShift());
    }

    public static bool ChangeLastCommandShiftBy(this IList<ElementShift> shiftCommands, short amountToChange)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().AddToShift(amountToChange));
    }

    public static bool LastShiftIsShiftLeftFromEnd(this IList<ElementShift> shiftCommands)
    {
        var lastCommand = shiftCommands.LastShift();
        if (lastCommand is { PinnedElementsFromIndex: ElementShift.FromEndPinnedIndex, ShiftAmount: < 0 }) // last command was shift right
        {
            return true;
        }
        return false;
    }

    public static ElementShift AddToShift(this ElementShift toIncrement, short amountToAdd) =>
        new((short)(toIncrement.ShiftAmount + amountToAdd), toIncrement.PinnedElementsFromIndex);

    public static ElementShift IncrementShift(this ElementShift toIncrement) =>
        new((short)(toIncrement.ShiftAmount + 1), toIncrement.PinnedElementsFromIndex);

    public static ElementShift DecrementShift(this ElementShift toIncrement) =>
        new((short)(toIncrement.ShiftAmount + 1), toIncrement.PinnedElementsFromIndex);
}

public interface IShiftable<in T> { }

public interface ISupportsElementsShift<in T, out TElement> : IShiftable<T>
{
    IReadOnlyList<ElementShift> ElementShifts { get; }

    ushort? ClearedElementsAfterIndex { get; }
    bool    HasRandomAccessUpdates    { get; }

    TElement this[int index] { get; }

}

public interface IMutableSupportsElementsShift<in T, TElement> : IShiftable<T> where T : ITrackableReset<T>
{
    IReadOnlyList<ElementShift> ElementShifts { get; set; }

    ushort? ClearedElementsAfterIndex { get; set; }
    bool    HasRandomAccessUpdates    { get; set; }

    TElement this[int index] { get; set; }

    ElementShift InsertAtStart(TElement toInsertAtStart);
    ElementShift InsertAt(int index, TElement toInsertAtStart);
    ElementShift DeleteAt(int index);

    ElementShift ApplyElementShift(ElementShift shiftToApply);

    ElementShift ClearAll();

    ElementShift ShiftElements(int byElements, int pinElementsFromIndex);

    void         CalculateShift(DateTime asAtTime, IReadOnlyList<TElement> updatedCollection);
}

public interface ICachedRecentCountHistory<in T, out TElement> : ISupportsElementsShift<T, TElement>
{
    int CachedMaxCount { get; }
}

public interface IMutableCachedRecentCountHistory<in T, TElement> : IMutableSupportsElementsShift<T, TElement>
  , ICachedRecentCountHistory<T, TElement> where T : ITrackableReset<T>
{
    new TElement this[int index] { get; set; }

    new IReadOnlyList<ElementShift> ElementShifts { get; set; }

    new ushort? ClearedElementsAfterIndex { get; set; }
    new bool    HasRandomAccessUpdates    { get; set; }

    new ElementShift InsertAtStart(TElement toInsertAtStart);
    new ElementShift InsertAt(int index, TElement toInsertAtStart);
    new ElementShift DeleteAt(int index);

    new int CachedMaxCount { get; set; }
}

public interface IExpiringCachedPeriodUpdateHistory<in T, out TElement> : ICachedRecentCountHistory<T, TElement>
{
    DateTime UpdateTime { get; }

    TimeBoundaryPeriod DuringPeriod { get; }
}

public interface IMutableExpiringCachedPeriodUpdateHistory<in T, TElement> : IExpiringCachedPeriodUpdateHistory<T, TElement>
  , IMutableCachedRecentCountHistory<T, TElement> where T : ITrackableReset<T>
{
    new DateTime UpdateTime { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }
}
