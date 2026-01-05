// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType.FixtureScaffolding;

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolArrayAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool, bool[]>
  
{
    public bool[]? OrderedCollectionAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolArrayAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class OrderedFromBoolSpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool, bool[]>
  
{
    public bool[]? OrderedCollectionAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolSpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class OrderedFromBoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool, bool[]>
  
{
    public bool[]? OrderedCollectionAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool>)OrderedCollectionAddFilteredBoolReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer
    : FormattedFilteredCollectionMoldScaffold<bool?, bool?[]>
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool?>)OrderedCollectionAddFilteredNullableBoolReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolListAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<bool, IReadOnlyList<bool>>
  
{
    public IReadOnlyList<bool>? OrderedCollectionAddFilteredBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolListAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? OrderedCollectionAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsStruct
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredBoolEnumerable, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableBoolEnumerable, ElementPredicate, ValueFormatString)
           .Complete();
}

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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredBoolEnumerator, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableBoolEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TBase, TFmt[]>
    where TFmt : ISpanFormattable, TBase
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, TFmt[]>
    where TFmt : ISpanFormattable, TFmtBase
{
    public TFmt[]? OrderedCollectionAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, TFmt[]>
    where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, TFmt[]>
    where TFmt : ISpanFormattable, TFmtBase
{
    public TFmt[]? OrderedCollectionAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmt>)OrderedCollectionAddFilteredSpanFormattableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
    where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmt?>)OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan.AsSpan(), ElementPredicate
                              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered
               ((ReadOnlySpan<TFmtStruct?>)OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IReadOnlyList<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IReadOnlyList<TFmt?>? OrderedCollectionAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredSpanFormattableEnumerable, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredSpanFormattableEnumerator, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]>
    where TCloaked : TFilterBase, TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerArray, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerArray, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsAnyNonNullable
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase> :
    RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerSpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsAnyNullableClass
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]>
    where TCloaked : class?, TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerNullableSpan.AsSpan()
                         , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerSpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsAnyNonNullable
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase> :
    RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsAnyNullableClass
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer
    <TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]>
    where TCloaked : class?, TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IReadOnlyList<TCloaked>>
    where TCloaked : TRevealBase, TFilterBase
    where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerList, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerList, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase, TFilterBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredCloakedBearerEnumerable, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase, TFilterBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredCloakedBearerEnumerator, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator, ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FilteredCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsTypeNonNullable
                | AcceptsStringBearer)]
public class OrderedFromStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, TBearer[]> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? OrderedCollectionRevealFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass
                | AcceptsStringBearer)]
public class OrderedFromStringBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerNullableSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FilteredCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsTypeNonNullable
                | AcceptsStringBearer)]
public class OrderedFromStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, TBearer[]> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? OrderedCollectionRevealFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearer>)OrderedCollectionRevealFilteredStringBearerReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered
               ((ReadOnlySpan<TBearer?>)OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FilteredCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered
               ((ReadOnlySpan<TBearerStruct?>)OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, IReadOnlyList<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? OrderedCollectionRevealFilteredStringBearerList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FilteredCollectionMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FilteredCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerable);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableStringBearerEnumerable, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredStringBearerEnumerator, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FilteredEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerator);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableStringBearerEnumerator, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate
                | AcceptsClass | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringArrayAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<string?, string?[]>
  

