// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Visitor;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;


public interface IFLoggerVisitor { }

public interface IFLoggerVisitor<out T> : IFLoggerVisitor where T : IFLoggerVisitor<T>
{
    T Accept(IFLoggerCommon logger);
    T Accept(IFLoggerRoot logger);
    T Accept(IFLoggerDescendant logger);
}

public class LoggerVisitor<T> : RecyclableObject, IFLoggerVisitor<T> where T : IFLoggerVisitor<T>
{
    public virtual T Accept(IFLoggerCommon node)
    {
        switch (node)
        {
            case IFLoggerDescendant descendantConfig: Accept(descendantConfig); break;
            case IFLoggerRoot rootConfig:                   Accept(rootConfig); break;
        }
        return (T)(object)this;
    }

    public virtual T Accept(IFLoggerRoot node)
    {
        return VisitAllExisting(node);
    }

    public virtual T Accept(IFLoggerDescendant node)
    {
        return VisitAllExisting(node);
    }

    protected T VisitAllExisting(IFLoggerCommon node)
    {
        foreach (var descendantConfigTreeNode in node.ImmediateEmbodiedChildren)
        {
            descendantConfigTreeNode.Visit((T)(object)this);
        }
        return (T)(object)this;
    }
}


public class RootToLeafCollectVisitor : LoggerVisitor<RootToLeafCollectVisitor>
{
    private List<IFLoggerCommon> nodes = new ();

    public override RootToLeafCollectVisitor Accept(IFLoggerRoot node)
    {
        nodes.Add(node);
        return this;
    }

    public override RootToLeafCollectVisitor Accept(IFLoggerDescendant node)
    {
        node.Parent.Visit(this);
        nodes.Add(node);
        return this;
    }

    public IReadOnlyList<IFLoggerCommon> NodeSequence => nodes.AsReadOnly();
}

public class BaseToLeafCollectVisitor : LoggerVisitor<BaseToLeafCollectVisitor>
{
    private List<IFLoggerDescendant> nodes = new ();

    public override BaseToLeafCollectVisitor Accept(IFLoggerRoot node) => this;

    public override BaseToLeafCollectVisitor Accept(IFLoggerDescendant node)
    {
        node.Parent.Visit(this);
        nodes.Add(node);
        return this;
    }

    public IReadOnlyList<IFLoggerDescendant> NodeSequence => nodes.AsReadOnly();

    public string FullName => NodeSequence.Select(dctn => dctn.Name).JoinToString(".");
}

public class LeafToRootCollectVisitor : LoggerVisitor<LeafToRootCollectVisitor>
{
    private readonly List<IFLoggerCommon> nodes = new ();

    public override LeafToRootCollectVisitor Accept(IFLoggerRoot node)
    {
        nodes.Add(node);
        return this;
    }

    public override LeafToRootCollectVisitor Accept(IFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Visit(this);
        return this;
    }

    public IReadOnlyList<IFLoggerCommon> NodeSequence => nodes.AsReadOnly();
}

public class LeafToBaseCollectVisitor : LoggerVisitor<LeafToBaseCollectVisitor>
{
    private readonly List<IFLoggerDescendant> nodes = new ();

    public override LeafToBaseCollectVisitor Accept(IFLoggerRoot node) => this;

    public override LeafToBaseCollectVisitor Accept(IFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Visit(this);
        return this;
    }

    public IReadOnlyList<IFLoggerDescendant> NodeSequence => nodes.AsReadOnly();
}

public class SourceOrCreateLoggerVisitor(string loggerFullName,  IFLoggerLoggerRegistry fLoggerRegistry) : LoggerVisitor<SourceOrCreateLoggerVisitor>
{
    private IFLogger?     foundLogger = null;

    private MutableString loggerName  = new();


    public override SourceOrCreateLoggerVisitor Accept(IFLoggerRoot node)
    {
        foreach (var childLogger in node.ImmediateEmbodiedChildren)
        {
            // WalkDownTree((FLoggerBase)childLogger);
        }
        return this;
    }

    // protected bool WalkDownTree(FLoggerBase ancestorLogger)
    // {
    //     loggerName.Clear();
    //     loggerName.Append(loggerFullName).Replace(ancestorLogger.FullName, "");
    //     if (ancestorLogger.FullName.IsNotNullOrEmpty())
    //     {
    //         loggerName.Remove(0);
    //     }
    //     var firstNamePart = loggerName.SplitFirstAsString();
    //     var isPathPart    = firstNamePart.Length < loggerName.Length;
    //
    //     var foundChild = ancestorLogger.ImmediateEmbodiedChildren.FirstOrDefault(fld => fld.Name == firstNamePart);
    //     
    //     if (foundChild == null)
    //     {
    //         if (isPathPart)
    //         {
    //
    //             var nextChild = ancestorLogger.AddDirectChild(new FLogger(firstNamePart, parent));
    //             WalkDownTree(nextChild, addingConfig);
    //         }
    //         else
    //         {
    //             parent.AddDirectChild(new DescendantConfigTreeNode(firstNamePart, parent, addingConfig.Inherits, addingConfig));
    //         }
    //     }
    //     else
    //     {
    //         if (isPathPart)
    //         {
    //             WalkDownTree(foundChild, addingConfig);
    //         }
    //         else
    //         {
    //             foreach (var loggerChildrenKvp in addingConfig.DescendantLoggers)
    //             {
    //                 WalkDownTree(foundChild, loggerChildrenKvp.Value);
    //             }
    //         }
    //     }
    // }
           
    public override SourceOrCreateLoggerVisitor Accept(IFLoggerDescendant node) => this;
}