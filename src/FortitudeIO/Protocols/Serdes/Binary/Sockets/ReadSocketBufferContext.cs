#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Sockets;

public interface IBufferReadContext : IMessageBufferContext
{
    DateTime DeserializerTimestamp { get; set; }
    object? MessageHeader { get; set; }
}

[TestClassNotRequired]
public class BufferReadContext : MessageBufferContext, IBufferReadContext
{
    public BufferReadContext(IBuffer buffer) : base(buffer) { }
    public DateTime DeserializerTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public object? MessageHeader { get; set; }
}

public interface ISocketBufferReadContext : IBufferReadContext
{
    DateTime DetectTimestamp { get; set; }
    DateTime ReceivingTimestamp { get; set; }
    IConversation? Conversation { get; set; }
    IPerfLogger? DispatchLatencyLogger { get; set; }
}

[TestClassNotRequired]
public class SocketBufferReadContext : BufferReadContext, ISocketBufferReadContext
{
    public SocketBufferReadContext() : base(new ReadWriteBuffer(Array.Empty<byte>())) { }
    public SocketBufferReadContext(IBuffer buffer) : base(buffer) { }

    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public IConversation? Conversation { get; set; }
    public IPerfLogger? DispatchLatencyLogger { get; set; }
}
