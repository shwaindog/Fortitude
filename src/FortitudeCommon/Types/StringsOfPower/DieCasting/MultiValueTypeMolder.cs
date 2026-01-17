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
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags )
    {
        Initialize(instanceOrContainer, typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName
                                       , remainingGraphDepth, typeFormatting, existingRefId, writeMethodType, createFormatFlags);
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

    public TExt AddBaseRevealStateFields<T>(T thisType) where T : IStringBearer
    {
        var msf              = MoldStateField;
        var markPreBodyStart = msf.Sb.Length;
        if (msf.SkipBody) return msf.StyleTypeBuilder;
        msf.Master.AddBaseFieldsStart();
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, msf.Master);
        if (msf.Sb.Length > markPreBodyStart)
        {
            msf.StyleFormatter.GraphBuilder.StartNextContentSeparatorPaddingSequence(msf.Sb, FormatFlags.DefaultCallerTypeFlags);
            msf.StyleFormatter.AddToNextFieldSeparatorAndPadding();
        }

        return Me;
    }
}
