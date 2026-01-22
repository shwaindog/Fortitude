// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType.FixtureScaffolding;

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class ComplexContentAsStringBool : FormattedMoldScaffold<bool>
{
    public bool BoolComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(BoolComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsString
               (nameof(BoolComplexContentAsString)
              , BoolComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class SimpleContentAsStringBool : FormattedMoldScaffold<bool>
{
    public bool BoolSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (BoolSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | DefaultTreatedAsStringOut
                | SupportsValueFormatString | DefaultBecomesNull)]
public class ComplexContentAsStringNullableBool : FormattedMoldScaffold<bool?>
{
    public bool? NullableBoolComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableBoolComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsStringOrNull
               (nameof(NullableBoolComplexContentAsString)
              , NullableBoolComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | DefaultTreatedAsStringOut | SupportsValueFormatString
                | DefaultBecomesNull)]
public class SimpleContentAsStringNullableBool : FormattedMoldScaffold<bool?>
{
    public bool? NullableBoolSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (NullableBoolSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringSpanFormattable<TFmt> : FormattedMoldScaffold<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt SpanFormattableComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsString
               (nameof(SpanFormattableComplexContentAsString)
              , SpanFormattableComplexContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringSpanFormattable<TFmt> : FormattedMoldScaffold<TFmt>
    where TFmt : ISpanFormattable
{
    public TFmt SpanFormattableSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (SpanFormattableSimpleContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringSpanFormattableOrNull<TFmt> :
    FormattedMoldScaffold<TFmt> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(SpanFormattableOrNullComplexContentAsString)
              , SpanFormattableOrNullComplexContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringSpanFormattableOrNull<TFmt> :
    FormattedMoldScaffold<TFmt> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (SpanFormattableOrNullSimpleContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable
                | SupportsSettingDefaultValue | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringSpanFormattableOrDefault<TFmt> :
    FormattedMoldScaffold<TFmt?>, IMoldSupportedDefaultValue<TFmt> where TFmt : ISpanFormattable
{
    public TFmt? SpanFormattableOrDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrDefaultComplexContentAsString);

    public TFmt DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(SpanFormattableOrDefaultComplexContentAsString)
              , SpanFormattableOrDefaultComplexContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringSpanFormattableOrDefault<TFmt> :
    FormattedMoldScaffold<TFmt?>, IMoldSupportedDefaultValue<TFmt> where TFmt : ISpanFormattable
{
    public TFmt? SpanFormattableOrDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmt DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (SpanFormattableOrDefaultSimpleContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringSpanFormattableOrStringDefault<TFmt> :
    FormattedMoldScaffold<TFmt>, IMoldSupportedDefaultValue<string> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SpanFormattableOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(SpanFormattableOrStringDefaultComplexContentAsString)
              , SpanFormattableOrStringDefaultComplexContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringSpanFormattableOrStringDefault<TFmt> :
    FormattedMoldScaffold<TFmt>, IMoldSupportedDefaultValue<string> where TFmt : ISpanFormattable?
{
    public TFmt SpanFormattableOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (SpanFormattableOrStringDefaultSimpleContentAsString,
                DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringNullableStructSpanFormattableOrNull<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(NullableStructSpanFormattableOrNullComplexContentAsString)
              , NullableStructSpanFormattableOrNullComplexContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringNullableStructSpanFormattableOrNull<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (NullableStructSpanFormattableOrNullSimpleContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable
                | SupportsSettingDefaultValue | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringNullableStructSpanFormattableOrDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrDefaultComplexContentAsString);

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsStringOrDefault
               (nameof(NullableStructSpanFormattableOrDefaultComplexContentAsString)
              , NullableStructSpanFormattableOrDefaultComplexContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringNullableStructSpanFormattableOrDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this).AsStringOrDefault
               (NullableStructSpanFormattableOrDefaultSimpleContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable
                | SupportsSettingDefaultValue | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackString)]
public class ComplexContentAsStringNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructSpanFormattableOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this).AsStringOrDefault
               (nameof(NullableStructSpanFormattableOrStringDefaultComplexContentAsString)
              , NullableStructSpanFormattableOrStringDefaultComplexContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallbackString)]
public class SimpleContentAsStringNullableStructSpanFormattableOrStringDefault<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? NullableStructSpanFormattableOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this).AsStringOrDefault
               (NullableStructSpanFormattableOrStringDefaultSimpleContentAsString
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCloakedBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerComplexContentAsString
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsString
               (nameof(CloakedBearerComplexContentAsString)
              , CloakedBearerComplexContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString ?? ""
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleContentAsStringCloakedBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked CloakedBearerSimpleContentAsString
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsString
               (CloakedBearerSimpleContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString ?? ""
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringCloakedBearerOrNull<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrNull
               (nameof(CloakedBearerOrNullComplexContentAsString)
              , CloakedBearerOrNullComplexContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString ?? ""
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleContentAsStringCloakedBearerOrNull<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrNullSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrNull
               (CloakedBearerOrNullSimpleContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer
                | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringCloakedBearerOrStringDefault<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrDefault
               (nameof(CloakedBearerOrStringDefaultComplexContentAsString)
              , CloakedBearerOrStringDefaultComplexContentAsString
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString ?? ""
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringCloakedBearerOrStringDefault<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? CloakedBearerOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CloakedBearerOrStringDefaultSimpleContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrDefault
               (CloakedBearerOrStringDefaultSimpleContentAsString, ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringNullableStructCloakedBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsString
               (nameof(NullableStructCloakedBearerComplexContentAsString)
              , NullableStructCloakedBearerComplexContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleContentAsStringNullableStructCloakedBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsString
               (NullableStructCloakedBearerSimpleContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringNullableStructCloakedBearerOrNull<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrNull
               (nameof(NullableStructCloakedBearerOrNullComplexContentAsString)
              , NullableStructCloakedBearerOrNullComplexContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString ?? ""
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleContentAsStringNullableStructCloakedBearerOrNull<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrNullSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrNull
               (NullableStructCloakedBearerOrNullSimpleContentAsString
              , ValueRevealer
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer
                | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringNullableStructCloakedBearerOrStringDefault<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrStringDefaultComplexContentAsString);
    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrDefault
               (nameof(NullableStructCloakedBearerOrStringDefaultComplexContentAsString)
              , NullableStructCloakedBearerOrStringDefaultComplexContentAsString
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringNullableStructCloakedBearerOrStringDefault<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? NullableStructCloakedBearerOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructCloakedBearerOrStringDefaultSimpleContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrDefault
               (NullableStructCloakedBearerOrStringDefaultSimpleContentAsString
              , ValueRevealer
              , DefaultValue
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer>
    where TBearer : IStringBearer
{
    public TBearer StringBearerComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsString
               (nameof(StringBearerComplexContentAsString)
              , StringBearerComplexContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleContentAsStringStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer> 
    where TBearer : IStringBearer
{
    public TBearer StringBearerSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsString
               (StringBearerSimpleContentAsString
              , tos.CallerContext.FormatString ?? ValueFormatString
              , tos.CallerContext.FormatFlags | FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringStringBearerOrNull<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? StringBearerOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrNull
               (nameof(StringBearerOrNullComplexContentAsString)
              , StringBearerOrNullComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleContentAsStringStringBearerOrNull<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? StringBearerOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrNullSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrNull(StringBearerOrNullSimpleContentAsString, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackString)]
public class ComplexContentAsStringStringBearerOrStringDefault<TBearer> : ProxyFormattedMoldScaffold<TBearer?>, IMoldSupportedDefaultValue<string>
    where TBearer : IStringBearer
{
    public TBearer? StringBearerOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrDefault
               (nameof(StringBearerOrStringDefaultComplexContentAsString)
              , StringBearerOrStringDefaultComplexContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallbackString)]
public class SimpleContentAsStringStringBearerOrStringDefault<TBearer> : ProxyFormattedMoldScaffold<TBearer?>, IMoldSupportedDefaultValue<string>
    where TBearer : IStringBearer
{
    public TBearer? StringBearerOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBearerOrStringDefaultSimpleContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrDefault
               (StringBearerOrStringDefaultSimpleContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringNullableStructStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsString
               (nameof(NullableStructStringBearerComplexContentAsString)
              , NullableStructStringBearerComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleContentAsStringNullableStructStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsString
               (NullableStructStringBearerSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringNullableStructStringBearerOrNull<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrNull
               (nameof(NullableStructStringBearerOrNullComplexContentAsString)
              , NullableStructStringBearerOrNullComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleContentAsStringNullableStructStringBearerOrNull<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?> where
    TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrNullSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrNull
               (NullableStructStringBearerOrNullSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallbackString)]
public class ComplexContentAsStringNullableStructStringBearerOrStringDefault<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
  , IMoldSupportedDefaultValue<string>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .RevealAsStringOrDefault
               (nameof(NullableStructStringBearerOrStringDefaultComplexContentAsString)
              , NullableStructStringBearerOrStringDefaultComplexContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallbackString)]
public class SimpleContentAsStringNullableStructStringBearerOrStringDefault<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
  , IMoldSupportedDefaultValue<string>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? NullableStructStringBearerOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(NullableStructStringBearerOrStringDefaultSimpleContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .RevealAsStringOrDefault
               (NullableStructStringBearerOrStringDefaultSimpleContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCharSpan : FormattedMoldScaffold<char[]> , ISupportsSettingValueFromString
{
    public char[] CharSpanComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanComplexContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(CharSpanComplexContentAsString)
              , CharSpanComplexContentAsString.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringCharSpan : FormattedMoldScaffold<char[]>, ISupportsSettingValueFromString
{
    public char[] CharSpanSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanSimpleContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (CharSpanSimpleContentAsString.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringCharAsSpanOrNull : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharAsSpanOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharAsSpanOrNullComplexContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(CharAsSpanOrNullComplexContentAsString)
              , CharAsSpanOrNullComplexContentAsString.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringCharAsSpanOrNull : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharAsSpanOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharAsSpanOrNullSimpleContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (CharAsSpanOrNullSimpleContentAsString.AsSpan()
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringCharSpanOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] CharSpanOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrStringDefaultComplexContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(CharSpanOrStringDefaultComplexContentAsString)
              , CharSpanOrStringDefaultComplexContentAsString.AsSpan()
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringCharSpanOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] CharSpanOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSpanOrStringDefaultSimpleContentAsString);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (CharSpanOrStringDefaultSimpleContentAsString.AsSpan()
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringReadOnlyCharSpan : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanComplexContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(ReadOnlyCharSpanComplexContentAsString)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringReadOnlyCharSpan : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanSimpleContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               ((ReadOnlySpan<char>)ReadOnlyCharSpanSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringReadOnlyCharSpanOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrNullComplexContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(ReadOnlyCharSpanOrNullComplexContentAsString)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanOrNullComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringReadOnlyCharSpanOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string ReadOnlyCharSpanOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrNullSimpleContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)ReadOnlyCharSpanOrNullSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringReadOnlyCharSpanOrStringDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string ReadOnlyCharSpanOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrStringDefaultComplexContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(ReadOnlyCharSpanOrStringDefaultComplexContentAsString)
              , (ReadOnlySpan<char>)ReadOnlyCharSpanOrStringDefaultComplexContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringReadOnlyCharSpanOrStringDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string ReadOnlyCharSpanOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ReadOnlyCharSpanOrStringDefaultSimpleContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               ((ReadOnlySpan<char>)ReadOnlyCharSpanOrStringDefaultSimpleContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty | SupportsValueFormatString)]
public class ComplexContentAsStringString : FormattedMoldScaffold<string>, ISupportsSettingValueFromString
{
    public string StringComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringComplexContentAsString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(StringComplexContentAsString)
              , StringComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty | SupportsValueFormatString)]
public class SimpleContentAsStringString : FormattedMoldScaffold<string>
{
    public string StringSimpleContentAsString
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
           .AsString
               (StringSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringStringRange : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(StringRangeComplexContentAsString)
              , StringRangeComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringStringRange : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeSimpleContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (StringRangeSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringStringRangeOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrNullComplexContentAsString);

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(StringRangeOrNullComplexContentAsString)
              , StringRangeOrNullComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringStringRangeOrNull : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string StringRangeOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrNullSimpleContentAsString);

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (StringRangeOrNullSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringStringRangeOrDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string StringRangeOrDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrDefaultComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(StringRangeOrDefaultComplexContentAsString)
              , StringRangeOrDefaultComplexContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringStringRangeOrDefault : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string StringRangeOrDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringRangeOrDefaultSimpleContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (StringRangeOrDefaultSimpleContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCharArray : FormattedMoldScaffold<char[]>, ISupportsSettingValueFromString
{
    public char[] CharArrayComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayComplexContentAsString);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(CharArrayComplexContentAsString)
              , CharArrayComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringCharArray : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] CharArraySimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArraySimpleContentAsString);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (CharArraySimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCharArrayRange : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(CharArrayRangeComplexContentAsString)
              , CharArrayRangeComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringCharArrayRange : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeSimpleContentAsString
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
           .AsString
               (CharArrayRangeSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringCharArrayRangeOrNull : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeOrNullComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(CharArrayRangeOrNullComplexContentAsString)
              , CharArrayRangeOrNullComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringCharArrayRangeOrNull : FormattedMoldScaffold<char[]?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? CharArrayRangeOrNullSimpleContentAsString
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
           .AsStringOrNull
               (CharArrayRangeOrNullSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringCharArrayRangeOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharArrayRangeOrStringDefaultComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(CharArrayRangeOrStringDefaultComplexContentAsString)
              , CharArrayRangeOrStringDefaultComplexContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringCharArrayRangeOrStringDefault : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] CharArrayRangeOrStringDefaultSimpleContentAsString
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

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (CharArrayRangeOrStringDefaultSimpleContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCharSequence<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceComplexContentAsString);

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }

            else { Value = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(CharSequenceComplexContentAsString)
              , CharSequenceComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringCharSequence<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceSimpleContentAsString
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }

            else { Value = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (CharSequenceSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringCharSequenceRange<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(CharSequenceRangeComplexContentAsString)
              , CharSequenceRangeComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringCharSequenceRange<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeSimpleContentAsString
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsString
               (CharSequenceRangeSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringCharSequenceRangeOrNull<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? CharSequenceRangeOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeOrNullComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(CharSequenceRangeOrNullComplexContentAsString)
              , CharSequenceRangeOrNullComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringCharSequenceRangeOrNull<TCharSeq> : FormattedMoldScaffold<TCharSeq?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? CharSequenceRangeOrNullSimpleContentAsString
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrNull
               (CharSequenceRangeOrNullSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringCharSequenceRangeOrStringDefault<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(CharSequenceRangeOrStringDefaultComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

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

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(CharSequenceRangeOrStringDefaultComplexContentAsString)
              , CharSequenceRangeOrStringDefaultComplexContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringCharSequenceRangeOrStringDefault<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
  , ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq CharSequenceRangeOrStringDefaultSimpleContentAsString
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }
    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (CharSequenceRangeOrStringDefaultSimpleContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringStringBuilder : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder StringBuilderComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderComplexContentAsString);

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(StringBuilderComplexContentAsString)
              , StringBuilderComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringStringBuilder : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder StringBuilderSimpleContentAsString
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
           .AsString
               (StringBuilderSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class ComplexContentAsStringStringBuilderRange : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder StringBuilderRangeComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsString
               (nameof(StringBuilderRangeComplexContentAsString)
              , StringBuilderRangeComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleContentAsStringStringBuilderRange : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder StringBuilderRangeSimpleContentAsString
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
           .AsString
               (StringBuilderRangeSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringStringBuilderRangeOrNull : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeOrNullComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrNull
               (nameof(StringBuilderRangeOrNullComplexContentAsString)
              , StringBuilderRangeOrNullComplexContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringStringBuilderRangeOrNull : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrNullSimpleContentAsString
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
           .AsStringOrNull
               (StringBuilderRangeOrNullSimpleContentAsString
              , FromIndex
              , Length
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class ComplexContentAsStringStringBuilderRangeOrStringDefault : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(StringBuilderRangeOrStringDefaultComplexContentAsString);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringOrDefault
               (nameof(StringBuilderRangeOrStringDefaultComplexContentAsString)
              , StringBuilderRangeOrStringDefaultComplexContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue)]
public class SimpleContentAsStringStringBuilderRangeOrStringDefault : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? StringBuilderRangeOrStringDefaultSimpleContentAsString
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
    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringOrDefault
               (StringBuilderRangeOrStringDefaultSimpleContentAsString
              , FromIndex
              , Length
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringMatch<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringMatch
               (nameof(MatchComplexContentAsString)
              , MatchComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringMatch<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringMatch
               (MatchSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class ComplexContentAsStringMatchOrNull<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchOrNullComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrNullComplexContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringMatchOrNull
               (nameof(MatchOrNullComplexContentAsString)
              , MatchOrNullComplexContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleContentAsStringMatchOrNull<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? MatchOrNullSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrNullSimpleContentAsString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringMatchOrNull
               (MatchOrNullSimpleContentAsString
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | IsComplexType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackString)]
public class ComplexContentAsStringMatchOrStringDefault<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? MatchOrStringDefaultComplexContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrStringDefaultComplexContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexContentType(this)
           .AsStringMatchOrDefault
               (nameof(MatchOrStringDefaultComplexContentAsString)
              , MatchOrStringDefaultComplexContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsContentType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallbackString)]
public class SimpleContentAsStringMatchOrStringDefault<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? MatchOrStringDefaultSimpleContentAsString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(MatchOrStringDefaultSimpleContentAsString);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleContentType(this)
           .AsStringMatchOrDefault
               (MatchOrStringDefaultSimpleContentAsString
              , DefaultValue
              , ValueFormatString
              , FormattingFlags)
           .Complete();
}
