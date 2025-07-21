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

public class FLogConfigBackTractCollectToRoot<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) 
    : FLogConfigVisitor<FLogConfigBackTractCollectToRoot<T, TCollect>>, IEnumerable<TCollect> 
    where T : FLogConfigVisitor<T>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;

    public FLogConfigBackTractCollectToRoot(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public FLogConfigBackTractCollectToRoot() : this(new List<TCollect>(), Always) { }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogAppConfig appConfig)
    {
        if(appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if(configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        configSourcesLookup.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if(loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerCommonConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if(loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if(loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) 
    {
        if(childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        childLoggersConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if(entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        entryPoolConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        if(appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appenderConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if(appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appendersCollectionConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) 
    {
        if(forwardToAppendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardToAppendersCollectionConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if(forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if(bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        if(queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if(containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        containsStringMatchConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if(logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logLevelMatchConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if(sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceKeysComparisonConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if(matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorLookupConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if(matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if(extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyExpressionConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if(extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyValuesLookupConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if(matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if(matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if(messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        messageTemplateConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if(sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if(logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig)
    {
        if(asyncBufferingInitializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncBufferingInitializationConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        if(initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig)
    {
        if(inheritingFormatAppendersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        inheritingFormatAppendersConfig.ParentConfig?.Visit(this);
        return this;
    }

    public override FLogConfigBackTractCollectToRoot<T, TCollect> Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if(fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileConfigSourceConfig.ParentConfig?.Visit(this);
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();
}