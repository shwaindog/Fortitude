#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Sockets;

public interface ISocketBufferReadContext : IMessageBufferContext
{
    ISocketReceiver SocketReceiver { get; set; }
    DateTime DetectTimestamp { get; set; }
    DateTime ReceivingTimestamp { get; set; }
    IConversation? Conversation { get; set; }
    IPerfLogger? DispatchLatencyLogger { get; set; }
}

[TestClassNotRequired]
public class SocketBufferReadContext : MessageBufferContext, ISocketBufferReadContext
{
    public SocketBufferReadContext() : base(new CircularReadWriteBuffer(Array.Empty<byte>())) { }
    public SocketBufferReadContext(IBuffer buffer) : base(buffer) { }

    public ISocketReceiver SocketReceiver { get; set; } = null!;
    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public IConversation? Conversation { get; set; }
    public IPerfLogger? DispatchLatencyLogger { get; set; }
}
