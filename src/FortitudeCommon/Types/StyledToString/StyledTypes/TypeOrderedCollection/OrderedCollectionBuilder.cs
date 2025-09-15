using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public partial class OrderedCollectionBuilder<TExt> : TypedStyledTypeBuilder<TExt>
    where TExt : StyledTypeBuilder
{
    private CollectionBuilderCompAccess<TExt> stb = null!;

    public OrderedCollectionBuilder<TExt> InitializeOrderedCollectionBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        stb = CompAsOrderedCollection;

        return this;
    }
    

    public override bool IsComplexType => false;
    
    public override void AppendOpening()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionStart(CompAccess, elementType!, true, CompAccess.TypeBeingBuilt);
        }
    }
    
    public override void AppendClosing()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendTypeClosing(CompAccess);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionEnd(CompAccess, elementType!, 1);
        }
    }

    protected CollectionBuilderCompAccess<TExt> CompAsOrderedCollection => (CollectionBuilderCompAccess<TExt>)CompAccess;
}

public class SimpleOrderedCollectionBuilder : OrderedCollectionBuilder<SimpleOrderedCollectionBuilder>
{
    public SimpleOrderedCollectionBuilder InitializeSimpleOrderedCollectionBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionBuilder>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, false);
    }
}

public class ComplexOrderedCollectionBuilder : OrderedCollectionBuilder<ComplexOrderedCollectionBuilder>
{
    private SelectTypeCollectionField<ComplexOrderedCollectionBuilder>? logOnlyInternalCollectionField;
    private SelectTypeField<ComplexOrderedCollectionBuilder>?           logOnlyInternalField;

    public ComplexOrderedCollectionBuilder InitializeComplexOrderedCollectionBuilder
    (
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        return this;
    }

    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionBuilder>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, true);
    }

    public SelectTypeField<ComplexOrderedCollectionBuilder> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.OwningAppender.Recycler
                         .Borrow<SelectTypeField<ComplexOrderedCollectionBuilder>>().Initialize(CompAccess);

    public SelectTypeCollectionField<ComplexOrderedCollectionBuilder> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.OwningAppender.Recycler
                         .Borrow<SelectTypeCollectionField<ComplexOrderedCollectionBuilder>>().Initialize(CompAccess);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        CompAccess.DecrementIndent();
        CompAccess = null!;
    }

    public ComplexOrderedCollectionBuilder AddBaseFieldsStart()
    {
        CompAccess.OwningAppender.AddBaseFieldsStart();

        return Me;
    }
}
