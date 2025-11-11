// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class TestCollections
{
    public static readonly bool[]     BoolArray = [ true, true, false, true, false, false, false, true, true, false, false, true, true, true];
    public static readonly bool?[]    NullBoolArray = [ true, null, false, true, false, null, false, true, true, null, false, true, null, null];
    public static readonly List<bool> BoolList  = [..BoolArray];

    public static readonly List<bool?> NullBoolList  = [..NullBoolArray];
    public static PalantírReveal<bool> BoolPad10Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "\"{0,10}\"");
    public static PalantírReveal<bool> BoolRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<bool> NullBoolPad10Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "\"{0,10}\"");

    public static PalantírReveal<bool> NullBoolRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);

    public static OrderedCollectionPredicate<bool> Bool_All_True       = (count, item) => EvaluateIsIncludedAndContinue(item);
    public static OrderedCollectionPredicate<bool> Bool_All_False      = (count, item) => EvaluateIsIncludedAndContinue(!item);
    public static OrderedCollectionPredicate<bool> Bool_First_8        = (count, item) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<bool> Bool_Skip_Odd_Index = (count, item) => EvaluateIsIncludedAndContinue(((count -1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<bool> Bool_All            = (count, item) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<bool> Bool_First_False            = (count, item) => First(!item);

    public static OrderedCollectionPredicate<bool> Bool_None      = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<bool?> NullBool_All_True        = (count, item) => EvaluateIsIncludedAndContinue(item.HasValue && item.Value);
    public static OrderedCollectionPredicate<bool?> NullBool_All_NullOrFalse = (count, item) => EvaluateIsIncludedAndContinue(!item.HasValue || !item.Value);
    public static OrderedCollectionPredicate<bool?> NullBool_First_8         = (count, item) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<bool?> NullBool_Skip_Odd_Index  = (count, item) => EvaluateIsIncludedAndContinue(((count -1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<bool?> NullBool_All             = (count, item) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<bool?> NullBool_None            = (count, item) => EvaluateIsIncludedAndContinue(false);
    public static OrderedCollectionPredicate<bool?> NullBool_First_False            = (count, item) => First(item is false);
    
    public static readonly float[] FloatArray = [(float)Math.PI, (float)Math.E, (float)Math.PI * 2, (float)Math.E * 2, (float)Math.PI * 4
      , (float)Math.E * 4, (float)Math.PI * 6, (float)Math.E * 6, (float)Math.PI * 8, (float)Math.E * 8, ];
    public static readonly List<float> FloatList = [..FloatArray];
    public static readonly float?[] NullFloatArray = [null, (float)Math.PI, (float)Math.E, null, null, (float)Math.PI * 3, null, null, null
      , (float)Math.E * 3, (float)Math.PI * 4, (float)Math.E * 4,  null, (float)Math.PI * 5, null, (float)Math.E * 5, ];

    public static readonly List<float?> NullFloatList = [..NullFloatArray];

    public static PalantírReveal<float> FloatRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<float> FloatF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");
    public static PalantírReveal<float> FloatF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");
    public static PalantírReveal<float?> NullFloatRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe);
    public static PalantírReveal<float?> NullFloatF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F2");
    public static PalantírReveal<float?> NullFloatF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F4");
    public static PalantírReveal<float?> NullFloatF2Default42Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, 42.0, "F2");

    public static PalantírReveal<float?> NullFloatF4Default42Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, "42", "F4");

    public static OrderedCollectionPredicate<float>  Float_Lt_10          = (count, item) => EvaluateIsIncludedAndContinue(item < 10.0f);
    public static OrderedCollectionPredicate<float>  Float_Gt_10          = (count, item) => EvaluateIsIncludedAndContinue(item > 10.0f);
    public static OrderedCollectionPredicate<float>  Float_First_5        = (count, item) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float>  Float_First_2        = (count, item) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float>  Float_Skip_Odd_Index = (count, item) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float>  Float_All            = (count, item) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<float> Float_First_Gt_10 = (count, item) => First(item > 10.0f);

    public static OrderedCollectionPredicate<float> Float_None           = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<float?> NullFloat_Lt_10          = (count, item) => EvaluateIsIncludedAndContinue(item < 10.0f);
    public static OrderedCollectionPredicate<float?> NullFloat_Gt_10          = (count, item) => EvaluateIsIncludedAndContinue(item > 10.0f);
    public static OrderedCollectionPredicate<float?> NullFloat_First_2        = (count, item) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float?> NullFloat_First_5        = (count, item) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float?> NullFloat_Skip_Odd_Index = (count, item) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float?> NullFloat_All            = (count, item) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<float?> NullFloat_First_Gt_10    = (count, item) => First(item is > 10.0f);

    public static OrderedCollectionPredicate<float?> NullFloat_None           = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly sbyte[]     SByteArray = [-128, -127, -126, -125, -124, 0, 1, 2, 3, 4, 5, 6, 7, 8, 120, 121, 122, 123, 124, 125, 126, 127];
    public static readonly List<sbyte> SByteList  = [..SByteArray];
    public static readonly sbyte?[]     NullSByteArray = [-128, -127, null, -125, -124, -2, -1, 0, 1, null, 4, 5, null, 120, 121, null, 126, 127];
    public static readonly List<sbyte?> NullSByteList  = [..SByteArray];
    
    public static PalantírReveal<sbyte> SByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<sbyte> SByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");
    public static PalantírReveal<sbyte> SByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");
    
    public static PalantírReveal<sbyte?> NullSByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F0");
    public static PalantírReveal<sbyte?> NullSByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "{0,15:F2}");
    public static PalantírReveal<sbyte?> NullSByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F4");
    public static PalantírReveal<sbyte?> NullSByteF0OrDefault42Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, 42, "F0");
    public static PalantírReveal<sbyte?> NullSByteF2OrDefault42Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, "42.00,", "{0,15:F2}");

    public static PalantírReveal<sbyte?> NullSByteF4OrDefault42Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, 42, "F4");

    public static OrderedCollectionPredicate<sbyte> SByte_Lt_10    = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte> SByte_Gt_10    = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte> SByte_All = (count, item) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<sbyte> SByte_None = (count, item) => EvaluateIsIncludedAndContinue(false);
    
    public static OrderedCollectionPredicate<sbyte?> NullSByte_Lt_10    = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_Gt_10    = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_All = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<sbyte?> NullSByte_None = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly byte[]     ByteArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte> ByteList  = [..ByteArray];
    public static readonly byte?[]     NullByteArray = [0, 1, null, 2, 3, 4, 5, null, 9, 245, null, null, null, null, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte?> NullByteList  = [..ByteArray];
    
    public static PalantírReveal<byte> ByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<byte> ByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "{0,15:F2}");
    public static PalantírReveal<byte> ByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");
    public static PalantírReveal<byte?> NullByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe);
    public static PalantírReveal<byte?> NullByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "{0,15:F2}");

    public static PalantírReveal<byte?> NullByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F4");

    public static OrderedCollectionPredicate<byte> Byte_Lt_10  = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<byte> Byte_Gt_10  = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<byte> Byte_All = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<byte> Byte_None  = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly long[] LongArray =
    [
        long.MinValue, long.MinValue + 1, long.MinValue + 2, long.MinValue + 2, long.MinValue + 4
      , -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, long.MaxValue - 4, long.MaxValue - 3, long.MaxValue - 2, long.MaxValue - 1, long.MaxValue
    ];
    public static readonly List<long> LongList = [..LongArray];
    
    public static PalantírReveal<long> LongF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<long> LongF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<long> LongF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<long> Long_Lt_10 = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<long> Long_Gt_10 = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<long> Long_All   = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<long> Long_None  = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly ulong[]     ULongArray = [0, 1, 2, 3, 4, 5, ulong.MaxValue - 3, ulong.MaxValue - 2, ulong.MaxValue - 1, ulong.MaxValue];
    public static readonly List<ulong> ULongList  = [..ULongArray];
    
    public static PalantírReveal<ulong> ULongF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<ulong> ULongF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<ulong> ULongF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<ulong> ULong_Lt_10 = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<ulong> ULong_Gt_10 = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<ulong> ULong_All   = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<ulong> ULong_None  = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly decimal[] DecimalArray =
    [
        new(Math.PI), new(Math.E), new(Math.PI * 2), new(Math.E * 2), new(Math.PI * 3), new(Math.E * 3), new(Math.PI * 4), new(Math.E * 4)
      , new(Math.PI * 5), new(Math.E * 5), new(Math.PI * 6), new(Math.E * 6), new(Math.PI * 7), new(Math.E * 7), new(Math.PI * 8), new(Math.E * 8)
      , new(Math.PI * 9), new(Math.E * 9), new(Math.PI * 10), new(Math.E * 10), new(Math.PI * 11), new(Math.E * 11), new(Math.PI * 12)
      , new(Math.E * 12)
    ];
    public static readonly List<decimal> DecimalList = [..DecimalArray];
    
    public static PalantírReveal<decimal> DecimalF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<decimal> DecimalF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<decimal> DecimalF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<decimal> Decimal_Lt_10 = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<decimal> Decimal_Gt_10 = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<decimal> Decimal_All   = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<decimal> Decimal_None  = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly BigInteger[] BigIntegerArray =
    [
        new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), new BigInteger(ulong.MaxValue) * new BigInteger(3)
      , new BigInteger(long.MinValue) * new BigInteger(2), new BigInteger(long.MinValue) * new BigInteger(3000)
    ];
    public static readonly List<BigInteger> BigIntegerList = [..BigIntegerArray];
    
    public static PalantírReveal<BigInteger> BigIntegerF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<BigInteger> BigIntegerF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<BigInteger> BigIntegerF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<BigInteger> BigInteger_Lt_10 = (count, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_Gt_10 = (count, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_All   = (count, item) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<BigInteger> BigInteger_None  = (count, item) => EvaluateIsIncludedAndContinue(false);

    public static readonly TimeSpanConfig[] StyledToStringArray =
    [
        new(23, 42, 21, 22, 7), new(921, 38, 9, 1, 6)
      , new(701, 52, 8, 15, 5), new(123, 3, 23, 7, 4)
      , new(259, 16, 42, 23, 3), new(20, 38, 9, 10, 2)
      , new(873, 41, 20, 3, 1), new(0)
    ];
    public static readonly List<TimeSpanConfig> StyledToStringList = [..StyledToStringArray];
    

    public static PalantírReveal<TimeSpanConfig> TimeSpanConfigF0Revealer = (showMe, tos) => showMe.RevealState(tos);

    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_Lt_250d = (count, item) => 
        EvaluateIsIncludedAndContinue(item.ToTimeSpan() < TimeSpan.FromDays(250));
    
    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_Gt_300d = (count, item) => 
        EvaluateIsIncludedAndContinue(item.ToTimeSpan() < TimeSpan.FromDays(300));
    
    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_All   = (count, item) => 
        EvaluateIsIncludedAndContinue(true);
    
    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_None  = (count, item) => 
        EvaluateIsIncludedAndContinue(false);
}
