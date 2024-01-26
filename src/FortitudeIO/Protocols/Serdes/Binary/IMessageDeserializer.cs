#region

using FortitudeCommon.Serdes;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer
{
    object? Deserialize(DispatchContext dispatchContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    new TM? Deserialize(DispatchContext dispatchContext);
}
