using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
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
    (
        Type typeBeingBuilt
      , StyledTypeStringAppender owningStyledTypeAppender
      , TypeAppendSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, owningStyledTypeAppender, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        return this;
    }

    public override bool IsComplexType => true;

    public override void AppendOpening()
    {
        CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
    }
    
    public override void AppendClosing()
    {
        CompAccess.StyleFormatter.AppendTypeClosing(CompAccess);
    }

    public SelectTypeKeyValueCollectionField<ComplexTypeBuilder> KeyedCollectionField
    {
        get => addKeyCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeKeyValueCollectionField<ComplexTypeBuilder>>().Initialize(CompAccess);
        protected set => addKeyCollectionField = value;
    }

    public SelectTypeCollectionField<ComplexTypeBuilder> CollectionField
    {
        get => addCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeCollectionField<ComplexTypeBuilder>>().Initialize(CompAccess);
        protected set => addCollectionField = value;
    }

    public SelectTypeField<ComplexTypeBuilder> Field
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
