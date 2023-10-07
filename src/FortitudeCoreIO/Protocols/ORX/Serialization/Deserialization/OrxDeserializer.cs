#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public sealed class OrxDeserializer<Tm> : BinaryDeserializer<Tm> where Tm : class
{
    private readonly OrxByteDeserializer<Tm> orxByteDeserializer;
    private readonly IRecycler recyclingFactory;

    public OrxDeserializer(IRecycler recyclingFactory)
    {
        this.recyclingFactory = recyclingFactory;
        orxByteDeserializer = new OrxByteDeserializer<Tm>(new OrxDeserializerLookup(recyclingFactory));
    }

    public override object Deserialize(DispatchContext dispatchContext)
    {
        var tradingMessage = (Tm)orxByteDeserializer.Deserialize(dispatchContext);
        Dispatch(tradingMessage, dispatchContext.MessageHeader!,
            dispatchContext.Session, dispatchContext.DispatchLatencyLogger);
        recyclingFactory.Recycle(tradingMessage);
        return tradingMessage;
    }
}
