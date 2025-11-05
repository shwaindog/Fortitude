// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolAlwaysAddStringBearer : IMoldSupportedValue<bool>, ISupportsValueFormatString
{
    public bool ComplexTypeFieldAlwaysAddBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddBool);
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddBool)
              , ComplexTypeFieldAlwaysAddBool
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolAlwaysAddStringBearer : IMoldSupportedValue<bool?>, ISupportsValueFormatString
{
    public bool? ComplexTypeFieldAlwaysAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddNullableBool)
              , ComplexTypeFieldAlwaysAddNullableBool
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableAlwaysAddStringBearer<TFmt> : IMoldSupportedValue<TFmt>, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt> where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattable);
    public TFmt Value { get; set; } = default!;

    public PalantírReveal<TFmt> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddSpanFormattable)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddSpanFormattable)
              , ComplexTypeFieldAlwaysAddSpanFormattable
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    protected bool Equals(FieldSpanFormattableAlwaysAddStringBearer<TFmt> other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldSpanFormattableAlwaysAddStringBearer<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmt>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public struct FieldSpanFormattableAlwaysAddStructStringBearer<TFmt> : IMoldSupportedValue<TFmt>, ISupportsValueFormatString
  , IPalantirRevealerFactory<TFmt>, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public FieldSpanFormattableAlwaysAddStructStringBearer() { }
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattableFromStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattableFromStruct);
    public TFmt Value { get; set; } = default!;

    public PalantírReveal<TFmt> CreateRevealer
    {
        get
        {
            var formatString = ValueFormatString;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddSpanFormattableFromStruct)}"
                      , cloaked, formatString)
                   .Complete();
        }
    }

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddSpanFormattableFromStruct)
              , ComplexTypeFieldAlwaysAddSpanFormattableFromStruct
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    private bool Equals(FieldSpanFormattableAlwaysAddStructStringBearer<TFmt> other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldSpanFormattableAlwaysAddStructStringBearer<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmt>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IPalantirRevealerFactory<TFmtStruct>, ISupportsValueFormatString, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable);
    public TFmtStruct? Value { get; set; }

    public PalantírReveal<TFmtStruct> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable)
              , ComplexTypeFieldAlwaysAddNullableSpanFormattable
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    protected bool Equals(FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> other) =>
        EqualityComparer<TFmtStruct?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public struct FieldNullableSpanFormattableAlwaysAddStructStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IPalantirRevealerFactory<TFmtStruct>, ISupportsValueFormatString, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public FieldNullableSpanFormattableAlwaysAddStructStringBearer() { }

    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattableFromStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableFromStruct);
    public TFmtStruct? Value { get; set; }

    public PalantírReveal<TFmtStruct> CreateRevealer
    {
        get
        {
            var formatString = ValueFormatString;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableFromStruct)}"
                      , cloaked, formatString)
                   .Complete();
        }
    }

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableFromStruct)
              , ComplexTypeFieldAlwaysAddNullableSpanFormattableFromStruct
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    private bool Equals(FieldNullableSpanFormattableAlwaysAddStructStringBearer<TFmtStruct> other) =>
        EqualityComparer<TFmtStruct?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldNullableSpanFormattableAlwaysAddStructStringBearer<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerAlwaysAddStringBearer<TTCloaked, TCloakedBase> : IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsValueRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldAlwaysAddCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs);

    public TTCloaked? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs)
              , ComplexTypeFieldAlwaysAddCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerAlwaysAddStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs);

    public TCloakedStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs)
              , ComplexTypeFieldAlwaysAddCloakedBearerStructAs
              , ValueRevealer, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerAlwaysAddStringBearer<TBearer> : IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerAs);

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddStringBearerAs)
              , ComplexTypeFieldAlwaysAddStringBearerAs
              , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerAlwaysAddStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs);

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs)
              , ComplexTypeFieldAlwaysAddStringBearerStructAs
              , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanAlwaysAddStringBearer : IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpanAs);

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanAlwaysAddStringBearer : IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs);

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , IPalantirRevealerFactory<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddString);
    public string? Value { get; set; }

    public PalantírReveal<string> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddString)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddString)
              , ComplexTypeFieldAlwaysAddString
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
    
    public FieldContentHandling FieldContentHandling { get; set; }
    protected bool Equals(FieldStringAlwaysAddStringBearer other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldStringAlwaysAddStringBearer)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public struct FieldStringAlwaysAddStructStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , IPalantirRevealerFactory<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public FieldStringAlwaysAddStructStringBearer() { }

    public string? ComplexTypeFieldAlwaysAddStringFromStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringFromStruct);
    public string? Value { get; set; }

    public PalantírReveal<string> CreateRevealer
    {
        get
        {
            var formatString = ValueFormatString;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddStringFromStruct)}"
                      , cloaked, formatString)
                   .Complete();
        }
    }

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringFromStruct)
              , ComplexTypeFieldAlwaysAddStringFromStruct
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
    public FieldContentHandling FieldContentHandling { get; set; }
    private bool Equals(FieldStringAlwaysAddStructStringBearer other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldStringAlwaysAddStructStringBearer)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringRangeAs);

    public string? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringRangeAs)
              , ComplexTypeFieldAlwaysAddStringRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayAlwaysAddStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayAs);

    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharArrayAs)
              , ComplexTypeFieldAlwaysAddCharArrayAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeAlwaysAddStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs);
    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs)
              , ComplexTypeFieldAlwaysAddCharArrayRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceAs);

    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(ComplexTypeFieldAlwaysAddCharSequenceAs)
              , ComplexTypeFieldAlwaysAddCharSequenceAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs);

    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs)
              , ComplexTypeFieldAlwaysAddCharSequenceRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderAs);

    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringBuilderAs)
              , ComplexTypeFieldAlwaysAddStringBuilderAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs);
    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs)
              , ComplexTypeFieldAlwaysAddStringBuilderRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldMatchAlwaysAddStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldAlwaysAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddMatch);
    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch
               (nameof(ComplexTypeFieldAlwaysAddMatch)
              , ComplexTypeFieldAlwaysAddMatch
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldObjectAlwaysAddStringBearer : IMoldSupportedValue<object?>, ISupportsValueFormatString, ISupportsFieldHandling
{
    public object? ComplexTypeFieldAlwaysAddObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddObject);
    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject
               (nameof(ComplexTypeFieldAlwaysAddObject)
              , ComplexTypeFieldAlwaysAddObject
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }
    
    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
