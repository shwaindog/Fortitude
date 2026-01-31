// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public class GraphInstanceRegistry : RecyclableObject, IList<GraphNodeVisit>
{
    protected readonly List<GraphNodeVisit> OrderedObjectGraph = new(64);

    private ISecretStringOfPower master;

    public int RegistryId { get; private set; } = -1;

    public int ThisRegistryNextRefId { get; set; } = 1;

    public int CurrentGraphNodeIndex { get; set; } = -1;

    public GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;
    
    public int CurrentDepth => CurrentNode?.GraphDepth ?? -1;
    
    public int RemainingDepth => (CurrentNode?.RemainingGraphDepth ?? master.Settings.DefaultGraphMaxDepth);

    public GraphInstanceRegistry Initialize(ISecretStringOfPower masterOfTheString, int asStringEnterCount = -1)
    {
        master     = masterOfTheString;
        RegistryId = asStringEnterCount;

        return this;
    }

    public void ClearObjectVisitedGraph()
    {
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            graphNodeVisit.Reset();
            OrderedObjectGraph[i] = new GraphNodeVisit();
        }
        OrderedObjectGraph.Clear();
        ThisRegistryNextRefId   = 1;
        CurrentGraphNodeIndex = -1;
    }
    
    public bool UseReferenceEqualsForVisited { get; set; }
    
    public bool UseEqualsForVisited
    {
        get => !UseReferenceEqualsForVisited;
        set => UseReferenceEqualsForVisited = !value;
    }
    
    public int NextRefId() => master.GetNextVisitedReferenceId(RegistryId);
    
    public bool WasVisitOnSameOrBaseType(Type objAsType, GraphNodeVisit startToLast)
    {
       if (startToLast.VisitedAsType == objAsType || 
           (objAsType.IsAssignableTo(startToLast.VisitedAsType) 
           && !startToLast.VisitedAsType.IsAssignableTo(objAsType))) { return true; }
       return false;
    }
    
    public bool WasVisitAsBaseTypeOrHigher(Type objAsType, GraphNodeVisit startToLast)
    {
       if (startToLast.VisitedAsType != objAsType 
        && (!objAsType.IsAssignableTo(startToLast.VisitedAsType)
         && startToLast.VisitedAsType.IsAssignableTo(objAsType))) { return true; }
       return false;
    }

    public bool WasVisitOnSameInstance(object objToStyle, GraphNodeVisit checkVisit)
    {
        var checkRef       = checkVisit.VisitedInstance;
        var isSameInstance = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, objToStyle) : Equals(checkRef, objToStyle);
        return isSameInstance;
    }

    public bool WasVisitOnExactlySameType(Type objAsType, GraphNodeVisit checkVisit)
    {
        if (objAsType == checkVisit.VisitedAsType) { return true; }
        return false;
    }

    public bool WasVisitOnAMoreDerivedType(Type objAsType, GraphNodeVisit checkVisit)
    {
        if (objAsType != checkVisit.VisitedAsType && 
            (checkVisit.VisitedAsType.IsAssignableTo(objAsType) 
           && !objAsType.IsAssignableTo(checkVisit.VisitedAsType))) { return true; }
        return false;
    }

    public bool WasVisitOnSameOrMoreDerivedType(Type objAsType, GraphNodeVisit checkVisit)
    {
        if (objAsType == checkVisit.VisitedAsType 
         || (checkVisit.VisitedAsType.IsAssignableTo(objAsType)
            && !objAsType.IsAssignableTo(checkVisit.VisitedAsType))) { return true; }
        return false;
    }

    public int InstanceIdAtVisit(int visitIndex)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return -1;
        return OrderedObjectGraph[visitIndex].RefId;
    }
    
    public VisitResult SourceGraphVisitRefId<T>(T toStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyle == null) return VisitResult.VisitNotChecked;
        return SourceGraphVisitRefId((object)toStyle, type, formatFlags);
    }

    public void UpdateVisitWriteMethod(int visitIndex, WriteMethodType newWriteMethod)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitWriteType(newWriteMethod);
    }

    public void UpdateVisitAddFormatFlags(int visitIndex, FormatFlags flagsToAdd)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitAddFormatFlags(flagsToAdd);
    }

    public void UpdateVisitRemoveFormatFlags(int visitIndex, FormatFlags flagsToRemove)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitRemoveFormatFlags(flagsToRemove);
    }

    public void UpdateVisitEncoders(int visitIndex, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitEncoders(contentEncoder, layoutEncoder);
    }

    public void RemoveVisitAt(int visitIndex)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph.RemoveAt(visitIndex);
    }

    private VisitResult SourceGraphVisitRefId(object objToStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType) return VisitResult.VisitNotChecked;
        var foundRefId              = 0;
        var isBaseOfFound           = false;
        var firstInstanceIndex      = -1;
        var firstMatchInstanceIndex = -1;
        var lastInstanceIndex       = -1;
        var thisVisitRepeatCount   = 0;
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
            {
                if (WasVisitOnSameOrMoreDerivedType(type, graphNodeVisit))
                {
                    foundRefId    = graphNodeVisit.RefId;
                    isBaseOfFound = WasVisitAsBaseTypeOrHigher(type, graphNodeVisit);
                    if (!isBaseOfFound 
                     && foundRefId == 0 
                     && !graphNodeVisit.CurrentFormatFlags.HasNoRevisitCheck()
                     && !formatFlags.HasNoRevisitCheck() )
                    {
                        foundRefId            = NextRefId();
                        OrderedObjectGraph[i] = graphNodeVisit.SetRefId(foundRefId);
                        thisVisitRepeatCount  = 0;
                    }
                    if (firstInstanceIndex < 0)
                    {
                        firstInstanceIndex = i;
                    }
                    if (!graphNodeVisit.CurrentFormatFlags.HasNoRevisitCheck() && !formatFlags.HasNoRevisitCheck())
                    {
                        firstMatchInstanceIndex = i;
                        break;
                    }
                }
            }
        }
        for (var i = OrderedObjectGraph.Count - 1; i >= 0; i--)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
            {
                if (WasVisitOnSameOrBaseType(type, graphNodeVisit))
                {
                    lastInstanceIndex = i;
                    thisVisitRepeatCount       = graphNodeVisit.RevisitCount;
                    break;
                }
            }
        }
        
        return new VisitResult( RegistryId, OrderedObjectGraph.Count, foundRefId
                             , firstInstanceIndex, firstMatchInstanceIndex, lastInstanceIndex, thisVisitRepeatCount, isBaseOfFound);
    }

    IEnumerator IEnumerable.           GetEnumerator() => GetEnumerator();

    public IEnumerator<GraphNodeVisit> GetEnumerator() => OrderedObjectGraph.GetEnumerator();

    public void Add(GraphNodeVisit item)
    {
        OrderedObjectGraph.Add(item);
    }
    
    public void Clear()
    {
        ClearObjectVisitedGraph();
    }
    
    public bool Contains(GraphNodeVisit item) => OrderedObjectGraph.Contains(item);

    public void CopyTo(GraphNodeVisit[] array, int arrayIndex)
    {
        for (var i = 0; i < OrderedObjectGraph.Count && i + arrayIndex < array.Length; i++)
        {
            array[arrayIndex + i] = OrderedObjectGraph[i];
        }
    }
    
    public bool Remove(GraphNodeVisit item) => OrderedObjectGraph.Remove(item);

    public int Count => OrderedObjectGraph.Count;
    
    public bool IsReadOnly => false;

    public int  IndexOf(GraphNodeVisit item) => OrderedObjectGraph.IndexOf(item);

    public void Insert(int index, GraphNodeVisit item)
    {
        OrderedObjectGraph.Insert(index,  item);
    }
    
    public void RemoveAt(int index)
    {
        OrderedObjectGraph.RemoveAt(index);
    }

    public GraphNodeVisit this[int index]
    {
        get => OrderedObjectGraph[index];
        set => OrderedObjectGraph[index] = value;
    }

    public override void StateReset()
    {
        ClearObjectVisitedGraph();
        base.StateReset();
    }
}
