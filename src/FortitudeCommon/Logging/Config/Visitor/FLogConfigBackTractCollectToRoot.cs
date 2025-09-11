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

public class FLogConfigBackTractCollectToRoot<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<TCollect>
    where T : FLogConfigBackTractCollectToRoot<T, TCollect>
{
    private static readonly Predicate<TCollect> Always = _ => true;

    private readonly Predicate<TCollect> meets = meetsCondition ?? Always;

    public FLogConfigBackTractCollectToRoot(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public FLogConfigBackTractCollectToRoot() : this(new List<TCollect>(), Always) { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();

    public override T Visit(IMutableFLogAppConfig appConfig)
    {
        if (appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        if (configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        configSourcesLookup.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        if (loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerCommonConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig)
    {
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerRootConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        loggerDescendantConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        childLoggersConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogBuildTypeAndDeployEnvConfig fLogDeployEnvBuildTypeConfig)
    {
        if (fLogDeployEnvBuildTypeConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fLogDeployEnvBuildTypeConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        if (entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        entryPoolConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAppenderReferenceConfig appenderConfig)
    {
        if (appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appenderConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        if (consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        consoleAppenderConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        if (appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        appendersCollectionConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        if (forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        forwardingAppenderConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        if (bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        bufferingAppenderConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogEntryQueueConfig queueConfig)
    {
        if (queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        queueConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        if (containsStringMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        containsStringMatchConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        if (logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logLevelMatchConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        if (sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceKeysComparisonConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        if (matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorLookupConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        if (matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchOperatorExpressionConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        if (extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyExpressionConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        if (extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        extractKeyValuesLookupConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        if (matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSequenceOccurenceConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        if (matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        matchSeqTriggerConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        if (messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        messageTemplateConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        if (sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        sequenceHandleActionConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        if (logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        logEntryPoolsInitConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        if (asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesInitConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        if (asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueuesLookupConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        if (asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        asyncQueueConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogInitializationConfig initializationConfig)
    {
        if (initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        initializationConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        if (fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileConfigSourceConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFlushBufferConfig flushBufferConfig)
    {
        if (flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        flushBufferConfig.ParentConfig?.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFileAppenderConfig fileAppenderConfig)
    {
        if (fileAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        fileAppenderConfig.ParentConfig?.Accept(Me);
        return Me;
    }
}

public class FLogConfigBackTractCollectToRoot<TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) :
    FLogConfigBackTractCollectToRoot<FLogConfigBackTractCollectToRoot<TCollect>, TCollect>(found, meetsCondition);
