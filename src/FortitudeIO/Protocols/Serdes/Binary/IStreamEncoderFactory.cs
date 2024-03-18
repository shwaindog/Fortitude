#region

using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IStreamEncoderFactory : IEnumerable<KeyValuePair<uint, IMessageSerializer>>
{
    int RegisteredSerializerCount { get; }
    IMessageSerializer<T> MessageEncoder<T>(IBufferContext bufferContext, T message) where T : class, IVersionedMessage;
    IMessageSerializer<T> MessageEncoder<T>(T message) where T : class, IVersionedMessage;
    IMessageSerializer<T> MessageEncoder<T>(uint id) where T : class, IVersionedMessage;
    IMessageSerializer MessageEncoder(IBufferContext bufferContext, uint id);
    IMessageSerializer MessageEncoder(uint id);
    void RegisterMessageSerializer(uint id, IMessageSerializer messageSerializer);
    void UnregisterMessageSerializer(uint id);
}
