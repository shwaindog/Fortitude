using FortitudeCommon.Logging.Config;

namespace FortitudeCommon.Logging.Core;


public enum LoggerTreeType
{
    Unknown
   , Root
   , Branch
   , Leaf
}

public interface IFLoggerCommon
{
    FLogLevel LogLevel { get; }
    LoggerTreeType TreeType { get; }
    string         Name     { get; }
    string         FullName { get; }
}


public interface IBranchFLogger : IFLoggerCommon
{
    IFLoggerCommon Parent   { get; }
}

public interface IFLogger : IBranchFLogger
{
    IMutableFLogEntry? Trace { get; }
    IMutableFLogEntry? Debug { get; }
    IMutableFLogEntry? Info  { get; }
    IMutableFLogEntry? Warn  { get; }
    IMutableFLogEntry? Error { get; }
}