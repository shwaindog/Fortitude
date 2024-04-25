#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public sealed class OrxDeserializer<Tm> : MessageDeserializer<Tm> where Tm : class, IVersionedMessage, new()
{
    private readonly OrxByteDeserializer<Tm> orxByteDeserializer;

    public OrxDeserializer(IRecycler recyclingFactory, uint msgId) =>
        orxByteDeserializer = new OrxByteDeserializer<Tm>(new OrxDeserializerLookup(recyclingFactory));

    public OrxDeserializer(OrxDeserializer<Tm> toClone) : base(toClone) => orxByteDeserializer = toClone.orxByteDeserializer;

    public override Tm Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var orxDeserializedMessage = orxByteDeserializer.Deserialize(readContext);
            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(orxDeserializedMessage, messageBufferContext);
            return orxDeserializedMessage;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    public override IMessageDeserializer Clone() => new OrxDeserializer<Tm>(this);
}
