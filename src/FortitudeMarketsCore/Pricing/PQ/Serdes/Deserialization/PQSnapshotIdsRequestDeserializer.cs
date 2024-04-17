#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

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
            fixed (byte* fptr = messageBufferContext.EncodedBuffer!.Buffer!)
            {
                var ptr = fptr + messageBufferContext.EncodedBuffer.ReadCursor;
                var requestsCount = StreamByteOps.ToUShort(ref ptr);
                var streamIDs = new uint[requestsCount];
                for (var i = 0; i < streamIDs.Length; i++) deserializedSnapshotIdsRequest.RequestSourceTickerIds.Add(StreamByteOps.ToUInt(ref ptr));
            }

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSnapshotIdsRequest, messageBufferContext);
            return deserializedSnapshotIdsRequest;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => new PQSnapshotIdsRequestDeserializer(this);
}
