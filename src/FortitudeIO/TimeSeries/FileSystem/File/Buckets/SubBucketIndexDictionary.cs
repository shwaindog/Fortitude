#region

using System.Collections;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SubBucketIndexInfo
{
    public SubBucketIndexInfo(uint bucketId, DateTime bucketPeriodStart, BucketFlags bucketFlags, TimeSeriesPeriod bucketPeriod
        , long parentOrFileOffset)
    {
        BucketId = bucketId;
        BucketPeriodStart = bucketPeriodStart.Ticks;
        BucketFlags = bucketFlags;
        BucketPeriod = bucketPeriod;
        ParentOrFileOffset = parentOrFileOffset;
    }

    public uint BucketId;
    public TimeSeriesPeriod BucketPeriod;
    public BucketFlags BucketFlags;
    private long BucketPeriodStart;
    public long ParentOrFileOffset;

    public DateTime BucketStartTime
    {
        get => DateTime.FromBinary(BucketPeriodStart);
        set => BucketPeriodStart = value.Ticks;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SubBucketIndexList
{
    public uint MaxIndexSizeEntries;
    public SubBucketIndexInfo LastAddedBucketIndexInfo;
    public SubBucketIndexInfo FirstIndexInList;
}

public interface IReadonlySubBucketIndexDictionary : IReadOnlyDictionary<uint, SubBucketIndexInfo>
{
    bool IsFixedSize { get; }
    long SizeInBytes { get; }
    void CacheAndCloseFileView();
    void OpenWithFileView(ShiftableMemoryMappedFileView memoryMappedFileView, bool isReadOnly);
}

public interface ISubBucketIndexDictionary : IDictionary<uint, SubBucketIndexInfo>, IReadonlySubBucketIndexDictionary
{
    SubBucketIndexInfo? LastAddedSubBucketIndexInfo { get; }
    uint NextEmptyIndexKey { get; }
    new SubBucketIndexInfo this[uint key] { get; set; }
    new ICollection<uint> Keys { get; }
    new ICollection<SubBucketIndexInfo> Values { get; }
    new int Count { get; }
    new bool IsReadOnly { get; }
    new bool ContainsKey(uint key);
    new bool TryGetValue(uint key, out SubBucketIndexInfo value);
    new bool Contains(KeyValuePair<uint, SubBucketIndexInfo> item);
    new void CopyTo(KeyValuePair<uint, SubBucketIndexInfo>[] array, int arrayIndex);
    new IEnumerator<KeyValuePair<uint, SubBucketIndexInfo>> GetEnumerator();
}

public unsafe class SubBucketIndexDictionary : ISubBucketIndexDictionary
{
    private const long FirstEntryFileCursorOffset = 8;
    private static IRecycler recycler = new Recycler();
    private readonly long internalIndexFileCursor;
    private List<KeyValuePair<uint, SubBucketIndexInfo>>? cacheBucketIndexInfos;
    private SubBucketIndexInfo cacheLastAddedSubBucketIndexInfo;
    private SubBucketIndexInfo* firstEntryBufferPointer;
    private SubBucketIndexInfo* lastAddedEntryBufferPointer;
    private uint maxPossibleIndexEntries;
    private ShiftableMemoryMappedFileView? memoryMappedFileView;
    private SubBucketIndexList* writableV1IndexHeaderSectionV1;

    public SubBucketIndexDictionary(ShiftableMemoryMappedFileView memoryMappedFileView,
        long fileStartCursorOffset, uint maxPossibleIndexEntries, bool isReadOnly)
    {
        internalIndexFileCursor = fileStartCursorOffset;
        OpenWithFileView(memoryMappedFileView, isReadOnly);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        this.maxPossibleIndexEntries = maxPossibleIndexEntries;
    }

    public bool IsFileViewOpen => memoryMappedFileView != null && firstEntryBufferPointer != null;

    public SubBucketIndexInfo? LastAddedSubBucketIndexInfo
    {
        get
        {
            if (!IsFileViewOpen) return cacheLastAddedSubBucketIndexInfo;
            return cacheLastAddedSubBucketIndexInfo = *lastAddedEntryBufferPointer;
        }
    }

    public uint NextEmptyIndexKey
    {
        get
        {
            if (!IsFileViewOpen) return cacheBucketIndexInfos?.Max(bio => bio.Key) + 1 ?? 0;
            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var indexEntry = firstEntryBufferPointer + i;
                if (indexEntry->BucketId == 0 || indexEntry->BucketFlags != BucketFlags.None) return i;
            }

            return uint.MaxValue;
        }
    }

    public SubBucketIndexInfo this[uint key]
    {
        get
        {
            SubBucketIndexInfo returnResult = default;
            if (key > maxPossibleIndexEntries) return returnResult;
            if (!IsFileViewOpen)
            {
                returnResult = cacheBucketIndexInfos?.FirstOrDefault(kvp => kvp.Key == key).Value ?? default;
                return returnResult;
            }

            var indexEntry = firstEntryBufferPointer + key;
            if (indexEntry->BucketId == 0 || indexEntry->BucketFlags == BucketFlags.None) return returnResult;
            returnResult = *indexEntry;
            return returnResult;
        }
        set => Add(key, value);
    }

    public ICollection<SubBucketIndexInfo> Values
    {
        get
        {
            if (!IsFileViewOpen)
            {
                cacheBucketIndexInfos ??= new List<KeyValuePair<uint, SubBucketIndexInfo>>();
                return cacheBucketIndexInfos.Select(kvp => kvp.Value).ToList();
            }

            var reusableList = recycler.Borrow<ReusableList<SubBucketIndexInfo>>();
            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = firstEntryBufferPointer + i;
                if (nonEmptyEntry->BucketId != 0 && nonEmptyEntry->BucketFlags != BucketFlags.None)
                {
                    var addResult = *nonEmptyEntry;
                    reusableList.Add(addResult);
                }
            }

            return reusableList;
        }
    }

    IEnumerable<uint> IReadOnlyDictionary<uint, SubBucketIndexInfo>.Keys => Keys;

    IEnumerable<SubBucketIndexInfo> IReadOnlyDictionary<uint, SubBucketIndexInfo>.Values => Values;

    public bool IsFixedSize => true;

    public long SizeInBytes => CalculateDictionarySizeInBytes(maxPossibleIndexEntries, internalIndexFileCursor);

    public void CacheAndCloseFileView()
    {
        cacheBucketIndexInfos ??= new List<KeyValuePair<uint, SubBucketIndexInfo>>();
        cacheBucketIndexInfos.Clear();
        cacheBucketIndexInfos.AddRange(this);
        cacheLastAddedSubBucketIndexInfo = *lastAddedEntryBufferPointer;
        IsReadOnly = true;
        memoryMappedFileView = null;
        writableV1IndexHeaderSectionV1 = null;
        lastAddedEntryBufferPointer = null;
        firstEntryBufferPointer = null;
    }

    public void OpenWithFileView(ShiftableMemoryMappedFileView shiftableMemoryMappedFileView, bool isReadOnly)
    {
        memoryMappedFileView = shiftableMemoryMappedFileView;
        IsReadOnly = isReadOnly;
        writableV1IndexHeaderSectionV1 = (SubBucketIndexList*)memoryMappedFileView.FileCursorBufferPointer(internalIndexFileCursor, !isReadOnly);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        firstEntryBufferPointer = &writableV1IndexHeaderSectionV1->FirstIndexInList;
        lastAddedEntryBufferPointer = &writableV1IndexHeaderSectionV1->LastAddedBucketIndexInfo;
        maxPossibleIndexEntries = writableV1IndexHeaderSectionV1->MaxIndexSizeEntries;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, SubBucketIndexInfo>> GetEnumerator()
    {
        if (!IsFileViewOpen)
            return cacheBucketIndexInfos?.GetEnumerator() ?? Enumerable.Empty<KeyValuePair<uint, SubBucketIndexInfo>>().GetEnumerator();
        var autoRecycleEnumerator = recycler.Borrow<AutoRecycleEnumerator<KeyValuePair<uint, SubBucketIndexInfo>>>();
        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = firstEntryBufferPointer + i;
            if (indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None)
                autoRecycleEnumerator.Add(new KeyValuePair<uint, SubBucketIndexInfo>(i, *indexEntry));
        }

        return autoRecycleEnumerator;
    }

    public void Add(KeyValuePair<uint, SubBucketIndexInfo> item)
    {
        if (!IsFileViewOpen || item.Key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + item.Key;
        *indexEntry = item.Value;
        *lastAddedEntryBufferPointer = item.Value;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(SubBucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(SubBucketIndexInfo));
    }

    public void Clear()
    {
        if (!IsFileViewOpen || IsReadOnly) return;
        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = firstEntryBufferPointer + i;
            *indexEntry = default;
            memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(SubBucketIndexInfo));
        }

        *lastAddedEntryBufferPointer = default;
        memoryMappedFileView!.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(SubBucketIndexInfo));
    }

    public bool Contains(KeyValuePair<uint, SubBucketIndexInfo> item)
    {
        if (item.Key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen) return cacheBucketIndexInfos?.Any(kvp => kvp.Key == item.Key) ?? false;
        var indexEntry = firstEntryBufferPointer + item.Key;
        return indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None;
    }

    public void CopyTo(KeyValuePair<uint, SubBucketIndexInfo>[] array, int arrayIndex)
    {
        if (!IsFileViewOpen)
        {
            cacheBucketIndexInfos?.CopyTo(array, arrayIndex);
            return;
        }

        var i = arrayIndex;
        foreach (var kvp in this)
        {
            if (arrayIndex > array.Length) return;
            array[arrayIndex++] = kvp;
        }
    }

    public bool Remove(KeyValuePair<uint, SubBucketIndexInfo> item) => Remove(item.Key);

    public int Count
    {
        get
        {
            if (!IsFileViewOpen) return cacheBucketIndexInfos?.Count ?? 0;
            var countNonNull = 0;
            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var indexEntry = firstEntryBufferPointer + i;
                if (indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None) countNonNull++;
            }

            return countNonNull;
        }
    }

    public bool IsReadOnly { get; private set; }


    public bool ContainsKey(uint key)
    {
        if (key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen) return cacheBucketIndexInfos?.Any(kvp => kvp.Key == key) ?? false;
        var indexEntry = firstEntryBufferPointer + key;
        return indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None;
    }

    public bool Remove(uint key)
    {
        if (!IsFileViewOpen || key > maxPossibleIndexEntries || IsReadOnly) return false;
        var indexEntry = firstEntryBufferPointer + key;
        if (indexEntry->BucketId == 0 || indexEntry->BucketFlags == BucketFlags.None) return false;
        var isAlsoLastAdded = Equals(*lastAddedEntryBufferPointer, *indexEntry);
        *indexEntry = default;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(SubBucketIndexInfo));
        if (isAlsoLastAdded)
        {
            SubBucketIndexInfo* largestNonEmptyIndex = null;

            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = firstEntryBufferPointer + i;
                if (nonEmptyEntry->BucketId != 0 && nonEmptyEntry->BucketFlags != BucketFlags.None) largestNonEmptyIndex = nonEmptyEntry;
            }

            if (largestNonEmptyIndex != null)
                *lastAddedEntryBufferPointer = *largestNonEmptyIndex;
            else
                *lastAddedEntryBufferPointer = default;
            memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(SubBucketIndexInfo));
        }

        return true;
    }

    public ICollection<uint> Keys
    {
        get
        {
            if (!IsFileViewOpen)
            {
                cacheBucketIndexInfos ??= new List<KeyValuePair<uint, SubBucketIndexInfo>>();
                return cacheBucketIndexInfos.Select(kvp => kvp.Key).ToList();
            }

            var reusableList = recycler.Borrow<ReusableList<uint>>();
            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var indexEntry = firstEntryBufferPointer + i;
                if (indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None) reusableList.Add(i);
            }

            return reusableList;
        }
    }

    public void Add(uint key, SubBucketIndexInfo value)
    {
        if (!IsFileViewOpen || key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + key;
        *indexEntry = value;
        *lastAddedEntryBufferPointer = value;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(SubBucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(SubBucketIndexInfo));
    }

    public bool TryGetValue(uint key, out SubBucketIndexInfo value)
    {
        value = default;
        if (key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen)
        {
            value = cacheBucketIndexInfos?.FirstOrDefault(kvp => kvp.Key == key).Value ?? default;
            return cacheBucketIndexInfos?.Any(kvp => kvp.Key == key) ?? false;
        }

        var indexEntry = firstEntryBufferPointer + key;
        if (indexEntry->BucketId == 0 || indexEntry->BucketFlags == BucketFlags.None) return false;
        value = *indexEntry;
        return true;
    }


    public static uint CalculateDictionarySizeInBytes(uint maxEntries, long proposedStartPosition)
    {
        if (maxEntries == 0) return 0;
        var fileRealignment = proposedStartPosition % 8;
        return (uint)(sizeof(SubBucketIndexList) + sizeof(SubBucketIndexInfo) * (maxEntries - 1) + fileRealignment);
    }
}
