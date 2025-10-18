// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.SimpleTypeScaffolds;

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class SimpleAsValueBoolWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool>
{
    public bool SimpleTypeAsValueBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueBool);
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueBool)
              , SimpleTypeAsValueBool)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class SimpleAsValueBoolNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool>
{
    public bool SimpleTypeAsValueBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueBool);
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue(SimpleTypeAsValueBool).Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class SimpleAsValueNullableBoolWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool?>
{
    public bool? SimpleTypeAsValueNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableBool)
              , SimpleTypeAsValueNullableBool)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class SimpleAsValueNullableBoolNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<bool?>
{
    public bool? SimpleTypeAsValueNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableBool).Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt> : IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsValueSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueSpanFormattable);
    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueSpanFormattable)
              , SimpleTypeAsValueSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueSpanFormattableNoFieldSimpleValueTypeStringBearer<TFmt> : IMoldSupportedValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsValueSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueSpanFormattable);
    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue(SimpleTypeAsValueSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassWithFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClass);
    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableClass)
              , SimpleTypeAsValueNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassNoFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClass);
    public TFmtClass? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableSpanFormattableClass, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault);
    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault);
    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault);
    public TFmtClass? Value { get; set; }

    public TFmtClass DefaultValue { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableClassWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> : 
  IMoldSupportedValue<TFmtClass?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithStringDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithStringDefault);
    public TFmtClass? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableClassWithStringDefault,
                DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructWithFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStruct);
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableStruct)
              , SimpleTypeAsValueNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructNoFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStruct);
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);
    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);
    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);
    public TFmtStruct? Value { get; set; }

    public TFmtStruct DefaultValue { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SimpleAsValueNullableSpanFormattableStructWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> : 
  IMoldSupportedValue<TFmtStruct?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithStringDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithStringDefault);
    public TFmtStruct? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableStructWithStringDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
    public TCloaked Value { get; set; } = default!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(SimpleTypeAsValueCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueNullableCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueNullableCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(SimpleTypeAsValueCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueNullableCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
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
           .RevealAsValueOrDefault
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class SimpleAsValueNullableCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?>, IMoldSupportedDefaultValue<string>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);
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
           .RevealAsValueOrDefault(SimpleTypeAsValueCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer> where TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer> where TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueNullableStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?> where
    TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueNullableStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?> where
    TBearer
    : IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueNullableStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
    where TBearer
    : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class SimpleAsValueNullableStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
    where TBearer
    : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBearer);
    public TBearer? Value { get; set; }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsValueCharSpanWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueCharSpan)
              , SimpleTypeAsValueCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsValueCharSpanWithDefaultWithFieldAsSpanSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueCharSpan)
              , SimpleTypeAsValueCharSpan.AsSpan()
              , DefaultValue
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString)]
public class SimpleAsValueCharSpanWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsValueCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray)]
public class SimpleAsValueCharSpanWithNoFieldAsSpanSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSpan);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrZero(SimpleTypeAsValueCharSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsValueCharReadOnlySpanWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueCharReadOnlySpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpanOrDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpanOrDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue)]
public class SimpleAsValueCharReadOnlySpanWithDefaultWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string SimpleTypeAsValueCharReadOnlySpanWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpanWithDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class SimpleAsValueCharReadOnlySpanWithFieldOrNullSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpan);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpan)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString)]
public class SimpleAsValueCharReadOnlySpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsValueCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpan);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrZero((ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray)]
public class SimpleAsValueCharSpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
{
    public char[] SimpleTypeAsValueCharSpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSpanOrDefault);
    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue(SimpleTypeAsValueCharSpanOrDefault.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString)]
public class SimpleAsValueCharReadOnlySpanWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsValueCharReadOnlySpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpanOrDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue((ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanOrDefault)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString)]
public class SimpleAsValueStringWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>
{
    public string SimpleTypeAsValueStringOrDefaultNoFormatting
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringOrDefaultNoFormatting);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue(SimpleTypeAsValueStringOrDefaultNoFormatting)
           .Complete();
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class SimpleAsValueStringWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueStringOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringOrDefault);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueStringOrDefault)
              , SimpleTypeAsValueStringOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class SimpleAsValueStringWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueString);
    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueString)
              , SimpleTypeAsValueString
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringRangeOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string Value { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueStringRangeOrDefault)
              , SimpleTypeAsValueStringRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringRangeSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string Value { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueStringRange)
              , SimpleTypeAsValueStringRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringRangeWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRangeWithDefaultValue
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringRangeWithDefaultValue);
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
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueStringRangeWithDefaultValue)
              , SimpleTypeAsValueStringRangeWithDefaultValue
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString)]
public class SimpleAsValueCharArrayWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharArrayOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayOrDefault);
    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueCharArrayOrDefault)
              , SimpleTypeAsValueCharArrayOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueCharArrayRangeOrDefault)
              , SimpleTypeAsValueCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (SimpleTypeAsValueCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueCharArrayRange)
              , SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[] Value { get; set; } = [];

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);
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
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueCharArrayRange)
              , SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharArrayRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<char[]>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);
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
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class SimpleAsValueCharSequenceWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceOrDefault);
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
           .AsValue
               (nameof(SimpleTypeAsValueCharSequenceOrDefault)
              , SimpleTypeAsValueCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class SimpleAsValueCharSequenceNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceOrDefault);
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
           .AsValue
               (SimpleTypeAsValueCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
           .AsValue
               (nameof(SimpleTypeAsValueCharSequenceRangeOrDefault)
              , SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
           .AsValue
               (SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeWithFieldSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
           .AsValueOrNull
               (nameof(SimpleTypeAsValueCharSequenceRangeOrDefault)
              , SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeNoFieldSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
           .AsValueOrNull
               (SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeWithFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeWithDefault);
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
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueCharSequenceRangeWithDefault)
              , SimpleTypeAsValueCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueCharSequenceRangeNoFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeWithDefault);
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
           .AsValueOrDefault
               (SimpleTypeAsValueCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class SimpleAsValueStringBuilderWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString 
{
    public StringBuilder SimpleTypeAsValueStringBuilderOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderOrDefault);
    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueStringBuilderOrDefault)
              , SimpleTypeAsValueStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class SimpleAsValueStringBuilderNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString 
{
    public StringBuilder SimpleTypeAsValueStringBuilderOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderOrDefault);
    public StringBuilder Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (SimpleTypeAsValueStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeWithFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
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
           .AsValue
               (nameof(SimpleTypeAsValueStringBuilderRangeOrDefault)
              , SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeNoFieldOrDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
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
           .AsValue
               (SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeWithFieldSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
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
           .AsValueOrNull
               (nameof(SimpleTypeAsValueStringBuilderRangeOrDefault)
              , SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeNoFieldSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
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
           .AsValueOrNull
               (SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeWithFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeWithDefault);
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
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueStringBuilderRangeWithDefault)
              , SimpleTypeAsValueStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class SimpleAsValueStringBuilderRangeNoFieldWithDefaultSimpleValueTypeStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting 
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeWithDefault);
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
           .AsValueOrDefault
               (SimpleTypeAsValueStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class SimpleAsValueMatchOrDefaultSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsValueMatchOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueMatchOrDefault);
    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatch(nameof(SimpleTypeAsValueMatchOrDefault), SimpleTypeAsValueMatchOrDefault, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class SimpleAsValueMatchSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? SimpleTypeAsValueMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueMatch);
    public TAny? Value { get; set; }
    
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatchOrNull(nameof(SimpleTypeAsValueMatch), SimpleTypeAsValueMatch, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(SimpleType | AcceptsSingleValue | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class SimpleAsValueMatchWithDefaultSimpleValueTypeStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
  , IMoldSupportedDefaultValue<string>
{
    public TAny? SimpleTypeAsValueMatchWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueMatchWithDefault);
    public TAny? Value { get; set; }
    
    public string DefaultValue { get; set; } = "";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatchOrDefault(nameof(SimpleTypeAsValueMatchWithDefault), SimpleTypeAsValueMatchWithDefault, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
