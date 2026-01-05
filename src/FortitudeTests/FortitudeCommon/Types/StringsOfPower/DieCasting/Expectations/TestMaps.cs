// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Logging.Config.ExampleConfig.FLogConfigExamples;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

public class TestMaps
{
        
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
        { AsyncDailyDblBufferedFileExample, AsyncDailyDblBufferedFileExample.LoadExampleToAppConfig() }
      , { SyncDailyFileExample, SyncDailyFileExample.LoadExampleToAppConfig() }
      , { SyncFileAndColoredConsoleExample, SyncFileAndColoredConsoleExample.LoadExampleToAppConfig() }
      , { SyncColoredConsoleExample, SyncColoredConsoleExample.LoadExampleToAppConfig() }
       ,
        {
            AsyncDblBufferedColoredConsoleExample
          , AsyncDblBufferedColoredConsoleExample.LoadExampleToAppConfig()
        }
      , { AsyncDblBufferedFileAndColoredConsoleExample, null }
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
            AsyncDblBufferedFileAndColoredConsoleExample
          , AsyncDblBufferedFileAndColoredConsoleExample.LoadExampleToAppConfig()
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
      , { new CharArrayStringBuilder("SecondKey"), SyncDailyFileExample.LoadExampleAsJsonObject() }
      , { new CharArrayStringBuilder(""), new JsonObject() }
      , { new CharArrayStringBuilder("FourthKey"), SyncFileAndColoredConsoleExample.LoadExampleAsJsonObject() }
      , { new CharArrayStringBuilder("FifthKey"), null }
    };
}
