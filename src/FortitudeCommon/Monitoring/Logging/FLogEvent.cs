using System;

namespace FortitudeCommon.Monitoring.Logging
{
    public sealed class FLogEvent
    {
        public Exception Exception;
        public FLogLevel Level;
        public IFLogger Logger;
        public DateTime LogTime;
        public string MsgFormat;
        public object MsgObject;
        public object[] MsgParams;
    }
}