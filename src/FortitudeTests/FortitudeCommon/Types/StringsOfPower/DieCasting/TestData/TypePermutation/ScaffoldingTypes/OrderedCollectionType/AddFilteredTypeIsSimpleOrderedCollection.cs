// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionType;

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct)]
public class BoolArrayAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolArrayAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct)]
public class BoolSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct)]
public class BoolReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool>)Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolReadOnlySpanSpanAddFilteredSimpleOrderedCollectionStringBearer
    : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>, IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<bool?>)Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct)]
public class BoolListAlwaysAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<List<bool>?>, IEnumerable<bool>
{
    public List<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolListAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<List<bool?>?>, IEnumerable<bool?>
{
    public List<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct)]
public class BoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<IEnumerable<bool>?>, IEnumerable<bool>
{
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer
    : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>, IMoldSupportedValue<IEnumerable<bool?>?>, IEnumerable<bool?>
{
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct)]
public class BoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool>
  , IMoldSupportedValue<IEnumerator<bool>?>, IEnumerable<bool>
{
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct)]
public class NullableBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsOrderedCollectionPredicate<bool?>
  , IMoldSupportedValue<IEnumerator<bool?>?>, IEnumerable<bool?>
{
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable, TBase
{
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableArrayAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableSpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt> where TFmt : ISpanFormattable
{
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmt>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt?> where TFmt : class, ISpanFormattable
{
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<TFmt?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => (IEnumerator<TFmt?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<TFmtStruct?[]?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<TFmtStruct?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<List<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public List<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableListAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<List<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public List<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<IEnumerable<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmt>, IMoldSupportedValue<IEnumerator<TFmt?>?>, IEnumerable<TFmt?> where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TFmtStruct?>, IMoldSupportedValue<IEnumerator<TFmtStruct?>?>
  , IEnumerable<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked> where TCloaked : TCloakedFilterBase, TCloakedRevealBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase> : IStringBearer
  , ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked[]?>
  , IEnumerable<TCloaked> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked?[]?>
      , IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase> : IStringBearer
  , ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked[]?>
  , IEnumerable<TCloaked> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>, IMoldSupportedValue<TCloaked?[]?>
      , IEnumerable<TCloaked> where TCloaked : class, TCloakedBase, TCloakedFilterBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<TCloakedStruct?[]?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IReadOnlyList<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IEnumerable<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IEnumerable<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class CloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TCloakedFilterBase, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
      , IMoldSupportedValue<IEnumerator<TCloaked?>?>, IEnumerable<TCloaked?> where TCloaked : TCloakedBase, TCloakedFilterBase
{
    public IEnumerator<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class NullableCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> : IStringBearer
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?>, IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>
  , IEnumerable<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate, ValueRevealer)
           .Complete();

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerArrayRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer[]?>, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable(Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerSpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer[]?>, IEnumerable<TBearer> where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearer>)Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class StringBearerNullableReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredNullable((ReadOnlySpan<TBearer?>)Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate  | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerReadOnlySpanRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered((ReadOnlySpan<TBearerStruct?>)Value.AsSpan(), ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate  | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IReadOnlyList<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerListRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFiltered(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IEnumerable<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerBase>, IMoldSupportedValue<IEnumerator<TBearer?>?>, IEnumerable<TBearer>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> : IStringBearer
  , ISupportsOrderedCollectionPredicate<TBearerStruct?>, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate(Value, ElementPredicate)
           .Complete();

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringArrayAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class StringSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string[]?>, IEnumerable<string>
{
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class StringReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string[]?>, IEnumerable<string>
{
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<string>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<string?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringListAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IReadOnlyList<string?>?>, IEnumerable<string?>
{
    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringEnumerableAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IEnumerable<string?>?>, IEnumerable<string?>
{
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<string>, IMoldSupportedValue<IEnumerator<string?>?>, IEnumerable<string?>
{
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<string> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceArrayAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceSpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceNullableSpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer
  , ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceReadOnlySpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer
  , ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq((ReadOnlySpan<TCharSeq>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class CharSequenceNullableReadOnlySpanAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer
  , ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqNullable((ReadOnlySpan<TCharSeq?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceListAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeq(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class CharSequenceEnumerableAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer
  , ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class CharSequenceEnumeratorAddFilteredSimpleOrderedCollectionCharSequenceBearer<TCharSeq, TCharSeqBase> : IStringBearer
  , ISupportsValueFormatString, ISupportsOrderedCollectionPredicate<TCharSeqBase>, IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , IEnumerable<TCharSeq?> where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredCharSeqEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TCharSeqBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderArrayAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered((ReadOnlySpan<StringBuilder>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringBuilderNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredNullable((ReadOnlySpan<StringBuilder?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderListAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFiltered(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringBuilderEnumerableAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IEnumerable<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class StringBuilderEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<StringBuilder>, IMoldSupportedValue<IEnumerator<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchArrayAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<T?[]?>, IEnumerable<T?> where T : TBase
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class MatchSpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<T[]?>, IEnumerable<T> where T : TBase
{
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class MatchNullableSpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<T?[]?>, IEnumerable<T?> where T : TBase
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class MatchReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<T[]?>, IEnumerable<T> where T : TBase
{
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch((ReadOnlySpan<T>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class MatchNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<T?[]?>, IEnumerable<T?> where T : TBase
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchNullable((ReadOnlySpan<T?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchListAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<IReadOnlyList<T?>?>, IEnumerable<T?> where T : TBase
{
    public IReadOnlyList<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatch(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<IEnumerable<T?>?>, IEnumerable<T?> where T : TBase
{
    public IEnumerable<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<T, TBase> : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<TBase>, IMoldSupportedValue<IEnumerator<T?>?>, IEnumerable<T?> where T : TBase
{
    public IEnumerator<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredMatchEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<TBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TBase>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class ObjectArrayAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class ObjectSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object[]?>, IEnumerable<object>
{
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class ObjectNullableSpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable(Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class ObjectReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object[]?>, IEnumerable<object>
{
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject((ReadOnlySpan<object>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | FilterPredicate | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class ObjectNullableReadOnlySpanAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectNullable((ReadOnlySpan<object?>)Value.AsSpan(), ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class ObjectListAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IReadOnlyList<object?>?>, IEnumerable<object?>
{
    public IReadOnlyList<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObject(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class ObjectEnumerableAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IEnumerable<object?>?>, IEnumerable<object?>
{
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class ObjectEnumeratorAddFilteredSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , ISupportsOrderedCollectionPredicate<object>, IMoldSupportedValue<IEnumerator<object?>?>, IEnumerable<object?>
{
    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredObjectEnumerate(Value, ElementPredicate, ValueFormatString)
           .Complete();

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!;
}
