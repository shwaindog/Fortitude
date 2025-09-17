using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public class ExplicitOrderedCollectionMold<TElement> : OrderedCollectionMold<ExplicitOrderedCollectionMold<TElement>>
{
    public override bool IsComplexType => CompAsOrderedCollection.CollectionInComplexType;
    protected static readonly Type TypeOfElement = typeof(TElement);

    private int elementCount = -1;

    public ExplicitOrderedCollectionMold<TElement> InitializeExplicitOrderedCollectionBuilder(
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

    public ExplicitOrderedCollectionMold<TElement> AddElement(TElement element)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
         CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElement<TFmtElement>(TFmtElement element, string? formatString = null)
        where TFmtElement : TElement, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (formatString is not null)
            CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString);
        else
            CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElement<TStructFmtElement>(TStructFmtElement? element, string? formatString = null)
        where TStructFmtElement : struct, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
            return this;
        }
        if (formatString is not null)
            CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString);
        else
            CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElement<TCust, TCustBase>(TCust element, StringBearerRevealState<TCustBase> stringBearerRevealState)
        where TCust : TCustBase
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        stringBearerRevealState(element, CompAsOrderedCollection.Master);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElement(string? element, string? formatString = null)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElement<TCharSeq>(TCharSeq? element, string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElement(StringBuilder? element, string? formatString)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddStyledElement<TStyled>(TStyled element) where TStyled : TElement, IStringBearer
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElement(TElement element, string? formatString = null)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (formatString is not null)
            CompAsOrderedCollection.AppendFormattedCollectionItemMatchOrNull(element, ++elementCount, formatString);
        else
            CompAsOrderedCollection.AppendCollectionItemMatchOrNull(element, ++elementCount);
        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(TElement element)
    {
        AddElement(element);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement element, string? formatString = null)
        where TFmtElement : TElement, ISpanFormattable
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TStructFmtElement>(TStructFmtElement? element, string? formatString = null)
        where TStructFmtElement : struct, ISpanFormattable
    {
        if (formatString is not null)
            CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString);
        else
            CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCust, TCustBase>(TCust element
      , StringBearerRevealState<TCustBase> stringBearerRevealState)
        where TCust : TCustBase
    {
        AddElement(element, stringBearerRevealState);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(string? element, string? formatString)
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq? element, string? formatString = null)
        where TCharSeq : ICharSequence
    {
        AddCharSequenceElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString)
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddStyledElementAndGoToNextElement<TStyled>(TStyled element)
        where TStyled : TElement, IStringBearer
    {
        AddStyledElement(element);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElementAndGoToNextElement(TElement element, string? formatString = null)
    {
        AddMatchElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AppendNextCollectionItemSeparator()
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.AddCollectionElementSeparator(CompAsOrderedCollection, TypeOfElement, elementCount);
        return this;
    }

    public StateExtractStringRange AppendCollectionComplete() => Complete();
}
