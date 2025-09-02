using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public partial class OrderedCollectionBuilder<TExt> : TypedStyledTypeBuilder<TExt>
    where TExt : StyledTypeBuilder
{
    private CollectionBuilderCompAccess<TExt> stb = null!;

    public OrderedCollectionBuilder<TExt> InitializeOrderedCollectionBuilder(IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings, string typeName, int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(owningAppender, typeSettings, typeName, existingRefId);

        stb = CompAsOrderedCollection;

        return this;
    }

    protected CollectionBuilderCompAccess<TExt> CompAsOrderedCollection => (CollectionBuilderCompAccess<TExt>)CompAccess;

    protected override string TypeOpeningDelimiter => "[";
    protected override string TypeClosingDelimiter => "]";
}

public class SimpleOrderedCollectionBuilder : OrderedCollectionBuilder<SimpleOrderedCollectionBuilder>
{
    public SimpleOrderedCollectionBuilder InitializeSimpleOrderedCollectionBuilder(IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings, string typeName, int  existingRefId)
    {
        InitializeOrderedCollectionBuilder(owningAppender, typeSettings, typeName,  existingRefId);

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

    protected override string TypeOpeningDelimiter => CompAsOrderedCollection.CollectionInComplexType ? "{" : "[";
    protected override string TypeClosingDelimiter => CompAsOrderedCollection.CollectionInComplexType ? "}" : "]";

    public ComplexOrderedCollectionBuilder InitializeComplexOrderedCollectionBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName, int existingRefId)
    {
        InitializeOrderedCollectionBuilder(owningAppender, typeSettings, typeName,  existingRefId);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionBuilder>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, true);
    }

    public SelectTypeField<ComplexOrderedCollectionBuilder>? LogOnlyField =>
        logOnlyInternalField ??= Style.AllowsUnstructured()
            ? PortableState.OwningAppender.Recycler.Borrow<SelectTypeField<ComplexOrderedCollectionBuilder>>().Initialize(CompAccess)
            : null;

    public SelectTypeCollectionField<ComplexOrderedCollectionBuilder>? LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??= Style.AllowsUnstructured()
            ? PortableState.OwningAppender.Recycler.Borrow<SelectTypeCollectionField<ComplexOrderedCollectionBuilder>>().Initialize(CompAccess)
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

    public ComplexOrderedCollectionBuilder AddBaseFieldsStart()
    {
        CompAccess.OwningAppender.AddBaseFieldsStart();

        return Me;
    }
}
