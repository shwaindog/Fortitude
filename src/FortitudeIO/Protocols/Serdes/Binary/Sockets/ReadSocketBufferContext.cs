#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Sockets;

public interface ISocketBufferReadContext : IMessageBufferContext
{
    DateTime DetectTimestamp { get; set; }
    DateTime ReceivingTimestamp { get; set; }
    IConversation? Conversation { get; set; }
    IPerfLogger? DispatchLatencyLogger { get; set; }
}

[TestClassNotRequired]
public class SocketBufferReadContext : MessageBufferContext, ISocketBufferReadContext
{
    public SocketBufferReadContext() : base(new ReadWriteBuffer(Array.Empty<byte>())) { }
    public SocketBufferReadContext(IBuffer buffer) : base(buffer) { }

    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public IConversation? Conversation { get; set; }
    public IPerfLogger? DispatchLatencyLogger { get; set; }
}
