using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public class ExplicitOrderedCollectionBuilder<TElement> : OrderedCollectionBuilder<ExplicitOrderedCollectionBuilder<TElement>>
{
    public override bool IsComplexType => CompAsOrderedCollection.CollectionInComplexType;
    protected static readonly Type TypeOfElement = typeof(TElement);

    private int elementCount = -1;

    public ExplicitOrderedCollectionBuilder<TElement> InitializeExplicitOrderedCollectionBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeOrderedCollectionBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElement(TElement element)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
         CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElement<TFmtElement>(TFmtElement element, string? formatString = null)
        where TFmtElement : TElement, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (formatString is not null)
            CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString);
        else
            CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElement<TStructFmtElement>(TStructFmtElement? element, string? formatString = null)
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

    public ExplicitOrderedCollectionBuilder<TElement> AddElement<TCust, TCustBase>(TCust element, CustomTypeStyler<TCustBase> customTypeStyler)
        where TCust : TCustBase
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        customTypeStyler(element, CompAsOrderedCollection.OwningAppender);
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElement(string? element, string? formatString = null)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddCharSequenceElement<TCharSeq>(TCharSeq? element, string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElement(StringBuilder? element, string? formatString)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount, formatString);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddStyledElement<TStyled>(TStyled element) where TStyled : TElement, IStyledToStringObject
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection, element, ++elementCount);
        ++elementCount;
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddMatchElement(TElement element, string? formatString = null)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (formatString is not null)
            CompAsOrderedCollection.AppendFormattedCollectionItemMatchOrNull(element, ++elementCount, formatString);
        else
            CompAsOrderedCollection.AppendCollectionItemMatchOrNull(element, ++elementCount);
        return this;
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement(TElement element)
    {
        AddElement(element);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement element, string? formatString = null)
        where TFmtElement : TElement, ISpanFormattable
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement<TStructFmtElement>(TStructFmtElement? element, string? formatString = null)
        where TStructFmtElement : struct, ISpanFormattable
    {
        if (formatString is not null)
            CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString);
        else
            CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement<TCust, TCustBase>(TCust element
      , CustomTypeStyler<TCustBase> customTypeStyler)
        where TCust : TCustBase
    {
        AddElement(element, customTypeStyler);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement(string? element, string? formatString)
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq? element, string? formatString = null)
        where TCharSeq : ICharSequence
    {
        AddCharSequenceElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString)
    {
        AddElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddStyledElementAndGoToNextElement<TStyled>(TStyled element)
        where TStyled : TElement, IStyledToStringObject
    {
        AddStyledElement(element);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AddMatchElementAndGoToNextElement(TElement element, string? formatString = null)
    {
        AddMatchElement(element, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionBuilder<TElement> AppendNextCollectionItemSeparator()
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.GoToNextCollectionItemStart(TypeOfElement, elementCount);
        return this;
    }

    public StyledTypeBuildResult AppendCollectionComplete() => Complete();
}
