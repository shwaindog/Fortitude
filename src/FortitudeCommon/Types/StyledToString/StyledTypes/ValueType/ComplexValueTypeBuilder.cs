// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

public class ComplexValueTypeBuilder : ValueTypeBuilder<ComplexValueTypeBuilder>
{
    private SelectTypeCollectionField<ComplexValueTypeBuilder>? logOnlyInternalCollectionField;
    private SelectTypeField<ComplexValueTypeBuilder>?           logOnlyInternalField;

    public ComplexValueTypeBuilder InitializeComplexValueTypeBuilder
    (
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeValueTypeBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, typeFormatting, existingRefId);

        return this;
    }

    protected override string TypeOpeningDelimiter => Stb.ValueInComplexType ? "{" : "";
    protected override string TypeClosingDelimiter => Stb.ValueInComplexType ? "}" : "";

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<ComplexValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }


    public SelectTypeField<ComplexValueTypeBuilder> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.OwningAppender.Recycler
                         .Borrow<SelectTypeField<ComplexValueTypeBuilder>>().Initialize(CompAccess);

    public SelectTypeCollectionField<ComplexValueTypeBuilder> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.OwningAppender.Recycler
                         .Borrow<SelectTypeCollectionField<ComplexValueTypeBuilder>>().Initialize(CompAccess);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        CompAccess?.DecrementIndent();
        CompAccess = null!;
    }

    public ComplexValueTypeBuilder AddBaseFieldsStart()
    {
        CompAccess.OwningAppender.AddBaseFieldsStart();

        return Me;
    }
}
