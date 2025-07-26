// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class AllAncestorLoggersOf : VisitAllLoggersCollectOnCriteria<AllAncestorLoggersOf>
{
    public AllAncestorLoggersOf(string loggerFullName) 
        : base(dlc => loggerFullName.Contains(dlc.FullName)) { }

    public AllAncestorLoggersOf() { }

    public AllAncestorLoggersOf(
        List<IMutableFLoggerDescendantConfig> found
      , Predicate<IMutableFLoggerDescendantConfig>? meetsCondition = null) 
        : base(found, meetsCondition) { }

    public AllAncestorLoggersOf Initialize(string loggerFullName)
    {
        base.Initialize((dlc) => loggerFullName.Contains(dlc.FullName));
        return this;
    }
}
