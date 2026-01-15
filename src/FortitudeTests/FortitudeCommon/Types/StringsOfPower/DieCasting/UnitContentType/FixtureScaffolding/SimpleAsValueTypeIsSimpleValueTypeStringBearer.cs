// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType.FixtureScaffolding;

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsValueOut)]
public class ComplexContentAsValueBool : FormattedMoldScaffold<bool>
{
    public bool BoolComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(BoolComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(BoolComplexContentAsValue)
              , BoolComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsValueOut)]
public class SimpleContentAsValueBool : FormattedMoldScaffold<bool>
{
    public bool BoolSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (BoolSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut)]
public class ComplexContentAsValueNullableBool : FormattedMoldScaffold<bool?>
{
    public bool? NullableBoolComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableBoolComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsValueOrNull
               (nameof(NullableBoolComplexContentAsValue)
              , NullableBoolComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueNullableBool : FormattedMoldScaffold<bool?>
{
    public bool? NullableBoolSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (NullableBoolSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueSpanFormattable<TFmt> : FormattedMoldScaffold<TFmt>
  , IEquatable<ComplexContentAsValueSpanFormattable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public ComplexContentAsValueSpanFormattable() => Value = default!;
    public ComplexContentAsValueSpanFormattable(TFmt value) => Value = value;
    public TFmt SpanFormattableComplexContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsValue
               (nameof(SpanFormattableComplexContentAsValue)
              , SpanFormattableComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public bool Equals(ComplexContentAsValueSpanFormattable<TFmt>? other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other!.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexContentAsValueSpanFormattable<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueSpanFormattable<TFmt> : FormattedMoldScaffold<TFmt>
  , IEquatable<SimpleContentAsValueSpanFormattable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public SimpleContentAsValueSpanFormattable() => Value = default!;
    public SimpleContentAsValueSpanFormattable(TFmt value) => Value = value;
    public TFmt SpanFormattableSimpleContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (SpanFormattableSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public bool Equals(SimpleContentAsValueSpanFormattable<TFmt>? other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other!.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleContentAsValueSpanFormattable<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public struct ComplexStructContentAsValueSpanFormattable<TFmt> : ISinglePropertyTestStringBearer
  , IEquatable<ComplexStructContentAsValueSpanFormattable<TFmt>>
    where TFmt : ISpanFormattable
{
    public ComplexStructContentAsValueSpanFormattable() { }

    public ComplexStructContentAsValueSpanFormattable(TFmt value) => Value = value;
    public TFmt? SpanFormattableComplexStructContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(SpanFormattableComplexStructContentAsValue);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(SpanFormattableComplexStructContentAsValue)
              , SpanFormattableComplexStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public FormatFlags FormattingFlags { get; set; }

    public TFmt? Value { get; set; } = default!;

    public PalantírReveal<TFmt> CreateRevealer
    {
        get
        {
            var formatString    = ValueFormatString;
            var formattingFlags = FormattingFlags;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(SpanFormattableComplexStructContentAsValue)}"
                      , cloaked
                      , formatString
                      , formattingFlags)
                   .Complete();
        }
    }

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public string? ValueFormatString { get; set; }


    public bool Equals(ComplexStructContentAsValueSpanFormattable<TFmt> other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexStructContentAsValueSpanFormattable<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public struct SimpleStructContentAsValueSpanFormattable<TFmt> : ISinglePropertyTestStringBearer
  , IEquatable<SimpleStructContentAsValueSpanFormattable<TFmt>>
    where TFmt : ISpanFormattable
{
    public SimpleStructContentAsValueSpanFormattable() { }

    public SimpleStructContentAsValueSpanFormattable(TFmt value) => Value = value;
    public TFmt? SpanFormattableSimpleStructContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(SpanFormattableSimpleStructContentAsValue);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(SpanFormattableSimpleStructContentAsValue)
              , SpanFormattableSimpleStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public FormatFlags FormattingFlags { get; set; }

    public TFmt? Value { get; set; } = default!;

    public PalantírReveal<TFmt> CreateRevealer
    {
        get
        {
            var formatString    = ValueFormatString;
            var formattingFlags = FormattingFlags;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(SpanFormattableSimpleStructContentAsValue)}"
                      , cloaked
                      , formatString
                      , formattingFlags)
                   .Complete();
        }
    }

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public string? ValueFormatString { get; set; }


    public bool Equals(SimpleStructContentAsValueSpanFormattable<TFmt> other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleStructContentAsValueSpanFormattable<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueSpanFormattableOrNull<TFmt> : FormattedMoldScaffold<TFmt> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(SpanFormattableOrNullComplexContentAsValue)
              , SpanFormattableOrNullComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueSpanFormattableOrNull<TFmt> : FormattedMoldScaffold<TFmt>
    where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullSimpleContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (SpanFormattableOrNullSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct ComplexStructContentAsValueSpanFormattableOrNull<TFmt> : ISinglePropertyTestStringBearer,
    IEquatable<ComplexStructContentAsValueSpanFormattableOrNull<TFmt>>
    where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullComplexStructContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SpanFormattableOrNullComplexStructContentAsValue);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(SpanFormattableOrNullComplexStructContentAsValue)
              , SpanFormattableOrNullComplexStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public TFmt Value { get; set; }

    public bool Equals(ComplexStructContentAsValueSpanFormattableOrNull<TFmt> other) =>
        Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexStructContentAsValueSpanFormattableOrNull<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct SimpleStructContentAsValueSpanFormattableOrNull<TFmt> : ISinglePropertyTestStringBearer,
    IEquatable<SimpleStructContentAsValueSpanFormattableOrNull<TFmt>>
    where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullSimpleStructContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SpanFormattableOrNullSimpleStructContentAsValue);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (SpanFormattableOrNullSimpleStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public TFmt Value { get; set; }

    public bool Equals(SimpleStructContentAsValueSpanFormattableOrNull<TFmt> other) =>
        Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleStructContentAsValueSpanFormattableOrNull<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull)]
public class ComplexContentAsValueSpanFormattableOrDefault<TFmt> :
    FormattedMoldScaffold<TFmt?>, IMoldSupportedDefaultValue<TFmt> where TFmt : ISpanFormattable?
{
    public TFmt? SpanFormattableOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrDefaultComplexContentAsValue);

    public TFmt DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(SpanFormattableOrDefaultComplexContentAsValue)
              , SpanFormattableOrDefaultComplexContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueNullableSpanFormattableOrStringDefault<TFmt> :
    FormattedMoldScaffold<TFmt>, IMoldSupportedDefaultValue<string> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrStringDefaultSimpleContentAsValueNullable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrStringDefaultSimpleContentAsValueNullable);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (SpanFormattableOrStringDefaultSimpleContentAsValueNullable
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructSpanFormattableOrNull<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(NullableStructSpanFormattableOrNullComplexContentAsValue)
              , NullableStructSpanFormattableOrNullComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructSpanFormattableOrNull<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (NullableStructSpanFormattableOrNullSimpleContentAsValue,
                tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructSpanFormattableOrDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrDefaultComplexContentAsValue);

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(NullableStructSpanFormattableOrDefaultComplexContentAsValue)
              , NullableStructSpanFormattableOrDefaultComplexContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructSpanFormattableOrDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrDefaultSimpleContentAsValue);

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (NullableStructSpanFormattableOrDefaultSimpleContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public struct ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    ISinglePropertyTestStringBearer,
    IEquatable<ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct>>,
    IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault() { }

    public ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault(TFmtStruct? value)
    {
        Value = value;
    }

    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultComplexStructContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(NullableStructSpanFormattableOrStringDefaultComplexStructContentAsValue);

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsValueOrNull
               (nameof(NullableStructSpanFormattableOrStringDefaultComplexStructContentAsValue)
              , NullableStructSpanFormattableOrStringDefaultComplexStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public TFmtStruct? Value { get; set; }

    public bool Equals(ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> other) =>
        Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public struct SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    ISinglePropertyTestStringBearer,
    IEquatable<SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct>>,
    IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault() { }

    public SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault(TFmtStruct? value)
    {
        Value = value;
    }

    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultSimpleStructContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(NullableStructSpanFormattableOrStringDefaultSimpleStructContentAsValue);

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this).AsValueOrNull
               (NullableStructSpanFormattableOrStringDefaultSimpleStructContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public TFmtStruct? Value { get; set; }

    public bool Equals(SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> other) =>
        Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleStructContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               ( nameof(NullableStructSpanFormattableOrStringDefaultComplexContentAsValue)
              , NullableStructSpanFormattableOrStringDefaultComplexContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (NullableStructSpanFormattableOrStringDefaultSimpleContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueCloakedBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerComplexContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValue
               (nameof(CloakedBearerComplexContentAsValue)
              , CloakedBearerComplexContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueCloakedBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerSimpleContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValue
               (CloakedBearerSimpleContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueCloakedBearerOrNull<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerOrNullComplexContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrNull
               (nameof(CloakedBearerOrNullComplexContentAsValue)
              , CloakedBearerOrNullComplexContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueCloakedBearerOrNull<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerOrNullSimpleContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrNull
               (CloakedBearerOrNullSimpleContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer 
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueCloakedBearerOrDefault<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrDefault
               (nameof(CloakedBearerOrDefaultComplexContentAsValue)
              , CloakedBearerOrDefaultComplexContentAsValue
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueCloakedBearerOrDefault<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrDefault
               (CloakedBearerOrDefaultSimpleContentAsValue
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructCloakedBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValue
               (nameof(NullableStructCloakedBearerComplexContentAsValue)
              , NullableStructCloakedBearerComplexContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructCloakedBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValue
               (NullableStructCloakedBearerSimpleContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructCloakedBearerOrNull<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrNull
               (nameof(NullableStructCloakedBearerOrNullComplexContentAsValue)
              , NullableStructCloakedBearerOrNullComplexContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueNullableCloakedBearerOrNull<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableCloakedBearerOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrNull
               (NullableCloakedBearerOrNullSimpleContentAsValue
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer 
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueNullableStructCloakedBearerOrDefault<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrDefault
               (nameof(NullableStructCloakedBearerOrDefaultComplexContentAsValue)
              , NullableStructCloakedBearerOrDefaultComplexContentAsValue
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueNullableCloakedBearerOrDefault<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableCloakedBearerOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrDefault
               (NullableCloakedBearerOrDefaultSimpleContentAsValue
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType  | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer>
    where TBearer : IStringBearer
{
    public TBearer StringBearerComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValue
               (nameof(StringBearerComplexContentAsValue)
              , StringBearerComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer>
    where TBearer : IStringBearer
{
    public TBearer StringBearerSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValue
               (StringBearerSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer 
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueStringBearerOrNull<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? StringBearerOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrNull
               (nameof(StringBearerOrNullComplexContentAsValue)
              , StringBearerOrNullComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueStringBearerOrNull<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? StringBearerOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrNullSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrNull
               (StringBearerOrNullSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer 
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueStringBearerOrStringDefault<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
  , IMoldSupportedDefaultValue<string> where TBearer : IStringBearer
{
    public TBearer? StringBearerOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrDefault
               (nameof(StringBearerOrStringDefaultComplexContentAsValue)
              , StringBearerOrStringDefaultComplexContentAsValue, DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer 
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueStringBearerOrStringDefault<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
  , IMoldSupportedDefaultValue<string> where TBearer : IStringBearer
{
    public TBearer? StringBearerOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrStringDefaultSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrDefault
               (StringBearerOrStringDefaultSimpleContentAsValue, DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValue
               (nameof(NullableStructStringBearerComplexContentAsValue)
              , NullableStructStringBearerComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValue
               (NullableStructStringBearerSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class ComplexContentAsValueNullableStructStringBearerOrNull<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrNullComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrNull
               (nameof(NullableStructStringBearerOrNullComplexContentAsValue)
              , NullableStructStringBearerOrNullComplexContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleContentAsValueNullableStructStringBearerOrNull<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrNullSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrNull
               (NullableStructStringBearerOrNullSimpleContentAsValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueNullableStructStringBearerOrStringDefault<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?>, IMoldSupportedDefaultValue<string> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsValueOrDefault
               (nameof(NullableStructStringBearerOrStringDefaultComplexContentAsValue)
              , NullableStructStringBearerOrStringDefaultComplexContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueNullableStructStringBearerOrStringDefault<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?>, IMoldSupportedDefaultValue<string> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrStringDefaultSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsValueOrDefault
               (NullableStructStringBearerOrStringDefaultSimpleContentAsValue
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray 
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueCharSpan : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharSpanComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanComplexContentAsValue);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(CharSpanComplexContentAsValue)
              , CharSpanComplexContentAsValue.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray 
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueCharSpan : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharSpanSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanSimpleContentAsValue);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (CharSpanSimpleContentAsValue.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray 
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                | DefaultBecomesFallbackString)]
public class ComplexContentAsValueCharSpanOrDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] CharSpanOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(CharSpanOrDefaultComplexContentAsValue)
              , CharSpanOrDefaultComplexContentAsValue.AsSpan()
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueCharSpanOrDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] CharSpanOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrDefaultSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (CharSpanOrDefaultSimpleContentAsValue.AsSpan()
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | DefaultTreatedAsValueOut
                | DefaultBecomesZero | SupportsValueFormatString)]
public class ComplexContentAsValueCharSpanOrZero : FormattedMoldScaffold<char[]>, ISupportsSettingValueFromString
{
    public char[] CharSpanOrZeroComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrZeroComplexContentAsValue);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrZero
               ( nameof(CharSpanOrZeroComplexContentAsValue)
              , CharSpanOrZeroComplexContentAsValue.AsSpan()
              , ValueFormatString  
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | DefaultTreatedAsValueOut
                | DefaultBecomesZero | SupportsValueFormatString)]
public class SimpleContentAsValueCharSpanOrZero : FormattedMoldScaffold<char[]>
{
    public char[] CharSpanOrZeroSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrZeroSimpleContentAsValue);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrZero
               (CharSpanOrZeroSimpleContentAsValue.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueReadOnlyCharSpan : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanComplexContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(ReadOnlyCharSpanComplexContentAsValue)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueReadOnlyCharSpan : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string ReadOnlyCharSpanSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               ((ReadOnlySpan<char>)ReadOnlyCharSpanSimpleContentAsValue
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueReadOnlyCharSpanOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrNullComplexContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               ((ReadOnlySpan<char>)nameof(ReadOnlyCharSpanOrNullComplexContentAsValue)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanOrNullComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueReadOnlyCharSpanOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrNullSimpleContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               ((ReadOnlySpan<char>)ReadOnlyCharSpanOrNullSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut
                | DefaultBecomesZero | SupportsValueFormatString)]
public class ComplexContentContentAsValueReadOnlyCharSpanOrZero : FormattedMoldScaffold<string>, ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanOrZeroComplexContentContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrZeroComplexContentContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrZero
               ( nameof(ReadOnlyCharSpanOrZeroComplexContentContentAsValue)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanOrZeroComplexContentContentAsValue
              , ValueFormatString  
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut
                | DefaultBecomesZero | SupportsValueFormatString)]
public class SimpleContentAsValueReadOnlyCharSpanOrZero : FormattedMoldScaffold<string>
{
    public string ReadOnlyCharSpanOrZeroSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrZero
               ((ReadOnlySpan<char>)ReadOnlyCharSpanOrZeroSimpleContentAsValue
              , ValueFormatString  
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackString)]
public class ComplexContentAsValueReadOnlyCharSpanOrStringDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string ReadOnlyCharSpanOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrStringDefaultComplexContentAsValue);
    
    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(ReadOnlyCharSpanOrStringDefaultComplexContentAsValue)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanOrStringDefaultComplexContentAsValue
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType  | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackString)]
public class SimpleContentAsValueReadOnlyCharSpanOrDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string ReadOnlyCharSpanOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrDefaultSimpleContentAsValue);
    
    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               ((ReadOnlySpan<char>)ReadOnlyCharSpanOrDefaultSimpleContentAsValue
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueString : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string StringComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringComplexContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(StringComplexContentAsValue)
              , StringComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueString : FormattedMoldScaffold<string>, ISupportsSettingValueFromString
{
    public string StringSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringSimpleContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (StringSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueStringOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string StringOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringOrNullComplexContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(StringOrNullComplexContentAsValue)
              , StringOrNullComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueStringOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string StringOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringOrNullSimpleContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (StringOrNullSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct ComplexStructContentAsValueString : ISinglePropertyTestStringBearer, ISupportsSettingValueFromString
{
    public string StringComplexStructContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(StringComplexStructContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(StringComplexStructContentAsValue)
              , StringComplexStructContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public string? Value { get; set; }

    public override string ToString() => $"{GetType().CachedCSharpNameNoConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct SimpleStructContentAsValueString : ISinglePropertyTestStringBearer, ISupportsSettingValueFromString
{
    public string StringSimpleStructContentAsValue
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(StringSimpleStructContentAsValue);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (StringSimpleStructContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FormatFlags FormattingFlags { get; set; }

    public string? Value { get; set; }

    public override string ToString() => $"{GetType().CachedCSharpNameNoConstraints()}({Value})";
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueStringRange : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(StringRangeComplexContentAsValue)
              , StringRangeComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueStringRange : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeSimpleContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (StringRangeSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueStringRangeOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrNullComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(StringRangeOrNullComplexContentAsValue)
              , StringRangeOrNullComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueStringRangeOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrNullSimpleContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (StringRangeOrNullSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueStringRangeOrDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string StringRangeOrDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(StringRangeOrDefaultComplexContentAsValue)
              , StringRangeOrDefaultComplexContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueStringRangeOrDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string StringRangeOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrDefaultSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (StringRangeOrDefaultSimpleContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class ComplexContentAsValueCharArray : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharArrayComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayComplexContentAsValue);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(CharArrayComplexContentAsValue)
              , CharArrayComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleContentAsValueCharArray : FormattedMoldScaffold<char[]>, ISupportsSettingValueFromString
{
    public char[] CharArraySimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArraySimpleContentAsValue);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (CharArraySimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueCharArrayRange : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(CharArrayRangeComplexContentAsValue)
              , CharArrayRangeComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueCharArrayRange : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeSimpleContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (CharArrayRangeSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueCharArrayRangeOrNull : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeOrNullComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(CharArrayRangeOrNullComplexContentAsValue)
              , CharArrayRangeOrNullComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueCharArrayRangeOrNull : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (CharArrayRangeOrNullSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueCharArrayRangeOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(CharArrayRangeOrStringDefaultComplexContentAsValue)
              , CharArrayRangeOrStringDefaultComplexContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueCharArrayRangeOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (CharArrayRangeOrStringDefaultSimpleContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueCharSequence<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceComplexContentAsValue);

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);

            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(CharSequenceComplexContentAsValue)
              , CharSequenceComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleContentAsValueCharSequence<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString
    where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);

            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (CharSequenceSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges 
                 | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueCharSequenceRange<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(CharSequenceRangeComplexContentAsValue)
              , CharSequenceRangeComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueCharSequenceRange<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);

            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (CharSequenceRangeSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueCharSequenceRangeOrNull<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeOrNullComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(CharSequenceRangeOrNullComplexContentAsValue)
              , CharSequenceRangeOrNullComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueCharSequenceRangeOrNull<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (CharSequenceRangeOrNullSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString 
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueCharSequenceRangeOrStringDefault<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(CharSequenceRangeOrStringDefaultComplexContentAsValue)
              , CharSequenceRangeOrStringDefaultComplexContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueCharSequenceRangeOrStringDefault<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (CharSequenceRangeOrStringDefaultSimpleContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueStringBuilder : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder StringBuilderComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderComplexContentAsValue);

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(StringBuilderComplexContentAsValue)
              , StringBuilderComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueStringBuilder : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder StringBuilderSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (StringBuilderSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class ComplexContentAsValueStringBuilderRange : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder StringBuilderRangeComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValue
               (nameof(StringBuilderRangeComplexContentAsValue)
              , StringBuilderRangeComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleContentAsValueStringBuilderRange : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder StringBuilderRangeSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValue
               (StringBuilderRangeSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueStringBuilderRangeOrNull : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeOrNullComplexContentAsValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrNull
               (nameof(StringBuilderRangeOrNullComplexContentAsValue)
              , StringBuilderRangeOrNullComplexContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueStringBuilderRangeOrNull : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrNull
               (StringBuilderRangeOrNullSimpleContentAsValue
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsValueStringBuilderRangeOrStringDefault : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueOrDefault
               (nameof(StringBuilderRangeOrStringDefaultComplexContentAsValue)
              , StringBuilderRangeOrStringDefaultComplexContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsValueStringBuilderRangeOrDefault : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueOrDefault
               (StringBuilderRangeOrDefaultSimpleContentAsValue
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueMatch<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchComplexContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueMatch
               (nameof(MatchComplexContentAsValue)
              , MatchComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueMatch<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchSimpleContentAsValue);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueMatch
               (MatchSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class ComplexContentAsValueMatchOrNull<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchOrNullComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrNullComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueMatchOrNull
               (nameof(MatchOrNullComplexContentAsValue)
              , MatchOrNullComplexContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleContentAsValueMatchOrNull<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchOrNullSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrNullSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueMatchOrNull
               (MatchOrNullSimpleContentAsValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString)]
public class ComplexContentAsValueMatchOrStringDefault<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? MatchOrStringDefaultComplexContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrStringDefaultComplexContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsValueMatchOrDefault
               (nameof(MatchOrStringDefaultComplexContentAsValue)
              , MatchOrStringDefaultComplexContentAsValue
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString)]
public class SimpleContentAsValueMatchOrStringDefault<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? MatchOrStringDefaultSimpleContentAsValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrStringDefaultSimpleContentAsValue);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsValueMatchOrDefault
               (MatchOrStringDefaultSimpleContentAsValue
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}
