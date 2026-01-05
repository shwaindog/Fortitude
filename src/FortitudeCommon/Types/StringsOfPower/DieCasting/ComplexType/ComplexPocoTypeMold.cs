using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.KeyedCollectionField;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;


public class ComplexPocoTypeMold : MultiValueTypeMolder<ComplexPocoTypeMold>
{
    private SelectTypeKeyedCollectionField<ComplexPocoTypeMold>? addKeyCollectionField;

    private SelectTypeCollectionField<ComplexPocoTypeMold>? addCollectionField;

    private UnitField.SelectTypeField<ComplexPocoTypeMold>?           addField;

    public ComplexPocoTypeMold InitializeComplexTypeBuilder
    (
        Type typeBeingBuilt
      , TheOneString owning
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
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

    public SelectTypeKeyedCollectionField<ComplexPocoTypeMold> KeyedCollectionField
    {
        get => addKeyCollectionField ??= State.Recycler.Borrow<SelectTypeKeyedCollectionField<ComplexPocoTypeMold>>().Initialize(State);
        protected set => addKeyCollectionField = value;
    }

    public SelectTypeCollectionField<ComplexPocoTypeMold> CollectionField
    {
        get => addCollectionField ??= State.Recycler.Borrow<SelectTypeCollectionField<ComplexPocoTypeMold>>().Initialize(State);
        protected set => addCollectionField = value;
    }

    public UnitField.SelectTypeField<ComplexPocoTypeMold> Field
    {
        get => addField ??= State.Recycler.Borrow<UnitField.SelectTypeField<ComplexPocoTypeMold>>().Initialize(State);
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
