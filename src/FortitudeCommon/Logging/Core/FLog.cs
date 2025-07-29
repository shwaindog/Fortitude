using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core;

public static class FLog
{
    public static IFLogger FLogger(string loggerName)
    {
        var ctx    = FLogContext.Context;
        var logger = ctx.LoggerRegistry.Root.GetOrCreateLogger(loggerName, ctx);
        return logger;
    }

    public static IFLogger GetFLogger(this string loggerName)
    {
        return FLogger(loggerName);
    }

    public static IFLogger FLogger(Type type)
    {
        return FLogger(type.FullName!);
    }

    public static IFLogger GetFLogger(this Type type)
    {
        return FLogger(type.FullName!);
    }

    public static IFLogger FLoggerForType
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            StackTrace stackTrace = new StackTrace(1, false);

            var type = stackTrace.GetFrame(1)!.GetMethod()!.DeclaringType;
            return FLogger(type!);
        }
    }
}
