// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
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
    void HandleConfigUpdateUpdateDescendants(IMutableFLoggerDescendantConfig newLoggerState, IFLogAppenderRegistry appenderRegistry);

    new IMutableFLoggerDescendantConfig ResolvedConfig { get; }

}

public class FLoggerDescendant : FLoggerBase, IMutableFLoggerDescendant
{
    public FLoggerDescendant(IMutableFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
        : base(loggerConsolidatedConfig, loggerRegistry)
    {
        Config = loggerConsolidatedConfig;

        Parent   = myParent;
        FullName = Visit(new BaseToLeafCollectVisitor()).FullName;
    }

    public IFLoggerCommon Parent { get; }

    public override IFLoggerDescendantConfig ResolvedConfig => (IFLoggerDescendantConfig)Config;

    IMutableFLoggerDescendantConfig IMutableFLoggerDescendant.ResolvedConfig => (IMutableFLoggerDescendantConfig)Config;

    public override LoggerTreeType TreeType => LoggerTreeType.Descendant;

    protected IMutableFLoggerRoot Root => Parent is FLoggerDescendant parentDescendant ? parentDescendant.Root : (IMutableFLoggerRoot)Parent;

    public override T Visit<T>(T visitor)
    {
        return visitor.Accept(this);
    }

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
