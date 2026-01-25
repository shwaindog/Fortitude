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
            int ObjVisitIndex
          , int ParentVisitIndex
          , Type ActualType
          , Type VisitedAsType
          , ITypeMolderDieCast? TypeBuilderComponentAccess  
          , WriteMethodType writeMethod
          , object? VisitedInstance
          , int CurrentBufferTypeStart
          , int IndentLevel  
          , CallerContext CallerContext  
          , FormattingState FormattingState
          , FormatFlags CurrentFormatFlags  
          , int RevisitCount = 0  
        )
        {
            this.ParentVisitIndex           = ParentVisitIndex;
            this.VisitedAsType              = VisitedAsType;
            this.ActualType                 = ActualType;
            this.ObjVisitIndex              = ObjVisitIndex;
            this.TypeBuilderComponentAccess = TypeBuilderComponentAccess;
            WriteMethod                     = writeMethod;
            this.VisitedInstance            = VisitedInstance;
            this.CurrentBufferTypeStart     = CurrentBufferTypeStart;
            this.IndentLevel                = IndentLevel;
            this.CallerContext.CopyFrom(CallerContext);
            
            this.FormattingState = FormattingState;
            
            this.RevisitCount    = RevisitCount;
        }

        public ITypeMolderDieCast? TypeBuilderComponentAccess { get; init; }

        public GraphNodeVisit SetRefId(int newRefId)
        {
            return this with
            {
                RefId = newRefId
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              // , WriteMethod = WriteMethod
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit SetBufferFirstFieldStart(int bufferFirstFieldStart, int indentLevel)
        {
            return this with
            {
               CurrentBufferExpectedFirstFieldStart = bufferFirstFieldStart
             , IndentLevel  = indentLevel
              // ,  RefId = RefId
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , WriteMethod = WriteMethod
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit MarkContentEndClearComponentAccess(int contentEndIndex, WriteMethodType writeMethod)
        {
            return this with
            {
                CurrentBufferTypeEnd = contentEndIndex
              , WriteMethod = writeMethod
              , TypeBuilderComponentAccess = null
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , RefId = RefId
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit UpdateVisitWriteType(WriteMethodType writeMethod)
        {
            return this with
            {
                WriteMethod = writeMethod
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , RefId = RefId
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit UpdateVisitAddFormatFlags(FormatFlags flagsToAdd)
        {
            return this with
            {
                CurrentFormatFlags = CurrentFormatFlags | flagsToAdd
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , RefId = RefId
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              // , WriteMethod = WriteMethod
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit UpdateVisitRemoveFormatFlags(FormatFlags flagsToRemove)
        {
            return this with
            {
                CurrentFormatFlags = CurrentFormatFlags & ~flagsToRemove
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , RefId = RefId
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , CurrentBufferTypeStart = CurrentBufferTypeStart
              // , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              // , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              // , WriteMethod = WriteMethod
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext
              // , FormattingState = FormattingState
              // , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit ShiftTypeBufferIndex(int amountToShift)
        {
            return this with
            {
                CurrentBufferTypeStart = CurrentBufferTypeStart + amountToShift
                , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart + amountToShift
                , CurrentBufferTypeEnd = CurrentBufferTypeEnd != -1 ? CurrentBufferTypeEnd + amountToShift : -1
              // , RefId = RefId
              // , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              // , WriteMethod = WriteMethod
              // , IndentLevel  = IndentLevel
              // , CallerContext = CallerContext  
              // , FormattingState = FormattingState  
              // , RevisitCount = RevisitCount  
            };
        }

        public int CurrentBufferTypeEnd { get; init; } = -1;

        public int CurrentBufferExpectedFirstFieldStart { get; init; }
        public int ObjVisitIndex { get; init; }
        public int ParentVisitIndex { get; init; }
        public Type VisitedAsType { get; init; }
        public Type ActualType { get; init; }
        public WriteMethodType WriteMethod { get; init; }
        public object? VisitedInstance { get; init; }
        public int CurrentBufferTypeStart { get; init; }
        public CallerContext CallerContext { get; init; }
        public FormattingState FormattingState { get; init; }
        
        public FormatFlags CurrentFormatFlags { get; init; }
        
        public int RevisitCount { get; init; }
        
        public int IndentLevel { get; init; }
        public int GraphDepth => FormattingState.GraphDepth;
        public int RemainingGraphDepth => FormattingState.RemainingGraphDepth;

        public IStyledTypeFormatting Formatter => FormattingState.Formatter;
        public IEncodingTransfer? GraphEncoder => FormattingState.GraphEncoder;
        public IEncodingTransfer? ParentEncoder => FormattingState.ParentEncoder;

        public void Reset()
        {
            if (VisitedInstance is IRecyclableStructContainer recyclableStructContainerObject)
            {
                recyclableStructContainerObject.DecrementRefCount();
            }
            CallerContext.Clear();
        }

        // ReSharper disable ParameterHidesMember
        // ReSharper disable InconsistentNaming
        public readonly void Deconstruct(out int ObjVisitIndex
              , out int ParentVisitIndex
              , out Type VisitedAsType
              , out Type ActualType
              , out WriteMethodType WriteMethod
              , out object? VisitedInstance
              , out int CurrentBufferTypeStart
              , out CallerContext CallerContext
              , out FormattingState FormattingState
            )
            // ReSharper restore ParameterHidesMember
            // ReSharper restore InconsistentNaming
        {
            ObjVisitIndex          = this.ObjVisitIndex;
            ParentVisitIndex       = this.ParentVisitIndex;
            VisitedAsType          = this.VisitedAsType;
            ActualType             = this.ActualType;
            WriteMethod            = this.WriteMethod;
            VisitedInstance        = this.VisitedInstance;
            CurrentBufferTypeStart = this.CurrentBufferTypeStart;
            CallerContext          = this.CallerContext;
            FormattingState        = this.FormattingState;
        }
    }