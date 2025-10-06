using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.DataStructures.Memory.Buffers;

public record struct CharArrayRange(char[] CharBuffer, int FromIndex, int Length);

public class RecyclingCharArray : ReusableObject<RecyclingCharArray>, ICapacityList<char>, IStringBearer, ICharSequence
{
    private char[]? backingArray;

    private int length;

    public RecyclingCharArray() { }

    public RecyclingCharArray(int size)
    {
        EnsureIsAtSize(size);
    }

    public RecyclingCharArray(RecyclingCharArray toClone)
    {
        backingArray = new char[toClone.Capacity];
        length       = toClone.Count;
    }

    public Span<char> WrittenAsSpan()   => backingArray.AsSpan()[..length];
    public Span<char> RemainingAsSpan() => backingArray.AsSpan()[length..];

    public CharArrayRange AsCharArrayRange => new(backingArray!, 0, length);

    public int Count
    {
        get => length;
        set => length = value;
    }

    public int Length
    {
        get => length;
        set => length = Math.Max(0, Math.Min(backingArray?.Length ?? 0, value));
    }

    public int Capacity => backingArray!.Length;

    public int RemainingCapacity => Capacity - Count;

    public char[] BackingArray => backingArray!;

    public bool IsReadOnly => false;

    public char this[int index]
    {
        get
        {
            if (index < backingArray!.Length - 1) { return backingArray[index]; }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
        set
        {
            if (index < backingArray!.Length - 1)
            {
                backingArray[index] = value;
                if (index >= length && value != '\x0') { length = index + 1; }
                else if (index == length - 1 && length > 0 && value == '\x0') { length--; }
                return;
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
    }


    public RecyclingCharArray EnsureIsAtSize(int size)
    {
        if (backingArray != null && backingArray.Length != size)
        {
            throw new ArgumentException($"Expected the array to already be initialized at size {size} or be empty and ready to be initialized");
        }
        backingArray ??= new char[size];
        return this;
    }

    public void Add(char item)
    {
        if (RemainingCapacity < 1) throw new IndexOutOfRangeException($"RecyclingCharArray is at capacity of {Capacity}");
        if (length < backingArray!.Length) { backingArray[length++] = item; }
    }

    public void Add(string item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < item.Length && i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(string item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(char[] item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(char[] item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(Span<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(Span<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(ReadOnlySpan<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(ReadOnlySpan<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(ReadOnlyMemory<char> item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan  = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, itemSpan.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = itemSpan[i]; }
        }
    }

    public void Add(ReadOnlyMemory<char> item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var itemSpan  = item.Span;
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = itemSpan[i]; }
        }
    }

    public void Add(StringBuilder item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(StringBuilder item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(ICharSequence item)
    {
        if (RemainingCapacity < item.Length)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Length} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(ICharSequence item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Length - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + lengthToAdd, item.Length));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(RecyclingCharArray item)
    {
        if (RemainingCapacity < item.Count)
            throw new
                IndexOutOfRangeException($"Attempting to add {item.Count} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Count);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public void Add(RecyclingCharArray item, int startIndex, int lengthToAdd)
    {
        var amountToAdd = Math.Min(item.Count - startIndex, lengthToAdd);
        if (RemainingCapacity < amountToAdd)
            throw new
                IndexOutOfRangeException($"Attempting to add {amountToAdd} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, Math.Min(startIndex + amountToAdd, item.Count));
            for (int i = startIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
        }
    }

    public unsafe void Add(char* item, int valueCount)
    {
        if (RemainingCapacity < valueCount)
            throw new
                IndexOutOfRangeException($"Attempting to add {valueCount} chars to RecyclingCharArray with RemainingCapacity = {RemainingCapacity}");
        if (length < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, valueCount);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = *(item + 1); }
        }
    }

    public void Clear()
    {
        for (int i = length - 1; i >= 0; i--) { backingArray![i] = '\x0'; }
        length = 0;
    }

    public bool Contains(char item)
    {
        for (int i = length - 1; i >= 0; i--)
        {
            var check = backingArray![i];
            if (check == item) return true;
        }
        return false;
    }

    public void CopyTo(char[] array, int arrayIndex)
    {
        var myIndex = 0;
        for (int i = arrayIndex; i < array.Length && myIndex < length; i++) { array[i] = backingArray![myIndex++]; }
    }

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
        var myIndex   = sourceIndex;
        var stopIndex = Math.Min(destination.Length, destinationIndex + count);
        for (int i = destinationIndex; i < stopIndex && myIndex < length; i++) { destination[i] = backingArray![myIndex++]; }
    }

    public void CopyTo(int sourceIndex, Span<char> destination, int count)
    {
        var myIndex   = sourceIndex;
        var stopIndex = Math.Min(destination.Length, count);
        for (int i = 0; i < stopIndex && myIndex < length; i++) { destination[i] = backingArray![myIndex++]; }
    }

    public bool IsEndOf(string checkSameChars)
    {
        var compareIndex = checkSameChars.Length - 1;
        for (int i = length - 1; i >= 0 && compareIndex >= 0; i++)
        {
            var bufferChar = backingArray![i];
            var checkChar  = checkSameChars[compareIndex];
            if (bufferChar != checkChar) { return false; }
        }
        return true;
    }

    public int IndexOf(char item)
    {
        for (int i = 0; i < length; i++)
        {
            var check = backingArray![i];
            if (check == item) return i;
        }
        return -1;
    }

    public void Insert(int index, char item)
    {
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + 1);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - 1]; }
        if (index < backingArray!.Length)
        {
            backingArray[index] = item;
            length++;
        }
    }

    public void Insert(int index, string item)
    {
        var itemLen         = item.Length;
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, string item, int itemMaxLen)
    {
        var itemLen         = Math.Min(item.Length, itemMaxLen);
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, itemLen);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, ReadOnlySpan<char> item)
    {
        var itemLen         = item.Length;
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, ReadOnlySpan<char> item, int fromIndex, int itemMaxCopy)
    {
        var itemLen         = Math.Min(item.Length - fromIndex, itemMaxCopy);
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, itemLen);
            for (int i = fromIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, Span<char> item)
    {
        var itemLen         = item.Length;
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, Span<char> item, int fromIndex, int itemMaxCopy)
    {
        var itemLen         = Math.Min(item.Length - fromIndex, itemMaxCopy);
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, itemLen);
            for (int i = fromIndex; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, char[] item)
    {
        var itemLen         = item.Length;
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[length++] = item[i]; }
            length += stopIndex;
        }
    }

    public void Insert(int index, char[] item, int fromIndex, int itemMaxCopy)
    {
        if (index < backingArray!.Length)
        {
            var itemLen         = Math.Min(item.Length - fromIndex, itemMaxCopy);
            var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
            for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
            var stopIndex = Math.Min(RemainingCapacity, itemLen);
            for (int i = fromIndex; i < stopIndex; i++) { backingArray[index++] = item[i]; }
            length = cappedLengthEnd + 1;
        }
    }

    public void Insert(int index, ICharSequence item)
    {
        if (index < backingArray!.Length)
        {
            var itemLen         = item.Length;
            var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
            for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[index++] = item[i]; }
            length = cappedLengthEnd + 1;
        }
    }

    public void Insert(int index, ICharSequence item, int fromIndex, int itemMaxCopy)
    {
        if (index < backingArray!.Length)
        {
            var itemLen         = Math.Min(item.Length - fromIndex, itemMaxCopy);
            var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);

            for (int i = cappedLengthEnd; i >= index + itemLen && i - itemLen > 0; i--) backingArray[i] = backingArray![i - itemLen];

            var stopIndex = Math.Min(RemainingCapacity, itemLen);

            for (int i = fromIndex; i < stopIndex; i++) backingArray[index++] = item[i];
            length = cappedLengthEnd + 1;
        }
    }

