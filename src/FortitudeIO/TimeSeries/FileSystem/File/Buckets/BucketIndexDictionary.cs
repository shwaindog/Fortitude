#region

using System.Collections;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketIndexList
{
    public uint MaxIndexSizeEntries;
    public BucketIndexInfo LastAddedBucketIndexInfo;
    public BucketIndexInfo FirstIndexInList;
}

public interface IReadonlyBucketIndexDictionary : IReadOnlyDictionary<uint, BucketIndexInfo>
{
    bool IsFixedSize { get; }
    long SizeInBytes { get; }
    void CacheAndCloseFileView();
    void OpenWithFileView(ShiftableMemoryMappedFileView memoryMappedFileView, bool isReadOnly);
}

public unsafe interface IBucketIndexDictionary : IDictionary<uint, BucketIndexInfo>, IReadonlyBucketIndexDictionary
{
    BucketIndexInfo? LastAddedBucketIndexInfo { get; }
    uint NextEmptyIndexKey { get; }
    new BucketIndexInfo this[uint key] { get; set; }
    new ICollection<uint> Keys { get; }
    new ICollection<BucketIndexInfo> Values { get; }
    new int Count { get; }
    new bool IsReadOnly { get; }
    new bool ContainsKey(uint key);
    new bool TryGetValue(uint key, out BucketIndexInfo value);
    new bool Contains(KeyValuePair<uint, BucketIndexInfo> item);
    new void CopyTo(KeyValuePair<uint, BucketIndexInfo>[] array, int arrayIndex);
    new IEnumerator<KeyValuePair<uint, BucketIndexInfo>> GetEnumerator();
    BucketIndexInfo* GetBucketIndexInfo(uint key);
}

public unsafe class BucketIndexDictionary : IBucketIndexDictionary
{
    private const long IndexFileCursorAlignment = 8;
    private static IRecycler recycler = new Recycler();
    private readonly long internalIndexFileCursor;
    private List<KeyValuePair<uint, BucketIndexInfo>>? cacheBucketIndexInfos;
    private BucketIndexInfo? cacheLastAddedEntry;
    private BucketIndexInfo* firstEntryBufferPointer;
    private BucketIndexInfo* lastAddedEntryBufferPointer;
    private uint maxPossibleIndexEntries;
    private ShiftableMemoryMappedFileView? memoryMappedFileView;
    private BucketIndexList* writableV1IndexHeaderSectionV1;

    public BucketIndexDictionary(ShiftableMemoryMappedFileView memoryMappedFileView,
        long fileStartCursorOffset, uint maxPossibleIndexEntries, bool isReadOnly)
    {
        internalIndexFileCursor = fileStartCursorOffset + fileStartCursorOffset % IndexFileCursorAlignment;
        OpenWithFileView(memoryMappedFileView, isReadOnly);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        this.maxPossibleIndexEntries = maxPossibleIndexEntries;
    }

    public bool IsFileViewOpen => memoryMappedFileView != null && firstEntryBufferPointer != null;

    public bool IsFixedSize => true;

    public long SizeInBytes => CalculateDictionarySizeInBytes(maxPossibleIndexEntries, internalIndexFileCursor);

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

    public BucketIndexInfo? LastAddedBucketIndexInfo
    {
        get
        {
            if (!IsFileViewOpen) return cacheLastAddedEntry;
            var checkLastAddedEntryNotNull = *lastAddedEntryBufferPointer;
            if (checkLastAddedEntryNotNull.BucketId == 0 || checkLastAddedEntryNotNull.BucketFlags == BucketFlags.None) return null;
            return cacheLastAddedEntry = checkLastAddedEntryNotNull;
        }
    }

    public void CacheAndCloseFileView()
    {
        cacheBucketIndexInfos ??= new List<KeyValuePair<uint, BucketIndexInfo>>();
        cacheBucketIndexInfos.Clear();
        cacheBucketIndexInfos.AddRange(this);
        if (lastAddedEntryBufferPointer != null)
            cacheLastAddedEntry = *lastAddedEntryBufferPointer;
        else
            cacheLastAddedEntry = default;

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
        writableV1IndexHeaderSectionV1 = (BucketIndexList*)memoryMappedFileView.FileCursorBufferPointer(internalIndexFileCursor, !isReadOnly);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        firstEntryBufferPointer = &writableV1IndexHeaderSectionV1->FirstIndexInList;
        lastAddedEntryBufferPointer = &writableV1IndexHeaderSectionV1->LastAddedBucketIndexInfo;
        maxPossibleIndexEntries = writableV1IndexHeaderSectionV1->MaxIndexSizeEntries;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, BucketIndexInfo>> GetEnumerator()
    {
        if (!IsFileViewOpen) return cacheBucketIndexInfos?.GetEnumerator() ?? Enumerable.Empty<KeyValuePair<uint, BucketIndexInfo>>().GetEnumerator();
        var autoRecycleEnumerator = recycler.Borrow<AutoRecycleEnumerator<KeyValuePair<uint, BucketIndexInfo>>>();
        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = firstEntryBufferPointer + i;
            if (indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None)
                autoRecycleEnumerator.Add(new KeyValuePair<uint, BucketIndexInfo>(i, *indexEntry));
        }

        return autoRecycleEnumerator;
    }

