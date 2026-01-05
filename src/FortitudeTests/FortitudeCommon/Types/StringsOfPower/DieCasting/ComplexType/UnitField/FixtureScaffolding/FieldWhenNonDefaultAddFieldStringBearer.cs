// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolWhenNonDefaultStringBearer : FormattedMoldScaffold<bool>, IMoldSupportedDefaultValue<bool>
{
    public bool ComplexTypeFieldWhenNonDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultBool);
    public bool DefaultValue { get; set; } = false;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultBool)
              , ComplexTypeFieldWhenNonDefaultBool
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonDefaultStringBearer<TFmt> : FormattedMoldScaffold<TFmt>, IMoldSupportedDefaultValue<TFmt>
   where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs);
    public TFmt DefaultValue { get; set; } = default!;


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs)
              , ComplexTypeFieldWhenNonDefaultSpanFormattableAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonDefaultStringBearer<TFmtStruct> : FormattedMoldScaffold<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs);
    public TFmtStruct? DefaultValue { get; set; } = default(TFmtStruct);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonDefaultStringBearer<TCloaked, TRevealBase>
    : ValueRevealerMoldScaffold<TCloaked, TRevealBase>, IMoldSupportedDefaultValue<TCloaked>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloaked DefaultValue { get; set; } = default!;


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field
           .WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonDefaultStringBearer<TCloakedStruct> : ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>
  , IMoldSupportedDefaultValue<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloakedStruct? DefaultValue { get; set; } = default(TCloakedStruct);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerWhenNonDefaultStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearer? DefaultValue { get; set; }


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonDefaultStringBearerAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonDefaultStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearerStruct? DefaultValue { get; set; }


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonDefaultStringBearerAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | CallsAsSpan | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanWhenNonDefaultStringBearer : FormattedMoldScaffold<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs)
              , ComplexTypeFieldWhenNonDefaultCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonDefaultWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonDefaultStringBearer : FormattedMoldScaffold<string>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs);

    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs)
              , ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringWhenNonDefaultStringBearer : FormattedMoldScaffold<string>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultStringAs)
              , ComplexTypeFieldWhenNonDefaultStringAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonDefaultStringBearer : FormattedMoldScaffold<string>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string ComplexTypeFieldWhenNonDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs);
    public string DefaultValue { get; set; } = "";


    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs)
              , ComplexTypeFieldWhenNonDefaultStringRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharArrayWhenNonDefaultStringBearer : FormattedMoldScaffold<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs)
              , ComplexTypeFieldWhenNonDefaultCharArrayAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonDefaultStringBearer : FormattedMoldScaffold<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs);
    public string DefaultValue { get; set; } = "";


    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs)
              , ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonDefaultStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs);
    public string DefaultValue { get; set; } = "";


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
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs)
              , ComplexTypeFieldWhenNonDefaultCharSequenceAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonDefaultStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs);
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
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonDefaultStringBearer : FormattedMoldScaffold<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs);
    public string DefaultValue { get; set; } = "";


    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs)
              , ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStringBuilder | SupportsValueFormatString |
                  SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonDefaultStringBearer : FormattedMoldScaffold<StringBuilder>
  , IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs);
    public string DefaultValue { get; set; } = "";


    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchWhenNonDefaultStringBearer<TAny> : FormattedMoldScaffold<TAny?>, IMoldSupportedDefaultValue<TAny?>
  
{
    public TAny? ComplexTypeFieldWhenNonDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultMatch);
    public TAny? DefaultValue { get; set; }



    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddMatch
               (nameof(ComplexTypeFieldWhenNonDefaultMatch)
              , ComplexTypeFieldWhenNonDefaultMatch, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectWhenNonDefaultStringBearer : FormattedMoldScaffold<object?>, IMoldSupportedDefaultValue<object?>
  
{
    public object? ComplexTypeFieldWhenNonDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultObject);
    public object? DefaultValue { get; set; }



    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddObject
               (nameof(ComplexTypeFieldWhenNonDefaultObject)
              , ComplexTypeFieldWhenNonDefaultObject, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}
