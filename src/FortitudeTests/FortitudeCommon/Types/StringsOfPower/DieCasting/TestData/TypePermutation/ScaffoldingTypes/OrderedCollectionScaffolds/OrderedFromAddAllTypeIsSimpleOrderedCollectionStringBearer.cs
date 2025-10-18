// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionScaffolds;

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray  | AcceptsStruct)]
public class OrderedFromBoolArrayAddAllSimpleOrderedCollectionStringBearer : IEnumerable<bool>, IMoldSupportedValue<bool[]?>
{
    public bool[]? OrderedCollectionAddAllBoolArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolArray);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolArray)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolArrayAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolArray);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolArray)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan  | AcceptsStruct)]
public class OrderedFromBoolSpanAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? OrderedCollectionAddAllBoolSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolSpan);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolSpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolSpanAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolSpan);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolSpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan  | AcceptsStruct)]
public class OrderedFromBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? OrderedCollectionAddAllBoolReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolReadOnlySpan);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool>)OrderedCollectionAddAllBoolReadOnlySpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolReadOnlySpan);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool?>)OrderedCollectionAddAllNullableBoolReadOnlySpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList  | AcceptsStruct)]
public class OrderedFromBoolListAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<List<bool>?>, IEnumerable<bool>
{
    public List<bool>? OrderedCollectionAddAllBoolList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolList);

    public List<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllBoolList)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolListAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<List<bool?>?>, IEnumerable<bool?>
{
    public List<bool?>? OrderedCollectionAddAllNullableBoolList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolList);

    public List<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableBoolList)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable  | AcceptsStruct)]
public class OrderedFromBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<IEnumerable<bool>?>, IEnumerable<bool>
{
    public IEnumerable<bool>? OrderedCollectionAddAllBoolEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerable);

    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllBoolEnumerable)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>
  , IEnumerable<bool?>
{
    public IEnumerable<bool?>? OrderedCollectionAddAllNullableBoolEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerable);

    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableBoolEnumerable)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator  | AcceptsStruct)]
