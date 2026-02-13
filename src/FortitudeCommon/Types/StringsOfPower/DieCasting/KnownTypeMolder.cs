// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface ITypeBuilderComponentSource<out T> : ITypeBuilderComponentSource where T : TypeMolder
{
    ITypeMolderDieCast<T> KnownTypeMoldState { get; }
    
    
    bool AppendGraphFields(IStyledTypeFormatting usingStyleTypeFormatter);
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
      , FormatFlags createFormatFlags);

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
    protected ITypeMolderDieCast<TMold> MoldStateField = null!;

    public void Initialize(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType  
      , FormatFlags createFormatFlags)
    {
        InitializeStyledTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName, remainingGraphDepth
                                  , moldGraphVisit, createFormatFlags);

        SourceBuilderComponentAccess(writeMethodType);
    }

    void IStateTransitioningTransitioningKnownTypeMolder.Free()
    {
        MoldStateField = null!;
        ((IRecyclableObject)this).DecrementRefCount();
    }

    protected ITypeMolderDieCast<TMold> State
    {
        [DebuggerStepThrough]
        get => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");
    }

    ITypeMolderDieCast ITypeBuilderComponentSource.MoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    ITypeMolderDieCast<TMold> ITypeBuilderComponentSource<TMold>.KnownTypeMoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    public override void StartTypeOpening()
    {
        if (PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        StartFormattingTypeOpening(MoldStateField.StyleFormatter);
        MyAppendGraphFields(MoldStateField.StyleFormatter);
    }

    public override void FinishTypeOpening()
    {
        if (PortableState.CreateFormatFlags.HasSuppressOpening()) return;
        CompleteTypeOpeningToTypeFields(MoldStateField.StyleFormatter);
    }

    public abstract void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter);
    public virtual  void CompleteTypeOpeningToTypeFields(IStyledTypeFormatting usingFormatter) { }

    public virtual void AppendClosing()
    {
        State.Sf.Gb.RemoveLastSeparatorAndPadding();
    }

    protected TMold Me => (TMold)(TypeMolder)this;

    public override AppendSummary Complete()
    {
        if (State == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        if (PortableState.CreateFormatFlags.DoesNotHaveSuppressClosing())
        {
            AppendClosing();
        }
        else
        {
            var gb = State.Sf.Gb;
            var hasUncommited = gb.CurrentSectionRanges.HasContent;
            if (!hasUncommited && !gb.LastContentSeparatorPaddingRanges.HasNonZeroLengthContent)
            {
                gb.RemoveLastSeparatorAndPadding();
            }
            else if (hasUncommited) { gb.Complete(State.CreateMoldFormatFlags); }
        }
        var currentAppenderIndex = State.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result               = BuildMoldStringRange(typeWriteRange);
        PortableState.CompleteResult = result;
        var tos  = State.Master;
        var verifyRemoved = MoldVisit;
        if (verifyRemoved.VisitId.RegistryId >= -1 
         && (tos.ActiveGraphRegistry.RegistryId != verifyRemoved.VisitId.RegistryId 
          || verifyRemoved.VisitId.VisitIndex >= tos.ActiveGraphRegistry.Count))
        {
            Debugger.Break();   
        }
        tos.TypeComplete(State); // calls DecrementRef count
        //MoldStateField = null!;

        if (verifyRemoved.VisitId.RegistryId >= -1 && verifyRemoved.VisitId.VisitIndex >= 0 
         && tos.ActiveGraphRegistry.Count > verifyRemoved.VisitId.VisitIndex 
        &&  tos.ActiveGraphRegistry[verifyRemoved.VisitId.VisitIndex].MoldState != null)
        {
            Debugger.Break();
        }
        return result;
    }

    protected bool MyAppendGraphFields(IStyledTypeFormatting usingFormatter) => 
        ((ITypeBuilderComponentSource<TMold>)this).AppendGraphFields(MoldStateField.StyleFormatter);

    bool ITypeBuilderComponentSource<TMold>.AppendGraphFields(IStyledTypeFormatting usingFormatter)
    {
        var msf         = MoldStateField;
        var createFlags = msf.CreateMoldFormatFlags;
        if (msf.Mold.RevisitedInstanceId != 0)
        {
            var charsWritten =
                usingFormatter
                   .AppendExistingReferenceId(msf, msf.Mold.RevisitedInstanceId, msf.CurrentWriteMethod, createFlags);
            msf.WroteRefId = charsWritten > 0;
        }
        if (MoldStateField.RemainingGraphDepth <= 0)
        {

            var charsWritten =
                usingFormatter
                    .AppendInstanceInfoField(msf, "$clipped", "maxDepth", msf.CurrentWriteMethod, createFlags);
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
            recycler.Borrow<TypeMolderDieCast<TMold>>()
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
