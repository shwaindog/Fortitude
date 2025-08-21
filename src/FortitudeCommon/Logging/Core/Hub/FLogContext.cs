// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.OSWrapper.FileSystem;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLogContext
{
    bool HasStarted { get; }

    bool IsInitialized { get; }
    
    IFileSystemController FLogFileSystemController { get; set; }

    IFLogAppenderRegistry  AppenderRegistry     { get; }
    IFLogLoggerRegistry    LoggerRegistry       { get; }
    IFLoggerAsyncRegistry  AsyncRegistry        { get; }
    IFLogConfigRegistry    ConfigRegistry       { get; }
    IFLogEntryPoolRegistry LogEntryPoolRegistry { get; }
}

public class FLogContext : IFLogContext
{
    internal static IFLogContext? NextInitializingContext;

    private static readonly object SyncLock = new();

    private IFileSystemController? flogFileSystemController;
    
    private IFLogAppenderRegistry  appenderRegistry     = null!;
    private IFLogLoggerRegistry    loggerRegistry       = null!;
    private IFLoggerAsyncRegistry  asyncRegistry        = null!;
    private IFLogConfigRegistry    configRegistry       = null!;
    private IFLogEntryPoolRegistry logEntryPoolRegistry = null!;

    private FLogContext() { }

    public static IFLogContext? NullOnUnstartedContext => FLoggerRoot.CurrentContext;

    public static IFLogContext NewUninitializedContext => new FLogContext();

    public static IFLogContext NextInitializedUnstartedContext
    {
        get
        {
            if (NextInitializingContext == null)
                lock (SyncLock)
                {
                    NextInitializingContext ??= NewUninitializedContext;
                }
            var instance = NewUninitializedContext;
            if (instance.IsInitialized) return instance;
            lock (SyncLock)
            {
                if (!instance.IsInitialized)
                {
                    return instance.DefaultInitializeContext();
                }
            }
            return instance;
        }
        set
        {
            NextInitializingContext = value;
            if (!value.IsInitialized)
            {
                throw new ArgumentException("Expected NextInitializedUnstartedContext to have been initialized");
            }
        }
    }

    public static IFLogContext Context
    {
        get
        {
            var instance = FLoggerRoot.CurrentContext;
            if (instance != null) return instance;
            instance = NextInitializedUnstartedContext;
            if (instance.HasStarted) return instance;
            lock (SyncLock)
            {
                if (!instance.HasStarted)
                {
                    return FLogBootstrap.Instance.StartFlogSetAsCurrentContext((FLogContext)instance);
                }
            }
            return instance;
        }
    }

    public bool HasStarted { get; internal set; }

    public bool IsInitialized { get; internal set; }

    public IFileSystemController FLogFileSystemController
    {
        get => flogFileSystemController ??= new FileSystemController();
        set => flogFileSystemController = value;
    }

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
            get => NextInitializingContext!;
            set =>
                // ReSharper disable once InconsistentlySynchronizedField
                NextInitializingContext = value;
        }
        public static IFLogContext NewEmptyContext => new FLogContext();

        public static IFLogContext NewDefaultConfigContext => new FLogContext().DefaultInitializeContext();

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
//
// internal static class TestFLogContextExtensions
// {
//     public static IFLogContext? SetAsContext(this IFLogContext? context, IFLogContext? newContext)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.TestContext = newContext;
//         return newContext;
//     }
//
//     public static IFLogContext? SetAsNewEmptyContext(this IFLogContext? context)
//     {
//         var emptyContext = FLogContext.TestFLogContextInternalsAccessor.NewEmptyContext;
//         FLogContext.TestFLogContextInternalsAccessor.TestContext = emptyContext;
//         return emptyContext;
//     }
//
//     public static IFLogContext? SetAsNewNewDefaultConfigContext(this IFLogContext? context)
//     {
//         var defaultContext = FLogContext.TestFLogContextInternalsAccessor.NewDefaultConfigContext;
//         FLogContext.TestFLogContextInternalsAccessor.TestContext = defaultContext;
//         return defaultContext;
//     }
//
//     public static IFLogContext? GetContextField()
//     {
//         return FLogContext.TestFLogContextInternalsAccessor.TestContext;
//     }
//
//     public static void SetHasStarted(this FLogContext context, bool hasStarted)
//     {
//         context.HasStarted = hasStarted;
//     }
//
//     public static void SetIsInitialized(this FLogContext context, bool hasStarted)
//     {
//         context.IsInitialized = hasStarted;
//     }
//
//     public static void SetAppenderRegistry(this FLogContext context, IFLogAppenderRegistry appenderRegistry)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.SetAppenderRegistry(context, appenderRegistry);
//     }
//
//     public static void SetLoggerRegistry(this FLogContext context, IFLogLoggerRegistry loggerRegistry)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.SetLoggerRegistry(context, loggerRegistry);
//     }
//
//     public static void SetAsyncRegistry(FLogContext context, IFLoggerAsyncRegistry asyncRegistry)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.SetAsyncRegistry(context, asyncRegistry);
//     }
//
//     public static void SetConfigRegistry(FLogContext context, IFLogConfigRegistry configRegistry)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.SetConfigRegistry(context, configRegistry);
//     }
//
//     public static void SetLogEntryPoolRegistry(FLogContext context, IFLogEntryPoolRegistry logEntryPoolRegistry)
//     {
//         FLogContext.TestFLogContextInternalsAccessor.SetLogEntryPoolRegistry(context, logEntryPoolRegistry);
//     }
// }
