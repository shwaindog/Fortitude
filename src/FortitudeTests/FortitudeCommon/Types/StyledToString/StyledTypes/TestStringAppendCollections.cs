// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeTests.FortitudeCommon.Types.StyledToString.StyledTypes;

[NoMatchingProductionClass]
public class TestStringAppendCollections
{
    public static readonly float[]     FloatArray = [(float)Math.PI, (float)Math.E, (float)Math.PI * 2, (float)Math.E * 2];
    public static readonly List<float> FloatList  = [..FloatArray];

    public static readonly sbyte[]     SByteArray = [-128, -127, -126, -125, -124, 0, 1, 2, 3, 4, 5, 6, 7, 8, 120, 121, 122, 123, 124, 125, 126, 127];
    public static readonly List<sbyte> SByteList  = [..SByteArray];

    public static readonly byte[]     ByteArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255];
    public static readonly List<byte> ByteList  = [..ByteArray];

    public static readonly long[] LongArray =
    [
        long.MinValue, long.MinValue + 1, long.MinValue + 2, long.MinValue + 2, long.MinValue + 4
      , -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, long.MaxValue - 4, long.MaxValue - 3, long.MaxValue - 2, long.MaxValue - 1, long.MaxValue
    ];
    public static readonly List<long> LongList = [..LongArray];

    public static readonly ulong[]     ULongArray = [0, 1, 2, 3, 4, 5, ulong.MaxValue - 3, ulong.MaxValue - 2, ulong.MaxValue - 1, ulong.MaxValue];
    public static readonly List<ulong> ULongList  = [..ULongArray];

    public static readonly decimal[] DecimalArray =
    [
        new(Math.PI), new(Math.E), new(Math.PI * 2), new(Math.E * 2), new(Math.PI * 3), new(Math.E * 3), new(Math.PI * 4), new(Math.E * 4)
      , new(Math.PI * 5), new(Math.E * 5), new(Math.PI * 6), new(Math.E * 6), new(Math.PI * 7), new(Math.E * 7), new(Math.PI * 8), new(Math.E * 8)
      , new(Math.PI * 9), new(Math.E * 9), new(Math.PI * 10), new(Math.E * 10), new(Math.PI * 11), new(Math.E * 11), new(Math.PI * 12)
      , new(Math.E * 12)
    ];
    public static readonly List<decimal> DecimalList = [..DecimalArray];

    public static readonly BigInteger[] BigIntegerArray =
    [
        new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), new BigInteger(ulong.MaxValue) * new BigInteger(3)
      , new BigInteger(long.MinValue) * new BigInteger(2), new BigInteger(long.MinValue) * new BigInteger(3000)
    ];
    public static readonly List<BigInteger> BigIntegerList = [..BigIntegerArray];

    public static readonly TimeSpanConfig[] StyledToStringArray =
    [
        new(23, 42, 21, 22, 7), new(921, 38, 9, 1, 6)
      , new(701, 52, 8, 15, 5), new(123, 3, 23, 7, 4)
      , new(259, 16, 42, 23, 3), new(20, 38, 9, 10, 2)
      , new(873, 41, 20, 3, 1), new(0)
    ];
    public static readonly List<TimeSpanConfig> StyledToStringList = [..StyledToStringArray];

    public static readonly Dictionary<int, double> IntToDoubleMap = new()
    {
        { 0, Math.PI }, { 1, Math.E }, { 2, -Math.PI * 100.0 }, { 3, Math.E * 10_000.0 * Math.PI }, { 4, Math.PI * 97.0 / Math.E }
      , { 5, Math.E * Math.PI }, { 6, Math.PI * 1_000_000.0 + Math.E * 100 }, { 7, Math.PI / 999_983.0 }, { 8, Math.E / 999_983.0 }, { 10, 0 }
      , { 11, -1.0 }
    };
    public static readonly KeyValuePair<int, double>[]     IntToDoubleMapArray = IntToDoubleMap.ToArray();
    public static readonly List<KeyValuePair<int, double>> IntToIntDoubleList  = IntToDoubleMap.ToList();

    public static readonly Dictionary<TimeSpan, decimal[]?> TimeSpanToDecimalArrayMap = new()
    {
        { TimeSpan.FromHours(9), [new decimal(Math.PI), new decimal(Math.E)] }
      , { TimeSpan.FromTicks(424_242_424_242_4), [new decimal(Math.E * 2), new decimal(Math.PI * 3)] }
      , { TimeSpan.FromTicks(2_424_242_424_242_4), [new decimal(Math.E / 999_983.0), new decimal(Math.PI * 1_000_000.0 + Math.E * 100)] }
      , { TimeSpan.Zero, [new decimal(Math.E * 10_000.0 * Math.PI), new decimal(long.MinValue) + new decimal(Math.PI)] }, { TimeSpan.FromDays(1), [] }
      , { TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(42)), null }, { TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(42)), [decimal.Zero] }
    };
    public static readonly KeyValuePair<TimeSpan, decimal[]?>[]     TimeSpanToDecimalArrayMapArray = TimeSpanToDecimalArrayMap.ToArray();
    public static readonly List<KeyValuePair<TimeSpan, decimal[]?>> TimeSpanToDecimalArrayMapList  = TimeSpanToDecimalArrayMap.ToList();

    public static readonly Dictionary<BigInteger, List<decimal?>?> BigIntToNullableDecimalListMap = new()
    {
        { new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), [new decimal(Math.PI), null, new decimal(Math.E)] }
      , { new BigInteger(long.MinValue) * new BigInteger(3000), [null, new decimal(Math.E * 2), new decimal(Math.PI * 3)] }
      , { BigInteger.Zero, [new decimal(Math.E / 999_983.0), new decimal(Math.PI * 1_000_000.0 + Math.E * 100), null] }
       ,
        {
            new BigInteger(ulong.MaxValue) * new BigInteger(3)
          , [new decimal(Math.E * 10_000.0 * Math.PI), new decimal(long.MinValue) + new decimal(Math.PI)]
        }
      , { new BigInteger(1), [] }, { BigInteger.MinusOne, null }
      , { new BigInteger(Math.PI * 1000_000) + new BigInteger(Math.E * 983.0), [decimal.Zero] }
    };
    public static readonly KeyValuePair<BigInteger, List<decimal?>?>[] BigIntToNullableDecimalListMapArray = BigIntToNullableDecimalListMap.ToArray();
    public static readonly List<KeyValuePair<BigInteger, List<decimal?>?>> BigIntToNullableDecimalListMapList
        = BigIntToNullableDecimalListMap.ToList();

    public static readonly Dictionary<DayOfWeek, Dictionary<string, TimeSpan>?> EnumToMapOfStringToTimeSpanMap = new()
    {
        { DayOfWeek.Sunday, new Dictionary<string, TimeSpan> { { "Opening", TimeSpan.FromHours(11) }, { "Closing", TimeSpan.FromHours(15) } } }
      , { DayOfWeek.Monday, new Dictionary<string, TimeSpan> { { "Opening", TimeSpan.FromHours(9) }, { "Closing", TimeSpan.FromHours(17) } } }
      , { DayOfWeek.Tuesday, new Dictionary<string, TimeSpan> { { "Opening", TimeSpan.FromHours(8) }, { "Closing", TimeSpan.FromHours(16) } } }
      , { DayOfWeek.Wednesday, new Dictionary<string, TimeSpan> { { "Opening", TimeSpan.FromHours(7) }, { "Closing", TimeSpan.FromHours(13) } } }
      , { DayOfWeek.Thursday, new Dictionary<string, TimeSpan> { { "Opening", TimeSpan.FromHours(10) }, { "Closing", TimeSpan.FromHours(18) } } }
      , { DayOfWeek.Friday, new Dictionary<string, TimeSpan>() }
      , { DayOfWeek.Saturday, null }
    };
    public static readonly KeyValuePair<DayOfWeek, Dictionary<string, TimeSpan>?>[] EnumToMapOfStringToTimeSpanMapArray
        = EnumToMapOfStringToTimeSpanMap.ToArray();
    public static readonly List<KeyValuePair<DayOfWeek, Dictionary<string, TimeSpan>?>> EnumToMapOfStringToTimeSpanMapList
        = EnumToMapOfStringToTimeSpanMap.ToList();

    public static readonly Dictionary<FLogExampleConfig, FLogAppConfig?> StructToStyledToStringObjMap = new()
    {
        { FLogConfigExamples.AsyncDailyDblBufferedFileExample, FLogConfigExamples.AsyncDailyDblBufferedFileExample.LoadExampleToAppConfig() }
      , { FLogConfigExamples.SyncDailyFileExample, FLogConfigExamples.SyncDailyFileExample.LoadExampleToAppConfig() }
      , { FLogConfigExamples.SyncFileAndColoredConsoleExample, FLogConfigExamples.SyncFileAndColoredConsoleExample.LoadExampleToAppConfig() }
      , { FLogConfigExamples.SyncColoredConsoleExample, FLogConfigExamples.SyncColoredConsoleExample.LoadExampleToAppConfig() }
       ,
        {
            FLogConfigExamples.AsyncDblBufferedColoredConsoleExample
          , FLogConfigExamples.AsyncDblBufferedColoredConsoleExample.LoadExampleToAppConfig()
        }
      , { FLogConfigExamples.AsyncDblBufferedFileAndColoredConsoleExample, null }
    };
    public static readonly KeyValuePair<FLogExampleConfig, FLogAppConfig?>[] StructToStyledToStringObjMapArray
        = StructToStyledToStringObjMap.ToArray();
    public static readonly List<KeyValuePair<FLogExampleConfig, FLogAppConfig?>> StructToStyledToStringObjMapList
        = StructToStyledToStringObjMap.ToList();

    public static readonly Dictionary<object, object?> ObjToObjMap = new()
    {
        { 1, new DirectoryInfo(".") }
      , { "StringKey", ulong.MaxValue }
      , { TimeSpan.FromHours(12), null }
      , { new DateTime(2025, 8, 29), new Rectangle(new Point(30, 100), new Size(100, 100)) }
      , { long.MinValue, new object() }
       ,
        {
            FLogConfigExamples.AsyncDblBufferedFileAndColoredConsoleExample
          , FLogConfigExamples.AsyncDblBufferedFileAndColoredConsoleExample.LoadExampleToAppConfig()
        }
      , { new object(), new StringBuilder().Append("StringBuilderValue") }
      , { new BigInteger(ulong.MaxValue) + new BigInteger(ulong.MaxValue), new StringBuilder().Append("StringBuilderValue") }
      , { new MutableString("MutableStringKey"), new decimal(Math.PI) * 10_000 + new decimal(Math.E) * 1_000 }
    };
    public static readonly KeyValuePair<object, object?>[]     ObjToObjMapArray = ObjToObjMap.ToArray();
    public static readonly List<KeyValuePair<object, object?>> ObjToObjMapList  = ObjToObjMap.ToList();

    public static readonly Dictionary<ICharSequence, JsonObject?> CharSeqToJsonObjMap = new()
    {
        { new CharArrayStringBuilder("FirstKey"), (JsonObject)JsonNode.Parse("""{ "FirstJsonObjectKey": 42.424242  }""")! }
      , { new CharArrayStringBuilder("SecondKey"), FLogConfigExamples.SyncDailyFileExample.LoadExampleAsJsonObject() }
      , { new CharArrayStringBuilder(""), new JsonObject() }
      , { new CharArrayStringBuilder("FourthKey"), FLogConfigExamples.SyncFileAndColoredConsoleExample.LoadExampleAsJsonObject() }
      , { new CharArrayStringBuilder("FifthKey"), null }
    };
}
