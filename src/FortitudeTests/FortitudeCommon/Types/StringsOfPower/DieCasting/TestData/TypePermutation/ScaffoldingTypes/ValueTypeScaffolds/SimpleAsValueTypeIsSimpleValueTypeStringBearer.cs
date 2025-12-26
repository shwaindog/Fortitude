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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsValueOut)]
public class SimpleAsValueBoolWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool>
{
    public bool SimpleTypeAsValueBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueBool)
              , SimpleTypeAsValueBool, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStruct | SupportsValueFormatString | DefaultTreatedAsValueOut)]
public class SimpleAsValueBoolNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool>
{
    public bool SimpleTypeAsValueBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue(SimpleTypeAsValueBool, ValueFormatString).Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | SupportsValueFormatString | DefaultTreatedAsValueOut)]
public class SimpleAsValueNullableBoolWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? SimpleTypeAsValueNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableBool);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableBool)
              , SimpleTypeAsValueNullableBool, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableBoolNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<bool?>
{
    public bool? SimpleTypeAsValueNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableBool, ValueFormatString, FormattingFlags).Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt> : FormattedMoldScaffold<TFmt>
, IEquatable<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt>>
   where TFmt : ISpanFormattable?
{
    public SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer() { }

    public SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer(TFmt value) => Value = value;
    public TFmt SimpleTypeAsValueSpanFormattable
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueSpanFormattable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueSpanFormattable)
              , SimpleTypeAsValueSpanFormattable, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
    
    public bool Equals(SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt>? other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other!.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public struct SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<TFmt> : IStringBearer,
    IEquatable<SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<TFmt>>
    where TFmt : ISpanFormattable
{
    public SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer() { }

    public SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer(TFmt value) => Value = value;
    public TFmt? SimpleTypeAsValueSpanFormattableStruct
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueSpanFormattableStruct);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueSpanFormattableStruct)
              , SimpleTypeAsValueSpanFormattableStruct, tos.CallerContext.FormatString ?? ValueFormatString, ContentHandlingFlags)
           .Complete();
    
    public FormatFlags ContentHandlingFlags { get; set; }
    
    public TFmt? Value { get; set; } = default!;

    public PalantírReveal<TFmt> CreateRevealer
    {
        get
        {
            var formatString    = ValueFormatString;
            var contentHandling = ContentHandlingFlags;
            return (cloaked, tos) =>
                tos.StartComplexType(cloaked)
                   .Field.AlwaysAdd
                       ($"CloakedRevealer{nameof(SimpleTypeAsValueSpanFormattableStruct)}"
                      , cloaked, formatString, contentHandling)
                   .Complete();
        }
    }
    
    public Delegate CreateRevealerDelegate => CreateRevealer;
    
    public string? ValueFormatString { get; set; }


    public bool Equals(SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<TFmt> other) =>
        EqualityComparer<TFmt>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<TFmt>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmt>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueSpanFormattableNoFieldSimpleValueTypeStringBearer<TFmt> : FormattedMoldScaffold<TFmt>
   where TFmt : ISpanFormattable?
{
    public TFmt SimpleTypeAsValueSpanFormattable
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue(SimpleTypeAsValueSpanFormattable, tos.CallerContext.FormatString ?? ValueFormatString
                                             , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableClassWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClass);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableClass)
              , SimpleTypeAsValueNullableSpanFormattableClass, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct SimpleAsValueNullableSpanFormattableClassWithFieldSimpleValueTypeStructStringBearer<TFmtClass> : IStringBearer,
    IEquatable<SimpleAsValueNullableSpanFormattableClassWithFieldSimpleValueTypeStructStringBearer<TFmtClass>>  
    where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClass);

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableClass)
              , SimpleTypeAsValueNullableSpanFormattableClass, tos.CallerContext.FormatString ?? ValueFormatString, ContentHandlingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }

    public FormatFlags ContentHandlingFlags { get; set; }

    public TFmtClass? Value { get; set; }
    
    public bool Equals(SimpleAsValueNullableSpanFormattableClassWithFieldSimpleValueTypeStructStringBearer<TFmtClass> other) => 
        Equals(Value, other.Value);
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStructStringBearer<TFmtClass>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmtClass>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableClassNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClass
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableSpanFormattableClass
                                                   , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableClassWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault);

    public TFmtClass DefaultValue { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableClassWithDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<string> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableClassWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableClassWithDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableClassWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<TFmtClass> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmtClass DefaultValue { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableClassWithDefault,
                DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableSpanFormattableClassWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtClass> :
    FormattedMoldScaffold<TFmtClass?>, IMoldSupportedDefaultValue<string> where TFmtClass : class, ISpanFormattable
{
    public TFmtClass? SimpleTypeAsValueNullableSpanFormattableClassWithStringDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableClassWithStringDefault,
                DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStruct);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableStruct)
              , SimpleTypeAsValueNullableSpanFormattableStruct, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStruct
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull(SimpleTypeAsValueNullableSpanFormattableStruct, 
                                                     tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructWithDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public struct SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer<TFmtStruct> : IStringBearer,
    IEquatable<SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer<TFmtStruct>>,  
    IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer() { }

    public SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer(TFmtStruct? value)
    {
        Value = value;
    }
    
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);

    public string DefaultValue { get; set; } = "";

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableStructWithDefault, tos.CallerContext.FormatString ?? ValueFormatString, ContentHandlingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }

    public FormatFlags ContentHandlingFlags { get; set; }

    public TFmtStruct? Value { get; set; }
    
    public bool Equals(SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer<TFmtStruct> other) => 
        Equals(Value, other.Value);
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStructStringBearer<TFmtStruct>)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Value != null ? EqualityComparer<TFmtStruct?>.Default.GetHashCode(Value) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructWithStringDefaultWithFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueNullableSpanFormattableStructWithDefault)
              , SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructWithDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<TFmtStruct> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public TFmtStruct DefaultValue { get; set; } = default!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableStructWithDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsOnlyNullableStructSpanFormattable | SupportsSettingDefaultValue
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesNull)]
public class SimpleAsValueNullableSpanFormattableStructWithStringDefaultNoFieldSimpleValueTypeStringBearer<TFmtStruct> :
    FormattedMoldScaffold<TFmtStruct?>, IMoldSupportedDefaultValue<string> where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? SimpleTypeAsValueNullableSpanFormattableStructWithStringDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueNullableSpanFormattableStructWithStringDefault
              , DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(SimpleTypeAsValueCloakedBearer, ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString
                        , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked, TRevealBase> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked SimpleTypeAsValueCloakedBearer
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(SimpleTypeAsValueCloakedBearer, ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString
                              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloaked, TRevealBase> :
    ValueRevealerMoldScaffold<TCloaked?, TRevealBase>, IMoldSupportedDefaultValue<string> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(SimpleTypeAsValueCloakedBearer, ValueRevealer, DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString
                                 , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableCloakedBearerNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(SimpleTypeAsValueCloakedBearer, ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableCloakedBearerWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableCloakedBearerOrNullWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableCloakedBearerOrNullNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(SimpleTypeAsValueCloakedBearer, ValueRevealer, tos.CallerContext.FormatString ?? ValueFormatString
                              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableCloakedBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string>
    where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCloakedBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault
               (nameof(SimpleTypeAsValueCloakedBearer)
              , SimpleTypeAsValueCloakedBearer
              , ValueRevealer, DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableCloakedBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TCloakedStruct> :
    ValueRevealerMoldScaffold<TCloakedStruct?, TCloakedStruct>, IMoldSupportedDefaultValue<string> where TCloakedStruct : struct
{
    public TCloakedStruct? SimpleTypeAsValueCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(SimpleTypeAsValueCloakedBearer, ValueRevealer, DefaultValue, tos.CallerContext.FormatString ?? ValueFormatString
                                 , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueStringBearerWithFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer>
    where TBearer : IStringBearer
{
    public TBearer SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, tos.CallerContext.FormatString ?? ValueFormatString
                        , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueStringBearerNoFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer>
    where TBearer : IStringBearer
{
    public TBearer SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, tos.CallerContext.FormatString ?? ValueFormatString
                        , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                              , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?> where
    TBearer : IStringBearer
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                              , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
  , IMoldSupportedDefaultValue<string> where TBearer : IStringBearer
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue
                                 , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearer> : ProxyFormattedMoldScaffold<TBearer?>
  , IMoldSupportedDefaultValue<string> where TBearer : IStringBearer
{
    public TBearer? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue
                                 , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableStringBearerWithFieldSimpleValueTypeStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                        , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableStringBearerNoFieldSimpleValueTypeStringBearer<TBearerStruct> : ProxyFormattedMoldScaffold<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValue(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                        , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableStringBearerOrNullWithFieldSimpleValueTypeStringBearer<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                              , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | DefaultTreatedAsValueOut
                | DefaultBecomesNull)]
public class SimpleAsValueNullableStringBearerOrNullNoFieldSimpleValueTypeStringBearer<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrNull(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer
                              , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableStringBearerWithDefaultWithFieldSimpleValueTypeStringBearer<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?>, IMoldSupportedDefaultValue<string> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue
                                 , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsNullableStruct | AcceptsStringBearer | SupportsSettingDefaultValue
                | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueNullableStringBearerWithDefaultNoFieldSimpleValueTypeStringBearer<TBearerStruct>
    : ProxyFormattedMoldScaffold<TBearerStruct?>, IMoldSupportedDefaultValue<string> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? SimpleTypeAsValueStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBearer);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .RevealAsValueOrDefault(nameof(SimpleTypeAsValueStringBearer), SimpleTypeAsValueStringBearer, DefaultValue
                                 , tos.CallerContext.FormatString ?? ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharSpanWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSpan);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueCharSpan)
              , SimpleTypeAsValueCharSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharSpanWithDefaultWithFieldAsSpanSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSpan);

    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueCharSpan)
              , SimpleTypeAsValueCharSpan.AsSpan()
              , DefaultValue
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharSpanWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSpan);

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueCharSpan).ToCharArray().AsSpan()
              , SimpleTypeAsValueCharSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueCharSpanWithNoFieldAsSpanSimpleValueTypeStringBearer : MoldScaffoldBase<char[]>
{
    public char[] SimpleTypeAsValueCharSpan
    {
        get => Value;
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
           .AsValueOrZero(SimpleTypeAsValueCharSpan.AsSpan(), FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharReadOnlySpanWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueCharReadOnlySpanOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpanOrDefault);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpanOrDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString |
                  SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharReadOnlySpanWithDefaultWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>
{
    public string SimpleTypeAsValueCharReadOnlySpanWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpanWithDefault);

    public string DefaultValue { get; set; } = "";

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpanWithDefault)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanWithDefault
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharReadOnlySpanWithFieldOrNullSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharReadOnlySpan);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueCharReadOnlySpan)
              , (ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueCharReadOnlySpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsValueCharReadOnlySpan
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
        tos.StartSimpleValueType(this)
           .AsValueOrZero((ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpan, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsArray | CallsAsSpan | AcceptsCharArray
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharSpanWithNoFieldOrDefaultSimpleValueTypeStringBearer : MoldScaffoldBase<char[]>
{
    public char[] SimpleTypeAsValueCharSpanOrDefault
    {
        get => Value;
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
           .AsValue(SimpleTypeAsValueCharSpanOrDefault.AsSpan(), FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueCharReadOnlySpanWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsValueCharReadOnlySpanOrDefault
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
        tos.StartSimpleValueType(this)
           .AsValue((ReadOnlySpan<char>)SimpleTypeAsValueCharReadOnlySpanOrDefault, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueStringWithNoFieldOrDefaultFieldSimpleValueTypeStringBearer : MoldScaffoldBase<string>
{
    public string SimpleTypeAsValueStringOrDefaultNoFormatting
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
        tos.StartSimpleValueType(this)
           .AsValue(SimpleTypeAsValueStringOrDefaultNoFormatting, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueStringOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringOrDefault);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               ((ReadOnlySpan<char>)nameof(SimpleTypeAsValueStringOrDefault)
              , SimpleTypeAsValueStringOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueStringWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueString
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueString)
              , SimpleTypeAsValueString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | CallsAsReadOnlySpan | AcceptsString | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public struct SimpleAsValueStringWithFieldSimpleValueTypeStructStringBearer : IStringBearer, ISupportsSettingValueFromString
{
    public string SimpleTypeAsValueString
    {
        get => Value!;
        set => Value = value;
    }

    public string PropertyName => nameof(SimpleTypeAsValueString);

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueString)
              , SimpleTypeAsValueString
              , ValueFormatString, ContentHandlingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }

    public FormatFlags ContentHandlingFlags { get; set; }

    public string? Value { get; set; }

    public override string ToString() => $"{GetType().CachedCSharpNameNoConstraints()}({Value})";
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringRangeOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueStringRangeOrDefault)
              , SimpleTypeAsValueStringRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueStringRangeSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRange
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueStringRange)
              , SimpleTypeAsValueStringRange
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueStringRangeWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<string>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public string SimpleTypeAsValueStringRangeWithDefaultValue
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringRangeWithDefaultValue);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueStringRangeWithDefaultValue)
              , SimpleTypeAsValueStringRangeWithDefaultValue
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueCharArrayWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString
{
    public char[] SimpleTypeAsValueCharArrayOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharArrayOrDefault);

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueCharArrayOrDefault)
              , SimpleTypeAsValueCharArrayOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharArrayRangeWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharArrayRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValue
               (nameof(SimpleTypeAsValueCharArrayRangeOrDefault)
              , SimpleTypeAsValueCharArrayRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharArrayRangeNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRangeOrDefault
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
        tos.StartSimpleValueType(this).AsValue(SimpleTypeAsValueCharArrayRangeOrDefault
                                             , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharArrayRangeWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrNull
               (nameof(SimpleTypeAsValueCharArrayRange)
              , SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharArrayRangeNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
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
        tos.StartSimpleValueType(this).AsValueOrNull
               (SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharArrayRangeWithFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharArrayRange);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (nameof(SimpleTypeAsValueCharArrayRange)
              , SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharArrayRangeNoFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<char[]>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public char[] SimpleTypeAsValueCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray() ?? [];
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this).AsValueOrDefault
               (SimpleTypeAsValueCharArrayRange
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharSequenceWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSequenceOrDefault);

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
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueCharSequenceOrDefault)
              , SimpleTypeAsValueCharSequenceOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | DefaultTreatedAsValueOut
                | DefaultBecomesZero)]
public class SimpleAsValueCharSequenceNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString
    where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceOrDefault
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);

            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (SimpleTypeAsValueCharSequenceOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharSequenceRangeWithFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueCharSequenceRangeOrDefault)
              , SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueCharSequenceRangeNoFieldOrDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);

            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharSequenceRangeWithFieldSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeOrDefault);
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
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueCharSequenceRangeOrDefault)
              , SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueCharSequenceRangeNoFieldSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeOrDefault
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

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (SimpleTypeAsValueCharSequenceRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharSequenceRangeWithFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueCharSequenceRangeWithDefault);

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
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueCharSequenceRangeWithDefault)
              , SimpleTypeAsValueCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueCharSequenceRangeNoFieldWithDefaultSimpleValueTypeStringBearer<TCharSeq> : FormattedMoldScaffold<TCharSeq>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq SimpleTypeAsValueCharSequenceRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
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
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (SimpleTypeAsValueCharSequenceRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringBuilderWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsValueStringBuilderOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBuilderOrDefault);

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueStringBuilderOrDefault)
              , SimpleTypeAsValueStringBuilderOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringBuilderNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString
{
    public StringBuilder SimpleTypeAsValueStringBuilderOrDefault
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
        tos.StartSimpleValueType(this)
           .AsValue
               (SimpleTypeAsValueStringBuilderOrDefault
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringBuilderRangeWithFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValue
               (nameof(SimpleTypeAsValueStringBuilderRangeOrDefault)
              , SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesZero)]
public class SimpleAsValueStringBuilderRangeNoFieldOrDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder SimpleTypeAsValueStringBuilderRangeOrDefault
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
        tos.StartSimpleValueType(this)
           .AsValue
               (SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueStringBuilderRangeWithFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeOrDefault);
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrNull
               (nameof(SimpleTypeAsValueStringBuilderRangeOrDefault)
              , SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueStringBuilderRangeNoFieldSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeOrDefault
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
           .AsValueOrNull
               (SimpleTypeAsValueStringBuilderRangeOrDefault
              , FromIndex, Length, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueStringBuilderRangeWithFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueStringBuilderRangeWithDefault);

    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (nameof(SimpleTypeAsValueStringBuilderRangeWithDefault)
              , SimpleTypeAsValueStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)]
public class SimpleAsValueStringBuilderRangeNoFieldWithDefaultSimpleValueTypeStringBearer : FormattedMoldScaffold<StringBuilder?>
  , ISupportsSettingValueFromString, IMoldSupportedDefaultValue<string>, ISupportsIndexRangeLimiting
{
    public StringBuilder? SimpleTypeAsValueStringBuilderRangeWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => "";
    public string DefaultValue { get; set; } = "";
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueOrDefault
               (SimpleTypeAsValueStringBuilderRangeWithDefault
              , FromIndex, Length, DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueMatchOrDefaultSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? SimpleTypeAsValueMatchOrDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueMatchOrDefault);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatch(nameof(SimpleTypeAsValueMatchOrDefault), SimpleTypeAsValueMatchOrDefault, ValueFormatString
                       , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsValueOut | DefaultBecomesNull)]
public class SimpleAsValueMatchSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
{
    public TAny? SimpleTypeAsValueMatch
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueMatch);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatchOrNull(nameof(SimpleTypeAsValueMatch), SimpleTypeAsValueMatch, ValueFormatString
                             , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(SimpleType | SingleValueCardinality | AcceptsAnyGeneric | SupportsValueFormatString
                | SupportsSettingDefaultValue | DefaultTreatedAsValueOut | DefaultBecomesFallbackString)]
public class SimpleAsValueMatchWithDefaultSimpleValueTypeStringBearer<TAny> : FormattedMoldScaffold<TAny?>
  , IMoldSupportedDefaultValue<string>
{
    public TAny? SimpleTypeAsValueMatchWithDefault
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(SimpleTypeAsValueMatchWithDefault);

    public string DefaultValue { get; set; } = "";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartSimpleValueType(this)
           .AsValueMatchOrDefault
               (nameof(SimpleTypeAsValueMatchWithDefault)
              , SimpleTypeAsValueMatchWithDefault
              , DefaultValue, ValueFormatString, FormattingFlags)
           .Complete();
}
