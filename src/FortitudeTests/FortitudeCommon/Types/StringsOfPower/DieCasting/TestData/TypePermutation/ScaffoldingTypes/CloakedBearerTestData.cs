﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class CloakedBearerTestData
{
    public static readonly IFormatExpectation[] AllCloakedBearerExpectations =
    [
        // byte
        new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStringBearer<byte>>(0, "")
        {
            { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStructStringBearer<byte>>(255)
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "255"
            }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStringBearer<byte>>(128, "C2")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "$128.00"
            }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStructStringBearer<byte>>(77, "\"{0,20}\"")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                  77\""
            }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStringBearer<byte>>(32, "", true, 32)
        {
            { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "32" }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStructStringBearer<byte>>(255, "{0[..1]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "2"
            }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStringBearer<byte>>(255, "{0[1..2]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "5"
            }
        }
      , new CloakedBearerExpect<byte, FieldSpanFormattableAlwaysAddStructStringBearer<byte>>(255, "{0[1..]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "55"
            }
        }

        // byte?
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (0, "", true)
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (0, "{0}")
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" } }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>
            (null, "null", true)
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" }
            }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>
            (null, "null", true)
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" }
            }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>(255)
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "255"
            }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(128, "C2")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "$128.00"
            }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>(144, "\"{0,20}\"")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                 144\""
            }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(64, "", true, 64)
        {
            { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "64" }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>(255, "{0[..1]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "2"
            }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStringBearer<byte>>(255, "{0[1..2]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "5"
            }
        }
      , new CloakedBearerExpect<byte?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<byte>>(255, "{0[1..]}")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "55"
            }
        }

        // DateTime
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStringBearer<DateTime>>(DateTime.MinValue, "O")
        {
            { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01T00:00:00.0000000" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , "\"0001-01-01T00:00:00.0000000\""
            }
        }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new DateTime(2020, 2, 2).AddTicks(2222222), "s", true
           , new DateTime(2020, 2, 2).AddTicks(2222222))
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02T00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02T00:00:00\"" }
            }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>(DateTime.MaxValue
       , "'{0:u}'")
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'9999-12-31 23:59:59Z'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites,
                       Json | Compact | Pretty)
              , "\"'9999-12-31 23:59:59Z'\""
            }
        }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (DateTime.MinValue, "\"{0,30:u}\"", true
           , new DateTime(2020, 1, 1))
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log |  Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json |  Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new CloakedBearerExpect<DateTime, FieldSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

        // DateTime?
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (DateTime.MinValue, "O")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "0001-01-01T00:00:00.0000000" 
                }
                ,
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\"" 
                }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (null, "null", true)
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" } }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (null, "null", true)
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" } }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new DateTime(2020, 2, 2).AddTicks(2222222), "s", true
           , new DateTime(2020, 2, 2).AddTicks(2222222))
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02T00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02T00:00:00\"" }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (DateTime.MaxValue, "'{0:u}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "'9999-12-31 23:59:59Z'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStringBearer<DateTime>>
            (new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites,
                           Log |  Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites,
                           Json |  Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
      , new CloakedBearerExpect<DateTime?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<DateTime>>
            (new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

        // TimeSpan
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (TimeSpan.Zero, "g")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0:00:00" }
                , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0:00:00\"" }
            }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "1.01:01:01.1111110"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
           , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "-2:22:22:22.2222220" } 
            , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"-2:22:22:22.2222220\"" } }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (TimeSpan.MaxValue, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "'10675199:02:48:05.4775807'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new TimeSpan(3, 3, 33, 33, 333, 333), "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "'03-03-33-33.333'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new CloakedBearerExpect<TimeSpan, FieldSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new TimeSpan(-4, -4, -44, -44, -444, -444), "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

        // TimeSpan?
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (TimeSpan.Zero, "g")
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0:00:00" }
                , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0:00:00\"" }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (null, "null", true)
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (null, "null", true)
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites), "null" }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "1.01:01:01.1111110"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
           , new TimeSpan(-2, -22, -22, -22, -222, -222))
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Log | Compact | Pretty), "-2:22:22:22.2222220" }
                , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                           , Json | Compact | Pretty), "\"-2:22:22:22.2222220\"" }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (TimeSpan.MaxValue, "'{0:G}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "'10675199:02:48:05.4775807'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStringBearer<TimeSpan>>
            (new TimeSpan(3, 3, 33, 33, 333, 333)
           , "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log | Compact | Pretty)
                  , "'03-03-33-33.333'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
      , new CloakedBearerExpect<TimeSpan?, FieldNullableSpanFormattableAlwaysAddStructStringBearer<TimeSpan>>
            (new TimeSpan(-4, -4, -44, -44, -444, -444), "\"{0,-30:G}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

        //  IPAddress and IPAddress?
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>(new IPAddress("\0\0\0\0"u8))
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                       , Log | Compact | Pretty)
              , "0.0.0.0"
            }
            ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                       , Json | Compact | Pretty)
              , "\"0.0.0.0\""
            }
        }
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>(null, "null", true)
            { { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" } }
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>(IPAddress.Loopback)
        {
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                       , Log | Compact | Pretty)
              , "127.0.0.1"
            }
            ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                       , Json | Compact | Pretty)
              , "\"127.0.0.1\""
            }
        }
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (new IPAddress([192, 168, 0, 1]), "'{0}'", true, new IPAddress([192, 168, 0, 1]))
            {
                { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "'192.168.0.1'" }
               , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'192.168.0.1'\"" }
            }
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStringBearer<IPAddress>>
            (IPAddress.Parse("255.255.255.254"), "'{0}'")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           ,  Log | Compact | Pretty)
                  , "'255.255.255.254'"
                }
                ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           ,  Json | Compact | Pretty)
                  , "\"'255.255.255.254'\""
                }
            }
      , new CloakedBearerExpect<IPAddress, FieldSpanFormattableAlwaysAddStructStringBearer<IPAddress>>
            (IPAddress.Parse("255.255.0.0"), "\"{0,17}\"")
            {
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"      255.255.0.0\""
                }
            }

      , new CloakedBearerExpect<string, FieldStringAlwaysAddStringBearer>
            ("It began with the forging of the Great Strings.", "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Log  | Compact | Pretty)
                  , "[It began with the forging of the Great Strings.]"
                }
                ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                           , Json  | Compact | Pretty)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }

      , new CloakedBearerExpect<string, FieldStringAlwaysAddStructStringBearer>
            ("It began with the forging of the Great Strings.", "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log  | Compact | Pretty)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json  | Compact | Pretty)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
    ];
}
