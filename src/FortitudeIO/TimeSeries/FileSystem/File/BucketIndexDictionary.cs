#region

using System.Collections;
using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public unsafe class BucketIndexDictionary : IDictionary<uint, BucketIndexOffset>, IReadOnlyDictionary<uint, BucketIndexOffset>
{
    private const long FirstEntryFileCursorOffset = 8;
    private static IRecycler recycler = new Recycler();
    private readonly uint maxPossibleIndexEntries;
    private readonly ShiftableMemoryMappedFileView memoryMappedFileView;
    private byte* firstEntryBufferPointer;
    private long firstEntryFileCursor;
    private InternalIndexHeaderSectionV1 writableV1IndexHeaderSectionV1;

    public BucketIndexDictionary(ShiftableMemoryMappedFileView memoryMappedFileView,
        long fileStartCursorOffset, uint maxPossibleIndexEntries, bool isReadOnly)
    {
        this.memoryMappedFileView = memoryMappedFileView;
        this.maxPossibleIndexEntries = maxPossibleIndexEntries;
        IsReadOnly = isReadOnly;
        firstEntryFileCursor = fileStartCursorOffset + FirstEntryFileCursorOffset;
        var indexFileBufferPointer = memoryMappedFileView.FileCursorBufferPointer(fileStartCursorOffset);
        firstEntryBufferPointer = indexFileBufferPointer + FirstEntryFileCursorOffset;
        writableV1IndexHeaderSectionV1 = Unsafe.AsRef<InternalIndexHeaderSectionV1>(indexFileBufferPointer);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, BucketIndexOffset>> GetEnumerator()
    {
        var autoRecycleEnumerator = recycler.Borrow<AutoRecycleEnumerator<KeyValuePair<uint, BucketIndexOffset>>>();
        for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry && i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
            if (indexEntry.BucketId != 0 && indexEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry)
                autoRecycleEnumerator.Add(new KeyValuePair<uint, BucketIndexOffset>(i, indexEntry));
        }

        return autoRecycleEnumerator;
    }

    public void Add(KeyValuePair<uint, BucketIndexOffset> item)
    {
        if (item.Key > maxPossibleIndexEntries || IsReadOnly) return;
        var bytesFromFirstIndexEntry = item.Key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        indexEntry.BucketId = item.Value.BucketId;
        indexEntry.BucketSize = item.Value.BucketSize;
        indexEntry.FileOffset = item.Value.FileOffset;
        indexEntry.EarliestEntryDateTime = item.Value.EarliestEntryDateTime;
        indexEntry.BucketIndexFlags = item.Value.BucketIndexFlags;
        memoryMappedFileView.FlushCursorDataToDisk(firstEntryFileCursor + bytesFromFirstIndexEntry, sizeof(BucketIndexOffset));
        if (item.Key > writableV1IndexHeaderSectionV1.LargestIndexSetEntry)
            writableV1IndexHeaderSectionV1.LargestIndexSetEntry = item.Key;
    }

    public void Clear()
    {
        if (IsReadOnly) return;
        for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry && i < maxPossibleIndexEntries; i++)
        {
            var indexEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
            indexEntry.BucketId = 0;
            indexEntry.BucketSize = 0;
            indexEntry.FileOffset = 0;
            indexEntry.EarliestEntryDateTime = default;
            indexEntry.BucketIndexFlags = BucketIndexFlags.EmptyEntry;
        }

        writableV1IndexHeaderSectionV1.LargestIndexSetEntry = 0;
    }

    public bool Contains(KeyValuePair<uint, BucketIndexOffset> item)
    {
        if (item.Key > maxPossibleIndexEntries) return false;
        var bytesFromFirstIndexEntry = item.Key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        return indexEntry.BucketId != 0 && indexEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry;
    }

    public void CopyTo(KeyValuePair<uint, BucketIndexOffset>[] array, int arrayIndex)
    {
        var i = arrayIndex;
        foreach (var kvp in this)
        {
            if (arrayIndex > array.Length) return;
            array[arrayIndex++] = kvp;
        }
    }

    public bool Remove(KeyValuePair<uint, BucketIndexOffset> item)
    {
        if (item.Key > maxPossibleIndexEntries || IsReadOnly) return false;
        var bytesFromFirstIndexEntry = item.Key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        if (indexEntry.BucketId == 0 || indexEntry.BucketIndexFlags == BucketIndexFlags.EmptyEntry) return false;
        indexEntry.BucketId = 0;
        indexEntry.BucketSize = 0;
        indexEntry.FileOffset = 0;
        indexEntry.EarliestEntryDateTime = default;
        indexEntry.BucketIndexFlags = BucketIndexFlags.EmptyEntry;
        memoryMappedFileView.FlushCursorDataToDisk(firstEntryFileCursor + bytesFromFirstIndexEntry, sizeof(BucketIndexOffset));
        if (item.Key == writableV1IndexHeaderSectionV1.LargestIndexSetEntry)
        {
            uint largestNonEmptyEntrySoFar = 0;
            for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry - 1 && i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
                if (nonEmptyEntry.BucketId != 0 && nonEmptyEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry) largestNonEmptyEntrySoFar = i;
            }

            writableV1IndexHeaderSectionV1.LargestIndexSetEntry = largestNonEmptyEntrySoFar;
        }

        return true;
    }

    public int Count
    {
        get
        {
            var countNonNull = 0;
            for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry && i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
                if (nonEmptyEntry.BucketId != 0 && nonEmptyEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry) countNonNull++;
            }

            return countNonNull;
        }
    }

    public bool IsReadOnly { get; }

    public void Add(uint key, BucketIndexOffset value)
    {
        if (key > maxPossibleIndexEntries || IsReadOnly) return;
        var bytesFromFirstIndexEntry = key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        indexEntry.BucketId = value.BucketId;
        indexEntry.BucketSize = value.BucketSize;
        indexEntry.FileOffset = value.FileOffset;
        indexEntry.EarliestEntryDateTime = value.EarliestEntryDateTime;
        indexEntry.BucketIndexFlags = value.BucketIndexFlags;
        memoryMappedFileView.FlushCursorDataToDisk(firstEntryFileCursor + bytesFromFirstIndexEntry, sizeof(BucketIndexOffset));
        if (key > writableV1IndexHeaderSectionV1.LargestIndexSetEntry)
            writableV1IndexHeaderSectionV1.LargestIndexSetEntry = key;
    }

    public bool ContainsKey(uint key)
    {
        if (key > maxPossibleIndexEntries) return false;
        var bytesFromFirstIndexEntry = key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        return indexEntry.BucketId != 0 && indexEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry;
    }

    public bool Remove(uint key)
    {
        if (key > maxPossibleIndexEntries || IsReadOnly) return false;
        var bytesFromFirstIndexEntry = key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        if (indexEntry.BucketId == 0 || indexEntry.BucketIndexFlags == BucketIndexFlags.EmptyEntry) return false;
        indexEntry.BucketId = 0;
        indexEntry.BucketSize = 0;
        indexEntry.FileOffset = 0;
        indexEntry.EarliestEntryDateTime = default;
        indexEntry.BucketIndexFlags = BucketIndexFlags.EmptyEntry;
        memoryMappedFileView.FlushCursorDataToDisk(firstEntryFileCursor + bytesFromFirstIndexEntry, sizeof(BucketIndexOffset));
        if (key == writableV1IndexHeaderSectionV1.LargestIndexSetEntry)
        {
            uint largestNonEmptyEntrySoFar = 0;
            for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry - 1 && i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
                if (nonEmptyEntry.BucketId != 0 && nonEmptyEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry) largestNonEmptyEntrySoFar = i;
            }

            writableV1IndexHeaderSectionV1.LargestIndexSetEntry = largestNonEmptyEntrySoFar;
        }

        return true;
    }

    public bool TryGetValue(uint key, out BucketIndexOffset value)
    {
        value = default;
        if (key > maxPossibleIndexEntries) return false;
        var bytesFromFirstIndexEntry = key * sizeof(BucketIndexOffset);
        var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
        var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
        if (indexEntry.BucketId == 0 || indexEntry.BucketIndexFlags == BucketIndexFlags.EmptyEntry) return false;
        value.BucketId = indexEntry.BucketId;
        value.BucketSize = indexEntry.BucketSize;
        value.FileOffset = indexEntry.FileOffset;
        value.EarliestEntryDateTime = indexEntry.EarliestEntryDateTime;
        value.BucketIndexFlags = indexEntry.BucketIndexFlags;
        return true;
    }

    public BucketIndexOffset this[uint key]
    {
        get
        {
            BucketIndexOffset returnResult = default;
            if (key > maxPossibleIndexEntries) return returnResult;
            var bytesFromFirstIndexEntry = key * sizeof(BucketIndexOffset);
            var addIndexEntryOffset = firstEntryBufferPointer + bytesFromFirstIndexEntry;
            var indexEntry = Unsafe.AsRef<BucketIndexOffset>(addIndexEntryOffset);
            if (indexEntry.BucketId == 0 || indexEntry.BucketIndexFlags == BucketIndexFlags.EmptyEntry) return returnResult;
            returnResult.BucketId = indexEntry.BucketId;
            returnResult.BucketSize = indexEntry.BucketSize;
            returnResult.FileOffset = indexEntry.FileOffset;
            returnResult.EarliestEntryDateTime = indexEntry.EarliestEntryDateTime;
            returnResult.BucketIndexFlags = indexEntry.BucketIndexFlags;
            return returnResult;
        }
        set => Add(key, value);
    }

    public ICollection<uint> Keys
    {
        get
        {
            var reusableList = recycler.Borrow<ReusableList<uint>>();
            for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry && i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
                if (nonEmptyEntry.BucketId != 0 && nonEmptyEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry) reusableList.Add(i);
            }

            return reusableList;
        }
    }

    public ICollection<BucketIndexOffset> Values
    {
        get
        {
            var reusableList = recycler.Borrow<ReusableList<BucketIndexOffset>>();
            for (uint i = 0; i < writableV1IndexHeaderSectionV1.LargestIndexSetEntry && i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = Unsafe.AsRef<BucketIndexOffset>(firstEntryBufferPointer + i * sizeof(BucketIndexOffset));
                if (nonEmptyEntry.BucketId != 0 && nonEmptyEntry.BucketIndexFlags != BucketIndexFlags.EmptyEntry)
                {
                    BucketIndexOffset addResult = default;
                    addResult.BucketId = nonEmptyEntry.BucketId;
                    addResult.BucketSize = nonEmptyEntry.BucketSize;
                    addResult.FileOffset = nonEmptyEntry.FileOffset;
                    addResult.EarliestEntryDateTime = nonEmptyEntry.EarliestEntryDateTime;
                    addResult.BucketIndexFlags = nonEmptyEntry.BucketIndexFlags;

                    reusableList.Add(addResult);
                }
            }

            return reusableList;
        }
    }

    IEnumerable<uint> IReadOnlyDictionary<uint, BucketIndexOffset>.Keys => Keys;

    IEnumerable<BucketIndexOffset> IReadOnlyDictionary<uint, BucketIndexOffset>.Values => Values;
}
