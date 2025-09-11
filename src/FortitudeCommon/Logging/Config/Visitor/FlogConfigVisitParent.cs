// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
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

public class FlogConfigVisitParent<T> : FLogConfigVisitor<T> where T : FlogConfigVisitParent<T>
{
    public IMutableFLogAppConfig? FoundAppConfig { get; private set; }

    public override T Visit(IMutableFLogAppConfig appConfig)
    {
        FoundAppConfig = appConfig;
        return Me; // is the root of all configs
    }

    public override T Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => configSourcesLookup.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => loggerCommonConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig) => loggerRootConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig) => loggerDescendantConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => childLoggersConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLogEntryPoolConfig entryPoolConfig) => entryPoolConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableAppenderReferenceConfig appenderConfig) => appenderConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig) => consoleAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) =>
        appendersCollectionConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig) => forwardingAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => bufferingAppenderConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLogEntryQueueConfig queueConfig) => queueConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig) =>
        containsStringMatchConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchLogLevelConfig logLevelMatchConfig) => logLevelMatchConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchSequenceKeysComparisonConfig logLevelMatchConfig) => logLevelMatchConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig) =>
        matchOperatorLookupConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig) =>
        matchOperatorExpressionConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig) =>
        extractKeyExpressionConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig) =>
        extractKeyValuesLookupConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig) =>
        matchSequenceOccurenceConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig) => matchSeqTriggerConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableLogMessageTemplateConfig messageTemplateConfig) => messageTemplateConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig) =>
        sequenceHandleActionConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig) =>
        logEntryPoolsInitConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig) => asyncQueuesInitConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig) => asyncQueuesLookupConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableAsyncQueueConfig asyncQueueConfig) => asyncQueueConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLogInitializationConfig initializationConfig) => initializationConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => fileConfigSourceConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFlushBufferConfig flushBufferConfig) => flushBufferConfig.ParentConfig?.Accept(Me) ?? Me;

    public override T Visit(IMutableFileAppenderConfig fileAppenderConfig) => fileAppenderConfig.ParentConfig?.Accept(Me) ?? Me;
}

public class FlogConfigVisitParent : FlogConfigVisitParent<FlogConfigVisitParent> { }
