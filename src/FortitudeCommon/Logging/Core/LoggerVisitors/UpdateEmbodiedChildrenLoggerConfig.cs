// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

internal class UpdateEmbodiedChildrenLoggerConfig(IMutableNamedChildLoggersLookupConfig explicitDefinedConfig, IFLogAppenderRegistry appenderRegistry)
    : LoggerVisitor<UpdateEmbodiedChildrenLoggerConfig>
{
    private IMutableFLoggerTreeCommonConfig? parentConfig;

    public override UpdateEmbodiedChildrenLoggerConfig Visit(IMutableFLoggerRoot node)
    {
        parentConfig = node.ResolvedConfig;
        foreach (var childLogger in node.ImmediateEmbodiedChildren) childLogger.Accept(Me);
        return this;
    }

    public override UpdateEmbodiedChildrenLoggerConfig Visit(IMutableFLoggerDescendant node)
    {
        parentConfig ??= node.ResolvedConfig;
        if (explicitDefinedConfig.TryGetValue(node.FullName, out var definedConfig))
        {
            var myParentConfig = parentConfig;
            parentConfig = definedConfig.CreateInheritedDescendantConfig(parentConfig);
            node.HandleConfigUpdate(parentConfig, appenderRegistry);
            foreach (var childLogger in node.ImmediateEmbodiedChildren) childLogger.Accept(Me);
            parentConfig = myParentConfig;
        }
        else
        {
            node.HandleConfigUpdate(parentConfig, appenderRegistry);
            foreach (var childLogger in node.ImmediateEmbodiedChildren) childLogger.Accept(Me);
        }
        return this;
    }
}
