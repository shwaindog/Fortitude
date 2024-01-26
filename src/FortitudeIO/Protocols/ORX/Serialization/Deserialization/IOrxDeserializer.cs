#region

using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public interface IOrxDeserializer : IMessageDeserializer
{
    unsafe object Deserialize(byte* ptr, int length, byte messageVersion);
}

public interface IOrxDeserializer<TM> : IMessageDeserializer<TM> where TM : class, IVersionedMessage, new()
{
    unsafe object Deserialize(byte* ptr, int length, byte messageVersion);
}
