// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct GraphNodeVisit
{
    public int RefId { get; private init; }

    public bool IsValueType => ActualType.IsValueType;

    public GraphNodeVisit(
        VisitId NodeVisitId
      , VisitId ParentVisitId
      , Type ActualType
      , Type VisitedAsType
      , IMoldWriteState? MoldState
      , WrittenAsFlags WrittenAs
      , object? VisitedInstance
      , int IndentLevel
      , FormattingState FormattingState
      , FormatFlags CurrentFormatFlags
      , int TypeOpenBufferIndex
      , int RevisitCount = 0
      , int FirstFieldBufferIndex = -1
      , int BufferLength = -1
      , bool HasInsertedInstanceId = false
      , int NumberOfLines = 0
      , int ParentLineNumber = 0
      , TypeMoldFlags WrittenFlags = TypeMoldFlags.None  
    )
    {
        this.NodeVisitId     = NodeVisitId;
        this.ParentVisitId   = ParentVisitId;
        this.VisitedAsType   = VisitedAsType;
        this.ActualType      = ActualType;
        this.MoldState       = MoldState;
        this.VisitedInstance = VisitedInstance;
        this.IndentLevel     = IndentLevel;

        this.FormattingState    = FormattingState;
        this.CurrentFormatFlags = CurrentFormatFlags;

        this.TypeOpenBufferIndex   = TypeOpenBufferIndex;
        this.FirstFieldBufferIndex = FirstFieldBufferIndex < 0 ? TypeOpenBufferIndex : FirstFieldBufferIndex;

        this.BufferLength = BufferLength;
        this.WrittenAs    = WrittenAs;

        this.RevisitCount = RevisitCount;

        this.HasInsertedInstanceId = HasInsertedInstanceId;

        this.NumberOfLines = NumberOfLines;
        
        this.ParentLineNumber = ParentLineNumber;
        this.WrittenFlags     = WrittenFlags;
    }

    public IMoldWriteState? MoldState { get; init; }

    public GraphNodeVisit SetRefId(int newRefId) => this with { RefId = newRefId };

    public GraphNodeVisit SetHasInsertedRefId(bool toValue) => this with { HasInsertedInstanceId = toValue };

    public GraphNodeVisit UpdateVisitWriteType(WrittenAsFlags writtenAs) => this with { WrittenAs = writtenAs };

    public GraphNodeVisit UpdateParentVisitId(VisitId newParentVisitId) => this with { ParentVisitId = newParentVisitId };

    public GraphNodeVisit UpdateVisitReplaceFormatFlags(FormatFlags flagsToReplaceWith) => this with { CurrentFormatFlags = flagsToReplaceWith };

    public GraphNodeVisit UpdateVisitAddFormatFlags(FormatFlags flagsToAdd) => this with { CurrentFormatFlags = CurrentFormatFlags | flagsToAdd };

    public GraphNodeVisit SetBufferFirstFieldStartAndIndentLevel(int bufferFirstFieldStart, int indentLevel) =>
        this with
        {
            FirstFieldBufferIndex = bufferFirstFieldStart
          , IndentLevel = indentLevel
        };

    public GraphNodeVisit SetBufferFirstFieldStartAndWrittenAs(int bufferFirstFieldStart, WrittenAsFlags writtenAs) =>
        this with
        {
            FirstFieldBufferIndex = bufferFirstFieldStart
          , WrittenAs = writtenAs
        };

    public GraphNodeVisit UpdateIndentLevel(int indentLevel) =>
        this with
        {
            IndentLevel = indentLevel
        };

    public GraphNodeVisit MarkContentEndClearComponentAccess(int contentEndIndex, int numberOfLines = 0)
    {
        var bufferLength = contentEndIndex - TypeOpenBufferIndex;
        return this with
        {
            BufferLength = bufferLength
          , NumberOfLines = numberOfLines
          , MoldState = null
        };
    }

    public GraphNodeVisit UpdateVisitRemoveFormatFlags(FormatFlags flagsToRemove) =>
        this with
        {
            CurrentFormatFlags = CurrentFormatFlags & ~flagsToRemove
        };

    public GraphNodeVisit ReplaceObjectInstance(object? withMaybeSomething) =>
        this with
        {
            VisitedInstance = withMaybeSomething
        };

    public GraphNodeVisit UpdateVisitEncoders(IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder) =>
        this with
        {
            FormattingState = FormattingState.UpdateEncoders(contentEncoder, layoutEncoder)
        };

    public GraphNodeVisit UpdateVisitFormatter(IStyledTypeFormatting updatedFormatter)
    {
        return this with
        {
            FormattingState = FormattingState.UpdateFormatter(updatedFormatter)
        };
    }

    public GraphNodeVisit UpdateMoldWriteState(IMoldWriteState toSet) =>
        this with
        {
            MoldState = toSet
        };

    public GraphNodeVisit UpdateVisitLinesAndLength(int deltaLength, int addedNewLines)
    {
        var numberOfLines = NumberOfLines + addedNewLines;
        var bufferLength  = BufferLength + deltaLength;
        return this with
        {
            NumberOfLines = numberOfLines
          , BufferLength = bufferLength
        };
    }

    public GraphNodeVisit UpdateMoldWrittenFlags(TypeMoldFlags writtenFlags)
    {
        return this with
        {
            WrittenFlags = writtenFlags
        };
    }

    public GraphNodeVisit AddParentNewLines(int deltaParentNewLines) =>
        this with
        {
            ParentLineNumber = ParentLineNumber + deltaParentNewLines
        };

    public GraphNodeVisit ShiftTypeBufferIndex(int amountToShift, int deltaIndentLevel, int deltaWidthPerNewLine, int prefixInsertedChars = 0
      , int suffixInsertedChars = 0, int deltaNewLinesAdded = 0)
    {
        var totalInserted = prefixInsertedChars + suffixInsertedChars; 
        var bufferLength  = BufferLength + totalInserted;
        var totalLineCount = NumberOfLines;
        if (bufferLength >= 0 && deltaWidthPerNewLine > 0 && totalLineCount > 0)
        {
            bufferLength += (deltaIndentLevel * deltaWidthPerNewLine * totalLineCount);
        }
        totalLineCount = NumberOfLines + deltaNewLinesAdded;
        var newOpenIndex       = totalInserted == 0 ? TypeOpenBufferIndex + amountToShift : TypeOpenBufferIndex;
        var newFirstFieldIndex = totalInserted == 0 ? FirstFieldBufferIndex + amountToShift : FirstFieldBufferIndex + prefixInsertedChars;
        
        return this with
        {
            TypeOpenBufferIndex =  newOpenIndex
          , FirstFieldBufferIndex = newFirstFieldIndex
          , BufferLength  = bufferLength
          , NumberOfLines  = totalLineCount
          , IndentLevel  =  IndentLevel + deltaIndentLevel
        };
    }

    public int BufferLength { get; init; } = 0;

    public int FirstFieldBufferIndex { get; init; }

    public VisitId NodeVisitId { get; init; }
    public VisitId ParentVisitId { get; init; }
    public Type VisitedAsType { get; init; }
    public Type ActualType { get; init; }
    public object? VisitedInstance { get; init; }
    public int TypeOpenBufferIndex { get; init; }
    public FormattingState FormattingState { get; init; }

    public FormatFlags CurrentFormatFlags { get; init; }

    public int RevisitCount { get; init; }

    public int IndentLevel { get; init; }

    public bool HasInsertedInstanceId { get; init; }

    public int NumberOfLines { get; init; }
    
    public int ParentLineNumber { get; init; }

    public WrittenAsFlags WrittenAs { get; init; }
    
    public TypeMoldFlags WrittenFlags { get; init; }
    
    public int GraphDepth => FormattingState.GraphDepth;
    public int RemainingGraphDepth => FormattingState.RemainingGraphDepth;

    public IStyledTypeFormatting Formatter => FormattingState.Formatter;
    public IEncodingTransfer ContentEncoder => FormattingState.ContentEncoder;
    public IEncodingTransfer LayoutEncoder => FormattingState.LayoutEncoder;

    public override string ToString() =>
        $"GraphNodeVisit {{ MoldType: {MoldState?.Mold?.GetType().Name ?? "Cleared"} {nameof(NodeVisitId)}: {NodeVisitId}," +
        $" {nameof(ActualType)}: {ActualType.ShortNameInCSharpFormat()}, {nameof(ParentVisitId)}: {ParentVisitId}{nameof(VisitedAsType)}: {VisitedAsType.Name}," +
        $" {nameof(VisitedAsType)}: {VisitedAsType.ShortNameInCSharpFormat()}}}";

    public void Reset()
    {
        if (VisitedInstance is IRecyclableStructContainer recyclableStructContainerObject) { recyclableStructContainerObject.DecrementRefCount(); }
    }
}
