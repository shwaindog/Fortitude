// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketIndexList
{
    public uint MaxIndexSizeEntries;
    public int  IndexSHA256;
    public uint LastAddedBucketIndexInfoKey;

    public BucketIndexInfo FirstIndexInList;
}

public interface IReadonlyBucketIndexDictionary : IReadOnlyDictionary<uint, BucketIndexInfo>
{
    bool IsFixedSize { get; }
    long SizeInBytes { get; }

    bool IsFileViewOpen { get; }
    void CacheAndCloseFileView();
    void OpenWithFileView(ShiftableMemoryMappedFileView memoryMappedFileView, bool isReadOnly);
}

public unsafe interface IBucketIndexDictionary : IDictionary<uint, BucketIndexInfo>, IReadonlyBucketIndexDictionary
{
    BucketIndexInfo? LastAddedBucketIndexInfo { get; }
    uint             NextEmptyIndexKey        { get; }
    new BucketIndexInfo this[uint key] { get; set; }
    new ICollection<uint>            Keys   { get; }
    new ICollection<BucketIndexInfo> Values { get; }

    new int  Count      { get; }
    new bool IsReadOnly { get; }
    new bool ContainsKey(uint key);
    new bool TryGetValue(uint key, out BucketIndexInfo value);
    new bool Contains(KeyValuePair<uint, BucketIndexInfo> item);
    new void CopyTo(KeyValuePair<uint, BucketIndexInfo>[] array, int arrayIndex);

    new IEnumerator<KeyValuePair<uint, BucketIndexInfo>> GetEnumerator();

    BucketIndexInfo* GetBucketIndexInfo(uint key);
    void             DumpIndexToLogs();
    void             FlushIndexToDisk();
}

