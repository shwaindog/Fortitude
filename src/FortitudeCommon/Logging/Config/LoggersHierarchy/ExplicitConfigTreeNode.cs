// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Hub;
using static FortitudeCommon.Logging.Config.Pooling.ISizeableItemPoolConfig;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public record ExplicitLogEntryPoolDefinition(PoolScope PoolScope, string PoolName, int NewItemCapacity, int ItemBatchSize)
{
    public ExplicitLogEntryPoolDefinition(ISizeableItemPoolConfig logEntryPoolCfg)
        : this(logEntryPoolCfg.PoolScope, logEntryPoolCfg.PoolName, logEntryPoolCfg.NewItemCapacity, logEntryPoolCfg.ItemBatchSize) { }
};

public abstract record ExplicitConfigTreeNode
(
    string Name
  , FLogLevel? DeclareLogLevel
  , INamedAppendersLookupConfig? DeclaredAppendersConfig
  , ExplicitLogEntryPoolDefinition? LogPoolDefinition = null)
{
    protected List<DescendantConfigTreeNode> Children = new();

    public virtual T Visit<T>(ConfigNodeVisitor<T> visitor) where T : ConfigNodeVisitor<T>
    {
        return visitor.Accept(this);
    }

    public virtual string FullName { get; } = "";

    public IReadOnlyList<DescendantConfigTreeNode> ChildNodes => Children;

    public abstract IAppenderRegistry AppenderRegistry { get; }

    // public IReadOnlyList<IFLoggerAppender> ResolvedAppenders =>
    //     DeclaredAppendersConfig
    //         .Select(ac => AppenderRegistry.AppenderFLogEntryPoolRegistry())

    internal DescendantConfigTreeNode AddDirectChild(DescendantConfigTreeNode newChild)
    {
        if (Children.Any(dctn => dctn.Name == newChild.Name))
        {
            throw new ArgumentException("Error same attempted to be added to the same parent in the hierarchy");
        }
        Children.Add(newChild);
        return newChild;
    }

    public bool TryGetValue(string immediateChildName, [NotNullWhen(true)] out DescendantConfigTreeNode? value)
    {
        value = ChildNodes.FirstOrDefault(dctn => dctn.Name == immediateChildName);
        return value != null;
    }
}

public record ExplicitRootConfigNode : ExplicitConfigTreeNode
{
    public ExplicitRootConfigNode(string Name, IFLoggerRootConfig DeclaredRootConfig, IAppenderRegistry AppenderRegistry)
        : base(Name
             , DeclaredRootConfig.LogLevel
             , DeclaredRootConfig?.Appenders
             , DeclaredRootConfig.LogEntryPool != null
                   ? new ExplicitLogEntryPoolDefinition(DeclaredRootConfig.LogEntryPool)
                   : new ExplicitLogEntryPoolDefinition(PoolScope.LoggersGlobal, LoggersGlobal, LogEntryDefaultStringCapacity, LogEntryBatchSize))
    {
        this.DeclaredRootConfig = DeclaredRootConfig;
        this.AppenderRegistry   = AppenderRegistry;
        Visit(new AddConfigNodeVisitor(DeclaredRootConfig));
    }


    public IFLoggerRootConfig DeclaredRootConfig { get; init; }

    public override IAppenderRegistry AppenderRegistry { get; }

    public override T Visit<T>(ConfigNodeVisitor<T> visitor)
    {
        return visitor.Accept(this);
    }

    public void Deconstruct(out string Name, out IFLoggerRootConfig DeclaredRootConfig, out IAppenderRegistry AppenderRegistry)
    {
        Name = this.Name;

        DeclaredRootConfig = this.DeclaredRootConfig;
        AppenderRegistry   = this.AppenderRegistry;
    }
}

public record DescendantConfigTreeNode
(
    string Name
  , ExplicitConfigTreeNode Parent
  , bool? Inherits = null
  , IFLoggerDescendantConfig? DeclaredConfig = null
  , ExplicitLogEntryPoolDefinition? LogPoolDefinition = null)
    : ExplicitConfigTreeNode(Name
                           , DeclaredConfig?.LogLevel
                           , DeclaredConfig?.Appenders
                           , DeclaredConfig?.LogEntryPool != null
                                 ? new ExplicitLogEntryPoolDefinition(DeclaredConfig.LogEntryPool)
                                 : LogPoolDefinition)
{
    private IAppenderRegistry? appenderRegistry;

    public override string FullName => Visit(new BaseToLeafVisitor()).FullName;

    public override IAppenderRegistry AppenderRegistry => appenderRegistry ??= RootConfigNode.AppenderRegistry;

    public ExplicitRootConfigNode RootConfigNode =>
        Parent is DescendantConfigTreeNode parentDescendant ? parentDescendant.RootConfigNode : (ExplicitRootConfigNode)Parent;

    public override T Visit<T>(ConfigNodeVisitor<T> visitor)
    {
        return visitor.Accept(this);
    }
}
