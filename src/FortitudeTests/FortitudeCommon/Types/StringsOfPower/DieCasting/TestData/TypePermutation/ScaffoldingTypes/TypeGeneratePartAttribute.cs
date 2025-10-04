// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

[Flags]
public enum TypeGeneratePartFlags : ulong
{
    None                      = 0x00_00_00_00_00
  , ValueType                 = 0x00_00_00_00_01
  , ComplexType               = 0x00_00_00_00_02
  , CollectionType            = 0x00_00_00_00_04
  , KeyedCollectionType       = 0x00_00_00_00_08
  , AcceptsCollection         = 0x00_00_00_00_10
  , AcceptsKeyValueCollection = 0x00_00_00_00_20
  , AcceptsSingleValue        = 0x00_00_00_00_40
  , AcceptsStruct             = 0x00_00_00_00_80
  , AcceptsClass              = 0x00_00_00_01_00
  , AcceptsNullableStruct     = 0x00_00_00_02_00
  , AcceptsNullableClass      = 0x00_00_00_04_00
  , AcceptsChars              = 0x00_00_00_08_00
  , AcceptsSpanFormattable    = 0x00_00_00_10_00
  , AcceptsIntegerNumber      = 0x00_00_00_30_00
  , AcceptsDecimalNumber      = 0x00_00_00_50_00
  , AcceptsDateTimeLike       = 0x00_00_00_90_00
  , AcceptsStringBearer       = 0x00_00_01_00_00
  , AcceptsArray              = 0x00_00_02_00_00
  , AcceptsList               = 0x00_00_04_00_00
  , AcceptsDictionary         = 0x00_00_08_00_00
  , AcceptsEnumerable         = 0x00_00_10_00_00
  , AcceptsEnumerator         = 0x00_00_20_00_00
  , AcceptsAny                = 0x00_00_2F_FF_80
  , CallsAsSpan               = 0x00_00_40_00_00
  , CallsAsReadOnlySpan       = 0x00_00_80_00_00
  , AcceptsMask               = 0x00_00_FF_FF_F0
  , AlwaysWrites              = 0x00_01_00_00_00
  , OnlyPopulatedWrites       = 0x00_02_00_00_00
  , NonNullWrites             = 0x00_04_00_00_00
  , NonNullAndPopulatedWrites = 0x00_08_00_00_00
  , OutputConditionMask       = 0x00_0F_00_00_00
  , FilterPredicate           = 0x00_10_00_00_00
  , StatefulFilter            = 0x00_20_00_00_00
  , SupportsValueFormatString         = 0x00_40_00_00_00
  , SupportsKeyFormatString           = 0x00_80_00_00_00
  , SupportsValueRevealer             = 0x01_00_00_00_00
  , SupportsKeyRevealer               = 0x02_00_00_00_00
  , SupportsCustomHandling    = 0x04_00_00_00_00
  , SupportsIndexSubRanges    = 0x08_00_00_00_00
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

public interface ISupportsValueFormatString
{
    [JsonIgnore] string? ValueFormatString { get; set; }
}

public interface ISupportsKeyFormatString
{
    [JsonIgnore] string? KeyFormatString { get; set; }
}

public interface ISupportsOrderedCollectionPredicate<TElement>
{
    public static OrderedCollectionPredicate<TElement> GetNoFilterPredicate => (_, _) => CollectionItemResult.IncludeContinueToNext;

    [JsonIgnore] OrderedCollectionPredicate<TElement> ElementPredicate { get; set; }
}

public interface ISupportsKeyedCollectionPredicate<TKey, TValue>
{
    public static KeyValuePredicate<TKey, TValue> GetNoFilterPredicate => (_, _, _) => CollectionItemResult.IncludeContinueToNext;
    [JsonIgnore] KeyValuePredicate<TKey, TValue> KeyValuePredicate { get; set; }
}

public interface ISupportsValueRevealer<TRevealerType>
{
    [JsonIgnore] PalantírReveal<TRevealerType> ValueRevealer { get; set; }
}

public interface ISupportsKeyRevealer<TCloaked>
{
    [JsonIgnore] PalantírReveal<TCloaked> KeyRevealer { get; set; }
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
