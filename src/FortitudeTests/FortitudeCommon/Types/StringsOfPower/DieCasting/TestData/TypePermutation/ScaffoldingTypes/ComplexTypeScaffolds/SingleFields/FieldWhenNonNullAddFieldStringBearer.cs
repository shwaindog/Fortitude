// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
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
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonNullStringBearer<TFmt> : FormattedMoldScaffold<TFmt?>
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddSpanFormattableAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonNullStringBearer<TFmtStruct> : FormattedMoldScaffold<TFmtStruct?>, ISupportsFieldHandling
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonNullStringBearer<TTCloaked, TCloakedRevealerBase> : ValueRevealerMoldScaffold<TTCloaked?, TCloakedRevealerBase>
  , ISupportsFieldHandling where TTCloaked : TCloakedRevealerBase
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonNullStringBearer<TCloakedStruct> : ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>
  , ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs),
                ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerWhenNonNullStringBearer<TBearer> : MoldScaffoldBase<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealStringBearerAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonNullStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
  , ISupportsFieldHandling
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | NonNullWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanWhenNonNullStringBearer : FormattedMoldScaffold<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullAddCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

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
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonNullStringBearer : FormattedMoldScaffold<string>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

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
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringWhenNonNullStringBearer : FormattedMoldScaffold<string?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringAs);
    public FieldContentHandling FieldContentHandling { get; set; }

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
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonNullStringBearer : FormattedMoldScaffold<string?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs);

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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsArray | SupportsValueFormatString)]
public class FieldCharArrayWhenNonNullStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs);
    public FieldContentHandling FieldContentHandling { get; set; }

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
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonNullStringBearer : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs);

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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonNullStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs);
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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceAs
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonNullStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs);

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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonNullStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs);
    public FieldContentHandling FieldContentHandling { get; set; }

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
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonNullStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs);

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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchWhenNonNullStringBearer<TAny> : FormattedMoldScaffold<TAny?>, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldWhenNonNullAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddMatch);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddMatch
               (nameof(ComplexTypeFieldWhenNonNullAddMatch)
              , ComplexTypeFieldWhenNonNullAddMatch
              , ValueFormatString, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectWhenNonNullStringBearer : FormattedMoldScaffold<object?>, ISupportsFieldHandling
{
    public object? ComplexTypeFieldWhenNonNullAddObject
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddObject);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddObject
               (nameof(ComplexTypeFieldWhenNonNullAddObject)
              , ComplexTypeFieldWhenNonNullAddObject
              , ValueFormatString, FieldContentHandling)
           .Complete();
}
