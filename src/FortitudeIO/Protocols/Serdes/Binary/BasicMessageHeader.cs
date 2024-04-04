#region

using FortitudeCommon.Serdes;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public readonly struct BasicMessageHeader
{
    public BasicMessageHeader(byte version, uint messageId, uint messageSize
        , ISerdeContext? deserializationContext = null)
    {
        Version = version;
        MessageId = messageId;
        MessageSize = messageSize;
        DeserializationContext = deserializationContext;
    }

    public byte Version { get; }
    public uint MessageId { get; }
    public uint MessageSize { get; }
    public ISerdeContext? DeserializationContext { get; }
}
