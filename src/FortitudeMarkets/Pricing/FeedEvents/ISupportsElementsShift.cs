using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents;

[Flags]
public enum ListShiftCommandType : byte // sent and stored in PQFieldFlags Scaling
{
    ShiftAllElementsAwayFromPinnedIndex = 0    // positive Shift is from pinned (FromIndex) + 1 to end, negative Shift is from pinned -1 to 0
  , ShiftAllElementsTowardPinnedIndex   = 0x01 // positive Shift is from 0 to pinned (FromIndex) -1, negative Shift is from end to pinned + 1
  , MoveSingleElement                   = 0x02
  , RemoveElementsRange                 = 0x04
  , InsertElementsRange                 = 0x08

  , ListShiftCommandMask = 0x1F

  , PQFieldFlagsReserved  = 0x80
  , PQFieldFlagsReserved1 = 0x40
  , PQFieldFlagsReserved2 = 0x20
}

public static class ListShiftCommandTypeExtensions
{
    public static bool HasMoveSingleElementFlag(this ListShiftCommandType flags) => (flags & ListShiftCommandType.MoveSingleElement) > 0;
    public static bool HasInsertElementsRangeFlag(this ListShiftCommandType flags) => (flags & ListShiftCommandType.InsertElementsRange) > 0;
}

public readonly struct ListShiftCommand
    (short shift, short fromIndex = 0, ListShiftCommandType shiftCommandType = ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex)
{
    public const short ShiftRightFromStartAwayFromPinnedIndex = short.MinValue;
    public const short ShiftLeftFromEndAwayFromPinnedIndex   = short.MaxValue;
    public const short ClearAllPinnedIndex                 = short.MaxValue;
    public const short ClearAllShiftAmount                 = short.MinValue;

    public short Shift     => shift;     // when ShiftCommandType is {ShiftAllElementsAwayFromPinnedIndex or ShiftAllElementsTowardPinnedIndex}
    public short DestinationIndex     => shift;     // when ShiftCommandType is {MoveSingleElement}
    public short Range     =>  shift;     // when ShiftCommandType is {RemoveElementsRange or InsertElementsRange}
    public short FromIndex  => fromIndex; // when ShiftCommandType is {MoveSingleElement or RemoveElementsRange or InsertElementsRange} 
    public short PinnedFromIndex => fromIndex; // when ShiftCommandType is {ShiftAllElementsAwayFromPinnedIndex or ShiftAllElementsTowardPinnedIndex} 

    public ListShiftCommandType ShiftCommandType { get; } = shiftCommandType;


    public void Deconstruct(out short shiftOut, out short fromIndexOut)
    {
        shiftOut = Shift;
        fromIndexOut = FromIndex;
    }

    #pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
    public static explicit operator uint(ListShiftCommand toConvert) => unchecked(((uint)toConvert.FromIndex << sizeof(short)*8 | (uint)(toConvert.Shift & 0xFF_FF)));

    public static explicit operator PQFieldFlags(ListShiftCommand toConvert) => (PQFieldFlags)((byte)toConvert.ShiftCommandType);

    public static explicit operator ListShiftCommand
        (PQFieldUpdate toConvert) =>
        unchecked(new( (short)(toConvert.Payload & 0xFF_FF), ((short)((toConvert.Payload >> sizeof(short)*8) & 0xFF_FF))
          , (ListShiftCommandType)(byte)(toConvert.Flag) & ListShiftCommandType.ListShiftCommandMask));
    #pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
    public override string ToString() => $"{nameof(ListShiftCommand)}{{{nameof(Shift)}: {Shift}, {nameof(FromIndex)}: {FromIndex}, " +
                                         $"{nameof(ShiftCommandType)}: {ShiftCommandType}}}";
}

public static class ListShiftCommandExtensions
{
    public static bool IsSwapWithDestination
        (this ListShiftCommand moveCommand) =>
        moveCommand.ShiftCommandType.HasInsertElementsRangeFlag() && moveCommand.ShiftCommandType.HasMoveSingleElementFlag();
    public static bool IsClearAll(this ListShiftCommand shiftCommand) =>
        shiftCommand is { Shift : ListShiftCommand.ClearAllShiftAmount, FromIndex: ListShiftCommand.ClearAllPinnedIndex };

