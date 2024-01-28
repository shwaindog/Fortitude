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

// TODO update Deserialize calls to use IBufferContext

// TODO update to confirm buffer is Binary and Read

// TODO make DispatchContext extra fields optional during Deserialization.

// TODO Look into make OrxDerserializer not expose buffer and offset

// TODO look to move Fortitude.IO Protocol Serialization to Serde if applicable.
