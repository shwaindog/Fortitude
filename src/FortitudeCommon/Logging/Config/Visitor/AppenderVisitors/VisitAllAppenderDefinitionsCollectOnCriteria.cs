// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.AppenderVisitors;

public class VisitAllAppenderDefinitionsCollectOnCriteria<T, TAppendConfig>
    (List<TAppendConfig> found, Predicate<TAppendConfig>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<TAppendConfig>
    where T : VisitAllAppenderDefinitionsCollectOnCriteria<T, TAppendConfig>
    where TAppendConfig : IMutableAppenderReferenceConfig
{
    protected static readonly Predicate<TAppendConfig> Always = _ => true;

    private Predicate<TAppendConfig> meets = meetsCondition ?? Always;

    public VisitAllAppenderDefinitionsCollectOnCriteria(Predicate<TAppendConfig> meetsCondition)
        : this(new List<TAppendConfig>(), meetsCondition) { }

    public VisitAllAppenderDefinitionsCollectOnCriteria() :
        this(new List<TAppendConfig>(), Always) { }

    public List<TAppendConfig> Appenders => found;

    public IFLogAppConfig? FoundRootAppConfig { get; private set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAppendConfig> GetEnumerator() => found.GetEnumerator();

    public VisitAllAppenderDefinitionsCollectOnCriteria<T, TAppendConfig> Initialize
        (Predicate<TAppendConfig>? meetsCondition = null)
    {
        Appenders.Clear();
        meets = meetsCondition ?? Always;

        return this;
    }

    public override T Visit(IMutableFLogAppConfig appConfig)
    {
        FoundRootAppConfig = appConfig;
        appConfig.RootLogger.Accept(Me);
        foreach (var appender in appConfig.Appenders) appender.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return Visit(rootLogger);
            case IMutableFLoggerDescendantConfig descendantLogger: return Visit(descendantLogger);

            default: return Me;
        }
    }

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if (FoundRootAppConfig == null) return loggerRootConfig.ParentConfig?.Accept(Me) ?? Me;
        loggerRootConfig.DescendantLoggers.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (FoundRootAppConfig == null) return loggerDescendantConfig.ParentConfig?.Accept(Me) ?? Me;
        loggerDescendantConfig.Appenders.Accept(Me);
        loggerDescendantConfig.DescendantLoggers.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (FoundRootAppConfig == null) return childLoggersConfig.ParentConfig?.Accept(Me) ?? Me;
        foreach (var childLogger in childLoggersConfig) childLogger.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAppenderReferenceConfig appenderConfig)
    {
        if (FoundRootAppConfig == null) return appenderConfig.ParentConfig?.Accept(Me) ?? Me;

        if (appenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        if (FoundRootAppConfig == null) return consoleAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

        if (consoleAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        consoleAppenderConfig.Defines?.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if (FoundRootAppConfig == null) return appendersCollectionConfig.ParentConfig?.Accept(Me) ?? Me;
        foreach (var childAppender in appendersCollectionConfig) childAppender.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if (FoundRootAppConfig == null) return forwardingAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

        if (forwardingAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ForwardToAppenders?.Accept(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if (FoundRootAppConfig == null) return bufferingAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

        if (bufferingAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.ForwardToAppenders?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFileAppenderConfig fileAppenderConfig)
    {
        if (FoundRootAppConfig == null) return fileAppenderConfig.ParentConfig?.Accept(Me) ?? Me;
        if (fileAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override void StateReset()
    {
        Appenders.Clear();

        base.StateReset();
    }
}
