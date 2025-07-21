using FortitudeCommon.Logging.Config;

namespace FortitudeCommon.Logging.Core.Hub;

public class FLoggerBootstrap
{
    private static FLoggerContext?   initialisingContext;
    private static FLoggerBootstrap? singletonInstance;

    private static object syncLock = new();

    public static FLoggerBootstrap Instance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (syncLock)
                {
                    singletonInstance ??= new FLoggerBootstrap();
                }
            }
            return singletonInstance;
        }
        set => singletonInstance = value;
    }

    public FLoggerContext DefaultStartup()
    {
        var ctx = GetSingletonContextInstance();

        // check for FLoggerConfig.json, FLogger-Username-Config.json, FLogger-Machine-Config.json, FLogger-Username-Machine-Config.json, 

        // check Web / App.config

        // Do Default config

        return null!;
    }

    public FLoggerContext Start(string filePath)
    {
        var ctx = GetSingletonContextInstance();
        return null!;
    }

    public FLoggerContext Start(IFLogAppConfig config)
    {
        var               ctx            = GetSingletonContextInstance();
        IFLoggerLoggerRegistry? loggerRegistry = ctx.LoggerRegistry ?? FLoggerImplementationRegistry.CreateLoggerRegistry(config.RootLogger);
        return Initialize(config, ctx);
    }

    private static FLoggerContext GetSingletonContextInstance()
    {
        if (initialisingContext == null)
        {
            lock (syncLock)
            {
                initialisingContext ??=
                    new FLoggerContext
                        (FLoggerImplementationRegistry.CreateConfigRegistry()
                       , FLoggerImplementationRegistry.CreateAppenderRegistry()
                       , FLoggerImplementationRegistry.CreateAsyncRegistry());
            }
        }
        return initialisingContext;
    }

    protected virtual FLoggerContext Initialize(IFLogAppConfig config, FLoggerContext toBeUpdated)
    {
        return toBeUpdated;
    }
}
