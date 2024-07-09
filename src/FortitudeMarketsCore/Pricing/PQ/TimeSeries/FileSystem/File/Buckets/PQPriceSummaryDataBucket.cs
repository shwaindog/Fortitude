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
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

public abstract class PQPriceSummaryDataBucket<TBucket, TEntry> : DataBucket<TEntry, TBucket>, IPricePeriodSummaryBucket<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPricePeriodSummaryBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>, IPricePeriodSummary
{
    private IMessageBufferContext? bufferContext;

    private PQPriceStoragePeriodSummarySerializer? indexEntrySerializer;

    private IPQPriceStoragePeriodSummaryDeserializer? messageDeserializer;

    private PQPriceStoragePeriodSummarySerializer? repeatedEntrySerializer;

    protected PQPriceSummaryDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public IPQPriceStoragePeriodSummaryDeserializer DefaultMessageDeserializer
    {
        get
        {
            if (messageDeserializer == null)
            {
                var srcTkrQtInfo = SourceTickerQuoteInfo;
                messageDeserializer = new PQPriceStoragePeriodSummaryDeserializer(new PQSourceTickerQuoteInfo(srcTkrQtInfo));
            }

            return messageDeserializer;
        }
    }

    public PQPriceStoragePeriodSummarySerializer IndexEntryMessageSerializer =>
        indexEntrySerializer ??= new PQPriceStoragePeriodSummarySerializer(StorageFlags.Complete);

    public PQPriceStoragePeriodSummarySerializer RepeatedEntryMessageSerializer =>
        repeatedEntrySerializer ??= new PQPriceStoragePeriodSummarySerializer(StorageFlags.Update);

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; set; } = null!;

    public override IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext)
    {
        bufferContext ??= new MessageBufferContext(readBuffer);

        bufferContext.EncodedBuffer = readBuffer;

        if (readBuffer.ReadCursor == 0) DefaultMessageDeserializer.DeserializedPriceSummary.HasUpdates = false;
        return ReadEntries(bufferContext, readerContext, DefaultMessageDeserializer);
    }

    public override AppendResult AppendEntry
        (IFixedByteArrayBuffer writeBuffer, IAppendContext<TEntry> entryContext, AppendResult appendResult)
    {
        var pqContext = entryContext as IPQPricePeriodSummaryAppendContext<TEntry>;
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
      , IPQPriceStoragePeriodSummaryDeserializer bufferDeserializer)
    {
        var entryCount = 0;
        while (readerContext.ContinueSearching && buffer.EncodedBuffer!.ReadCursor < buffer.EncodedBuffer.WriteCursor)
        {
            entryCount++;
            bufferDeserializer.DeserializedPriceSummary.HasUpdates = false;
            bufferDeserializer.Deserialize(buffer);
            var toReturn = readerContext.GetNextEntryToPopulate;
            toReturn.CopyFrom(bufferDeserializer.DeserializedPriceSummary, CopyMergeFlags.FullReplace);
            if (readerContext.ProcessCandidateEntry(toReturn)) yield return toReturn;
        }
    }

    public virtual AppendResult AppendEntry
    (IMessageBufferContext bufferContext,
        IPQPriceStoragePeriodSummary lastEntryLevel, PQPriceStoragePeriodSummarySerializer useSerializer, AppendResult appendResult)
    {
        useSerializer.Serialize(lastEntryLevel, bufferContext);
        if (bufferContext.LastWriteLength <= 0) return new AppendResult(StorageAttemptResult.StorageSizeFailure);
        appendResult.SerializedSize = bufferContext.LastWriteLength;

        ExpandedDataSize      += (ulong)bufferContext.LastWriteLength;
        TotalDataEntriesCount += 1;
        return appendResult;
    }
}
