// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType.FixtureScaffolding;

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
{
    public bool[]? OrderedCollectionAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllBoolArray?.GetType() ?? typeof(bool[]);
    public override string PropertyName => nameof(OrderedCollectionAddAllBoolArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableBoolArray?.GetType() ?? typeof(bool?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
{
    public bool[]? OrderedCollectionAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }


    public override Type OrderedCollectionType => typeof(Span<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, bool[]>
{
    public bool[]? OrderedCollectionAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }


    public override Type OrderedCollectionType => typeof(ReadOnlySpan<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool>)OrderedCollectionAddAllBoolReadOnlySpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, bool?[]>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }


    public override Type OrderedCollectionType => typeof(ReadOnlySpan<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool?>)OrderedCollectionAddAllNullableBoolReadOnlySpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, IReadOnlyList<bool>>
{
    public IReadOnlyList<bool>? OrderedCollectionAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllBoolList?.GetType() ?? typeof(IReadOnlyList<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, IReadOnlyList<bool?>>
{
    public IReadOnlyList<bool?>? OrderedCollectionAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableBoolList?.GetType() ?? typeof(IReadOnlyList<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsNonNullableSpanFormattable
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

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableArray?.GetType() ?? typeof(TFmt[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsOnlyNullableStructSpanFormattable
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

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableArray?.GetType() ?? typeof(TFmtStruct?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsSpanFormattableExceptNullableStruct
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

    public override Type OrderedCollectionType => typeof(Span<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsOnlyNullableClassSpanFormattable
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

    public override Type OrderedCollectionType => typeof(Span<TFmt?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableNullableSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsOnlyNullableStructSpanFormattable
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

    public override Type OrderedCollectionType => typeof(Span<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsSpanFormattableExceptNullableStruct
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

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt>)OrderedCollectionAddAllSpanFormattableReadOnlySpan
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsOnlyNullableClassSpanFormattable
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

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TFmt?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt?>)OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsOnlyNullableStructSpanFormattable
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

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmtStruct?>)OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsSpanFormattableExceptNullableStruct
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

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableList?.GetType() ?? typeof(IReadOnlyList<TFmt?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsOnlyNullableStructSpanFormattable
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

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableList?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStruct | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerArray?.GetType() ?? typeof(TCloaked?[]);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerArray
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsAnyNullableStruct | SupportsValueRevealer
                | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableCloakedBearerArray?.GetType() ?? typeof(TCloakedStruct?[]);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerArray
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyNonNullable
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerSpan.AsSpan()
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TCloaked?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerNullableSpan.AsSpan()
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerSpan.AsSpan()
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyNonNullable
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked>)OrderedCollectionRevealAllCloakedBearerReadOnlySpan.AsSpan()
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TCloaked?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked?>)OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan.AsSpan()
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloakedStruct?>)OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked?, TRevealBase, IReadOnlyList<TCloaked?>?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked?>? OrderedCollectionRevealAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerList?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerList
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableCloakedBearerList?.GetType() 
                                               ?? typeof(IReadOnlyList<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerList
                    , ValueRevealer
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : FormattedCollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerArray?.GetType() ?? typeof(TBearer[]);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerArray
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerArray?.GetType() ?? typeof(TBearerStruct?[]);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerArray
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsTypeNonNullable | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : FormattedCollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerSpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : FormattedCollectionMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TBearer?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerNullableSpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerSpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsTypeNonNullable
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : FormattedCollectionMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer>)OrderedCollectionRevealAllStringBearerReadOnlySpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>
    : FormattedCollectionMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TBearer?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer?>)OrderedCollectionRevealAllStringBearerNullableReadOnlySpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct |
                  AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearerStruct?>)OrderedCollectionRevealAllNullableStringBearerReadOnlySpan.AsSpan()
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearer> : FormattedCollectionMoldScaffold<TBearer?,
    IReadOnlyList<TBearer?>>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer?>? OrderedCollectionRevealAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerList?.GetType() ?? typeof(IReadOnlyList<TBearer?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerList
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerList?.GetType() 
                                               ?? typeof(IReadOnlyList<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerList
                    , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
{
    public string?[]? OrderedCollectionAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringArray?.GetType() 
                                               ?? typeof(string?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string, string[]>
{
    public string[]? OrderedCollectionAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<string>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
{
    public string?[]? OrderedCollectionAddAllStringNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringNullableSpan.AsSpan()
                         , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string, string[]>
{
    public string[]? OrderedCollectionAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<string>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<string>)OrderedCollectionAddAllStringReadOnlySpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, string?[]>
{
    public string?[]? OrderedCollectionAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<string?>)OrderedCollectionAddAllStringNullableReadOnlySpan.AsSpan()
                         , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, IReadOnlyList<string?>>
{
    public IReadOnlyList<string?>? OrderedCollectionAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringList?.GetType() ?? typeof(IReadOnlyList<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : FormattedCollectionMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceArray?.GetType() ?? typeof(TCharSeq?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceArray, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : FormattedCollectionMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceSpan.AsSpan()
                        , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> :
    FormattedCollectionMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq((ReadOnlySpan<TCharSeq>)OrderedCollectionAddAllCharSequenceReadOnlySpan.AsSpan()
                        , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq?, IReadOnlyList<TCharSeq?>> where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? OrderedCollectionAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceList?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceList
                        , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringBuilderArray?.GetType() ?? typeof(StringBuilder?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderArray
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<StringBuilder>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderSpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringBuilderNullableSpan.AsSpan()
                         , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<StringBuilder>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<StringBuilder>)OrderedCollectionAddAllStringBuilderReadOnlySpan.AsSpan()
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<StringBuilder?>)OrderedCollectionAddAllStringBuilderNullableReadOnlySpan.AsSpan()
                         , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
{
    public IReadOnlyList<StringBuilder?>? OrderedCollectionAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringBuilderList?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderList
                 , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchArrayAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
{
    public TAny?[]? OrderedCollectionAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchArray?.GetType() ?? typeof(TAny?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchArray
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan
                | AcceptsOnlyNonNullableGeneric | SupportsValueFormatString)]
public class OrderedFromMatchSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, TAny[]>
{
    public TAny[]? OrderedCollectionAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchSpan.AsSpan()
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchNullableSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
{
    public TAny?[]? OrderedCollectionAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<TAny?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchNullableSpan.AsSpan()
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan
                | AcceptsOnlyNonNullableGeneric | SupportsValueFormatString)]
public class OrderedFromMatchReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, TAny[]>
{
    public TAny[]? OrderedCollectionAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<TAny>)OrderedCollectionAddAllMatchReadOnlySpan.AsSpan()
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class OrderedFromMatchNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, TAny?[]>
{
    public TAny?[]? OrderedCollectionAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<TAny?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<TAny?>)OrderedCollectionAddAllMatchNullableReadOnlySpan.AsSpan()
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchListAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny?, IReadOnlyList<TAny?>>
{
    public IReadOnlyList<TAny?>? OrderedCollectionAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchList?.GetType() ?? typeof(IReadOnlyList<TAny?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchList
                      , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectArrayAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>
{
    public object?[]? OrderedCollectionAddAllObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllObjectArray?.GetType() ?? typeof(object?[]);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectArray
                       , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object, object[]>
{
    public object[]? OrderedCollectionAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<object>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectSpan.AsSpan()
                       , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsSpan | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectNullableSpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>

{
    public object?[]? OrderedCollectionAddAllObjectNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(Span<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable(OrderedCollectionAddAllObjectNullableSpan.AsSpan()
                               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object, object[]>

{
    public object[]? OrderedCollectionAddAllObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<object>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject((ReadOnlySpan<object>)OrderedCollectionAddAllObjectReadOnlySpan.AsSpan()
                       , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableObject
                | SupportsValueFormatString)]
public class OrderedFromObjectNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, object?[]>


{
    public object?[]? OrderedCollectionAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => typeof(ReadOnlySpan<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable((ReadOnlySpan<object?>)OrderedCollectionAddAllObjectNullableReadOnlySpan.AsSpan()
                               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsList | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectListAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, IReadOnlyList<object?>>


{
    public IReadOnlyList<object?>? OrderedCollectionAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllObjectList?.GetType() ?? typeof(IReadOnlyList<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectList
                       , ValueFormatString, FormattingFlags)
           .Complete();
}
