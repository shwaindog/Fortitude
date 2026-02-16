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
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType
                 , createFormatFlags | AsCollection);

        stb = CompAsOrderedCollection;

        return this;
    }


    public override bool IsComplexType => false;

    public int ResultCount { get; set; }

    public int TotalCount { get; private set; }

    protected Type ElementType { get; set; }

    public override void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter)
    {
        if (CompAsOrderedCollection.SupportsMultipleFields) { MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField); }
        else
        {
            var elementType = MoldStateField.Mold.TypeBeingBuilt.GetIterableElementType();
            usingFormatter.StartFormatCollectionOpen
                (MoldStateField, elementType!, true
               , MoldStateField.TypeBeingBuilt, MoldStateField.CreateMoldFormatFlags);
        }
    }

    public override void CompleteTypeOpeningToTypeFields(IStyledTypeFormatting usingFormatter)
    {
        if (CompAsOrderedCollection.SupportsMultipleFields) { MoldStateField.StyleFormatter.FinishComplexTypeOpening(MoldStateField); }
        else
        {
            var elementType = MoldStateField.Mold.TypeBeingBuilt.GetIterableElementType();
            usingFormatter.FinishFormatCollectionOpen
                (MoldStateField, elementType!, true
               , MoldStateField.TypeBeingBuilt, MoldStateField.CreateMoldFormatFlags);
        }
    }

    public override void AppendClosing()
    {
        if (CompAsOrderedCollection.SupportsMultipleFields) { State.StyleFormatter.AppendComplexTypeClosing(State); }
        else
        {
            var formatter   = MoldStateField.StyleFormatter;
            var elementType = State.Mold.TypeBeingBuilt.GetIterableElementType();
            formatter.FormatCollectionEnd(State, ResultCount, elementType!, ResultCount, "", MoldStateField.CreateMoldFormatFlags);
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
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType
           , createFormatFlags | AsCollection);

        return this;
    }

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod, true);
    }
}

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>?         logOnlyInternalCollectionField;
    private ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>?                         logOnlyInternalField;
    private ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalMapCollectionField;

    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , FormatFlags createFormatFlags)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags | AsCollection);

        return this;
    }

    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionMold>>()
                                 .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod, false);
    }

    public ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.UnitField.SelectTypeField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    public ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.CollectionField.SelectTypeCollectionField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    public ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold> LogOnlyKeyedCollectionField =>
        logOnlyInternalMapCollectionField ??=
            PortableState
                .Master
                .Recycler
                .Borrow<ComplexType.MapCollectionField.SelectTypeKeyedCollectionField<ComplexOrderedCollectionMold>>()
                .Initialize(MoldStateField);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        base.InheritedStateReset();
    }

    public ComplexOrderedCollectionMold AddBaseRevealStateFields<T>(T thisType) where T : IStringBearer
    {
        var msf              = MoldStateField;
        var markPreBodyStart = msf.Sb.Length;
        if (msf.SkipBody) return msf.Mold;

        MoldStateField.Master.AddBaseFieldsStart();
        TargetStringBearerRevealState.CallBaseStyledToStringIfSupported(thisType, msf.Master);
        if (msf.Sb.Length > markPreBodyStart) { msf.Sf.AddToNextFieldSeparatorAndPadding(); }

        return Me;
    }
}
