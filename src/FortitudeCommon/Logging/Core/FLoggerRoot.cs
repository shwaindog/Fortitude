// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerVisitors;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerRoot : IFLoggerCommon
{
    new IFLoggerRootConfig ResolvedConfig { get; }
    IFLogger GetOrCreateLogger(string loggerFullName, IFLogContext flogContext);
}

public interface IMutableFLoggerRoot : IFLoggerRoot, IMutableFLoggerCommon
{
    new IMutableFLoggerRootConfig ResolvedConfig { get; }
    void HandleRootLoggerConfigUpdate(IMutableFLoggerRootConfig newRootLoggerState);
}

public class FLoggerRoot : FLoggerBase, IMutableFLoggerRoot
{
    private IFLogContext? currentContext;

    private IFLogLoggerRegistry loggerRegistry = null!;

    static FLoggerRoot() => ImmortalInstance ??= new FLoggerRoot();

    private FLoggerRoot()
    {
        Name     = null!;
        FullName = "";
    }

    public static FLoggerRoot ImmortalInstance { get; }

    internal static IFLogContext? CurrentContext => ImmortalInstance.currentContext;

    public override IFLoggerRootConfig ResolvedConfig => (IFLoggerRootConfig)Config;

    IMutableFLoggerRootConfig IMutableFLoggerRoot.ResolvedConfig => (IMutableFLoggerRootConfig)Config;

    public override LoggerTreeType TreeType => LoggerTreeType.Root;

    public void HandleRootLoggerConfigUpdate(IMutableFLoggerRootConfig newRootLoggerState)
    {
        if (FullName != newRootLoggerState.FullName)
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        var appenderRegistry = loggerRegistry.AppenderRegistry;
        if (ResolvedConfig.AreEquivalent(newRootLoggerState)) return;
        HandleConfigUpdate(newRootLoggerState, appenderRegistry);
        Config = newRootLoggerState;
        Visit(new UpdateEmbodiedChildrenLoggerConfig(newRootLoggerState.AllLoggers(), appenderRegistry));
    }

    public IFLogger GetOrCreateLogger(string loggerFullName, IFLogContext flogContext)
    {
        var sourceLogger = Visit(new SourceOrCreateLoggerVisitor(loggerFullName, flogContext, flogContext.ConfigRegistry.AppConfig.ConfigRootPath));
        if (sourceLogger.SourcedLogger == null) throw new ArgumentException($"Was not able to create a logger for {loggerFullName}");
        return sourceLogger.SourcedLogger;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    internal void InitializeFinalStepSetContext(IFLogContext flogContext)
    {
        currentContext?.AsyncRegistry.ShutdownAsyncQueues();
        currentContext                      = flogContext;
        FLogContext.NextInitializingContext = null;

        loggerRegistry = flogContext.LoggerRegistry;
        ReInitializeRoot((IMutableFLoggerRootConfig)flogContext.ConfigRegistry.AppConfig.RootLogger, loggerRegistry);

        Visit(new UpdateLoggerConfigVisitor(flogContext.ConfigRegistry.AppConfig.ConfigRootPath, flogContext));
    }
}