public class OrderedFromBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<IEnumerator<bool>?>, IEnumerable<bool>
{
    public IEnumerator<bool>? OrderedCollectionAddAllBoolEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerator);

    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllBoolEnumerator)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>
  , IEnumerable<bool?>
{
    public IEnumerator<bool?>? OrderedCollectionAddAllNullableBoolEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerator);

    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableBoolEnumerator)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableArray);

    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableArray);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableSpan);

    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddAllSpanFormattableNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableSpan);

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllSpanFormattableNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableSpan);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableReadOnlySpan);

    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt>)OrderedCollectionAddAllSpanFormattableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan);

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<TFmt?>)OrderedCollectionAddAllSpanFormattableNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmtStruct?>)OrderedCollectionAddAllNullableSpanFormattableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? OrderedCollectionAddAllSpanFormattableList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableList);

    public IReadOnlyList<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableList);

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllNullableSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt?>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public IEnumerator<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerArray);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerArray, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerArray);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerArray, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerSpan);

    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableSpan);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(OrderedCollectionRevealAllCloakedBearerNullableSpan.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerSpan);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked[]? OrderedCollectionRevealAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerReadOnlySpan);

    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked>)OrderedCollectionRevealAllCloakedBearerReadOnlySpan.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[]? OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TCloaked?>)OrderedCollectionRevealAllCloakedBearerNullableReadOnlySpan.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloakedStruct?>)OrderedCollectionRevealAllNullableCloakedBearerReadOnlySpan, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IReadOnlyList<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked?>? OrderedCollectionRevealAllCloakedBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerList);

    public IReadOnlyList<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllCloakedBearerList, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerList);

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableCloakedBearerList, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IEnumerable<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked?>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public IEnumerable<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerable);

    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }
    
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IEnumerator<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked?>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public IEnumerator<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerator);

    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerArray);

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerArray)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerArray);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerArray)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerSpan);

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerSpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableSpan);

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(OrderedCollectionRevealAllStringBearerNullableSpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerSpan);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerSpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public TBearer[]? OrderedCollectionRevealAllStringBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerReadOnlySpan);

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer>)OrderedCollectionRevealAllStringBearerReadOnlySpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer
{
    public TBearer?[]? OrderedCollectionRevealAllStringBearerNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerNullableReadOnlySpan);

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TBearer?>)OrderedCollectionRevealAllStringBearerNullableReadOnlySpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerReadOnlySpan);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearerStruct?>)OrderedCollectionRevealAllNullableStringBearerReadOnlySpan.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<IReadOnlyList<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer?>? OrderedCollectionRevealAllStringBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerList);

    public IReadOnlyList<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllStringBearerList)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerList);

    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(OrderedCollectionRevealAllNullableStringBearerList)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<IEnumerable<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IEnumerable<TBearer?>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public IEnumerable<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerable)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableStringBearerEnumerable)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IMoldSupportedValue<IEnumerator<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IEnumerator<TBearer?>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public IEnumerator<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerator)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllNullableStringBearerEnumerator)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringArrayAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<string?[]?>
  , IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddAllStringArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringArray);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<string[]?>
  , IEnumerable<string>
{
    public string[]? OrderedCollectionAddAllStringSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringSpan);

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<string?[]?>
  , IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddAllStringNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringNullableSpan);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<string[]?>
  , IEnumerable<string>
{
    public string[]? OrderedCollectionAddAllStringReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringReadOnlySpan);

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<string>)OrderedCollectionAddAllStringReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringNullableReadOnlySpan);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<string?>)OrderedCollectionAddAllStringNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringListAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<string?>?>, IEnumerable<string?>
{
    public IReadOnlyList<string?>? OrderedCollectionAddAllStringList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringList);

    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<string?>?>, IEnumerable<string?>
{
    public IEnumerable<string?>? OrderedCollectionAddAllStringEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringEnumerable);

    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<string?>?>, IEnumerable<string?>
{
    public IEnumerator<string?>? OrderedCollectionAddAllStringEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringEnumerator);

    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceArray);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceSpan);

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableSpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceNullableSpan);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable(OrderedCollectionAddAllCharSequenceNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq> : ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public TCharSeq[]? OrderedCollectionAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceReadOnlySpan);

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq((ReadOnlySpan<TCharSeq>)OrderedCollectionAddAllCharSequenceReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : ISupportsValueFormatString, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? OrderedCollectionAddAllCharSequenceNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceNullableReadOnlySpan);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable((ReadOnlySpan<TCharSeq?>)OrderedCollectionAddAllCharSequenceNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? OrderedCollectionAddAllCharSequenceList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceList);

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(OrderedCollectionAddAllCharSequenceList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TCharSeq?>?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(OrderedCollectionAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TCharSeq?>?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(OrderedCollectionAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderArray);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderSpan);

    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableSpan);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(OrderedCollectionAddAllStringBuilderNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? OrderedCollectionAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderReadOnlySpan);

    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<StringBuilder>)OrderedCollectionAddAllStringBuilderReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderNullableReadOnlySpan);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<StringBuilder?>)OrderedCollectionAddAllStringBuilderNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IReadOnlyList<StringBuilder?>? OrderedCollectionAddAllStringBuilderList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderList);

    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(OrderedCollectionAddAllStringBuilderList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerable);

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerator);

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(OrderedCollectionAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchArrayAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString, IMoldSupportedValue<TAny?[]?>
  , IEnumerable<TAny?>
{
    public TAny?[]? OrderedCollectionAddAllMatchArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchArray);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder | AcceptsSpanFormattable 
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString, IMoldSupportedValue<TAny[]?>
  , IEnumerable<TAny>
{
    public TAny[]? OrderedCollectionAddAllMatchSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchSpan);

    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny> GetEnumerator() => (IEnumerator<TAny>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableSpanAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString, IMoldSupportedValue<TAny?[]?>
  , IEnumerable<TAny?>
{
    public TAny?[]? OrderedCollectionAddAllMatchNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchNullableSpan);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable(OrderedCollectionAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString, IMoldSupportedValue<TAny[]?>
  , IEnumerable<TAny>
{
    public TAny[]? OrderedCollectionAddAllMatchReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchReadOnlySpan);

    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<TAny>)OrderedCollectionAddAllMatchReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny> GetEnumerator() => (IEnumerator<TAny>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString
  , IMoldSupportedValue<TAny?[]?>, IEnumerable<TAny?>
{
    public TAny?[]? OrderedCollectionAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchNullableReadOnlySpan);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable((ReadOnlySpan<TAny?>)OrderedCollectionAddAllMatchNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchListAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<TAny?>?>, IEnumerable<TAny?>
{
    public IReadOnlyList<TAny?>? OrderedCollectionAddAllMatchList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchList);

    public IReadOnlyList<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(OrderedCollectionAddAllMatchList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<TAny?>?>, IEnumerable<TAny?>
{
    public IEnumerable<TAny?>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public IEnumerable<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(OrderedCollectionAddAllMatchEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<TAny?>?>, IEnumerable<TAny?>
{
    public IEnumerator<TAny?>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public IEnumerator<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(OrderedCollectionAddAllMatchEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectArrayAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<object?[]?>
  , IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddAllObjectArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectArray);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<object[]?>
  , IEnumerable<object>
{
    public object[]? OrderedCollectionAddAllObjectSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectSpan);

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableSpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<object?[]?>
  , IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddAllObjectNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectNullableSpan);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable(OrderedCollectionAddAllObjectNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString, IMoldSupportedValue<object[]?>
  , IEnumerable<object>
{
    public object[]? OrderedCollectionAddAllObjectReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectReadOnlySpan);

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject((ReadOnlySpan<object>)OrderedCollectionAddAllObjectReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectNullableReadOnlySpan);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable((ReadOnlySpan<object?>)OrderedCollectionAddAllObjectNullableReadOnlySpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectListAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<object?>?>, IEnumerable<object?>
{
    public IReadOnlyList<object?>? OrderedCollectionAddAllObjectList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectList);

    public IReadOnlyList<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(OrderedCollectionAddAllObjectList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectEnumerableAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<object?>?>, IEnumerable<object?>
{
    public IEnumerable<object?>? OrderedCollectionAddAllObjectEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerable);

    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(OrderedCollectionAddAllObjectEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectEnumeratorAddAllSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<object?>?>, IEnumerable<object?>
{
    public IEnumerator<object?>? OrderedCollectionAddAllObjectEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerator);

    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(OrderedCollectionAddAllObjectEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
