using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerRootConfig : IFLoggerTreeCommonConfig
{
    INamedChildLoggersLookupConfig Loggers { get; }

    INamedChildLoggersLookupConfig AllLoggers();

    IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName);
}

public interface IMutableFLoggerRootConfig : IFLoggerRootConfig, IMutableFLoggerMatchedAppenders
{
    new IMutableNamedChildLoggersLookupConfig Loggers { get; set; }

    new FLogLevel LogLevel { get; set; }
}
