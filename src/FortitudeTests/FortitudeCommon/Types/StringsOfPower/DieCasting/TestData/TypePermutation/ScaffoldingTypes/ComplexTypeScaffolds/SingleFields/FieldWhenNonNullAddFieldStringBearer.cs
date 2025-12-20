// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolWhenNonNullStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? ComplexTypeFieldWhenNonNullAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableBool)
              , ComplexTypeFieldWhenNonNullAddNullableBool
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonNullStringBearer<TFmt> : FormattedMoldScaffold<TFmt?>
   where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddSpanFormattableAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonNullStringBearer<TFmtStruct> : FormattedMoldScaffold<TFmtStruct?>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonNullStringBearer<TTCloaked, TRevealBase> : ValueRevealerMoldScaffold<TTCloaked?, TRevealBase>
    where TTCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
              , ValueRevealer, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonNullStringBearer<TCloakedStruct> : ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>
   where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs),
                ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
              , ValueRevealer, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerWhenNonNullStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealStringBearerAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonNullStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | NonNullWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanWhenNonNullStringBearer : FormattedMoldScaffold<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonNullAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSpanAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharSpanAs)
              , ComplexTypeFieldWhenNonNullAddCharSpanAs.AsSpan()
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonNullStringBearer : FormattedMoldScaffold<string>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs)
              , ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs.AsSpan()
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringWhenNonNullStringBearer : FormattedMoldScaffold<string?>
  , ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldWhenNonNullAddStringAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringAs);

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringAs)
              , ComplexTypeFieldWhenNonNullAddStringAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonNullStringBearer : FormattedMoldScaffold<string?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldWhenNonNullAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs);

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringRangeAs
              , FromIndex, Length, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsArray | SupportsValueFormatString)]
public class FieldCharArrayWhenNonNullStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs);

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonNullStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs);

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
              , FromIndex, Length, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonNullStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs);

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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonNullStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs);

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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
              , FromIndex, Length, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonNullStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs);

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderAs
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonNullStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs);

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
              , FromIndex, Length, ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchWhenNonNullStringBearer<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? ComplexTypeFieldWhenNonNullAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddMatch);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddMatch
               (nameof(ComplexTypeFieldWhenNonNullAddMatch)
              , ComplexTypeFieldWhenNonNullAddMatch
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectWhenNonNullStringBearer : FormattedMoldScaffold<object?>
{
    public object? ComplexTypeFieldWhenNonNullAddObject
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddObject);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddObject
               (nameof(ComplexTypeFieldWhenNonNullAddObject)
              , ComplexTypeFieldWhenNonNullAddObject
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
}
