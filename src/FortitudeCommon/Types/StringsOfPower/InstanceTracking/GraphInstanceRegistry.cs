// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public class GraphInstanceRegistry : RecyclableObject, IList<GraphNodeVisit>
{
    protected readonly List<GraphNodeVisit> OrderedObjectGraph = new(64);

    private ISecretStringOfPower master = null!;

    public sbyte RegistryId { get; private set; } = -1;

    public int ThisRegistryNextRefId { get; set; } = 1;

    public int CurrentGraphNodeIndex { get; set; } = -1;

    public VisitId CurrentGraphNodeVisitId => new (RegistryId, CurrentGraphNodeIndex);

    public int TryCurrentGraphNodeChecked(VisitId toSetTo)
    {
        if (toSetTo.RegistryId == VisitId.NoVisitCheckRequiredRegistryId) return 0;
        if (toSetTo.RegistryId != RegistryId)
        {
            Debugger.Break();
            return -2;
        }
        if (toSetTo is { RegistryId: -1, VisitIndex: -1 }) return -1;
        if (toSetTo.VisitIndex < 0 || toSetTo.VisitIndex >= OrderedObjectGraph.Count)
        {
            Debugger.Break();
            return -1;
        }
        CurrentGraphNodeIndex = toSetTo.VisitIndex;
        return 1;
    }

    public int NextFreeSlot => OrderedObjectGraph.Count;

    public GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;
    
    public int CurrentDepth => CurrentNode?.GraphDepth ?? -1;
    
    public int RemainingDepth => (CurrentNode?.RemainingGraphDepth ?? master.Settings.DefaultGraphMaxDepth);

    public GraphInstanceRegistry Initialize(ISecretStringOfPower masterOfTheString, sbyte asStringEnterCount = -1)
    {
        master     = masterOfTheString;
        RegistryId = asStringEnterCount;

        return this;
    }

    public bool HasRegistered(VisitId visitId)
    {
        return visitId.IsRegisterable && RegistryId == visitId.RegistryId && visitId.VisitIndex < OrderedObjectGraph.Count;
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
        ThisRegistryNextRefId = 1;
        CurrentGraphNodeIndex = -1;
        
    }
    
    public bool UseReferenceEqualsForVisited { get; set; }
    
    public bool UseEqualsForVisited
    {
        get => !UseReferenceEqualsForVisited;
        set => UseReferenceEqualsForVisited = !value;
    }
    
    public int NextRefId() => RegistryId == -1 
        ? ThisRegistryNextRefId++
        : master.GetNextVisitedReferenceId(RegistryId);
    
    public bool WasVisitOnSameOrBaseType(Type objAsType, GraphNodeVisit startToLast)
    {
       if (startToLast.VisitedAsType == objAsType || 
           (objAsType.IsAssignableTo(startToLast.VisitedAsType) 
           && !startToLast.VisitedAsType.IsAssignableTo(objAsType))) { return true; }
       return false;
    }
    
    public int WasVisitAsBaseTypeOrHigher(Type objAsType, GraphNodeVisit startToLast)
    {
        if (startToLast.VisitedAsType != objAsType
         && (!objAsType.IsAssignableTo(startToLast.VisitedAsType)
          && startToLast.VisitedAsType.IsAssignableTo(objAsType)))
        {
            return 1;
        }
        return 0;
    }

    public bool WasVisitOnSameInstance(object objToStyle, GraphNodeVisit checkVisit)
    {
        var checkRef           = checkVisit.VisitedInstance;
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

    public int TypeBufferStartIndexAtVisit(int visitIndex)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return -1;
        return OrderedObjectGraph[visitIndex].TypeOpenBufferIndex;
    }
    
    public VisitResult SourceGraphVisitRefId<T>(VisitId requesterVisitId, T toStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyle == null) return VisitResult.VisitNotChecked;
        return SourceGraphVisitRefId(requesterVisitId, (object)toStyle, type, formatFlags);
    }

    public void SetBufferFirstFieldStartAndIndentLevel(int visitIndex, int firstFieldStart, int indentLevel)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].SetBufferFirstFieldStartAndIndentLevel(firstFieldStart, indentLevel);
    }

    public void SetBufferFirstFieldStartAndWrittenAs(int visitIndex, int firstFieldStart, WrittenAsFlags writtenAs)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].SetBufferFirstFieldStartAndWrittenAs(firstFieldStart, writtenAs);
    }

    public void UpdateVisitWriteMethod(int visitIndex, WrittenAsFlags writeAs)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitWriteType(writeAs);
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

    public void UpdateVisitFormatter(int visitIndex, IStyledTypeFormatting updatedFormatter)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].UpdateVisitFormatter(updatedFormatter);
    }

    public void RemoveComparisonInstanceAtVisit(int visitIndex)
    {
        if (visitIndex >= OrderedObjectGraph.Count || visitIndex < 0) return;
        OrderedObjectGraph[visitIndex] = OrderedObjectGraph[visitIndex].ReplaceObjectInstance(null);
    }

    public int UpdateVisitLinesAndLength(VisitId visitId, int deltaLength, int addedNewLines)
    {
        if (visitId.VisitIndex >= OrderedObjectGraph.Count || visitId.VisitIndex < 0) return -1;
        var node = OrderedObjectGraph[visitId.VisitIndex];
        if(node.TypeOpenBufferIndex + node.BufferLength > master.WriteBuffer.Length ) Debugger.Break();
        OrderedObjectGraph[visitId.VisitIndex] = node.UpdateVisitLinesAndLength(deltaLength, addedNewLines);
        return node.BufferLength;
    }

    public void UpdateAddParentNewLines(VisitId visitId, int deltaParentNewLines)
    {
        if (visitId.VisitIndex >= OrderedObjectGraph.Count || visitId.VisitIndex < 0) return;
        OrderedObjectGraph[visitId.VisitIndex] = OrderedObjectGraph[visitId.VisitIndex].AddParentNewLines(deltaParentNewLines);
    }

    private VisitResult SourceGraphVisitRefId(VisitId requesterVisitId, object objToStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType) return VisitResult.VisitNotChecked;
        var foundRefId              = 0;
        var firstInstanceIndex      = -1;
        var firstMatchInstanceIndex = -1;
        var lastInstanceIndex       = -1;
        var thisVisitRepeatCount    = 0;
        var fwdIndex = 0;
        for (; fwdIndex < OrderedObjectGraph.Count; fwdIndex++)
        {
            var graphNodeVisit = OrderedObjectGraph[fwdIndex];
            if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
            {
                var nodeVisitable = !graphNodeVisit.CurrentFormatFlags.HasNoRevisitCheck();
                if (!nodeVisitable) continue;
                var bothVisitable = nodeVisitable&& !formatFlags.HasNoRevisitCheck();
                
                if (firstInstanceIndex < 0)
                {
                    firstInstanceIndex = fwdIndex;
                }
                foundRefId              = graphNodeVisit.RefId;
                if (foundRefId == 0 && bothVisitable )
                {
                    foundRefId                   = NextRefId();
                    OrderedObjectGraph[fwdIndex] = graphNodeVisit.SetRefId(foundRefId);
                    thisVisitRepeatCount         = 0;
                    firstMatchInstanceIndex      = fwdIndex;
                    break;
                }
                if (firstInstanceIndex < 0)
                {
                    firstInstanceIndex = fwdIndex;
                }
                if (foundRefId > 0 && bothVisitable)
                {
                    foundRefId              = graphNodeVisit.RefId;
                    thisVisitRepeatCount    = graphNodeVisit.RevisitCount + 1;
                    firstMatchInstanceIndex = fwdIndex;
                    break;
                }
            }
        }
        var bkwdIndex = (sbyte)(OrderedObjectGraph.Count - 1);
        for (; bkwdIndex >= fwdIndex; bkwdIndex--)
        {
            var graphNodeVisit = OrderedObjectGraph[bkwdIndex];
            if (WasVisitOnSameInstance(objToStyle, graphNodeVisit))
            {
                lastInstanceIndex = bkwdIndex;
                thisVisitRepeatCount       = graphNodeVisit.RevisitCount + 1;
                break;
            }
        }
        
        return new VisitResult( new VisitId( RegistryId, NextFreeSlot), requesterVisitId, foundRefId
                             , firstInstanceIndex, firstMatchInstanceIndex, lastInstanceIndex, thisVisitRepeatCount);
    }

    public VisitResult VisitNotChecked(VisitId requesterVisitId) => new ( new VisitId(VisitId.NoVisitCheckPerformedRegistryId, NextFreeSlot), requesterVisitId);
    
    public VisitResult VisitCheckNotRequired(VisitId requesterVisitId) => new ( new VisitId(VisitId.NoVisitCheckRequiredRegistryId, NextFreeSlot), requesterVisitId);

    IEnumerator IEnumerable.           GetEnumerator() => GetEnumerator();

    public IEnumerator<GraphNodeVisit> GetEnumerator() => OrderedObjectGraph.GetEnumerator();

    public void Add(GraphNodeVisit item)
    {
        // Console.Out.WriteLine("Registering " + item);
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
        set
        {
            if (value.TypeOpenBufferIndex + value.BufferLength > master.WriteBuffer.Length)
            {
                Console.Out.WriteLine($"VisitIndex: {index} TypeOpenBufferIndex : {value.TypeOpenBufferIndex}  BufferLength : {value.BufferLength} but Master.WriteBuffer.Length: {master.WriteBuffer.Length} ");
                // Debugger.Break();
            }
            OrderedObjectGraph[index] = value;
        }
    }

    public override void StateReset()
    {
        ClearObjectVisitedGraph();
        RegistryId                   = -1;
        master                       = null!;
        
        UseReferenceEqualsForVisited = false;
        base.StateReset();
    }
}
