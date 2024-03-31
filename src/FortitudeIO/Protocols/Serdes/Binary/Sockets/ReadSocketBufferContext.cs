#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Sockets;

[TestClassNotRequired]
public class ReadBufferContext : MessageBufferContext
{
    public ReadBufferContext(IBuffer buffer) : base(buffer) { }
    public DateTime DeserializerTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public object? MessageHeader { get; set; }
}

[TestClassNotRequired]
public class ReadSocketBufferContext : ReadBufferContext
{
    public ReadSocketBufferContext() : base(new ReadWriteBuffer(Array.Empty<byte>())) { }
    public ReadSocketBufferContext(IBuffer buffer) : base(buffer) { }

    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public IConversation? Conversation { get; set; }
    public IPerfLogger? DispatchLatencyLogger { get; set; }
}
