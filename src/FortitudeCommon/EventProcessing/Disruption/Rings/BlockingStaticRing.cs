#region

using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public class BlockingStaticRing<T> : IBlockingQueue<T> where T : class
{
    internal readonly RingCell<T>[]  Cells;
    private readonly  IClaimStrategy claimStrategy;

    private readonly Sequence   conCursor;
    private readonly Sequence[] conCursors;
    private readonly Sequence   pubCursor;

    internal readonly int RingMask;
    internal readonly int RingSize;

    private readonly IIntraOSThreadSignal consumerReturnedItemSignal;
    private readonly IIntraOSThreadSignal publishItemSignal;


    private PaddedLong pushAttemptToken = new(0);
    private PaddedLong popAttemptToken  = new(0);

    public BlockingStaticRing(string name, int size, ClaimStrategyType claimStrategyType)
    {
        RingSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        RingMask = RingSize - 1;
        Cells    = new RingCell<T>[RingSize];

        pubCursor  = new Sequence(Sequence.InitialValue, RingMask);
        conCursor  = new Sequence(Sequence.InitialValue, RingMask);
        conCursors = [conCursor];

        var osParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        consumerReturnedItemSignal = osParallelController.SingleOSThreadActivateSignal(false);
        publishItemSignal          = osParallelController.SingleOSThreadActivateSignal(false);
        claimStrategy              = claimStrategyType.GetInstance(name, RingSize, true, consumerReturnedItemSignal);

        for (var i = 0; i < Cells.Length; i++) Cells[i] = new RingCell<T>();
    }

    public RingCell<T> this[int index]
    {
        get => Cells[index & RingMask];
        set => Cells[index & RingMask] = value;
    }

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var snapCon = conCursor.Value;
            var snapPub = pubCursor.Value;
            var count   = snapCon > snapPub ? snapPub + (RingSize - snapCon) : snapPub - snapCon;
            return count;
        }
    }

    public int Capacity => RingSize;

    public int RemainingCapacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => RingSize - Count;
    }

    public void Add(T item)
    {
        var publishIndex = Claim();
        var cell         = this[publishIndex];
        cell.Value = item;
        Publish(publishIndex);
    }

    // Unsafe returns item removed or not added to queue or null if no item needed to be removed
    public T? UnsafePopPush(T item)
    {
        int attemptCount = 0;
        while (attemptCount++ < 10)
        {
            if (RemainingCapacity > 10)
            {
                Add(item);
                return null;
            }
            if (Interlocked.CompareExchange(ref pushAttemptToken.Value, 1, 0) == 0)
            {
                try
                {
                    if (!IsFull)
                    {
                        var snapPub = pubCursor.Value;
                        var cell    = this[snapPub];
                        var popped  = cell.Value;
                        cell.Value = item;

                        conCursor.Decrement();
                        pubCursor.Decrement();
                        return popped;
                    }
                }
                finally
                {
                    Thread.VolatileWrite(ref pushAttemptToken.Value, 0);
                }
            }
            else
            {
                Thread.SpinWait(200);
            }
        }
        return item;
    }


    public bool IsFull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var isFull = conCursor.Value == ((pubCursor.Value + 1) & RingMask);
            return isFull;
        }
    }


    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var isEmpty = pubCursor.Value == conCursor.Value;
            return isEmpty;
        }
    }

    public bool TryAdd(T item)
    {
        int attemptCount = 0;
        while (attemptCount++ < 10)
        {
            if (RemainingCapacity > 10)
            {
                Add(item);
                return true;
            }
            if (Interlocked.CompareExchange(ref pushAttemptToken.Value, 1, 0) == 0)
            {
                try
                {
                    if (!IsFull)
                    {
                        Add(item);
                        return true;
                    }
                }
                finally
                {
                    Thread.VolatileWrite(ref pushAttemptToken.Value, 0);
                }
            }
            else
            {
                Thread.SpinWait(200);
            }
        }
        return false;
    }

    public T? ReplaceLastAdded(T item, int offset = 0)
    {
        var cell          = this[conCursor.Value - offset];
        var replacedValue = cell.Value;
        cell.Value = item;
        return replacedValue;
    }

    public int NextPeekIndex() => conCursor.Value + 1;

    public T? PeekAt(int index)
    {
        if (pubCursor.Value - index < 0 && conCursor.Value > index) return null;
        var entry = Cells[index & RingMask];
        return entry.Value;
    }

    public int RemovalAll()
    {
        var oldConsumer = conCursor.Value;
        var pubValue    = pubCursor.Value;
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
        while (IsEmpty) publishItemSignal.WaitOne(10);
        return UncheckedTake(conCursor.Increment());
    }

    private T UncheckedTake(int takeAt)
    {
        var entry = Cells[takeAt & RingMask];
        return entry.Value!;
    }

    public bool TryTake(out T item)
    {
        item = null!;

        int attemptCount = 0;
        while (attemptCount++ < 10)
        {
            if (Count > 10)
            {
                item = UncheckedTake(conCursor.Increment());
                return true;
            }
            if (Interlocked.CompareExchange(ref popAttemptToken.Value, 1, 0) == 0)
            {
                try
                {
                    var currentConsumer = conCursor.Value;
                    if (pubCursor.Value != currentConsumer) return false;
                    item = UncheckedTake(conCursor.Increment());
                    return true;
                }
                finally
                {
                    Thread.VolatileWrite(ref popAttemptToken.Value, 0);
                }
            }
            Thread.SpinWait(200);
        }
        return false;
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
