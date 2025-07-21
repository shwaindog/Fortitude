// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Hub;

public class FLoggerContext
{
    private static FLoggerContext? singletonInstance;

    private static object syncLock = new ();

    public static FLoggerContext Instance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (syncLock)
                {
                    if (singletonInstance == null)
                    {
                        singletonInstance = 
                            FLoggerBootstrap.Instance.DefaultStartup();

                    }
                }
            }
            return singletonInstance;
        }
    }

    public FLoggerContext(IFLoggerConfigRegistry configRegistry, IFloggerAppenderRegistry appenderRegistry, IFLoggerAsyncRegistry asyncRegistry)
    {
        ConfigRegistry          = configRegistry;
        AppenderRegistry = appenderRegistry;
        AsyncRegistry   = asyncRegistry;
    }

    public IFloggerAppenderRegistry AppenderRegistry { get; set; }

    public IFLoggerLoggerRegistry LoggerRegistry { get; set; }

    public IFLoggerAsyncRegistry AsyncRegistry { get; set; }

    public IFLoggerConfigRegistry ConfigRegistry { get; set; }
}
