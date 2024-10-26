// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public class PQSourceTickerInfoResponseDeserializer : MessageDeserializer<PQSourceTickerInfoResponse>
{
    private const int EstimatedSourceTickerSerializationSize = 120;

    private readonly IRecycler recycler;

    public PQSourceTickerInfoResponseDeserializer(IRecycler recycler) => this.recycler = recycler;

    public PQSourceTickerInfoResponseDeserializer(PQSourceTickerInfoResponseDeserializer toClone) : base(toClone) => recycler = toClone.recycler;

    public override unsafe PQSourceTickerInfoResponse? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedSourceTickerInfoResponse = recycler.Borrow<PQSourceTickerInfoResponse>();

            using var fixedBuffer = messageBufferContext.EncodedBuffer!;

            var end = fixedBuffer.ReadBuffer + fixedBuffer.RemainingStorage;
            var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;

            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref ptr);
            deserializedSourceTickerInfoResponse.RequestId  = StreamByteOps.ToInt(ref ptr);
            deserializedSourceTickerInfoResponse.ResponseId = StreamByteOps.ToInt(ref ptr);
            var requestsCount = StreamByteOps.ToUShort(ref ptr);
            for (var i = 0; i < requestsCount; i++)
            {
                if (ptr + EstimatedSourceTickerSerializationSize > end)
                {
                    messageBufferContext.LastReadLength = -1;
                    return null;
                }

                deserializedSourceTickerInfoResponse.SourceTickerInfos.Add(DeserializeSourceTickerInfo(ref ptr, fixedBuffer));
            }

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSourceTickerInfoResponse, messageBufferContext);
            return deserializedSourceTickerInfoResponse;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    private unsafe ISourceTickerInfo DeserializeSourceTickerInfo(ref byte* currPtr, IBuffer fixedBuffer)
    {
        var srcTkrInfo = recycler.Borrow<SourceTickerInfo>();
        SourceTickerInfoDeserializer.DeserializeSourceTickerInfo(srcTkrInfo, ref currPtr, fixedBuffer);

        return srcTkrInfo;
    }

    public override IMessageDeserializer Clone() => new PQSourceTickerInfoResponseDeserializer(this);
}
