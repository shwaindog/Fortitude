// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionType;

public class BoolArrayAddFilteredSimpleOrderedCollectionStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolArrayAddFilteredSimpleOrderedCollectionStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolSpanAddFilteredSimpleOrderedCollectionStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolSpanAddFilteredSimpleOrderedCollectionStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool>)value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)value!.GetEnumerator();
}

public class NullableBoolReadOnlySpanSpanAddFilteredSimpleOrderedCollectionStringBearer
    (bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool?>)value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)value!.GetEnumerator();
}

public class BoolListAlwaysAddFilteredSimpleOrderedCollectionStringBearer(List<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!.GetEnumerator();
}

public class NullableBoolListAddFilteredSimpleOrderedCollectionStringBearer(List<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!.GetEnumerator();
}

public class BoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!.GetEnumerator();
}

public class NullableBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer
    (IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!.GetEnumerator();
}

public class BoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    : IStringBearer, IEnumerable<bool>
{
    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => value!;
}

public class NullableBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer(IEnumerator<bool?>? value
  , OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer, IEnumerable<bool?>
{
    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => value!;
}

public class SpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TBase>(TFmt?[]? value
  , OrderedCollectionPredicate<TBase> filterPredicate, string? formatString = null)
    : IStringBearer, IEnumerable<TFmt> where TFmt : ISpanFormattable, TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class NullableSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(TFmtStruct?[]? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null)
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class SpanFormattableNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(TFmt?[]? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)value!.GetEnumerator();
}

public class NullableSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(TFmtStruct?[]? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null)
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(TFmt[]? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmt>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)value!.GetEnumerator();
}

public class SpanFormattableNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(TFmt?[]? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<TFmt?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)value!.GetEnumerator();
}

public class NullableSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(TFmtStruct?[]? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmtStruct?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)value!.GetEnumerator();
}

