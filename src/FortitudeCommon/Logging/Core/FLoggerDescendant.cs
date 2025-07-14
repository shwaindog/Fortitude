// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.LoggerVisitors;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerDescendant : IFLoggerCommon
{
    IFLoggerCommon Parent { get; }
}

public interface IMutableFLoggerDescendant : IFLoggerDescendant
{
    void HandleConfigUpdate(ConsolidatedLoggerConfig newLoggerState);
}

public class FLoggerDescendant : FLoggerBase, IMutableFLoggerDescendant
{
    public FLoggerDescendant(ConsolidatedLoggerConfig loggerConsolidatedConfig, IFLoggerCommon myParent) : base(loggerConsolidatedConfig)
    {
        Parent = myParent;
        FullName = Visit(new BaseToLeafCollectVisitor()).FullName;
    }

    public IFLoggerCommon Parent { get; }

    public override LoggerTreeType TreeType => LoggerTreeType.Descendant;

    public void HandleConfigUpdate(ConsolidatedLoggerConfig newLoggerState)
    {
        if (FullName != newLoggerState.LoggerFullName)
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        LogLevel = newLoggerState.LogLevel;
        Appenders = newLoggerState.Appenders;
        if (newLoggerState.DeclaredConfigNodes[^1].FullName == FullName)
        {

        }
    }

    protected FLoggerRoot Root => Parent is FLoggerDescendant parentDescendant ? parentDescendant.Root : (FLoggerRoot)Parent;

    public override T Visit<T>(T visitor) 
    {
        return visitor.Accept(this);
    }
}
