using System.Collections;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class VisitAllLoggersCollectOnCriteria<T>(List<IMutableFLoggerDescendantConfig> found, Predicate<IMutableFLoggerDescendantConfig>? meetsCondition = null) 
    : FLogConfigVisitor<T>, IEnumerable<IMutableFLoggerDescendantConfig> 
    where T : VisitAllLoggersCollectOnCriteria<T>
{
    protected static readonly Predicate<IMutableFLoggerDescendantConfig> Always = _ => true;

    private Predicate<IMutableFLoggerDescendantConfig> meets = meetsCondition ?? Always;

    public List<IMutableFLoggerDescendantConfig> Loggers => found;

    public VisitAllLoggersCollectOnCriteria(Predicate<IMutableFLoggerDescendantConfig> meetsCondition) 
        : this(new List<IMutableFLoggerDescendantConfig>(), meetsCondition) { }

    public VisitAllLoggersCollectOnCriteria() : this(new List<IMutableFLoggerDescendantConfig>(), Always) { }

    public VisitAllLoggersCollectOnCriteria<T> Initialize(Predicate<IMutableFLoggerDescendantConfig>? meetsCondition = null)
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
        loggerRootConfig.DescendantLoggers.Visit(Me);
        return Me;
    }

    public override T Accept(IMutableFLoggerDescendantConfig loggerDescendantConfig)
    {
        if (FoundRootLoggerConfig == null)
        {
            return loggerDescendantConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        if(meets(loggerDescendantConfig)) found.Add(loggerDescendantConfig);
        loggerDescendantConfig.DescendantLoggers.Visit(Me);
        return Me;
    }
    
    public override T Accept(IMutableNamedChildLoggersLookupConfig childLoggersConfig)
    {
        if (FoundRootLoggerConfig == null)
        {
            return childLoggersConfig.ParentConfig?.Visit(Me) ?? Me;
        }
        foreach (var childLogger in childLoggersConfig)
        {
            return childLogger.Value.Visit(Me);
        }
        return Me;
    }


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableFLoggerDescendantConfig> GetEnumerator() => found.GetEnumerator();

    public override void StateReset()
    {
        Loggers.Clear();

        base.StateReset();
    }
}