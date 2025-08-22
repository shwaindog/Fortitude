using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerViews;

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

    public static T GetFLoggerAs<T>(string loggerName) where T : ILoggerView
    {
        return FLogger(loggerName).As<T>();
    }

    public static T FLoggerAs<T>(string loggerName) where T : ILoggerView
    {
        return FLogger(loggerName).As<T>();
    }

    public static IFLogger FLogger(Type type)
    {
        return FLogger(type.FullName!);
    }

    public static IFLogger GetFLogger(this Type type)
    {
        return FLogger(type.FullName!);
    }

    public static T FLogger<T>(Type type) where T : ILoggerView
    {
        return FLogger(type.FullName!).As<T>();
    }

    public static T GetFLoggerAs<T>(this Type type) where T : ILoggerView
    {
        return FLogger(type.FullName!).As<T>();
    }

    public static IFLogger FLoggerForType
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            StackTrace stackTrace = new StackTrace(1, false);
            
            var method = stackTrace.GetFrame(0)!.GetMethod();
            var type   = method?.DeclaringType ?? typeof(string);
            // Console.Out.WriteLine($"Frame[{0}] = type-{type.FullName}:method-{method?.Name}");
            if (type.FullName?.StartsWith("System.") ?? true)
            {
                StackTrace fullStackTrace = new StackTrace(0, false);
                
                for (int i = 0; i < fullStackTrace.FrameCount; i++)
                {
                    var frame = fullStackTrace.GetFrame(1);
                    Console.Out.WriteLine($"Frame[{i}] = {frame}");
                }
            }

            return FLogger(type!);
        }
    }
}