    public static bool IsMoveSingleElement(this ListShiftCommand shiftCommand) =>
        shiftCommand is { ShiftCommandType: ListShiftCommandType.MoveSingleElement };

    public static bool IsRemoveElementsRange(this ListShiftCommand shiftCommand) =>
        shiftCommand is { ShiftCommandType: ListShiftCommandType.RemoveElementsRange };

    public static bool IsInsertElementsRange(this ListShiftCommand shiftCommand) =>
        shiftCommand is { ShiftCommandType: ListShiftCommandType.InsertElementsRange };

    public static bool IsShiftElementsAwayFromPinnedIndex(this ListShiftCommand shiftCommand) =>
        shiftCommand is { ShiftCommandType: ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex };

    public static bool IsShiftElementsTowardFromPinnedIndex(this ListShiftCommand shiftCommand) =>
        shiftCommand is { ShiftCommandType: ListShiftCommandType.ShiftAllElementsTowardPinnedIndex };

    public static bool LastShiftIsShiftRightFromStart(this IList<ListShiftCommand> shiftCommands)
    {
        var lastCommand = shiftCommands.SafeLastShift();
        if (lastCommand is
            { FromIndex: ListShiftCommand.ShiftRightFromStartAwayFromPinnedIndex, Shift: > 0 }) // last command was shift right
        {
            return true;
        }
        return false;
    }

    public static bool HasLastShift(this IList<ListShiftCommand> shiftCommands) => shiftCommands.Any();

    public static ListShiftCommand? SafeLastShift(this IList<ListShiftCommand> shiftCommand)
    {
        if (shiftCommand.HasLastShift())
        {
            var lastAddedCommand = shiftCommand[^1];
            return lastAddedCommand;
        }
        return null;
    }

    public static ListShiftCommand LastShift(this IList<ListShiftCommand> shiftCommand)
    {
        if (shiftCommand.HasLastShift())
        {
            var lastAddedCommand = shiftCommand[^1];
            return lastAddedCommand;
        }
        throw new ArgumentException("No last shift commands exist");
    }

    public static bool ReplaceLastShift(this IList<ListShiftCommand> shiftCommands, ListShiftCommand replaceWith)
    {
        if (shiftCommands.HasLastShift())
        {
            shiftCommands[^1] = replaceWith;
            return true;
        }
        return false;
    }

    public static bool AddShiftRightFromStart(this IList<ListShiftCommand> shiftCommands, short amount = 1)
    {
        shiftCommands.Add(new ListShiftCommand(amount, ListShiftCommand.ShiftRightFromStartAwayFromPinnedIndex));
        return false;
    }

    public static bool AppendShiftAwayFromIndex(this IList<ListShiftCommand> shiftCommands, short fromIndex, short amount)
    {
        shiftCommands.Add(new ListShiftCommand(amount, fromIndex));
        return false;
    }

    public static bool AppendRemoveRangeFromIndex(this IList<ListShiftCommand> shiftCommands, short fromIndex, short amount)
    {
        shiftCommands.Add(new ListShiftCommand(amount, fromIndex, ListShiftCommandType.RemoveElementsRange));
        return false;
    }

    public static bool AppendShiftTowardFromIndex(this IList<ListShiftCommand> shiftCommands, short fromIndex, short amount)
    {
        shiftCommands.Add(new ListShiftCommand(amount, fromIndex, ListShiftCommandType.ShiftAllElementsTowardPinnedIndex));
        return false;
    }

    public static bool AppendMoveSingleElement(this IList<ListShiftCommand> shiftCommands, short fromIndex, short toIndex)
    {
        shiftCommands.Add(new ListShiftCommand(toIndex, fromIndex, ListShiftCommandType.MoveSingleElement));
        return false;
    }

    public static bool AppendSwapMoveSingleElement(this IList<ListShiftCommand> shiftCommands, short fromIndex, short toIndex)
    {
        shiftCommands.Add(new ListShiftCommand(toIndex, fromIndex, ListShiftCommandType.MoveSingleElement | ListShiftCommandType.InsertElementsRange));
        return false;
    }

    public static bool Append(this IList<ListShiftCommand> shiftCommands, ListShiftCommand toAppend)
    {
        shiftCommands.Add(toAppend);
        return false;
    }