public class SpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(List<TFmt?>? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(List<TFmtStruct?>? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class SpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(IEnumerable<TFmt?>? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(IEnumerable<TFmtStruct?>? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class SpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt>(IEnumerator<TFmt?>? value
  , OrderedCollectionPredicate<TFmt> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => value!;
}

public class NullableSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct>(IEnumerator<TFmtStruct?>? value
  , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => value!;
}

public class CloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked> where TCloaked : TCloakedFilterBase, TCloakedRevealBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>(TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class CloakedBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    (TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>(TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class CloakedBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    (TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)value!.GetEnumerator();
}

public class NullableCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)value!.GetEnumerator();
}

public class CloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    (IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class CloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    (IEnumerable<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!.GetEnumerator();
}

public class NullableCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(value, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class CloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    (IEnumerator<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedBase> palantírReveal)
    : IStringBearer, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => value!;
}

public class NullableCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
    : IStringBearer, IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter, palantírReveal)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => value!;
}

public class StringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(TBearer?[]? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(TBearer[]? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class StringBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(TBearer?[]? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>(TBearerStruct?[]? value
  , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(TBearer[]? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearer>)value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class StringBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(TBearer?[]? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable((ReadOnlySpan<TBearer?>)value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>(TBearerStruct?[]? value
  , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearerStruct?>)value.AsSpan(), Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)value!.GetEnumerator();
}

public class StringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(IReadOnlyList<TBearer?>? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value
  , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(IEnumerable<TBearer?>? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)value!.GetEnumerator();
}

public class NullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>(IEnumerable<TBearerStruct?>? value
  , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase>(IEnumerator<TBearer?>? value
  , OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => value!;
}

public class NullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>(IEnumerator<TBearerStruct?>? value
  , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(value, Filter)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => value!;
}

public class StringArrayAddFilteredSimpleOrderedCollectionStringBearer(string?[]? value, OrderedCollectionPredicate<string> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringSpanAddFilteredSimpleOrderedCollectionStringBearer(string[]? value, OrderedCollectionPredicate<string> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<string>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)value!.GetEnumerator();
}

public class StringNullableSpanAddFilteredSimpleOrderedCollectionStringBearer(string?[]? value
  , OrderedCollectionPredicate<string> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(string[]? value, OrderedCollectionPredicate<string> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<string>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<string>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)value!.GetEnumerator();
}

public class StringNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(string?[]? value
  , OrderedCollectionPredicate<string> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<string?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)value!.GetEnumerator();
}

public class StringListAddFilteredSimpleOrderedCollectionStringBearer(IReadOnlyList<string?>? value
  , OrderedCollectionPredicate<string> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!.GetEnumerator();
}

public class StringEnumerableAddFilteredSimpleOrderedCollectionStringBearer(IEnumerable<string?>? value
  , OrderedCollectionPredicate<string> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!.GetEnumerator();
}

public class StringEnumeratorAddFilteredSimpleOrderedCollectionStringBearer(IEnumerator<string?>? value
  , OrderedCollectionPredicate<string> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<string?>
{
    public OrderedCollectionPredicate<string> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => value!;
}

public class CharSequenceArrayAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(TCharSeq?[]? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceSpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(TCharSeq[]? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)value!.GetEnumerator();
}

public class CharSequenceNullableSpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(TCharSeq?[]? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null)
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceReadOnlySpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(TCharSeq[]? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq((ReadOnlySpan<TCharSeq>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)value!.GetEnumerator();
}

public class CharSequenceNullableReadOnlySpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(TCharSeq?[]? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable((ReadOnlySpan<TCharSeq?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)value!.GetEnumerator();
}

public class CharSequenceListAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(IReadOnlyList<TCharSeq?>? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!.GetEnumerator();
}

public class CharSequenceEnumerableAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(IEnumerable<TCharSeq?>? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!.GetEnumerator();
}

public class CharSequenceEnumeratorAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase>(IEnumerator<TCharSeq?>? value
  , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, string? formatString = null) 
    : IStringBearer, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public OrderedCollectionPredicate<TCharSeqBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => value!;
}

public class StringBuilderArrayAddFilteredSimpleOrderedCollectionStringBearer(StringBuilder?[]? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderSpanAddFilteredSimpleOrderedCollectionStringBearer(StringBuilder[]? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)value!.GetEnumerator();
}

public class StringBuilderNullableSpanAddFilteredSimpleOrderedCollectionStringBearer(StringBuilder?[]? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(StringBuilder[]? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<StringBuilder>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)value!.GetEnumerator();
}

public class StringBuilderNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(StringBuilder?[]? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<StringBuilder?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)value!.GetEnumerator();
}

public class StringBuilderListAddFilteredSimpleOrderedCollectionStringBearer(IReadOnlyList<StringBuilder?>? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBuilderEnumerableAddFilteredSimpleOrderedCollectionStringBearer(IEnumerable<StringBuilder?>? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!.GetEnumerator();
}

public class StringBuilderEnumeratorAddFilteredSimpleOrderedCollectionStringBearer(IEnumerator<StringBuilder?>? value
  , OrderedCollectionPredicate<StringBuilder> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<StringBuilder?>
{
    public OrderedCollectionPredicate<StringBuilder> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => value!;
}

public class MatchArrayAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchSpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<T> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)value!.GetEnumerator();
}

public class MatchNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<T> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch((ReadOnlySpan<T>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)value!.GetEnumerator();
}

public class MatchNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(T?[]? value
  , OrderedCollectionPredicate<TBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable((ReadOnlySpan<T?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)value!.GetEnumerator();
}

public class MatchListAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(IReadOnlyList<T?>? value
  , OrderedCollectionPredicate<TBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!.GetEnumerator();
}

public class MatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(IEnumerable<T?>? value
  , OrderedCollectionPredicate<TBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!.GetEnumerator();
}

public class MatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<T, TBase>(IEnumerator<T?>? value
  , OrderedCollectionPredicate<TBase> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<T?> where T : TBase
{
    public OrderedCollectionPredicate<TBase> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => value!;
}

public class ObjectArrayAddFilteredSimpleOrderedCollectionStringBearer(object?[]? value, OrderedCollectionPredicate<object> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectSpanAddFilteredSimpleOrderedCollectionStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<object>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)value!.GetEnumerator();
}

public class ObjectNullableSpanAddFilteredSimpleOrderedCollectionStringBearer(object?[]? value, OrderedCollectionPredicate<object> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable(value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate
  , string? formatString = null) : IStringBearer, IEnumerable<object>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject((ReadOnlySpan<object>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)value!.GetEnumerator();
}

public class ObjectNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer(object?[]? value
  , OrderedCollectionPredicate<object> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable((ReadOnlySpan<object?>)value.AsSpan(), Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)value!.GetEnumerator();
}

public class ObjectListAddFilteredSimpleOrderedCollectionStringBearer(IReadOnlyList<object?>? value
  , OrderedCollectionPredicate<object> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!.GetEnumerator();
}

public class ObjectEnumerableAddFilteredSimpleOrderedCollectionStringBearer(IEnumerable<object?>? value
  , OrderedCollectionPredicate<object> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!.GetEnumerator();
}

public class ObjectEnumeratorAddFilteredSimpleOrderedCollectionStringBearer(IEnumerator<object?>? value
  , OrderedCollectionPredicate<object> filterPredicate, string? formatString = null) : IStringBearer, IEnumerable<object?>
{
    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public string? FormatString { get; set; } = formatString;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(value, Filter, FormatString)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => value!;
}
