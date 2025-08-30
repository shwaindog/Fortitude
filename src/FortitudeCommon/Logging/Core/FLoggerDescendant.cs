// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.ActivationProfiles;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerVisitors;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerDescendant : IFLoggerCommon
{
    IFLoggerCommon Parent { get; }

    new IFLoggerDescendantConfig ResolvedConfig { get; }
}

public interface IMutableFLoggerDescendant : IFLoggerDescendant, IMutableFLoggerCommon
{
    new IMutableFLoggerDescendantConfig ResolvedConfig { get; }
    void HandleConfigUpdateUpdateDescendants(IMutableFLoggerDescendantConfig newLoggerState, IFLogAppenderRegistry appenderRegistry);
}

public class FLoggerDescendant : FLoggerBase, IMutableFLoggerDescendant
{
    protected LoggerActivationFlags ConfigActivationProfile;

    protected LoggerActivationFlags CurrentFLoggerExecutionEnvironment;

    public FLoggerDescendant(IMutableFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
        : base(loggerConsolidatedConfig, loggerRegistry)
    {
        Config = loggerConsolidatedConfig;

        Parent   = myParent;
        FullName = Visit(new BaseToLeafCollectVisitor()).FullName;
    }

    protected override IMutableFLoggerTreeCommonConfig Config
    {
        get => base.Config;
        set
        {
            base.Config = value;

            CurrentFLoggerExecutionEnvironment = value.FLogEnvironment.AsLoggerActionFlags;
            ConfigActivationProfile = (value is IMutableFLoggerDescendantConfig descendantConfig
                ? descendantConfig.JustThisLoggerActivation?.AsLoggerActionFlags
                : null) ?? value.DescendantActivation.AsLoggerActionFlags;
        }
    }

    protected IMutableFLoggerRoot Root => Parent is FLoggerDescendant parentDescendant ? parentDescendant.Root : (IMutableFLoggerRoot)Parent;

    public IFLoggerCommon Parent { get; }

    public override IFLoggerDescendantConfig ResolvedConfig => (IFLoggerDescendantConfig)Config;

    IMutableFLoggerDescendantConfig IMutableFLoggerDescendant.ResolvedConfig => (IMutableFLoggerDescendantConfig)Config;

    public override LoggerTreeType TreeType => LoggerTreeType.Descendant;

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public void HandleConfigUpdateUpdateDescendants(IMutableFLoggerDescendantConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry)
    {
        if (FullName != newRootLoggerState.FullName)
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        if (ResolvedConfig.AreEquivalent(newRootLoggerState)) return;
        HandleConfigUpdate(newRootLoggerState, appenderRegistry);
        Config = newRootLoggerState;

        Visit(new UpdateEmbodiedChildrenLoggerConfig(Root.ResolvedConfig.AllLoggers(), appenderRegistry));
    }
}
