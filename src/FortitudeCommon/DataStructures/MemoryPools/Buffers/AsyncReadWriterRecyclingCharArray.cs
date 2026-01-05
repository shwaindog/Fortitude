using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.DataStructures.MemoryPools.Buffers;

public class AsyncReadWriterRecyclingCharArray : ReusableObject<AsyncReadWriterRecyclingCharArray>, IDisposable
  , ICapacityList<char>, IStringBearer
{
    public const int DefaultAcquireLockTimeoutMs = 2_000;

    ReaderWriterLock rwl = new();

    private RecyclingCharArray? protectedCharArray;

    private enum LastLockAcquired
    {
        None
      , Reader
      , Writer
    }

    private readonly Stack<LastLockAcquired> lastRequest = new();

    public AsyncReadWriterRecyclingCharArray()
    {
        lastRequest.Push(LastLockAcquired.None);
    }

    public AsyncReadWriterRecyclingCharArray(int size)
    {
        EnsureIsAtSize(size);
        lastRequest.Push(LastLockAcquired.None);
    }

    public AsyncReadWriterRecyclingCharArray(AsyncReadWriterRecyclingCharArray toClone)
    {
        lastRequest.Push(LastLockAcquired.None);
        protectedCharArray = toClone.Count.SourceRecyclingCharArray();
        protectedCharArray.CopyFrom(toClone.protectedCharArray!);
    }

    public AsyncReadWriterRecyclingCharArray EnsureIsAtSize(int size)
    {
        protectedCharArray ??= size.SourceRecyclingCharArray();
        protectedCharArray.EnsureIsAtSize(size);
        return this;
    }

    public RecyclingCharArray BackingRecyclingCharArray => protectedCharArray!;

    public int Count => protectedCharArray!.Count;

    public int Capacity => protectedCharArray!.Capacity;

    public int AcquireLockTimeOutMs { get; set; }

    public int RemainingCapacity    => protectedCharArray!.RemainingCapacity;

    public bool IsReadOnly => false;

    public char this[int index]
    {
        get
        {
            if (index < protectedCharArray!.Count - 1)
            {
                using var readerLock = AcquireReaderLock();
                return protectedCharArray[index];
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
        set
        {
            if (index < protectedCharArray!.Count - 1)
            {
                using var writerLock = AcquireWriterLock();
                protectedCharArray[index] = value;
                return;
            }
            throw new ArgumentException($"Tried to access char array at {index} which beyond end of the array");
        }
    }

    public void Add(char item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(string item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(string item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(char[] item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(char[] item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(ReadOnlySpan<char> item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(ReadOnlySpan<char> item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(ReadOnlyMemory<char> item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(ReadOnlyMemory<char> item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(StringBuilder item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(StringBuilder item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(IFrozenString item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(IFrozenString item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public void Add(RecyclingCharArray item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item);
    }

    public void Add(RecyclingCharArray item, int startIndex, int lengthToAdd)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, startIndex, lengthToAdd);
    }

    public unsafe void Add(char* item, int valueCount)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Add(item, valueCount);
    }

    public void Clear()
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Clear();
    }

    public bool Contains(char item)
    {
        using var readerLock = AcquireReaderLock();
        return protectedCharArray!.Contains(item);
    }

    public void CopyTo(char[] array, int arrayIndex)
    {
        using var readerLock = AcquireReaderLock();
        protectedCharArray!.CopyTo(array, arrayIndex);
    }

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
        using var readerLock = AcquireReaderLock();
        protectedCharArray!.CopyTo(sourceIndex, destination, destinationIndex, count);
    }

    public void CopyTo(int sourceIndex, Span<char> destination, int count)
    {
        using var readerLock = AcquireReaderLock();
        protectedCharArray!.CopyTo(sourceIndex, destination, count);
    }

    public IDisposable AcquireReaderLock(int? timeoutMs = null)
    {
        rwl.AcquireReaderLock(timeoutMs ?? AcquireLockTimeOutMs);
        lastRequest.Push(LastLockAcquired.Reader);
        return this;
    }

    public bool TryAcquireReaderLock(int? timeoutMs = 0)
    {
        if (rwl.IsWriterLockHeld) return false;
        if (rwl.IsReaderLockHeld) return true;
        AcquireReaderLock(timeoutMs);
        return rwl.IsReaderLockHeld;
    }

    public void ReleaseReaderLock()
    {
        if (!rwl.IsReaderLockHeld) return;
        rwl.ReleaseReaderLock();
    }

    public bool TryAcquireWriterLock(int? timeoutMs = 0)
    {
        AcquireWriterLock(timeoutMs);
        return rwl.IsWriterLockHeld;
    }

    public IDisposable AcquireWriterLock(int? timeoutMs = null)
    {
        rwl.AcquireWriterLock(timeoutMs ?? AcquireLockTimeOutMs);
        lastRequest.Push(LastLockAcquired.Writer);
        return this;
    }

    public void ReleaseWriterLock()
    {
        if (!rwl.IsWriterLockHeld) return;
        rwl.ReleaseWriterLock();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator()
    {
        rwl.AcquireReaderLock(DefaultAcquireLockTimeoutMs);
        return protectedCharArray!.GetEnumerator();
    }

    public void Dispose()
    {
        var lastLock = lastRequest.Pop();
        switch (lastLock)
        {
            case LastLockAcquired.Reader: ReleaseReaderLock(); break;
            case LastLockAcquired.Writer: ReleaseWriterLock(); break;
        }
        if (lastRequest.Count == 0)
        {
            if (rwl.IsReaderLockHeld || rwl.IsWriterLockHeld)
            {
                rwl.ReleaseLock();
            }
            lastRequest.Push(LastLockAcquired.None);
        }
    }

    public bool IsEndOf(string checkSameChars)
    {
        using var readerLock   = AcquireReaderLock();
        return protectedCharArray!.IsEndOf(checkSameChars);
    }

    public int IndexOf(char item)
    {
        using var readerLock = AcquireReaderLock();
        return protectedCharArray!.IndexOf(item);
    }

    public void Insert(int index, char item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, string item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, string item, int itemMaxLen)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item, itemMaxLen);
    }

    public void Insert(int index, ReadOnlySpan<char> item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, Span<char> item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, char[] item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, char[] item, int fromIndex, int itemMaxCopy)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item, fromIndex, itemMaxCopy);
    }

    public void Insert(int index, IFrozenString item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public void Insert(int index, StringBuilder item)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Insert(index, item);
    }

    public bool Remove(char item)
    {
        using var writerLock = AcquireWriterLock();
        return protectedCharArray!.Remove(item);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0)
        {
            using var writerLock = AcquireWriterLock();
            protectedCharArray!.RemoveAt(index);
        }
    }

    public void RemoveAt(int index, int lengthToRemove)
    {
        if (index >= 0)
        {
            using var writerLock = AcquireWriterLock();
            protectedCharArray!.RemoveAt(index, lengthToRemove);
        }
    }

    public void Replace(char find, char replace)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace);
    }

    public void Replace(char find, char replace, int startIndex, int searchLength)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex, searchLength);
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace);
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex);
    }

    public void Replace(ReadOnlySpan<char> find, ReadOnlySpan<char> replace, int startIndex, int searchLength)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex, searchLength);
    }

    public void Replace(IFrozenString find, IFrozenString replace)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace);
    }

    public void Replace(IFrozenString find, IFrozenString replace, int startIndex)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex);
    }

    public void Replace(IFrozenString find, IFrozenString replace, int startIndex, int searchLength)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex, searchLength);
    }

    public void Replace(StringBuilder find, StringBuilder replace)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace);
    }

    public void Replace(StringBuilder find, StringBuilder replace, int startIndex)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex);
    }

    public void Replace(StringBuilder find, StringBuilder replace, int startIndex, int searchLength)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.Replace(find, replace, startIndex, searchLength);
    }

    public void ToUpper()
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.ToUpper();
    }

    public void ToLower()
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray!.ToLower();
    }

    public AsyncReadWriterRecyclingCharArray EnsureCapacity(int minReqCapacity)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray = protectedCharArray!.EnsureCapacity(minReqCapacity);

        return this;
    }

    public AsyncReadWriterRecyclingCharArray FreeExcessCapacity(int newMaxCapacity)
    {
        using var writerLock = AcquireWriterLock();
        protectedCharArray = protectedCharArray!.FreeExcessCapacity(newMaxCapacity);

        return this;
    }

    public override AsyncReadWriterRecyclingCharArray Clone() =>
        Recycler?.Borrow<AsyncReadWriterRecyclingCharArray>().EnsureIsAtSize(Capacity).CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new AsyncReadWriterRecyclingCharArray(this);

    public override AsyncReadWriterRecyclingCharArray CopyFrom
        (AsyncReadWriterRecyclingCharArray source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        using var myWriter    = AcquireWriterLock();
        using var otherSource = source.AcquireReaderLock();
        if (Capacity < source.Count)
        {
            throw new InvalidProgramException("Data loss will occur copying a larger char[] into char into one with insufficient capacity." +
                                              $"Either retrieve a larger {nameof(AsyncReadWriterRecyclingCharArray)} or reduce the size before attempting the operation ");
        }
        protectedCharArray!.CopyFrom(source.protectedCharArray!, copyMergeFlags);
        return this;
    }

    public StateExtractStringRange RevealState(ITheOneString tos)
    {
        return
         tos.StartComplexContentType(this)
           .AsStringOrDefault(nameof(protectedCharArray), protectedCharArray)
           .Complete();
    }

    public override string ToString() => this.DefaultToString(Recycler);

    public string ToString(int fromIndex, int maxLength)
    {
        return protectedCharArray!.ToString(fromIndex, maxLength);
    }
}
