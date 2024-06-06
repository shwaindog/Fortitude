// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PQDataBucket<TEntry, TBucket, TSerializeType> : DataBucket<TEntry, TBucket>, IPriceQuoteBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceQuoteBucket<TEntry>
    where TSerializeType : PQLevel0Quote, new()
{
    private IMessageBufferContext? bufferContext;
    private PQQuoteSerializer?     indexEntrySerializer;
    private TSerializeType?        lastEntryLevel0Quote;

    private IPQDeserializer<TSerializeType>? messageDeserializer;

    private PQQuoteSerializer? repeatedEntrySerializer;

    protected PQDataBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public IPQDeserializer<TSerializeType> DefaultMessageDeserializer
    {
        get
        {
            if (messageDeserializer == null)
            {
                var srcTkrQtInfo = SourceTickerQuoteInfo;
                messageDeserializer = new PQQuoteStorageDeserializer<TSerializeType>(srcTkrQtInfo);
            }

            return messageDeserializer;
        }
    }

    public TSerializeType LastEntryQuote
    {
        get
        {
            return lastEntryLevel0Quote ??= new TSerializeType()
            {
                SourceTickerQuoteInfo = SourceTickerQuoteInfo
            };
        }
    }

    public PQQuoteSerializer IndexEntryMessageSerializer =>
        indexEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Complete, PQSerializationFlags.ForStorageIncludeReceiverTimes);

    public PQQuoteSerializer RepeatedEntryMessageSerializer =>
        repeatedEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Update, PQSerializationFlags.ForStorageIncludeReceiverTimes);

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; set; } = null!;

    public override IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext)
    {
        bufferContext ??= new MessageBufferContext(readBuffer);

        bufferContext.EncodedBuffer = readBuffer;

        if (readBuffer.ReadCursor == 0) DefaultMessageDeserializer.PublishedQuote.StateReset();
        return ReadEntries(bufferContext, readerContext, DefaultMessageDeserializer);
    }

    public override StorageAttemptResult AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return StorageAttemptResult.BucketClosedForAppend;
        var entryStorageTime = entry.StorageTime(StorageTimeResolver);
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return checkWithinRange;
        if (entry.SourceTickerQuoteInfo!.SourceId != SourceTickerQuoteInfo.SourceId ||
            entry.SourceTickerQuoteInfo.TickerId != SourceTickerQuoteInfo.TickerId)
            return StorageAttemptResult.EntryNotCompatible;

        bufferContext               ??= new MessageBufferContext(growableBuffer);
        bufferContext.EncodedBuffer =   growableBuffer;

        var messageSerializer = RepeatedEntryMessageSerializer;
        if (growableBuffer.WriteCursor == 0)
        {
            LastEntryQuote.StateReset();
            LastEntryQuote.CopyFrom(entry, CopyMergeFlags.FullReplace);
            messageSerializer = IndexEntryMessageSerializer;
        }
        else
        {
            LastEntryQuote.HasUpdates = false;
            LastEntryQuote.CopyFrom(entry);
        }
        return AppendEntry(bufferContext, LastEntryQuote, messageSerializer);
    }

    public virtual IEnumerable<TEntry> ReadEntries(IMessageBufferContext buffer, IReaderContext<TEntry> readerContext
      , IPQDeserializer<TSerializeType> bufferDeserializer)
    {
        while (readerContext.ContinueSearching && buffer.EncodedBuffer!.ReadCursor < buffer.EncodedBuffer.WriteCursor)
        {
            bufferDeserializer.PublishedQuote.HasUpdates = false;
            bufferDeserializer.Deserialize(buffer);
            var toReturn = readerContext.GetNextEntryToPopulate;
            toReturn.CopyFrom(bufferDeserializer.PublishedQuote, CopyMergeFlags.FullReplace);
            if (readerContext.ProcessCandidateEntry(toReturn)) yield return toReturn;
        }
    }

    public virtual StorageAttemptResult AppendEntry(IMessageBufferContext bufferContext,
        TSerializeType lastEntryLevel, PQQuoteSerializer useSerializer)
    {
        useSerializer.Serialize(lastEntryLevel, bufferContext);
        if (bufferContext.LastWriteLength <= 0) return StorageAttemptResult.StorageSizeFailure;
        ExpandedDataSize      += (ulong)bufferContext.LastWriteLength;
        TotalDataEntriesCount += 1;
        return StorageAttemptResult.PeriodRangeMatched;
    }
}
