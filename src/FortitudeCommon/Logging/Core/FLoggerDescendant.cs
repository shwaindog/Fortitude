// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Visitor;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerVisitors;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerDescendant : IFLoggerCommon
{
    IFLoggerCommon Parent { get; }
}

public interface IMutableFLoggerDescendant : IFLoggerDescendant
{
    void HandleConfigUpdate(IFLoggerDescendantConfig newLoggerState, IFloggerAppenderRegistry appenderRegistry);
}

public class FLoggerDescendant : FLoggerBase, IMutableFLoggerDescendant
{
    public FLoggerDescendant(IFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent) : base(loggerConsolidatedConfig)
    {
        Parent = myParent;
        FullName = Visit(new BaseToLeafCollectVisitor()).FullName;
    }

    public IFLoggerCommon Parent { get; }

    public override LoggerTreeType TreeType => LoggerTreeType.Descendant;

    public void HandleConfigUpdate(IFLoggerDescendantConfig newLoggerState, IFloggerAppenderRegistry appenderRegistry)
    {
        if (FullName != newLoggerState.ResolveFullName())
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        LogLevel = newLoggerState.LogLevel;
        
        foreach (var appenderConfig in newLoggerState.Appenders)  
        {
            if (Appenders.All(a => a.AppenderName != appenderConfig.Key))
            {
                appenderRegistry.RegistryAppenderInterest((toAdd) => LoggerAppenders.Add(toAdd), appenderConfig.Key);
            }
        }
        for (var i = 0; i < Appenders.Count; i++)
        {
            var existingAppender = Appenders[i];
            if (!newLoggerState.Appenders.ContainsKey(existingAppender.AppenderName))
            {
                LoggerAppenders.RemoveAt(i);
                i--;
            }
        }
    }

    protected FLoggerRoot Root => Parent is FLoggerDescendant parentDescendant ? parentDescendant.Root : (FLoggerRoot)Parent;

    public override T Visit<T>(T visitor) 
    {
        return visitor.Accept(this);
    }
}
