// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.SimpleTypeScaffolds;

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class SimpleAsStringBoolWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<bool>
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringBool)
              , SimpleTypeAsStringBool)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class SimpleAsStringBoolNoFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<bool>
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringBool).Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class SimpleAsStringNullableBoolWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<bool?>
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableBool)
              , SimpleTypeAsStringNullableBool)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class SimpleAsStringNullableBoolNoFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<bool?>
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableBool).Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringSpanFormattable)
              , SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringSpanFormattableNoFieldSimpleValueTypeStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassWithFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableClass)
              , SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassNoFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> : IStringBearer
  , IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructWithFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableStruct)
              , SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructNoFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}


[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}


[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringNullableCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringNullableCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringNullableCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsStringNullableCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer> where TBearer
    : IStringBearer, IMoldSupportedValue<TBearer?>
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer> where TBearer
    : IStringBearer, IMoldSupportedValue<TBearer?>
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringNullableStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?> where
    TBearer
    : IStringBearer, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringNullableStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?> where
    TBearer
    : IStringBearer, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringNullableStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>
    where TBearer
    : IStringBearer, IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsStringNullableStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>
    where TBearer
    : IStringBearer, IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsStringCharSpanWithDefaultWithFieldAsSpanSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsStringCharSpan
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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharSpanWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharSpan
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharSpan)
              , SimpleTypeAsStringCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharSpanWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharSpan
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharSpan)
              , SimpleTypeAsStringCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars)]
public class SimpleAsStringCharSpanWithNoFieldAsSpanSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsStringCharSpan
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull(SimpleTypeAsStringCharSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsStringCharReadOnlySpanWithDefaultWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrNullSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpan
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpan)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpan
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars)]
public class SimpleAsStringCharSpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsStringCharSpanOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringCharSpanOrDefault.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanOrDefault)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars)]
public class SimpleAsStringStringWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
{
    public string SimpleTypeAsStringStringOrDefaultNoFormatting
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringStringOrDefaultNoFormatting)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringStringWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringStringOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringStringOrDefault)
              , SimpleTypeAsStringStringOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringStringWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringString
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringString)
              , SimpleTypeAsStringString
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringRangeOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringRangeSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRange
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringRangeWithDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeWithDefaultValue
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharArrayWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharArrayOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeNoFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharArrayRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharSequenceWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceOrDefault)
              , SimpleTypeAsStringCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringCharSequenceNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeWithFieldSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeNoFieldSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeWithFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
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

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSequenceRangeWithDefault)
              , SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringCharSequenceRangeNoFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
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
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}










[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringStringBuilderWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString 
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringStringBuilderOrDefault)
              , SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class SimpleAsStringStringBuilderNoFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString 
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder Value { get; set; } = default!;

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeWithFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeNoFieldSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

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
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsStringStringBuilderRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

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
}


[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class SimpleAsStringMatchOrDefaultSimpleValueTypeStringBearer<TAny> : IStringBearer, IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsStringMatchOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatch(nameof(SimpleTypeAsStringMatchOrDefault), SimpleTypeAsStringMatchOrDefault, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}


[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class SimpleAsStringMatchSimpleValueTypeStringBearer<TAny> : IStringBearer, IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsStringMatch
    {
        get => Value;
        set => Value = value;
    }

    public TAny? Value { get; set; }
    
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrNull(nameof(SimpleTypeAsStringMatch), SimpleTypeAsStringMatch, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}


[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class SimpleAsStringMatchWithDefaultSimpleValueTypeStringBearer<TAny> : IStringBearer, IMoldSupportedValue<TAny?>, ISupportsValueFormatString
  , IMoldSupportedDefaultValue<string>
{
    public TAny? SimpleTypeAsStringMatchWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public TAny? Value { get; set; }
    
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrDefault(nameof(SimpleTypeAsStringMatchWithDefault), SimpleTypeAsStringMatchWithDefault, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
