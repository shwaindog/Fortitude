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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<bool?>, IMoldSupportedDefaultValue<bool?>
  
{
    public bool? ComplexTypeFieldWhenNonNullOrDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultBool);
    public bool? DefaultValue { get; set; } = false;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultBool)
              , ComplexTypeFieldWhenNonNullOrDefaultBool
              , DefaultValue.IfNullableGetNonNullableUnderlyingDefault(), ValueFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonNullOrDefaultStringBearer<TFmt> : FormattedMoldScaffold<TFmt?>
  , IMoldSupportedDefaultValue<TFmt?>, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs);
    public TFmt? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonNullOrDefaultStringBearer<TFmtStruct> : FormattedMoldScaffold<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct?>, ISupportsFieldHandling
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs);
    public TFmtStruct? DefaultValue { get; set; } = default(TFmtStruct);

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
              , DefaultValue.IfNullableGetNonNullableUnderlyingDefault(), ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonNullOrDefaultStringBearer<TCloaked, TCloakedRevealerBase> : 
    ValueRevealerMoldScaffold<TCloaked?, TCloakedRevealerBase>
  , IMoldSupportedDefaultValue<TCloaked>, ISupportsFieldHandling
    where TCloaked : TCloakedRevealerBase
{
    public TCloaked? ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs);
    public TCloaked DefaultValue { get; set; } = default!;

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonNullOrDefaultStringBearer<TCloakedStruct> : 
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsFieldHandling
    where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs);
    public TCloakedStruct? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerWhenNonNullOrDefaultStringBearer<TBearer> : MoldScaffoldBase<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs);
    public TBearer? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonNullOrDefaultStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>, ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs);
    public TBearerStruct? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<string>
  , IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<string?>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<string?>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString
                | SupportsSettingDefaultValue)]
public class FieldCharArrayWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<char[]>
  , IMoldSupportedDefaultValue<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonNullOrDefaultStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

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
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharSequence | SupportsValueFormatString |
                  SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonNullOrDefaultStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

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
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStringBuilder | SupportsValueFormatString |
                  SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
  , ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs);
    public string DefaultValue { get; set; } = "";

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsAnyGeneric | SupportsValueFormatString
                 )]
public class FieldMatchWhenNonNullOrDefaultStringBearer<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<TAny?>, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldWhenNonNullOrDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultMatch);
    public TAny? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddMatch
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultMatch)
              , ComplexTypeFieldWhenNonNullOrDefaultMatch
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableObject | SupportsValueFormatString
                 )]
public class FieldObjectWhenNonNullOrDefaultStringBearer : FormattedMoldScaffold<object?>, IMoldSupportedDefaultValue<object?>
  , ISupportsFieldHandling
{
    public object? ComplexTypeFieldWhenNonNullOrDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultObject);
    public object? DefaultValue { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddObject
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultObject)
              , ComplexTypeFieldWhenNonNullOrDefaultObject
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
