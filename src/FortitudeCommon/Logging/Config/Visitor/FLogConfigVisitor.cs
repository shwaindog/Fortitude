// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
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
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Config.Visitor;

public interface IFLogConfigVisitor<TBase> : IReusableObject<IFLogConfigVisitor<TBase>>
    where TBase : IFLogConfigVisitor<TBase>
{
    TBase Accept(IMutableFLogAppConfig appConfig);

    TBase Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup);

    TBase Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig);

    TBase Accept(IMutableFLoggerRootConfig loggerRootConfig);

    TBase Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig);

    TBase Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig);

    TBase Accept(IMutableAppenderReferenceConfig appenderConfig);

    TBase Accept(IMutableConsoleAppenderConfig consoleAppenderConfig);

    TBase Accept(IMutableFLogEntryPoolConfig entryPoolConfig);

    TBase Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig);

    TBase Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig);

    TBase Accept(IMutableFLogEntryQueueConfig queueConfig);

    TBase Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig);

    TBase Accept(IMutableMatchLogLevelConfig logLevelMatchConfig);

    TBase Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig);

    TBase Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig);

    TBase Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig);

    TBase Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig);

    TBase Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig);

    TBase Accept(IMutableMatchSequenceOccurenceConfig sequenceOccurenceConfig);

    TBase Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig);

    TBase Accept(IMutableLogMessageTemplateConfig messageTemplateConfig);

    TBase Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig);

    TBase Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig);

    TBase Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig);

    TBase Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig);

    TBase Accept(IMutableAsyncQueueConfig asyncQueueConfig);

    TBase Accept(IMutableFLogInitializationConfig initializationConfig);

    TBase Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig);

    TBase Accept(IMutableFlushBufferConfig flushBufferConfig);

    TBase Accept(IMutableFileAppenderConfig fileAppenderConfig);
}

public class FLogConfigVisitor<T> : ReusableObject<IFLogConfigVisitor<T>>, IFLogConfigVisitor<T>
    where T : FLogConfigVisitor<T>
{
    protected T Me => (T)this;

    public virtual T Accept(IMutableFLogAppConfig appConfig) => (T)this;

    public virtual T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => (T)this;

    public virtual T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => (T)this;

    public virtual T Accept(IMutableFLoggerRootConfig loggerRootConfig) => (T)this;

    public virtual T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig) => (T)this;

    public virtual T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => (T)this;

    public virtual T Accept(IMutableFLogEntryPoolConfig entryPoolConfig) => (T)this;

    public virtual T Accept(IMutableAppenderReferenceConfig appenderConfig) => (T)this;

    public virtual T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig) => (T)this;

    public virtual T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) => (T)this;

    public virtual T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig) => (T)this;

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

    public virtual T Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig) => (T)this;

    public virtual T Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig) => (T)this;

    public virtual T Accept(IMutableAsyncQueueConfig asyncQueueConfig) => (T)this;

    public virtual T Accept(IMutableFLogInitializationConfig initializationConfig) => (T)this;

    public virtual T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => (T)this;

    public virtual T Accept(IMutableFlushBufferConfig flushBufferConfig) => (T)this;

    public virtual T Accept(IMutableFileAppenderConfig fileAppenderConfig) => (T)this;

    public override IFLogConfigVisitor<T> Clone() =>
        Recycler?.Borrow<FLogConfigVisitor<T>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogConfigVisitor<T>();

    public override IFLogConfigVisitor<T> CopyFrom(IFLogConfigVisitor<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

    public virtual T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => (T)this;
}
