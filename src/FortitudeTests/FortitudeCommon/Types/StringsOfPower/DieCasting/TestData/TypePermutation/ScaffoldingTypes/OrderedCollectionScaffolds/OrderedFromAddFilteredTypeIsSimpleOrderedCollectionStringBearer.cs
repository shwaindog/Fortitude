// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionScaffolds;

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolArrayAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? OrderedCollectionAddFilteredBoolArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolArray);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolArray, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolArrayAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolArray);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolArray, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? OrderedCollectionAddFilteredBoolSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolSpan);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolSpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolSpan);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolSpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? OrderedCollectionAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolReadOnlySpan);

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool>)OrderedCollectionAddFilteredBoolReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer
    : ISupportsOrderedCollectionPredicate<bool?>, IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? OrderedCollectionAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolReadOnlySpan);

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool?>)OrderedCollectionAddFilteredNullableBoolReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolListAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<List<bool>?>, IEnumerable<bool>
{
    public List<bool>? OrderedCollectionAddFilteredBoolList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolList);

    public List<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredBoolList, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolListAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<List<bool?>?>, IEnumerable<bool?>
{
    public List<bool?>? OrderedCollectionAddFilteredNullableBoolList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolList);

    public List<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableBoolList, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<IEnumerable<bool>?>, IEnumerable<bool>
{
    public IEnumerable<bool>? OrderedCollectionAddFilteredBoolEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerable);

    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredBoolEnumerable, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer
    : ISupportsOrderedCollectionPredicate<bool?>, IMoldSupportedValue<IEnumerable<bool?>?>, IEnumerable<bool?>
{
    public IEnumerable<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerable);

    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableBoolEnumerable, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct)]
