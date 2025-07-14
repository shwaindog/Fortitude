// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core;

public interface IFLoggerRoot : IFLoggerCommon
{
    IFLoggerRootConfig ResolveLoggerConfig();

    IFLogger GetOrCreateLogger(string loggerFullName);
}

public class FLoggerRoot : FLoggerBase, IFLoggerRoot
{
    private readonly IFLoggerRegistry   loggerRegistry;
    private readonly IFLoggerRootConfig rootConfig;

    private ExplicitRootConfigNode loggerExplicitConfigTree;

    public FLoggerRoot(ConsolidatedLoggerConfig rootLoggerConfig, IFLoggerRegistry loggerRegistry) : base(rootLoggerConfig)
    {
        this.loggerRegistry = loggerRegistry;

        FullName            = Name;
        rootConfig          = ((ExplicitRootConfigNode)rootLoggerConfig.DeclaredConfigNodes.First()).DeclaredRootConfig;

        loggerExplicitConfigTree = rootConfig.CrystallisedDeclaredConfigTree();
    }

    public IFLoggerRootConfig ResolveLoggerConfig() => rootConfig;

    public override LoggerTreeType TreeType => LoggerTreeType.Root;
    
    public IFLogger GetOrCreateLogger(string loggerFullName)
    {
        // loggerExplicitConfigTree
        return null!;
    }


    public override T Visit<T>(T visitor) 
    {
        return visitor.Accept(this);
    }
}
