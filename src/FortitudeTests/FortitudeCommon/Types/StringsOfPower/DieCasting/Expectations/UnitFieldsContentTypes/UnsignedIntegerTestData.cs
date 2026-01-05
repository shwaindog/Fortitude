// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public class UnsignedIntegerTestData
{
    
    private static PositionUpdatingList<ISingleFieldExpectation>? unsignedIntegerExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> UnsignedIntegerExpectations => unsignedIntegerExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(UnsignedIntegerTestData))
        {
            
        // byte
        new FieldExpect<byte>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<byte>(255)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"255\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "255"
            }
        }
      , new FieldExpect<byte>(128, "C2")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$128.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "$128.00"
            }
        }
      , new FieldExpect<byte>(77, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"77                  \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyJson)
              , "\"77                  \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u002277                  \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"77                  \""
            }
        }
      , new FieldExpect<byte>(32, "", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "32" }
        }
      , new FieldExpect<byte>(255, "{0[..1]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "2"
            }
        }
      , new FieldExpect<byte>(255, "{0[1..2]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"5\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "5"
            }
        }
      , new FieldExpect<byte>(255, "{0[1..]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"55\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "55"
            }
        }

        // byte?
      , new FieldExpect<byte?>(0, "{0}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<byte?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites)
              , "null"
            }
        }
      , new FieldExpect<byte?>(255)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"255\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "255"
            }
        }
      , new FieldExpect<byte?>(128, "C2")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$128.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "$128.00"
            }
        }
      , new FieldExpect<byte?>(144, "\"{0,20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog)
              , "\"                 144\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyJson)
              , "\"                 144\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"\\u0022                 144\\u0022\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"                 144\""
            }
        }
      , new FieldExpect<byte?>(64, "", true, 64)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"64\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "64" }
        }
      , new FieldExpect<byte?>(255, "{0[..1]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "2"
            }
        }
      , new FieldExpect<byte?>(255, "{0[1..2]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"5\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "5"
            }
        }
      , new FieldExpect<byte?>(255, "{0[1..]}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"55\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "55"
            }
        }

        // char
      , new FieldExpect<char>('\0', "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog ), "\0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, AnyLog), "\"\0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"\\u0000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut
                     , AnyLog)
              , "\0"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut
                     , AnyJson)
              , """
                "\u0000"
                """
            }
        }
      , new FieldExpect<char>('A')
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog), "A" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"A\"" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyLog) , "A" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyJson) , "\"A\"" }
        }
      , new FieldExpect<char>(' ', "'{0}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog), "' '" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"' '\"" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask , AnyLog) , "' '" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask , AnyJson) , "\"' '\"" }
        }
      , new FieldExpect<char>('z', "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"z                   \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"z                   \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022z                   \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask)
              , "\"z                   \""
            }
        }

        // char?
      , new FieldExpect<char?>('\0', "")
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut, AnyLog), "\0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut, AnyLog), "\"\0\"" }
          , { new EK(IsContentType | CallsViaMatch), "\"\\u0000\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog), "\0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, AnyLog), "\"\0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"\\u0000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, AnyLog), "\0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, AnyJson), "\"\\u0000\"" }
        }
      , new FieldExpect<char?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , AnyLog), "\0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , AnyLog), "\"\0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"\\u0000\"" }
          , { new EK(IsContentType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"\\u0000\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, AnyLog), "\0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, AnyLog), "\"\0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue | DefaultBecomesZero, AnyJson), "\"\\u0000\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue | DefaultBecomesZero
                   , AnyJson), "\"\\u0000\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue | DefaultBecomesZero
                   , AnyLog) , "\0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, AnyLog) , "\"\0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"\\u0000\"" }
          ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<char?>('A')
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog), "A" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"A\"" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyLog) , "A" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyJson) , "\"A\"" }
        }
      , new FieldExpect<char?>(' ', "'{0}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyLog), "' '" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"' '\"" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyLog) , "' '" }
          , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, AnyJson) , "\"' '\"" }
        }
      , new FieldExpect<char?>('z', "\"{0,20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"                   z\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, AnyJson)
              , "\"                   z\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"\\u0022                   z\\u0022\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask)
              , "\"                   z\""
            }
        }
        

        // ushort
      , new FieldExpect<ushort>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ushort>(32000, "N2")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<ushort>(32, "C0", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<ushort>(ushort.MaxValue, "'{0:B16}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1111111111111111'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'1111111111111111'"
            }
        }
      , new FieldExpect<ushort>(55, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // ushort?
      , new FieldExpect<ushort?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ushort?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<ushort?>(32000, "N2")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "32,000.00"
            }
        }
      , new FieldExpect<ushort?>(32, "C8", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
        }
      , new FieldExpect<ushort?>(ushort.MaxValue, "'{0:B16}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1111111111111111'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'1111111111111111'"
            }
        }
      , new FieldExpect<ushort?>(55, "\"{0,20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }
        
        // uint
      , new FieldExpect<uint>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<uint>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<uint>(32, "C0", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<uint>(uint.MaxValue, "'{0:X8}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFF'"
            }
        }
      , new FieldExpect<uint>(uint.MinValue, "'{0:X9}'", true, 100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'000000000'"
            }
        }
      , new FieldExpect<uint>(55, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // uint?
      , new FieldExpect<uint?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<uint?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<uint?>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<uint?>(32, "C8", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
              , "$32.00000000"
            }
        }
      , new FieldExpect<uint?>(uint.MaxValue, "'{0:X8}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFF'"
            }
        }
      , new FieldExpect<uint?>(uint.MinValue, "'{0:X9}'", true, 100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'000000000'"
            }
        }
      , new FieldExpect<uint?>(55, "\"{0,20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // ulong
      , new FieldExpect<ulong>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ulong>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<ulong>(32, "C0", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
        }
      , new FieldExpect<ulong>(ulong.MaxValue, "'{0:X16}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<ulong>(ulong.MinValue, "'{0:X17}'", true, 100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'00000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'00000000000000000'"
            }
        }
      , new FieldExpect<ulong>(55, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u002255                  \u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"55                  \""
            }
        }

        // ulong?
      , new FieldExpect<ulong?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<ulong?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<ulong?>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "0x7D00"
            }
        }
      , new FieldExpect<ulong?>(32, "C8", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
        }
      , new FieldExpect<ulong?>(ulong.MaxValue, "'{0:X16}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'FFFFFFFFFFFFFFFF'"
            }
        }
      , new FieldExpect<ulong?>(ulong.MinValue, "'{0:X17}'", true, 100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'00000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "'00000000000000000'"
            }
        }
      , new FieldExpect<ulong?>(55, "\"{0,20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, AnyLog), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
           ,
            { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022                  55\u0022"
                """
            }
           ,{
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"                  55\""
            }
        }

        // UInt128
      , new FieldExpect<UInt128>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyLog)
              , "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyJson)
              , "\"0\"" }
        }
      , new FieldExpect<UInt128>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<UInt128>(32, "C0", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyLog)
              , "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyJson)
              , "\"$32\"" }
        }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "'{0:X32}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<UInt128>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "'000000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"'000000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<UInt128>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable, AnyLog)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,{
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022340,282,366,920,938,463,463,374,607,431,768,211,455 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }

        // UInt128?
      , new FieldExpect<UInt128?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyLog)
              , "0" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyJson)
              , "\"0\"" }
        }
      , new FieldExpect<UInt128?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , AnyLog), "0" }
          , { new EK(IsContentType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, AnyLog), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<UInt128?>(32000, "0x{0:X}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "0x7D00"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"0x7D00\""
            }
        }
      , new FieldExpect<UInt128?>(32, "C0", true, 32)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyLog)
              , "$32" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, AnyJson)
              , "\"$32\"" }
        }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "'{0:X32}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\""
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'000000000000000000000000000000000'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyLog)
              , "'000000000000000000000000000000000'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask
                     | DefaultTreatedAsValueOut, AnyJson)
              , "\"'000000000000000000000000000000000'\""
            }
        }
      , new FieldExpect<UInt128?>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable, AnyLog)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,{
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022340,282,366,920,938,463,463,374,607,431,768,211,455 \u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AllOutputConditionsMask)
              , "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \""
            }
        }
    };
}
