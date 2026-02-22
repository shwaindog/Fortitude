using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class ExplicitOrderedCollectionMold<TElement> : OrderedCollectionMold<ExplicitOrderedCollectionMold<TElement>>
{
    public override bool IsComplexType => false;
    private bool hasInitialized;
    
    protected static readonly Type TypeOfElement = typeof(TElement);

    public ExplicitOrderedCollectionMold<TElement> InitializeExplicitOrderedCollectionBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeOrderedCollectionBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                                         , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags, typeof(TElement));

        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(bool), true, formatFlags);
        }
        ResultCount++;
        CompAsOrderedCollectionMold.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString ?? "", formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(bool?), true, formatFlags);
        }
        ResultCount++;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtElement : TElement?, ISpanFormattable?
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TFmtElement), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtStructElement>(TFmtStructElement? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStructElement : struct, ISpanFormattable
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TFmtStructElement?), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloaked, TRevealBase>(TCloaked? element
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TCloaked), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount
                                                                      , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloakedStruct>(TCloakedStruct? element
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TCloakedStruct?), true, formatFlags);
        }
        ++ResultCount;
        if (element is null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter
                               .CollectionNextItemFormat(CompAsOrderedCollectionMold, element.Value, ++ResultCount
                                                       , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearer>(TBearer element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TElement?
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TBearer), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter
                               .CollectionNextStringBearerFormat(CompAsOrderedCollectionMold, element, ++ResultCount
                                                               , formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearerStruct>(TBearerStruct? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, TElement, IStringBearer
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TBearerStruct?), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter
                               .CollectionNextStringBearerFormat(CompAsOrderedCollectionMold, element.Value
                                                               , ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(string? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(string), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TCharSeq), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextCharSeqFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(StringBuilder), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return CompAsOrderedCollectionMold.Mold;
        }
        CompAsOrderedCollectionMold.StyleFormatter.CollectionNextItemFormat(CompAsOrderedCollectionMold, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElementAndGoToNextElement(TElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            CompAsOrderedCollectionMold.ConditionalCollectionPrefix(CompAsOrderedCollectionMold.InstanceOrType, typeof(TElement), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (CompAsOrderedCollectionMold.Settings.NullWritesNullString)
            {
                CompAsOrderedCollectionMold.StyleFormatter.AppendFormattedNull(CompAsOrderedCollectionMold.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
        }
        CompAsOrderedCollectionMold.AppendFormattedCollectionItemMatchOrNull(element, ++ResultCount, formatString ?? "",  formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AppendNextCollectionItemSeparator()
    {
        if (CompAsOrderedCollectionMold.SkipBody) return this;
        CompAsOrderedCollectionMold.StyleFormatter.AddCollectionElementSeparatorAndPadding(CompAsOrderedCollectionMold, TypeOfElement, ResultCount);
        return this;
    }
    
    
    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        base.FinishTypeOpening(usingFormatter, formatFlags);
        // usingFormatter.AppendOpenCollection(State, TypeOfElement, true, formatFlags);
    }
    
    public override void AppendClosing()
    {
        State.StyleFormatter.AppendCloseCollection(State, ResultCount, TypeOfElement, TotalCount, "", State.CreateMoldFormatFlags);
        base.AppendClosing();
    }

    public AppendSummary AppendCollectionComplete() => Complete();

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
    }
    
    protected override CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>> CompAsOrderedCollectionMold =>  
        (CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>>)MoldStateField!;
}
