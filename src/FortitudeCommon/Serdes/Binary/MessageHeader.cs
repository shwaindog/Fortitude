namespace FortitudeCommon.Serdes.Binary;

public struct MessageHeader
{
    public MessageHeader(byte version, byte flags, uint messageId, uint messageSize
        , ISerdeContext? deserializationContext = null)
    {
        Version = version;
        Flags = flags;
        MessageId = messageId;
        MessageSize = messageSize;
        DeserializationContext = deserializationContext;
    }

    public MessageHeader(MessageHeader toClone)
    {
        Version = toClone.Version;
        Flags = toClone.Flags;
        MessageId = toClone.MessageId;
        MessageSize = toClone.MessageSize;
        DeserializationContext = toClone.DeserializationContext;
    }

    public byte Version { get; set; }
    public byte Flags { get; set; }
    public uint MessageId { get; set; }
    public uint MessageSize { get; set; }
    public ISerdeContext? DeserializationContext { get; set; }

    public const int SerializationSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);
}

public static class MessageHeaderExtensions
{
    public static MessageHeader SetVersion(this MessageHeader original, byte version)
    {
        var updateMessageHeader = new MessageHeader(original)
        {
            Version = version
        };
        return updateMessageHeader;
    }

    public static MessageHeader SetFlags(this MessageHeader original, byte flags)
    {
        var updateMessageHeader = new MessageHeader(original)
        {
            Flags = flags
        };
        return updateMessageHeader;
    }

    public static MessageHeader SetMessageId(this MessageHeader original, uint messageId)
    {
        var updateMessageHeader = new MessageHeader(original)
        {
            MessageId = messageId
        };
        return updateMessageHeader;
    }

    public static MessageHeader SetMessageSize(this MessageHeader original, uint messageSize)
    {
        var updateMessageHeader = new MessageHeader(original)
        {
            MessageSize = messageSize
        };
        return updateMessageHeader;
    }

    public static MessageHeader SetMessageSize(this MessageHeader original, ISerdeContext serdeContext)
    {
        var updateMessageHeader = new MessageHeader(original)
        {
            DeserializationContext = serdeContext
        };
        return updateMessageHeader;
    }
}
