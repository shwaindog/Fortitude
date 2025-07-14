using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

internal class CrystallisedAppenderConfig(string AppenderName, string AppenderConfigRef)
{
    // protected List<CrystallisedAppenderConfig> ForwardToAppenders = new();
    //
    // public virtual T Visit<T>(ConfigNodeVisitor<T> visitor) where T : ConfigNodeVisitor<T>
    // {
    //     return visitor.Accept(this);
    // }
    //
    // public virtual string FullName { get; } = "";
    //
    // public IReadOnlyList<DescendantConfigTreeNode> ChildNodes => Children;
    //
    // internal DescendantConfigTreeNode AddDirectChild(DescendantConfigTreeNode newChild)
    // {
    //     if (Children.Any(dctn => dctn.Name == newChild.Name))
    //     {
    //         throw new ArgumentException("Error same attempted to be added to the same parent in the hierarchy");
    //     }
    //     Children.Add(newChild);
    //     return newChild;
    // }
    //
    // public bool TryGetValue(string immediateChildName, [NotNullWhen(true)] out DescendantConfigTreeNode? value)
    // {
    //     value = ChildNodes.FirstOrDefault(dctn => dctn.Name == immediateChildName);
    //     return value != null;
    // }
}