// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLogContext
{
    bool HasStarted { get; }

    bool IsInitialized { get; }

    IFLogAppenderRegistry  AppenderRegistry     { get; }
    IFLogLoggerRegistry    LoggerRegistry       { get; }
    IFLoggerAsyncRegistry  AsyncRegistry        { get; }
    IFLogConfigRegistry    ConfigRegistry       { get; }
    IFLogEntryPoolRegistry LogEntryPoolRegistry { get; }
}

public class FLogContext : IFLogContext
{
    private static IFLogContext? singletonInstance;

    private static readonly object SyncLock = new();

    private IFLogAppenderRegistry  appenderRegistry     = null!;
    private IFLogLoggerRegistry    loggerRegistry       = null!;
    private IFLoggerAsyncRegistry  asyncRegistry        = null!;
    private IFLogConfigRegistry    configRegistry       = null!;
    private IFLogEntryPoolRegistry logEntryPoolRegistry = null!;

    private FLogContext() { }

    public static IFLogContext UninitializedInstance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (SyncLock)
                {
                    singletonInstance ??= new FLogContext();
                }
            }
            // ReSharper disable once InconsistentlySynchronizedField
            return singletonInstance;
        }
        set
        {
            lock (SyncLock)
            {
                if (singletonInstance != null && (singletonInstance.IsInitialized || singletonInstance.HasStarted))
                {
                    Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName} once it has been initialized or started");
                    throw new InvalidOperationException($"Can not replace {typeof(FLogContext).FullName} once it has been initialized or started");
                }
                singletonInstance = value;
            }
        }
    }

    public static IFLogContext UnstartedInstance
    {
        get
        {
            var instance = UninitializedInstance;
            if (!instance.IsInitialized)
            {
                return instance.DefaultInitializeContext();
            }
            return instance;
        }
    }

    public static IFLogContext Context
    {
        get
        {
            var instance = UninitializedInstance;
            if (!instance.HasStarted)
            {
                return FLogBootstrap.Instance.StartFlog((FLogContext)instance);
            }
            return instance;
        }
    }

    public bool HasStarted { get; internal set; }

    public bool IsInitialized { get; internal set; }

    public IFLogAppenderRegistry AppenderRegistry
    {
        get => appenderRegistry;
        set
        {
            if (appenderRegistry != null && HasStarted)
            {
                Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName}.{nameof(AppenderRegistry)} once it has been initialized and started");
                throw new
                    InvalidOperationException($"Can not replace {typeof(FLogContext).FullName}.{nameof(AppenderRegistry)} once it has been initialized or started");
            }
            appenderRegistry = value;
        }
    }

    public IFLogLoggerRegistry LoggerRegistry
    {
        get => loggerRegistry;
        set
        {
            if (loggerRegistry != null && (HasStarted || loggerRegistry.Root.ImmediateEmbodiedChildren.Count > 0))
            {
                Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName}.{nameof(LoggerRegistry)} once it has been started or has created loggers");
                throw new
                    InvalidOperationException($"Can not replace {typeof(FLogContext).FullName}.{nameof(LoggerRegistry)} once it has been started or created loggers");
            }
            loggerRegistry = value;
        }
    }

    public IFLoggerAsyncRegistry AsyncRegistry
    {
        get => asyncRegistry;
        set
        {
            if (asyncRegistry != null && HasStarted)
            {
                Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName}.{nameof(AsyncRegistry)} once it has been initialized and started");
                throw new
                    InvalidOperationException($"Can not replace {typeof(FLogContext).FullName}.{nameof(AsyncRegistry)} once it has been initialized or started");
            }
            asyncRegistry = value;
        }
    }

    public IFLogConfigRegistry ConfigRegistry
    {
        get => configRegistry;
        set
        {
            if (configRegistry != null && HasStarted)
            {
                Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName}.{nameof(ConfigRegistry)} once it has been initialized and started");
                throw new
                    InvalidOperationException($"Can not replace {typeof(FLogContext).FullName}.{nameof(ConfigRegistry)} once it has been initialized or started");
            }
            configRegistry = value;
        }
    }

    public IFLogEntryPoolRegistry LogEntryPoolRegistry
    {
        get => logEntryPoolRegistry;
        set
        {
            if (logEntryPoolRegistry != null && HasStarted)
            {
                Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName}.{nameof(LogEntryPoolRegistry)} once it has been initialized and started");
                throw new
                    InvalidOperationException($"Can not replace {typeof(FLogContext).FullName}.{nameof(LogEntryPoolRegistry)} once it has been initialized or started");
            }
            logEntryPoolRegistry = value;
        }
    }

    internal static class TestFLogContextInternalsAccessor
    {
        public static IFLogContext? TestContext
        {
            // ReSharper disable once InconsistentlySynchronizedField
            get => singletonInstance!;
            set =>
                // ReSharper disable once InconsistentlySynchronizedField
                singletonInstance = value;
        }

        public static void SetAppenderRegistry(FLogContext context, IFLogAppenderRegistry appenderRegistry)
        {
            context.appenderRegistry = appenderRegistry;
        }

        public static void SetLoggerRegistry(FLogContext context, IFLogLoggerRegistry loggerRegistry)
        {
            context.loggerRegistry = loggerRegistry;
        }

        public static void SetAsyncRegistry(FLogContext context, IFLoggerAsyncRegistry asyncRegistry)
        {
            context.asyncRegistry = asyncRegistry;
        }

        public static void SetConfigRegistry(FLogContext context, IFLogConfigRegistry configRegistry)
        {
            context.configRegistry = configRegistry;
        }

        public static void SetLogEntryPoolRegistry(FLogContext context, IFLogEntryPoolRegistry logEntryPoolRegistry)
        {
            context.logEntryPoolRegistry = logEntryPoolRegistry;
        }
    }
}

internal static class TestFLogContextExtensions
{
    public static void SetAsContext(this IFLogContext context)
    {
        FLogContext.TestFLogContextInternalsAccessor.TestContext = context;
    }

    public static IFLogContext? GetContextField()
    {
        return FLogContext.TestFLogContextInternalsAccessor.TestContext;
    }

    public static void SetHasStarted(this FLogContext context, bool hasStarted)
    {
        context.HasStarted = hasStarted;
    }

    public static void SetIsInitialized(this FLogContext context, bool hasStarted)
    {
        context.IsInitialized = hasStarted;
    }

    public static void SetAppenderRegistry(this FLogContext context, IFLogAppenderRegistry appenderRegistry)
    {
        FLogContext.TestFLogContextInternalsAccessor.SetAppenderRegistry(context, appenderRegistry);
    }

    public static void SetLoggerRegistry(this FLogContext context, IFLogLoggerRegistry loggerRegistry)
    {
        FLogContext.TestFLogContextInternalsAccessor.SetLoggerRegistry(context, loggerRegistry);
    }

    public static void SetAsyncRegistry(FLogContext context, IFLoggerAsyncRegistry asyncRegistry)
    {
        FLogContext.TestFLogContextInternalsAccessor.SetAsyncRegistry(context, asyncRegistry);
    }

    public static void SetConfigRegistry(FLogContext context, IFLogConfigRegistry configRegistry)
    {
        FLogContext.TestFLogContextInternalsAccessor.SetConfigRegistry(context, configRegistry);
    }

    public static void SetLogEntryPoolRegistry(FLogContext context, IFLogEntryPoolRegistry logEntryPoolRegistry)
    {
        FLogContext.TestFLogContextInternalsAccessor.SetLogEntryPoolRegistry(context, logEntryPoolRegistry);
    }
}