    public static bool IncrementLastCommandShift(this IList<ListShiftCommand> shiftCommands)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().IncrementShift());
    }

    public static bool DecrementLastCommandShift(this IList<ListShiftCommand> shiftCommands)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().DecrementShift());
    }

    public static bool ChangeLastCommandShiftBy(this IList<ListShiftCommand> shiftCommands, short amountToChange)
    {
        return shiftCommands.HasLastShift() && shiftCommands.ReplaceLastShift(shiftCommands.LastShift().AddToShift(amountToChange));
    }

    public static bool LastShiftIsShiftLeftFromEnd(this IList<ListShiftCommand> shiftCommands)
    {
        var lastCommand = shiftCommands.LastShift();
        if (lastCommand is
            { FromIndex: ListShiftCommand.ShiftLeftFromEndAwayFromPinnedIndex, Shift: < 0 }) // last command was shift right
        {
            return true;
        }
        return false;
    }

    public static ListShiftCommand AddToShift(this ListShiftCommand toIncrement, short amountToAdd) =>
        new((short)(toIncrement.Shift + amountToAdd), toIncrement.FromIndex, toIncrement.ShiftCommandType);

    public static void AlterAllShiftsBy(this IList<ListShiftCommand> shiftCommands, short amountToAdd)
    {
        for (int i = 0; i < shiftCommands.Count; i++)
        {
            shiftCommands[i] = shiftCommands[i].AddToShift(amountToAdd);
        }
    }

    public static ListShiftCommand IncrementShift(this ListShiftCommand toIncrement) => new((short)(toIncrement.Shift + 1), toIncrement.FromIndex, toIncrement.ShiftCommandType);

    public static ListShiftCommand DecrementShift(this ListShiftCommand toIncrement) => new((short)(toIncrement.Shift + 1), toIncrement.FromIndex, toIncrement.ShiftCommandType);
}

public interface IShiftable<in T> { }

public interface ISupportsElementsShift<in T, TElement> : IShiftable<T>
{
    IReadOnlyList<ListShiftCommand> ElementShifts { get; }

    int? ClearedElementsAfterIndex { get; }

    bool    HasRandomAccessUpdates    { get; }

    void CalculateShift(DateTime asAtTime, IReadOnlyList<TElement> updatedCollection);

    TElement this[int index] { get; }
}

public interface IMutableSupportsElementsShift<in T, TElement> : IShiftable<T> where T : ITrackableReset<T>
{
    IReadOnlyList<ListShiftCommand> ElementShifts { get; set; }

    int? ClearedElementsAfterIndex { get; set; }

    bool    HasRandomAccessUpdates    { get; set; }

    TElement this[int index] { get; set; }

    ListShiftCommand InsertAtStart(TElement toInsertAtStart);

    bool AppendAtEnd(TElement toAppendAtEnd);

    ListShiftCommand InsertAt(int index, TElement toInsertAtStart);

    ListShiftCommand DeleteAt(int index);

    ListShiftCommand Delete(TElement toDelete);

    ListShiftCommand ApplyElementShift(ListShiftCommand shiftCommandToApply);

    ListShiftCommand MoveToStart(TElement existingItem);

    ListShiftCommand MoveSingleElementToStart(int indexToMoveToStart);

    ListShiftCommand MoveSingleElementToEnd(int indexToMoveToEnd);

    ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift );

    ListShiftCommand MoveSingleElementBy(TElement existingItem, int shift );

    ListShiftCommand MoveToEnd(TElement existingItem);

    ListShiftCommand ClearAll();

    ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex);

    ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex);
}

public interface ICachedRecentCountHistory<in T, TElement> : ISupportsElementsShift<T, TElement>
{
    int CachedMaxCount { get; }
}

public interface IMutableCachedRecentCountHistory<in T, TElement> : IMutableSupportsElementsShift<T, TElement>
  , ICachedRecentCountHistory<T, TElement> where T : ITrackableReset<T>
{
    new TElement this[int index] { get; set; }

    new IReadOnlyList<ListShiftCommand> ElementShifts { get; set; }

    new int? ClearedElementsAfterIndex { get; set; }
    new bool    HasRandomAccessUpdates    { get; set; }

    new ListShiftCommand InsertAtStart(TElement toInsertAtStart);
    new ListShiftCommand InsertAt(int index, TElement toInsertAtStart);

    new int CachedMaxCount { get; set; }
}

public interface IExpiringCachedPeriodUpdateHistory<in T, TElement> : ICachedRecentCountHistory<T, TElement>
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
