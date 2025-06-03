// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PQCandleDataBucket<TBucket, TEntry> : DataBucket<TEntry, TBucket>, ICandleBucket<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, ICandleBucket<TEntry>
    where TEntry : ITimeSeriesEntry, ICandle
{
    private IMessageBufferContext? bufferContext;

    private PQStorageCandleSerializer? indexEntrySerializer;

    private IPQStorageCandleDeserializer? messageDeserializer;

    private PQStorageCandleSerializer? repeatedEntrySerializer;

    protected PQCandleDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public IPQStorageCandleDeserializer DefaultMessageDeserializer
    {
        get
        {
            if (messageDeserializer == null)
            {
                var srcTkrInfo = PricingInstrumentId as ISourceTickerInfo ?? new SourceTickerInfo(PricingInstrumentId);
                messageDeserializer = new PQStorageCandleDeserializer(new PQSourceTickerInfo(srcTkrInfo));
            }

            return messageDeserializer;
        }
    }

    public PQStorageCandleSerializer IndexEntryMessageSerializer =>
        indexEntrySerializer ??= new PQStorageCandleSerializer(PQMessageFlags.Complete);

    public PQStorageCandleSerializer RepeatedEntryMessageSerializer =>
        repeatedEntrySerializer ??= new PQStorageCandleSerializer(PQMessageFlags.Update);

    public IPricingInstrumentId PricingInstrumentId { get; set; } = null!;

    public override IEnumerable<TEntry> ReadEntries(IMessageQueueBuffer readBuffer, IReaderContext<TEntry> readerContext)
    {
        bufferContext ??= new MessageBufferContext(readBuffer);

        bufferContext.EncodedBuffer = readBuffer;

        if (readBuffer.ReadCursor == 0) DefaultMessageDeserializer.DeserializedCandle.HasUpdates = false;
        return ReadEntries(bufferContext, readerContext, DefaultMessageDeserializer);
    }

    public override AppendResult AppendEntry
        (IFixedByteArrayBuffer writeBuffer, IAppendContext<TEntry> entryContext, AppendResult appendResult)
    {
        var pqContext = entryContext as IPQCandleAppendContext<TEntry>;
        var entry     = entryContext.CurrentEntry!;

        bufferContext ??= new MessageBufferContext(writeBuffer);

        bufferContext.EncodedBuffer = writeBuffer;

        var messageSerializer = RepeatedEntryMessageSerializer;
        var lastEntryQuote    = pqContext!.SerializeEntry;
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
    (IMessageBufferContext buffer, IReaderContext<TEntry> readerContext
      , IPQStorageCandleDeserializer bufferDeserializer)
    {
        var entryCount = 0;
        while (readerContext.ContinueSearching && buffer.EncodedBuffer!.ReadCursor < buffer.EncodedBuffer.WriteCursor)
        {
            entryCount++;
            bufferDeserializer.DeserializedCandle.HasUpdates = false;
            bufferDeserializer.Deserialize(buffer);
            var toReturn = readerContext.GetNextEntryToPopulate;
            toReturn.CopyFrom(bufferDeserializer.DeserializedCandle, CopyMergeFlags.FullReplace);

            if (readerContext.IsReverseChronologicalOrder)
            {
                if (!readerContext.CheckExceededPeriodRangeTime(toReturn))
                    readerContext.ReadReverseAddToStart(toReturn);
                else
                    break;
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
        IPQStorageCandle lastEntryLevel, PQStorageCandleSerializer useSerializer, AppendResult appendResult)
    {
        useSerializer.Serialize(lastEntryLevel, bufferContext);
        if (bufferContext.LastWriteLength <= 0) return new AppendResult(StorageAttemptResult.StorageSizeFailure);
        appendResult.SerializedSize = bufferContext.LastWriteLength;

        ExpandedDataSize      += (ulong)bufferContext.LastWriteLength;
        TotalDataEntriesCount += 1;
        return appendResult;
    }
}
