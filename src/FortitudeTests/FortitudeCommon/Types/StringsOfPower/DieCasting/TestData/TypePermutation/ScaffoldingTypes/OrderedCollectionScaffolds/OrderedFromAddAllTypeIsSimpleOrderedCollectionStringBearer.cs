// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.OrderedCollectionScaffolds;

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray  | AcceptsStruct)]
public class OrderedFromBoolArrayAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IEnumerable<bool>, IMoldSupportedValue<bool[]?>
{
    public bool[]? Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolArrayAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan  | AcceptsStruct)]
public class OrderedFromBoolSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan  | AcceptsStruct)]
public class OrderedFromBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, IEnumerable<bool>
{
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool>)Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => (IEnumerator<bool>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, IEnumerable<bool?>
{
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<bool?>)Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => (IEnumerator<bool?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList  | AcceptsStruct)]
public class OrderedFromBoolListAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<List<bool>?>, IEnumerable<bool>
{
    public List<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolListAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<List<bool?>?>, IEnumerable<bool?>
{
    public List<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable  | AcceptsStruct)]
public class OrderedFromBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool>?>, IEnumerable<bool>
{
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool?>?>
  , IEnumerable<bool?>
{
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator  | AcceptsStruct)]
public class OrderedFromBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool>?>, IEnumerable<bool>
{
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator  | AcceptsNullableStruct)]
public class OrderedFromNullableBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool?>?>
  , IEnumerable<bool?>
{
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<bool?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableFormattableArrayAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableSpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableSpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmt[]?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmt>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmt?[]?>, IEnumerable<TFmt>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<TFmt?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TFmtStruct?[]?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<TFmtStruct?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => (IEnumerator<TFmtStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableListAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => (IEnumerator<TFmt>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TFmt?>?>, IEnumerable<TFmt>
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmt> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, IEnumerable<TFmtStruct?>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TFmtStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(Value.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked[]?>, IEnumerable<TCloaked>
    where TCloaked : TCloakedBase
{
    public TCloaked[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloaked>)Value.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<TCloaked?[]?>, IEnumerable<TCloaked>
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TCloaked?>)Value.AsSpan(), ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => (IEnumerator<TCloaked>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<TCloakedStruct?[]?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TCloakedStruct?>)Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => (IEnumerator<TCloakedStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IReadOnlyList<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerListRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IEnumerable<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, ISupportsValueRevealer<TCloakedBase>, IMoldSupportedValue<IEnumerator<TCloaked?>?>, IEnumerable<TCloaked?>
    where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : IStringBearer, ISupportsValueRevealer<TCloakedStruct>, IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, IEnumerable<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloakedStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerArrayRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerNullableSpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable(Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerSpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class OrderedFromStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
  , IEnumerable<TBearer>
    where TBearer : IStringBearer
{
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearer>)Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class OrderedFromStringBearerNullableReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
  , IEnumerable<TBearer> where TBearer : class, IStringBearer
{
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllNullable((ReadOnlySpan<TBearer?>)Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerReadOnlySpanRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll((ReadOnlySpan<TBearerStruct?>)Value.AsSpan())
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => (IEnumerator<TBearerStruct?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerListRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, IEnumerable<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAll(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IEnumerable<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => (IEnumerator<TBearer>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class OrderedFromStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearer?>?>
  , IEnumerable<TBearer> where TBearer : IStringBearer
{
    public IEnumerator<TBearer?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearer> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer)]
public class OrderedFromNullableStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, IEnumerable<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(Value)
           .Complete();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TBearerStruct?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringArrayAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<string?[]?>
  , IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<string[]?>
  , IEnumerable<string>
{
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringNullableSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<string?[]?>
  , IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<string[]?>
  , IEnumerable<string>
{
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<string>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<string?[]?>, IEnumerable<string?>
{
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<string?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => (IEnumerator<string?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringListAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<string?>?>, IEnumerable<string?>
{
    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<string?>?>, IEnumerable<string?>
{
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<string?>?>, IEnumerable<string?>
{
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<string?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromCharSequenceArrayAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromCharSequenceSpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableSpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromCharSequenceReadOnlySpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<TCharSeq[]?>, IEnumerable<TCharSeq> where TCharSeq : ICharSequence
{
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq((ReadOnlySpan<TCharSeq>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq> GetEnumerator() => (IEnumerator<TCharSeq>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceNullableReadOnlySpanAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<TCharSeq?[]?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqNullable((ReadOnlySpan<TCharSeq?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => (IEnumerator<TCharSeq?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromCharSequenceListAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, IEnumerable<TCharSeq?> where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeq(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerable<TCharSeq?>?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddAllSimpleOrderedCollectionCharSequenceBearer<TCharSeq>
    : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<IEnumerator<TCharSeq?>?>, IEnumerable<TCharSeq?>
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllCharSeqEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCharSeq?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringBuilderArrayAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringBuilderSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringBuilderReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder[]?>, IEnumerable<StringBuilder>
{
    public StringBuilder[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll((ReadOnlySpan<StringBuilder>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder> GetEnumerator() => (IEnumerator<StringBuilder>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<StringBuilder?[]?>, IEnumerable<StringBuilder?>
{
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllNullable((ReadOnlySpan<StringBuilder?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => (IEnumerator<StringBuilder?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsClass | AcceptsNullableClass | AcceptsChars | SupportsValueFormatString)]
public class OrderedFromStringBuilderListAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAll(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsClass | AcceptsNullableClass | AcceptsChars |
                  SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<StringBuilder?>?>, IEnumerable<StringBuilder?>
{
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<StringBuilder?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromMatchArrayAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<T?[]?>
  , IEnumerable<T?>
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchSpanAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<T[]?>
  , IEnumerable<T>
{
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableSpanAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<T?[]?>
  , IEnumerable<T?>
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<T[]?>
  , IEnumerable<T>
{
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch((ReadOnlySpan<T>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromMatchNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<T?[]?>, IEnumerable<T?>
{
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchNullable((ReadOnlySpan<T?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => (IEnumerator<T?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromMatchListAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<T?>?>, IEnumerable<T?>
{
    public IReadOnlyList<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatch(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromMatchEnumerableAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<T?>?>, IEnumerable<T?>
{
    public IEnumerable<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromMatchEnumeratorAddAllSimpleOrderedCollectionStringBearer<T> : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<T?>?>, IEnumerable<T?>
{
    public IEnumerator<T?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllMatchEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T?> GetEnumerator() => Value!;
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromObjectArrayAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<object?[]?>
  , IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<object[]?>
  , IEnumerable<object>
{
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsSpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableSpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<object?[]?>
  , IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable(Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsStruct | AcceptsClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString, IMoldSupportedValue<object[]?>
  , IEnumerable<object>
{
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject((ReadOnlySpan<object>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableStruct | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | AcceptsArray | AcceptsList | AcceptsDictionary | AcceptsEnumerable | AcceptsEnumerator | SupportsValueFormatString)]
public class OrderedFromObjectNullableReadOnlySpanAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<object?[]?>, IEnumerable<object?>
{
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectNullable((ReadOnlySpan<object?>)Value.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => (IEnumerator<object?>)Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsList | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromObjectListAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IReadOnlyList<object?>?>, IEnumerable<object?>
{
    public IReadOnlyList<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObject(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerable | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromObjectEnumerableAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerable<object?>?>, IEnumerable<object?>
{
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!.GetEnumerator();
}

[TypeGeneratePart(CollectionType | AcceptsCollection | AcceptsEnumerator | AcceptsAny | SupportsValueFormatString)]
public class OrderedFromObjectEnumeratorAddAllSimpleOrderedCollectionStringBearer : IStringBearer, ISupportsValueFormatString
  , IMoldSupportedValue<IEnumerator<object?>?>, IEnumerable<object?>
{
    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllObjectEnumerate(Value, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object?> GetEnumerator() => Value!;
}
