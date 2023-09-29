using System;

namespace FortitudeCommon.Monitoring.Logging
{
    public interface IFLoggerFactory
    {
        IFLogger GetLogger(string loggerName);
        IFLogger GetLogger(Type ownerType);
    }
}