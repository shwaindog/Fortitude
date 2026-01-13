using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold> : KnownTypeMolder<TOCMold>
    where TOCMold : TypeMolder
{
    private CollectionBuilderCompAccess<TOCMold> stb = null!;

    public OrderedCollectionMold<TOCMold> InitializeOrderedCollectionBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags)
    {
        Initialize(typeBeingBuilt, master, typeSettings, typeName
                 , remainingGraphDepth, typeFormatting, existingRefId
                 , createFormatFlags | AsCollection);

        stb = CompAsOrderedCollection;

        return this;
    }


    public override bool IsComplexType => false;

    public int ResultCount { get; set; }

    public int TotalCount { get; private set; }

    public override void AppendTypeOpeningToGraphFields()
    {
        if (CompAsOrderedCollection.CollectionInComplexType) { MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField); }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            MoldStateField.StyleFormatter.FormatCollectionStart
                (MoldStateField, elementType!, true
               , MoldStateField.TypeBeingBuilt, MoldStateField.CreateContentHandling);
        }
    }

    public override void CompleteTypeOpeningToTypeFields() { }

    public override void AppendClosing()
    {
        var formatter = MoldStateField.StyleFormatter;
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            formatter.AppendComplexTypeClosing(MoldStateField);
        }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            formatter.FormatCollectionEnd(MoldStateField, ResultCount, elementType!, ResultCount, "", MoldStateField.CreateContentHandling);
        }
    }

    protected virtual CollectionBuilderCompAccess<TOCMold> CompAsOrderedCollection => (CollectionBuilderCompAccess<TOCMold>)MoldStateField;
}

public class SimpleOrderedCollectionMold : OrderedCollectionType.OrderedCollectionMold<SimpleOrderedCollectionMold>
{
    public SimpleOrderedCollectionMold InitializeSimpleOrderedCollectionBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (typeBeingBuilt, master, typeSettings, typeName
           , remainingGraphDepth, typeFormatting, existingRefId
           , createFormatFlags | AsCollection);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, false);
    }
}

public class ComplexOrderedCollectionMold : OrderedCollectionType.OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>?                 logOnlyInternalField;

    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (typeBeingBuilt, master, typeSettings, typeName
           , remainingGraphDepth, typeFormatting, existingRefId
           , createFormatFlags | AsCollection);

        return this;
    }

    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, true);
    }

    public ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.Master.Recycler
                         .Borrow<ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>>().Initialize(MoldStateField);

    public ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.Master.Recycler
                         .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>>().Initialize(MoldStateField);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        base.InheritedStateReset();
    }

    public ComplexOrderedCollectionMold AddBaseFieldsStart()
    {
        MoldStateField.Master.AddBaseFieldsStart();

        return Me;
    }
}
