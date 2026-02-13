// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
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
          , ITypeMolderDieCast? MoldState  
          , WrittenAsFlags WrittenAs
          , object? VisitedInstance
          , int IndentLevel  
          , CallerContext CallerContext  
          , FormattingState FormattingState
          , FormatFlags CurrentFormatFlags
          , int TypeOpenBufferIndex  
          , int RevisitCount = 0
          , int FirstFieldBufferIndex = -1
          , int BufferLength = -1
          , bool HasInsertedInstanceId = false  
        )
        {
            this.NodeVisitId     = NodeVisitId;
            this.ParentVisitId   = ParentVisitId;
            this.VisitedAsType   = VisitedAsType;
            this.ActualType      = ActualType;
            this.MoldState       = MoldState;
            this.VisitedInstance = VisitedInstance;
            this.IndentLevel     = IndentLevel;
            this.CallerContext.CopyFrom(CallerContext);
            
            this.FormattingState       = FormattingState;
            this.CurrentFormatFlags    = CurrentFormatFlags;
            this.TypeOpenBufferIndex   = TypeOpenBufferIndex;
            this.FirstFieldBufferIndex = FirstFieldBufferIndex < 0 ? TypeOpenBufferIndex : FirstFieldBufferIndex;
            this.BufferLength          = BufferLength;
            this.WrittenAs             = WrittenAs;
            
            this.RevisitCount          = RevisitCount;
            this.HasInsertedInstanceId = HasInsertedInstanceId;
        }

        public ITypeMolderDieCast? MoldState { get; init; }

        public GraphNodeVisit SetRefId(int newRefId) => this with { RefId = newRefId };

        public GraphNodeVisit SetHasInsertedRefId(bool toValue) => this with { HasInsertedInstanceId = toValue };

        public GraphNodeVisit UpdateVisitWriteType(WrittenAsFlags writtenAs) => this with { WrittenAs = writtenAs };

        public GraphNodeVisit UpdateVisitAddFormatFlags(FormatFlags flagsToAdd) => this with { CurrentFormatFlags = CurrentFormatFlags | flagsToAdd };

        public GraphNodeVisit SetBufferFirstFieldStart(int bufferFirstFieldStart, int indentLevel) =>
            this with
            {
                FirstFieldBufferIndex = bufferFirstFieldStart
              , IndentLevel  = indentLevel
            };

        public GraphNodeVisit UpdateIndentLevel(int indentLevel) =>
            this with
            {
                IndentLevel  = indentLevel
            };

        public GraphNodeVisit MarkContentEndClearComponentAccess(int contentEndIndex, WrittenAsFlags writtenAsFlags) =>
            this with
            {
                BufferLength = contentEndIndex - TypeOpenBufferIndex
              , WrittenAs = writtenAsFlags
              , MoldState = null
            };

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

        public GraphNodeVisit UpdateVisitLength(int deltaLength) =>
            this with
            {
                BufferLength = BufferLength + deltaLength
            };

        public GraphNodeVisit ShiftTypeBufferIndex(int amountToShift) =>
            this with
            {
                TypeOpenBufferIndex = TypeOpenBufferIndex + amountToShift
              , FirstFieldBufferIndex = FirstFieldBufferIndex + amountToShift
            };

        public int BufferLength { get; init; } = 0;

        public int FirstFieldBufferIndex { get; init; }
        
        public VisitId NodeVisitId { get; init; }
        public VisitId ParentVisitId { get; init; }
        public Type VisitedAsType { get; init; }
        public Type ActualType { get; init; }
        public object? VisitedInstance { get; init; }
        public int TypeOpenBufferIndex { get; init; }
        public CallerContext CallerContext { get; init; }
        public FormattingState FormattingState { get; init; }
        
        public FormatFlags CurrentFormatFlags { get; init; }
        
        public int RevisitCount { get; init; }
        
        public int IndentLevel { get; init; }
        
        public bool HasInsertedInstanceId { get; init; }
        
        public WrittenAsFlags WrittenAs { get; init; }
        public int GraphDepth => FormattingState.GraphDepth;
        public int RemainingGraphDepth => FormattingState.RemainingGraphDepth;

        public IStyledTypeFormatting Formatter => FormattingState.Formatter;
        public IEncodingTransfer ContentEncoder => FormattingState.ContentEncoder;
        public IEncodingTransfer LayoutEncoder => FormattingState.LayoutEncoder;

        public override string ToString() => 
        $"GraphNodeVisit {{ MoldType: {MoldState?.Mold?.GetType().Name ?? "Cleared" } {nameof(NodeVisitId)}: {NodeVisitId}," +
        $" {nameof(ParentVisitId)}: {ParentVisitId}{nameof(VisitedAsType)}: {VisitedAsType.Name}:  {nameof(ActualType)}: {ActualType.Name}:    }}";

        public void Reset()
        {
            if (VisitedInstance is IRecyclableStructContainer recyclableStructContainerObject)
            {
                recyclableStructContainerObject.DecrementRefCount();
            }
            CallerContext.Clear();
        }
    }