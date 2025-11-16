// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class SimpleAsStringBoolWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool>
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringBool)
              , SimpleTypeAsStringBool, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsStringOut)]
public class SimpleAsStringBoolNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool>
{
    public bool SimpleTypeAsStringBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringBool, ValueFormatString).Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | DefaultTreatedAsStringOut | SupportsValueFormatString
                | DefaultBecomesNull)]
public class SimpleAsStringNullableBoolWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableBool)
              , SimpleTypeAsStringNullableBool, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | DefaultTreatedAsStringOut | SupportsValueFormatString
                | DefaultBecomesNull)]
public class SimpleAsStringNullableBoolNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? SimpleTypeAsStringNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableBool, ValueFormatString).Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt> : FormattedMoldScaffold<TFmt>
   where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringSpanFormattable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringSpanFormattable)
              , SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringSpanFormattableNoFieldSimpleValueTypeStringBearer<TFmt> : FormattedMoldScaffold<TFmt>
   where TFmt : ISpanFormattable
{
    public TFmt SimpleTypeAsStringSpanFormattable
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString(SimpleTypeAsStringSpanFormattable, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableClassWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClass);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableClass)
              , SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableClassNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableClass, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault);

    public TFmtClass DefaultValue { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmtClass DefaultValue { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<string> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableClassWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableClassWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<string> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsStringNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableClassWithDefault,
                DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableStructWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStruct);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringNullableSpanFormattableStruct)
              , SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringNullableSpanFormattableStructNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull(SimpleTypeAsStringNullableSpanFormattableStruct, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault);

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableSpanFormattableStructWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsStringNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringNullableSpanFormattableStructWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsStringCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringNullableCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringNullableCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);
    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault
               (nameof(SimpleTypeAsStringCloakedBearer)
              , SimpleTypeAsStringCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringNullableCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsStringCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCloakedBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer>
    where TBearer : IStringBearer
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer> where TBearer
    : FormattedMoldScaffold<TBearer?>
{
    public TBearer SimpleTypeAsStringStringBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer?> where
    TBearer
    : FormattedMoldScaffold<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer?> where
    TBearer : FormattedMoldScaffold<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallback)]
public class SimpleAsStringStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer?>
    where TBearer : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallback)]
public class SimpleAsStringStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearer> : MoldScaffoldBase<TBearer?>
    where TBearer
    : IMoldSupportedDefaultValue<string>, IMoldSupportedValue<TBearer?>
{
    public TBearer? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableStringBearerWithFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesEmpty)]
public class SimpleAsStringNullableStringBearerNoFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsString(SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringNullableStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesNull)]
public class SimpleAsStringNullableStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?> where
    TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrNull(SimpleTypeAsStringStringBearer)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallback)]
public class SimpleAsStringNullableStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(nameof(SimpleTypeAsStringStringBearer), SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsStringOut
                | DefaultBecomesFallback)]