public unsafe class BucketIndexDictionary : IBucketIndexDictionary
{
    private const long IndexFileCursorAlignment = 8;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(BucketIndexDictionary));

    private static IRecycler recycler = new Recycler();

    private readonly List<KeyValuePair<uint, BucketIndexInfo>> cacheBucketIndexInfos = new();
    private readonly long                                      internalIndexFileCursor;

    private BucketIndexInfo? cacheLastAddedEntry;
    private BucketIndexInfo* firstEntryBufferPointer;

    private long  firstEntryFileOffset;
    private uint  lastAddedEntryIndexKey = uint.MaxValue;
    private uint* lastAddedEntryIndexKeyBufferPtr;
    private uint  maxPossibleIndexEntries;

    private ShiftableMemoryMappedFileView? memoryMappedFileView;

    private long             requiredViewFileCursorOffset;
    private byte*            requiredViewStartAddress;
    private BucketIndexList* writableV1IndexHeaderSectionV1;

    public BucketIndexDictionary(ShiftableMemoryMappedFileView memoryMappedFileView,
        long fileStartCursorOffset, uint maxPossibleIndexEntries, bool isReadOnly)
    {
        var headerRealignmentOffset = fileStartCursorOffset % IndexFileCursorAlignment > 0 ? 8 - fileStartCursorOffset % IndexFileCursorAlignment : 0;
        internalIndexFileCursor = fileStartCursorOffset + headerRealignmentOffset;
        OpenWithFileView(memoryMappedFileView, isReadOnly);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        this.maxPossibleIndexEntries                        = maxPossibleIndexEntries;
    }

    public bool IsFileViewOpen =>
        memoryMappedFileView != null && firstEntryBufferPointer != null &&
        memoryMappedFileView.LowerViewFileCursorOffset == requiredViewFileCursorOffset &&
        memoryMappedFileView.StartAddress == requiredViewStartAddress;

    public bool IsFixedSize => true;

    public long SizeInBytes => CalculateDictionarySizeInBytes(maxPossibleIndexEntries, internalIndexFileCursor);

    public uint NextEmptyIndexKey
    {
        get
        {
            if (!IsFileViewOpen) return cacheBucketIndexInfos.Max(bio => bio.Key) + 1;
            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var indexEntry = firstEntryBufferPointer + i;
                if (indexEntry->BucketId == 0) return i;
            }

            return uint.MaxValue;
        }
    }

    public BucketIndexInfo? LastAddedBucketIndexInfo
    {
        get
        {
            if (!IsFileViewOpen) return cacheLastAddedEntry;
            var checkLastAddedEntryNotNull = this[lastAddedEntryIndexKey];
            if (checkLastAddedEntryNotNull.BucketId == 0 || checkLastAddedEntryNotNull.BucketFlags == BucketFlags.None) return null;
            return cacheLastAddedEntry = checkLastAddedEntryNotNull;
        }
    }

    public void CacheAndCloseFileView()
    {
        if (IsFileViewOpen)
        {
            cacheLastAddedEntry = lastAddedEntryIndexKey != uint.MaxValue ? this[lastAddedEntryIndexKey] : default(BucketIndexInfo?);
            if (!IsReadOnly) FlushIndexToDisk();
            cacheBucketIndexInfos.Clear();
            cacheBucketIndexInfos.AddRange(this);
        }

        IsReadOnly                      = true;
        memoryMappedFileView            = null;
        writableV1IndexHeaderSectionV1  = null;
        lastAddedEntryIndexKeyBufferPtr = null;
        firstEntryBufferPointer         = null;
    }

    public void OpenWithFileView(ShiftableMemoryMappedFileView shiftableMemoryMappedFileView, bool isReadOnly)
    {
        memoryMappedFileView         = shiftableMemoryMappedFileView;
        requiredViewFileCursorOffset = memoryMappedFileView.LowerViewFileCursorOffset;
        requiredViewStartAddress     = memoryMappedFileView.StartAddress;

        IsReadOnly = isReadOnly;
        writableV1IndexHeaderSectionV1
            = (BucketIndexList*)memoryMappedFileView.FileCursorBufferPointer(internalIndexFileCursor, shouldGrow: !isReadOnly);
        firstEntryFileOffset = internalIndexFileCursor +
                               (nint)((byte*)&writableV1IndexHeaderSectionV1->FirstIndexInList - (byte*)writableV1IndexHeaderSectionV1);
        // Logger.Info("Opening BucketIndex at FileCursor {0}", internalIndexFileCursor);
        writableV1IndexHeaderSectionV1->MaxIndexSizeEntries = maxPossibleIndexEntries;
        requiredViewFileCursorOffset                        = memoryMappedFileView.LowerViewFileCursorOffset;
        firstEntryBufferPointer                             = &writableV1IndexHeaderSectionV1->FirstIndexInList;
        lastAddedEntryIndexKeyBufferPtr                     = &writableV1IndexHeaderSectionV1->LastAddedBucketIndexInfoKey;
        lastAddedEntryIndexKey                              = *lastAddedEntryIndexKeyBufferPtr;
        maxPossibleIndexEntries                             = writableV1IndexHeaderSectionV1->MaxIndexSizeEntries;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<uint, BucketIndexInfo>> GetEnumerator()
    {
        if (!IsFileViewOpen) return cacheBucketIndexInfos.GetEnumerator();
        EnsureViewIsCorrect();
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
        EnsureViewIsCorrect();
        if (!IsFileViewOpen || item.Key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + item.Key;
        *indexEntry                      = item.Value;
        lastAddedEntryIndexKey           = item.Key;
        *lastAddedEntryIndexKeyBufferPtr = item.Key;
        cacheBucketIndexInfos.Add(item);
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryIndexKeyBufferPtr, sizeof(uint));
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

        *lastAddedEntryIndexKeyBufferPtr = default;
        memoryMappedFileView!.FlushPtrDataToDisk(lastAddedEntryIndexKeyBufferPtr, sizeof(uint));
    }

    public bool Contains(KeyValuePair<uint, BucketIndexInfo> item)
    {
        if (item.Key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen) return cacheBucketIndexInfos.Any(kvp => kvp.Key == item.Key);
        var indexEntry = firstEntryBufferPointer + item.Key;
        return indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None;
    }

    public void CopyTo(KeyValuePair<uint, BucketIndexInfo>[] array, int arrayIndex)
    {
        if (!IsFileViewOpen)
        {
            cacheBucketIndexInfos.CopyTo(array, arrayIndex);
            return;
        }
        EnsureViewIsCorrect();

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
            if (!IsFileViewOpen) return cacheBucketIndexInfos.Count;
            EnsureViewIsCorrect();
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
        EnsureViewIsCorrect();
        if (!IsFileViewOpen || key > maxPossibleIndexEntries || IsReadOnly) return;
        var indexEntry = firstEntryBufferPointer + key;
        *indexEntry                      = value;
        lastAddedEntryIndexKey           = key;
        *lastAddedEntryIndexKeyBufferPtr = key;
        cacheBucketIndexInfos.Add(new KeyValuePair<uint, BucketIndexInfo>(key, value));
        // Logger.Info("BucketIndex at {0} with key {1} at file offset {2} added {3}", internalIndexFileCursor, key,
        //             (nint)(firstEntryFileOffset + ((byte*)indexEntry - (byte*)firstEntryBufferPointer)), value);
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryIndexKeyBufferPtr, sizeof(uint));
    }

    public bool ContainsKey(uint key)
    {
        if (key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen) return cacheBucketIndexInfos.Any(kvp => kvp.Key == key);
        EnsureViewIsCorrect();
        var indexEntry = firstEntryBufferPointer + key;
        return indexEntry->BucketId != 0 && indexEntry->BucketFlags != BucketFlags.None;
    }

    public bool Remove(uint key)
    {
        if (!IsFileViewOpen || key > maxPossibleIndexEntries || IsReadOnly) return false;
        EnsureViewIsCorrect();
        var indexEntry = firstEntryBufferPointer + key;
        if (indexEntry->BucketId == 0 || indexEntry->BucketFlags == BucketFlags.None) return false;

        var isAlsoLastAdded = Equals(*lastAddedEntryIndexKeyBufferPtr, key);
        *indexEntry = default;
        memoryMappedFileView!.FlushPtrDataToDisk(indexEntry, sizeof(BucketIndexInfo));
        if (isAlsoLastAdded)
        {
            uint largestNonEmptyIndexKey = 0;

            for (uint i = 0; i < maxPossibleIndexEntries; i++)
            {
                var nonEmptyEntry = firstEntryBufferPointer + i;
                if (nonEmptyEntry->BucketId != 0 && nonEmptyEntry->BucketFlags != BucketFlags.None) largestNonEmptyIndexKey = i;
            }

            *lastAddedEntryIndexKeyBufferPtr = largestNonEmptyIndexKey;
            memoryMappedFileView.FlushPtrDataToDisk(lastAddedEntryIndexKeyBufferPtr, sizeof(uint));
        }

        return true;
    }

    public bool TryGetValue(uint key, out BucketIndexInfo value)
    {
        value = default;
        if (key > maxPossibleIndexEntries) return false;
        if (!IsFileViewOpen)
        {
            value = cacheBucketIndexInfos.FirstOrDefault(kvp => kvp.Key == key).Value;
            return cacheBucketIndexInfos.Any(kvp => kvp.Key == key);
        }
        EnsureViewIsCorrect();

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
                returnResult = cacheBucketIndexInfos.FirstOrDefault(kvp => kvp.Key == key).Value;
                return returnResult;
            }
            EnsureViewIsCorrect();

            var indexEntry = firstEntryBufferPointer + key;
            if (indexEntry->BucketId == 0 || indexEntry->BucketFlags == BucketFlags.None) return returnResult;
            returnResult = *indexEntry;
            return returnResult;
        }
        set => Add(key, value);
    }

    public BucketIndexInfo* GetBucketIndexInfo(uint bucketId)
    {
        if (!IsFileViewOpen) return null;

        EnsureViewIsCorrect();
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
            if (!IsFileViewOpen) return cacheBucketIndexInfos.Select(kvp => kvp.Key).ToList();

            EnsureViewIsCorrect();
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
            if (!IsFileViewOpen) return cacheBucketIndexInfos.Select(kvp => kvp.Value).ToList();

            EnsureViewIsCorrect();
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

    public void DumpIndexToLogs()
    {
        if (!IsFileViewOpen)
        {
            Logger.Warn("Could not dump index logs because view name {0} with file LowerViewFileCursorOffset is {1} and requires {2}",
                        memoryMappedFileView?.ViewName, memoryMappedFileView?.LowerViewFileCursorOffset, requiredViewFileCursorOffset);
            return;
        }

        for (uint i = 0; i < maxPossibleIndexEntries; i++)
        {
            var nonEmptyEntry = firstEntryBufferPointer + i;
            var addResult     = *nonEmptyEntry;
            Logger.Info("BucketIndex at {0} with key {1} at file offset {2} dumping {3}", internalIndexFileCursor, i,
                        (nint)(firstEntryFileOffset + ((byte*)nonEmptyEntry - (byte*)firstEntryBufferPointer)), addResult);
        }
    }

    IEnumerable<uint> IReadOnlyDictionary<uint, BucketIndexInfo>.Keys => Keys;

    IEnumerable<BucketIndexInfo> IReadOnlyDictionary<uint, BucketIndexInfo>.Values => Values;

    public void FlushIndexToDisk()
    {
        if (IsFileViewOpen) memoryMappedFileView!.FlushCursorDataToDisk(internalIndexFileCursor, (int)SizeInBytes);
        // DumpIndexToLogs();
    }

    private void EnsureViewIsCorrect()
    {
        if (memoryMappedFileView!.LowerViewFileCursorOffset != requiredViewFileCursorOffset ||
            memoryMappedFileView.StartAddress != requiredViewStartAddress)
            throw new ArgumentException("View has been moved and should not be accessed!");
        // Debugger.Break();
    }

    public static uint CalculateDictionarySizeInBytes(uint maxEntries, long proposedStartPosition)
    {
        if (maxEntries == 0) return 0;
        var fileRealignment = proposedStartPosition % 8 > 0 ? 8 - proposedStartPosition % 8 : 0;
        return (uint)(sizeof(BucketIndexList) + sizeof(BucketIndexInfo) * (maxEntries - 1) + fileRealignment);
    }
}
