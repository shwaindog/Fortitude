// // Licensed under the MIT license.
// // Copyright Alexis Sawenko 2025 all rights reserved
//
// using FortitudeCommon.DataStructures.Collections;
// using FortitudeCommon.Extensions;
// using FortitudeCommon.Logging.Config.LoggersHierarchy;
//
// namespace FortitudeCommon.Logging.Config.Visitor;
//
// public class ConfigNodeVisitor<T> where T : ConfigNodeVisitor<T>
// {
//     public virtual T Accept(ExplicitConfigTreeNode node)
//     {
//         switch (node)
//         {
//             case DescendantConfigTreeNode descendantConfig: Accept(descendantConfig); break;
//             case ExplicitRootConfigNode rootConfig:         Accept(rootConfig); break;
//         }
//         return (T)this;
//     }
//
//     public virtual T Accept(ExplicitConfigLoggerNode node)
//     {
//         switch (node)
//         {
//             case DescendantConfigTreeNode descendantConfig: Accept(descendantConfig); break;
//             case ExplicitRootConfigNode rootConfig:         Accept(rootConfig); break;
//         }
//         return (T)this;
//     }
//
//     public virtual T Accept(ExplicitRootConfigNode node)
//     {
//         return VisitAllExisting(node);
//     }
//
//     public virtual T Accept(DescendantConfigTreeNode node)
//     {
//         return VisitAllExisting(node);
//     }
//
//     protected T VisitAllExisting(ExplicitConfigLoggerNode node)
//     {
//         foreach (var descendantConfigTreeNode in node.ChildLoggers)
//         {
//             descendantConfigTreeNode.Visit(this);
//         }
//         return (T)this;
//     }
// }
//
//
// public class RootToLeafVisitor : ConfigNodeVisitor<RootToLeafVisitor>
// {
//     private List<ExplicitConfigTreeNode> nodes = new ();
//
//     public override RootToLeafVisitor Accept(ExplicitRootConfigNode node)
//     {
//         nodes.Add(node);
//         return this;
//     }
//
//     public override RootToLeafVisitor Accept(DescendantConfigTreeNode node)
//     {
//         node.Parent.Visit(this);
//         nodes.Add(node);
//         return this;
//     }
//
//     public IReadOnlyList<ExplicitConfigTreeNode> NodeSequence => nodes.AsReadOnly();
// }
//
// public class BaseToLeafVisitor : ConfigNodeVisitor<BaseToLeafVisitor>
// {
//     private List<DescendantConfigTreeNode> nodes = new ();
//
//     public override BaseToLeafVisitor Accept(ExplicitRootConfigNode node) => this;
//
//     public override BaseToLeafVisitor Accept(DescendantConfigTreeNode node)
//     {
//         node.Parent.Visit(this);
//         nodes.Add(node);
//         return this;
//     }
//
//     public IReadOnlyList<DescendantConfigTreeNode> NodeSequence => nodes.AsReadOnly();
//
//     public string FullName => NodeSequence.Select(dctn => dctn.Name).JoinToString(".");
// }
//
//
// public class AddConfigNodeVisitor(IFLoggerRootConfig rootConfig) : ConfigNodeVisitor<AddConfigNodeVisitor>
// {
//     public override AddConfigNodeVisitor Accept(ExplicitRootConfigNode node)
//     {
//         foreach (var loggerConfigKvp in rootConfig.DescendantLoggers)
//         {
//             WalkDownTree(node, loggerConfigKvp.Value);
//         }
//         return this;
//     }
//
//     protected void WalkDownTree(ExplicitConfigLoggerNode parent, IFLoggerDescendantConfig addingConfig)
//     {
//         var ancestorName  = parent.FullName.IsNotNullOrEmpty() ? parent.FullName + "." : "";
//         var remainingPath = addingConfig.Name.Replace(ancestorName, "");
//         var firstNamePart = remainingPath.Split(".").First();
//         var isPathPart    = firstNamePart.Length < remainingPath.Length;
//
//         if (!parent.TryGetValue(firstNamePart, out var foundChild))
//         {
//             if (isPathPart)
//             {
//                 var nextChild = parent.AddDirectChild(new DescendantConfigTreeNode(firstNamePart, parent));
//                 WalkDownTree(nextChild, addingConfig);
//             }
//             else
//             {
//                 parent.AddDirectChild(new DescendantConfigTreeNode(firstNamePart, parent, addingConfig.Inherits, addingConfig));
//             }
//         }
//         else
//         {
//             if (isPathPart)
//             {
//                 WalkDownTree(foundChild, addingConfig);
//             }
//             else
//             {
//                 foreach (var loggerChildrenKvp in addingConfig.DescendantLoggers)
//                 {
//                     WalkDownTree(foundChild, loggerChildrenKvp.Value);
//                 }
//             }
//         }
//     }
//
//     public override AddConfigNodeVisitor Accept(DescendantConfigTreeNode node) => this;
// }
//
//
// public class ResolveLastExplicitConfig(string loggerFullName) : ConfigNodeVisitor<ResolveLastExplicitConfig>
// {
//
//     public override ResolveLastExplicitConfig Accept(ExplicitRootConfigNode node)
//     {
//         // foreach (var loggerConfigKvp in node.ChildNodes)
//         // {
//         //     WalkDownTree(node, loggerConfigKvp.Value);
//         // }
//         return this;
//     }
//
//     protected void WalkDownTree(ExplicitConfigLoggerNode parent, IFLoggerDescendantConfig addingConfig)
//     {
//         var ancestorName  = parent.FullName.IsNotNullOrEmpty() ? parent.FullName + "." : "";
//         var remainingPath = addingConfig.Name.Replace(ancestorName, "");
//         var firstNamePart = remainingPath.Split(".").First();
//         var isPathPart    = firstNamePart.Length < remainingPath.Length;
//
//         if (!parent.TryGetValue(firstNamePart, out var foundChild))
//         {
//             if (isPathPart)
//             {
//                 var nextChild = parent.AddDirectChild(new DescendantConfigTreeNode(firstNamePart, parent));
//                 WalkDownTree(nextChild, addingConfig);
//             }
//             else
//             {
//                 parent.AddDirectChild(new DescendantConfigTreeNode(firstNamePart, parent, addingConfig.Inherits, addingConfig));
//             }
//         }
//         else
//         {
//             if (isPathPart)
//             {
//                 WalkDownTree(foundChild, addingConfig);
//             }
//             else
//             {
//                 foreach (var loggerChildrenKvp in addingConfig.DescendantLoggers)
//                 {
//                     WalkDownTree(foundChild, loggerChildrenKvp.Value);
//                 }
//             }
//         }
//     }
//
//     public override ResolveLastExplicitConfig Accept(DescendantConfigTreeNode node) => this;
// }