// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class SimpleAsStringBoolWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool>, ISupportsValueFormatString
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringBool);
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringBool)
              , SimpleTypeAsStringBool, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class SimpleAsStringBoolNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool>, ISupportsValueFormatString
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringBool, ValueFormatString).Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | DefaultTreatedAsStringOut | SupportsValueFormatString
                | DefaultBecomesNull)]
public class SimpleAsStringNullableBoolWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool?>, ISupportsValueFormatString
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableBool)
              , SimpleTypeAsStringNullableBool, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | DefaultTreatedAsStringOut | SupportsValueFormatString
                | DefaultBecomesNull)]
public class SimpleAsStringNullableBoolNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool?>, ISupportsValueFormatString
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableBool, ValueFormatString).Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt> : IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringSpanFormattable);
    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringSpanFormattable)
              , SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringSpanFormattableNoFieldSimpleValueTypeStringBearer<TFmt> : IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableClassWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClass);
    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableClass)
              , SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableClassNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault);
    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault);
    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableStructWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStruct);
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableStruct)
              , SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableStructNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault);
    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault);
    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsSettingDefaultValue | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> :
    IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloaked? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | DefaultTreatedAsStringOut 
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | DefaultTreatedAsStringOut 
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | DefaultTreatedAsStringOut 
                | DefaultBecomesNull)]
public class SimpleAsStringNullableCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | DefaultTreatedAsStringOut 
                | DefaultBecomesNull)]
public class SimpleAsStringNullableCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | SupportsSettingDefaultValue 
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedStruct> 
    where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber 
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer | SupportsSettingDefaultValue 
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    IMoldSupportedValue<TCloakedStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedStruct> 
    where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public TCloakedStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer>
    where TBearer : IStringBearer
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer> where TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString( SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?> where
    TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?> where
    TBearer : IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull( SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
    where TBearer
    : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsStringBearer   | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
    where TBearer
    : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringNullableStringBearerWithFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringNullableStringBearerNoFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct> 
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString( SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?> 
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?> where
    TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull( SimpleTypeAsStringStringBearer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut 
                | DefaultBecomesFallback)]
public class SimpleAsStringNullableStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut 
                | DefaultBecomesFallback)]
public class SimpleAsStringNullableStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBearer);
    public TBearerStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsSettingDefaultValue  | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSpanWithDefaultWithFieldAsSpanSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsStringCharSpan.AsSpan()
              , DefaultValue
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan  | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut
                  | DefaultBecomesFallback)]
public class SimpleAsStringCharSpanWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsStringCharSpan.AsSpan(), DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan  | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSpanWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharSpan)
              , SimpleTypeAsStringCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan  | AcceptsChars | AcceptsCharArray
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSpanWithNoFieldAsSpanSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull(SimpleTypeAsStringCharSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut
                  | DefaultBecomesFallback)]
public class SimpleAsStringCharReadOnlySpanWithDefaultWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrNullSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpan);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpan)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan  | AcceptsChars | AcceptsCharArray
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsStringCharSpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringCharSpanOrDefault.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut |  DefaultBecomesEmpty)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanOrDefault)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut |  DefaultBecomesEmpty)]
public class SimpleAsStringStringWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringStringOrDefaultNoFormatting
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringStringOrDefaultNoFormatting)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringStringOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringOrDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringStringOrDefault)
              , SimpleTypeAsStringStringOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan  | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringString);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringString)
              , SimpleTypeAsStringString
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringRangeOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string Value { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringStringRangeOrDefault)
              , SimpleTypeAsStringStringRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringRangeSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string Value { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringStringRange)
              , SimpleTypeAsStringStringRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringRangeWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeWithDefaultValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringRangeWithDefaultValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string Value { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringStringRangeWithDefaultValue)
              , SimpleTypeAsStringStringRangeWithDefaultValue
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharArrayOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharArrayOrDefault);
    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringCharArrayOrDefault)
              , SimpleTypeAsStringCharArrayOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharArrayRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringCharArrayRangeOrDefault)
              , SimpleTypeAsStringCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut |  DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (SimpleTypeAsStringCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharArrayRangeWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringCharArrayRange)
              , SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharArrayRangeNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharArrayRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringCharArrayRange)
              , SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharArrayRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSequenceOrDefault);
    public TCharSeq Value { get; set; } = default!;

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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceOrDefault)
              , SimpleTypeAsStringCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public TCharSeq Value { get; set; } = default!;

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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceRangeWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeOrDefault);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceRangeNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSequenceRangeWithFieldSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeOrDefault);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSequenceRangeNoFieldSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSequenceRangeWithFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeWithDefault);
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

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSequenceRangeWithDefault)
              , SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString 
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSequenceRangeNoFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
  , ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
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
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBuilderOrDefault);
    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringStringBuilderOrDefault)
              , SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringStringBuilderRangeOrDefault)
              , SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBuilderRangeWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringStringBuilderRangeOrDefault)
              , SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBuilderRangeNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBuilderRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeWithDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringStringBuilderRangeWithDefault)
              , SimpleTypeAsStringStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString 
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBuilderRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (SimpleTypeAsStringStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringMatchOrDefaultSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsStringMatchOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringMatchOrDefault);
    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatch(nameof(SimpleTypeAsStringMatchOrDefault), SimpleTypeAsStringMatchOrDefault, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringMatchSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsStringMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringMatch);
    public TAny? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrNull(nameof(SimpleTypeAsStringMatch), SimpleTypeAsStringMatch, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality  | AcceptsAnyGeneric | SupportsValueFormatString | SupportsSettingDefaultValue 
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringMatchWithDefaultSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
  , IMoldSupportedDefaultValue<string>
{
    public TAny? SimpleTypeAsStringMatchWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsStringMatchWithDefault);
    public TAny? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrDefault(nameof(SimpleTypeAsStringMatchWithDefault), SimpleTypeAsStringMatchWithDefault, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
