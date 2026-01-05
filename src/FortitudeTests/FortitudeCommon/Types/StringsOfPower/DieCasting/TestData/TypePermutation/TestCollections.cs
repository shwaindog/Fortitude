// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
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

    public static OrderedCollectionPredicate<bool>  Bool_First_8            = (count, _) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<bool>  Bool_Second_5           = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<bool>  Bool_First_False        = (_, item) => First(!item);
    public static OrderedCollectionPredicate<bool?> NullBool_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<bool?> NullBool_Skip_Odd_Index = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
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

    public static OrderedCollectionPredicate<float> Float_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float> Float_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<float> Float_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float> Float_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float> Float_First_Gt_10    = (_, item) => First(item > 10.0f);

    public static OrderedCollectionPredicate<float?> NullFloat_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<float?> NullFloat_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<float?> NullFloat_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<float?> NullFloat_Skip_Odd_Index = (_, _) => EvaluateIsIncludedAndContinue(true, 1);
    public static OrderedCollectionPredicate<float?> NullFloat_First_Gt_10    = (_, item) => First(item is > 10.0f);


    public static readonly sbyte[] SByteArray = [-128, -127, -126, -125, -124, 0, 1, 2, 3, 4, 5, 6, 7, 8, 120, 121, 122, 123, 124, 125, 126, 127];
    public static readonly List<sbyte> SByteList = [..SByteArray];
    public static readonly sbyte?[] NullSByteArray = [-128, -127, null, -125, -124, -2, -1, 0, 1, null, 4, 5, null, 120, 121, null, 126, 127];
    public static readonly List<sbyte?> NullSByteList = [..SByteArray];


    public static PalantírReveal<sbyte> SByteF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrNull(showMe, "F0");
    public static PalantírReveal<sbyte> SByteF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrNull(showMe, "{0,15:F2}");
    public static PalantírReveal<sbyte> SByteF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrNull(showMe, "F4");
    public static PalantírReveal<sbyte> SByteF0OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrDefault(showMe, 42, "F0");
    public static PalantírReveal<sbyte> SByteF2OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrDefault(showMe, "42.00,", "{0,15:F2}");

    public static PalantírReveal<sbyte> SByteF4OrDefault42Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueOrDefault(showMe, 42, "F4");

    public static OrderedCollectionPredicate<sbyte> SByte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte> SByte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte> SByte_All   = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<sbyte> SByte_None  = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<sbyte?> NullSByte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<sbyte?> NullSByte_All   = (_, _) => IncludedAndContinue;

    public static OrderedCollectionPredicate<sbyte?> NullSByte_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly byte[]      ByteArray     = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte>  ByteList      = [..ByteArray];
    public static readonly byte?[]     NullByteArray = [0, 1, null, 2, 3, 4, 5, null, 9, 245, null, null, null, null, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte?> NullByteList  = [..ByteArray];

    public static PalantírReveal<byte> ByteF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe);
    public static PalantírReveal<byte> ByteF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "{0,15:F2}");
    public static PalantírReveal<byte> ByteF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<byte> Byte_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<byte> Byte_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<byte> Byte_All   = (_, _) => IncludedAndContinue;

    public static OrderedCollectionPredicate<byte> Byte_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly long[] LongArray =
    [
        long.MinValue, long.MinValue + 1, long.MinValue + 2, long.MinValue + 2, long.MinValue + 4
      , -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, long.MaxValue - 4, long.MaxValue - 3, long.MaxValue - 2, long.MaxValue - 1, long.MaxValue
    ];
    public static readonly List<long> LongList = [..LongArray];

    public static PalantírReveal<long> LongF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<long> LongF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<long> LongF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<long> Long_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<long> Long_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<long> Long_All   = (_, _) => IncludedAndContinue;

    public static OrderedCollectionPredicate<long> Long_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly ulong[]     ULongArray = [0, 1, 2, 3, 4, 5, ulong.MaxValue - 3, ulong.MaxValue - 2, ulong.MaxValue - 1, ulong.MaxValue];
    public static readonly List<ulong> ULongList  = [..ULongArray];

    public static PalantírReveal<ulong> ULongF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<ulong> ULongF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<ulong> ULongF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<ulong> ULong_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<ulong> ULong_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<ulong> ULong_All   = (_, _) => IncludedAndContinue;

    public static OrderedCollectionPredicate<ulong> ULong_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly decimal[] DecimalArray =
    [
        new(Math.PI), new(Math.E), new(Math.PI * 4), new(Math.E * 4), new(Math.PI * 7), new(Math.E * 7), new(Math.PI * 10), new(Math.E * 10)
      , new(Math.PI * 12), new(Math.E * 12)
    ];
    public static readonly List<decimal> DecimalList = [..DecimalArray];

    public static PalantírReveal<decimal> DecimalF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<decimal> DecimalF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<decimal> DecimalF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<decimal> Decimal_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<decimal> Decimal_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<decimal> Decimal_All   = (_, _) => IncludedAndContinue;

    public static OrderedCollectionPredicate<decimal> Decimal_None = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static readonly BigInteger[] BigIntegerArray =
    [
        new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), new BigInteger(ulong.MaxValue) * new BigInteger(3)
      , new BigInteger(long.MinValue) * new BigInteger(2), new BigInteger(long.MinValue) * new BigInteger(3000)
    ];
    public static readonly List<BigInteger> BigIntegerList = [..BigIntegerArray];

    public static PalantírReveal<BigInteger> BigIntegerF0Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F0");
    public static PalantírReveal<BigInteger> BigIntegerF2Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F2");

    public static PalantírReveal<BigInteger> BigIntegerF4Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValue(showMe, "F4");

    public static OrderedCollectionPredicate<BigInteger> BigInteger_Lt_10 = (_, item) => EvaluateIsIncludedAndContinue(item < 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_Gt_10 = (_, item) => EvaluateIsIncludedAndContinue(item > 10);
    public static OrderedCollectionPredicate<BigInteger> BigInteger_All   = (_, _) => IncludedAndContinue;

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

    public static OrderedCollectionPredicate<Version>  Version_First_5            = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<Version>  Version_Second_5           = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<Version>  Version_First_2            = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<Version>  Version_First_MjrGt_10     = (_, item) => First(item.Major > 10);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_2        = (count, _) => StopOnFirstExclusion(count <= 2);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_5        = (count, _) => EvaluateIsIncludedAndContinue(count <= 5);
    public static OrderedCollectionPredicate<Version?> NullVersion_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<Version?> NullVersion_First_MjrGt_10 = (_, item) => First(item?.Major is > 10);

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

    public static OrderedCollectionPredicate<string> String_LenLt_100      = (_, item) => EvaluateIsIncludedAndContinue(item.Length < 100);
    public static OrderedCollectionPredicate<string> String_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<string> String_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<string> String_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

    public static OrderedCollectionPredicate<string?> NullString_LenLt_100      = (_, item) => EvaluateIsIncludedAndContinue(item?.Length < 100);
    public static OrderedCollectionPredicate<string?> NullString_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<string?> NullString_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<string?> NullString_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

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

    public static OrderedCollectionPredicate<MutableString> MutableString_First_5 = (count, _) =>
        StopOnFirstExclusion(count <= 5);

    public static OrderedCollectionPredicate<MutableString> MutableString_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

    public static OrderedCollectionPredicate<MutableString?> NullMutableString_LenLt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item?.Length < 100);

    public static OrderedCollectionPredicate<MutableString?> NullMutableString_Second_5 = (count, _) =>
        BetweenRetrieveRange(count, 6, 11);

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

    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_LenLt_100
        = (_, item) => EvaluateIsIncludedAndContinue(item.Length < 100);
    public static OrderedCollectionPredicate<CharArrayStringBuilder> CharArrayStringBuilder_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);

    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<CharArrayStringBuilder?> NullCharArrayStringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

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

    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_LenLt_100 = (_, item) =>
        EvaluateIsIncludedAndContinue(item.Length < 100);

    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<StringBuilder> StringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_LenLt_100 = (_, item) =>
        EvaluateIsIncludedAndContinue(item?.Length < 100);

    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_First_5        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<StringBuilder?> NullStringBuilder_Skip_Odd_Index = (_, _) => IncludedAndSkipNext(1);

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
        (showMe, tos) => showMe.RevealState(tos);

    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_First_5 =
        (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Second_5 =
        (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>> StringBearerClassList_Skip_Odd_Index =
        (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStringBearer<decimal>?> NullStringBearerClassList_Skip_Odd_Index
        = (_, _) => IncludedAndSkipNext(1);


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

    public static PalantírReveal<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructListRevealer =
        (showMe, tos) => showMe.RevealState(tos);

    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_First_5 =
        (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Second_5 =
        (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>> StringBearerStructList_Skip_Odd_Index =
        (_, _) => IncludedAndSkipNext(1);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<FieldSpanFormattableAlwaysAddStructStringBearer<decimal>?> NullStringBearerStructList_Skip_Odd_Index
        = (_, _) => IncludedAndSkipNext(1);


    public static readonly object[] ObjArray =
    [
        true
      , (byte)186
      , new MySpanFormattableClass("MySpanFormattableClass")
      , ushort.MaxValue
      , 'c'
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1()  
      , double.MinValue
      , "String"
      , new Uri("https://github.com/shwaindog/Fortitude/")
      , false
      , new MyOtherTypeStruct("MyOtherTypeStruct.ToString()")
      , short.MinValue
      , WithDefaultULongWithFlagsEnum.WDUWFE_First8Mask  
      , double.MaxValue
      , new Version(1, 1, 1, 1)
      , new CharArrayStringBuilder("CharArrayStringBuilder")
      , new MySpanFormattableStruct("MySpanFormattableStruct")
      , ulong.MaxValue
      , float.MaxValue
      , Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D")
      , int.MinValue
      , NoDefaultLongNoFlagsEnum.NDLNFE_3  
      , new MutableString("MutableString")
      , uint.MaxValue
      , new MyOtherTypeClass("MyOtherTypeClass.ToString()")
      , long.MinValue
      , Rune.GetRuneAt("𝄞", 0)
      , decimal.MinValue
      , new StringBuilder("StringBuilder")
    ];
    public static readonly List<object> ObjList = [..ObjArray];

    public static Lazy<List<object?>> NullObjArray = new(() =>
    {
        var withNulls = new List<object?>();
        for (int i = 0; i < DecimalArray.Length; i++)
        {
            var objAtIndex = DecimalArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(objAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(objAtIndex);
                    break;
                default: withNulls.Add(objAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<object> ObjPad10Revealer = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<object> ObjRevealer      = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);

    public static OrderedCollectionPredicate<object> Obj_All_Numbers    = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<object> Obj_All_NonNumbers = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<object> Obj_First_8        = (count, _) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<object> Obj_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<object> Obj_Skip_Odd_Index = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<object> Obj_All            = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<object> Obj_None           = (_, _) => EvaluateIsIncludedAndContinue(false);

    public static OrderedCollectionPredicate<object?> NullObj_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<object?> NullObj_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<object?> NullObj_All_NonNull    = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<object?> NullObj_First_8        = (count, _) => StopOnFirstExclusion(count <= 8);
    public static OrderedCollectionPredicate<object?> NullObj_Second_5       = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<object?> NullObj_Skip_Odd_Index = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<object?> NullObj_All            = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<object?> NullObj_None           = (_, _) => NotIncludedAndContinue;


    // 1. Start No Flags enums
    // 1. a) No Default No Flags
    public static readonly NoDefaultLongNoFlagsEnum[] NoDefaultLongNoFlagsEnumArray =
    [
        NoDefaultLongNoFlagsEnum.NDLNFE_4
      , NoDefaultLongNoFlagsEnum.NDLNFE_34
      , NoDefaultLongNoFlagsEnum.NDLNFE_1.JustUnnamed()
      , NoDefaultLongNoFlagsEnum.NDLNFE_1
      , NoDefaultLongNoFlagsEnum.NDLNFE_1.Default()
      , NoDefaultLongNoFlagsEnum.NDLNFE_13
      , NoDefaultLongNoFlagsEnum.NDLNFE_2
    ];
    public static readonly List<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnumList = [..NoDefaultLongNoFlagsEnumArray];
    public static readonly Lazy<List<NoDefaultLongNoFlagsEnum?>> NullNoDefaultLongNoFlagsEnumList = new(() =>
    {
        var withNulls = new List<NoDefaultLongNoFlagsEnum?>();
        for (int i = 0; i < NoDefaultLongNoFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = NoDefaultLongNoFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum> NoDefaultLongNoFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultLongNoFlagsEnum?> NullNoDefaultLongNoFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    public static readonly NoDefaultULongNoFlagsEnum[] NoDefaultULongNoFlagsEnumArray =
    [
        NoDefaultULongNoFlagsEnum.NDUNFE_4
      , NoDefaultULongNoFlagsEnum.NDUNFE_34
      , NoDefaultULongNoFlagsEnum.NDUNFE_1.JustUnnamed()
      , NoDefaultULongNoFlagsEnum.NDUNFE_1
      , NoDefaultULongNoFlagsEnum.NDUNFE_1.Default()
      , NoDefaultULongNoFlagsEnum.NDUNFE_13
      , NoDefaultULongNoFlagsEnum.NDUNFE_2
    ];
    public static readonly List<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnumList = [..NoDefaultULongNoFlagsEnumArray];
    public static readonly Lazy<List<NoDefaultULongNoFlagsEnum?>> NullNoDefaultULongNoFlagsEnumList = new(() =>
    {
        var withNulls = new List<NoDefaultULongNoFlagsEnum?>();
        for (int i = 0; i < NoDefaultULongNoFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = NoDefaultULongNoFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum> NoDefaultULongNoFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultULongNoFlagsEnum?> NullNoDefaultULongNoFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    // 1. b) With Default No Flags
    public static readonly WithDefaultLongNoFlagsEnum[] WithDefaultLongNoFlagsEnumArray =
    [
        WithDefaultLongNoFlagsEnum.WDLNFE_4
      , WithDefaultLongNoFlagsEnum.WDLNFE_34
      , WithDefaultLongNoFlagsEnum.WDLNFE_1.JustUnnamed()
      , WithDefaultLongNoFlagsEnum.WDLNFE_1
      , WithDefaultLongNoFlagsEnum.WDLNFE_1.Default()
      , WithDefaultLongNoFlagsEnum.WDLNFE_13
      , WithDefaultLongNoFlagsEnum.WDLNFE_2
    ];
    public static readonly List<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnumList = [..WithDefaultLongNoFlagsEnumArray];
    public static readonly Lazy<List<WithDefaultLongNoFlagsEnum?>> NullWithDefaultLongNoFlagsEnumList = new(() =>
    {
        var withNulls = new List<WithDefaultLongNoFlagsEnum?>();
        for (int i = 0; i < WithDefaultLongNoFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = WithDefaultLongNoFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum> WithDefaultLongNoFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultLongNoFlagsEnum?> NullWithDefaultLongNoFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    public static readonly WithDefaultULongNoFlagsEnum[] WithDefaultULongNoFlagsEnumArray =
    [
        WithDefaultULongNoFlagsEnum.WDUNFE_4
      , WithDefaultULongNoFlagsEnum.WDUNFE_34
      , WithDefaultULongNoFlagsEnum.WDUNFE_1.JustUnnamed()
      , WithDefaultULongNoFlagsEnum.WDUNFE_1
      , WithDefaultULongNoFlagsEnum.WDUNFE_1.Default()
      , WithDefaultULongNoFlagsEnum.WDUNFE_13
      , WithDefaultULongNoFlagsEnum.WDUNFE_2
    ];
    public static readonly List<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnumList = [..WithDefaultULongNoFlagsEnumArray];
    public static readonly Lazy<List<WithDefaultULongNoFlagsEnum?>> NullWithDefaultULongNoFlagsEnumList = new(() =>
    {
        var withNulls = new List<WithDefaultULongNoFlagsEnum?>();
        for (int i = 0; i < WithDefaultULongNoFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = WithDefaultULongNoFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum> WithDefaultULongNoFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_First_5
        = (count, _) => StopOnFirstExclusion(count <= 5);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_Second_5
        = (count, _) => BetweenRetrieveRange(count, 6, 11);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultULongNoFlagsEnum?> NullWithDefaultULongNoFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    // 2. Start With Flags enums
    // 1. a) No Default With Flags
    public static readonly NoDefaultLongWithFlagsEnum[] NoDefaultLongWithFlagsEnumArray =
    [
        NoDefaultLongWithFlagsEnum.NDLWFE_4
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag6Mask()
      , NoDefaultLongWithFlagsEnum.NDLWFE_34
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.JustUnnamed()
      , NoDefaultLongWithFlagsEnum.NDLWFE_1
      | NoDefaultLongWithFlagsEnum.NDLWFE_2
      | NoDefaultLongWithFlagsEnum.NDLWFE_6
      | NoDefaultLongWithFlagsEnum.NDLWFE_7
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.Default()
      , NoDefaultLongWithFlagsEnum.NDLWFE_13
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask()
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask()
      , NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2MaskPlusUnnamed()
      , NoDefaultLongWithFlagsEnum.NDLWFE_2
      , NoDefaultLongWithFlagsEnum.NDLWFE_2.First4Mask()
    ];
    public static readonly List<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnumList = [..NoDefaultLongWithFlagsEnumArray];
    public static readonly Lazy<List<NoDefaultLongWithFlagsEnum?>> NullNoDefaultLongWithFlagsEnumList = new(() =>
    {
        var withNulls = new List<NoDefaultLongWithFlagsEnum?>();
        for (int i = 0; i < NoDefaultLongWithFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = NoDefaultLongWithFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum> NoDefaultLongWithFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultLongWithFlagsEnum?> NullNoDefaultLongWithFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    public static readonly NoDefaultULongWithFlagsEnum[] NoDefaultULongWithFlagsEnumArray =
    [
        NoDefaultULongWithFlagsEnum.NDUWFE_4
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag6Mask()
      , NoDefaultULongWithFlagsEnum.NDUWFE_34
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.JustUnnamed()
      , NoDefaultULongWithFlagsEnum.NDUWFE_1
      | NoDefaultULongWithFlagsEnum.NDUWFE_2
      | NoDefaultULongWithFlagsEnum.NDUWFE_6
      | NoDefaultULongWithFlagsEnum.NDUWFE_7
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.Default()
      , NoDefaultULongWithFlagsEnum.NDUWFE_13
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag2Mask()
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask()
      , NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2MaskPlusUnnamed()
      , NoDefaultULongWithFlagsEnum.NDUWFE_2
      , NoDefaultULongWithFlagsEnum.NDUWFE_2.First4Mask()
    ];
    public static readonly List<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnumList = [..NoDefaultULongWithFlagsEnumArray];
    public static readonly Lazy<List<NoDefaultULongWithFlagsEnum?>> NullNoDefaultULongWithFlagsEnumList = new(() =>
    {
        var withNulls = new List<NoDefaultULongWithFlagsEnum?>();
        for (int i = 0; i < NoDefaultULongWithFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = NoDefaultULongWithFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum> NoDefaultULongWithFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<NoDefaultULongWithFlagsEnum?> NullNoDefaultULongWithFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    public static readonly WithDefaultLongWithFlagsEnum[] WithDefaultLongWithFlagsEnumArray =
    [
        WithDefaultLongWithFlagsEnum.WDLWFE_4
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag6Mask()
      , WithDefaultLongWithFlagsEnum.WDLWFE_34
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.JustUnnamed()
      , WithDefaultLongWithFlagsEnum.WDLWFE_1
      | WithDefaultLongWithFlagsEnum.WDLWFE_2
      | WithDefaultLongWithFlagsEnum.WDLWFE_6
      | WithDefaultLongWithFlagsEnum.WDLWFE_7
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.Default()
      , WithDefaultLongWithFlagsEnum.WDLWFE_13
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask()
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask()
      , WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2MaskPlusUnnamed()
      , WithDefaultLongWithFlagsEnum.WDLWFE_2
      , WithDefaultLongWithFlagsEnum.WDLWFE_2.First4Mask()
    ];
    public static readonly List<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnumList = [..WithDefaultLongWithFlagsEnumArray];
    public static readonly Lazy<List<WithDefaultLongWithFlagsEnum?>> NullWithDefaultLongWithFlagsEnumList = new(() =>
    {
        var withNulls = new List<WithDefaultLongWithFlagsEnum?>();
        for (int i = 0; i < WithDefaultLongWithFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = WithDefaultLongWithFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum> WithDefaultLongWithFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_All  = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultLongWithFlagsEnum?> NullWithDefaultLongWithFlagsEnum_None = (_, _) => NotIncludedAndContinue;


    public static readonly WithDefaultULongWithFlagsEnum[] WithDefaultULongWithFlagsEnumArray =
    [
        WithDefaultULongWithFlagsEnum.WDUWFE_4
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag6Mask()
      , WithDefaultULongWithFlagsEnum.WDUWFE_34
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.JustUnnamed()
      , WithDefaultULongWithFlagsEnum.WDUWFE_1
      | WithDefaultULongWithFlagsEnum.WDUWFE_2
      | WithDefaultULongWithFlagsEnum.WDUWFE_6
      | WithDefaultULongWithFlagsEnum.WDUWFE_7
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.Default()
      , WithDefaultULongWithFlagsEnum.WDUWFE_13
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask()
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask()
      , WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2MaskPlusUnnamed()
      , WithDefaultULongWithFlagsEnum.WDUWFE_2
      , WithDefaultULongWithFlagsEnum.WDUWFE_2.First4Mask()
    ];
    public static readonly List<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnumList = [..WithDefaultULongWithFlagsEnumArray];
    public static readonly Lazy<List<WithDefaultULongWithFlagsEnum?>> NullWithDefaultULongWithFlagsEnumList = new(() =>
    {
        var withNulls = new List<WithDefaultULongWithFlagsEnum?>();
        for (int i = 0; i < WithDefaultULongWithFlagsEnumArray.Length; i++)
        {
            var enumAtIndex = WithDefaultULongWithFlagsEnumArray[i];
            switch (i)
            {
                case 0:
                case 8:
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                case 3:
                    withNulls.Add(null);
                    withNulls.Add(null);
                    withNulls.Add(enumAtIndex);
                    break;
                default: withNulls.Add(enumAtIndex); break;
            }
        }
        withNulls.Add(null);
        return withNulls;
    });

    public static PalantírReveal<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnumPad10Revealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe, "\"{0,10}\"");
    public static PalantírReveal<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnumRevealer
        = (showMe, tos) => tos.StartSimpleContentType(showMe).AsValueMatch(showMe);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!item.GetType().IsNumericType());
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum> WithDefaultULongWithFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_All_Numbers
        = (_, item) => EvaluateIsIncludedAndContinue(item?.GetType().IsNumericType() ?? false);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_All_NonNumbers
        = (_, item) => EvaluateIsIncludedAndContinue(!(item?.GetType().IsNumericType() ?? false));
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_All_NonNull
        = (_, item) => EvaluateIsIncludedAndContinue(item != null);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_First_3
        = (count, _) => StopOnFirstExclusion(count <= 3);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_Second_3
        = (count, _) => BetweenRetrieveRange(count, 4, 7);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_Skip_Odd_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count - 1) % 2) == 0, 1);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_Skip_Even_Index
        = (count, _) => EvaluateIsIncludedAndContinue(((count) % 2) == 0, count == 1 ? 0 : 1);
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?> NullWithDefaultULongWithFlagsEnum_All = (_, _) => IncludedAndContinue;
    public static OrderedCollectionPredicate<WithDefaultULongWithFlagsEnum?>
        NullWithDefaultULongWithFlagsEnum_None = (_, _) => NotIncludedAndContinue;
}
