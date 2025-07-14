using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerRootConfig : IFLoggerTreeCommonConfig
{
    INamedChildLoggersLookupConfig AllLoggers();

    IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName);

    ExplicitRootConfigNode CrystallisedDeclaredConfigTree();
}

public interface IMutableFLoggerRootConfig : IFLoggerRootConfig, IMutableFLoggerTreeCommonConfig
{
    new FLogLevel LogLevel { get; set; }
}
