using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

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

    public override void AppendOpening()
    {
        if (CompAsOrderedCollection.CollectionInComplexType) { MoldStateField.StyleFormatter.AppendComplexTypeOpening(MoldStateField); }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            MoldStateField.StyleFormatter.FormatCollectionStart
                (MoldStateField, elementType!, true
               , MoldStateField.TypeBeingBuilt, MoldStateField.CreateContentHandling);
        }
    }

    public override void AppendClosing()
    {
        if (CompAsOrderedCollection.CollectionInComplexType) { MoldStateField.StyleFormatter.AppendTypeClosing(MoldStateField); }
        else
        {
            var elementType = MoldStateField.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            MoldStateField.StyleFormatter.FormatCollectionEnd(MoldStateField, ResultCount, elementType!, ResultCount, "", MoldStateField.CreateContentHandling);
        }
    }

    protected virtual CollectionBuilderCompAccess<TOCMold> CompAsOrderedCollection => (CollectionBuilderCompAccess<TOCMold>)MoldStateField;
}

public class SimpleOrderedCollectionMold : OrderedCollectionMold<SimpleOrderedCollectionMold>
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

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalCollectionField;
    private TypeFields.SelectTypeField<ComplexOrderedCollectionMold>?                    logOnlyInternalField;

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

    public TypeFields.SelectTypeField<ComplexOrderedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFields.SelectTypeField<ComplexOrderedCollectionMold>>().Initialize(MoldStateField);

    public TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold>>().Initialize(MoldStateField);

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
