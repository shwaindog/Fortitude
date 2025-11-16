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
public class FieldBoolAlwaysAddStringBearer : FormattedMoldScaffold<bool>
{
    public bool ComplexTypeFieldAlwaysAddBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddBool)
              , ComplexTypeFieldAlwaysAddBool
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolAlwaysAddStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? ComplexTypeFieldAlwaysAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddNullableBool)
              , ComplexTypeFieldAlwaysAddNullableBool
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableAlwaysAddStringBearer<TFmt> : FormattedMoldScaffold<TFmt>
  , IPalantirRevealerFactory<TFmt> where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattable
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattable);

    public PalantírReveal<TFmt> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddSpanFormattable)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddSpanFormattable)
              , ComplexTypeFieldAlwaysAddSpanFormattable
              , ValueFormatString)
           .Complete();

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
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public struct FieldSpanFormattableAlwaysAddStructStringBearer<TFmt> : IMoldSupportedValue<TFmt>
  , IPalantirRevealerFactory<TFmt>, ISupportsValueFormatString , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public FieldSpanFormattableAlwaysAddStructStringBearer() { }
    public TFmt? ComplexTypeFieldAlwaysAddSpanFormattableFromStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattableFromStruct);

    public FieldContentHandling ContentHandlingFlags { get; set; }
    
    public TFmt? Value { get; set; } = default!;

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
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldSpanFormattableAlwaysAddStructStringBearer<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmt>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> : FormattedMoldScaffold<TFmtStruct?>
  , IPalantirRevealerFactory<TFmtStruct>, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable);

    public PalantírReveal<TFmtStruct> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable)
              , ComplexTypeFieldAlwaysAddNullableSpanFormattable
              , ValueFormatString, FieldContentHandling)
           .Complete();

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
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
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

    public FieldContentHandling ContentHandlingFlags { get; set; }
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
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldNullableSpanFormattableAlwaysAddStructStringBearer<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    #pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    public override int GetHashCode() => EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value);
    #pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer
                 )]
public class FieldCloakedBearerAlwaysAddStringBearer<TTCloaked, TRevealBase> : ValueRevealerMoldScaffold<TTCloaked, TRevealBase>
  , ISupportsFieldHandling 
    where TTCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TTCloaked? ComplexTypeFieldAlwaysAddCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs)
              , ComplexTypeFieldAlwaysAddCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsAnyNullableStruct | SupportsValueRevealer
                 )]
public class FieldNullableCloakedBearerAlwaysAddStringBearer<TCloakedStruct> : ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>
 , ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs)
              , ComplexTypeFieldAlwaysAddCloakedBearerStructAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerAlwaysAddStringBearer<TBearer> : MoldScaffoldBase<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddStringBearerAs)
              , ComplexTypeFieldAlwaysAddStringBearerAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerAlwaysAddStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
  , ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs)
              , ComplexTypeFieldAlwaysAddStringBearerStructAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanAlwaysAddStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpanAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanAlwaysAddStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringAlwaysAddStringBearer : FormattedMoldScaffold<string?>
  , IPalantirRevealerFactory<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddString);

    public PalantírReveal<string> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(ComplexTypeFieldAlwaysAddString)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddString)
              , ComplexTypeFieldAlwaysAddString
              , ValueFormatString, FieldContentHandling)
           .Complete();


    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }
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
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public struct FieldStringAlwaysAddStructStringBearer : IMoldSupportedValue<string?>
  , IPalantirRevealerFactory<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public FieldStringAlwaysAddStructStringBearer() { }

    public string? ComplexTypeFieldAlwaysAddStringFromStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringFromStruct);

    public FieldContentHandling ContentHandlingFlags { get; set; }
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
        if (obj.GetType() != GetType()) return false;
        return Equals((FieldStringAlwaysAddStructStringBearer)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeAlwaysAddStringBearer : FormattedMoldScaffold<string?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringRangeAs);

    public FieldContentHandling FieldContentHandling { get; set; }
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringRangeAs)
              , ComplexTypeFieldAlwaysAddStringRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharArrayAlwaysAddStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharArrayAs)
              , ComplexTypeFieldAlwaysAddCharArrayAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeAlwaysAddStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs)
              , ComplexTypeFieldAlwaysAddCharArrayRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceAlwaysAddStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceAs);

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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(ComplexTypeFieldAlwaysAddCharSequenceAs)
              , ComplexTypeFieldAlwaysAddCharSequenceAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeAlwaysAddStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs);

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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs)
              , ComplexTypeFieldAlwaysAddCharSequenceRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderAlwaysAddStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringBuilderAs)
              , ComplexTypeFieldAlwaysAddStringBuilderAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeAlwaysAddStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs);

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs)
              , ComplexTypeFieldAlwaysAddStringBuilderRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchAlwaysAddStringBearer<TAny> : FormattedMoldScaffold<TAny?>, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldAlwaysAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddMatch);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch
               (nameof(ComplexTypeFieldAlwaysAddMatch)
              , ComplexTypeFieldAlwaysAddMatch
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | AlwaysWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectAlwaysAddStringBearer : FormattedMoldScaffold<object?>, ISupportsFieldHandling
{
    public object? ComplexTypeFieldAlwaysAddObject
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddObject);


    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject
               (nameof(ComplexTypeFieldAlwaysAddObject)
              , ComplexTypeFieldAlwaysAddObject
              , ValueFormatString, FieldContentHandling)
           .Complete();
}
