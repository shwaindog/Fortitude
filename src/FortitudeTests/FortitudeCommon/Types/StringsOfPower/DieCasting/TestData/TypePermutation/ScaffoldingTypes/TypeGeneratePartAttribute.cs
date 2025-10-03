// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[Flags]
public enum TypeGeneratePartFlags : uint
{
    None                      = 0x00_00_00_00
  , ValueType                 = 0x00_00_00_01
  , ComplexType               = 0x00_00_00_02
  , CollectionType            = 0x00_00_00_04
  , KeyedCollectionType       = 0x00_00_00_08
  , AcceptsCollection         = 0x00_00_00_10
  , AcceptsKeyValueCollection = 0x00_00_00_20
  , AcceptsSingleValue        = 0x00_00_00_40
  , AcceptsStruct             = 0x00_00_00_80
  , AcceptsClass              = 0x00_00_01_00
  , AcceptsNullableStruct     = 0x00_00_02_00
  , AcceptsNullableClass      = 0x00_00_04_00
  , AcceptsChars              = 0x00_00_08_00
  , AcceptsSpanFormattable    = 0x00_00_10_00
  , AcceptsIntegerNumber      = 0x00_00_30_00
  , AcceptsDecimalNumber      = 0x00_00_50_00
  , AcceptsDateTimeLike       = 0x00_00_90_00
  , AcceptsStringBearer       = 0x00_01_00_00
  , AcceptsAny                = 0x00_01_FF_80
  , AcceptsMask               = 0x00_01_FF_F0
  , AlwaysWrites              = 0x00_02_00_00
  , OnlyPopulatedWrites       = 0x00_04_00_00
  , NonNullWrites             = 0x00_08_00_00
  , NonNullAndPopulatedWrites  = 0x00_10_00_00
  , OutputConditionMask       = 0x00_1E_00_00
  , CollectionFilterPredicate = 0x00_20_00_00
  , KeyValueFilterPredicate   = 0x00_40_00_00
  , SelectValueFilter         = 0x00_80_00_00
  , OneFormatString           = 0x01_00_00_00
  , TwoFormatStrings          = 0x02_00_00_00
  , OnePalantirRevealer       = 0x04_00_00_00
  , TwoPalantirRevealers      = 0x08_00_00_00
  , SupportsCustomHandling    = 0x10_00_00_00
  , SupportsIndexSubRanges    = 0x20_00_00_00
}

public class TypeGeneratePartAttribute(TypeGeneratePartFlags flags) : Attribute { }

public interface IMoldSupportedValue<TValue>
{
    [JsonIgnore] TValue Value { get; set; }
}

public interface IMoldSupportedDefaultValue<TValue>
{
    [JsonIgnore] TValue DefaultValue { get; set; }
}

public interface ISupportsSingleFormatString
{
    [JsonIgnore] string? FormatString { get; set; }
}

public interface ISupportsDualFormatStrings
{
    [JsonIgnore] string? KeyFormatString { get; set; }

    [JsonIgnore] string? ValueFormatString { get; set; }
}

public interface ISupportsOrderedCollectionPredicate<TElement>
{
    [JsonIgnore] OrderedCollectionPredicate<TElement> ElementPredicate { get; set; }
}

public interface ISupportsKeyedCollectionPredicate<TKey, TValue>
{
    [JsonIgnore] KeyValuePredicate<TKey, TValue> KeyValuePredicate { get; set; }
}

public interface ISupportsSingleRevealer<TRevealerType>
{
    [JsonIgnore] PalantírReveal<TRevealerType> PalantirRevealer { get; set; }
}

public interface ISupportsDualRevealer<TCloaked>
{
    [JsonIgnore] PalantírReveal<TCloaked> KeyRevealer { get; set; }

    [JsonIgnore] PalantírReveal<TCloaked> ValueRevealer { get; set; }
}

public interface ISupportsIndexRangeLimiting
{
    [JsonIgnore] int FromIndex { get; set; }

    [JsonIgnore] int Length { get; set; }
}

public interface ISupportsFieldHandling
{
    [JsonIgnore] FieldContentHandling FieldContentHandling { get; set; }
}

public interface ISupportsSettingValueFromString
{
    [JsonIgnore] string? StringValue { get; set; }
}
