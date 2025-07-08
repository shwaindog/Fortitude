namespace FortitudeCommon.Logging.Config
{
    public enum FLogLevel
    {
        None = 0,
        Trace,
        Debug,
        Info,
        Warn,
        Error
    }

    public static class FLogLevelExtensions
    {
        public static bool IsTraceEnabled(this FLogLevel level) => level >= FLogLevel.Trace;
        public static bool IsDebugEnabled(this FLogLevel level) => level >= FLogLevel.Debug;
        public static bool IsInfoEnabled(this FLogLevel level)  => level >= FLogLevel.Info;
        public static bool IsWarnEnabled(this FLogLevel level)  => level >= FLogLevel.Warn;
        public static bool IsErrorEnabled(this FLogLevel level) => level >= FLogLevel.Error;
    }
}