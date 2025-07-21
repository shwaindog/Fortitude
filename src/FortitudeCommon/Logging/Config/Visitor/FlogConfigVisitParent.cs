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

public class FlogConfigVisitParent<T> : FLogConfigVisitor<FlogConfigVisitParent<T>> where T : FLogConfigVisitor<T>
{
    public IMutableFLogAppConfig? FoundAppConfig { get; private set; }

    public override FlogConfigVisitParent<T> Accept(IMutableFLogAppConfig appConfig)
    {
        FoundAppConfig = appConfig;
        return this; // is the root of all configs
    }

    public override FlogConfigVisitParent<T> Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => 
        configSourcesLookup.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => 
        loggerCommonConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLoggerRootConfig loggerRootConfig) => 
        loggerRootConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig) => 
        loggerDescendantConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => 
        childLoggersConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLogEntryPoolConfig entryPoolConfig) => 
        entryPoolConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableAppenderReferenceConfig appenderConfig) => 
        appenderConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) => 
        appendersCollectionConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) => 
        forwardToAppendersCollectionConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig) => 
        forwardingAppenderConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => 
        bufferingAppenderConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLogEntryQueueConfig queueConfig) => 
        queueConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig) => 
        containsStringMatchConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchLogLevelConfig logLevelMatchConfig) => 
        logLevelMatchConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchSequenceKeysComparisonConfig logLevelMatchConfig) => 
        logLevelMatchConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig) => 
        matchOperatorLookupConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig) => 
        matchOperatorExpressionConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig) => 
        extractKeyExpressionConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig) => 
        extractKeyValuesLookupConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig) => 
        matchSequenceOccurenceConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig) => 
        matchSeqTriggerConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableLogMessageTemplateConfig messageTemplateConfig) => 
        messageTemplateConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig) => 
        sequenceHandleActionConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig) => 
        logEntryPoolsInitConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig) => 
        asyncBufferingInitializationConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLogInitializationConfig initializationConfig) => 
        initializationConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig) => 
        inheritingFormatAppendersConfig.ParentConfig?.Visit(this) ?? this;

    public override FlogConfigVisitParent<T> Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => 
        fileConfigSourceConfig.ParentConfig?.Visit(this) ?? this;
}