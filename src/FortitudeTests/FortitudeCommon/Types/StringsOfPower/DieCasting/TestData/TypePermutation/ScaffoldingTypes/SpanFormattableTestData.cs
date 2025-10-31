// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class SpanFormattableTestData
{
    public static readonly IFormatExpectation[] AllSpanFormattableExpectations =
    [
        // byte
        new FieldExpect<byte>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<byte>(255)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"255\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "255"
            }
        }
      , new FieldExpect<byte>(128, "C2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$128.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "$128.00"
            }
        }
      , new FieldExpect<byte>(77, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"77                  \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"77                  \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u002277                  \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"77                  \""
            }
        }
      , new FieldExpect<byte>(32, "", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "32" }
        }
      , new FieldExpect<byte>(255, "{0[..1]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2"
            }
        }
      , new FieldExpect<byte>(255, "{0[1..2]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"5\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "5"
            }
        }
      , new FieldExpect<byte>(255, "{0[1..]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"55\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "55"
            }
        }

        // byte?
      , new FieldExpect<byte?>(0, "{0}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<byte?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites)
              , "null"
            }
        }
      , new FieldExpect<byte?>(255)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"255\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "255"
            }
        }
      , new FieldExpect<byte?>(128, "C2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$128.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "$128.00"
            }
        }
      , new FieldExpect<byte?>(144, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , "\"                 144\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"                 144\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022                 144\\u0022\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                 144\""
            }
        }
      , new FieldExpect<byte?>(64, "", true, 64)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"64\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "64" }
        }
      , new FieldExpect<byte?>(255, "{0[..1]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2"
            }
        }
      , new FieldExpect<byte?>(255, "{0[1..2]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"5\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "5"
            }
        }
      , new FieldExpect<byte?>(255, "{0[1..]}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"55\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "55"
            }
        }

        // char
      , new FieldExpect<char>('\0', "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty ), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\\u0000" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut
                     , Log | Compact | Pretty)
              , "\0"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut
                     , Json | Compact | Pretty)
              , """
                "\u0000"
                """
            }
        }
      , new FieldExpect<char>('A')
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "A" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"A\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "A"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"A\""
            }
        }
      , new FieldExpect<char>(' ', "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "' '" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"' '\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "' '"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"' '\""
            }
        }
      , new FieldExpect<char>('z', "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"z                   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"z                   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022z                   \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"z                   \""
            }
        }

        // char?
      , new FieldExpect<char?>('\0', "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"\\u0000\"" }
        }
      , new FieldExpect<char?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback | DefaultBecomesZero 
                   , Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesZero
                   , Json | Compact | Pretty), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesZero, Log | Compact | Pretty) 
              , "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback, Log | Compact | Pretty)
              , "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<char?>('A')
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "A" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"A\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "A"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"A\""
            }
        }
      , new FieldExpect<char?>(' ', "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "' '" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultTreatedAsValueOut), "\"' '\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "' '"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"' '\""
            }
        }
      , new FieldExpect<char?>('z', "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                   z\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"                   z\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022                   z\\u0022\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                   z\""
            }
        }

        // short
      , new FieldExpect<short>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<short>(32000, "N2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<short>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<short>(-16328, "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-16328'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-16328'"
            }
        }
      , new FieldExpect<short>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // short?
      , new FieldExpect<short?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
              , "0"
            }
        }
      , new FieldExpect<short?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<short?>(32000, "N2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<short?>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<short?>(-16328, "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-16328'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-16328'"
            }
        }
      , new FieldExpect<short?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // ushort
      , new FieldExpect<ushort>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ushort>(32000, "N2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<ushort>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<ushort>(ushort.MaxValue, "'{0:B16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1111111111111111'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1111111111111111'"
            }
        }
      , new FieldExpect<ushort>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // ushort?
      , new FieldExpect<ushort?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ushort?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<ushort?>(32000, "N2")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<ushort?>(32, "C8", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
        }
      , new FieldExpect<ushort?>(ushort.MaxValue, "'{0:B16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1111111111111111'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1111111111111111'"
            }
        }
      , new FieldExpect<ushort?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // Half
      , new FieldExpect<Half>(Half.Zero)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<Half>(Half.MinValue / (Half)2.0, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-32750\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-32750"
            }
        }
      , new FieldExpect<Half>(Half.One, "", true, Half.One)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<Half>(Half.NaN, "", true, Half.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<Half>(Half.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "\"NaN\"" }
        }
      , new FieldExpect<Half>(Half.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'65500'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'65500'"
            }
        }
      , new FieldExpect<Half>(Half.MinValue, "'{0:c}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$65,504.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$65,504.00'"
            }
        }
      , new FieldExpect<Half>((Half)(Math.E * 10.0), "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"27\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "27"
            }
        }
      , new FieldExpect<Half>((Half)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14                \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14                \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.14                \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14                \""
            }
        }

        // Half?
      , new FieldExpect<Half?>(Half.Zero)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<Half?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<Half?>(Half.MinValue / (Half)2.0, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-32750\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-32750"
            }
        }
      , new FieldExpect<Half?>(Half.One, "", true, Half.One)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<Half?>(Half.NaN, "", true, Half.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<Half?>(Half.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "NaN"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "\"NaN\"" }
        }
      , new FieldExpect<Half?>(Half.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'65500'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'65500'"
            }
        }
      , new FieldExpect<Half?>(Half.MinValue, "'{0:c}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$65,504.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$65,504.00'"
            }
        }
      , new FieldExpect<Half?>((Half)(Math.E * 10.0), "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"27\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "27"
            }
        }
      , new FieldExpect<Half?>((Half)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14                \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14                \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.14                \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"3.14                \""
            }
        }

        // int
      , new FieldExpect<int>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<int>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<int>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "$32"
            }
        }
      , new FieldExpect<int>(int.MaxValue, "'{0:X8}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'7FFFFFFF'"
            }
        }
      , new FieldExpect<int>(int.MinValue, "'{0:X9}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'080000000'"
            }
        }
      , new FieldExpect<int>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,
           {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // int?
      , new FieldExpect<int?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<int?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<int?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<int?>(32, "C8", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
              , "$32.00000000"
            }
        }
      , new FieldExpect<int?>(int.MaxValue, "'{0:X8}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'7FFFFFFF'"
            }
        }
      , new FieldExpect<int?>(int.MinValue, "'{0:X9}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'080000000'"
            }
        }
      , new FieldExpect<int?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // uint
      , new FieldExpect<uint>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<uint>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<uint>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<uint>(uint.MaxValue, "'{0:X8}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFF'"
            }
        }
      , new FieldExpect<uint>(uint.MinValue, "'{0:X9}'", true, 100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'000000000'"
            }
        }
      , new FieldExpect<uint>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // uint?
      , new FieldExpect<uint?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<uint?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<uint?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<uint?>(32, "C8", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
              , "$32.00000000"
            }
        }
      , new FieldExpect<uint?>(uint.MaxValue, "'{0:X8}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFF'"
            }
        }
      , new FieldExpect<uint?>(uint.MinValue, "'{0:X9}'", true, 100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'000000000'"
            }
        }
      , new FieldExpect<uint?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // float
      , new FieldExpect<float>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<float>(1 - float.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.4028235E+38\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "3.4028235E+38"
            }
        }
      , new FieldExpect<float>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<float>(float.NaN, "", true, float.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<float>(float.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "NaN"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<float>(float.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'3.4028235E+38'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'3.4028235E+38'"
            }
        }
      , new FieldExpect<float>(float.MinValue, "'{0:c}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , "\"'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float>((float)Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.1415927           \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.1415927           \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.1415927           \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.1415927           \""
            }
        }

        // float?
      , new FieldExpect<float?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<float?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<float?>(1 - float.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.4028235E+38\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "3.4028235E+38"
            }
        }
      , new FieldExpect<float?>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<float?>(float.NaN, "", true, float.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                     , Log | Compact | Pretty)
              , "NaN"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                     , Json | Compact | Pretty)
              , "\"NaN\""
            }
        }
      , new FieldExpect<float?>(float.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "\"NaN\"" }
        }
      , new FieldExpect<float?>(float.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'3.4028235E+38'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'3.4028235E+38'"
            }
        }
      , new FieldExpect<float?>(float.MinValue, "'{0:c}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , "\"'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float?>((float)Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<float?>((float)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.1415927           \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.1415927           \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.1415927           \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.1415927           \""
            }
        }

        // long
      , new FieldExpect<long>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<long>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<long>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<long>(long.MaxValue, "'{0:X16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'7FFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<long>(long.MinValue, "'{0:X17}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'08000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'08000000000000000'"
            }
        }
      , new FieldExpect<long>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // long?
      , new FieldExpect<long?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<long?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<long?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<long?>(32, "C8", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
        }
      , new FieldExpect<long?>(long.MaxValue, "'{0:X16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'7FFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<long?>(long.MinValue, "'{0:X17}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'08000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'08000000000000000'"
            }
        }
      , new FieldExpect<long?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // ulong
      , new FieldExpect<ulong>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ulong>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<ulong>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<ulong>(ulong.MaxValue, "'{0:X16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<ulong>(ulong.MinValue, "'{0:X17}'", true, 100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'00000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'00000000000000000'"
            }
        }
      , new FieldExpect<ulong>(55, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // ulong?
      , new FieldExpect<ulong?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ulong?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<ulong?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<ulong?>(32, "C8", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
        }
      , new FieldExpect<ulong?>(ulong.MaxValue, "'{0:X16}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<ulong?>(ulong.MinValue, "'{0:X17}'", true, 100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'00000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'00000000000000000'"
            }
        }
      , new FieldExpect<ulong?>(55, "\"{0,20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // double
      , new FieldExpect<double>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<double>(1 - double.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1.7976931348623157E+308\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "1.7976931348623157E+308"
            }
        }
      , new FieldExpect<double>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<double>(double.NaN, "", true, double.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<double>(double.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<double>(double.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.7976931348623157E+308'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1.7976931348623157E+308'"
            }
        }
      , new FieldExpect<double>(double.MinValue, "'{0:c}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , """
                '-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<double>(Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<double>(Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.141592653589793   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.141592653589793   \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.141592653589793   \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.141592653589793   \""
            }
        }

        // double?
      , new FieldExpect<double?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<double?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<double?>(1 - double.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1.7976931348623157E+308\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "1.7976931348623157E+308"
            }
        }
      , new FieldExpect<double?>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<double?>(double.NaN, "", true, double.NaN)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<double?>(double.NaN, "\"{0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<double?>(double.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.7976931348623157E+308'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1.7976931348623157E+308'"
            }
        }
      , new FieldExpect<double?>(double.MinValue, "'{0:c}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , """
                '-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<double?>(Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<double?>(Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.141592653589793   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.141592653589793   \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.141592653589793   \u0022"
                """ 
            }
         ,  {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.141592653589793   \""
            }
        }

        // decimal
      , new FieldExpect<decimal>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<decimal>(decimal.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-79228162514264337593543950335\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-79228162514264337593543950335"
            }
        }
      , new FieldExpect<decimal>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<decimal>(decimal.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'79228162514264337593543950335'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'79228162514264337593543950335'"
            }
        }
      , new FieldExpect<decimal>(decimal.MinValue, "'{0:c}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$79,228,162,514,264,337,593,543,950,335.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal>((decimal)Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<decimal>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14159265358979    \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"3.14159265358979    \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.14159265358979    \u0022"
                """
          }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14159265358979    \""
            }
        }

        // decimal?
      , new FieldExpect<decimal?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<decimal?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<decimal?>(decimal.MinValue, "R")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-79228162514264337593543950335\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-79228162514264337593543950335"
            }
        }
      , new FieldExpect<decimal?>(1, "", true, 1)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<decimal?>(decimal.MaxValue, "'{0:G}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'79228162514264337593543950335'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'79228162514264337593543950335'"
            }
        }
      , new FieldExpect<decimal?>(decimal.MinValue, "'{0:c}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$79,228,162,514,264,337,593,543,950,335.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal?>((decimal)Math.E * 1_000_000, "N0")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<decimal?>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14159265358979    \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14159265358979    \"" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u00223.14159265358979    \u0022"
                """ 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.14159265358979    \"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14159265358979    \""
            }
        }

        // Int128
      , new FieldExpect<Int128>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<Int128>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<Int128>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<Int128>(Int128.MaxValue, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<Int128>(Int128.MinValue, "'{0:X33}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'080000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'080000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<Int128>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty )
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022170,141,183,460,469,231,731,687,303,715,884,105,727 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
        }

        // Int128?
      , new FieldExpect<Int128?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<Int128?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                     , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<Int128?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<Int128?>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<Int128?>(Int128.MaxValue, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<Int128?>(Int128.MinValue, "'{0:X33}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'080000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'080000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<Int128?>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty )
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty )
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022170,141,183,460,469,231,731,687,303,715,884,105,727 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
            }
        }

        // UInt128
      , new FieldExpect<UInt128>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<UInt128>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<UInt128>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<UInt128>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'000000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'000000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,{
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022340,282,366,920,938,463,463,374,607,431,768,211,455 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }

        // UInt128?
      , new FieldExpect<UInt128?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<UInt128?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<UInt128?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<UInt128?>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'000000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"'000000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,{
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022340,282,366,920,938,463,463,374,607,431,768,211,455 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }

        // BigInteger
      , new FieldExpect<BigInteger>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<BigInteger>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0x7D00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"0x7D00\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<BigInteger>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty )
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'" 
            }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\""
            }
        }
      , new FieldExpect<BigInteger>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'E700000000000000000000000000000000'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'E700000000000000000000000000000000'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "'E700000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'E700000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \"" }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Json | Compact | Pretty)
              , """
                "\u002234,028,236,692,093,846,346,337,460,743,176,821,145,500  \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }

        // BigInteger?
      , new FieldExpect<BigInteger?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0\"" }
        }
      , new FieldExpect<BigInteger?>(null, "", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut, Log | Compact | Pretty ), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
                     , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<BigInteger?>(32000, "0x{0:X}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0x7D00" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0x7D00\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<BigInteger?>(32, "C0", true, 32)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"$32\"" }
        }
      , new FieldExpect<BigInteger?>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty )
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'" 
            }
           ,
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\""
            }
        }
      , new FieldExpect<BigInteger?>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'E700000000000000000000000000000000'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'E700000000000000000000000000000000'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'E700000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'E700000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<BigInteger?>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , "\"\\u002234,028,236,692,093,846,346,337,460,743,176,821,145,500  \\u0022\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }

        // Complex
      , new FieldExpect<Complex>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<0; 0>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c0; 0\u003e"
                """
            }
           ,{ new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<0; 0>" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c0; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(32000, "{0:N0}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32,000; 0>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32,000; 0>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "<32,000; 0>"
            }
         , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(32.0d, 1), "N0", true
                               , new Complex(32.0d, 1))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32; 1>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32; 1>\"" }
          , {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u003c32; 1\u003e"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<32; 1>" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c32; 1\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'<1,000,000.00; 1,000,000.00>'\"" }
          , {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , """
                "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """ 
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """ 
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<Complex>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<3.141592653589793; 2.718281828459045>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "\u0022\u003c3.141592653589793; 2.718281828459045\u003e\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
        }

        // Complex?
      , new FieldExpect<Complex?>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<0; 0>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c0; 0\u003e"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     , Log | Compact | Pretty)
              , "<0; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     , Json | Compact | Pretty)
              , """
                "\u003c0; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(null, "", true)
        {  
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "<0; 0>" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback, Json | Compact | Pretty), "\"\\u003c0; 0\\u003e\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero, Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "<0; 0>" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback), "\"\\u003c0; 0\\u003e\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero)
              , "<0; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<Complex?>(32000, "{0:N0}")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32,000; 0>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32,000; 0>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "<32,000; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32; 1>" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32; 1>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u003c32; 1\u003e"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<32; 1>" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c32; 1\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'<1,000,000.00; 1,000,000.00>'\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , """
                "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<Complex?>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<3.141592653589793; 2.718281828459045>\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "\u0022\u003c3.141592653589793; 2.718281828459045\u003e\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
        }

        // DateTime
      , new FieldExpect<DateTime>(DateTime.MinValue, "O")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
               , "0001-01-01T00:00:00.0000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01T00:00:00.0000000\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "0001-01-01T00:00:00.0000000"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites
                     , Json | Compact | Pretty)
              , "\"0001-01-01T00:00:00.0000000\""
            }
        }
      , new FieldExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "2000-01-01T01:01:01.1111111" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01T01:01:01.1111111\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "2020-02-02T00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02T00:00:00\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02T00:00:00" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02T00:00:00\"" }
        }
      , new FieldExpect<DateTime>(DateTime.MaxValue, "'{0:u}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'9999-12-31 23:59:59Z'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31 23:59:59Z'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'9999-12-31 23:59:59Z'"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'9999-12-31 23:59:59Z'\""
            }
        }
      , new FieldExpect<DateTime>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty) , "\"          0001-01-01 00:00:00Z\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"          0001-01-01 00:00:00Z\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable) 
              , """
                "\u0022          0001-01-01 00:00:00Z\u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new FieldExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'1980-07-31 11:48:13'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1980-07-31 11:48:13'\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty) , "\"2009-11-12T19:49:00.0000000   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"2009-11-12T19:49:00.0000000   \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable) 
              , """
                "\u00222009-11-12T19:49:00.0000000   \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"2009-11-12T19:49:00.0000000   \""
            }
        }

        // DateTime?
      , new FieldExpect<DateTime?>(DateTime.MinValue, "O")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0001-01-01T00:00:00.0000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , "\"0001-01-01T00:00:00.0000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01T00:00:00.0000000" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , "\"0001-01-01T00:00:00.0000000\""
            }
        }
      , new FieldExpect<DateTime?>(null, "yyyy-MM-ddTHH:mm:ss", true)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty)
              , "0001-01-01T00:00:00" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"0001-01-01T00:00:00\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesNull
              , Log | Compact | Pretty ), "0001-01-01T00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesNull
                     ), "\"0001-01-01T00:00:00\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
              , Log | Compact | Pretty ), "0001-01-01T00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero | DefaultBecomesFallback), "\"0001-01-01T00:00:00\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback), "\"0001-01-01T00:00:00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<DateTime?>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "2000-01-01T01:01:01.1111111" }
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
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "2020-02-02T00:00:00" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02T00:00:00\"" }
              , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'9999-12-31 23:59:59Z'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31 23:59:59Z'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022          0001-01-01 00:00:00Z\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"          0001-01-01 00:00:00Z\""
            }
        }
      , new FieldExpect<DateTime?>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'1980-07-31 11:48:13'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1980-07-31 11:48:13'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u00222009-11-12T19:49:00.0000000   \u0022"
                """
            }
          , {
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
          , {
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
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'10675199:02:48:05.4775807'" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'10675199:02:48:05.4775807'\"" }
          , {
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
          ,{ new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut ), "\"    -10675199.02:48:05.4775808\"" }
          ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022    -10675199.02:48:05.4775808\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"    -10675199.02:48:05.4775808\""
            }
        }
      , new FieldExpect<TimeSpan>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                  "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'03-03-33-33.333'" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-03-33-33.333'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022-4:04:44:44.4444440           \u0022"
                """
            }
          , {
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
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty)
              , "00:00:00" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"00:00:00\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero, Log | Compact | Pretty)
              , "00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable  | DefaultBecomesFallback), "\"00:00:00\"" }
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
          , {
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
          , {
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
          , {
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
           ,{ new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut ), "\"    -10675199.02:48:05.4775808\"" }
           ,
            {
                new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022    -10675199.02:48:05.4775808\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"    -10675199.02:48:05.4775808\""
            }
        }
      , new FieldExpect<TimeSpan?>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                   "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'03-03-33-33.333'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-03-33-33.333'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty), "'03-03-33-33.333'"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty) , "\"'03-03-33-33.333'\""
            }
        }
      , new FieldExpect<TimeSpan?>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                 , "\"{0,-30:G}\"")
        {
           { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"-4:04:44:44.4444440           \"" }
         , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"-4:04:44:44.4444440           \"" }
         , { new EK(SimpleType | AcceptsSpanFormattable)
             , """
               "\u0022-4:04:44:44.4444440           \u0022"
               """
            }
         , {
              new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
             , "\"-4:04:44:44.4444440           \""
          }
        }

        // DateOnly
      , new FieldExpect<DateOnly>(DateOnly.MinValue, "o")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty ), "0001-01-01" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0001-01-01\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0001-01-01" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0001-01-01\"" }
        }
      , new FieldExpect<DateOnly>(new DateOnly(2000, 1, 1), "o")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "2000-01-01" }
           ,{ new EK(SimpleType | AcceptsSpanFormattable), "\"2000-01-01\"" }
          , {
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
           ,{ new EK(SimpleType | AcceptsSpanFormattable), "\"2020-02-02\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "2020-02-02" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"2020-02-02\"" }
        }
      , new FieldExpect<DateOnly>(DateOnly.MaxValue, "'{0:o}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'9999-12-31'" }
           ,{ new EK(SimpleType | AcceptsSpanFormattable), "\"'9999-12-31'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022                    0001-01-01\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                    0001-01-01\""
            }
        }
      , new FieldExpect<DateOnly>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1980\\07\\31'" }
           ,{ new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , """
                "'1980\07\31'"
                """
            }
           ,{ new EK(SimpleType | AcceptsSpanFormattable)
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
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty) , "\"2009-11-12                    \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"2009-11-12                    \"" }
          , { new EK(SimpleType | AcceptsSpanFormattable) 
              , """
                "\u00222009-11-12                    \u0022"
                """ 
            }
          , {
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
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty)
              , "0001-01-01" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"0001-01-01\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
                   , Log | Compact | Pretty) , "0001-01-01" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero | DefaultBecomesFallback) 
              , "\"0001-01-01\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
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
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"                    0001-01-01\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable) 
              , """
                "\u0022                    0001-01-01\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                    0001-01-01\""
            }
        }
      , new FieldExpect<DateOnly?>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1980\\07\\31'" }
          , { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'1980\\07\\31'\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u00222009-11-12                    \u0022"
                """
            }
          , {
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
          , {
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
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022                      00:00:00\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"                      00:00:00\""
            }
        }
      , new FieldExpect<TimeOnly>(new TimeOnly(3, 33, 33, 333, 333),
                                  "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'03-33-33.333'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-33-33.333'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u002204:44:44.4444440              \u0022"
                """
            }
          , {
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
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, 
                     Log | Compact | Pretty), "00:00:00" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"00:00:00\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesNull
                   , Log | Compact | Pretty), "00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesNull), "\"00:00:00\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty), "00:00:00" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesZero), "\"00:00:00\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"00:00:00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites)
              , "null"
            }
        }
      , new FieldExpect<TimeOnly?>(new TimeOnly(1, 1, 1, 111, 111), "o")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "01:01:01.1111110" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"01:01:01.1111110\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "22:22:22.2222220" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"22:22:22.2222220\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'23:59:59.9999999'" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'23:59:59.9999999'\"" } 
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u0022                      00:00:00\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"                      00:00:00\""
            }
        }
      , new FieldExpect<TimeOnly?>(new TimeOnly(3, 33, 33, 333, 333),
                                   "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'03-33-33.333'" 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'03-33-33.333'\"" } 
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable), 
                """
                "\u002204:44:44.4444440              \u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"04:44:44.4444440              \""
            }
        }

        // Rune
      , new FieldExpect<Rune>(Rune.GetRuneAt("\0", 0))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\ud834\udd1e"
                """
            }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\ud834\udd60                  "
                """ 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022\ud834\udd60                  \u0022"
                """
            }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
            { new EK(SimpleType | AcceptsAnyGeneric  | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesNull
                   , Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback | DefaultBecomesNull 
                   , Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback | DefaultBecomesNull
                   , Json | Compact | Pretty), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty) , "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty) , "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
          //   { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          // , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty), "\0" }
          // , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback, Log | Compact | Pretty), "\"\0\"" }
          // , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "\"\\u0000\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback
          //          , Log | Compact | Pretty), "\0" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback
          //          , Log | Compact | Pretty), "\"\0\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback
          //          , Json | Compact | Pretty)
          //     , "\"\\u0000\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          // , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites) , "null" }
        }
      , new FieldExpect<Rune?>(Rune.GetRuneAt("𝄞", 0))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "𝄞" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"𝄞\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
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
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\ud834\udd60                  "
                """ 
            }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022\ud834\udd60                  \u0022"
                """
            }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "00000000-0000-0000-0000-000000000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"00000000-0000-0000-0000-000000000000\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "beefca4e-beef-ca4e-beef-c0ffeebabe51" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022    beeeeeef-beef-beef-beef-caaaaaaaaa4e\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\""
            }
        }

        // Guid?
      , new FieldExpect<Guid?>(Guid.Empty)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "00000000-0000-0000-0000-000000000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"00000000-0000-0000-0000-000000000000\"" }
          , {
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
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback, Log | Compact | Pretty)
              , "00000000-0000-0000-0000-000000000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty) , "00000000-0000-0000-0000-000000000000"
            }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallback | DefaultBecomesZero
                   , Log | Compact | Pretty) , "\"00000000-0000-0000-0000-000000000000\""
            }
          , { new EK(SimpleType | AcceptsAnyGeneric) , "\"00000000-0000-0000-0000-000000000000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback |  DefaultBecomesNull
                   , Log | Compact | Pretty), "00000000-0000-0000-0000-000000000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback |  DefaultBecomesNull)
              , "\"00000000-0000-0000-0000-000000000000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback |  DefaultBecomesZero
                   , Log | Compact | Pretty), "00000000-0000-0000-0000-000000000000" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallback |  DefaultBecomesZero)
              , "\"00000000-0000-0000-0000-000000000000\"" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          // , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull) , "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable) , "\"00000000-0000-0000-0000-000000000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsStringOut) , "null" }
        }
      , new FieldExpect<Guid?>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "beefca4e-beef-ca4e-beef-c0ffeebabe51" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "beefca4e-beef-ca4e-beef-c0ffeebabe51"
            }
         , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"beefca4e-beef-ca4e-beef-c0ffeebabe51\""
            }
        }
      , new FieldExpect<Guid?>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022    beeeeeef-beef-beef-beef-caaaaaaaaa4e\u0022"
                """
            }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'255.255.255.254/31'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254/31'\"" }
          , {
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
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022   255.255.0.0/16\u0022"
                """
            }
          , {
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
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0.0.0.0/0" }
          , { new EK(SimpleType | AcceptsAnyGeneric), "\"0.0.0.0/0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0.0.0.0/0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback), "\"0.0.0.0/0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallback
                   , Log | Compact | Pretty), "0.0.0.0/0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0/0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites) , "null" }
        }
      , new FieldExpect<IPNetwork?>(new IPNetwork(IPAddress.Loopback, 32))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "127.0.0.1/32" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"127.0.0.1/32\"" }
          , {
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
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'255.255.255.254/31'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254/31'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'255.255.255.254/31'"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'255.255.255.254/31'\""
            }
        }
      , new FieldExpect<IPNetwork?>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"   255.255.0.0/16\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"   255.255.0.0/16\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022   255.255.0.0/16\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"   255.255.0.0/16\""
            }
        }

        // Version and Version?  (Class)
      , new FieldExpect<Version>(new Version())
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "0.0"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty) , "\"0.0\""
            }
        }
      , new FieldExpect<Version>(null, "{0}", true, new Version())
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut |  DefaultBecomesFallback
                     , Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallback
                     , Log | Compact | Pretty) , "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback) , "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallback
                     , Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallback), "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                     , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<Version, string>(null, "", true, "")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut |  DefaultBecomesFallback
                   , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallback
                   , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesNull)
              , "null"
            }
        }
      , new FieldExpect<Version>(new Version(1, 1))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "1.1" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"1.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "1.1"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"1.1\""
            }
        }
      , new FieldExpect<Version>(new Version("1.2.3.4"), "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1.2.3.4'" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.2.3.4'\"" }
           , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'1.2.3.4'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'1.2.3.4'\""
            }
        }
      , new FieldExpect<Version>(new Version(1, 0), "'{0}'", true
                               , new Version(1, 0))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1.0'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1.0'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "'1.0'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'1.0'\"" }
        }
      , new FieldExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"          5.6.7.8\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          5.6.7.8\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022          5.6.7.8\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"          5.6.7.8\""
            }
        }

        //  IPAddress and IPAddress?
      , new FieldExpect<IPAddress>(new IPAddress("\0\0\0\0"u8))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0.0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "0.0.0.0"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"0.0.0.0\""
            }
        }
      , new FieldExpect<IPAddress, string>(null, "", true, "")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                     , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                   , Log | Compact | Pretty) , "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero )
              , "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallback
                   , Log | Compact | Pretty) , "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallback), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Loopback)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "127.0.0.1" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"127.0.0.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "127.0.0.1"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"127.0.0.1\""
            }
        }
      , new FieldExpect<IPAddress>(new IPAddress([192, 168, 0, 1]), "'{0}'", true
                                 , new IPAddress([192, 168, 0, 1]))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'192.168.0.1'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'192.168.0.1'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "'192.168.0.1'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'192.168.0.1'\"" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'255.255.255.254'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'255.255.255.254'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'255.255.255.254'\""
            }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.0.0"), "\"{0,17}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty ) , "\"      255.255.0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"      255.255.0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022      255.255.0.0\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"      255.255.0.0\""
            }
        }

        //  Uri and Uri?
      , new FieldExpect<Uri>(new Uri("https://learn.microsoft.com/en-us/dotnet/api"))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                , "https://learn.microsoft.com/en-us/dotnet/api" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"https://learn.microsoft.com/en-us/dotnet/api\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "https://learn.microsoft.com/en-us/dotnet/api"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"https://learn.microsoft.com/en-us/dotnet/api\""
            }
        }
      , new FieldExpect<Uri, string>(null, "", false, "")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                     , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                    , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallback
                        , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallback), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable), "null" }
        }
      , new FieldExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'"
                           , true, new Uri("https://github.com/shwaindog/Fortitude"))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'https://github.com/shwaindog/Fortitude'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'https://github.com/shwaindog/Fortitude'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "'https://github.com/shwaindog/Fortitude'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , "\"'https://github.com/shwaindog/Fortitude'\""
            }
        }
      , new
            FieldExpect<Uri>(new
                                 Uri("https://github.com/shwaindog/Fortitude/tree/main/src/FortitudeTests/FortitudeCommon/Types/StringsOfPower/DieCasting/TestData")
                           , "{0[..38]}")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "https://github.com/shwaindog/Fortitude" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"https://github.com/shwaindog/Fortitude\"" }
              , {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty) , "https://github.com/shwaindog/Fortitude"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty) , "\"https://github.com/shwaindog/Fortitude\""
                }
            }
      , new FieldExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\""
            }
        }
    ];
}
