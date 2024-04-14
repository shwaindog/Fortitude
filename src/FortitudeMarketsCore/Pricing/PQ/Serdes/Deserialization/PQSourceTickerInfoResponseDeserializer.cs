#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public class PQSourceTickerInfoResponseDeserializer : MessageDeserializer<PQSourceTickerInfoResponse>
{
    private const int EstimatedSourceTickerSerializationSize = 120;
    private readonly IRecycler recycler;

    public PQSourceTickerInfoResponseDeserializer(IRecycler recycler) => this.recycler = recycler;


    public override unsafe PQSourceTickerInfoResponse? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedSourceTickerInfoResponse = recycler.Borrow<PQSourceTickerInfoResponse>();
            fixed (byte* fptr = messageBufferContext.EncodedBuffer!.Buffer!)
            {
                var end = fptr + messageBufferContext.EncodedBuffer.RemainingStorage;
                var ptr = fptr + messageBufferContext.EncodedBuffer.ReadCursor;
                var requestsCount = StreamByteOps.ToUShort(ref ptr);
                for (var i = 0; i < requestsCount; i++)
                {
                    if (ptr + EstimatedSourceTickerSerializationSize > end)
                    {
                        messageBufferContext.LastReadLength = -1;
                        return null;
                    }

                    deserializedSourceTickerInfoResponse.SourceTickerQuoteInfos.Add(DeserializeSourceTickerQuoteInfo(ref ptr));
                }
            }

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSourceTickerInfoResponse, messageBufferContext);
            return deserializedSourceTickerInfoResponse;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    private unsafe ISourceTickerQuoteInfo DeserializeSourceTickerQuoteInfo(ref byte* currPtr)
    {
        var deserializedSourceTickerQuoteInfo = recycler.Borrow<SourceTickerQuoteInfo>();
        deserializedSourceTickerQuoteInfo.SourceId = StreamByteOps.ToUShort(ref currPtr);
        deserializedSourceTickerQuoteInfo.TickerId = StreamByteOps.ToUShort(ref currPtr);
        deserializedSourceTickerQuoteInfo.RoundingPrecision = StreamByteOps.ToDecimal(ref currPtr);
        deserializedSourceTickerQuoteInfo.MinSubmitSize = StreamByteOps.ToDecimal(ref currPtr);
        deserializedSourceTickerQuoteInfo.MaxSubmitSize = StreamByteOps.ToDecimal(ref currPtr);
        deserializedSourceTickerQuoteInfo.IncrementSize = StreamByteOps.ToDecimal(ref currPtr);
        deserializedSourceTickerQuoteInfo.MinimumQuoteLife = StreamByteOps.ToUShort(ref currPtr);
        deserializedSourceTickerQuoteInfo.LayerFlags = (LayerFlags)StreamByteOps.ToUInt(ref currPtr);
        deserializedSourceTickerQuoteInfo.MaximumPublishedLayers = *currPtr++;
        deserializedSourceTickerQuoteInfo.LastTradedFlags = (LastTradedFlags)StreamByteOps.ToUShort(ref currPtr);
        deserializedSourceTickerQuoteInfo.Source = StreamByteOps.ToStringWithSizeHeader(ref currPtr);
        deserializedSourceTickerQuoteInfo.Ticker = StreamByteOps.ToStringWithSizeHeader(ref currPtr);

        return deserializedSourceTickerQuoteInfo;
    }
}
