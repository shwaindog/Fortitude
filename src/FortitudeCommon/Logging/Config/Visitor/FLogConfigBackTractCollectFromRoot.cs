using System.Collections;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;
using FortitudeCommon.Logging.Config.ConfigSources;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;

namespace FortitudeCommon.Logging.Config.Visitor;

public class FLogConfigBackTractCollectFromRoot<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) 
    : FLogConfigVisitor<FLogConfigBackTractCollectFromRoot<T, TCollect>>, IEnumerable<TCollect> 
    where T : FLogConfigVisitor<T>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;

    public FLogConfigBackTractCollectFromRoot(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public FLogConfigBackTractCollectFromRoot() : this(new List<TCollect>(), Always) { }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogAppConfig appConfig)
    {
        if(appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        configSourcesLookup.ParentConfig?.Visit(this);
        if(configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        loggerCommonConfig.ParentConfig?.Visit(this);
        if(loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        loggerRootConfig.ParentConfig?.Visit(this);
        if(loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        loggerDescendantConfig.ParentConfig?.Visit(this);
        if(loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) 
    {
        childLoggersConfig.ParentConfig?.Visit(this);
        if(childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        entryPoolConfig.ParentConfig?.Visit(this);
        if(entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        appenderConfig.ParentConfig?.Visit(this);
        if(appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        appendersCollectionConfig.ParentConfig?.Visit(this);
        if(appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) 
    {
        forwardToAppendersCollectionConfig.ParentConfig?.Visit(this);
        if(forwardToAppendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        forwardingAppenderConfig.ParentConfig?.Visit(this);
        if(forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        bufferingAppenderConfig.ParentConfig?.Visit(this);
        if(bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        queueConfig.ParentConfig?.Visit(this);
        if(queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        containsStringMatchConfig.ParentConfig?.Visit(this);
        if(containsStringMatchConfig is TCollect toAdd) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        logLevelMatchConfig.ParentConfig?.Visit(this);
        if(logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        sequenceKeysComparisonConfig.ParentConfig?.Visit(this);
        if(sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        matchOperatorLookupConfig.ParentConfig?.Visit(this);
        if(matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        matchOperatorExpressionConfig.ParentConfig?.Visit(this);
        if(matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        extractKeyExpressionConfig.ParentConfig?.Visit(this);
        if(extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        extractKeyValuesLookupConfig.ParentConfig?.Visit(this);
        if(extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        matchSequenceOccurenceConfig.ParentConfig?.Visit(this);
        if(matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        matchSeqTriggerConfig.ParentConfig?.Visit(this);
        if(matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        messageTemplateConfig.ParentConfig?.Visit(this);
        if(messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        sequenceHandleActionConfig.ParentConfig?.Visit(this);
        if(sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        logEntryPoolsInitConfig.ParentConfig?.Visit(this);
        if(logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig)
    {
        asyncBufferingInitializationConfig.ParentConfig?.Visit(this);
        if(asyncBufferingInitializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        initializationConfig.ParentConfig?.Visit(this);
        if(initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig)
    {
        inheritingFormatAppendersConfig.ParentConfig?.Visit(this);
        if(inheritingFormatAppendersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectFromRoot<T, TCollect> Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        fileConfigSourceConfig.ParentConfig?.Visit(this);
        if(fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();
}