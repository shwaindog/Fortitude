// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.WriteMethodType;

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

    public record struct GraphNodeVisit
    {
        public int RefId { get; private init; }

        public bool IsValueType;

        public GraphNodeVisit(int ObjVisitIndex
          , int ParentVisitIndex
          , Type VistedAsType
          , ITypeMolderDieCast? TypeBuilderComponentAccess  
          , WriteMethodType writeMethod
          , object? VisitedInstance
          , int CurrentBufferTypeStart
          , int IndentLevel  
          , CallerContext callerContext  
          , FormattingState FormattingState
          , int revisitCount = 0  
        )
        {
            this.ParentVisitIndex           = ParentVisitIndex;
            this.VistedAsType               = VistedAsType;
            IsValueType                     = VistedAsType.IsValueType;
            this.ObjVisitIndex              = ObjVisitIndex;
            this.TypeBuilderComponentAccess = TypeBuilderComponentAccess;
            WriteMethod                     = writeMethod;
            this.VisitedInstance            = VisitedInstance;
            this.CurrentBufferTypeStart     = CurrentBufferTypeStart;
            this.IndentLevel                = IndentLevel;
            CallerContext.CopyFrom(callerContext);
            
            this.FormattingState = FormattingState;
            
            this.RevisitCount    = revisitCount;
        }

        public ITypeMolderDieCast? TypeBuilderComponentAccess { get; init; }

        public GraphNodeVisit SetRefId(int newRefId)
        {
            return this with
            {
                RefId = newRefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              , WriteMethod = WriteMethod
              , IndentLevel  = IndentLevel
              , CallerContext = CallerContext
              , FormattingState = FormattingState
              , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit SetBufferFirstFieldStart(int bufferFirstFieldStart, int indentLevel)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferExpectedFirstFieldStart = bufferFirstFieldStart
              , WriteMethod = WriteMethod
              , IndentLevel  = indentLevel
              , CallerContext = CallerContext
              , FormattingState = FormattingState
              , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit MarkContentEndClearComponentAccess(int contentEndIndex, WriteMethodType writeMethod)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = null
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart
              , CurrentBufferTypeEnd = contentEndIndex
              , WriteMethod = writeMethod
              , IndentLevel  = IndentLevel
              , CallerContext = CallerContext
              , FormattingState = FormattingState
              , RevisitCount = RevisitCount
            };
        }

        public GraphNodeVisit ShiftTypeBufferIndex(int amountToShift)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart + amountToShift
              , CurrentBufferExpectedFirstFieldStart = CurrentBufferExpectedFirstFieldStart + amountToShift
              , CurrentBufferTypeEnd = CurrentBufferTypeEnd != -1 ? CurrentBufferTypeEnd + amountToShift : -1
              , WriteMethod = WriteMethod
              , IndentLevel  = IndentLevel
              , CallerContext = CallerContext  
              , FormattingState = FormattingState  
              , RevisitCount = RevisitCount  
            };
        }

        public int CurrentBufferTypeEnd { get; init; } = -1;

        public int CurrentBufferExpectedFirstFieldStart { get; init; }
        public int ObjVisitIndex { get; set; }
        public int ParentVisitIndex { get; set; }
        public Type VistedAsType { get; set; }
        public WriteMethodType WriteMethod { get; set; }
        public object? VisitedInstance { get; set; }
        public int CurrentBufferTypeStart { get; set; }
        public CallerContext CallerContext { get; set; }
        public FormattingState FormattingState { get; set; }
        
        public int RevisitCount { get; set; }
        
        public int IndentLevel { get; set; }
        public int GraphDepth => FormattingState.GraphDepth;
        public int RemainingGraphDepth => FormattingState.RemainingGraphDepth;
        public FormatFlags CreateWithFlags => FormattingState.CreateWithFlags;

        public IStyledTypeFormatting? Formatter => FormattingState.Formatter;
        public IEncodingTransfer? GraphEncoder => FormattingState.GraphEncoder;
        public IEncodingTransfer? ParentEncoder => FormattingState.ParentEncoder;

        public void Reset()
        {
            ObjVisitIndex    = 0;
            ParentVisitIndex = 0;
            VistedAsType     = typeof(GraphNodeVisit);
            WriteMethod      = None;
            if (VisitedInstance is IRecyclableStructContainer recyclableStructContainerObject)
            {
                recyclableStructContainerObject.DecrementRefCount();
            }
            VisitedInstance        = null;
            FormattingState        = default;
            CurrentBufferTypeStart = 0;
            IsValueType            = false;
            RevisitCount           = 0;
            CallerContext.Clear();
        }

        // ReSharper disable ParameterHidesMember
        // ReSharper disable InconsistentNaming
        public readonly void Deconstruct(out int ObjVisitIndex
              , out int ParentVisitIndex
              , out Type VistedAsType
              , out WriteMethodType WriteMethod
              , out object? StylingObjInstance
              , out int CurrentBufferTypeStart
              , out CallerContext CallerContext
              , out FormattingState FormattingState
            )
            // ReSharper restore ParameterHidesMember
            // ReSharper restore InconsistentNaming
        {
            ObjVisitIndex          = this.ObjVisitIndex;
            ParentVisitIndex       = this.ParentVisitIndex;
            VistedAsType           = this.VistedAsType;
            WriteMethod            = this.WriteMethod;
            StylingObjInstance     = this.VisitedInstance;
            CurrentBufferTypeStart = this.CurrentBufferTypeStart;
            CallerContext          = this.CallerContext;
            FormattingState        = this.FormattingState;
        }
    }