using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Logging.Config;

namespace FortitudeCommon.Logging.Core.Hub;

public class FLoggerBootstrap
{
    private static FLoggerContext?   initialisingContext;
    private static FLoggerBootstrap? singletonInstance;

    private static object syncLock = new ();

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
        if (initialisingContext == null)
        {
            lock (syncLock)
            {
                initialisingContext ??= new FLoggerContext();
            }
        }


        // check for FLoggerConfig.json, FLogger-Username-Config.json, FLogger-Machine-Config.json, FLogger-Username-Machine-Config.json, 

        // check Web / App.config

        // Do Default config

        return null!;
    }

    public FLoggerContext Start(string filePath)
    {
        if (initialisingContext == null)
        {
            lock (syncLock)
            {
                initialisingContext ??= new FLoggerContext();
            }
        }
        return null!;
    }

    public FLoggerContext Start(IFLoggerAppConfig config)
    {
        if (initialisingContext == null)
        {
            lock (syncLock)
            {
                initialisingContext ??= new FLoggerContext();
            }
        }
        return Initialize(config, initialisingContext);
    }

    protected virtual FLoggerContext Initialize( IFLoggerAppConfig config, FLoggerContext toBeUpdated)
    {
        return toBeUpdated;
    }
}