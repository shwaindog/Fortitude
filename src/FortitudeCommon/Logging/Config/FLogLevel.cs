using System.Text;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Config
{
    public enum FLogLevel
    {
        None = 0
      , Trace
      , Debug
      , Info
      , Warn
      , Error
    }

    public enum FLogLevelMatch
    {
        None = 0
      , Any
      , ConfigDefined
      , Trace
      , Debug
      , Info
      , Warn
      , Error
    }

    public static class FLogLevelExtensions
    {
        public static StructStyler<FLogLevel> FLogLevelFormatter = FormatFlogLevelAppender;

        public static StructStyler<FLogLevel> Styler(this FLogLevel flogLevel) => FLogLevelFormatter;

        public static int LoggableRange => (int)(FLogLevel.Error - FLogLevel.None);

        public static void FormatFlogLevelAppender(this FLogLevel flogLevel, IStyledTypeStringAppender sbc)
        {
            var sb = sbc.WriteBuffer;

            switch (flogLevel)
            {
                case FLogLevel.Trace: sb.Append($"{nameof(FLogLevel.Trace)}"); break;
                case FLogLevel.Debug: sb.Append($"{nameof(FLogLevel.Debug)}"); break;
                case FLogLevel.Info:  sb.Append($"{nameof(FLogLevel.Info)}"); break;
                case FLogLevel.Warn:  sb.Append($"{nameof(FLogLevel.Warn)}"); break;
                case FLogLevel.Error: sb.Append($"{nameof(FLogLevel.Error)}"); break;

                default: sb.Append($"{nameof(FLogLevel.None)}"); break;
            }
        }

        public static bool IsTraceEnabled(this FLogLevel level) => level >= FLogLevel.Trace;
        public static bool IsDebugEnabled(this FLogLevel level) => level >= FLogLevel.Debug;
        public static bool IsInfoEnabled(this FLogLevel level)  => level >= FLogLevel.Info;
        public static bool IsWarnEnabled(this FLogLevel level)  => level >= FLogLevel.Warn;
        public static bool IsErrorEnabled(this FLogLevel level) => level >= FLogLevel.Error;
    }
}