    public void Insert(int index, StringBuilder item)
    {
        var itemLen         = item.Length;
        var cappedLengthEnd = Math.Min(backingArray!.Length - 1, length + itemLen);
        for (int i = cappedLengthEnd; i > index && i > 0; i--) { backingArray[i] = backingArray![i - itemLen]; }
        if (index < backingArray!.Length)
        {
            var stopIndex = Math.Min(RemainingCapacity, item.Length);
            for (int i = 0; i < stopIndex; i++) { backingArray[index++] = item[i]; }
            length = cappedLengthEnd;
        }
    }

    public bool Remove(char item)
    {
        var index = IndexOf(item);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if (index >= 0)
        {
            for (int i = index; i < backingArray!.Length - 1 && i < length; i++) { backingArray[i] = backingArray[i + 1]; }
            if (length > 0) backingArray![length - 1] = '\x0';
            length--;
        }
    }

    public void RemoveAt(int index, int lengthToRemove)
    {
        if (index >= 0)
        {
            var shiftAmount = Math.Min(length, lengthToRemove);

            var stopIndex = Math.Min(shiftAmount, backingArray!.Length);
            for (int i = index; i < stopIndex; i++) { backingArray[i] = backingArray[i + shiftAmount]; }
            if (length > 0) backingArray![length - shiftAmount] = '\x0';
            length -= shiftAmount;
        }
    }

    public void Replace(char find, char replace)
    {
        Replace(find, replace, 0, length);
    }

