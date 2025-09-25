using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;

public record struct ByteArrayRange(byte[] ByteBuffer, int FromIndex, int Length);

public class RecyclingByteArray: ReusableObject<RecyclingByteArray>, ICapacityList<byte>
{
    private byte[]? backingArray;

    private int length;

    public RecyclingByteArray() { }

    public RecyclingByteArray(int size)
    {
        EnsureIsAtSize(size);
    }
    
    public RecyclingByteArray(RecyclingByteArray toClone)
    {
        backingArray = new byte[toClone.Capacity];
        length       = toClone.Count;
    }

    public RecyclingByteArray EnsureIsAtSize(int size)
    {
        if (backingArray != null && backingArray.Length != size)
        {
            throw new ArgumentException($"Expected the array to already be initialized at size {size} or be empty and ready to be initialized");
        }
        backingArray ??= new byte[size];
        return this;
    }

    public int Capacity => backingArray!.Length;
    
    public int Count
    {
        get => length;
        set => length = value;
    }
    
    public Span<byte> WrittenAsSpan()   => backingArray.AsSpan()[..length];
    
    public Span<byte> RemainingAsSpan() => backingArray.AsSpan()[length..];

    public int RemainingCapacity => Capacity - Count;
    
    public ByteArrayRange AsByteArrayRange => new (backingArray!, 0, length);
    
    public byte this[int index]
    {
        get
        {
            if (index < backingArray!.Length - 1)
            {
                return backingArray[index];
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
        set
        {
            if (index < backingArray!.Length - 1)
            {
                backingArray[index] = value;
                if (index >= length && value != '\x0')
                {
                    length = index + 1;
                }
                else if (index == length - 1 && length > 0 && value == '\x0')
                {
                    length--;
                }
                return;
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
    }
    
    public void Add(byte item)
    {
        if (RemainingCapacity < 1) throw new IndexOutOfRangeException($"RecyclingCharArray is at capacity of {Capacity}");
        if (length < backingArray!.Length)
        {
            backingArray[length++] = item;
        }
    }

    public void Add(byte[] item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < item.Length && i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }
    
    public void Add(byte[] item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }

    public void Add(Span<byte> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }

    public void Add(Span<byte> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }

    public void Add(ReadOnlySpan<byte> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }

    public void Add(ReadOnlySpan<byte> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                backingArray[length++] = item[i];
            }
        }
    }

    public void Add(ReadOnlyMemory<byte> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan  = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, itemSpan.Length);
            for (int i = 0; i < stopIndex; i++)
            {
                backingArray[length++] = itemSpan[i];
            }
        }
    }

    public void Add(ReadOnlyMemory<byte> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                backingArray[length++] = itemSpan[i];
            }
        }
    }

    public void Add(Encoder encoder,  char item)
    {
        if (RemainingCapacity < 1) throw new IndexOutOfRangeException($"RecyclingCharArray is at capacity of {Capacity}");
        if (length < backingArray!.Length)
        {
            var charSpan = stackalloc char[1].ResetMemory();
            charSpan[0] = item;
            length += encoder.GetBytes(charSpan, RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder,  string item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            length      += encoder.GetBytes(item, RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder,  string item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Length));
            length      += encoder.GetBytes(item.AsSpan()[startIndex..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder,  char[] item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            length      += encoder.GetBytes(item[..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, char[] item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Length));
            length      += encoder.GetBytes(item[startIndex..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, Span<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            length      += encoder.GetBytes(item, RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, Span<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Length));
            length      += encoder.GetBytes(item[startIndex..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, ReadOnlySpan<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            length      += encoder.GetBytes(item[..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, ReadOnlySpan<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Length));
            length      += encoder.GetBytes(item[startIndex..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder,ReadOnlyMemory<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan  = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, itemSpan.Length);
            length      += encoder.GetBytes(itemSpan[..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder,ReadOnlyMemory<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            length      += encoder.GetBytes(itemSpan[startIndex..stopIndex], RemainingAsSpan(), false);
        }
    }

    public void Add(Encoder encoder, StringBuilder item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }

    public void Add(Encoder encoder, StringBuilder item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }

    public void Add(Encoder encoder, ICharSequence item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }

    public void Add(Encoder encoder, ICharSequence item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }

    public void Add(Encoder encoder, RecyclingCharArray item)
    {
        if (RemainingCapacity < item.Count)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Count} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Count);
            for (int i = 0; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }

    public void Add(Encoder encoder, RecyclingCharArray item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Count - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Count));
            for (int i = startIndex; i < stopIndex; i++)
            {
                Add(encoder, item[i]);
            }
        }
    }
    

    public void CopyTo(byte[] array, int arrayIndex)
    {
        var myIndex = 0;
        for (int i = arrayIndex; i < array.Length && myIndex < length; i++)
        {
            array[i] = backingArray![myIndex++];
        }
    }
    
    public void Clear()
    {
        for (int i = length - 1; i >= 0; i--)
        {
            backingArray![i] = 0;
        }
        length = 0;
    }

    public override void StateReset()
    {
        Clear();
        base.StateReset();
    }
    
    public override RecyclingByteArray Clone() =>
        Recycler?.Borrow<RecyclingByteArray>().EnsureIsAtSize(Capacity).CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new RecyclingByteArray(this);

    
    public override RecyclingByteArray CopyFrom(RecyclingByteArray source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (Capacity < source.Count)
        {
            throw new InvalidProgramException("Data loss will occur copying a larger char[] into char into one with insufficient capacity." +
                                              $"Either retrieve a larger {nameof(RecyclingByteArray)} or reduce the size before attempting the operation ");
        }
        source.CopyTo(backingArray!, 0);
        length = source.Count;
        return this;
    }
    

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<byte> GetEnumerator() => 
        Recycler != null ? this.RecycledEnumerator(Recycler) : backingArray!.Cast<byte>().GetEnumerator();
}

public static class RecyclingByteArrayExtensions
{
    public static IEnumerator<byte> RecycledEnumerator(this RecyclingByteArray rca, IRecycler recycler) =>
        recycler.Borrow<RecyclingByteArrayEnumerator>().Initialize(rca);
   
    private class RecyclingByteArrayEnumerator : RecyclableObject, IEnumerator<byte>
    {
        private RecyclingByteArray? rca;

        private int currentPosition = -1;

        public RecyclingByteArrayEnumerator Initialize(RecyclingByteArray rcArray)
        {
            rca = rcArray;

            currentPosition = -1;

            return this;
        }

        public void Dispose()
        {
            Reset();
            rca = null;
            DecrementRefCount();
        }

        public bool MoveNext() => ++currentPosition < rca!.Count;

        public void Reset()
        {
            currentPosition = -1;
        }

        public byte Current => rca![currentPosition];

        object IEnumerator.Current => Current;
    }
}