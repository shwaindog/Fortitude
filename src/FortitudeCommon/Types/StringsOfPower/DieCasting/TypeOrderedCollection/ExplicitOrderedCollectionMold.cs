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
    
    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(TElement element)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItem(element, ++elementCount, CompAsOrderedCollection.Sb);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement? element, string? formatString = null)
        where TFmtElement : TElement, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString ?? "");
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TStructFmtElement>(TStructFmtElement? element, string? formatString = null)
        where TStructFmtElement : struct, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(element, ++elementCount, CompAsOrderedCollection.Sb, formatString ?? "");
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloaked, TCloakedBase>(TCloaked? element
      , PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        palantírReveal(element, CompAsOrderedCollection.Master);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloakedStruct>(TCloakedStruct? element
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element is null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        palantírReveal(element.Value, CompAsOrderedCollection.Master);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearer>(TBearer? element)
        where TBearer : IStringBearer, TElement
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Master, element, ++elementCount);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearerStruct>(TBearerStruct? element)
        where TBearerStruct : struct, TElement, IStringBearer
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Master, element, ++elementCount);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(string? element, string? formatString)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++elementCount, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq? element, string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++elementCount, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++elementCount, formatString);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElementAndGoToNextElement(TElement? element, string? formatString = null)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.Sb.Append(CompAsOrderedCollection.Settings.NullStyle);
                ++elementCount;
                return AppendNextCollectionItemSeparator();
            }
        }
        if (formatString is not null)
            CompAsOrderedCollection.AppendFormattedCollectionItemMatchOrNull(element, ++elementCount, formatString);
        else
            CompAsOrderedCollection.AppendCollectionItemMatchOrNull(element, ++elementCount);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AppendNextCollectionItemSeparator()
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.AddCollectionElementSeparator(CompAsOrderedCollection.Sb, TypeOfElement, elementCount);
        return this;
    }

    public StateExtractStringRange AppendCollectionComplete() => Complete();
}
