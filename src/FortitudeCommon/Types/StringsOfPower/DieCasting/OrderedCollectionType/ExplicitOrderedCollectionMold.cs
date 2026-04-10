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
    
    public override MoldType MoldType => MoldType.SimpleOrderedCollectionMold;

    public ExplicitOrderedCollectionMold<TElement> InitializeExplicitOrderedCollectionBuilder(
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType  
      , CallerContext callerContext  
      , CreateContext createContext )
    {
        InitializeOrderedCollectionBuilder(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                                         , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext, createContext);

        return this;
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(bool), true, formatFlags);
        }
        ResultCount++;
        WriteStateAsCollectionMoldWriteState.StyleFormatter
                               .CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString ?? "", formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(bool? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(bool?), true, formatFlags);
        }
        ResultCount++;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter
                               .CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtElement>(TFmtElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtElement : TElement?, ISpanFormattable?
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TFmtElement), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TFmtStructElement>(TFmtStructElement? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStructElement : struct, ISpanFormattable
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TFmtStructElement?), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString ?? "", formatFlags | AsCollection);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloaked, TRevealBase>(TCloaked? element
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TCloaked), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount
                                                                      , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement<TCloakedStruct>(TCloakedStruct? element
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TCloakedStruct?), true, formatFlags);
        }
        ++ResultCount;
        if (element is null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter
                               .CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element.Value, ++ResultCount
                                                       , palantírReveal, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearer>(TBearer element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TElement?
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TBearer), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter
                               .CollectionNextStringBearerFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount
                                                               , formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddBearerElementAndGoToNextElement<TBearerStruct>(TBearerStruct? element
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, TElement, IStringBearer
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TBearerStruct?), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, "", formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter
                               .CollectionNextStringBearerFormat(WriteStateAsCollectionMoldWriteState, element.Value
                                                               , ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(string? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(string), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddCharSequenceElementAndGoToNextElement<TCharSeq>(TCharSeq element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence?
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TCharSeq), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextCharSeqFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddElementAndGoToNextElement(StringBuilder? element, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(StringBuilder), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
            return WriteStateAsCollectionMoldWriteState.Mold;
        }
        WriteStateAsCollectionMoldWriteState.StyleFormatter.CollectionNextItemFormat(WriteStateAsCollectionMoldWriteState, element, ++ResultCount, formatString, formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AddMatchElementAndGoToNextElement(TElement? element, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        if (!hasInitialized)
        {
            hasInitialized = true;
            WriteStateAsCollectionMoldWriteState.ConditionalCollectionPrefix(WriteStateAsCollectionMoldWriteState.InstanceOrType, typeof(TElement), true, formatFlags);
        }
        ++ResultCount;
        if (element == null)
        {
            if (WriteStateAsCollectionMoldWriteState.Settings.NullWritesNullString)
            {
                WriteStateAsCollectionMoldWriteState.StyleFormatter.AppendFormattedNull(WriteStateAsCollectionMoldWriteState.Sb, formatString, formatFlags);
                return AppendNextCollectionItemSeparator();
            }
        }
        WriteStateAsCollectionMoldWriteState.AppendFormattedCollectionItemMatchOrNull(element, ++ResultCount, formatString ?? "",  formatFlags);
        return AppendNextCollectionItemSeparator();
    }

    public ExplicitOrderedCollectionMold<TElement> AppendNextCollectionItemSeparator()
    {
        if (WriteStateAsCollectionMoldWriteState.SkipBody) return this;
        WriteStateAsCollectionMoldWriteState.StyleFormatter.AddCollectionElementSeparatorAndPadding(WriteStateAsCollectionMoldWriteState, TypeOfElement, ResultCount);
        return this;
    }
    
    
    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        base.FinishTypeOpening(usingFormatter, formatFlags);
        // usingFormatter.AppendOpenCollection(State, TypeOfElement, true, formatFlags);
    }
    
    public override void AppendClosing(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (State.WroteInnerTypeClose)
        {
            State.StyleFormatter.Gb.Complete(formatFlags);
        }
        else
        {
            State.StyleFormatter.AppendCloseCollection(State, ResultCount, TypeOfElement, TotalCount, "", State.CreateMoldFormatFlags);
            State.StyleFormatter.Gb.Complete(formatFlags);
        }
        base.AppendClosing(formatFlags);
    }
    
    public override AppendSummary Complete(FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (State == null) { throw new NullReferenceException("Expected MoldState to be set"); }
        AppendClosing(State.CreateMoldFormatFlags);
        return RunShutdown();
    }

    public AppendSummary AppendCollectionComplete() => Complete();

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField = recycler.Borrow<CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>>>()
                             .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
    }
    
    protected override CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>> WriteStateAsCollectionMoldWriteState =>  
        (CollectionMoldWriteState<ExplicitOrderedCollectionMold<TElement>>)MoldStateField!;
}
