// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class KnownTypeMolder<TMold> : TypeMolder, ITypeBuilderComponentSource<TMold>
    where TMold : TypeMolder
{
    protected ITypeMolderDieCast<TMold> CompAccess = null!;

    protected void InitializeTypedStyledTypeBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainignGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FieldContentHandling createFormatFlags )
    {
        InitializeStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainignGraphDepth
                                 ,  typeFormatting,  existingRefId, createFormatFlags);

        SourceBuilderComponentAccess();
    }

    ITypeMolderDieCast ITypeBuilderComponentSource.ComponentAccess => CompAccess;

    ITypeMolderDieCast<TMold> ITypeBuilderComponentSource<TMold>.CompAccess => CompAccess;

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

    protected TMold Me => (TMold)(TypeMolder)this;

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
        CompAccess = recycler.Borrow<TypeMolderDieCast<TMold>>()
                             .Initialize((TMold)(ITypeBuilderComponentSource<TMold>)this, PortableState);
    }

    protected override void InheritedStateReset()
    {
        CompAccess?.DecrementIndent();
        CompAccess = null!;

        MeRecyclable.StateReset();
    }
}
