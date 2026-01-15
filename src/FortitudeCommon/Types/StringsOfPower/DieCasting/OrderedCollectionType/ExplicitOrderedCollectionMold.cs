using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class ExplicitOrderedCollectionMold<TElement> : OrderedCollectionType.OrderedCollectionMold<ExplicitOrderedCollectionMold<TElement>>
{
    public override bool IsComplexType => false;
    
    protected static readonly Type TypeOfElement = typeof(TElement);

    public ExplicitOrderedCollectionMold<TElement> InitializeExplicitOrderedCollectionBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        InitializeOrderedCollectionBuilder(instanceOrContainer, typeBeingBuilt, master, typeSettings, typeName
                                         , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString ?? "", formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ResultCount++;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtElement : TElement?, ISpanFormattable?
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtStructElement>(TFmtStructElement? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStructElement : struct, ISpanFormattable
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloaked, TRevealBase>(TCloaked? element
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, "", formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Master, element, ++ResultCount
                                                                      , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloakedStruct>(TCloakedStruct? element
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element is null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, "", formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollection.Master, element.Value, ++ResultCount
                                                       , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearer>(TBearer element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TElement?
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, "", formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter
                               .CollectionNextStringBearerFormat(CompAsOrderedCollection.Master, element, ++ResultCount
                                                               , formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearerStruct>(TBearerStruct? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, TElement, IStringBearer
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, "", formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter
                               .CollectionNextStringBearerFormat(CompAsOrderedCollection.Master, element.Value
                                                               , ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(string? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextCharSeqFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollection.StyleTypeBuilder;
        }
        CompAsOrderedCollection.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollection.Sb, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElementAndGoToNextElement(TElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        if (element == null)
        {
            if (CompAsOrderedCollection.Settings.NullWritesNullString)
            {
                CompAsOrderedCollection.StyleFormatter.AppendFormattedNull(CompAsOrderedCollection.Sb, formatString, formatFlags);
                ++ResultCount;
                return AppendNextCollectionItemSeparator();
            }
        }
        CompAsOrderedCollection.AppendFormattedCollectionItemMatchOrNull(element, ++ResultCount, formatString ?? "",  formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AppendNextCollectionItemSeparator()
    {
        if (CompAsOrderedCollection.SkipBody) return this;
        CompAsOrderedCollection.StyleFormatter.AddCollectionElementSeparatorAndPadding(CompAsOrderedCollection, TypeOfElement, ResultCount);
        return this;
    }

    public StateExtractStringRange AppendCollectionComplete() => Complete();

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionBuilderCompAccess<ExplicitOrderedCollectionMold<TElement>>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, false);
    }
    
    protected override CollectionBuilderCompAccess<ExplicitOrderedCollectionMold<TElement>> CompAsOrderedCollection =>  
        (CollectionBuilderCompAccess<ExplicitOrderedCollectionMold<TElement>>)MoldStateField!;
}
