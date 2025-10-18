// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class CloakedBearerTestData
{
    public static readonly IFormatExpectation[] AllCloakedBearerExpectations =
    [
        // byte
        new CloakedBearerExpect<byte>(0, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" }
        }
      , new CloakedBearerExpect<byte>(255, typeof(FieldSpanFormattableAlwaysAddStringBearer<>))
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new CloakedBearerExpect<byte>(128, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "C2")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" } }
      , new CloakedBearerExpect<byte>(77, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "\"{0,-20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"77                  \"" }
        }
      , new CloakedBearerExpect<byte>(32, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "", true, 32)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "32" }
        }
      , new CloakedBearerExpect<byte>(255, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "{0[..1]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" } }
      , new CloakedBearerExpect<byte>(255, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "{0[1..2]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" } }
      , new CloakedBearerExpect<byte>(255, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }

        // byte?

        // byte?
      , new CloakedBearerExpect<byte?>(0, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "", true)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" }
        }
      , new CloakedBearerExpect<byte?>(0, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "{0}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0" } }
      , new CloakedBearerExpect<byte?>(null, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "null", true)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" }
        }
      , new CloakedBearerExpect<byte?>(255, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>))
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new CloakedBearerExpect<byte?>(128, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "C2")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" }
        }
      , new CloakedBearerExpect<byte?>(144, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "\"{0,20}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                 144\"" }
        }
      , new CloakedBearerExpect<byte?>(64, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "", true, 64)
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "64" }
        }
      , new CloakedBearerExpect<byte?>(255, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "{0[..1]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" }
        }
      , new CloakedBearerExpect<byte?>(255, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "{0[1..2]}")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" }
        }
      , new CloakedBearerExpect<byte?>(255, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "{0[1..]}")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }

        // DateTime
      , new CloakedBearerExpect<DateTime>(DateTime.MinValue, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "O")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new CloakedBearerExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new CloakedBearerExpect<DateTime>(new DateTime(2020, 2, 2).AddTicks(2222222)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "s", true
                                        , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new CloakedBearerExpect<DateTime>(DateTime.MaxValue, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "'{0:u}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new CloakedBearerExpect<DateTime>(DateTime.MinValue, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "\"{0,30:u}\"", true
                                        , new DateTime(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new CloakedBearerExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new CloakedBearerExpect<DateTime>(new DateTime(2009, 11, 12, 19, 49, 0)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12T19:49:00.0000000   \""
            }
        }

        // DateTime?
      , new CloakedBearerExpect<DateTime?>(DateTime.MinValue, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "O")
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new CloakedBearerExpect<DateTime?>(null, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "null", true)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new CloakedBearerExpect<DateTime?>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "o")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new CloakedBearerExpect<DateTime?>(new DateTime(2020, 2, 2).AddTicks(2222222)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "s", true
                                         , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new CloakedBearerExpect<DateTime?>(DateTime.MaxValue, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "'{0:u}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new CloakedBearerExpect<DateTime?>(DateTime.MinValue, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "\"{0,30:u}\"", true
                                         , new DateTime(2020, 1, 1))
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new CloakedBearerExpect<DateTime?>(new DateTime(1980, 7, 31, 11, 48, 13)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new CloakedBearerExpect<DateTime?>(new DateTime(2009, 11, 12, 19, 49, 0)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "\"{0,-30:O}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"2009-11-12T19:49:00.0000000   \""
            }
        }

        // TimeSpan
      , new CloakedBearerExpect<TimeSpan>(TimeSpan.Zero, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "g")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" }
        }
      , new CloakedBearerExpect<TimeSpan>(new TimeSpan(1, 1, 1, 1, 111, 111)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "c")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new CloakedBearerExpect<TimeSpan>(new TimeSpan(-2, -22, -22, -22, -222, -222)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "G", true
                                        , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new CloakedBearerExpect<TimeSpan>
            (TimeSpan.MaxValue, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "'{0:G}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'10675199:02:48:05.4775807'"
                }
            }
      , new CloakedBearerExpect<TimeSpan>
            (TimeSpan.MinValue
           , typeof(FieldSpanFormattableAlwaysAddStringBearer<>)
           , "\"{0,30:c}\"", true, TimeSpan.Zero)
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new CloakedBearerExpect<TimeSpan>
            (new TimeSpan(3, 3, 33, 33, 333, 333)
           , typeof(FieldSpanFormattableAlwaysAddStringBearer<>),
             "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "'03-03-33-33.333'"
                }
            }
      , new CloakedBearerExpect<TimeSpan>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                        , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "\"{0,-30:G}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"-4:04:44:44.4444440           \""
            }
        }

        // TimeSpan?
      , new CloakedBearerExpect<TimeSpan?>(TimeSpan.Zero, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>)
                                         , "g") { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "0:00:00" } }
      , new CloakedBearerExpect<TimeSpan?>(null, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "null", true)
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites, "null" } }
      , new CloakedBearerExpect<TimeSpan?>(new TimeSpan(1, 1, 1, 1, 111, 111)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "c")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new CloakedBearerExpect<TimeSpan?>(new TimeSpan(-2, -22, -22, -22, -222, -222)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "G", true
                                         , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new CloakedBearerExpect<TimeSpan?>(TimeSpan.MaxValue, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>)
                                         , "'{0:G}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'10675199:02:48:05.4775807'" } }
      , new CloakedBearerExpect<TimeSpan?>(TimeSpan.MinValue, typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>)
                                         , "\"{0,30:c}\"", true, TimeSpan.Zero)
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"    -10675199.02:48:05.4775808\""
            }
        }
      , new CloakedBearerExpect<TimeSpan?>(new TimeSpan(3, 3, 33, 33, 333, 333)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>),
                                           "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
        }
      , new CloakedBearerExpect<TimeSpan?>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                         , typeof(FieldNullableSpanFormattableAlwaysAddStringBearer<>), "\"{0,-30:G}\"")
        {
            {
                AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"-4:04:44:44.4444440           \""
            }
        }

        //  IPAddress and IPAddress?
      , new CloakedBearerExpect<IPAddress>(new IPAddress("\0\0\0\0"u8), typeof(FieldSpanFormattableAlwaysAddStringBearer<>))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0.0.0.0" }
        }
      , new CloakedBearerExpect<IPAddress>(null, typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "null", true)
            { { AcceptsSpanFormattable | AlwaysWrites, "null" } }
      , new CloakedBearerExpect<IPAddress>(IPAddress.Loopback, typeof(FieldSpanFormattableAlwaysAddStringBearer<>))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1" }
        }
      , new CloakedBearerExpect<IPAddress>(new IPAddress([192, 168, 0, 1])
                                         , typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "'{0}'", true, new IPAddress([192, 168, 0, 1]))
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, "'192.168.0.1'" }
        }
      , new CloakedBearerExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "'{0}'")
            { { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254'" } }
      , new CloakedBearerExpect<IPAddress>(IPAddress.Parse("255.255.0.0"), typeof(FieldSpanFormattableAlwaysAddStringBearer<>), "\"{0,17}\"")
        {
            { AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"      255.255.0.0\"" }
        }

      , new CloakedBearerExpect<string>("It began with the forging of the Great Strings."
                                      , typeof(FieldStringAlwaysAddStringBearer), "[{0}]")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]"
            }
        }
    ];
}
