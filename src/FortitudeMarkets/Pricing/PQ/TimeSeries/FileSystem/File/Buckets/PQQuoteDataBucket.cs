// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PQQuoteDataBucket<TEntry, TBucket, TSerializeType> : DataBucket<TEntry, TBucket>, IPriceQuoteBucket<TEntry>
    where TEntry : ITimeSeriesEntry, ITickInstant
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceQuoteBucket<TEntry>
    where TSerializeType : PQTickInstant, new()
{
    private IMessageBufferContext? bufferContext;
    private PQQuoteSerializer?     indexEntrySerializer;

    private IPQQuoteDeserializer<TSerializeType>? messageDeserializer;

    private PQQuoteSerializer? repeatedEntrySerializer;

    protected PQQuoteDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public IPQQuoteDeserializer<TSerializeType> DefaultMessageDeserializer
    {
        get
        {
            if (messageDeserializer == null)
            {
                var srcTkrInfo = PricingInstrumentId as ISourceTickerInfo ?? new SourceTickerInfo(PricingInstrumentId);
                messageDeserializer = new PQQuoteStorageDeserializer<TSerializeType>(srcTkrInfo);
            }

            return messageDeserializer;
        }
    }

    public PQQuoteSerializer IndexEntryMessageSerializer =>
        indexEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Complete, PQSerializationFlags.ForStorageIncludeReceiverTimes);

    public PQQuoteSerializer RepeatedEntryMessageSerializer =>
        repeatedEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Update, PQSerializationFlags.ForStorageIncludeReceiverTimes);

    public IPricingInstrumentId PricingInstrumentId { get; set; } = null!;

    public override IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext)
    {
        bufferContext ??= new MessageBufferContext(readBuffer);

        bufferContext.EncodedBuffer = readBuffer;

        if (readBuffer.ReadCursor == 0) DefaultMessageDeserializer.PublishedQuote.ResetFields();
        return ReadEntries(bufferContext, readerContext, DefaultMessageDeserializer);
    }

    public override AppendResult AppendEntry(IFixedByteArrayBuffer writeBuffer, IAppendContext<TEntry> entryContext, AppendResult appendResult)
    {
        var pqContext = entryContext as IPQQuoteAppendContext<TEntry, TSerializeType>;
        var entry     = entryContext.CurrentEntry;
        if (entry!.SourceTickerInfo!.SourceId != PricingInstrumentId.SourceId
         || entry.SourceTickerInfo.InstrumentId != PricingInstrumentId.InstrumentId || pqContext == null)
            return new AppendResult(StorageAttemptResult.EntryNotCompatible);

        bufferContext ??= new MessageBufferContext(writeBuffer);

        bufferContext.EncodedBuffer = writeBuffer;

        var messageSerializer = RepeatedEntryMessageSerializer;
        var lastEntryQuote    = pqContext.SerializeEntry;
        lastEntryQuote.HasUpdates = false;
        if (writeBuffer.WriteCursor == 0)
        {
            lastEntryQuote.CopyFrom(entry, CopyMergeFlags.FullReplace);
            messageSerializer = IndexEntryMessageSerializer;
        }
        else
        {
            lastEntryQuote.CopyFrom(entry);
        }
        return AppendEntry(bufferContext, lastEntryQuote, messageSerializer, appendResult);
    }

    public virtual IEnumerable<TEntry> ReadEntries
        (IMessageBufferContext buffer, IReaderContext<TEntry> readerContext, IPQQuoteDeserializer<TSerializeType> bufferDeserializer)
    {
        var entryCount = 0;
        while (readerContext.ContinueSearching && buffer.EncodedBuffer!.ReadCursor < buffer.EncodedBuffer.WriteCursor)
        {
            entryCount++;
            bufferDeserializer.PublishedQuote.HasUpdates = false;
            bufferDeserializer.Deserialize(buffer);
            var toReturn = readerContext.GetNextEntryToPopulate;
            toReturn.CopyFrom(bufferDeserializer.PublishedQuote, CopyMergeFlags.FullReplace);
            if (readerContext.IsReverseChronologicalOrder)
            {
                if (!readerContext.CheckExceededPeriodRangeTime(toReturn))
                {
                    readerContext.ReadReverseAddToStart(toReturn);
                }
                else
                {
                    toReturn.DecrementRefCount();
                    break;
                }
            }
            else if (readerContext.ProcessCandidateEntry(toReturn))
            {
                yield return toReturn;
            }
        }
        if (!readerContext.IsReverseChronologicalOrder) yield break;
        foreach (var checkEntry in readerContext.ReadReverse())
        {
            if (readerContext.ProcessCandidateEntry(checkEntry))
            {
                checkEntry.IncrementRefCount(); // clearing reverse results will decrement items
                yield return checkEntry;
            }
            if (!readerContext.ContinueSearching) break;
        }
        readerContext.ClearReadReverse();
    }

    public virtual AppendResult AppendEntry
    (IMessageBufferContext bufferContext,
        TSerializeType lastEntryLevel, PQQuoteSerializer useSerializer, AppendResult appendResult)
    {
        useSerializer.Serialize(lastEntryLevel, bufferContext);
        if (bufferContext.LastWriteLength <= 0) return new AppendResult(StorageAttemptResult.StorageSizeFailure);
        appendResult.SerializedSize = bufferContext.LastWriteLength;

        ExpandedDataSize      += (ulong)bufferContext.LastWriteLength;
        TotalDataEntriesCount += 1;
        return appendResult;
    }
}
