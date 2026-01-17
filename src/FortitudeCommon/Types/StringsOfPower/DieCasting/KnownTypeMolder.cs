// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface IStateTransitioningTransitioningKnownTypeMolder : IDisposable
{
    void Initialize(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , WriteMethodType writeMethodType  
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
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        InitializeStyledTypeBuilder(instanceOrContainer, typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth
                                  , typeFormatting, existingRefId, createFormatFlags);

        SourceBuilderComponentAccess(writeMethodType);
    }

    void IStateTransitioningTransitioningKnownTypeMolder.Free()
    {
        MoldStateField = null!;
        ((IRecyclableObject)this).DecrementRefCount();
    }

    protected ITypeMolderDieCast<TMold> State => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    ITypeMolderDieCast ITypeBuilderComponentSource.MoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    ITypeMolderDieCast<TMold> ITypeBuilderComponentSource<TMold>.KnownTypeMoldState =>
        MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    public override void StartTypeOpening()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        StartFormattingTypeOpening();
        AppendGraphFields();
    }

    public override void FinishTypeOpening()
    {
        if (!PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) { CompleteTypeOpeningToTypeFields(); }
    }

    public abstract void StartFormattingTypeOpening();
    public virtual  void CompleteTypeOpeningToTypeFields() { }

    public virtual void AppendClosing()
    {
        MoldStateField.StyleFormatter.GraphBuilder.RemoveLastSeparatorAndPadding();
    }

    protected TMold Me => (TMold)(TypeMolder)this;

    public override StateExtractStringRange Complete()
    {
        if (MoldStateField == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        if (!PortableState.AppenderSettings.SkipTypeParts.HasTypeEndFlag()) { AppendClosing(); }
        else
        {
            MoldStateField.StyleFormatter.GraphBuilder.RemoveLastSeparatorAndPadding();
            MoldStateField.StyleFormatter.GraphBuilder.AddHighWaterMark();
        }
        var currentAppenderIndex = MoldStateField.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result =
            new StateExtractStringRange(TypeName ??
                                        TypeBeingBuilt.CachedCSharpNameWithConstraints(), MoldStateField.Master, typeWriteRange);
        PortableState.CompleteResult = result;
        MoldStateField.Master.TypeComplete(MoldStateField);
        MoldStateField = null!;
        ((IRecyclableObject)this).DecrementRefCount();
        return result;
    }

    protected bool AppendGraphFields()
    {
        var msf         = MoldStateField;
        var createFlags = msf.CreateMoldFormatFlags;
        if (msf.StyleTypeBuilder.ExistingRefId != 0)
        {
            var charsWritten =
                msf.StyleFormatter
                   .AppendExistingReferenceId(msf, msf.StyleTypeBuilder.ExistingRefId, msf.WriteMethod, createFlags);
            msf.WroteRefId = charsWritten > 0;
            return true;
        }
        if (MoldStateField.RemainingGraphDepth <= 0)
        {
            var formatter = msf.StyleFormatter;

            var charsWritten =
                formatter
                    .AppendInstanceInfoField(msf, "$clipped", "maxDepth", msf.WriteMethod, createFlags);
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

    protected virtual void SourceBuilderComponentAccess(WriteMethodType writeMethodType)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField =
            recycler.Borrow<TypeMolderDieCast<TMold>>()
                    .Initialize((TMold)(ITypeBuilderComponentSource<TMold>)this, PortableState, writeMethodType);
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
