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

public class FlogConfigVisitChildrenCollect<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) 
    : FLogConfigVisitor<FlogConfigVisitChildrenCollect<T, TCollect>>, IEnumerable<TCollect> 
    where T : FLogConfigVisitor<T>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;
    public FlogConfigVisitChildrenCollect(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }
    public FlogConfigVisitChildrenCollect() : this(new List<TCollect>()) { }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogAppConfig appConfig)
    {
        if(appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appConfig.Appenders.Visit(this);
        appConfig.RootLogger.Visit(this);
        appConfig.ConfigSourcesLookup.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if(configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var configSource in configSourcesLookup)
        {
            configSource.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if(loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger : 
                return rootLogger.Visit(this);
            case IMutableFLoggerDescendantConfig descendantLogger : 
                return descendantLogger.Visit(this);
            default: return this;
        }
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if(loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.LogEntryPool?.Visit(this);
        loggerRootConfig.Appenders.Visit(this);
        loggerRootConfig.DescendantLoggers.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if(loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.LogEntryPool?.Visit(this);
        loggerDescendantConfig.Appenders.Visit(this);
        loggerDescendantConfig.DescendantLoggers.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) 
    {
        if(childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpLogger in childLoggersConfig)
        {
            kvpLogger.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if(entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        if(appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if(appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var appender in appendersCollectionConfig)
        {
            appender.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableForwardingAppendersLookupConfig forwardToAppendersCollectionConfig) 
    {
        if(forwardToAppendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var fwdToAppender in forwardToAppendersCollectionConfig)
        {
            fwdToAppender.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if(forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ForwardToAppenders.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if(bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.InboundQueue.Visit(this);
        bufferingAppenderConfig.ForwardToAppenders.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        if(queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.LogEntryPool?.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if(containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if(logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if(sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if(matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var matchOperator in matchOperatorLookupConfig)
        {
            matchOperator.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if(matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.IsTrue?.Visit(this);
        matchOperatorExpressionConfig.IsFalse?.Visit(this);
        matchOperatorExpressionConfig.All?.Visit(this);
        matchOperatorExpressionConfig.Any?.Visit(this);
        matchOperatorExpressionConfig.Or?.Visit(this);
        matchOperatorExpressionConfig.And?.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if(extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if(extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var kvpExtractConfig in extractKeyValuesLookupConfig)
        {
            kvpExtractConfig.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if(matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.StartSequence.Visit(this);
        matchSequenceOccurenceConfig.OnSequenceComplete.Visit(this);
        matchSequenceOccurenceConfig.OnSequenceAbort.Visit(this);
        matchSequenceOccurenceConfig.OnSequenceTimeout.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if(matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.TriggeredWhenEntry?.Visit(this);
        matchSeqTriggerConfig.OnTriggerExtract?.Visit(this);
        matchSeqTriggerConfig.AbortWhenEntry?.Visit(this);
        matchSeqTriggerConfig.NextTriggerStep?.Visit(this);
        matchSeqTriggerConfig.CompletedWhenEntry?.Visit(this);
        matchSeqTriggerConfig.SequenceFinalTrigger?.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if(messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if(sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.SendMessage?.Visit(this);
        sequenceHandleActionConfig.SendToAppender?.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if(logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.DefaultLogEntryPool.Visit(this);
        logEntryPoolsInitConfig.GlobalLogEntryPool.Visit(this);
        logEntryPoolsInitConfig.LargeMessageLogEntryPool.Visit(this);
        logEntryPoolsInitConfig.VeryLargeMessageLogEntryPool.Visit(this);
        logEntryPoolsInitConfig.LoggersGlobalLogEntryPool.Visit(this);
        logEntryPoolsInitConfig.AppendersGlobalLogEntryPool.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogAsyncBufferingInitializationConfig asyncBufferingInitializationConfig)
    {
        if(asyncBufferingInitializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        if(initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.AsyncBufferingInit.Visit(this);
        initializationConfig.LogEntryPoolsInit.Visit(this);
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IAppendableInheritingAppendersLookupConfig inheritingFormatAppendersConfig)
    {
        if(inheritingFormatAppendersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        foreach (var formattingAppender in inheritingFormatAppendersConfig)
        {
            formattingAppender.Value.Visit(this);
        }
        return this;
    }

    public override FlogConfigVisitChildrenCollect<T, TCollect> Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if(fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();
}