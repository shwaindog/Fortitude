// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class RootToLoggerCollectVisitor<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null)
    : FLogConfigVisitor<T>, IEnumerable<TCollect> where T : RootToLoggerCollectVisitor<T, TCollect> where TCollect : IMutableFLoggerTreeCommonConfig
{
    protected static readonly Predicate<TCollect> Always = _ => true;

    private Predicate<TCollect> meets = meetsCondition ?? Always;

    public RootToLoggerCollectVisitor(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public RootToLoggerCollectVisitor() : this(new List<TCollect>(), Always) { }

    public List<TCollect> Loggers => found;

    public string FullName => Loggers.Select(dctn => dctn.Name).JoinToString(".");

    public IMutableFLoggerRootConfig? FoundRootLoggerConfig { get; private set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();

    public RootToLoggerCollectVisitor<T, TCollect> Initialize(Predicate<TCollect>? meetsCondition = null)
    {
        Loggers.Clear();
        meets = meetsCondition ?? Always;

        return this;
    }

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
        if (loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        loggerDescendantConfig.ParentConfig?.Accept(Me);
        if (loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Visit(IMutableNamedChildLoggersLookupConfig childLoggersConfig) => childLoggersConfig.ParentConfig?.Accept(Me) ?? Me;

    public override void StateReset()
    {
        Loggers.Clear();

        base.StateReset();
    }
}
