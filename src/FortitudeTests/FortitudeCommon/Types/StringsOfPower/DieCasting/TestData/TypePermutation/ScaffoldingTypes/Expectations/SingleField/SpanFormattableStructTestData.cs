// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Text;
using FortitudeCommon.DataStructures.Lists.PositionAware;

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class SpanFormattableStructTestData
{
    
    private static PositionUpdatingList<ISingleFieldExpectation>? spanFormattableStructExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> SpanFormattableStructExpectations => spanFormattableStructExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(SpanFormattableStructTestData))
        {

            // DateTime
            new FieldExpect<DateTime>(DateTime.MinValue, "O")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "0001-01-01T00:00:00.0000000"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01T00:00:00.0000000\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                         , Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
          , new FieldExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01T01:01:01.1111111\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
          , new FieldExpect<DateTime>(new DateTime(2020, 2, 2)
                                          .AddTicks(2222222), "s", true
                                    , new DateTime(2020, 2, 2).AddTicks(2222222))
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "2020-02-02T00:00:00"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02T00:00:00\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02T00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02T00:00:00\"" }
            }
          , new FieldExpect<DateTime>(DateTime.MaxValue, "'{0:u}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'9999-12-31 23:59:59Z'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31 23:59:59Z'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
          , new FieldExpect<DateTime>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"          0001-01-01 00:00:00Z\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          0001-01-01 00:00:00Z\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022          0001-01-01 00:00:00Z\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
          , new FieldExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1980-07-31 11:48:13'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
          , new FieldExpect<DateTime>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"2009-11-12T19:49:00.0000000   \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"2009-11-12T19:49:00.0000000   \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u00222009-11-12T19:49:00.0000000   \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

            // DateTime?
          , new FieldExpect<DateTime?>(DateTime.MinValue, "O")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "0001-01-01T00:00:00.0000000"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01T00:00:00.0000000" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"0001-01-01T00:00:00.0000000\""
                }
            }
          , new FieldExpect<DateTime?>(null, "yyyy-MM-ddTHH:mm:ss", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty), "0001-01-01T00:00:00"
                }
              , { new EK(SimpleType | CallsViaMatch), "\"0001-01-01T00:00:00\"" }
             , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "0001-01-01T00:00:00"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue ) , "\"0001-01-01T00:00:00\""
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero 
                        |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue, Log | Compact | Pretty) , "0001-01-01T00:00:00"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero |  DefaultBecomesFallbackString 
                       | DefaultBecomesFallbackValue), "\"0001-01-01T00:00:00\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<DateTime?>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01T01:01:01.1111111\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "2000-01-01T01:01:01.1111111"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01T01:01:01.1111111\""
                }
            }
          , new FieldExpect<DateTime?>
                (new DateTime(2020, 2, 2).AddTicks(2222222), "s", true
               , new DateTime(2020, 2, 2).AddTicks(2222222))
                {
                    {
                        new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "2020-02-02T00:00:00"
                    }
                  , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02T00:00:00\"" }
                   ,
                    {
                        new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                      , "2020-02-02T00:00:00"
                    }
                   ,
                    {
                        new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                      , "\"2020-02-02T00:00:00\""
                    }
                }
          , new FieldExpect<DateTime?>(DateTime.MaxValue, "'{0:u}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'9999-12-31 23:59:59Z'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31 23:59:59Z'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'9999-12-31 23:59:59Z'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31 23:59:59Z'\""
                }
            }
          , new FieldExpect<DateTime?>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"          0001-01-01 00:00:00Z\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          0001-01-01 00:00:00Z\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u0022          0001-01-01 00:00:00Z\u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"          0001-01-01 00:00:00Z\""
                }
            }
          , new FieldExpect<DateTime?>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1980-07-31 11:48:13'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'1980-07-31 11:48:13'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'1980-07-31 11:48:13'\""
                }
            }
          , new FieldExpect<DateTime?>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"2009-11-12T19:49:00.0000000   \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"2009-11-12T19:49:00.0000000   \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u00222009-11-12T19:49:00.0000000   \u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12T19:49:00.0000000   \""
                }
            }

            // TimeSpan
          , new FieldExpect<TimeSpan>(TimeSpan.Zero, "!nV^1LD")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "00:00:00" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"00:00:00\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"00:00:00\"" }
            }
          , new FieldExpect<TimeSpan>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "1.01:01:01.1111110" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"1.01:01:01.1111110\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
          , new FieldExpect<TimeSpan>
                (new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
               , new TimeSpan(-2, -22, -22, -22, -222, -222))
                {
                    {
                        new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "-2:22:22:22.2222220"
                    }
                  , { new EK(SimpleType | AcceptsSpanFormattable), "\"-2:22:22:22.2222220\"" }
                  , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "-2:22:22:22.2222220" }
                   ,
                    {
                        new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                      , "\"-2:22:22:22.2222220\""
                    }
                }
          , new FieldExpect<TimeSpan>(TimeSpan.MaxValue, "'{0:G}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'10675199:02:48:05.4775807'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'10675199:02:48:05.4775807'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
          , new FieldExpect<TimeSpan>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"    -10675199.02:48:05.4775808\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"    -10675199.02:48:05.4775808\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022    -10675199.02:48:05.4775808\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
          , new FieldExpect<TimeSpan>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                      "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'03-03-33-33.333'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-03-33-33.333'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
          , new FieldExpect<TimeSpan>(new TimeSpan(-4, -4, -44, -44, -444, -444), "\"{0,-30:G}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"-4:04:44:44.4444440           \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"-4:04:44:44.4444440           \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022-4:04:44:44.4444440           \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

            // TimeSpan?
          , new FieldExpect<TimeSpan?>(TimeSpan.Zero, "!nV^1LD")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "00:00:00" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"00:00:00\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"00:00:00\"" }
            }
          , new FieldExpect<TimeSpan?>(null, "", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty), "00:00:00"
                }
              , { new EK(SimpleType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"00:00:00\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                         , Log | Compact | Pretty) , "00:00:00"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "00:00:00"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString 
                       | DefaultBecomesFallbackValue), "\"00:00:00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<TimeSpan?>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "1.01:01:01.1111110" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"1.01:01:01.1111110\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "1.01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.01:01:01.1111110\""
                }
            }
          , new FieldExpect<TimeSpan?>(new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
                                     , new TimeSpan(-2, -22, -22, -22, -222, -222))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "-2:22:22:22.2222220" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"-2:22:22:22.2222220\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "-2:22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"-2:22:22:22.2222220\""
                }
            }
          , new FieldExpect<TimeSpan?>(TimeSpan.MaxValue, "'{0:G}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'10675199:02:48:05.4775807'" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'10675199:02:48:05.4775807'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'10675199:02:48:05.4775807'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'10675199:02:48:05.4775807'\""
                }
            }
          , new FieldExpect<TimeSpan?>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"    -10675199.02:48:05.4775808\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"    -10675199.02:48:05.4775808\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022    -10675199.02:48:05.4775808\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    -10675199.02:48:05.4775808\""
                }
            }
          , new FieldExpect<TimeSpan?>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                       "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'03-03-33-33.333'" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-03-33-33.333'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'03-03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-03-33-33.333'\""
                }
            }
          , new FieldExpect<TimeSpan?>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                     , "\"{0,-30:G}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"-4:04:44:44.4444440           \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"-4:04:44:44.4444440           \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022-4:04:44:44.4444440           \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"-4:04:44:44.4444440           \""
                }
            }

            // DateOnly
          , new FieldExpect<DateOnly>(DateOnly.MinValue, "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0001-01-01" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0001-01-01\"" }
            }
          , new FieldExpect<DateOnly>(new DateOnly(2000, 1, 1), "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "2000-01-01" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "2000-01-01"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01\""
                }
            }
          , new FieldExpect<DateOnly>(new DateOnly(2020, 2, 2), "o", true
                                    , new DateOnly(2020, 2, 2))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "2020-02-02" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02\"" }
            }
          , new FieldExpect<DateOnly>(DateOnly.MaxValue, "'{0:o}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'9999-12-31'" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'9999-12-31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31'\""
                }
            }
          , new FieldExpect<DateOnly>(DateOnly.MinValue, "\"{0,30:o}\"", true
                                    , new DateOnly(2020, 1, 1))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                    0001-01-01\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                    0001-01-01\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022                    0001-01-01\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                    0001-01-01\""
                }
            }
          , new FieldExpect<DateOnly>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1980\\07\\31'" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , """
                    "'1980\07\31'"
                    """
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "'1980\\07\\31'"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'1980\\07\\31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "'1980\\07\\31'"
                    """
                }
            }
          , new FieldExpect<DateOnly>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"2009-11-12                    \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"2009-11-12                    \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u00222009-11-12                    \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12                    \""
                }
            }

            // DateOnly?
          , new FieldExpect<DateOnly?>(DateOnly.MinValue, "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0001-01-01" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0001-01-01\"" }
            }
          , new FieldExpect<DateOnly?>(null, "yyyy-MM-dd", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty) , "0001-01-01"
                }
              , { new EK(SimpleType | CallsViaMatch), "\"0001-01-01\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero 
                        |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "0001-01-01"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue) , "\"0001-01-01\""
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<DateOnly?>(new DateOnly(2000, 1, 1), "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "2000-01-01" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "2000-01-01"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"2000-01-01\""
                }
            }
          , new FieldExpect<DateOnly?>(new DateOnly(2020, 2, 2), "o", true
                                     , new DateOnly(2020, 2, 2))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "2020-02-02" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02\"" }
            }
          , new FieldExpect<DateOnly?>(DateOnly.MaxValue, "'{0:o}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'9999-12-31'" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'9999-12-31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'9999-12-31'\""
                }
            }
          , new FieldExpect<DateOnly?>(DateOnly.MinValue, "\"{0,30:o}\"", true, new DateOnly(2020, 1, 1))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                    0001-01-01\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"                    0001-01-01\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022                    0001-01-01\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                    0001-01-01\""
                }
            }
          , new FieldExpect<DateOnly?>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1980\\07\\31'" }
              , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'1980\\07\\31'\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "'1980\\07\\31'"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'1980\\07\\31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "'1980\\07\\31'"
                    """
                }
            }
          , new FieldExpect<DateOnly?>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"2009-11-12                    \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"2009-11-12                    \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u00222009-11-12                    \u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"2009-11-12                    \""
                }
            }

            // TimeOnly
          , new FieldExpect<TimeOnly>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "00:00:00" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"00:00:00\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"00:00:00\"" }
            }
          , new FieldExpect<TimeOnly>(new TimeOnly(1, 1, 1, 111, 111), "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "01:01:01.1111110" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"01:01:01.1111110\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"01:01:01.1111110\""
                }
            }
          , new FieldExpect<TimeOnly>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                                    , new TimeOnly(22, 22, 22, 222, 222))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "22:22:22.2222220" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"22:22:22.2222220\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "22:22:22.2222220" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"22:22:22.2222220\"" }
            }
          , new FieldExpect<TimeOnly>(TimeOnly.MaxValue, "'{0:o}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'23:59:59.9999999'" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'23:59:59.9999999'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'23:59:59.9999999'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'23:59:59.9999999'\""
                }
            }
          , new FieldExpect<TimeOnly>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                                    , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                      00:00:00\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                      00:00:00\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u0022                      00:00:00\u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"                      00:00:00\""
                }
            }
          , new FieldExpect<TimeOnly>(new TimeOnly(3, 33, 33, 333, 333),
                                      "'{0:hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'03-33-33.333'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-33-33.333'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                  , "\"'03-33-33.333'\""
                }
            }
          , new FieldExpect<TimeOnly>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"04:44:44.4444440              \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"04:44:44.4444440              \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u002204:44:44.4444440              \u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"04:44:44.4444440              \""
                }
            }

            // TimeOnly?
          , new FieldExpect<TimeOnly?>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "00:00:00" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"00:00:00\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "00:00:00" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"00:00:00\"" }
            }
          , new FieldExpect<TimeOnly?>(null, "HH:mm:ss.FFFFFFF", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                        , CompactLog | Pretty) , "00:00:00" }
              , { new EK(SimpleType | CallsViaMatch), "\"00:00:00\"" }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut  
                         | DefaultBecomesFallbackValue | DefaultBecomesNull, Log | Compact | Pretty)
                  , "00:00:00"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue 
                       | DefaultBecomesNull), "\"00:00:00\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut 
                         | DefaultBecomesFallbackValue | DefaultBecomesFallbackString | DefaultBecomesZero, Log | Compact | Pretty)
                  , "00:00:00"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue 
                       | DefaultBecomesZero), "\"00:00:00\"" }
              ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites)
                  , "null"
                }
            }
          , new FieldExpect<TimeOnly?>(new TimeOnly(1, 1, 1, 111, 111), "o")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "01:01:01.1111110" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"01:01:01.1111110\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "01:01:01.1111110"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"01:01:01.1111110\""
                }
            }
          , new FieldExpect<TimeOnly?>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                                     , new TimeOnly(22, 22, 22, 222, 222))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "22:22:22.2222220" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"22:22:22.2222220\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "22:22:22.2222220"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"22:22:22.2222220\""
                }
            }
          , new FieldExpect<TimeOnly?>(TimeOnly.MaxValue, "'{0:o}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'23:59:59.9999999'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'23:59:59.9999999'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'23:59:59.9999999'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'23:59:59.9999999'\""
                }
            }
          , new FieldExpect<TimeOnly?>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                                     , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                      00:00:00\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                      00:00:00\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u0022                      00:00:00\u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"                      00:00:00\""
                }
            }
          , new FieldExpect<TimeOnly?>(new TimeOnly(3, 33, 33, 333, 333),
                                       "'{0:hh\\-mm\\-ss\\.fff}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'03-33-33.333'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-33-33.333'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'03-33-33.333'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'03-33-33.333'\""
                }
            }
          , new FieldExpect<TimeOnly?>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"04:44:44.4444440              \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"04:44:44.4444440              \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable), """
                                                                 "\u002204:44:44.4444440              \u0022"
                                                                 """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"04:44:44.4444440              \""
                }
            }

            // Rune
          , new FieldExpect<Rune>(Rune.GetRuneAt("\0", 0))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0000"
                    """
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\0" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , """
                    "\u0000"
                    """
                }
            }
          , new FieldExpect<Rune>(Rune.GetRuneAt("𝄞", 0))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "𝄞" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"𝄞\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\ud834\udd1e"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "𝄞"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "\ud834\udd1e"
                    """
                }
            }
          , new FieldExpect<Rune>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'𝄢'" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"'𝄢'\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "'\ud834\udd22'"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'𝄢'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "'\ud834\udd22'"
                    """
                }
            }
          , new FieldExpect<Rune>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"𝅘𝅥𝅮                  \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
                  , """
                    "\ud834\udd60                  "
                    """
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022\ud834\udd60                  \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "\"𝅘𝅥𝅮                  \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "\ud834\udd60                  "
                    """
                }
            }

            // Rune?
          , new FieldExpect<Rune?>(Rune.GetRuneAt("\0", 0))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0000"
                    """
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\0" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , """
                    "\u0000"
                    """
                }
            }
          , new FieldExpect<Rune?>(null, "", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue, Log | Compact | Pretty), "\0" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue, Log | Compact | Pretty), "\"\0\"" }
              , { new EK(SimpleType | CallsViaMatch), "\"\\u0000\"" }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut 
                         | DefaultBecomesNull | DefaultBecomesFallbackValue, Log | Compact | Pretty) , "\"\0\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue
                         | DefaultBecomesNull , Json | Compact | Pretty) , "\"\\u0000\""
                }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty)
                  , "\0"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackString
                         | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty)
                  , "\"\0\""
                }
              , { new EK(SimpleType | AcceptsSpanFormattable 
                       | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\\u0000\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull) , "null" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<Rune?>(Rune.GetRuneAt("𝄞", 0))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "𝄞" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"𝄞\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\ud834\udd1e"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "𝄞"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "\ud834\udd1e"
                    """
                }
            }
          , new FieldExpect<Rune?>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'𝄢'" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"'𝄢'\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "'\ud834\udd22'"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'𝄢'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "'\ud834\udd22'"
                    """
                }
            }
          , new FieldExpect<Rune?>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"𝅘𝅥𝅮                  \"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
                  , """
                    "\ud834\udd60                  "
                    """
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022\ud834\udd60                  \u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "\"𝅘𝅥𝅮                  \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "\ud834\udd60                  "
                    """
                }
            }

            // Guid
          , new FieldExpect<Guid>(Guid.Empty)
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"00000000-0000-0000-0000-000000000000\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"00000000-0000-0000-0000-000000000000\""
                }
            }
          , new FieldExpect<Guid>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                  , "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\""
                }
            }
          , new FieldExpect<Guid>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D")
                                , "'{0}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\""
                }
            }
          , new FieldExpect<Guid>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022    beeeeeef-beef-beef-beef-caaaaaaaaa4e\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
                }
            }

            // Guid?
          , new FieldExpect<Guid?>(Guid.Empty)
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"00000000-0000-0000-0000-000000000000\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"00000000-0000-0000-0000-000000000000\""
                }
            }
          , new FieldExpect<Guid?>(null, "", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue, Log | Compact | Pretty), "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut 
                         | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue 
                         | DefaultBecomesZero, Log | Compact | Pretty)
                  , "\"00000000-0000-0000-0000-000000000000\""
                }
              , { new EK(SimpleType | CallsViaMatch), "\"00000000-0000-0000-0000-000000000000\"" }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut 
                         | DefaultBecomesFallbackValue | DefaultBecomesNull, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue 
                         | DefaultBecomesNull), "\"00000000-0000-0000-0000-000000000000\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut 
                         | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty)
                  , "00000000-0000-0000-0000-000000000000"
                }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString 
                         , Log | Compact | Pretty) , "00000000-0000-0000-0000-000000000000"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackString)
                  , "\"00000000-0000-0000-0000-000000000000\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue 
                         | DefaultBecomesZero), "\"00000000-0000-0000-0000-000000000000\""
                }
                // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
                // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"00000000-0000-0000-0000-000000000000\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsStringOut), "null" }
            }
          , new FieldExpect<Guid?>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\""
                }
            }
          , new FieldExpect<Guid?>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\""
                }
            }
          , new FieldExpect<Guid?>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022    beeeeeef-beef-beef-beef-caaaaaaaaa4e\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
                }
            }

            // IPNetwork
          , new FieldExpect<IPNetwork>(new IPNetwork())
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0.0.0/0" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0/0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0.0.0.0/0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0.0.0.0/0\"" }
            }
          , new FieldExpect<IPNetwork>(new IPNetwork(IPAddress.Loopback, 32))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "127.0.0.1/32" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"127.0.0.1/32\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "127.0.0.1/32"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1/32\""
                }
            }
          , new FieldExpect<IPNetwork>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'255.255.255.254/31'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254/31'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'255.255.255.254/31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'255.255.255.254/31'\""
                }
            }
          , new FieldExpect<IPNetwork>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"   255.255.0.0/16\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"   255.255.0.0/16\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022   255.255.0.0/16\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"   255.255.0.0/16\""
                }
            }

            // IPNetwork?
          , new FieldExpect<IPNetwork?>(new IPNetwork())
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0.0.0/0" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0/0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0.0.0.0/0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0.0.0.0/0\"" }
            }
          , new FieldExpect<IPNetwork?>(null, "", true)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                         , Log | Compact | Pretty)
                  , "0.0.0.0/0"
                }
              , { new EK(SimpleType | CallsViaMatch), "\"0.0.0.0/0\"" }
             ,  {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut 
                        |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "0.0.0.0/0"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString 
                       | DefaultBecomesFallbackValue), "\"0.0.0.0/0\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable  | DefaultBecomesNull), "null" }
              , {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                         , Log | Compact | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                         | DefaultBecomesFallbackValue , Log | Compact | Pretty)
                  , "0.0.0.0/0"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0/0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<IPNetwork?>(new IPNetwork(IPAddress.Loopback, 32))
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "127.0.0.1/32" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"127.0.0.1/32\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "127.0.0.1/32"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1/32\""
                }
            }
          , new FieldExpect<IPNetwork?>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'")
            {
                {
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'255.255.255.254/31'"
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254/31'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "'255.255.255.254/31'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'255.255.255.254/31'\""
                }
            }
          , new FieldExpect<IPNetwork?>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"   255.255.0.0/16\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"   255.255.0.0/16\"" }
               ,
                {
                    new EK(SimpleType | AcceptsSpanFormattable)
                  , """
                    "\u0022   255.255.0.0/16\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"   255.255.0.0/16\""
                }
            }
        };
}
