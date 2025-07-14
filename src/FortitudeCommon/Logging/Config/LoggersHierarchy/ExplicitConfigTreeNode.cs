// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Hub;
using static FortitudeCommon.Logging.Config.Pooling.IFLogEntryPoolConfig;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public record ExplicitLogEntryPoolDefinition(PoolScope PoolScope, string PoolName, int NewItemCapacity, int ItemBatchSize)
{
    public ExplicitLogEntryPoolDefinition(IFLogEntryPoolConfig ifLogEntryPoolCfg)
        : this(ifLogEntryPoolCfg.PoolScope, ifLogEntryPoolCfg.PoolName, ifLogEntryPoolCfg.NewItemCapacity, ifLogEntryPoolCfg.ItemBatchSize) { }
}

public enum NodeConfigType
{
    ConfigRoot
    , LoggerRoot
    , LoggerDescendant
    , AppenderRoot
    , AppenderReference
    , AppenderDefinition
}

public abstract record ExplicitConfigTreeNode(string Name , INamedAppendersLookupConfig? ChildAppenders)
{
    public virtual T Visit<T>(ConfigNodeVisitor<T> visitor) where T : ConfigNodeVisitor<T>
    {
        return visitor.Accept(this);
    }

    public abstract IAppenderRegistry AppenderRegistry { get; }

    // public IReadOnlyList<IFLoggerAppender> ResolvedAppenders =>
    //     DeclaredAppendersConfig
    //         .Select(ac => AppenderRegistry.AppenderFLogEntryPoolRegistry())
}

public abstract record ExplicitConfigRoot(string Name , INamedAppendersLookupConfig? ChildAppenders)
{


    // public IReadOnlyList<IFLoggerAppender> ResolvedAppenders =>
    //     DeclaredAppendersConfig
    //         .Select(ac => AppenderRegistry.AppenderFLogEntryPoolRegistry())
}



public abstract record ExplicitConfigLoggerNode 
(
    string Name
  , FLogLevel? DeclareLogLevel
  , INamedAppendersLookupConfig? ChildAppenders
  , ExplicitLogEntryPoolDefinition? LogPoolDefinition = null) : ExplicitConfigTreeNode(Name, ChildAppenders)
{
    internal List<DescendantConfigTreeNode> ChildLoggers = new();

    public virtual string FullName { get; } = "";

    // public IReadOnlyList<IFLoggerAppender> ResolvedAppenders =>
    //     DeclaredAppendersConfig
    //         .Select(ac => AppenderRegistry.AppenderFLogEntryPoolRegistry())

    internal DescendantConfigTreeNode AddDirectChild(DescendantConfigTreeNode newChild)
    {
        if (ChildLoggers.Any(dctn => dctn.Name == newChild.Name))
        {
            throw new ArgumentException("Error same attempted to be added to the same parent in the hierarchy");
        }
        ChildLoggers.Add(newChild);
        return newChild;
    }

    public bool TryGetValue(string immediateChildName, [NotNullWhen(true)] out DescendantConfigTreeNode? value)
    {
        value = ChildLoggers.FirstOrDefault(dctn => dctn.Name == immediateChildName);
        return value != null;
    }
}

public record ExplicitRootConfigNode : ExplicitConfigLoggerNode
{
    public ExplicitRootConfigNode(string Name, IFLoggerRootConfig DeclaredRootConfig, IAppenderRegistry AppenderRegistry)
        : base(Name
             , DeclaredRootConfig.LogLevel
             , DeclaredRootConfig?.Appenders
             , DeclaredRootConfig.LogEntryPool != null
                   ? new ExplicitLogEntryPoolDefinition(DeclaredRootConfig.LogEntryPool)
                   : new ExplicitLogEntryPoolDefinition(PoolScope.LoggersGlobal, LoggersGlobal, DefaultLogEntryStringCapacity, DefaultLogEntryBatchSize))
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
    : ExplicitConfigLoggerNode(Name
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
