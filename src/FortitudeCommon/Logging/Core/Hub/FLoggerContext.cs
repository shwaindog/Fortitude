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

    public IAppenderRegistry AppenderRegistry { get; }

    public IFLoggerRegistry LoggerRegistry { get; }

    public IAsyncRegistry AsyncRegistry { get; }

    public IConfigRegistry ConfigRegistry { get; }
}
