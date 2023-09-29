using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;
namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization
{
    public sealed class OrxDeserializer<Tm> : BinaryDeserializer<Tm> where Tm : class
    {
        private readonly IRecycler recyclingFactory;
        private readonly OrxByteDeserializer<Tm> orxByteDeserializer;

        public OrxDeserializer(IRecycler recyclingFactory)
        {
            this.recyclingFactory = recyclingFactory;
            orxByteDeserializer = new OrxByteDeserializer<Tm>(new OrxDeserializerLookup(recyclingFactory));
        }

        public override object Deserialize(DispatchContext dispatchContext)
        {
            var tradingMessage = orxByteDeserializer.Deserialize(dispatchContext) as Tm;
            Dispatch(tradingMessage, dispatchContext.MessageHeader, 
                dispatchContext.Session, dispatchContext.DispatchLatencyLogger);
            recyclingFactory.Recycle(tradingMessage);
            return tradingMessage;
        }
    }
}
