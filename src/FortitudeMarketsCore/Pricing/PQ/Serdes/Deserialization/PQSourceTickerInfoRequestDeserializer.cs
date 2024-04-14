#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public class PQSourceTickerInfoRequestDeserializer : MessageDeserializer<PQSourceTickerInfoRequest>
{
    private readonly IRecycler recycler;

    public PQSourceTickerInfoRequestDeserializer(IRecycler recycler) => this.recycler = recycler;


    public override PQSourceTickerInfoRequest? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedSnapshotIdsRequest = recycler.Borrow<PQSourceTickerInfoRequest>();
            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSnapshotIdsRequest, messageBufferContext);
            return deserializedSnapshotIdsRequest;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }
}