public class OrderedFromBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<IEnumerator<bool>?>, IEnumerable<bool>
{
    public IEnumerator<bool>? OrderedCollectionAddFilteredBoolEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerator);

    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredBoolEnumerator, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<IEnumerator<bool?>?>, IEnumerable<bool?>
{
    public IEnumerator<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerator);

    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableBoolEnumerator, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable, TBase
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableArray);

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableArray);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddFilteredSpanFormattableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableSpan);

    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableNullableSpan);

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(OrderedCollectionAddFilteredSpanFormattableNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableSpan);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public TFmt[]? OrderedCollectionAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableReadOnlySpan);

    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmt>)OrderedCollectionAddFilteredSpanFormattableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public TFmt?[]? OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan);

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<TFmt?>)OrderedCollectionAddFilteredSpanFormattableNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan);

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmtStruct?>)OrderedCollectionAddFilteredNullableSpanFormattableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<List<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public List<TFmt?>? OrderedCollectionAddFilteredSpanFormattableList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableList);

    public List<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredSpanFormattableList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<List<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public List<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableList);

    public List<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredNullableSpanFormattableList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<IEnumerable<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);

    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredSpanFormattableEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable);

    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<IEnumerator<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt?>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);

    public IEnumerator<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredSpanFormattableEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<IEnumerator<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator);

    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked> where TCloaked : TCloakedFilterBase, TCloakedRevealBase
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerArray);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerArray, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerArray);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerArray, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
  ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked[]?>
  , IEnumerable<TCloaked> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? OrderedCollectionRevealFilteredCloakedBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerSpan);

    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerSpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked?[]?>
      , IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerNullableSpan);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(OrderedCollectionRevealFilteredCloakedBearerNullableSpan.AsSpan()
                                 , ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerSpan);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerSpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase> : 
  ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked[]?>
  , IEnumerable<TCloaked> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public TCloaked[]? OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan);

    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked?[]?>
      , IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public TCloaked?[]? OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan);

    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(OrderedCollectionRevealFilteredCloakedBearerNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan);

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerReadOnlySpan.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IReadOnlyList<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerList);

    public IReadOnlyList<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredCloakedBearerList, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerList);

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableCloakedBearerList, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IEnumerable<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IEnumerable<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public IEnumerable<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredCloakedBearerEnumerable, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable);

    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IEnumerator<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IEnumerator<TCloaked?>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public IEnumerator<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredCloakedBearerEnumerator, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : 
  ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator);

    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

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

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerArray);

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerArray, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>
    : ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerArray);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerArray, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer[]?>, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? OrderedCollectionRevealFilteredStringBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerSpan);

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerNullableSpan);

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(OrderedCollectionRevealFilteredStringBearerNullableSpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : 
  ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerSpan);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer[]?>, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? OrderedCollectionRevealFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerReadOnlySpan);

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearer>)OrderedCollectionRevealFilteredStringBearerReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan);

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable((ReadOnlySpan<TBearer?>)OrderedCollectionRevealFilteredStringBearerNullableReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate  | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : 
  ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan);

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearerStruct?>)OrderedCollectionRevealFilteredNullableStringBearerReadOnlySpan.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate  | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IReadOnlyList<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? OrderedCollectionRevealFilteredStringBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerList);

    public IReadOnlyList<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredStringBearerList, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : 
  ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerList);

    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(OrderedCollectionRevealFilteredNullableStringBearerList, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IEnumerable<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);

    public IEnumerable<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredStringBearerEnumerable, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : 
  ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerable);

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableStringBearerEnumerable, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : 
  ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IEnumerator<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);

    public IEnumerator<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredStringBearerEnumerator, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : 
  ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerator);

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(OrderedCollectionRevealFilteredNullableStringBearerEnumerator, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringArrayAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddFilteredStringArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringArray);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string[]?>, IEnumerable<string>
{
    public string[]? OrderedCollectionAddFilteredStringSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringSpan);

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddFilteredStringNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringNullableSpan);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(OrderedCollectionAddFilteredStringNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string[]?>, IEnumerable<string>
{
    public string[]? OrderedCollectionAddFilteredStringReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringReadOnlySpan);

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<string>)OrderedCollectionAddFilteredStringReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? OrderedCollectionAddFilteredStringNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringNullableReadOnlySpan);

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<string?>)OrderedCollectionAddFilteredStringNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsString | SupportsValueFormatString)]
public class OrderedFromStringListAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IReadOnlyList<string?>?>, IEnumerable<string?>
{
    public IReadOnlyList<string?>? OrderedCollectionAddFilteredStringList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringList);

    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IEnumerable<string?>?>, IEnumerable<string?>
{
    public IEnumerable<string?>? OrderedCollectionAddFilteredStringEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerable);

    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsString |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IEnumerator<string?>?>, IEnumerable<string?>
{
    public IEnumerator<string?>? OrderedCollectionAddFilteredStringEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerator);

    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceArray);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq[]? OrderedCollectionAddFilteredCharSequenceSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceSpan);

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : 
  ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceNullableSpan);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable(OrderedCollectionAddFilteredCharSequenceNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : 
  ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq[]? OrderedCollectionAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceReadOnlySpan);

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq((ReadOnlySpan<TCharSeq>)OrderedCollectionAddFilteredCharSequenceReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : 
  ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan);

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable((ReadOnlySpan<TCharSeq?>)OrderedCollectionAddFilteredCharSequenceNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IReadOnlyList<TCharSeq?>? OrderedCollectionAddFilteredCharSequenceList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceList);

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(OrderedCollectionAddFilteredCharSequenceList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : 
  ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerable<TCharSeq?>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(OrderedCollectionAddFilteredCharSequenceEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> : 
  ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerator<TCharSeq?>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(OrderedCollectionAddFilteredCharSequenceEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderArray);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? OrderedCollectionAddFilteredStringBuilderSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderSpan);

    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderNullableSpan);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(OrderedCollectionAddFilteredStringBuilderNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? OrderedCollectionAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderReadOnlySpan);

    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<StringBuilder>)OrderedCollectionAddFilteredStringBuilderReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan);

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<StringBuilder?>)OrderedCollectionAddFilteredStringBuilderNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IReadOnlyList<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderList);

    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(OrderedCollectionAddFilteredStringBuilderList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IEnumerable<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerable);

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringBuilderEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBuilder |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IEnumerator<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerator);

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(OrderedCollectionAddFilteredStringBuilderEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchArrayAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<TAny?[]?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchArray);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchSpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<TAny[]?>, IEnumerable<TAny> where TAny : TAnyFilterBase
{
    public TAny[]? OrderedCollectionAddFilteredMatchSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchSpan);

    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny> GetEnumerator() => (IEnumerator<TAny>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<TAny?[]?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchNullableSpan);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable(OrderedCollectionAddFilteredMatchNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<TAny[]?>, IEnumerable<TAny> where TAny : TAnyFilterBase
{
    public TAny[]? OrderedCollectionAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchReadOnlySpan);

    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch((ReadOnlySpan<TAny>)OrderedCollectionAddFilteredMatchReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny> GetEnumerator() => (IEnumerator<TAny>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<TAny?[]?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public TAny?[]? OrderedCollectionAddFilteredMatchNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchNullableReadOnlySpan);

    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable((ReadOnlySpan<TAny?>)OrderedCollectionAddFilteredMatchNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => (IEnumerator<TAny?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchListAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<IReadOnlyList<TAny?>?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny?>? OrderedCollectionAddFilteredMatchList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchList);

    public IReadOnlyList<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(OrderedCollectionAddFilteredMatchList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<IEnumerable<TAny?>?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public IEnumerable<TAny?>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);

    public IEnumerable<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(OrderedCollectionAddFilteredMatchEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TAnyFilterBase>, IMoldSupportedValue<IEnumerator<TAny?>?>, IEnumerable<TAny?> where TAny : TAnyFilterBase
{
    public IEnumerator<TAny?>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);

    public IEnumerator<TAny?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(OrderedCollectionAddFilteredMatchEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TAny?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectArrayAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddFilteredObjectArray
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectArray);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectArray, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object[]?>, IEnumerable<object>
{
    public object[]? OrderedCollectionAddFilteredObjectSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectSpan);

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddFilteredObjectNullableSpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectNullableSpan);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable(OrderedCollectionAddFilteredObjectNullableSpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object[]?>, IEnumerable<object>
{
    public object[]? OrderedCollectionAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectReadOnlySpan);

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject((ReadOnlySpan<object>)OrderedCollectionAddFilteredObjectReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? OrderedCollectionAddFilteredObjectNullableReadOnlySpan
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectNullableReadOnlySpan);

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable((ReadOnlySpan<object?>)OrderedCollectionAddFilteredObjectNullableReadOnlySpan.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectListAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IReadOnlyList<object?>?>, IEnumerable<object?>
{
    public IReadOnlyList<object?>? OrderedCollectionAddFilteredObjectList
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectList);

    public IReadOnlyList<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(OrderedCollectionAddFilteredObjectList, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectEnumerableAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IEnumerable<object?>?>, IEnumerable<object?>
{
    public IEnumerable<object?>? OrderedCollectionAddFilteredObjectEnumerable
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerable);

    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(OrderedCollectionAddFilteredObjectEnumerable, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(OrderedCollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromObjectEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IEnumerator<object?>?>, IEnumerable<object?>
{
    public IEnumerator<object?>? OrderedCollectionAddFilteredObjectEnumerator
    {
        get => Value;
        set =>Value = value;
    }

    public string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerator);

    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(OrderedCollectionAddFilteredObjectEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
