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

public class FlogConfigVisitParent<T> : FLogConfigVisitor<T>
    where T : FlogConfigVisitParent<T>
{
    public IMutableFLogAppConfig? FoundAppConfig { get; private set; }

    public override T Accept(IMutableFLogAppConfig appConfig)
    {
        FoundAppConfig = appConfig;
        return Me; // is the root of all configs
    }

    public override T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => 
        configSourcesLookup.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => 
        loggerCommonConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig) => 
        loggerRootConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig) => 
        loggerDescendantConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => 
        childLoggersConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLogEntryPoolConfig entryPoolConfig) => 
        entryPoolConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableAppenderReferenceConfig appenderConfig) => 
        appenderConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig) => 
        consoleAppenderConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) => 
        appendersCollectionConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) => 
        forwardToAppendersCollectionConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig) => 
        forwardingAppenderConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => 
        bufferingAppenderConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLogEntryQueueConfig queueConfig) => 
        queueConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig) => 
        containsStringMatchConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig) => 
        logLevelMatchConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchSequenceKeysComparisonConfig logLevelMatchConfig) => 
        logLevelMatchConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig) => 
        matchOperatorLookupConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig) => 
        matchOperatorExpressionConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig) => 
        extractKeyExpressionConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig) => 
        extractKeyValuesLookupConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig) => 
        matchSequenceOccurenceConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig) => 
        matchSeqTriggerConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig) => 
        messageTemplateConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig) => 
        sequenceHandleActionConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig) => 
        logEntryPoolsInitConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig) => 
        asyncQueuesInitConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig) => 
        asyncQueuesLookupConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableAsyncQueueConfig asyncQueueConfig) => 
        asyncQueueConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLogInitializationConfig initializationConfig) => 
        initializationConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig) => 
        inheritingFormatAppendersConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => 
        fileConfigSourceConfig.ParentConfig?.Visit(Me) ?? Me;

    public override T Accept(IMutableFlushBufferConfig flushBufferConfig) => 
        flushBufferConfig.ParentConfig?.Visit(Me) ?? Me;
}

public class FlogConfigVisitParent : FlogConfigVisitParent<FlogConfigVisitParent> { }