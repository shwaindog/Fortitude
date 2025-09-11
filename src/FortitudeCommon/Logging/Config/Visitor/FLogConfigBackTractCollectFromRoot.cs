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

    public override T Visit(IMutableFLogAppConfig appConfig)
    {
        if (appConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup)
    {
        configSourcesLookup.ParentConfig?.Accept(Me);
        if (configSourcesLookup is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        loggerCommonConfig.ParentConfig?.Accept(Me);
        if (loggerCommonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig)
    {
        loggerRootConfig.ParentConfig?.Accept(Me);
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        loggerDescendantConfig.ParentConfig?.Accept(Me);
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        childLoggersConfig.ParentConfig?.Accept(Me);
        if (childLoggersConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogBuildTypeAndDeployEnvConfig fLogDeployEnvBuildTypeConfig)
    {
        fLogDeployEnvBuildTypeConfig.ParentConfig?.Accept(Me);
        if (fLogDeployEnvBuildTypeConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogEntryPoolConfig entryPoolConfig)
    {
        entryPoolConfig.ParentConfig?.Accept(Me);
        if (entryPoolConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableAppenderReferenceConfig appenderConfig)
    {
        appenderConfig.ParentConfig?.Accept(Me);
        if (appenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableConsoleAppenderConfig consoleAppenderConfig)
    {
        consoleAppenderConfig.ParentConfig?.Accept(Me);
        if (consoleAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableNamedAppendersLookupConfig appendersCollectionConfig)
    {
        appendersCollectionConfig.ParentConfig?.Accept(Me);
        if (appendersCollectionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableForwardingAppenderConfig forwardingAppenderConfig)
    {
        forwardingAppenderConfig.ParentConfig?.Accept(Me);
        if (forwardingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableBufferingAppenderConfig bufferingAppenderConfig)
    {
        bufferingAppenderConfig.ParentConfig?.Accept(Me);
        if (bufferingAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogEntryQueueConfig queueConfig)
    {
        queueConfig.ParentConfig?.Accept(Me);
        if (queueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchEntryContainsStringConfig containsStringMatchConfig)
    {
        containsStringMatchConfig.ParentConfig?.Accept(Me);
        if (containsStringMatchConfig is TCollect toAdd) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchLogLevelConfig logLevelMatchConfig)
    {
        logLevelMatchConfig.ParentConfig?.Accept(Me);
        if (logLevelMatchConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceKeysComparisonConfig sequenceKeysComparisonConfig)
    {
        sequenceKeysComparisonConfig.ParentConfig?.Accept(Me);
        if (sequenceKeysComparisonConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableMatchOperatorLookupConfig matchOperatorLookupConfig)
    {
        matchOperatorLookupConfig.ParentConfig?.Accept(Me);
        if (matchOperatorLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchOperatorExpressionConfig matchOperatorExpressionConfig)
    {
        matchOperatorExpressionConfig.ParentConfig?.Accept(Me);
        if (matchOperatorExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableExtractKeyExpressionConfig extractKeyExpressionConfig)
    {
        extractKeyExpressionConfig.ParentConfig?.Accept(Me);
        if (extractKeyExpressionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableExtractedMessageKeyValuesConfig extractKeyValuesLookupConfig)
    {
        extractKeyValuesLookupConfig.ParentConfig?.Accept(Me);
        if (extractKeyValuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceOccurenceConfig matchSequenceOccurenceConfig)
    {
        matchSequenceOccurenceConfig.ParentConfig?.Accept(Me);
        if (matchSequenceOccurenceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableMatchSequenceTriggerConfig matchSeqTriggerConfig)
    {
        matchSeqTriggerConfig.ParentConfig?.Accept(Me);
        if (matchSeqTriggerConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableLogMessageTemplateConfig messageTemplateConfig)
    {
        messageTemplateConfig.ParentConfig?.Accept(Me);
        if (messageTemplateConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableSequenceHandleActionConfig sequenceHandleActionConfig)
    {
        sequenceHandleActionConfig.ParentConfig?.Accept(Me);
        if (sequenceHandleActionConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableLogEntryPoolsInitializationConfig logEntryPoolsInitConfig)
    {
        logEntryPoolsInitConfig.ParentConfig?.Accept(Me);
        if (logEntryPoolsInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableAsyncQueuesInitConfig asyncQueuesInitConfig)
    {
        asyncQueuesInitConfig.ParentConfig?.Accept(Me);
        if (asyncQueuesInitConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IAppendableAsyncQueueLookupConfig asyncQueuesLookupConfig)
    {
        asyncQueuesLookupConfig.ParentConfig?.Accept(Me);
        if (asyncQueuesLookupConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableAsyncQueueConfig asyncQueueConfig)
    {
        asyncQueueConfig.ParentConfig?.Accept(Me);
        if (asyncQueueConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogInitializationConfig initializationConfig)
    {
        initializationConfig.ParentConfig?.Accept(Me);
        if (initializationConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLogFileConfigSourceConfig fileConfigSourceConfig)
    {
        fileConfigSourceConfig.ParentConfig?.Accept(Me);
        if (fileConfigSourceConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFlushBufferConfig flushBufferConfig)
    {
        flushBufferConfig.ParentConfig?.Accept(Me);
        if (flushBufferConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFileAppenderConfig fileAppenderConfig)
    {
        fileAppenderConfig.ParentConfig?.Accept(Me);
        if (fileAppenderConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }
}
