using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;


public class ComplexTypeMold : MultiValueTypeMolder<ComplexTypeMold>
{
    private SelectTypeKeyValueCollectionField<ComplexTypeMold>? addKeyCollectionField;

    private SelectTypeCollectionField<ComplexTypeMold>? addCollectionField;

    private SelectTypeField<ComplexTypeMold>?           addField;

    public ComplexTypeMold InitializeComplexTypeBuilder
    (
        Type typeBeingBuilt
      , TheOneString owning
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FieldContentHandling createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, owning, appendSettings, typeName, remainingGraphDepth
                                      , typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    public override bool IsComplexType => true;

    public override void AppendOpening()
    {
        CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess);
    }
    
    public override void AppendClosing()
    {
        CompAccess.StyleFormatter.AppendTypeClosing(CompAccess);
    }

    public SelectTypeKeyValueCollectionField<ComplexTypeMold> KeyedCollectionField
    {
        get => addKeyCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeKeyValueCollectionField<ComplexTypeMold>>().Initialize(CompAccess);
        protected set => addKeyCollectionField = value;
    }

    public SelectTypeCollectionField<ComplexTypeMold> CollectionField
    {
        get => addCollectionField ??= CompAccess.Recycler.Borrow<SelectTypeCollectionField<ComplexTypeMold>>().Initialize(CompAccess);
        protected set => addCollectionField = value;
    }

    public SelectTypeField<ComplexTypeMold> Field
    {
        get => addField ??= CompAccess.Recycler.Borrow<SelectTypeField<ComplexTypeMold>>().Initialize(CompAccess);
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
