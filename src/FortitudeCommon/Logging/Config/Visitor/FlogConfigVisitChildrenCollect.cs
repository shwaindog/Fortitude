// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
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

public class FlogConfigVisitChildrenCollect<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<TCollect>
    where T : FlogConfigVisitChildrenCollect<T, TCollect>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;
    public FlogConfigVisitChildrenCollect(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }
    public FlogConfigVisitChildrenCollect() : this(new List<TCollect>()) { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();

    public override T Accept(IMutableFLogAppConfig appConfig)
    {
        if (appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appConfig.Appenders.Visit(Me);
        appConfig.RootLogger.Visit(Me);
        appConfig.ConfigSourcesLookup.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if (configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var configSource in configSourcesLookup) configSource.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if (loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return rootLogger.Visit(Me);
            case IMutableFLoggerDescendantConfig descendantLogger: return descendantLogger.Visit(Me);

            default: return Me;
        }
    }

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.LogEntryPool?.Visit(Me);
        loggerRootConfig.Appenders.Visit(Me);
        loggerRootConfig.DescendantLoggers.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.LogEntryPool?.Visit(Me);
        loggerDescendantConfig.Appenders.Visit(Me);
        loggerDescendantConfig.DescendantLoggers.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpLogger in childLoggersConfig) kvpLogger.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if (entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        if (appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        if (consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if (appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var appender in appendersCollectionConfig) appender.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if (forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ForwardToAppenders.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if (bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.InboundQueue.Visit(Me);
        bufferingAppenderConfig.ForwardToAppenders.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        if (queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.LogEntryPool?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if (containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if (logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if (sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if (matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var matchOperator in matchOperatorLookupConfig) matchOperator.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if (matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.IsTrue?.Visit(Me);
        matchOperatorExpressionConfig.IsFalse?.Visit(Me);
        matchOperatorExpressionConfig.All?.Visit(Me);
        matchOperatorExpressionConfig.Any?.Visit(Me);
        matchOperatorExpressionConfig.Or?.Visit(Me);
        matchOperatorExpressionConfig.And?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if (extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if (extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpExtractConfig in extractKeyValuesLookupConfig) kvpExtractConfig.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if (matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.StartSequence.Visit(Me);
        matchSequenceOccurenceConfig.OnSequenceComplete.Visit(Me);
        matchSequenceOccurenceConfig.OnSequenceAbort.Visit(Me);
        matchSequenceOccurenceConfig.OnSequenceTimeout.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if (matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.TriggeredWhenEntry?.Visit(Me);
        matchSeqTriggerConfig.OnTriggerExtract?.Visit(Me);
        matchSeqTriggerConfig.AbortWhenEntry?.Visit(Me);
        matchSeqTriggerConfig.NextTriggerStep?.Visit(Me);
        matchSeqTriggerConfig.CompletedWhenEntry?.Visit(Me);
        matchSeqTriggerConfig.SequenceFinalTrigger?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if (messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if (sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.SendMessage?.Visit(Me);
        sequenceHandleActionConfig.SendToAppender?.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if (logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.GlobalLogEntryPool.Visit(Me);
        logEntryPoolsInitConfig.LargeMessageLogEntryPool.Visit(Me);
        logEntryPoolsInitConfig.VeryLargeMessageLogEntryPool.Visit(Me);
        logEntryPoolsInitConfig.LoggersGlobalLogEntryPool.Visit(Me);
        logEntryPoolsInitConfig.AppendersGlobalLogEntryPool.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        if (asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesInitConfig.AsyncQueues.Visit(Me);
        return Me;
    }

    public override T Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        if (asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var formattingAppender in asyncQueuesLookupConfig) formattingAppender.Value.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        if (asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        if (initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.AsyncBufferingInit.Visit(Me);
        initializationConfig.LogEntryPoolsInit.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if (fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFlushBufferConfig flushBufferConfig)
    {
        if (flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFileAppenderConfig fileAppenderConfig)
    {
        if (fileAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileAppenderConfig.FlushConfig.Visit(Me);
        fileAppenderConfig.InheritsFrom?.Visit(Me);
        fileAppenderConfig.Defines?.Visit(Me);
        return Me;
    }
}
