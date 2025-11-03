// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
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
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Config.Visitor;

public interface IFLogConfigVisitor<TBase> : IReusableObject<IFLogConfigVisitor<TBase>>
    where TBase : IFLogConfigVisitor<TBase>
{
    TBase Visit(IMutableFLogAppConfig appConfig);

    TBase Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup);

    TBase Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig);

    TBase Visit(IMutableFLoggerRootConfig loggerRootConfig);

    TBase Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig);

    TBase Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig);
    
    TBase Visit(IMutableFLogBuildTypeAndDeployEnvConfig fLogDeployEnvBuildTypeConfig);

    TBase Visit(IMutableAppenderReferenceConfig appenderConfig);

    TBase Visit(IMutableConsoleAppenderConfig consoleAppenderConfig);

    TBase Visit(IMutableFLogEntryPoolConfig entryPoolConfig);

    TBase Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig);

    TBase Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig);

    TBase Visit(IMutableFLogEntryQueueConfig queueConfig);

    TBase Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig);

    TBase Visit(IMutableMatchLogLevelConfig logLevelMatchConfig);

    TBase Visit(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig);

    TBase Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig);

    TBase Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig);

    TBase Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig);

    TBase Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig);

    TBase Visit(IMutableMatchSequenceOccurenceConfig sequenceOccurenceConfig);

    TBase Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig);

    TBase Visit(IMutableLogMessageTemplateConfig messageTemplateConfig);

    TBase Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig);

    TBase Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig);

    TBase Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig);

    TBase Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig);

    TBase Visit(IMutableAsyncQueueConfig asyncQueueConfig);

    TBase Visit(IMutableFLogInitializationConfig initializationConfig);

    TBase Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig);

    TBase Visit(IMutableFlushBufferConfig flushBufferConfig);

    TBase Visit(IMutableFileAppenderConfig fileAppenderConfig);
}

public class FLogConfigVisitor<T> : ReusableObject<IFLogConfigVisitor<T>>, IFLogConfigVisitor<T>
    where T : FLogConfigVisitor<T>
{
    protected T Me => (T)this;

    public virtual T Visit(IMutableFLogAppConfig appConfig) => (T)this;

    public virtual T Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup) => (T)this;

    public virtual T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig) => (T)this;

    public virtual T Visit(IMutableFLoggerRootConfig loggerRootConfig) => (T)this;

    public virtual T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig) => (T)this;

    public virtual T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => (T)this;
    
    public virtual T Visit(IMutableFLogBuildTypeAndDeployEnvConfig fLogDeployEnvBuildTypeConfig) => (T)this;

    public virtual T Visit(IMutableFLogEntryPoolConfig entryPoolConfig) => (T)this;

    public virtual T Visit(IMutableAppenderReferenceConfig appenderConfig) => (T)this;

    public virtual T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig) => (T)this;

    public virtual T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig) => (T)this;

    public virtual T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig) => (T)this;

    public virtual T Visit(IMutableFLogEntryQueueConfig queueConfig) => (T)this;

    public virtual T Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig) => (T)this;

    public virtual T Visit(IMutableMatchLogLevelConfig logLevelMatchConfig) => (T)this;

    public virtual T Visit(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig) => (T)this;

    public virtual T Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig) => (T)this;

    public virtual T Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig) => (T)this;

    public virtual T Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig) => (T)this;

    public virtual T Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig) => (T)this;

    public virtual T Visit(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig) => (T)this;

    public virtual T Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig) => (T)this;

    public virtual T Visit(IMutableLogMessageTemplateConfig messageTemplateConfig) => (T)this;

    public virtual T Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig) => (T)this;

    public virtual T Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig) => (T)this;

    public virtual T Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig) => (T)this;

    public virtual T Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig) => (T)this;

    public virtual T Visit(IMutableAsyncQueueConfig asyncQueueConfig) => (T)this;

    public virtual T Visit(IMutableFLogInitializationConfig initializationConfig) => (T)this;

    public virtual T Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig) => (T)this;

    public virtual T Visit(IMutableFlushBufferConfig flushBufferConfig) => (T)this;

    public virtual T Visit(IMutableFileAppenderConfig fileAppenderConfig) => (T)this;

    public override IFLogConfigVisitor<T> Clone() =>
        Recycler?.Borrow<FLogConfigVisitor<T>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogConfigVisitor<T>();

    public override IFLogConfigVisitor<T> CopyFrom(IFLogConfigVisitor<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

    public virtual T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig) => (T)this;
}
