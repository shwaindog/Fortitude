// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface ITypeBuilderComponentSource<out T> : ITypeBuilderComponentSource where T : TypeMolder
{
    IMoldWriteState<T> KnownTypeMoldState { get; }

    bool AppendGraphFields<TValue>(TValue instance, VisitResult visitResult, IStyledTypeFormatting usingFormatter
      , WrittenAsFlags writeMethod, TypeMoldFlags moldWrittenFlags, FormatFlags formatFlags = DefaultCallerTypeFlags);
}

public interface IStateTransitioningTransitioningKnownTypeMolder : IDisposable
{
    void Initialize(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext  
      , CreateContext createContext);

    void Free();
}

public interface INextStateTransitioningKnownTypeMolder<out TMold> : IStateTransitioningTransitioningKnownTypeMolder
    where TMold : TypeMolder
{
    TMold Initialize(IStateTransitioningTransitioningKnownTypeMolder previousStateTransitioning);
}

public abstract class KnownTypeMolder<TMold> : TypeMolder, ITypeBuilderComponentSource<TMold>, IStateTransitioningTransitioningKnownTypeMolder
    where TMold : TypeMolder
{
    protected IMoldWriteState<TMold> MoldStateField = null!;

    public void Initialize(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext  
      , CreateContext createContext)
    {
        InitializeStyledTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName, remainingGraphDepth
                                  , moldGraphVisit, callerContext, createContext);

        SourceBuilderComponentAccess(writeMethodType);
    }

    void IStateTransitioningTransitioningKnownTypeMolder.Free()
    {
        MoldStateField = null!;
        ((IRecyclableObject)this).DecrementRefCount();
    }

    protected IMoldWriteState<TMold> State
    {
        [DebuggerStepThrough] get => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");
    }

    public override bool IsComplexType => State.CreateWriteMethod.SupportsMultipleFields();

    public FormatFlags CallerFormatFlags => State.CallerFormatFlags;
    
    public string? CallerFormatString => State.CallerFormatString;
    
    public override WrittenAsFlags WrittenAs
    {
        get => State.CurrentWriteMethod;
        
        protected set
        {
            State.CurrentWriteMethod = value;
            base.WrittenAs = value;
        }
    }


    IMoldWriteState ITypeBuilderComponentSource.MoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    IMoldWriteState<TMold> ITypeBuilderComponentSource<TMold>.KnownTypeMoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    public override void StartTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        StartTypeOpening(MoldStateField.StyleFormatter, formatFlags);
    }

    public override void FinishTypeOpening(FormatFlags formatFlags)
    {
        if (PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        FinishTypeOpening(MoldStateField.StyleFormatter, formatFlags);
    }

    public virtual void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mws = State;
        if (IsComplexType)
        {
            usingFormatter.StartComplexTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags);
            if (mws.Style.IsLog() && mws.StartedTypeName)
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
            }
        }
        else { 
            usingFormatter.StartSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags);
            if (mws.Style.IsLog() && mws.StartedTypeName)
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
            }
        }
    }

    public virtual void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        var mws = State;
        if (IsComplexType)
        {
            usingFormatter.FinishComplexTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags);
            if (mws.Style.IsNotLog())
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
            }
        }
        else
        {
            usingFormatter.FinishSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CreateWriteMethod, formatFlags);
            if (mws.Style.IsNotLog())
            {
                MyAppendGraphFields(MoldStateField.InstanceOrType, MoldStateField.MoldGraphVisit, usingFormatter
                                  , MoldStateField.CreateWriteMethod, MoldStateField.MoldWrittenFlags, formatFlags);
            }
        }
    }

    public virtual void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var mws = State;
        if (mws.CurrentWriteMethod.SupportsMultipleFields())
        {
            State.StyleFormatter.AppendComplexTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod, formatFlags);
        }
        else
        {
            State.Sf.Gb.RemoveLastSeparatorAndPadding();
        }
    }

    protected TMold Me => (TMold)(TypeMolder)this;

    protected virtual AppendSummary RunShutdown()
    {
        var currentAppenderIndex = State.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result               = BuildMoldStringRange(typeWriteRange);
        PortableState.CompleteResult = result;
        var tos           = State.Master;
        tos.TypeComplete(State); // calls DecrementRef count
        return result;
    }

    public override AppendSummary Complete(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (State == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        if (State.CreateMoldFormatFlags.DoesNotHaveSuppressClosing()) { AppendClosing(formatFlags); }
        else
        {
            var gb            = State.Sf.Gb;
            var hasUncommited = gb.CurrentSectionRanges.HasContent;
            if (!hasUncommited && !gb.LastContentSeparatorPaddingRanges.HasNonZeroLengthContent) { gb.RemoveLastSeparatorAndPadding(); }
            else if (hasUncommited) { gb.Complete(State.CreateMoldFormatFlags); }
        }
        return RunShutdown();
    }

    protected bool MyAppendGraphFields<T>(T instance, VisitResult visitResult, IStyledTypeFormatting usingFormatter
      , WrittenAsFlags writeMethod, TypeMoldFlags moldWrittenFlags, FormatFlags formatFlags) =>
        ((ITypeBuilderComponentSource<TMold>)this)
        .AppendGraphFields(instance, visitResult, usingFormatter, writeMethod, moldWrittenFlags, formatFlags);

    bool ITypeBuilderComponentSource<TMold>.AppendGraphFields<T>(T instance, VisitResult visitResult, IStyledTypeFormatting usingFormatter
      , WrittenAsFlags writeMethod, TypeMoldFlags moldWrittenFlags, FormatFlags formatFlags)
    {
        var msf = MoldStateField;
        if (visitResult.IsARevisit)
        {
            var charsWritten =
                usingFormatter
                    .AppendExistingReferenceId(msf, visitResult.InstanceId, TypeBeingBuilt, writeMethod, moldWrittenFlags, formatFlags);
            msf.WroteRefId = charsWritten > 0;
        }
        if (MoldStateField.RemainingGraphDepth <= 0)
        {
            var charsWritten =
                usingFormatter
                    .AppendInstanceInfoField(msf, "$clipped", "maxDepth", msf.CurrentWriteMethod, formatFlags);
            if (charsWritten > 0)
            {
                msf.WasDepthClipped = true;
                msf.SkipBody        = true;
                msf.SkipFields      = true;
            }
            else
            {
                msf.WasDepthClipped = true;
                msf.SkipFields      = true;
            }
            return true;
        }

        return false;
    }

    protected virtual void SourceBuilderComponentAccess(WrittenAsFlags currentWriteMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField =
            recycler.Borrow<MoldWriteState<TMold>>()
                    .Initialize((TMold)(ITypeBuilderComponentSource<TMold>)this, PortableState, currentWriteMethod);
    }

    protected override void InheritedStateReset()
    {
        if (MoldStateField != null!)
        {
            MoldStateField.DecrementRefCount();
            MoldStateField = null!;
        }
    }
}

public abstract class TwoStateTransitioningKnownTypeMolder<TCurrentMold, TNextMold> : KnownTypeMolder<TCurrentMold>
    where TCurrentMold : TypeMolder
    where TNextMold : TypeMolder, INextStateTransitioningKnownTypeMolder<TNextMold>, new()
{
    protected TwoStateTransitioningKnownTypeMolder() { }

    public TNextMold NextMold => ((IRecyclableObject)this).Recycler!.Borrow<TNextMold>().Initialize(this);
}
