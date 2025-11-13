// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class KnownTypeMolder<T> : TypeMolder, ITypeBuilderComponentSource<T>
    where T : TypeMolder
{
    protected ITypeMolderDieCast<T> CompAccess = null!;

    protected void InitializeTypedStyledTypeBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string typeName
      , int remainignGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainignGraphDepth,  typeFormatting,  existingRefId);

        SourceBuilderComponentAccess();
    }

    ITypeMolderDieCast ITypeBuilderComponentSource.ComponentAccess => CompAccess;

    ITypeMolderDieCast<T> ITypeBuilderComponentSource<T>.CompAccess => CompAccess;

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
        CompAccess.Sb.RemoveLastWhiteSpacedCommaIfFound();
    }

    protected T Me => (T)(TypeMolder)this;

    public override StateExtractStringRange Complete()
    {
        if (!PortableState.AppenderSettings.SkipTypeParts.HasTypeEndFlag())
        {
            AppendClosing();
        }
        var currentAppenderIndex = CompAccess.Master.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result               = new StateExtractStringRange(TypeName, CompAccess.Master, typeWriteRange);
        PortableState.CompleteResult = result;
        CompAccess.Master.TypeComplete(CompAccess);
        return result;
    }

    protected bool AppendGraphFields()
    {
        if (CompAccess.StyleTypeBuilder.ExistingRefId != 0)
        {
            CompAccess.Sb.Append("\"$ref\":\"").Append(CompAccess.StyleTypeBuilder.ExistingRefId).Append("\" ");
            return true;
        }
        if (CompAccess.RemainingGraphDepth <= 0)
        {
            CompAccess.Sb.Append("\"$clipped\":\"maxDepth\"").Append(" ");
            CompAccess.SkipBody = true;
            CompAccess.SkipFields = true;
            return true;
        }
        
        return false;
    }

    protected virtual void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        CompAccess = recycler.Borrow<TypeMolderDieCast<T>>()
                             .Initialize((T)(ITypeBuilderComponentSource<T>)this, PortableState);
    }

    protected override void InheritedStateReset()
    {
        CompAccess?.DecrementIndent();
        CompAccess = null!;

        MeRecyclable.StateReset();
    }
}
