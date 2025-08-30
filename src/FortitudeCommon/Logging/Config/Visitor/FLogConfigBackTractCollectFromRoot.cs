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

public class FLogConfigBackTractCollectFromRoot<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<TCollect>
    where T : FLogConfigBackTractCollectFromRoot<T, TCollect>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;

    public FLogConfigBackTractCollectFromRoot(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public FLogConfigBackTractCollectFromRoot() : this(new List<TCollect>(), Always) { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();

    public override T Accept(IMutableFLogAppConfig appConfig)
    {
        if (appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        configSourcesLookup.ParentConfig?.Visit(Me);
        if (configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        loggerCommonConfig.ParentConfig?.Visit(Me);
        if (loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        loggerRootConfig.ParentConfig?.Visit(Me);
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        loggerDescendantConfig.ParentConfig?.Visit(Me);
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        childLoggersConfig.ParentConfig?.Visit(Me);
        if (childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableActivationProfileConfig activationProfileConfig)
    {
        activationProfileConfig.ParentConfig?.Visit(Me);
        if (activationProfileConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        entryPoolConfig.ParentConfig?.Visit(Me);
        if (entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableAppenderReferenceConfig appenderConfig)
    {
        appenderConfig.ParentConfig?.Visit(Me);
        if (appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        consoleAppenderConfig.ParentConfig?.Visit(Me);
        if (consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        appendersCollectionConfig.ParentConfig?.Visit(Me);
        if (appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        forwardingAppenderConfig.ParentConfig?.Visit(Me);
        if (forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        bufferingAppenderConfig.ParentConfig?.Visit(Me);
        if (bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLogEntryQueueConfig queueConfig)
    {
        queueConfig.ParentConfig?.Visit(Me);
        if (queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        containsStringMatchConfig.ParentConfig?.Visit(Me);
        if (containsStringMatchConfig is TCollect toAdd) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        logLevelMatchConfig.ParentConfig?.Visit(Me);
        if (logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        sequenceKeysComparisonConfig.ParentConfig?.Visit(Me);
        if (sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        matchOperatorLookupConfig.ParentConfig?.Visit(Me);
        if (matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        matchOperatorExpressionConfig.ParentConfig?.Visit(Me);
        if (matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        extractKeyExpressionConfig.ParentConfig?.Visit(Me);
        if (extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        extractKeyValuesLookupConfig.ParentConfig?.Visit(Me);
        if (extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        matchSequenceOccurenceConfig.ParentConfig?.Visit(Me);
        if (matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        matchSeqTriggerConfig.ParentConfig?.Visit(Me);
        if (matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        messageTemplateConfig.ParentConfig?.Visit(Me);
        if (messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        sequenceHandleActionConfig.ParentConfig?.Visit(Me);
        if (sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        logEntryPoolsInitConfig.ParentConfig?.Visit(Me);
        if (logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        asyncQueuesInitConfig.ParentConfig?.Visit(Me);
        if (asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        asyncQueuesLookupConfig.ParentConfig?.Visit(Me);
        if (asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        asyncQueueConfig.ParentConfig?.Visit(Me);
        if (asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLogInitializationConfig initializationConfig)
    {
        initializationConfig.ParentConfig?.Visit(Me);
        if (initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        fileConfigSourceConfig.ParentConfig?.Visit(Me);
        if (fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFlushBufferConfig flushBufferConfig)
    {
        flushBufferConfig.ParentConfig?.Visit(Me);
        if (flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFileAppenderConfig fileAppenderConfig)
    {
        fileAppenderConfig.ParentConfig?.Visit(Me);
        if (fileAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }
}
