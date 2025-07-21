// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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

public interface IFLogConfigVisitor<out T> where T : IFLogConfigVisitor<T>
{
    T Accept(IMutableFLogAppConfig appConfig);

    T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup);

    T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig);

    T Accept(IMutableFLoggerRootConfig loggerRootConfig);

    T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig);

    T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig);

    T Accept(IMutableAppenderReferenceConfig appenderConfig);

    T Accept(IMutableFLogEntryPoolConfig entryPoolConfig);

    T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig);

    T Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig);

    T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig);

    T Accept(IMutableFLogEntryQueueConfig queueConfig);

    T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig);

    T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig);

    T Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig);

    T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig);

    T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig);

    T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig);

    T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig);

    T Accept(IMutableMatchSequenceOccurenceConfig sequenceOccurenceConfig);

    T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig);

    T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig);

    T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig);

    T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig);

    T Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig);

    T Accept(IMutableFLogInitializationConfig initializationConfig);

    T Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig);

    T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig);
}

public class FLogConfigVisitor<T> : IFLogConfigVisitor<T> where T : FLogConfigVisitor<T>
{
    public virtual T Accept(IMutableFLogAppConfig appConfig) => (T)this;

    public virtual T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => (T)this;

    public virtual T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => (T)this;

    public virtual T Accept(IMutableFLoggerRootConfig loggerRootConfig) => (T)this;

    public virtual T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig) => (T)this;

    public virtual T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => (T)this;

    public virtual T Accept(IMutableFLogEntryPoolConfig entryPoolConfig) => (T)this;

    public virtual T Accept(IMutableAppenderReferenceConfig appenderConfig) => (T)this;

    public virtual T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) => (T)this;

    public virtual T Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) => (T)this;

    public virtual T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig) => (T)this;

    public virtual T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => (T)this;

    public virtual T Accept(IMutableFLogEntryQueueConfig queueConfig) => (T)this;

    public virtual T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig) => (T)this;

    public virtual T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig) => (T)this;

    public virtual T Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig) => (T)this;

    public virtual T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig) => (T)this;

    public virtual T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig) => (T)this;

    public virtual T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig) => (T)this;

    public virtual T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig) => (T)this;

    public virtual T Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig) => (T)this;

    public virtual T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig) => (T)this;

    public virtual T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig) => (T)this;

    public virtual T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig) => (T)this;

    public virtual T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig) => (T)this;

    public virtual T Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig) => (T)this;

    public virtual T Accept(IMutableFLogInitializationConfig initializationConfig) => (T)this;

    public virtual T Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig) => (T)this;

    public virtual T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => (T)this;
}
