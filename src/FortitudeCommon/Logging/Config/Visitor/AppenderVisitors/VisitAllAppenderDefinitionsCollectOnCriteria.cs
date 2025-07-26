// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
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

    public List<TAppendConfig> Appenders => found;

    public VisitAllAppenderDefinitionsCollectOnCriteria(Predicate<TAppendConfig> meetsCondition) 
        : this(new List<TAppendConfig>(), meetsCondition) { }

    public VisitAllAppenderDefinitionsCollectOnCriteria() : 
        this(new List<TAppendConfig>(), Always) { }

    public VisitAllAppenderDefinitionsCollectOnCriteria<T, TAppendConfig> Initialize
        (Predicate<TAppendConfig>? meetsCondition = null)
    {
        Appenders.Clear();
        meets = meetsCondition ?? Always;

        return this;
    }
    
    public IFLogAppConfig? FoundRootAppConfig { get; private set; }

    public override T Accept(IMutableFLogAppConfig appConfig)
    {
        FoundRootAppConfig = appConfig;
        appConfig.RootLogger.Visit(Me);
        foreach (var appender in appConfig.Appenders)
        {
            appender.Value.Visit(Me);
        }
        return Me;
    }

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return Accept(rootLogger);
            case IMutableFLoggerDescendantConfig descendantLogger: return Accept(descendantLogger);

            default: return Me;
        }
    }

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return loggerRootConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        loggerRootConfig.DescendantLoggers.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return loggerDescendantConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        loggerDescendantConfig.Appenders.Visit(Me);
        loggerDescendantConfig.DescendantLoggers.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return childLoggersConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        foreach (var childLogger in childLoggersConfig)
        {
            return childLogger.Value.Visit(Me);
        }
        return Me;
    }

    public override T Accept(IMutableAppenderReferenceConfig appenderConfig) 
    {
        if (FoundRootAppConfig == null)
        {
            return appenderConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        
        if(appenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return consoleAppenderConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        
        if(consoleAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        consoleAppenderConfig.Defines?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return appendersCollectionConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        foreach (var childAppender in appendersCollectionConfig)  
        {
            return childAppender.Value.Visit(Me);
        }
        return Me;
    }

    public override T Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return forwardToAppendersCollectionConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        foreach (var childAppender in forwardToAppendersCollectionConfig)  
        {
            return childAppender.Value.Visit(Me);
        }
        return Me;
    }

    public override T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig) 
    {
        if (FoundRootAppConfig == null)
        {
            return forwardingAppenderConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        
        if(forwardingAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ForwardToAppenders?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if (FoundRootAppConfig == null)
        {
            return bufferingAppenderConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        
        if(bufferingAppenderConfig is TAppendConfig toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.ForwardToAppenders?.Visit(Me);
        return Me;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAppendConfig> GetEnumerator() => found.GetEnumerator();

    public override void StateReset()
    {
        Appenders.Clear();

        base.StateReset();
    }
}
