// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class SourceOrCreateLoggerVisitor
    (string loggerFullName, IFLogContext flogContext, string flogAppConfigPath) : LoggerVisitor<SourceOrCreateLoggerVisitor>
{
    private static readonly char[] NamePartDelimiter = ['.'];

    private readonly MutableString loggerNameScratch = new();

    public IFLogger? SourcedLogger { get; private set; }

    public override SourceOrCreateLoggerVisitor Accept(IMutableFLoggerRoot node)
    {
        foreach (var childLogger in node.ImmediateEmbodiedChildren)
            if (WalkDownTree(childLogger))
                return this;
        WalkDownTree(node);
        return this;
    }

    protected bool WalkDownTree(IMutableFLoggerCommon ancestorLogger)
    {
        var ancestorFullName = ancestorLogger.FullName;
        if (!loggerFullName.StartsWith(ancestorFullName)) return false;
        loggerNameScratch.Clear();
        loggerNameScratch.Append(loggerFullName);
        if (ancestorFullName.IsNotNullOrEmpty())
        {
            loggerNameScratch.Replace(ancestorFullName, "");
            loggerNameScratch.Remove(0);
        }
        var firstNamePart = loggerNameScratch.SplitFirstAsString(NamePartDelimiter);
        var isPathPart    = firstNamePart.Length < loggerNameScratch.Length;

        var foundChild = ancestorLogger.ImmediateEmbodiedChildren.FirstOrDefault(fld => fld.Name == firstNamePart);

        if (foundChild == null)
        {
            var subLoggerName =
                loggerNameScratch
                    .Clear().Append(ancestorLogger.FullName)
                    .Append(ancestorLogger.FullName.IsNotNullOrEmpty() ? "." : "").Append(firstNamePart).ToString();
            var configPathBuilder =
                loggerNameScratch
                    .Clear().Append(flogAppConfigPath).Append(ConfigSection.KeySeparator)
                    .Append(nameof(FLogAppConfig.RootLogger)).Append(ConfigSection.KeySeparator)
                    .Append(nameof(FLoggerTreeCommonConfig.DescendantLoggers)).Append(ConfigSection.KeySeparator);
            foreach (var ancestorGeneration in ancestorFullName.Split(".").Where(s => s.IsNotEmpty()))
                configPathBuilder
                    .Append(ancestorGeneration).Append(ConfigSection.KeySeparator)
                    .Append(nameof(FLoggerTreeCommonConfig.DescendantLoggers)).Append(ConfigSection.KeySeparator);
            var configPath = configPathBuilder.Append(firstNamePart).ToString();

            var definedConfig   = flogContext.ConfigRegistry.FindLoggerConfigIfGiven(subLoggerName);
            var explicitConfig  = definedConfig ?? ancestorLogger.ResolvedConfig;
            var subLoggerConfig = FLogCreate.MakeClonedDescendantLoggerConfig(explicitConfig, configPath);
            subLoggerConfig.Name = firstNamePart;
            ancestorLogger.ResolvedConfig.DescendantLoggers.Add(subLoggerConfig);
            var subLogger = FLogCreate.MakeLogger(subLoggerConfig, ancestorLogger, flogContext.LoggerRegistry);
            ancestorLogger.AddDirectChild(subLogger);
            if (isPathPart) return WalkDownTree(subLogger);
            SourcedLogger = subLogger;
            return true;
        }
        if (isPathPart)
        {
            WalkDownTree(foundChild);
        }
        else
        {
            SourcedLogger = foundChild;
            return true;
        }
        return false;
    }
}
