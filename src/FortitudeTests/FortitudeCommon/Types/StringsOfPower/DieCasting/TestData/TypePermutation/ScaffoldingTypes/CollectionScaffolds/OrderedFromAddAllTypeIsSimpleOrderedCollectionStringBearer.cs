// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.CollectionScaffolds;

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
{
    public bool[]? OrderedCollectionAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? OrderedCollectionAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
  
{
    public bool[]? OrderedCollectionAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? OrderedCollectionAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
  
{
    public bool[]? OrderedCollectionAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool>)OrderedCollectionAddAllBoolReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? OrderedCollectionAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool?>)OrderedCollectionAddAllNullableBoolReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, IReadOnlyList<bool>>
  
{
    public IReadOnlyList<bool>? OrderedCollectionAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? OrderedCollectionAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? OrderedCollectionAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? OrderedCollectionAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? OrderedCollectionAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? OrderedCollectionAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNonNullableSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNonNullableSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsOnlyNullableClassSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddAllSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllSpanFormattableNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNonNullableSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt>)OrderedCollectionAddAllSpanFormattableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsOnlyNullableClassSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<TFmt?>)OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmtStruct?>)OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt?, IReadOnlyList<TFmt?>>
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? OrderedCollectionAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt?, IEnumerable<TFmt?>>
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedEnumeratorMoldScaffold<TFmt?, IEnumerator<TFmt?>>
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt?>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStruct | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerArray, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerArray, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyNonNullable
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, TCloaked[]>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]>
    where TCloaked : class?, TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerNullableSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyNonNullable
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, TCloaked[]>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked>)OrderedCollectionRevealAllCloakedBearerReadOnlySpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked?>)OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloakedStruct?>)OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, IReadOnlyList<TCloaked?>>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked?>? OrderedCollectionRevealAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, IEnumerable<TCloaked?>>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked?>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerEnumeratorMoldScaffold<TCloaked?, TRevealBase, IEnumerator<TCloaked?>>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked?>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class OrderedFromStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : CollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerArray)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : CollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerArray)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsTypeNonNullable | AcceptsStringBearer)]
public class OrderedFromStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : CollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass
                | AcceptsStringBearer)]
public class OrderedFromStringBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : CollectionMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerNullableSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : CollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsTypeNonNullable
                | AcceptsStringBearer)]
public class OrderedFromStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : CollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer>)OrderedCollectionRevealAllStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : CollectionMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer?>)OrderedCollectionRevealAllStringBearerNullableReadOnlySpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct |
                  AcceptsStringBearer)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : CollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearerStruct?>)OrderedCollectionRevealAllNullableStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class OrderedFromStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearer> : CollectionMoldScaffold<TBearer?,
    IReadOnlyList<TBearer?>>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer?>? OrderedCollectionRevealAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerList)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : CollectionMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerList)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : CollectionMoldScaffold<TBearer?,
    IEnumerable<TBearer?>>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer?>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : CollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> :
    EnumeratorMoldScaffold<TBearer?, IEnumerator<TBearer?>> where TBearer : IStringBearer
{
    public IEnumerator<TBearer?>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : EnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
  
{
    public string?[]? OrderedCollectionAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string, string[]>
  
{
    public string[]? OrderedCollectionAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
  
{
    public string?[]? OrderedCollectionAddAllStringNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string, string[]>
  
{
    public string[]? OrderedCollectionAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<string>)OrderedCollectionAddAllStringReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsString 
                 | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
  

{
    public string?[]? OrderedCollectionAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<string?>)OrderedCollectionAddAllStringNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, IReadOnlyList<string?>>
  

{
    public IReadOnlyList<string?>? OrderedCollectionAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsString 
                 | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, IEnumerable<string?>>
  

{
    public IEnumerable<string?>? OrderedCollectionAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<string?, IEnumerator<string?>>
  

{
    public IEnumerator<string?>? OrderedCollectionAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : FormattedCollectionMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsCharSequence
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : FormattedCollectionMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsCharSequence
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableSpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> :
    FormattedCollectionMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable(OrderedCollectionAddAllCharSequenceNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsCharSequence
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> :
    FormattedCollectionMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq((ReadOnlySpan<TCharSeq>)OrderedCollectionAddAllCharSequenceReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan
                | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq?, TCharSeq?[]> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable((ReadOnlySpan<TCharSeq?>)OrderedCollectionAddAllCharSequenceNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq?, IReadOnlyList<TCharSeq?>> where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? OrderedCollectionAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq?, IEnumerable<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(OrderedCollectionAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedEnumeratorMoldScaffold<TCharSeq?, IEnumerator<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(OrderedCollectionAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<StringBuilder, StringBuilder[]>
  

{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringBuilderNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder, StringBuilder[]>

{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<StringBuilder>)OrderedCollectionAddAllStringBuilderReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>

{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<StringBuilder?>)OrderedCollectionAddAllStringBuilderNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>

{
    public IReadOnlyList<StringBuilder?>? OrderedCollectionAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddAllSimpleOrderedCollectionStringBearer :
    FormattedEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>

{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchArrayAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
  
{
    public TAny?[]? OrderedCollectionAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan
                | AcceptsOnlyNonNullableGeneric | SupportsValueFormatString)]
public class OrderedFromMatchSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, TAny[]>
  
{
    public TAny[]? OrderedCollectionAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchNullableSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
  
{
    public TAny?[]? OrderedCollectionAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable(OrderedCollectionAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan
                | AcceptsOnlyNonNullableGeneric | SupportsValueFormatString)]
public class OrderedFromMatchReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, TAny[]>
  
{
    public TAny[]? OrderedCollectionAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<TAny>)OrderedCollectionAddAllMatchReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class OrderedFromMatchNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
  

{
    public TAny?[]? OrderedCollectionAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable((ReadOnlySpan<TAny?>)OrderedCollectionAddAllMatchNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchListAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, IReadOnlyList<TAny?>>
  

{
    public IReadOnlyList<TAny?>? OrderedCollectionAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, IEnumerable<TAny?>>
  

{
    public IEnumerable<TAny?>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(OrderedCollectionAddAllMatchEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedEnumeratorMoldScaffold<TAny?, IEnumerator<TAny?>>
  

{
    public IEnumerator<TAny?>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(OrderedCollectionAddAllMatchEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>
  
{
    public object?[]? OrderedCollectionAddAllObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object, object[]>
  
{
    public object[]? OrderedCollectionAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectNullableSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>
  
{
    public object?[]? OrderedCollectionAddAllObjectNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable(OrderedCollectionAddAllObjectNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object, object[]>
  
{
    public object[]? OrderedCollectionAddAllObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject((ReadOnlySpan<object>)OrderedCollectionAddAllObjectReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>
  

{
    public object?[]? OrderedCollectionAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable((ReadOnlySpan<object?>)OrderedCollectionAddAllObjectNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, IReadOnlyList<object?>>
  

{
    public IReadOnlyList<object?>? OrderedCollectionAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, IEnumerable<object?>>
  

{
    public IEnumerable<object?>? OrderedCollectionAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(OrderedCollectionAddAllObjectEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(OrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<object?, IEnumerator<object?>>
  

{
    public IEnumerator<object?>? OrderedCollectionAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(OrderedCollectionAddAllObjectEnumerator, ValueFormatString)
           .Complete();
}
