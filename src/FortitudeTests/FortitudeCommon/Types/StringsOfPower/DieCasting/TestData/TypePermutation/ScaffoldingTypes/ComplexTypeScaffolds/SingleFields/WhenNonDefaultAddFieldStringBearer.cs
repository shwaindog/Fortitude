using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<bool>,  IMoldSupportedDefaultValue<bool>
{
    public bool ComplexTypeFieldWhenNonDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public bool Value { get; set; }

    public bool DefaultValue { get; set; } = false;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultBool), ComplexTypeFieldWhenNonDefaultBool, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableWhenNonDefaultStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, IMoldSupportedDefaultValue<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt Value { get; set; } = default!;

    public TFmt DefaultValue { get; set; } = default!;
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultSpanFormattable), ComplexTypeFieldWhenNonDefaultSpanFormattable, DefaultValue
                                  , FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class SpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, IMoldSupportedDefaultValue<TFmt>, ISupportsSingleFormatString
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmt Value { get; set; } = default!;

    public TFmt DefaultValue { get; set; } = default!;
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultSpanFormattable), ComplexTypeFieldWhenNonDefaultSpanFormattable
                                    , DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableWhenNonDefaultStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>
  , ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; }
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattable)
                                  , ComplexTypeFieldWhenNonDefaultNullableSpanFormattable, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class NullableSpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>
  , ISupportsSingleFormatString, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; }
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattable)
                                    , ComplexTypeFieldWhenNonDefaultNullableSpanFormattable, DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerWhenNonDefaultStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked>, IMoldSupportedDefaultValue<TCloaked>, ISupportsSingleRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public TCloaked DefaultValue { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer), ComplexTypeFieldWhenNonDefaultCloakedBearer
                                     , PalantirRevealer
                                     , DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsChars
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class CloakedBearerWithHandlingWhenNonDefaultStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, IMoldSupportedValue<TCloaked>, IMoldSupportedDefaultValue<TCloaked>, ISupportsSingleRevealer<TCloakedBase>, ISupportsFieldHandling where TCloaked : TCloakedBase
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public TCloaked Value { get; set; } = default!;

    public TCloaked DefaultValue { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field
           .WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer)
                                 , ComplexTypeFieldWhenNonDefaultCloakedBearer
                                 , PalantirRevealer, DefaultValue, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerWhenNonDefaultStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>, IMoldSupportedDefaultValue<TCloakedStruct?>
  , ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public TCloakedStruct? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer), ComplexTypeFieldWhenNonDefaultCloakedBearer, PalantirRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class NullableCloakedBearerWithHandlingWhenNonDefaultStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>, IMoldSupportedDefaultValue<TCloakedStruct?>
  , ISupportsSingleRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? Value { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? DefaultValue { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs), ComplexTypeFieldWhenNonDefaultCloakedBearerAs
                                       , PalantirRevealer, DefaultValue, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerWhenNonDefaultStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>, IMoldSupportedDefaultValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public TBearer? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultStringBearer), ComplexTypeFieldWhenNonDefaultStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class StringBearerWithHandlingWhenNonDefaultStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>, IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearer? Value { get; set; }

    public TBearer? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs), ComplexTypeFieldWhenNonDefaultStringBearerAs, DefaultValue
                                       , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class NullableStringBearerWhenNonDefaultStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>, IMoldSupportedDefaultValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct? Value { get; set; }

    public TBearerStruct? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultStringBearer), ComplexTypeFieldWhenNonDefaultStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class NullableStringBearerWithHandlingWhenNonDefaultStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>, IMoldSupportedDefaultValue<TBearerStruct?> 
  , ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearerStruct? Value { get; set; }

    public TBearerStruct? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs), ComplexTypeFieldWhenNonDefaultStringBearerAs, DefaultValue
                                       , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSpanWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharSpan), ComplexTypeFieldWhenNonDefaultCharSpan.AsSpan(), DefaultValue
                                  , FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharSpanWithHandlingWhenNonDefaultAddStringBearer  : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }
    
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs), ComplexTypeFieldWhenNonDefaultCharSpanAs.AsSpan()
                                    , DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharReadOnlySpanWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string Value { get; set; } = null!;

    public string DefaultValue { get; set; } = "";
    
    public string? FormatString { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpan), ComplexTypeFieldWhenNonDefaultCharReadOnlySpan.AsSpan()
                                  , DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharReadOnlySpanWithHandlingWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlyAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";
    
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlyAs), ComplexTypeFieldWhenNonDefaultCharReadOnlyAs.AsSpan()
                                    , DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultString
    {
        get => Value;
        set => Value = value;
    }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultString), ComplexTypeFieldWhenNonDefaultString, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringWithHandlingWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString, ISupportsFieldHandling
  , ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultString
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";
    public string? FormatString { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultString), ComplexTypeFieldWhenNonDefaultString, DefaultValue, FieldContentHandling
                                    , FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringRangeWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string ComplexTypeFieldWhenNonDefaultStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";
    public string? FormatString { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultStringRange), ComplexTypeFieldWhenNonDefaultStringRange
                                  , FromIndex, Length, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringRangeWithHandlingWhenNonDefaultAddStringBearer : IStringBearer, IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultStringRange
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";
    public string? FormatString { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultStringRange), ComplexTypeFieldWhenNonDefaultStringRange
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharArrayWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArray
    {
        get => Value;
        set => Value = value;
    }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharArray), ComplexTypeFieldWhenNonDefaultCharArray, DefaultValue
                                  , FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharArrayWithHandlingWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs), ComplexTypeFieldWhenNonDefaultCharArrayAs, DefaultValue, FieldContentHandling
                                    , FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharArrayRangeWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharArrayRange), ComplexTypeFieldWhenNonDefaultCharArrayRange
                                  , FromIndex, Length, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharArrayRangeWithHandlingWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }
    
    public FieldContentHandling FieldContentHandling { get; set; }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs), ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceWhenNonDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq Value { get; set; } = default!;

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(ComplexTypeFieldWhenNonDefaultCharSequence), ComplexTypeFieldWhenNonDefaultCharSequence
                                         , DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharSequenceWithHandlingWhenNonDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCharSeq Value { get; set; } = default!;

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }
    public string? FormatString { get; set; }
    
    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(ComplexTypeFieldWhenNonDefaultCharSequence), ComplexTypeFieldWhenNonDefaultCharSequence
                                           , DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class CharSequenceRangeWhenNonDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public TCharSeq Value { get; set; } = default!;

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRange), ComplexTypeFieldWhenNonDefaultCharSequenceRange
                                         , FromIndex, Length, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharSequenceRangeWithHandlingWhenNonDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCharSeq Value { get; set; } = default!;

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRange), ComplexTypeFieldWhenNonDefaultCharSequenceRange
                                           , FromIndex, Length, DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilder), ComplexTypeFieldWhenNonDefaultAddStringBuilder
                                  , DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringBuilderWithHandlingWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>, 
    ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilder), ComplexTypeFieldWhenNonDefaultAddStringBuilder
                                    , DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringBuilderRangeWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRange), ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
                                  , FromIndex, Length, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringBuilderRangeWithHandlingWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? FormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRange), ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchWhenNonDefaultStringBearer<T> : IStringBearer, IMoldSupportedValue<T?>, IMoldSupportedDefaultValue<T?>, ISupportsSingleFormatString
{
    public T? ComplexTypeFieldWhenNonDefaultMatch 
    {
        get => Value;
        set => Value = value;
    }

    public T? Value { get; set; }
    public string? FormatString { get; set; }

    public T? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddMatch(nameof(ComplexTypeFieldWhenNonDefaultMatch), ComplexTypeFieldWhenNonDefaultMatch, DefaultValue, FormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | OneFormatString)]
public class ObjectWhenNonDefaultStringBearer : IStringBearer, IMoldSupportedValue<object?>, IMoldSupportedDefaultValue<object?>
{
    public object? ComplexTypeFieldWhenNonDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public object? Value { get; set; }
    public string? FormatString { get; set; }

    public object? DefaultValue { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddObject(nameof(ComplexTypeFieldWhenNonDefaultObject), ComplexTypeFieldWhenNonDefaultObject, DefaultValue
                                        , FormatString)
           .Complete();
}
