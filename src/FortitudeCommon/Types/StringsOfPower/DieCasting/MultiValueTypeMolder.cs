// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class MultiValueTypeMolder<TExt> : KnownTypeMolder<TExt> where TExt : TypeMolder
{
    private TypeFieldCollection.SelectTypeCollectionField<TExt>? logOnlyInternalCollectionField;
    private TypeFields.SelectTypeField<TExt>?  logOnlyInternalField;
    
    protected void InitializeMultiValueTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);
    }


    public TypeFields.SelectTypeField<TExt>? LogOnlyInternalField =>
        logOnlyInternalField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<TypeFields.SelectTypeField<TExt>>().Initialize(CompAccess)
            : null;

    public TypeFieldCollection.SelectTypeCollectionField<TExt>? LogOnlyInternalCollectionField =>
        logOnlyInternalCollectionField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<TypeFieldCollection.SelectTypeCollectionField<TExt>>().Initialize(CompAccess)
            : null;

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        CompAccess?.DecrementIndent();
        CompAccess = null!;
    }

    public TExt AddBaseStyledToStringFields<T>(T thisType) where T : IStringBearer
    {
        if (CompAccess.SkipBody) return CompAccess.StyleTypeBuilder;
        CompAccess.Master.AddBaseFieldsStart();
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, CompAccess.Master);
        
        return Me;
    }
}
