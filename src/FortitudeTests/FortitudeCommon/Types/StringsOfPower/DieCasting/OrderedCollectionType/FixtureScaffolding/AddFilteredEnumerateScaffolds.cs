// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType.FixtureScaffolding;

[TypeGeneratePart(  IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class OrderedFromBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? OrderedCollectionAddFilteredBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateBool
               (OrderedCollectionAddFilteredBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer
    : FormattedFilteredCollectionMoldScaffold<bool?, IEnumerable<bool?>>
{
    public IEnumerable<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerable<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate<IEnumerable<TFmt>?, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<IEnumerable<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<IEnumerable<TBearer>?, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<string?, IEnumerable<string?>>

{
    public IEnumerable<string?>? OrderedCollectionAddFilteredStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate
               (OrderedCollectionAddFilteredStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerable<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateCharSeq<IEnumerable<TCharSeq>?, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase?
{
    public IEnumerable<TAny>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateMatch<IEnumerable<TAny>?, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, IEnumerable<object?>>
{
    public IEnumerable<object?>? OrderedCollectionAddFilteredObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateObject
               (OrderedCollectionAddFilteredObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

