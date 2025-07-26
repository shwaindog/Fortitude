using System.Collections;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;
using FortitudeCommon.Logging.Config.ConfigSources;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;

namespace FortitudeCommon.Logging.Config.Visitor;

public class FLogConfigBackTractCollectToRoot<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) 
    : FLogConfigVisitor<T>, IEnumerable<TCollect> 
    where T : FLogConfigBackTractCollectToRoot<T, TCollect>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;

    public FLogConfigBackTractCollectToRoot(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public FLogConfigBackTractCollectToRoot() : this(new List<TCollect>(), Always) { }

    public override T Accept(IMutableFLogAppConfig appConfig)
    {
        if(appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if(configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        configSourcesLookup.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if(loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerCommonConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if(loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if(loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) 
    {
        if(childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        childLoggersConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if(entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        entryPoolConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        if(appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appenderConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig) 
    {
        if(consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        consoleAppenderConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if(appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appendersCollectionConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) 
    {
        if(forwardToAppendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardToAppendersCollectionConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if(forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if(bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        if(queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if(containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        containsStringMatchConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if(logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logLevelMatchConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if(sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceKeysComparisonConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if(matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorLookupConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if(matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if(extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyExpressionConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if(extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyValuesLookupConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if(matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if(matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if(messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        messageTemplateConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if(sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if(logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        if(asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesInitConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        if(asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesLookupConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        if(asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueueConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        if(initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig)
    {
        if(inheritingFormatAppendersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        inheritingFormatAppendersConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if(fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileConfigSourceConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFlushBufferConfig flushBufferConfig)
    {
        if(flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        flushBufferConfig.ParentConfig?.Visit(Me);
        return Me;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();
}

public class FLogConfigBackTractCollectToRoot<TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) : 
    FLogConfigBackTractCollectToRoot<FLogConfigBackTractCollectToRoot<TCollect>, TCollect>(found, meetsCondition);