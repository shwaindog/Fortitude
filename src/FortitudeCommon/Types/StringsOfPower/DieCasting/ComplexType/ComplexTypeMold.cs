using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyedCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;


public class ComplexTypeMold : MultiValueTypeMolder<ComplexTypeMold>
{
    private SelectTypeKeyedCollectionField<ComplexTypeMold>? addKeyCollectionField;

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
        State.StyleFormatter.AppendComplexTypeOpening(State);
    }
    
    public override void AppendClosing()
    {
        State.StyleFormatter.AppendTypeClosing(State);
    }

    public virtual void StartContent()
    {
    }

    public SelectTypeKeyedCollectionField<ComplexTypeMold> KeyedCollectionField
    {
        get => addKeyCollectionField ??= State.Recycler.Borrow<SelectTypeKeyedCollectionField<ComplexTypeMold>>().Initialize(State);
        protected set => addKeyCollectionField = value;
    }

    public SelectTypeCollectionField<ComplexTypeMold> CollectionField
    {
        get => addCollectionField ??= State.Recycler.Borrow<SelectTypeCollectionField<ComplexTypeMold>>().Initialize(State);
        protected set => addCollectionField = value;
    }

    public SelectTypeField<ComplexTypeMold> Field
    {
        get => addField ??= State.Recycler.Borrow<SelectTypeField<ComplexTypeMold>>().Initialize(State);
        protected set => addField = value;
    }

    protected override void InheritedStateReset()
    {
        addCollectionField?.DecrementRefCount();
        addCollectionField = null;
        addField?.DecrementRefCount();
        addField = null;

        base.InheritedStateReset();
    }
}
