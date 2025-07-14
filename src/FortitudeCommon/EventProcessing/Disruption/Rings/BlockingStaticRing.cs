#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public class BlockingStaticRing<T> : IBlockingQueue<T> where T : class, new()
{
    internal readonly RingCell<T>[] Cells;
    private readonly IClaimStrategy claimStrategy;
    private readonly Sequence conCursor;
    private readonly Sequence[] conCursors;

    private readonly IIntraOSThreadSignal consumerReturnedItemSignal;
    private readonly Sequence pubCursor;
    private readonly IIntraOSThreadSignal publishItemSignal;
    internal readonly int RingMask;
    internal readonly int RingSize;

    public BlockingStaticRing(string name, int size, ClaimStrategyType claimStrategyType)
    {
        RingSize   = MemoryUtils.CeilingNextPowerOfTwo(size);
        RingMask   = RingSize - 1;
        Cells      = new RingCell<T>[RingSize];

        pubCursor  = new(Sequence.InitialValue, RingMask);
        conCursor  = new(Sequence.InitialValue, RingMask);
        conCursors = new[] { conCursor };


        var osParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        consumerReturnedItemSignal = osParallelController.SingleOSThreadActivateSignal(false);
        publishItemSignal = osParallelController.SingleOSThreadActivateSignal(false);
        claimStrategy = claimStrategyType.GetInstance(name, RingSize, true, consumerReturnedItemSignal);

        for (var i = 0; i < Cells.Length; i++) Cells[i] = new RingCell<T>();
    }

    public RingCell<T> this[int index]
    {
        get => Cells[index & RingMask];
        set => Cells[index & RingMask] = value;
    }

    public int Count => pubCursor.Value - conCursor.Value;

    public int Capacity => Cells.Length;

    public void Add(T item)
    {
        var publishIndex = Claim();
        var cell = this[publishIndex];
        cell.Value = item;
        Publish(publishIndex);
    }

    public bool TryAdd(T item)
    {
        if (pubCursor.Value - conCursor.Value < RingMask)
        {
            Add(item);
            return true;
        }

        return false;
    }

    public int NextPeekIndex() => conCursor.Value + 1;

    public T? PeekAt(int index)
    {
        if (pubCursor.Value - index < 0) return null;
        var entry = Cells[index & RingMask];
        return entry.Value;
    }

    public int RemovalAll()
    {
        var oldConsumer = conCursor.Value;
        var pubValue = pubCursor.Value;
        for (var i = oldConsumer; i < pubValue; i++)
        {
            var checkItem = this[i];
            if (checkItem.Value is IRecyclableObject recyclableObject)
            {
                recyclableObject.DecrementRefCount();
            }
        }
        conCursor.Value = pubValue;
        return pubValue - oldConsumer;
    }

    public T Take()
    {
        var currentConsumer = conCursor.Value;
        return Take(currentConsumer);
    }

    private T Take(int currentConsumer)
    {
        var nextConsumerEntryIndex = currentConsumer + 1;
        while (pubCursor.Value != currentConsumer) publishItemSignal.WaitOne(10);
        var entry = Cells[nextConsumerEntryIndex & RingMask];
        conCursor.Value++;
        return entry.Value!;
    }

    public bool TryTake(out T item)
    {
        item = null!;
        var currentConsumer        = conCursor.Value;
        if (pubCursor.Value != conCursor.Value) return false;
        item = Take(currentConsumer);
        return true;
    }

    public int Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, conCursors);
        return sequence;
    }

    public void Publish(int sequence)
    {
        claimStrategy.Serialize(pubCursor, sequence);
        pubCursor.Value = sequence;
    }
}
