#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.State;
using LegacyApi = FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Sockets;

[TestClassNotRequired]
public class ReadBufferContext : IBufferContext
{
    public DateTime DeserializerTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public byte MessageVersion { get; set; }
    public int MessageSize { get; set; }
    public object? MessageHeader { get; set; }
    public IBuffer? EncodedBuffer { get; set; }
    public MarshalType MarshalType => MarshalType.Binary;
    public int LastReadLength { get; set; } = -1;
    public int LastWriteLength { get; set; } = -1;
    public ContextDirection Direction { get; set; } = ContextDirection.Read;
}

[TestClassNotRequired]
public class ReadSocketBufferContext : ReadBufferContext
{
    public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
    public LegacyApi.ISocketSessionConnection? LegacySession { get; set; }
    public ISession? Session { get; set; }
    public ISocketSessionContext? SessionContext { get; set; }
    public IConversation? Conversation => SessionContext!.OwningConversation;
    public IPerfLogger? DispatchLatencyLogger { get; set; }
}
