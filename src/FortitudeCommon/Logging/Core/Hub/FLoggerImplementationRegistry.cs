using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Core.Hub;

public static class FLoggerImplementationRegistry
{
    static FLoggerImplementationRegistry()
    {
        CreateConfigRegistry   = DefaultConfigRegistry;
        CreateAppenderRegistry = DefaultAppenderRegistry;
        CreateLoggerRegistry   = DefaultLoggerRegistry;
        CreateAsyncRegistry    = DefaultAsyncRegistry;
    }

    public static Func<IFLoggerConfigRegistry> CreateConfigRegistry { get; set; }

    public static Func<IFloggerAppenderRegistry>  CreateAppenderRegistry { get; set; }

    public static Func<IFLoggerRootConfig, IFLoggerLoggerRegistry>  CreateLoggerRegistry { get; set; }

    public static Func<IFLoggerAsyncRegistry>  CreateAsyncRegistry { get; set; }

    private static IFLoggerConfigRegistry DefaultConfigRegistry()
    {
        return new FLoggerConfigRegistry();
    }

    private static IFloggerAppenderRegistry DefaultAppenderRegistry()
    {
        return new FloggerAppenderRegistry();
    }

    private static IFLoggerLoggerRegistry DefaultLoggerRegistry(IFLoggerRootConfig rootLogger)
    {
        return new FLoggerLoggerRegistry(rootLogger);
    }

    private static IFLoggerAsyncRegistry DefaultAsyncRegistry()
    {
        return new FLoggerAsyncRegistry();
    }

}