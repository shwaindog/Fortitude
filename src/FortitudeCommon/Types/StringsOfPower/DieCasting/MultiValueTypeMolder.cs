// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public abstract class MultiValueTypeMolder<TExt> : KnownTypeMolder<TExt> where TExt : TypeMolder
{
    private ComplexType.CollectionField.SelectTypeCollectionField<TExt>? logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<TExt>?                 logOnlyInternalField;
    
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


    public ComplexType.UnitField.SelectTypeField<TExt>? LogOnlyInternalField =>
        logOnlyInternalField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<ComplexType.UnitField.SelectTypeField<TExt>>().Initialize(MoldStateField)
            : null;

    public ComplexType.CollectionField.SelectTypeCollectionField<TExt>? LogOnlyInternalCollectionField =>
        logOnlyInternalCollectionField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.Master.Recycler.Borrow<ComplexType.CollectionField.SelectTypeCollectionField<TExt>>().Initialize(MoldStateField)
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
