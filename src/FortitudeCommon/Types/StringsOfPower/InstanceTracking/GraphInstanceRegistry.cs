// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public class GraphInstanceRegistry(ITheOneString master) : IList<GraphNodeVisit>
{
    protected readonly List<GraphNodeVisit> OrderedObjectGraph = new(64);

    protected int NextObjVisitedRefId   = 1;
    public int CurrentGraphNodeIndex { get; set; } = -1;

    public GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;
    
    public int CurrentDepth => CurrentNode?.GraphDepth ?? -1;
    
    public int RemainingDepth => (CurrentNode?.RemainingGraphDepth ?? master.Settings.DefaultGraphMaxDepth);
    
    public void ClearObjectVisitedGraph()
    {
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            graphNodeVisit.Reset();
            OrderedObjectGraph[i] = new GraphNodeVisit();
        }
        OrderedObjectGraph.Clear();
        NextObjVisitedRefId   = 1;
        CurrentGraphNodeIndex = -1;
    }
    
    public bool UseReferenceEqualsForVisited { get; set; }
    
    public bool UseEqualsForVisited
    {
        get => !UseReferenceEqualsForVisited;
        set => UseReferenceEqualsForVisited = !value;
    }
    
    public int NextRefId() => NextObjVisitedRefId++;
    
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
        if (checkVisit.CurrentFormatFlags.HasNoRevisitCheck()) return false;
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
    
    public VisitResult SourceGraphVisitRefId<T>(T toStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyle == null || formatFlags.HasNoRevisitCheck()) return VisitResult.Empty;
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

    private VisitResult SourceGraphVisitRefId(object objToStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || formatFlags.HasNoRevisitCheck()) return VisitResult.Empty;
        var foundRefId         = 0;
        var newlyAssigned      = false;
        var isBaseOfFound      = false;
        var firstInstanceIndex = -1;
        var lastInstanceIndex  = -1;
        var repeatCount        = -1;
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
            {
                if (WasVisitOnSameOrMoreDerivedType(type, graphNodeVisit))
                {
                    foundRefId    = graphNodeVisit.RefId;
                    isBaseOfFound = WasVisitAsBaseTypeOrHigher(type, graphNodeVisit);
                    if (!isBaseOfFound && foundRefId == 0)
                    {
                        foundRefId            = NextRefId();
                        newlyAssigned         = true;
                        OrderedObjectGraph[i] = graphNodeVisit.SetRefId(foundRefId);
                    }
                    else { repeatCount = 0; }
                    firstInstanceIndex = i;
                    break;
                }
            }
        }
        if (foundRefId > 0)
        {
            for (var i = OrderedObjectGraph.Count - 1; i >= 0; i--)
            {
                var graphNodeVisit = OrderedObjectGraph[i];
                if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
                {
                    if (WasVisitOnSameOrBaseType(type, graphNodeVisit))
                    {
                        lastInstanceIndex = i;
                        repeatCount       = graphNodeVisit.RevisitCount;
                        break;
                    }
                }
            }
        }
        
        return new VisitResult( OrderedObjectGraph.Count, foundRefId, newlyAssigned
                             , firstInstanceIndex, lastInstanceIndex, repeatCount, isBaseOfFound);
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
}
