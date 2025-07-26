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
    void HandleConfigUpdateUpdateDescendants(IFLoggerDescendantConfig newLoggerState, IFLogAppenderRegistry appenderRegistry);
}

public class FLoggerDescendant : FLoggerBase, IMutableFLoggerDescendant
{
    public FLoggerDescendant(IFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
        : base(loggerConsolidatedConfig, loggerRegistry)
    {
        ResolvedConfig = loggerConsolidatedConfig;

        Parent   = myParent;
        FullName = Visit(new BaseToLeafCollectVisitor()).FullName;
    }

    public IFLoggerCommon Parent { get; }

    public new IFLoggerDescendantConfig ResolvedConfig { get; protected set; }

    public override LoggerTreeType TreeType => LoggerTreeType.Descendant;

    protected FLoggerRoot Root => Parent is FLoggerDescendant parentDescendant ? parentDescendant.Root : (FLoggerRoot)Parent;

    public override T Visit<T>(T visitor)
    {
        return visitor.Accept(this);
    }

    public void HandleConfigUpdateUpdateDescendants(IFLoggerDescendantConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry)
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
        ResolvedConfig = newRootLoggerState;

        Visit(new UpdateEmbodiedChildrenLoggerConfig(Root.ResolvedConfig.AllLoggers(), appenderRegistry));
    }
}