    public void Add(KeyValuePair<uint, BucketIndexInfo> item)
    {
        if (!IsFileViewOpen || item.Key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + item.Key;
        *indexEntry = item.Value;
        *lastAddedEntryBufferPointer = item.Value;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(BucketIndexInfo));
    }

    public void Clear()
    {
        if (!IsFileViewOpen || IsReadOnly) return;
        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = firstEntryBufferPointer + i;
            *indexEntry = default;
            memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        }

        *lastAddedEntryBufferPointer = default;
        memoryMappedFileView!.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(BucketIndexInfo));
    }

    public bool Contains(KeyValuePair<uint, BucketIndexInfo> item)
    {
        if (item.Key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen) return cacheBucketIndexInfos?.Any(kvp => kvp.Key == item.Key) ?? false;
        var indexEntry = firstEntryBufferPointer + item.Key;
        return indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None;
    }

    public void CopyTo(KeyValuePair<uint, BucketIndexInfo>[] array, int arrayIndex)
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

    public bool Remove(KeyValuePair<uint, BucketIndexInfo> item) => Remove(item.Key);

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

    public void Add(uint key, BucketIndexInfo value)
    {
        if (!IsFileViewOpen || key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + key;
        *indexEntry = value;
        *lastAddedEntryBufferPointer = value;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(BucketIndexInfo));
    }

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
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        if (isAlsoLastAdded)
        {
            BucketIndexInfo* largestNonEmptyIndex = null;

            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = firstEntryBufferPointer + i;
                if (nonEmptyEntry->BucketId != 0 && nonEmptyEntry->BucketFlags != BucketFlags.None) largestNonEmptyIndex = nonEmptyEntry;
            }

            if (largestNonEmptyIndex != null)
                *lastAddedEntryBufferPointer = *largestNonEmptyIndex;
            else
                *lastAddedEntryBufferPointer = default;
            memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryBufferPointer, sizeof(BucketIndexInfo));
        }

        return true;
    }

    public bool TryGetValue(uint key, out BucketIndexInfo value)
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

    public BucketIndexInfo this[uint key]
    {
        get
        {
            BucketIndexInfo returnResult = default;
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

    public BucketIndexInfo* GetBucketIndexInfo(uint bucketId)
    {
        BucketIndexInfo returnResult = default;
        if (!IsFileViewOpen) return null;

        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = firstEntryBufferPointer + i;
            if (indexEntry->BucketId == bucketId) return indexEntry;
        }

        return null;
    }

    public ICollection<uint> Keys
    {
        get
        {
            if (!IsFileViewOpen)
            {
                cacheBucketIndexInfos ??= new List<KeyValuePair<uint, BucketIndexInfo>>();
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

    public ICollection<BucketIndexInfo> Values
    {
        get
        {
            if (!IsFileViewOpen)
            {
                cacheBucketIndexInfos ??= new List<KeyValuePair<uint, BucketIndexInfo>>();
                return cacheBucketIndexInfos.Select(kvp => kvp.Value).ToList();
            }

            var reusableList = recycler.Borrow<ReusableList<BucketIndexInfo>>();
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

    IEnumerable<uint> IReadOnlyDictionary<uint, BucketIndexInfo>.Keys => Keys;

    IEnumerable<BucketIndexInfo> IReadOnlyDictionary<uint, BucketIndexInfo>.Values => Values;

    public static uint CalculateDictionarySizeInBytes(uint maxEntries, long proposedStartPosition)
    {
        if (maxEntries == 0) return 0;
        var fileRealignment = proposedStartPosition % 8;
        return (uint)(sizeof(BucketIndexList) + sizeof(BucketIndexInfo) * (maxEntries - 1) + fileRealignment);
    }
}
