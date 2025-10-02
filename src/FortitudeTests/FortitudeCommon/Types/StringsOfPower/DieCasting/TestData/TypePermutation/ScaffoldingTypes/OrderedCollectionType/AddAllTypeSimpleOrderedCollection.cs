// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionType;

public class BoolArrayAddAllSimpleOrderedCollectionStringBearer(bool[]? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolArrayAddAllSimpleOrderedCollectionStringBearer(bool?[]? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolSpanAddAllSimpleOrderedCollectionStringBearer(bool[]? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolSpanAddAllSimpleOrderedCollectionStringBearer(bool[]? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(bool[]? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool>)value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(bool?[]? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool?>)value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolListAddAllSimpleOrderedCollectionStringBearer(List<bool>? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!.GetEnumerator();
}

public class NullableBoolListAddAllSimpleOrderedCollectionStringBearer(List<bool?>? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!.GetEnumerator();
}

public class BoolEnumerableAddAllSimpleOrderedCollectionStringBearer(IEnumerable<bool>? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!.GetEnumerator();
}

public class NullableBoolEnumerableAddAllSimpleOrderedCollectionStringBearer(IEnumerable<bool?>? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!.GetEnumerator();
}

public class BoolEnumeratorAddAllSimpleOrderedCollectionStringBearer(IEnumerator<bool>? value) : IStringBearer, IEnumerable<bool>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!;
}

public class NullableBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer(IEnumerator<bool?>? value) : IStringBearer, IEnumerable<bool?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!;
}

public class SpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (TFmt[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (TFmt[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class SpanFormattableNullableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (TFmt?[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (TFmt[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class SpanFormattableNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (TFmt?[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<TFmt?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmtStruct?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (IReadOnlyList<TFmt?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (IReadOnlyList<TFmtStruct?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class SpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (IEnumerable<TFmt?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (IEnumerable<TFmtStruct?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class SpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    (IEnumerator<TFmt?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => value!;
}

public class NullableSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    (IEnumerator<TFmtStruct?>? value, string? formatString = null) : IStringBearer, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!;
}

public class CloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    (TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    (TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    (TCloaked[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value.AsSpan(), palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class CloakedBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    (TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(value.AsSpan(), palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    (TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value.AsSpan(), palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    (TCloaked[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked>)value.AsSpan(), palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class CloakedBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    (TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TCloaked?>)value.AsSpan(), palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    (TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloakedStruct?>)value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
(
    IReadOnlyList<TCloaked?>? value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
(
    IReadOnlyList<TCloakedStruct?>? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class CloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
(
    IEnumerable<TCloaked?>? value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
(
    IEnumerable<TCloakedStruct?>? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class CloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
(
    IEnumerator<TCloaked?>? value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!;
}

public class NullableCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
(
    IEnumerator<TCloakedStruct?>? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!;
}

public class StringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearer>(TBearer[]? value) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (TBearerStruct?[]? value) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>(TBearer[]? value) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class StringBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>(TBearer?[]? value) : IStringBearer, IEnumerable<TBearer>
    where TBearer : class, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (TBearerStruct?[]? value) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>(TBearer[]? value) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer>)value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class StringBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer>(TBearer?[]? value)
    : IStringBearer, IEnumerable<TBearer> where TBearer : class, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TBearer?>)value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (TBearerStruct?[]? value) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearerStruct?>)value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearer>(IReadOnlyList<TBearer?>? value)
    : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (IReadOnlyList<TBearerStruct?>? value) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer>(IEnumerable<TBearer?>? value)
    : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (IEnumerable<TBearerStruct?>? value) : IStringBearer, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer>(IEnumerator<TBearer?>? value)
    : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => value!;
}

public class NullableStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    (IEnumerator<TBearerStruct?>? value) : IStringBearer, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!;
}

public class StringArrayAddAllSimpleOrderedCollectionStringBearer(string?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringSpanAddAllSimpleOrderedCollectionStringBearer(string[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<string>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)value!.GetEnumerator();
}

public class StringNullableSpanAddAllSimpleOrderedCollectionStringBearer(string?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(string[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<string>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<string>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)value!.GetEnumerator();
}

public class StringNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(string?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<string?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringListAddAllSimpleOrderedCollectionStringBearer(IReadOnlyList<string?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!.GetEnumerator();
}

public class StringEnumerableAddAllSimpleOrderedCollectionStringBearer(IEnumerable<string?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!.GetEnumerator();
}

public class StringEnumeratorAddAllSimpleOrderedCollectionStringBearer(IEnumerator<string?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<string?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!;
}

public class CharSequenceArrayAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>(TCharSeq?[]? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(value, formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceSpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>(TCharSeq[]? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(value.AsSpan(), formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)value!.GetEnumerator();
}

public class CharSequenceNullableSpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>(TCharSeq?[]? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable(value.AsSpan(), formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceReadOnlySpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>(TCharSeq[]? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq((ReadOnlySpan<TCharSeq>)value.AsSpan(), formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)value!.GetEnumerator();
}

public class CharSequenceNullableReadOnlySpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    (TCharSeq?[]? value, string? formatCharSequence = null) : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable((ReadOnlySpan<TCharSeq?>)value.AsSpan(), formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceListAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    (IReadOnlyList<TCharSeq?>? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(value, formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!.GetEnumerator();
}

public class CharSequenceEnumerableAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    (IEnumerable<TCharSeq?>? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(value, formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!.GetEnumerator();
}

public class CharSequenceEnumeratorAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    (IEnumerator<TCharSeq?>? value, string? formatCharSequence = null)
    : IStringBearer, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(value, formatCharSequence)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!;
}

public class StringBuilderArrayAddAllSimpleOrderedCollectionStringBearer(StringBuilder?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderSpanAddAllSimpleOrderedCollectionStringBearer(StringBuilder[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)value!.GetEnumerator();
}

public class StringBuilderNullableSpanAddAllSimpleOrderedCollectionStringBearer(StringBuilder?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(StringBuilder[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<StringBuilder>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)value!.GetEnumerator();
}

public class StringBuilderNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(StringBuilder?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<StringBuilder?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderListAddAllSimpleOrderedCollectionStringBearer(IReadOnlyList<StringBuilder?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBuilderEnumerableAddAllSimpleOrderedCollectionStringBearer(IEnumerable<StringBuilder?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBuilderEnumeratorAddAllSimpleOrderedCollectionStringBearer(IEnumerator<StringBuilder?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<StringBuilder?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!;
}

public class MatchArrayAddAllSimpleOrderedCollectionStringBearer<T>(T?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchSpanAddAllSimpleOrderedCollectionStringBearer<T>(T[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<T>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)value!.GetEnumerator();
}

public class MatchNullableSpanAddAllSimpleOrderedCollectionStringBearer<T>(T?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<T>(T[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<T>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<T>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)value!.GetEnumerator();
}

public class MatchNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<T>(T?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable((ReadOnlySpan<T?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchListAddAllSimpleOrderedCollectionStringBearer<T>(IReadOnlyList<T?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!.GetEnumerator();
}

public class MatchEnumerableAddAllSimpleOrderedCollectionStringBearer<T>(IEnumerable<T?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!.GetEnumerator();
}

public class MatchEnumeratorAddAllSimpleOrderedCollectionStringBearer<T>(IEnumerator<T?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<T?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!;
}

public class ObjectArrayAddAllSimpleOrderedCollectionStringBearer(object?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectSpanAddAllSimpleOrderedCollectionStringBearer(object[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<object>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)value!.GetEnumerator();
}

public class ObjectNullableSpanAddAllSimpleOrderedCollectionStringBearer(object?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable(value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(object[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<object>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject((ReadOnlySpan<object>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)value!.GetEnumerator();
}

public class ObjectNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer(object?[]? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable((ReadOnlySpan<object?>)value.AsSpan(), formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectListAddAllSimpleOrderedCollectionStringBearer(IReadOnlyList<object?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!.GetEnumerator();
}

public class ObjectEnumerableAddAllSimpleOrderedCollectionStringBearer(IEnumerable<object?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!.GetEnumerator();
}

public class ObjectEnumeratorAddAllSimpleOrderedCollectionStringBearer(IEnumerator<object?>? value, string? formatString = null)
    : IStringBearer, IEnumerable<object?>
{
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(value, formatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!;
}
