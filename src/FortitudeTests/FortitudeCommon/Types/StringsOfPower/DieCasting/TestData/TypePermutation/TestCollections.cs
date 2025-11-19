// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;

// ReSharper disable InconsistentNaming
// ReSharper disable FormatStringProblem
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public static class TestCollections
{
    public static readonly bool[]     BoolArray     = [true, true, false, true, false, false, false, true, true, false, false, true, true, true];
    public static readonly bool?[]    NullBoolArray = [true, null, false, true, false, null, false, true, true, null, false, true, null, null];
    public static readonly List<bool> BoolList      = [..BoolArray];

    public static readonly List<bool?> NullBoolList = [..NullBoolArray];
    public static PalantírReveal<bool> BoolPad10Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "\"{0,10}\"");
    public static PalantírReveal<bool> BoolRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<bool> NullBoolPad10Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "\"{0,10}\"");

    public static PalantírReveal<bool> NullBoolRevealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);

    public static OrderedCollectionPredicate<bool> Bool_All_True       = (_, item) => EvaluateIsIncludedAndContinue(item);
    public static OrderedCollectionPredicate<bool> Bool_All_False      = (_, item) => EvaluateIsIncludedAndContinue(!item);
    public static OrderedCollectionPredicate<bool> Bool_First_8        = (count, _) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<bool> Bool_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<bool> Bool_Skip_Odd_Index = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<bool> Bool_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<bool> Bool_First_False    = (_, item) => First(!item);

    public static OrderedCollectionPredicate<bool> Bool_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<bool?> NullBool_All_True = (_, item) => EvaluateIsIncludedAndContinue(item.HasValue && item.Value);
    public static OrderedCollectionPredicate<bool?> NullBool_All_NullOrFalse
        = (_, item) => EvaluateIsIncludedAndContinue(!item.HasValue || !item.Value);
    public static OrderedCollectionPredicate<bool?> NullBool_First_8        = (count, _) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<bool?> NullBool_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<bool?> NullBool_Skip_Odd_Index = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<bool?> NullBool_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<bool?> NullBool_None           = (_, _) => EvaluateIsIncludedAndContinue(false);
    public static OrderedCollectionPredicate<bool?> NullBool_First_False    = (_, item) => First(item is false);

    public static readonly float[] FloatArray =
    [
        (float)Math.PI, (float)Math.E, (float)Math.PI * 2, (float)Math.E * 2, (float)Math.PI * 4
      , (float)Math.E * 4, (float)Math.PI * 6, (float)Math.E * 6, (float)Math.PI * 8, (float)Math.E * 8,
    ];
    public static readonly List<float> FloatList = [..FloatArray];
    public static readonly float?[] NullFloatArray =
    [
        null, (float)Math.PI, (float)Math.E, null, null, (float)Math.PI * 3, null, null, null
      , (float)Math.E * 3, (float)Math.PI * 4, (float)Math.E * 4, null, (float)Math.PI * 5, null, (float)Math.E * 5,
    ];

    public static readonly List<float?> NullFloatList = [..NullFloatArray];

    public static PalantírReveal<float> FloatRevealer   = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<float> FloatF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");
    public static PalantírReveal<float> FloatF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<float> Float_Lt_10          = (_, item) => EvaluateIsIncludedAndContinue(item < 10.0f);
    public static OrderedCollectionPredicate<float> Float_Gt_10          = (_, item) => EvaluateIsIncludedAndContinue(item > 10.0f);
    public static OrderedCollectionPredicate<float> Float_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float> Float_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<float> Float_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float> Float_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float> Float_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<float> Float_First_Gt_10    = (_, item) => First(item > 10.0f);

    public static OrderedCollectionPredicate<float> Float_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<float?> NullFloat_Lt_10          = (_, item) => EvaluateIsIncludedAndContinue(item < 10.0f);
    public static OrderedCollectionPredicate<float?> NullFloat_Gt_10          = (_, item) => EvaluateIsIncludedAndContinue(item > 10.0f);
    public static OrderedCollectionPredicate<float?> NullFloat_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float?> NullFloat_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float?> NullFloat_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<float?> NullFloat_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float?> NullFloat_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<float?> NullFloat_First_Gt_10    = (_, item) => First(item is > 10.0f);

    public static OrderedCollectionPredicate<float?> NullFloat_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly sbyte[] SByteArray = [-128, -127, -126, -125, -124, 0, 1, 2, 3, 4, 5, 6, 7, 8, 120, 121, 122, 123, 124, 125, 126, 127];
    public static readonly List<sbyte> SByteList = [..SByteArray];
    public static readonly sbyte?[] NullSByteArray = [-128, -127, null, -125, -124, -2, -1, 0, 1, null, 4, 5, null, 120, 121, null, 126, 127];
    public static readonly List<sbyte?> NullSByteList = [..SByteArray];


    public static PalantírReveal<sbyte> SByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F0");
    public static PalantírReveal<sbyte> SByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "{0,15:F2}");
    public static PalantírReveal<sbyte> SByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrNull(showMe, "F4");
    public static PalantírReveal<sbyte> SByteF0OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, 42, "F0");
    public static PalantírReveal<sbyte> SByteF2OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, "42.00,", "{0,15:F2}");

    public static PalantírReveal<sbyte> SByteF4OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValueOrDefault(showMe, 42, "F4");

    public static OrderedCollectionPredicate<sbyte> SByte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte> SByte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte> SByte_All   = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<sbyte> SByte_None  = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<sbyte?> NullSByte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<sbyte?> NullSByte_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly byte[]      ByteArray     = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte>  ByteList      = [..ByteArray];
    public static readonly byte?[]     NullByteArray = [0, 1, null, 2, 3, 4, 5, null, 9, 245, null, null, null, null, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte?> NullByteList  = [..ByteArray];

    public static PalantírReveal<byte> ByteF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<byte> ByteF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "{0,15:F2}");
    public static PalantírReveal<byte> ByteF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<byte> Byte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<byte> Byte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<byte> Byte_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<byte> Byte_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly long[] LongArray =
    [
        long.MinValue, long.MinValue + 1, long.MinValue + 2, long.MinValue + 2, long.MinValue + 4
      , -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, long.MaxValue - 4, long.MaxValue - 3, long.MaxValue - 2, long.MaxValue - 1, long.MaxValue
    ];
    public static readonly List<long> LongList = [..LongArray];

    public static PalantírReveal<long> LongF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<long> LongF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<long> LongF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<long> Long_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<long> Long_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<long> Long_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<long> Long_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly ulong[]     ULongArray = [0, 1, 2, 3, 4, 5, ulong.MaxValue - 3, ulong.MaxValue - 2, ulong.MaxValue - 1, ulong.MaxValue];
    public static readonly List<ulong> ULongList  = [..ULongArray];

    public static PalantírReveal<ulong> ULongF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<ulong> ULongF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<ulong> ULongF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<ulong> ULong_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<ulong> ULong_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<ulong> ULong_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<ulong> ULong_None = (_, _) => EvaluateIsIncludedAndContinue(false);

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

    public static OrderedCollectionPredicate<decimal> Decimal_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<decimal> Decimal_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<decimal> Decimal_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<decimal> Decimal_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly BigInteger[] BigIntegerArray =
    [
        new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), new BigInteger(ulong.MaxValue) * new BigInteger(3)
      , new BigInteger(long.MinValue) * new BigInteger(2), new BigInteger(long.MinValue) * new BigInteger(3000)
    ];
    public static readonly List<BigInteger> BigIntegerList = [..BigIntegerArray];

    public static PalantírReveal<BigInteger> BigIntegerF0Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<BigInteger> BigIntegerF2Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<BigInteger> BigIntegerF4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<BigInteger> BigInteger_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_All   = (_, _) => EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<BigInteger> BigInteger_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly TimeSpanConfig[] StyledToStringArray =
    [
        new(23, 42, 21, 22, 7), new(921, 38, 9, 1, 6)
      , new(701, 52, 8, 15, 5), new(123, 3, 23, 7, 4)
      , new(259, 16, 42, 23, 3), new(20, 38, 9, 10, 2)
      , new(873, 41, 20, 3, 1), new(0)
    ];
    public static readonly List<TimeSpanConfig> StyledToStringList = [..StyledToStringArray];


    public static PalantírReveal<TimeSpanConfig> TimeSpanConfigF0Revealer = (showMe, tos) => showMe.RevealState(tos);

    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_Lt_250d = (_, item) =>
        EvaluateIsIncludedAndContinue(item.ToTimeSpan() < TimeSpan.FromDays(250));

    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_Gt_300d = (_, item) =>
        EvaluateIsIncludedAndContinue(item.ToTimeSpan() < TimeSpan.FromDays(300));

    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_All = (_, _) =>
        EvaluateIsIncludedAndContinue(true);

    public static OrderedCollectionPredicate<TimeSpanConfig> TimeSpanConfig_None = (_, _) =>
        EvaluateIsIncludedAndContinue(false);


    public static readonly Version[] VersionsArray =
    [
        new(0, 0), new(0, 1, 1), new(1, 1, 1, 1), new(2, 1, 123456)
      , new(4, 2, 25), new(8, 3, 3, 3), new(0, 4), new(16, 0, 0), new(32, 2563, 1000000, 1),
    ];
    public static readonly Version?[] NullVersionsArray =
    [
        null, new(0, 0), null, new(0, 1, 1), new(1, 1, 1, 1)
      , new(2, 1, 123456), new(8, 3, 3, 3), null, null, null, null, new(16, 0, 0)
      , new(32, 2563, 1000000, 1), null, null, null
    ];
    public static readonly List<Version>  VersionsList     = [..VersionsArray];
    public static readonly List<Version?> NullVersionsList = [..NullVersionsArray];

    public static PalantírReveal<Version> VersionRevealer            = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<Version> VersionRevealer_Pad_20     = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "{0,20}");
    public static PalantírReveal<Version> VersionRevealer_F4Revealer = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<Version> Version_MjrLt_10       = (_, item) => EvaluateIsIncludedAndContinue(item.Major < 10);
    public static OrderedCollectionPredicate<Version> Version_MjrGt_10       = (_, item) => EvaluateIsIncludedAndContinue(item.Major > 10);
    public static OrderedCollectionPredicate<Version> Version_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<Version> Version_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<Version> Version_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<Version> Version_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<Version> Version_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<Version> Version_First_MjrGt_10 = (_, item) => First(item.Major > 10);
    public static OrderedCollectionPredicate<Version> Version_None           = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<Version?> NullVersion_MjrLt_10       = (_, item) => EvaluateIsIncludedAndContinue(item?.Major < 10);
    public static OrderedCollectionPredicate<Version?> NullVersion_Gt_10          = (_, item) => EvaluateIsIncludedAndContinue(item?.Major > 10);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_5        = (count, _) => EvaluateIsIncludedAndContinue(count <= 5);
    public static OrderedCollectionPredicate<Version?> NullVersion_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<Version?> NullVersion_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<Version?> NullVersion_All            = (_, _) => EvaluateIsIncludedAndContinue(true);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_MjrGt_10 = (_, item) => First(item?.Major is > 10);
    public static OrderedCollectionPredicate<Version?> NullVersion_None           = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static string[] StringArray =
    [
        "<Title author=\"A RR Token\">Origin of the 𝄞trings & spin offs</Title>"
      , "It began with the forging of the Great Strings."
      , "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
      , "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
      , "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. "
      , "For within these strings was bound the flexibility, mutability and the operators to govern each language"
      , "But they were all of them deceived, for another string was made."
      , "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, " +
        " and into this string he poured his unambiguity, his immutability desires and his will to replace all "
      , "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them"
      , "<ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>"
    ];

    public static List<string> StringList = [..StringArray];
    public static Lazy<List<string?>> NullStringList = new(() =>
    {
        var stringWithNulls = new List<string?>();
        for (int i = 0; i < StringArray.Length; i++)
        {
            var stringAtIndex = StringArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    stringWithNulls.Add(null);
                    stringWithNulls.Add(stringAtIndex);
                    break;
                case 3:
                    stringWithNulls.Add(null);
                    stringWithNulls.Add(null);
                    stringWithNulls.Add(stringAtIndex);
                    break;
                default: stringWithNulls.Add(stringAtIndex); break;
            }
        }
        return stringWithNulls;
    });

    public static PalantírReveal<string> StringRevealer        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<string> StringRevealer_Pad_20 = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0}|");
    public static PalantírReveal<string> StringRevealer_Pad_80 = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0,80}|");

    public static OrderedCollectionPredicate<string> String_LenLt_100       = (_, item) => EvaluateIsIncludedAndContinue(item.Length < 100);
    public static OrderedCollectionPredicate<string> String_LenGt_100       = (_, item) => EvaluateIsIncludedAndContinue(item.Length > 100);
    public static OrderedCollectionPredicate<string> String_First_5         = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<string> String_Second_5        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<string> String_First_2         = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<string> String_Skip_Odd_Index  = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<string> String_All             = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<string> String_First_LenGt_100 = (_, item) => First(item.Length > 100);
    public static OrderedCollectionPredicate<string> String_None            = (_, _) => NotIncludedAndComplete;

    public static OrderedCollectionPredicate<string?> NullString_LenLt_100       = (_, item) => EvaluateIsIncludedAndContinue(item?.Length < 100);
    public static OrderedCollectionPredicate<string?> NullString_LenGt_100       = (_, item) => EvaluateIsIncludedAndContinue(item?.Length > 100);
    public static OrderedCollectionPredicate<string?> NullString_First_2         = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<string?> NullString_First_5         = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<string?> NullString_Second_5        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<string?> NullString_Skip_Odd_Index  = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<string?> NullString_All             = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<string?> NullString_First_LenGt_100 = (_, item) => First(item?.Length is > 100);
    public static OrderedCollectionPredicate<string?> NullString_None            = (_, _) => NotIncludedAndComplete;

    public static Lazy<List<char[]>> CharArrayList = new(() => StringArray.Select(s => s.ToCharArray()).ToList());
    public static Lazy<List<char[]?>> NullCharArrayList = new(() =>
    {
        var withNulls = new List<char[]?>();
        for (int i = 0; i < StringArray.Length; i++)
        {
            var stringAtIndex = StringArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(stringAtIndex.ToCharArray());
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(stringAtIndex.ToCharArray());
                    break;
                default: withNulls.Add(stringAtIndex.ToCharArray()); break;
            }
        }
        return withNulls;
    });

    public static Lazy<List<MutableString>> MutableStringList = new(() => StringArray.Select(s => new MutableString(s)).ToList());
    public static Lazy<List<MutableString?>> NullMutableStringList = new(() =>
    {
        var withNulls = new List<MutableString?>();
        for (int i = 0; i < StringArray.Length; i++)
        {
            var stringAtIndex = StringArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(new MutableString(stringAtIndex));
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(new MutableString(stringAtIndex));
                    break;
                default: withNulls.Add(new MutableString(stringAtIndex)); break;
            }
        }
        return withNulls;
    });

    public static PalantírReveal<MutableString> MutableStringRevealer = (showMe, tos) => 
        tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<MutableString> MutableStringRevealer_Pad_20
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0}|");
    public static PalantírReveal<MutableString> MutableStringRevealer_Pad_80
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0,80}|");

    public static OrderedCollectionPredicate<MutableString> MutableString_LenLt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item.Length < 100);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_LenGt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item.Length > 100);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_First_5 = (count, _) => 
        StopOnFirstExclusion(count <= 5);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_Second_5 = (count, _) => 
        BetweenRetrieveRange(count, 6, 11);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_First_2 = (count, _) => StopOnFirstExclusion(count <= 2);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);
    
    public static OrderedCollectionPredicate<MutableString> MutableString_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<MutableString> MutableString_First_LenGt_100 = (_, item) => First(item.Length > 100);
    public static OrderedCollectionPredicate<MutableString> MutableString_None = (_, _) => NotIncludedAndComplete;

    public static OrderedCollectionPredicate<MutableString?> NullMutableString_LenLt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Length < 100);
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_LenGt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Length > 100);
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_First_2         = (count, _) => 
        StopOnFirstExclusion(count <= 2);
    
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_First_5         = (count, _) => 
        StopOnFirstExclusion(count <= 5);
    
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_Second_5        = (count, _) => 
        BetweenRetrieveRange(count, 6, 11);
    
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_Skip_Odd_Index  = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_All             = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_First_LenGt_100 = (_, item) => First(item?.Length is > 100);
    public static OrderedCollectionPredicate<MutableString?> NullMutableString_None            = (_, _) => NotIncludedAndComplete;

    public static Lazy<List<CharArrayStringBuilder>> CharArrayStringBuilderList
        = new(() => StringArray.Select(s => new CharArrayStringBuilder(s)).ToList());
    public static Lazy<List<CharArrayStringBuilder?>> NullCharArrayStringBuilderList = new(() =>
    {
        var withNulls = new List<CharArrayStringBuilder?>();
        for (int i = 0; i < StringArray.Length; i++)
        {
            var stringAtIndex = StringArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(new CharArrayStringBuilder(stringAtIndex));
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(new CharArrayStringBuilder(stringAtIndex));
                    break;
                default: withNulls.Add(new CharArrayStringBuilder(stringAtIndex)); break;
            }
        }
        return withNulls;
    });

    public static PalantírReveal<CharArrayStringBuilder> CharArrayStringBuilderAsValueRevealer
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe);
    public static PalantírReveal<CharArrayStringBuilder> CharArrayStringBuilderAsValueRevealer_Pad_20
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0}|");
    public static PalantírReveal<CharArrayStringBuilder> CharArrayStringBuilderAsValueRevealer_Pad_80
        = (showMe, tos) => tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0,80}|");

    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_LenLt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item.Length < 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_LenGt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item.Length > 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_First_5 = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_First_2 = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_First_LenGt_100 = (_, item) => First(item.Length > 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_None = (_, _) => NotIncludedAndComplete;

    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_LenLt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Length < 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_LenGt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Length > 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_First_2
        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_All            = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_First_LenGt_100
        = (_, item) => First(item?.Length is > 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_None = (_, _) => NotIncludedAndComplete;

    public static Lazy<List<StringBuilder>> StringBuilderList = new(() => StringArray.Select(s => new StringBuilder(s)).ToList());
    public static Lazy<List<StringBuilder?>> NullStringBuilderList = new(() =>
    {
        var withNulls = new List<StringBuilder?>();
        for (int i = 0; i < StringArray.Length; i++)
        {
            var stringAtIndex = StringArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(new StringBuilder(stringAtIndex));
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(new StringBuilder(stringAtIndex));
                    break;
                default: withNulls.Add(new StringBuilder(stringAtIndex)); break;
            }
        }
        return withNulls;
    });

    public static PalantírReveal<StringBuilder> StringBuilderRevealer = (showMe, tos) => 
        tos.StartSimpleValueType(showMe).AsValue(showMe);
    
    public static PalantírReveal<StringBuilder> StringBuilderRevealer_Pad_20 = (showMe, tos) => 
        tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0}|");
    
    public static PalantírReveal<StringBuilder> StringBuilderRevealer_Pad_80 = (showMe, tos) => 
        tos.StartSimpleValueType(showMe).AsValue(showMe, "|{0,80}|");

    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_LenLt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item.Length < 100);
    
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_LenGt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item.Length > 100);
    
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_First_5 = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_Second_5 = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_First_2 = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_First_LenGt_100 = (_, item) => First(item.Length > 100);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_None = (_, _) => NotIncludedAndComplete;

    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_LenLt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item?.Length < 100);
    
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_LenGt_100 = (_, item) => 
        EvaluateIsIncludedAndContinue(item?.Length > 100);
    
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_First_2 = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_First_5 = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_Second_5 = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_All            = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_First_LenGt_100 = (_, item) => First(item?.Length is > 100);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_None = (_, _) => NotIncludedAndComplete;

    public static readonly Lazy<List<FieldSpanFormattableAlwaysAddStringBearer<decimal>>> StringBearerClassList = new(() =>
    {
        return DecimalArray.Select(d => new FieldSpanFormattableAlwaysAddStringBearer<decimal>
        {
            Value = d
        }).ToList();
    });
    public static Lazy<List<FieldSpanFormattableAlwaysAddStringBearer<decimal>?>> NullStringBearerClassList = new(() =>
    {
        var withNulls = new List<FieldSpanFormattableAlwaysAddStringBearer<decimal>?>();
        for (int i = 0; i < DecimalArray.Length; i++)
        {
            var decimalAtIndex = DecimalArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
                default:
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassListRevealer =
        (showMe, tos) => tos.StartSimpleValueType(showMe).RevealAsValue("", showMe);

    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Lt_20 =
        (_, item) => EvaluateIsIncludedAndContinue(item.Value < 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Gt_20 =
        (_, item) => EvaluateIsIncludedAndContinue(item.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_First_5 =
        (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Second_5 =
        (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_First_2 =
        (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Skip_Odd_Index =
        (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_All =
        (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_First_Gt_20 =
        (_, item) => First(item.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_None =
        (_, _) => NotIncludedAndComplete;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_LenLt_20
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Value < 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_LenGt_20
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_First_2
        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_Skip_Odd_Index
        = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_All
        = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_First_LenGt_20
        = (_, item) => First(item?.Value is > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_None
        = (_, _) => NotIncludedAndComplete;
    
    
    public static readonly Lazy<List<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>>> StringBearerStructList = new(() =>
    {
        return DecimalArray.Select(d => new FieldSpanFormattableAlwaysAddStructStringBearer<decimal>
        {
            Value = d
        }).ToList();
    });
    public static Lazy<List<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?>> NullStringBearerStructList = new(() =>
    {
        var withNulls = new List<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?>();
        for (int i = 0; i < DecimalArray.Length; i++)
        {
            var decimalAtIndex = DecimalArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStructStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStructStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
                default:
                    withNulls.Add(new FieldSpanFormattableAlwaysAddStructStringBearer<decimal>
                    {
                        Value = decimalAtIndex
                    });
                    break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructListAsValueRevealer =
        (showMe, tos) => tos.StartSimpleValueType(showMe).RevealAsValue("", showMe);

    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Lt_20 =
        (_, item) => EvaluateIsIncludedAndContinue(item.Value < 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Gt_20 =
        (_, item) => EvaluateIsIncludedAndContinue(item.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_First_5 =
        (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Second_5 =
        (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_First_2 =
        (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Skip_Odd_Index =
        (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_All =
        (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_First_Gt_20 =
        (_, item) => First(item.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_None =
        (_, _) => NotIncludedAndComplete;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_LenLt_20
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Value < 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_LenGt_20
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Value > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_First_2
        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_Skip_Odd_Index
        = (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_All
        = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_First_LenGt_20
        = (_, item) => First(item?.Value is > 20);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_None
        = (_, _) => NotIncludedAndComplete;
}
