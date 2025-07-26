// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;

public class ConfigAncestorsOfLogger : RootToLoggerCollectVisitor<ConfigAncestorsOfLogger, IMutableFLoggerDescendantConfig>
{
    public ConfigAncestorsOfLogger(Predicate<IMutableFLoggerDescendantConfig> meetsCondition) : base(meetsCondition) { }
    public ConfigAncestorsOfLogger() { }
}
