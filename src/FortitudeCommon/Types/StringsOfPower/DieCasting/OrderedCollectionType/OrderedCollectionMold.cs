using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold> : KnownTypeMolder<TOCMold>
    where TOCMold : TypeMolder
{
    private CollectionBuilderCompAccess<TOCMold> stb = null!;

    public OrderedCollectionMold<TOCMold> InitializeOrderedCollectionBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType
                 , createFormatFlags | AsCollection);
        WrittenAs = WrittenAsFlags.AsCollection;

        stb = CompAsOrderedCollection;

        return this;
    }


    public override bool IsComplexType => false;

    public int ResultCount { get; set; }

    public int TotalCount { get; private set; }

    public override void StartFormattingTypeOpening()
    {
        if (CompAsOrderedCollection.SupportsMultipleFields) { MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField); }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            MoldStateField.StyleFormatter.FormatCollectionStart
                (MoldStateField, elementType!, true
               , MoldStateField.TypeBeingBuilt, MoldStateField.CreateMoldFormatFlags);
        }
    }

    public override void CompleteTypeOpeningToTypeFields() { }

    public override void AppendClosing()
    {
        var formatter = MoldStateField.StyleFormatter;
        if (CompAsOrderedCollection.SupportsMultipleFields)
        {
            formatter.AppendComplexTypeClosing(MoldStateField);
        }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            formatter.FormatCollectionEnd(MoldStateField, ResultCount, elementType!, ResultCount, "", MoldStateField.CreateMoldFormatFlags);
        }
    }

    protected virtual CollectionBuilderCompAccess<TOCMold> CompAsOrderedCollection => (CollectionBuilderCompAccess<TOCMold>)MoldStateField;
}

public class SimpleOrderedCollectionMold : OrderedCollectionMold<SimpleOrderedCollectionMold>
{
    public SimpleOrderedCollectionMold InitializeSimpleOrderedCollectionBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType
           , createFormatFlags | AsCollection);

        return this;
    }

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
    }
}

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>?                 logOnlyInternalField;

    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags | AsCollection);

        return this;
    }

    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess(WriteMethodType writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
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
