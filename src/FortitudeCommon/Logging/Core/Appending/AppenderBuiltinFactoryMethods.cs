using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Appending.Formatting.ConsoleOut;
using FortitudeCommon.Logging.Core.Appending.Formatting.FileSystem;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using static FortitudeCommon.Logging.Config.Appending.FLoggerBuiltinAppenderType;

namespace FortitudeCommon.Logging.Core.Appending;

public static class AppenderBuiltinFactoryMethods
{
    public static IMutableFLogAppender? GetBuiltAppenderReferenceConfig(this IMutableAppenderDefinitionConfig appenderConfig, IFLogContext context)
    {
        var appenderTypeConfig = appenderConfig.AppenderType;
        switch (appenderTypeConfig)
        {
            case nameof(NullEntry):               return new NullAppender((NullAppenderConfig)appenderConfig);
            case nameof(BufferedForwarding): return new FLogBufferingAppender((IBufferingAppenderConfig)appenderConfig, context);
            case nameof(ConsoleOut):         return new FLogConsoleAppender((IConsoleAppenderConfig)appenderConfig, context);
            case nameof(NullConsole):         return new NullConsoleAppender((IConsoleAppenderConfig)appenderConfig, context);
            case nameof(FileUnbounded):      return new FLogFileAppender((IFileAppenderConfig)appenderConfig, context);
            case nameof(NullFile):      return new NullFileAppender((IFileAppenderConfig)appenderConfig, context);
            case nameof(ForwardingAppender): return new FLogForwardingAppender((IForwardingAppenderConfig)appenderConfig, context);

            default:
                string[] assemblyAndTypeFullName;
                if (appenderTypeConfig.IsNotNullOrEmpty() && (assemblyAndTypeFullName = appenderTypeConfig.Split(',', 2)).Length == 2)
                {
                    return (IMutableFLogAppender?)Activator.CreateInstanceFrom
                        (assemblyAndTypeFullName[0], assemblyAndTypeFullName[1], true, BindingFlags.CreateInstance
                       , null, [appenderConfig, context], CultureInfo.InvariantCulture, [])!.Unwrap();
                }
                return null;
        }
    }
}
