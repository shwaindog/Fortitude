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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolWhenNonDefaultStringBearer : IMoldSupportedValue<bool>, IMoldSupportedDefaultValue<bool>, ISupportsValueFormatString
{
    public bool ComplexTypeFieldWhenNonDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultBool);
    public bool DefaultValue { get; set; } = false;

    public bool Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultBool)
              , ComplexTypeFieldWhenNonDefaultBool
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWhenNonDefaultStringBearer<TFmt> : IMoldSupportedValue<TFmt>, IMoldSupportedDefaultValue<TFmt>
  , ISupportsValueFormatString, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs);
    public TFmt DefaultValue { get; set; } = default!;

    public TFmt Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs)
              , ComplexTypeFieldWhenNonDefaultSpanFormattableAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWhenNonDefaultStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct?>, ISupportsValueFormatString, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs);
    public TFmtStruct? DefaultValue { get; set; } = default(TFmtStruct);

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWhenNonDefaultStringBearer<TCloaked, TCloakedBase>
    : IMoldSupportedValue<TCloaked>, IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase>
      , ISupportsFieldHandling where TCloaked : TCloakedBase
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field
           .WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWhenNonDefaultStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloakedStruct? DefaultValue { get; set; } = default(TCloakedStruct);

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWhenNonDefaultStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonDefaultStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class FieldNullableStringBearerWhenNonDefaultStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>, ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonDefaultStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs)
              , ComplexTypeFieldWhenNonDefaultCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonDefaultWrites | AcceptsString | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWhenNonDefaultStringBearer : IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs);
    public string? ValueFormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs)
              , ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsString | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsFieldHandling, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultStringAs)
              , ComplexTypeFieldWhenNonDefaultStringAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs)
              , ComplexTypeFieldWhenNonDefaultStringRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs)
              , ComplexTypeFieldWhenNonDefaultCharArrayAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs)
              , ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs)
              , ComplexTypeFieldWhenNonDefaultCharSequenceAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

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
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

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
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>,
    ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs)
              , ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd
               (nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldMatchWhenNonDefaultStringBearer<TAny> : IMoldSupportedValue<TAny?>, IMoldSupportedDefaultValue<TAny?>
  , ISupportsValueFormatString, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldWhenNonDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultMatch);
    public TAny? DefaultValue { get; set; }

    public TAny? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddMatch
               (nameof(ComplexTypeFieldWhenNonDefaultMatch)
              , ComplexTypeFieldWhenNonDefaultMatch, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }
    
    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonDefaultWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldObjectWhenNonDefaultStringBearer : IMoldSupportedValue<object?>, IMoldSupportedDefaultValue<object?>
  , ISupportsValueFormatString, ISupportsFieldHandling
{
    public object? ComplexTypeFieldWhenNonDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultObject);
    public object? DefaultValue { get; set; }

    public object? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddObject
               (nameof(ComplexTypeFieldWhenNonDefaultObject)
              , ComplexTypeFieldWhenNonDefaultObject, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }
    
    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
