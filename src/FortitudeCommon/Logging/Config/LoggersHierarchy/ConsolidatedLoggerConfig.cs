// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using IFLoggerAppender = FortitudeCommon.Logging.Core.Appending.IFLoggerAppender;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public class ConsolidatedLoggerConfigBuilder
{
    public ConsolidatedLoggerConfigBuilder(string loggerFullName, FLogLevel rootLogLevel, IReadOnlyList<IFLoggerAppender> rootAppenders, ExplicitLogEntryPoolDefinition rootLogEntryPoolDefinition)
    {
        LoggerFullName = loggerFullName;
        LogLevel       = rootLogLevel;
        Appenders      = rootAppenders.ToList();
        LogEntryPool   = rootLogEntryPoolDefinition;
    }

    public string LoggerFullName { get; }

    public FLogLevel LogLevel { get; set; }

    public List<IFLoggerAppender> Appenders { get; }

    public ExplicitLogEntryPoolDefinition LogEntryPool { get; set; }

    public List<ExplicitConfigLoggerNode> DeclaredConfigNodes { get; } = new();

    public ConsolidatedLoggerConfig Build()
    {
        var fullNameSpan = LoggerFullName.AsSpan();
        var name = LoggerFullName.IsNotNullOrEmpty() ? new String(fullNameSpan.LastSplitFrom('.')) : "Root";
        return new(name, LoggerFullName, LogLevel, Appenders.AsReadOnly(), LogEntryPool, DeclaredConfigNodes.AsReadOnly());
    }
}

public record ConsolidatedLoggerConfig
(
    string Name
  , string LoggerFullName
  , FLogLevel LogLevel
  , IReadOnlyList<IFLoggerAppender> Appenders
  , ExplicitLogEntryPoolDefinition LogEntryPool
  , IList<ExplicitConfigLoggerNode> DeclaredConfigNodes
)
{
    // public static ConsolidatedLoggerConfigBuilder Builder(string loggerFullName, ExplicitRootConfigNode rootConfigNode)
    // {
    //     return new ConsolidatedLoggerConfigBuilder(loggerFullName, rootConfigNode.DeclaredRootConfig.LogLevel, rootConfigNode.DeclaredAppendersConfig);
    // }
}
