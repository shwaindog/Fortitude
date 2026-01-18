using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;


public class ComplexPocoTypeMold : MultiValueTypeMolder<ComplexPocoTypeMold>
{
    private MapCollectionField.SelectTypeKeyedCollectionField<ComplexPocoTypeMold>? addKeyCollectionField;

    private SelectTypeCollectionField<ComplexPocoTypeMold>? addCollectionField;

    private UnitField.SelectTypeField<ComplexPocoTypeMold>?           addField;

    public ComplexPocoTypeMold InitializeComplexTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , TheOneString owning
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , IStyledTypeFormatting typeFormatting
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(instanceOrContainer, typeBeingBuilt, owning, appendSettings, typeName, remainingGraphDepth
                                      , moldGraphVisit, typeFormatting, writeMethodType, createFormatFlags);

        return this;
    }

    public override bool IsComplexType => true;

    public override void StartFormattingTypeOpening()
    {
        State.StyleFormatter.StartComplexTypeOpening(State);
    }
    
    public override void AppendClosing()
    {
        State.StyleFormatter.AppendComplexTypeClosing(State);
    }

    public virtual void StartContent()
    {
    }

    public MapCollectionField.SelectTypeKeyedCollectionField<ComplexPocoTypeMold> KeyedCollectionField
    {
        get => addKeyCollectionField ??= State.Recycler.Borrow<MapCollectionField.SelectTypeKeyedCollectionField<ComplexPocoTypeMold>>().Initialize(State);
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
