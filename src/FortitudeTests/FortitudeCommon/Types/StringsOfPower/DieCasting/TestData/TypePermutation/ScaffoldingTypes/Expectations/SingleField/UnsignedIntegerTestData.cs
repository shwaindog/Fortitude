// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public class UnsignedIntegerTestData
{
    
    private static PositionUpdatingList<ISingleFieldExpectation>? unsignedIntegerExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> UnsignedIntegerExpectations => unsignedIntegerExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(UnsignedIntegerTestData))
        {
            
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut), "\\u0000" }
          , { new EK(SimpleType | CallsViaMatch), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\\u0000" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"\\u0000\"" }
        }
      , new FieldExpect<char?>(null, "", true)
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\\u0000" }
          , { new EK(SimpleType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty), "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, Log | Compact | Pretty), "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, Json | Compact | Pretty), "\\u0000" }
          , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue | DefaultBecomesZero
                   , Json | Compact | Pretty), "\"\\u0000\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue | DefaultBecomesZero
                   , Log | Compact | Pretty) , "\0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, Log | Compact | Pretty) , "\"\0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\\u0000" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0000\"" }
          ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<char?>('A')
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "A" }
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut, Log | Compact | Pretty), "' '" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut), "' '" }
          , { new EK(SimpleType | CallsViaMatch), "\"' '\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "' '" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "' '" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "' '" }
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
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
            { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
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

        // UInt128
      , new FieldExpect<UInt128>(0, "")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0\"" }
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
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"$32\"" }
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
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"0\"" }
        }
      , new FieldExpect<UInt128?>(null, "", true)
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
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
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"$32\"" }
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
    };
}
