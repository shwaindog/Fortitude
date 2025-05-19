namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents;


public readonly struct ElementShift(short shiftAmount, ushort clearAtIndex = 0)
{
    public short ShiftAmount { get; } = shiftAmount;
    public ushort ClearAtIndex { get; } = clearAtIndex;
}

public interface ISupportsElementsShift<in T> where T : ISupportsElementsShift<T>
{
    ElementShift CalculateShift(DateTime asAtTime, IReadOnlyList<T> previousCollection, IReadOnlyList<T> updatedCollection);
}