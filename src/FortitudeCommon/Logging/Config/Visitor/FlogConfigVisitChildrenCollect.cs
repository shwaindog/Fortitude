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
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
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

    public override T Visit(IMutableFLogAppConfig appConfig)
    {
        if (appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appConfig.Appenders.Accept(Me);
        appConfig.RootLogger.Accept(Me);
        appConfig.ConfigSourcesLookup.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if (configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var configSource in configSourcesLookup) configSource.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if (loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return rootLogger.Accept(Me);
            case IMutableFLoggerDescendantConfig descendantLogger: return descendantLogger.Accept(Me);

            default: return Me;
        }
    }

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.LogEntryPool?.Accept(Me);
        loggerRootConfig.Appenders.Accept(Me);
        loggerRootConfig.DescendantLoggers.Accept(Me);
        loggerRootConfig.DescendantActivation.Accept(Me);
        loggerRootConfig.FLogEnvironment.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.LogEntryPool?.Accept(Me);
        loggerDescendantConfig.Appenders.Accept(Me);
        loggerDescendantConfig.DescendantLoggers.Accept(Me);
        loggerDescendantConfig.DescendantActivation.Accept(Me);
        loggerDescendantConfig.FLogEnvironment.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpLogger in childLoggersConfig) kvpLogger.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogBuildTypeAndDeployEnvConfig fLogDeployEnvBuildTypeConfig)
    {
        if (fLogDeployEnvBuildTypeConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if (entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableAppenderReferenceConfig appenderConfig)
    {
        if (appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        if (consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if (appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var appender in appendersCollectionConfig) appender.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if (forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ForwardToAppenders.Accept(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if (bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.InboundQueue.Accept(Me);
        bufferingAppenderConfig.ForwardToAppenders.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogEntryQueueConfig queueConfig)
    {
        if (queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.LogEntryPool?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if (containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if (logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if (sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if (matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var matchOperator in matchOperatorLookupConfig) matchOperator.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if (matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.IsTrue?.Accept(Me);
        matchOperatorExpressionConfig.IsFalse?.Accept(Me);
        matchOperatorExpressionConfig.All?.Accept(Me);
        matchOperatorExpressionConfig.Any?.Accept(Me);
        matchOperatorExpressionConfig.Or?.Accept(Me);
        matchOperatorExpressionConfig.And?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if (extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if (extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpExtractConfig in extractKeyValuesLookupConfig) kvpExtractConfig.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if (matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.StartSequence.Accept(Me);
        matchSequenceOccurenceConfig.OnSequenceComplete.Accept(Me);
        matchSequenceOccurenceConfig.OnSequenceAbort.Accept(Me);
        matchSequenceOccurenceConfig.OnSequenceTimeout.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if (matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.TriggeredWhenEntry?.Accept(Me);
        matchSeqTriggerConfig.OnTriggerExtract?.Accept(Me);
        matchSeqTriggerConfig.AbortWhenEntry?.Accept(Me);
        matchSeqTriggerConfig.NextTriggerStep?.Accept(Me);
        matchSeqTriggerConfig.CompletedWhenEntry?.Accept(Me);
        matchSeqTriggerConfig.SequenceFinalTrigger?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if (messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if (sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.SendMessage?.Accept(Me);
        sequenceHandleActionConfig.SendToAppender?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if (logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.GlobalLogEntryPool.Accept(Me);
        logEntryPoolsInitConfig.LargeMessageLogEntryPool.Accept(Me);
        logEntryPoolsInitConfig.VeryLargeMessageLogEntryPool.Accept(Me);
        logEntryPoolsInitConfig.LoggersGlobalLogEntryPool.Accept(Me);
        logEntryPoolsInitConfig.AppendersGlobalLogEntryPool.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        if (asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesInitConfig.AsyncQueues.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        if (asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var formattingAppender in asyncQueuesLookupConfig) formattingAppender.Value.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        if (asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogInitializationConfig initializationConfig)
    {
        if (initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.AsyncBufferingInit.Accept(Me);
        initializationConfig.LogEntryPoolsInit.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if (fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFlushBufferConfig flushBufferConfig)
    {
        if (flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFileAppenderConfig fileAppenderConfig)
    {
        if (fileAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileAppenderConfig.FlushConfig.Accept(Me);
        fileAppenderConfig.InheritsFrom?.Accept(Me);
        fileAppenderConfig.Defines?.Accept(Me);
        return Me;
    }
}