public class SimpleAsStringNullableStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearerStruct> : MoldScaffoldBase<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsStringStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsStringOrDefault(SimpleTypeAsStringStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSpanWithDefaultWithFieldAsSpanSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
   , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSpan);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsStringCharSpan.AsSpan()
              , DefaultValue
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSpanWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSpan);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsStringCharSpan.AsSpan(), DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSpanWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSpan);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharSpan)
              , SimpleTypeAsStringCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSpanWithNoFieldAsSpanSimpleValueTypeStringBearer : MoldScaffoldBase<char[]>
{
    public char[] SimpleTypeAsStringCharSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull(SimpleTypeAsStringCharSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharReadOnlySpanWithDefaultWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanWithDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanWithDefault
              , DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharReadOnlySpanWithFieldOrNullSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringCharReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharReadOnlySpan);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringCharReadOnlySpan)
              , (ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsStringCharReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsChars | AcceptsCharArray
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : MoldScaffoldBase<char[]>
{
    public char[] SimpleTypeAsStringCharSpanOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringCharSpanOrDefault.AsSpan())
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharReadOnlySpanWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsStringCharReadOnlySpanOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString((ReadOnlySpan<char>)SimpleTypeAsStringCharReadOnlySpanOrDefault)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsStringStringOrDefaultNoFormatting
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString(SimpleTypeAsStringStringOrDefaultNoFormatting)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringStringOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringOrDefault);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsStringStringOrDefault)
              , SimpleTypeAsStringStringOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsChars | AcceptsString
                | SupportsValueFormatString | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsStringString
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringString)
              , SimpleTypeAsStringString
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringRangeOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringStringRangeOrDefault)
              , SimpleTypeAsStringStringRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsString | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringRangeSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRange
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringStringRange)
              , SimpleTypeAsStringStringRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringRangeWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsStringStringRangeWithDefaultValue
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringRangeWithDefaultValue);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringStringRangeWithDefaultValue)
              , SimpleTypeAsStringStringRangeWithDefaultValue
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsStringCharArrayOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharArrayOrDefault);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringCharArrayOrDefault)
              , SimpleTypeAsStringCharArrayOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayRangeWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharArrayRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsString
               (nameof(SimpleTypeAsStringCharArrayRangeOrDefault)
              , SimpleTypeAsStringCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharArrayRangeNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRangeOrDefault
    {
        get => Value!;
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
        tos.StartSimpleValueType(this).AsString
               (SimpleTypeAsStringCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharArrayRangeWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrNull
               (nameof(SimpleTypeAsStringCharArrayRange)
              , SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharArrayRangeNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value!;
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
        tos.StartSimpleValueType(this).AsStringOrNull
               (SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharArrayRangeWithFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsStringOrDefault
               (nameof(SimpleTypeAsStringCharArrayRange)
              , SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharArray | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharArrayRangeNoFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsStringCharArrayRange
    {
        get => Value!;
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
        tos.StartSimpleValueType(this).AsStringOrDefault
               (SimpleTypeAsStringCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSequenceOrDefault);

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
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceOrDefault)
              , SimpleTypeAsStringCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceOrDefault
    {
        get => Value!;
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
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringCharSequenceOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceRangeWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeOrDefault);
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
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringCharSequenceRangeNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value!;
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
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSequenceRangeWithFieldSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeOrDefault);
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
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringCharSequenceRangeOrDefault)
              , SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringCharSequenceRangeNoFieldSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeOrDefault
    {
        get => Value!;
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
            {
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            }
            else
            {
                Value = (TCharSeq)(object)new MutableString(value);
            }
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (SimpleTypeAsStringCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSequenceRangeWithFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringCharSequenceRangeWithDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
            {
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            }
            else
            {
                Value = (TCharSeq)(object)new MutableString(value);
            }
        }
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringCharSequenceRangeWithDefault)
              , SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsCharSequence | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringCharSequenceRangeNoFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
  , ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsStringCharSequenceRangeWithDefault
    {
        get => Value!;
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
            {
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            }
            else
            {
                Value = (TCharSeq)(object)new MutableString(value);
            }
        }
    }
    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (SimpleTypeAsStringCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBuilderOrDefault);

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringStringBuilderOrDefault)
              , SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsStringStringBuilderOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderOrDefault
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderRangeWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (nameof(SimpleTypeAsStringStringBuilderRangeOrDefault)
              , SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesEmpty)]
public class SimpleAsStringStringBuilderRangeNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value!.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsString
               (SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBuilderRangeWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (nameof(SimpleTypeAsStringStringBuilderRangeOrDefault)
              , SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringStringBuilderRangeNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeOrDefault
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
        tos.StartSimpleValueType(this)
           .AsStringOrNull
               (SimpleTypeAsStringStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBuilderRangeWithFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringStringBuilderRangeWithDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (nameof(SimpleTypeAsStringStringBuilderRangeWithDefault)
              , SimpleTypeAsStringStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsChars | AcceptsStringBuilder | SupportsValueFormatString
                | SupportsIndexSubRanges | SupportsSettingDefaultValue | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringStringBuilderRangeNoFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsStringStringBuilderRangeWithDefault
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
        tos.StartSimpleValueType(this)
           .AsStringOrDefault
               (SimpleTypeAsStringStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringMatchOrDefaultSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? SimpleTypeAsStringMatchOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringMatchOrDefault);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatch(nameof(SimpleTypeAsStringMatchOrDefault), SimpleTypeAsStringMatchOrDefault, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull)]
public class SimpleAsStringMatchSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? SimpleTypeAsStringMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringMatch);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrNull(nameof(SimpleTypeAsStringMatch), SimpleTypeAsStringMatch, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString | SupportsSettingDefaultValue
                | DefaultTreatedAsStringOut | DefaultBecomesFallback)]
public class SimpleAsStringMatchWithDefaultSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? SimpleTypeAsStringMatchWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsStringMatchWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsStringMatchOrDefault(nameof(SimpleTypeAsStringMatchWithDefault), SimpleTypeAsStringMatchWithDefault, DefaultValue, ValueFormatString)
           .Complete();

}
