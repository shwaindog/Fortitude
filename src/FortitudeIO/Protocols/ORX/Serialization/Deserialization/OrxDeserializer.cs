#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public sealed class OrxDeserializer<Tm> : MessageDeserializer<Tm> where Tm : class, IVersionedMessage, new()
{
    private readonly OrxByteDeserializer<Tm> orxByteDeserializer;
    private readonly IRecycler recyclingFactory;

    public OrxDeserializer(IRecycler recyclingFactory)
    {
        this.recyclingFactory = recyclingFactory;
        orxByteDeserializer = new OrxByteDeserializer<Tm>(new OrxDeserializerLookup(recyclingFactory));
    }

    public override Tm Deserialize(ISerdeContext readContext)
    {
        if (readContext is DispatchContext dispatchContext)
        {
            var tradingMessage = (Tm)orxByteDeserializer.Deserialize(dispatchContext);
            Dispatch(tradingMessage, dispatchContext.MessageHeader!,
                dispatchContext.Session, dispatchContext.DispatchLatencyLogger);
            return tradingMessage;
        }
        else
        {
            throw new ArgumentException("Expected readContext to be of type DispatchContext");
        }
    }
}
