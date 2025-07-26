// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerVisitors;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerRoot : IFLoggerCommon
{
    IFLogger GetOrCreateLogger(string loggerFullName, IFLogContext flogContext);

    new IFLoggerRootConfig ResolvedConfig { get; }
}

public interface IMutableFLoggerRoot : IFLoggerRoot, IMutableFLoggerCommon
{
    void HandleRootLoggerConfigUpdate(IFLoggerRootConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry);
}

public class FLoggerRoot : FLoggerBase, IMutableFLoggerRoot
{
    private readonly IFLogLoggerRegistry   loggerRegistry;

    public FLoggerRoot(IFLoggerRootConfig rootLoggerConfig, IFLogLoggerRegistry loggerRegistry) 
        : base(rootLoggerConfig, loggerRegistry)
    {
        ResolvedConfig = rootLoggerConfig;

        this.loggerRegistry = loggerRegistry;

        ResolvedConfig = rootLoggerConfig;
        FullName       = Name;
        Name           = "";

        // rootConfig          = ((ExplicitRootConfigNode)rootLoggerConfig.DeclaredConfigNodes.First()).DeclaredRootConfig;
    }

    public new IFLoggerRootConfig ResolvedConfig { get; protected set; }

    public override LoggerTreeType TreeType => LoggerTreeType.Root;

    public void HandleRootLoggerConfigUpdate(IFLoggerRootConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry)
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
        Visit(new UpdateEmbodiedChildrenLoggerConfig(newRootLoggerState.AllLoggers(), appenderRegistry));
    }
    
    public IFLogger GetOrCreateLogger(string loggerFullName, IFLogContext flogContext)
    {
        var sourceLogger = Visit(new SourceOrCreateLoggerVisitor(loggerFullName, flogContext));
        if (sourceLogger.SourcedLogger == null)
        {
            throw new ArgumentException($"Was not able to create a logger for {loggerFullName}");
        }
        return sourceLogger.SourcedLogger;
    }

    public override T Visit<T>(T visitor) 
    {
        return visitor.Accept(this);
    }
}
