// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public class PQSourceTickerInfoRequestDeserializer : MessageDeserializer<PQSourceTickerInfoRequest>
{
    private readonly IRecycler recycler;

    public PQSourceTickerInfoRequestDeserializer(IRecycler recycler) => this.recycler = recycler;

    public PQSourceTickerInfoRequestDeserializer(PQSourceTickerInfoRequestDeserializer toClone) : base(toClone) => recycler = toClone.recycler;


    public override unsafe PQSourceTickerInfoRequest? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var       deserializedSnapshotIdsRequest                  = recycler.Borrow<PQSourceTickerInfoRequest>();
            using var fixedBuffer                                     = messageBufferContext.EncodedBuffer!;
            var       ptr                                             = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref ptr);
            deserializedSnapshotIdsRequest.RequestId = StreamByteOps.ToInt(ref ptr);

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSnapshotIdsRequest, messageBufferContext);
            return deserializedSnapshotIdsRequest;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => new PQSourceTickerInfoRequestDeserializer(this);
}
