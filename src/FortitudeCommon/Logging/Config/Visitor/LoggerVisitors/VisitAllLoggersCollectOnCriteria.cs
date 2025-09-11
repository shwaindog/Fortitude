// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class VisitAllLoggersCollectOnCriteria<T>
    (List<IMutableFLoggerDescendantConfig> found, Predicate<IMutableFLoggerDescendantConfig>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<IMutableFLoggerDescendantConfig>
    where T : VisitAllLoggersCollectOnCriteria<T>
{
    protected static readonly Predicate<IMutableFLoggerDescendantConfig> Always = _ => true;

    private Predicate<IMutableFLoggerDescendantConfig> meets = meetsCondition ?? Always;

    public VisitAllLoggersCollectOnCriteria(Predicate<IMutableFLoggerDescendantConfig> meetsCondition)
        : this(new List<IMutableFLoggerDescendantConfig>(), meetsCondition) { }

    public VisitAllLoggersCollectOnCriteria() : this(new List<IMutableFLoggerDescendantConfig>(), Always) { }

    public VisitAllLoggersCollectOnCriteria<T> Initialize(Predicate<IMutableFLoggerDescendantConfig>? meetsCondition = null)
    {
        Loggers.Clear();
        meets = meetsCondition ?? Always;

        return this;
    }

    public List<IMutableFLoggerDescendantConfig> Loggers => found;

    public IMutableFLoggerRootConfig? FoundRootLoggerConfig { get; private set; }

    public override T Visit(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return Visit(rootLogger);
            case IMutableFLoggerDescendantConfig descendantLogger: return Visit(descendantLogger);

            default: return Me;
        }
    }

    public override T Visit(IMutableFLoggerRootConfig loggerRootConfig)
    {
        FoundRootLoggerConfig = loggerRootConfig;
        loggerRootConfig.DescendantLoggers.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (FoundRootLoggerConfig == null) return loggerDescendantConfig.ParentConfig?.Accept(Me) ?? Me;
        if (meets(loggerDescendantConfig)) found.Add(loggerDescendantConfig);
        loggerDescendantConfig.DescendantLoggers.Accept(Me);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (FoundRootLoggerConfig == null) return childLoggersConfig.ParentConfig?.Accept(Me) ?? Me;
        foreach (var childLogger in childLoggersConfig) childLogger.Value.Accept(Me);
        return Me;
    }

    public override void StateReset()
    {
        Loggers.Clear();

        base.StateReset();
    }


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableFLoggerDescendantConfig> GetEnumerator() => found.GetEnumerator();
}
