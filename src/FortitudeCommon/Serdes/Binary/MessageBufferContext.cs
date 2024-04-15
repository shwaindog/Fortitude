#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeCommon.Serdes.Binary;

public interface IMessageBufferContext : IBufferContext
{
    MessageHeader MessageHeader { get; set; }
    DateTime DeserializerTime { get; set; }
}

public class MessageBufferContext : BufferContext, IMessageBufferContext
{
    public MessageBufferContext(IBuffer buffer) : base(buffer) { }
    public MessageHeader MessageHeader { get; set; }
    public DateTime DeserializerTime { get; set; } = DateTimeConstants.UnixEpoch;
}
