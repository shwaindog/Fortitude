using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.DataStructures.Memory;

public class RecyclingCharArray : ReusableObject<RecyclingCharArray>, ICapacityList<char>, IStyledToStringObject
{
    private char[]? backingArray;

    private int length = 0;

    public RecyclingCharArray() { }

    public RecyclingCharArray(int size)
    {
        EnsureIsAtSize(size);
    }

    public RecyclingCharArray(RecyclingCharArray toClone)
    {
        backingArray = new char[toClone.Capacity];
        length = toClone.Count;
    }

    public RecyclingCharArray EnsureIsAtSize(int size)
    {
        if (backingArray != null && backingArray.Length != size)
        {
            throw new ArgumentException($"Expected the array to already be initialized at size {size} or be empty and ready to be initialized");
        }
        backingArray = new char[size];
        return this;
    }

    public void Add(char item)
    {
        if (length < backingArray!.Length)
        {
            backingArray[length++] = item;
        }
    }

    public void Clear()
    {
        for (int i = length - 1; i >= 0; i--)
        {
            backingArray![i] = '\x0';
        }
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
        for (int i = arrayIndex; i < array.Length && myIndex < length ; i++)
        {
            array[i] = backingArray![myIndex++];
        }
    }

    public int Count => length;

    public int Capacity => backingArray!.Length;

    public char[] BackingArray => backingArray!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator() => backingArray!.Cast<char>().GetEnumerator();

    public bool IsEndOf(string checkSameChars)
    {
        var compareIndex = checkSameChars.Length - 1;
        for (int i = length -1; i >= 0 && compareIndex >= 0; i++)
        {
            var bufferChar = backingArray![i];
            var checkChar  = checkSameChars[compareIndex];
            if (bufferChar != checkChar)
            {
                return false;
            }
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
        for (int i = cappedLengthEnd; i > index && i > 0; i--)
        {
            backingArray[i] = backingArray![i -1];
        }
        if (index < backingArray!.Length - 1)
        {
            backingArray[index] = item;
            length++;
        }
    }

    public bool IsReadOnly => false;

    public char this[int index]
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
                } else if (index == length -1 && length > 0 && value == '\x0')
                {
                    length--;
                }
                return;
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
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
            for (int i = index; i < backingArray!.Length -1 && i < length; i++)
            {
                backingArray[i]  = backingArray[i + 1];
            }
            if(length > 0) backingArray![length - 1] = '\x0';
            length--;
        }
    }

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

    public void ToString(IStyledTypeStringAppender sbc)
    {
        for (int i = 0; i < length; i++)
        {
            sbc.BackingStringBuilder.Append(backingArray![i]);
        }
    }

    public override string ToString() => this.DefaultToString(Recycler);
}
