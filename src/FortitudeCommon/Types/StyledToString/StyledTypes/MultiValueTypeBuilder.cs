// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public abstract class MultiValueTypeBuilder<TExt> : TypedStyledTypeBuilder<TExt> where TExt : StyledTypeBuilder
{
    private SelectTypeCollectionField<TExt>? logOnlyInternalCollectionField;
    private SelectTypeField<TExt>?           logOnlyInternalField;
    
    protected void InitializeMultiValueTypeBuilder(IStyleTypeAppenderBuilderAccess owningStyledTypeAppender
      , TypeAppendSettings appendSettings, string typeName)
    {
        InitializeTypedStyledTypeBuilder(owningStyledTypeAppender, appendSettings, typeName);
    }


    public SelectTypeField<TExt>? LogOnlyInternalField =>
        logOnlyInternalField ??= Style.AllowsUnstructured()
            ? PortableState.OwningAppender.Recycler.Borrow<SelectTypeField<TExt>>().Initialize(CompAccess)
            : null;

    public SelectTypeCollectionField<TExt>? LogOnlyInternalCollectionField =>
        logOnlyInternalCollectionField ??= Style.AllowsUnstructured()
            ? PortableState.OwningAppender.Recycler.Borrow<SelectTypeCollectionField<TExt>>().Initialize(CompAccess)
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

    public TExt AddBaseFieldsStart()
    {
        CompAccess.OwningAppender.AddBaseFieldsStart();

        return Me;
    }
}
