using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

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
      , FieldContentHandling createFormatFlags )
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName
                                       , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        stb = CompAsOrderedCollection;

        return this;
    }
    

    public override bool IsComplexType => false;
    
    public override void AppendOpening()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionStart(CompAccess, elementType!, true, CompAccess.TypeBeingBuilt);
        }
    }
    
    public override void AppendClosing()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendTypeClosing(CompAccess);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionEnd(CompAccess, 1, elementType!, 1, "");
        }
    }

    protected virtual CollectionBuilderCompAccess<TOCMold> CompAsOrderedCollection =>  (CollectionBuilderCompAccess<TOCMold>)CompAccess;
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
      , FieldContentHandling createFormatFlags )
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, master, typeSettings, typeName
                                         , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<SimpleOrderedCollectionMold>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, false);
    }
}

public class ComplexOrderedCollectionMold : OrderedCollectionMold<ComplexOrderedCollectionMold>
{
    private TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold>? logOnlyInternalCollectionField;
    private TypeFields.SelectTypeField<ComplexOrderedCollectionMold>?  logOnlyInternalField;

    public ComplexOrderedCollectionMold InitializeComplexOrderedCollectionBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FieldContentHandling createFormatFlags )
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, master, typeSettings, typeName
                                         , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    public override bool IsComplexType => true;

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        CompAccess = recycler.Borrow<CollectionBuilderCompAccess<ComplexOrderedCollectionMold>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, true);
    }

    public TypeFields.SelectTypeField<ComplexOrderedCollectionMold> LogOnlyField =>
        logOnlyInternalField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFields.SelectTypeField<ComplexOrderedCollectionMold>>().Initialize(CompAccess);

    public TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold> LogOnlyCollectionField =>
        logOnlyInternalCollectionField ??=
            PortableState.Master.Recycler
                         .Borrow<TypeFieldCollection.SelectTypeCollectionField<ComplexOrderedCollectionMold>>().Initialize(CompAccess);

    protected override void InheritedStateReset()
    {
        logOnlyInternalCollectionField?.DecrementRefCount();
        logOnlyInternalCollectionField = null!;
        logOnlyInternalField?.DecrementRefCount();
        logOnlyInternalField = null!;

        CompAccess.DecrementIndent();
        CompAccess = null!;
    }

    public ComplexOrderedCollectionMold AddBaseFieldsStart()
    {
        CompAccess.Master.AddBaseFieldsStart();

        return Me;
    }
}
