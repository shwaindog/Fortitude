// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public abstract class MultiValueTypeBuilder<TExt> : TypedStyledTypeBuilder<TExt> where TExt : StyledTypeBuilder
{
    private SelectTypeCollectionField<TExt>? logOnlyInternalCollectionField;
    private SelectTypeField<TExt>?           logOnlyInternalField;
    
    protected void InitializeMultiValueTypeBuilder
    (
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningStyledTypeAppender
      , TypeAppendSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, owningStyledTypeAppender, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);
    }


    public SelectTypeField<TExt>? LogOnlyInternalField =>
        logOnlyInternalField ??= Settings.Style.AllowsUnstructured()
            ? PortableState.OwningAppender.Recycler.Borrow<SelectTypeField<TExt>>().Initialize(CompAccess)
            : null;

    public SelectTypeCollectionField<TExt>? LogOnlyInternalCollectionField =>
        logOnlyInternalCollectionField ??= Settings.Style.AllowsUnstructured()
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

    public TExt AddBaseStyledToStringFields<T>(T thisType) where T : IStyledToStringObject
    {
        if (CompAccess.SkipBody) return CompAccess.StyleTypeBuilder;
        CompAccess.OwningAppender.AddBaseFieldsStart();
        TargetToStringInvoker.CallBaseStyledToStringIfSupported(thisType, CompAccess.OwningAppender);
        
        return Me;
    }
}
