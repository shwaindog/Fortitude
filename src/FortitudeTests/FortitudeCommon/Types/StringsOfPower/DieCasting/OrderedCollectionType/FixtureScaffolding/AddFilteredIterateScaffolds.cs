// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType.FixtureScaffolding;

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? OrderedCollectionAddFilteredBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateBool
               (OrderedCollectionAddFilteredBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
{
    public IEnumerator<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerator<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate<IEnumerator<TFmt>?, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<IEnumerator<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerator<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<IEnumerator<TBearer>?, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<string?, IEnumerator<string?>>
{
    public IEnumerator<string?>? OrderedCollectionAddFilteredStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate
               (OrderedCollectionAddFilteredStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public IEnumerator<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateCharSeq<IEnumerator<TCharSeq>?, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateMatch<IEnumerator<TAny>?, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<object?, IEnumerator<object?>>
{
    public IEnumerator<object?>? OrderedCollectionAddFilteredObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateObject
               (OrderedCollectionAddFilteredObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}
