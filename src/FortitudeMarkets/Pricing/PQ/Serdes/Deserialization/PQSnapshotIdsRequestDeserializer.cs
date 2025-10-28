// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public class PQSnapshotIdsRequestDeserializer : MessageDeserializer<PQSnapshotIdsRequest>
{
    private readonly IRecycler recycler;

    public PQSnapshotIdsRequestDeserializer(IRecycler recycler) => this.recycler = recycler;

    public PQSnapshotIdsRequestDeserializer(PQSnapshotIdsRequestDeserializer toClone) : base(toClone) => recycler = toClone.recycler;

    public override unsafe PQSnapshotIdsRequest? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedSnapshotIdsRequest = recycler.Borrow<PQSnapshotIdsRequest>();

            using var fixedBuffer = messageBufferContext.EncodedBuffer!;

            var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;

            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref ptr);

            var requestsCount = StreamByteOps.ToUShort(ref ptr);
            var streamIDs     = new uint[requestsCount];

            for (var i = 0; i < streamIDs.Length; i++) deserializedSnapshotIdsRequest.RequestSourceTickerIds.Add(StreamByteOps.ToUInt(ref ptr));

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSnapshotIdsRequest, messageBufferContext);
            return deserializedSnapshotIdsRequest;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => new PQSnapshotIdsRequestDeserializer(this);
}
