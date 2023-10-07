#region

using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Types;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Protocols.Serialization
{
    [TestClassNotRequired]
    public class DispatchContext
    {
        public DateTime DetectTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
        public DateTime ReceivingTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
        public DateTime DeserializerTimestamp { get; set; } = DateTimeConstants.UnixEpoch;
        public ReadWriteBuffer EncodedBuffer { get; set; }
        public byte MessageVersion { get; set; }
        public int MessageSize { get; set; }

        public object MessageHeader { get; set; }

        //[Obsolete] TODO restore when replacement is known
        public ISocketSessionConnection Session { get; set; }
        public ISocketConversation Conversation { get; set; }
        public IPerfLogger DispatchLatencyLogger { get; set; }
    }
}
