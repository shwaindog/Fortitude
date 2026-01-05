// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class MultiValueTypeMolder<TExt> : KnownTypeMolder<TExt> where TExt : TypeMolder
{
    private TypeFieldCollection.SelectTypeCollectionField<TExt>? logOnlyInternalCollectionField;
    private SelectTypeField<TExt>?  logOnlyInternalField;
    
    protected void InitializeMultiValueTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        Initialize(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName
                                       , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);
    }


    public TypeFields.SelectTypeField<TExt>? LogOnlyInternalField =>
        logOnlyInternalField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<TypeFields.SelectTypeField<TExt>>().Initialize(MoldStateField)
            : null;

    public TypeFieldCollection.SelectTypeCollectionField<TExt>? LogOnlyInternalCollectionField =>
        logOnlyInternalCollectionField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<TypeFieldCollection.SelectTypeCollectionField<TExt>>().Initialize(MoldStateField)
            : null;

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        base.InheritedStateReset();
    }

    public TExt AddBaseStyledToStringFields<T>(T thisType) where T : IStringBearer
    {
        if (MoldStateField.SkipBody) return MoldStateField.StyleTypeBuilder;
        MoldStateField.Master.AddBaseFieldsStart();
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, MoldStateField.Master);
        
        return Me;
    }
}
