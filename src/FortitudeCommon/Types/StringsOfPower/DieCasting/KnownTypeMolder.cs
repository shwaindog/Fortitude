// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface IStateTransitioningTransitioningKnownTypeMolder : IDisposable
{
    void Initialize(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
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
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        InitializeStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth
                                 ,  typeFormatting,  existingRefId, createFormatFlags);

        SourceBuilderComponentAccess();
    }

    void IStateTransitioningTransitioningKnownTypeMolder.Free()
    {
        MoldStateField = null!;
        ((IRecyclableObject)this).DecrementRefCount();
    }
    
    protected ITypeMolderDieCast<TMold> State => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    ITypeMolderDieCast ITypeBuilderComponentSource.MoldState => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    ITypeMolderDieCast<TMold> ITypeBuilderComponentSource<TMold>.KnownTypeMoldState => MoldStateField ?? throw new NullReferenceException("Expected MoldState to be set");

    public override void Start()
    {
        if (!PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag())
        {
            AppendOpening();
            AppendGraphFields();
        }
    }
    
    public abstract void AppendOpening();

    public virtual void AppendClosing()
    {
        MoldStateField.StyleFormatter.GraphBuilder.RemoveLastSeparatorAndPadding();
    }

    protected TMold Me => (TMold)(TypeMolder)this;

    public override StateExtractStringRange Complete()
    {
        if (MoldStateField == null)
        {
            throw new NullReferenceException("Expected MoldState to be set");
        }
        if (!PortableState.AppenderSettings.SkipTypeParts.HasTypeEndFlag())
        {
            AppendClosing();
        }
        else
        {
            MoldStateField.StyleFormatter.GraphBuilder.RemoveLastSeparatorAndPadding();
            MoldStateField.StyleFormatter.GraphBuilder.AddHighWaterMark();
        }
        var currentAppenderIndex = MoldStateField.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result               = new StateExtractStringRange(TypeName ?? TypeBeingBuilt.CachedCSharpNameWithConstraints(), MoldStateField.Master, typeWriteRange);
        PortableState.CompleteResult = result;
        MoldStateField.Master.TypeComplete(MoldStateField);
        MoldStateField =  null!;
        ((IRecyclableObject)this).DecrementRefCount();
        return result;
    }

    protected bool AppendGraphFields()
    {
        if (MoldStateField.StyleTypeBuilder.ExistingRefId != 0)
        {
            MoldStateField.Sb.Append("\"$ref\":\"").Append(MoldStateField.StyleTypeBuilder.ExistingRefId).Append("\" ");
            return true;
        }
        if (MoldStateField.RemainingGraphDepth <= 0)
        {
            MoldStateField.Sb.Append("\"$clipped\":\"maxDepth\"").Append(" ");
            MoldStateField.SkipBody = true;
            MoldStateField.SkipFields = true;
            return true;
        }
        
        return false;
    }

    protected virtual void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<TypeMolderDieCast<TMold>>()
                             .Initialize((TMold)(ITypeBuilderComponentSource<TMold>)this, PortableState);
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

