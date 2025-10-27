using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public partial class OrderedCollectionMold<TOCMold> : KnownTypeMolder<TOCMold>
    where TOCMold : TypeMolder
{
    private CollectionBuilderCompAccess<TOCMold> stb = null!;

    public OrderedCollectionMold<TOCMold> InitializeOrderedCollectionBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        stb = CompAsOrderedCollection;

        return this;
    }
    

    public override bool IsComplexType => false;
    
    public override void AppendOpening()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess.Sb, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionStart(CompAccess.Sb, elementType!, true, CompAccess.TypeBeingBuilt);
        }
    }
    
    public override void AppendClosing()
    {
        if (CompAsOrderedCollection.CollectionInComplexType)
        {
            CompAccess.StyleFormatter.AppendTypeClosing(CompAccess.Sb);
        }
        else
        {
            var elementType = CompAccess.StyleTypeBuilder.TypeBeingBuilt.GetIterableElementType();
            CompAccess.StyleFormatter.FormatCollectionEnd(CompAccess.Sb, elementType!, 1);
        }
    }

    protected CollectionBuilderCompAccess<TOCMold> CompAsOrderedCollection => (CollectionBuilderCompAccess<TOCMold>)CompAccess;
}

public class SimpleOrderedCollectionMold : OrderedCollectionMold<SimpleOrderedCollectionMold>
{
    public SimpleOrderedCollectionMold InitializeSimpleOrderedCollectionBuilder(
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

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
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

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
