using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class RootToLoggerCollectVisitor<T, TCollect>(List<TCollect> found, Predicate<TCollect>? meetsCondition = null) 
    : FLogConfigVisitor<T>, IEnumerable<TCollect> 
    where T : RootToLoggerCollectVisitor<T, TCollect>
    where TCollect : IMutableFLoggerTreeCommonConfig
{
    protected static readonly Predicate<TCollect> Always = _ => true;

    private Predicate<TCollect> meets = meetsCondition ?? Always;

    public List<TCollect> Loggers => found;

    public string FullName => Loggers.Select(dctn => dctn.Name).JoinToString(".");

    public RootToLoggerCollectVisitor(Predicate<TCollect> meetsCondition) : this(new List<TCollect>(), meetsCondition) { }

    public RootToLoggerCollectVisitor() : this(new List<TCollect>(), Always) { }

    public RootToLoggerCollectVisitor<T, TCollect> Initialize(Predicate<TCollect>? meetsCondition = null)
    {
        Loggers.Clear();
        meets = meetsCondition ?? Always;

        return this;
    }

    public IMutableFLoggerRootConfig? FoundRootLoggerConfig { get; private set; }

    public override T Accept(IMutableFLoggerTreeCommonConfig loggerCommonConfig)
    {
        switch (loggerCommonConfig)
        {
            case IMutableFLoggerRootConfig rootLogger:             return Accept(rootLogger);
            case IMutableFLoggerDescendantConfig descendantLogger: return Accept(descendantLogger);

            default: return Me;
        }
    }

    public override T Accept(IMutableFLoggerRootConfig loggerRootConfig)
    {
        FoundRootLoggerConfig = loggerRootConfig;
        if(loggerRootConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        loggerDescendantConfig.ParentConfig?.Visit(Me);
        if(loggerDescendantConfig is TCollect toAdd && meets(toAdd)) found.Add(toAdd);
        return Me;
    }

    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig) =>
        childLoggersConfig.ParentConfig?.Visit(Me) ?? Me;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCollect> GetEnumerator() => found.GetEnumerator();

    public override void StateReset()
    {
        Loggers.Clear();

        base.StateReset();
    }
}