{
    public string?[]? OrderedCollectionAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringSpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<string, string[]>
  

{
    public string[]? OrderedCollectionAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<string?, string?[]>
  

{
    public string?[]? OrderedCollectionAddFilteredStringNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(OrderedCollectionAddFilteredStringNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<string, string[]>
  

{
    public string[]? OrderedCollectionAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<string>)OrderedCollectionAddFilteredStringReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsString |  AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<string?, string?[]>
  

{
    public string?[]? OrderedCollectionAddFilteredStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable
               ((ReadOnlySpan<string?>)OrderedCollectionAddFilteredStringNullableReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringListAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<string?, IReadOnlyList<string?>>
  

{
    public IReadOnlyList<string?>? OrderedCollectionAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringList, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringEnumerable, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate 
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, TCharSeq[]>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase?, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public TCharSeq[]? OrderedCollectionAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsCharSequence | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, TCharSeq[]>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, TCharSeq[]>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq[]? OrderedCollectionAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq
               ((ReadOnlySpan<TCharSeq>)OrderedCollectionAddFilteredCharSequenceReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsCharSequence | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, TCharSeq[]>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq
               ((ReadOnlySpan<TCharSeq?>)OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IReadOnlyList<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public IReadOnlyList<TCharSeq?>? OrderedCollectionAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceList, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(OrderedCollectionAddFilteredCharSequenceEnumerable, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerator<TCharSeq?>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(OrderedCollectionAddFilteredCharSequenceEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder, StringBuilder[]>

{
    public StringBuilder[]? OrderedCollectionAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(OrderedCollectionAddFilteredStringBuilderNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder, StringBuilder[]>

{
    public StringBuilder[]? OrderedCollectionAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered
               ((ReadOnlySpan<StringBuilder>)OrderedCollectionAddFilteredStringBuilderReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable
               ((ReadOnlySpan<StringBuilder?>)OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>

{
    public IReadOnlyList<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderList, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringBuilderEnumerable, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringBuilderEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchArrayAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | SupportsValueFormatString
                | AcceptsOnlyNonNullableGeneric | FilterPredicate)]
public class OrderedFromMatchSpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, TAny[]>
   where TAny : TAnyFilterBase
{
    public TAny[]? OrderedCollectionAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | SupportsValueFormatString
                | AcceptsOnlyNonNullableGeneric | FilterPredicate)]
public class OrderedFromMatchReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny[]? OrderedCollectionAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch((ReadOnlySpan<TAny>)OrderedCollectionAddFilteredMatchReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch((ReadOnlySpan<TAny?>)OrderedCollectionAddFilteredMatchNullableReadOnlySpan.AsSpan(), ElementPredicate
                                   , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchListAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IReadOnlyList<TAny>>
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny?>? OrderedCollectionAddFilteredMatchList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerable<TAny?>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(OrderedCollectionAddFilteredMatchEnumerable, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny?>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(OrderedCollectionAddFilteredMatchEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | SupportsValueFormatString
                | AcceptsNullableObject | FilterPredicate)]
public class OrderedFromObjectArrayAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, object?[]>
{
    public object?[]? OrderedCollectionAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | SupportsValueFormatString
                | AcceptsNonNullableObject | FilterPredicate)]
public class OrderedFromObjectSpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<object, object[]>
  

{
    public object[]? OrderedCollectionAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | SupportsValueFormatString
                | AcceptsNullableObject | FilterPredicate)]
public class OrderedFromObjectNullableSpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, object?[]>
{
    public object?[]? OrderedCollectionAddFilteredObjectNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable(OrderedCollectionAddFilteredObjectNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | SupportsValueFormatString
                | AcceptsNonNullableObject | FilterPredicate)]
public class OrderedFromObjectReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : FormattedFilteredCollectionMoldScaffold<object, object[]>
  

{
    public object[]? OrderedCollectionAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject((ReadOnlySpan<object>)OrderedCollectionAddFilteredObjectReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableObject
                | SupportsValueFormatString | FilterPredicate)]
public class OrderedFromObjectNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, object?[]>
{
    public object?[]? OrderedCollectionAddFilteredObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable((ReadOnlySpan<object?>)OrderedCollectionAddFilteredObjectNullableReadOnlySpan.AsSpan(), ElementPredicate
                                    , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectListAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, IReadOnlyList<object?>>
{
    public IReadOnlyList<object?>? OrderedCollectionAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectList, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(OrderedCollectionAddFilteredObjectEnumerable, ElementPredicate, ValueFormatString)
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


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(OrderedCollectionAddFilteredObjectEnumerator, ElementPredicate, ValueFormatString)
           .Complete();
}