    public void Replace(char find, char replace, int startIndex, int searchLength)
    {
        var stopIndex = Math.Min(length, searchLength);
        for (var i = startIndex; i < stopIndex; i++)
        {
            if (backingArray![i] == find) { backingArray[i] = replace; }
        }
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace)
    {
        Replace(find, replace, 0, length);
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex)
    {
        Replace(find, replace, startIndex, length - startIndex);
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex, int searchLength)
    {
        if (startIndex >= 0 && startIndex < length)
        {
            var arraySpan = backingArray.AsSpan().Slice(startIndex);
            length += arraySpan.ReplaceCapped(length, find, replace);
        }
    }

    public void Replace(ICharSequence find, ICharSequence replace)
    {
        Replace(find, replace, 0, length);
    }

    public void Replace(ICharSequence find, ICharSequence replace, int startIndex)
    {
        Replace(find, replace, startIndex, length - startIndex);
    }

    public void Replace(ICharSequence find, ICharSequence replace, int startIndex, int searchLength)
    {
        if (startIndex >= 0 && startIndex < length)
        {
            var arraySpan = backingArray.AsSpan().Slice(startIndex);
            length += arraySpan.ReplaceCapped(length, find, replace);
        }
    }

    public void Replace(StringBuilder find, StringBuilder replace)
    {
        Replace(find, replace, 0, length);
    }

    public void Replace(StringBuilder find, StringBuilder replace, int startIndex)
    {
        Replace(find, replace, startIndex, length - startIndex);
    }

    public void Replace(StringBuilder find, StringBuilder replace, int startIndex, int searchLength)
    {
        if (startIndex >= 0 && startIndex < length)
        {
            var arraySpan = backingArray.AsSpan().Slice(startIndex);
            length += arraySpan.ReplaceCapped(length, find, replace);
        }
    }

    public void ToUpper()
    {
        var stopIndex = length;
        for (var i = 0; i < stopIndex; i++) { backingArray![i] = char.ToUpper(backingArray![i]); }
    }

    public void ToLower()
    {
        var stopIndex = length;
        for (var i = 0; i < stopIndex; i++) { backingArray![i] = char.ToLower(backingArray![i]); }
    }

    public RecyclingCharArray EnsureCapacity(int minReqCapacity)
    {
        if (RemainingCapacity < minReqCapacity)
        {
            var newRecyclingArray = (Capacity * 2).SourceRecyclingCharArray();
            var myCa              = backingArray!;
            var newCa             = newRecyclingArray.BackingArray;
            for (var i = 0; i < Count; i++) { newCa[i] = myCa[i]; }
            newRecyclingArray.Count = Count;
            DecrementRefCount();
            return newRecyclingArray;
        }
        return this;
    }

    public RecyclingCharArray FreeExcessCapacity(int newMaxCapacity)
    {
        if ((Capacity / 2) > newMaxCapacity)
        {
            var newRecyclingArray = newMaxCapacity.SourceRecyclingCharArray();
            var myCa              = backingArray!;
            var newCa             = newRecyclingArray.BackingArray;
            for (var i = 0; i < Count && i < newCa.Length; i++) { newCa[i] = myCa[i]; }
            newRecyclingArray.Count = Math.Min(Count, newCa.Length);
            DecrementRefCount();
            return newRecyclingArray;
        }
        return this;
    }

    public int CompareTo(string other)
    {
        var otherLen = other.Length;
        var maxLen   = Math.Max(length, otherLen);
        for (var i = 0; i < maxLen; i++)
        {
            if (i >= length) return 1;
            if (i >= otherLen) return -1;
            var charDiff = backingArray![i] - other[i];
            if (charDiff != 0) return charDiff;
        }
        return 0;
    }

    public int CompareTo(ICharSequence other)
    {
        var otherLen = other.Length;
        var maxLen   = Math.Max(length, otherLen);
        for (var i = 0; i < maxLen; i++)
        {
            if (i >= length) return 1;
            if (i >= otherLen) return -1;
            var charDiff = backingArray![i] - other[i];
            if (charDiff != 0) return charDiff;
        }
        return 0;
    }

