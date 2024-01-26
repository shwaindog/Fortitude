#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

[TestClassNotRequired]
public class DispatchContext : IBufferContext
{
    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime DeserializerTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public byte MessageVersion { get; set; }
    public int MessageSize { get; set; }
    public object? MessageHeader { get; set; }
    public ISocketSessionConnection? Session { get; set; }
    public ISocketConversation? Conversation { get; set; }
    public IPerfLogger? DispatchLatencyLogger { get; set; }
    public IBuffer? EncodedBuffer { get; set; }
    public MarshalType MarshalType => MarshalType.Binary;
    public int LastReadLength { get; set; } = -1;
    public int LastWriteLength { get; set; } = -1;
    public ContextDirection Direction { get; set; } = ContextDirection.Unknown;
}
