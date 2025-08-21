using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class UpdateLoggerConfigVisitor(string flogAppConfigPath, IFLogContext flogContext) : LoggerVisitor<UpdateLoggerConfigVisitor>
{
    private readonly MutableString loggerNameScratch = new();

    public override UpdateLoggerConfigVisitor Accept(IMutableFLoggerRoot node)
    {
        foreach (var childLogger in node.ImmediateEmbodiedChildren)
        {
            WalkDownTree(childLogger, node);
        }
        return this;
    }

    protected void WalkDownTree(IMutableFLoggerDescendant toUpdate, IMutableFLoggerCommon ancestorLogger)
    {
        var configPathBuilder =
            loggerNameScratch
                .Clear().Append(flogAppConfigPath).Append(ConfigSection.KeySeparator)
                .Append(nameof(FLogAppConfig.RootLogger)).Append(ConfigSection.KeySeparator)
                .Append(nameof(FLoggerTreeCommonConfig.DescendantLoggers)).Append(ConfigSection.KeySeparator);
        foreach (var ancestorGeneration in ancestorLogger.FullName.Split(".").Where(s => s.IsNotEmpty()))
        {
            configPathBuilder
                .Append(ancestorGeneration).Append(ConfigSection.KeySeparator)
                .Append(nameof(FLoggerTreeCommonConfig.DescendantLoggers)).Append(ConfigSection.KeySeparator);
        }
        var configPath      = configPathBuilder.Append(toUpdate.Name).ToString();
        var definedConfig   = flogContext.ConfigRegistry.FindLoggerConfigIfGiven(toUpdate.FullName);
        var explicitConfig  = definedConfig ?? ancestorLogger.ResolvedConfig;
        var subLoggerConfig = FLogCreate.MakeClonedDescendantLoggerConfig(explicitConfig, configPath);
        subLoggerConfig.Name = toUpdate.FullName;
        toUpdate.HandleConfigUpdate(subLoggerConfig, flogContext.AppenderRegistry);
        foreach (var grandChildLogger in toUpdate.ImmediateEmbodiedChildren)
        {
            WalkDownTree(grandChildLogger, toUpdate);
        }
    }
}

