// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ComplexValueTypeMold : ValueTypeMold<ComplexValueTypeMold>
{
    private TypeFieldCollection.SelectTypeCollectionField<ComplexValueTypeMold>? logOnlyInternalCollectionField;
    private TypeFields.SelectTypeField<ComplexValueTypeMold>?  logOnlyInternalField;

    public ComplexValueTypeMold InitializeComplexValueTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        InitializeValueTypeBuilder(typeBeingBuilt, master, typeSettings, typeName
                                 , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    public override bool IsComplexType => true;
    
    public override void AppendOpening()
    {
        MoldStateField.StyleFormatter.AppendComplexTypeOpening(MoldStateField);
    }
    
    public override void AppendClosing()
    {
        MoldStateField.StyleFormatter.AppendTypeClosing(MoldStateField);
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<ValueTypeDieCast<ComplexValueTypeMold>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }
    
    public TypeFields.SelectTypeField<ComplexValueTypeMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFields.SelectTypeField<ComplexValueTypeMold>>().Initialize(MoldStateField);

    public TypeFieldCollection.SelectTypeCollectionField<ComplexValueTypeMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFieldCollection.SelectTypeCollectionField<ComplexValueTypeMold>>().Initialize(MoldStateField);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        MoldStateField.DecrementIndent();
        MoldStateField = null!;
    }

    public ComplexValueTypeMold AddBaseFieldsStart()
    {
        MoldStateField.Master.AddBaseFieldsStart();

        return Me;
    }
}
