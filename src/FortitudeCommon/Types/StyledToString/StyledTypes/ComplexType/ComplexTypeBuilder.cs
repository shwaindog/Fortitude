using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.ComplexType;


public class ComplexTypeBuilder : MultiValueTypeBuilder<ComplexTypeBuilder>
{
    private SelectTypeKeyValueCollectionField<ComplexTypeBuilder>? addKeyCollectionField;

    private SelectTypeCollectionField<ComplexTypeBuilder>? addCollectionField;

    private SelectTypeField<ComplexTypeBuilder>?           addField;

    public ComplexTypeBuilder InitializeComplexTypeBuilder
    (StyledTypeStringAppender owningStyledTypeAppender
      , TypeAppendSettings appendSettings, string typeName)
    {
        InitializeMultiValueTypeBuilder(owningStyledTypeAppender, appendSettings, typeName);

        return this;
    }

    public SelectTypeKeyValueCollectionField<ComplexTypeBuilder> AddKeyedCollectionField
    {
        get => addKeyCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeKeyValueCollectionField<ComplexTypeBuilder>>().Initialize(CompAccess);
        protected set => addKeyCollectionField = value;
    }

    public SelectTypeCollectionField<ComplexTypeBuilder> AddCollectionField
    {
        get => addCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeCollectionField<ComplexTypeBuilder>>().Initialize(CompAccess);
        protected set => addCollectionField = value;
    }

    public SelectTypeField<ComplexTypeBuilder> AddField
    {
        get => addField ??= CompAccess.Recycler.Borrow<SelectTypeField<ComplexTypeBuilder>>().Initialize(CompAccess);
        protected set => addField = value;
    }


    public bool InBase { get; private set; }


    protected override void InheritedStateReset()
    {
        addCollectionField?.DecrementRefCount();
        addCollectionField = null;
        addField?.DecrementRefCount();
        addField = null;

        base.InheritedStateReset();
    }
}
