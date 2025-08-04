namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public class SimpleOrderedCollectionBuilder : TypedStyledTypeBuilder<SimpleOrderedCollectionBuilder>
{
    private IAddAllTypeIsOrderedCollection<SimpleOrderedCollectionBuilder>?      addAll;
    private IAddFilteredTypeIsOrderedCollection<SimpleOrderedCollectionBuilder>? addFiltered;

    public SimpleOrderedCollectionBuilder InitializeSimpleOrderedCollectionBuilder(IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings, string typeName)
    {
        InitializeTypedStyledTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }

    protected CollectionBuilderCompAccess<SimpleOrderedCollectionBuilder> CompAsSimpleBuilder =>
        (CollectionBuilderCompAccess<SimpleOrderedCollectionBuilder>)CompAccess;

    protected override string TypeOpeningDelimiter => "[";
    protected override string TypeClosingDelimiter => "]";

    private IAddAllTypeIsOrderedCollection<SimpleOrderedCollectionBuilder> AddAll
    {
        get => addAll ?? CompAccess.Recycler.Borrow<AddAllTypeIsOrderedCollection<SimpleOrderedCollectionBuilder>>().Initialize(CompAsSimpleBuilder);
        set => addAll = value;
    }

    public IAddFilteredTypeIsOrderedCollection<SimpleOrderedCollectionBuilder> AddFiltered
    {
        get =>
            addFiltered ?? CompAccess.Recycler.Borrow<AddFilteredTypeIsOrderedCollection<SimpleOrderedCollectionBuilder>>()
                                     .Initialize(CompAsSimpleBuilder);
        set => addFiltered = value;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionBuilder>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, false);
    }
}

public class ComplexOrderedCollectionBuilder : MultiValueTypeBuilder<ComplexOrderedCollectionBuilder>
{
    private AddAllTypeIsOrderedCollection<ComplexOrderedCollectionBuilder>?      addAll;
    private AddFilteredTypeIsOrderedCollection<ComplexOrderedCollectionBuilder>? addFiltered;

    protected override string TypeOpeningDelimiter => CompAsComplexBuilder.CollectionInComplexType ? "{" : "[";
    protected override string TypeClosingDelimiter => CompAsComplexBuilder.CollectionInComplexType ? "}" : "]";

    public ComplexOrderedCollectionBuilder InitializeComplexOrderedCollectionBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        InitializeMultiValueTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }

    protected CollectionBuilderCompAccess<ComplexOrderedCollectionBuilder> CompAsComplexBuilder =>
        (CollectionBuilderCompAccess<ComplexOrderedCollectionBuilder>)CompAccess;

    private AddAllTypeIsOrderedCollection<ComplexOrderedCollectionBuilder> AddAll
    {
        get =>
            addAll ?? CompAccess.Recycler
                                .Borrow<AddAllTypeIsOrderedCollection<ComplexOrderedCollectionBuilder>>()
                                .Initialize(CompAsComplexBuilder);
        set => addAll = value;
    }

    public AddFilteredTypeIsOrderedCollection<ComplexOrderedCollectionBuilder> AddFiltered
    {
        get =>
            addFiltered ?? CompAccess.Recycler
                                     .Borrow<AddFilteredTypeIsOrderedCollection<ComplexOrderedCollectionBuilder>>()
                                     .Initialize(CompAsComplexBuilder);
        set => addFiltered = value;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionBuilder>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, true);
    }

    protected override void InheritedStateReset()
    {
        addAll?.DecrementRefCount();
        addAll = null!;
        addFiltered?.DecrementRefCount();
        addFiltered = null!;
        base.InheritedStateReset();
    }
}