    public bool Contains(ICharSequence subStr)
    {
        for (var i = 0; i < length - subStr.Length; i++)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return true;
            }
        }
        return false;
    }

    public bool Contains(string subStr)
    {
        for (var i = 0; i < length - subStr.Length; i++)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return true;
            }
        }
        return false;
    }

    public void CopyTo(char[] array, int arrayIndex, int myLength, int fromMyIndex = 0)
    {
        var myIndex   = fromMyIndex;
        var maxLength = Math.Min(array.Length, Math.Min(length - myIndex, myLength));
        for (var i = arrayIndex; i < maxLength; i++) { array[i] = backingArray![myIndex++]; }
    }

    public void CopyTo(RecyclingCharArray array, int? arrayIndex = null, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex   = fromMyIndex;
        var maxLength = Math.Min(array.Length, Math.Min(length - myIndex, myLength));
        for (var i = arrayIndex ?? 0; i < maxLength; i++) { array[i] = backingArray![myIndex++]; }
    }

    public void CopyTo(Span<char> charSpan, int spanIndex, int myLength = int.MaxValue, int fromMyIndex = 0)
    {
        var myIndex   = fromMyIndex;
        var maxLength = Math.Min(charSpan.Length, Math.Min(length - myIndex, myLength));
        for (var i = spanIndex; i < maxLength; i++) { charSpan[i] = backingArray![myIndex++]; }
    }

    public bool EquivalentTo(string other)
    {
        return CompareTo(other) == 0;
    }

    public int IndexOf(string subStr)
    {
        return IndexOf(subStr, 0);
    }

    public int IndexOf(ICharSequence subStr)
    {
        return IndexOf(subStr, 0);
    }

    public int IndexOf(string subStr, int fromThisPos)
    {
        for (var i = fromThisPos; i < length - subStr.Length; i++)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return i;
            }
        }
        return -1;
    }

    public int IndexOf(ICharSequence subStr, int fromThisPos)
    {
        for (var i = fromThisPos; i < length - subStr.Length; i++)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return i;
            }
        }
        return -1;
    }

    public int LastIndexOf(ICharSequence subStr, int fromThisPos)
    {
        var startAt = Math.Min(fromThisPos, length - subStr.Length);
        for (var i = startAt; i >= 0; i--)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return i;
            }
        }
        return -1;
    }

    public int LastIndexOf(string subStr)
    {
        return LastIndexOf(subStr, length - subStr.Length);
    }

    public int LastIndexOf(ICharSequence subStr)
    {
        return LastIndexOf(subStr, length - subStr.Length);
    }

    public int LastIndexOf(string subStr, int fromThisPos)
    {
        var startAt = Math.Min(fromThisPos, length - subStr.Length);
        for (var i = startAt; i >= 0; i--)
        {
            for (var j = 0; j < subStr.Length; j++)
            {
                var myChar      = backingArray![i + j];
                var compareChar = subStr[j];
                if (myChar != compareChar) break;
                if (j == subStr.Length - 1) return i;
            }
        }
        return -1;
    }

    public override void StateReset()
    {
        for (var i = length - 1; i >= 0; i--) { backingArray![i] = '\0'; }
        length = 0;
        base.StateReset();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator() =>
        Recycler != null ? this.RecycledEnumerator(Recycler) : backingArray!.Cast<char>().GetEnumerator();

    public override RecyclingCharArray Clone() =>
        Recycler?.Borrow<RecyclingCharArray>().EnsureIsAtSize(Capacity).CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new RecyclingCharArray(this);

    public override RecyclingCharArray CopyFrom(RecyclingCharArray source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (Capacity < source.Count)
        {
            throw new InvalidProgramException("Data loss will occur copying a larger char[] into char into one with insufficient capacity." +
                                              $"Either retrieve a larger {nameof(RecyclingCharArray)} or reduce the size before attempting the operation ");
        }
        source.CopyTo(backingArray!, 0);
        length = source.Count;
        return this;
    }

    public bool Equals(string? toCompare)
    {
        if (toCompare == null) return false;
        return CompareTo(toCompare) == 0;
    }

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        return
            tos.StartSimpleValueType(this)
                .AsStringOrNull(nameof(backingArray), backingArray, 0, length).Complete();
    }

    public override string ToString() => new(WrittenAsSpan());

    public string ToString(int fromIndex, int maxLength)
    {
        maxLength = Math.Min(length - fromIndex, maxLength);
        var arraySpan = backingArray.AsSpan().Slice(fromIndex, maxLength);
        return arraySpan.ToString();
    }
}

public static class RecyclingCharArrayExtensions
{
    public static IEnumerator<char> RecycledEnumerator(this RecyclingCharArray rca, IRecycler recycler) =>
        recycler.Borrow<RecyclingCharArrayEnumerator>().Initialize(rca);

    private class RecyclingCharArrayEnumerator : RecyclableObject, IEnumerator<char>
    {
        private RecyclingCharArray? rca;

        private int currentPosition = -1;

        public RecyclingCharArrayEnumerator Initialize(RecyclingCharArray rcArray)
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

        public char Current => rca![currentPosition];

        object IEnumerator.Current => Current;
    }
}
