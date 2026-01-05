// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public static class SignedIntegerTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? signedIntegerExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> SignedIntegerExpectations => signedIntegerExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(SignedIntegerTestData))
        {
            // short
            new FieldExpect<short>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<short>(32000, "N2")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "32,000.00"
                }
            }
          , new FieldExpect<short>(32, "C0", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
            }
          , new FieldExpect<short>(-16328, "'{0}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-16328'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'-16328'"
                }
            }
          , new FieldExpect<short>(55, "\"{0,-20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // short?
          , new FieldExpect<short?>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
                  , "0"
                }
            }
          , new FieldExpect<short?>(null, "", true)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "0" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString 
                          | DefaultBecomesFallbackValue) , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackString |
                           DefaultBecomesFallbackValue)
                  , "\"0\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<short?>(32000, "N2")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"32,000.00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "32,000.00"
                }
            }
          , new FieldExpect<short?>(32, "C0", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
            }
          , new FieldExpect<short?>(-16328, "'{0}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-16328'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'-16328'"
                }
            }
          , new FieldExpect<short?>(55, "\"{0,20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // int
          , new FieldExpect<int>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<int>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "0x7D00"
                }
            }
          , new FieldExpect<int>(32, "C0", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "$32"
                }
            }
          , new FieldExpect<int>(int.MaxValue, "'{0:X8}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFF'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'7FFFFFFF'"
                }
            }
          , new FieldExpect<int>(int.MinValue, "'{0:X9}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'080000000'"
                }
            }
          , new FieldExpect<int>(55, "\"{0,-20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<int?>(null, "", true)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "0" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString 
                          | DefaultBecomesFallbackValue) , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackString |
                           DefaultBecomesFallbackValue)
                  , "\"0\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<int?>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "0x7D00"
                }
            }
          , new FieldExpect<int?>(32, "C8", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut)
                  , "$32.00000000"
                }
            }
          , new FieldExpect<int?>(int.MaxValue, "'{0:X8}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFF'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'7FFFFFFF'"
                }
            }
          , new FieldExpect<int?>(int.MinValue, "'{0:X9}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'080000000'"
                }
            }
          , new FieldExpect<int?>(55, "\"{0,20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // long
          , new FieldExpect<long>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<long>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "0x7D00"
                }
            }
          , new FieldExpect<long>(32, "C0", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32" }
            }
          , new FieldExpect<long>(long.MaxValue, "'{0:X16}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFF'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'7FFFFFFFFFFFFFFF'"
                }
            }
          , new FieldExpect<long>(long.MinValue, "'{0:X17}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'08000000000000000'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'08000000000000000'"
                }
            }
          , new FieldExpect<long>(55, "\"{0,-20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"55                  \"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"55                  \"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // long?
          , new FieldExpect<long?>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<long?>(null, "", true)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "0" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString 
                          | DefaultBecomesFallbackValue) , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackString |
                           DefaultBecomesFallbackValue) , "\"0\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<long?>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "0x7D00"
                }
            }
          , new FieldExpect<long?>(32, "C8", true, 32)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32.00000000\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "$32.00000000" }
            }
          , new FieldExpect<long?>(long.MaxValue, "'{0:X16}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFF'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'7FFFFFFFFFFFFFFF'"
                }
            }
          , new FieldExpect<long?>(long.MinValue, "'{0:X17}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'08000000000000000'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut)
                  , "'08000000000000000'"
                }
            }
          , new FieldExpect<long?>(55, "\"{0,20}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"                  55\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"                  55\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // Int128
          , new FieldExpect<Int128>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"0\"" }
            }
          , new FieldExpect<Int128>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "$32" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"$32\"" }
            }
          , new FieldExpect<Int128>(Int128.MaxValue, "'{0:X32}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000000000000000000000000000'\"" }
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
                    new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
                  , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"0\"" }
            }
          , new FieldExpect<Int128?>(null, "", true)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                         , Log | Compact | Pretty)
                  , "0"
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString 
                          | DefaultBecomesFallbackValue , Log | Compact | Pretty) , "0"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesFallbackString | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<Int128?>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0x7D00\"" }
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "$32" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"$32\"" }
            }
          , new FieldExpect<Int128?>(Int128.MaxValue, "'{0:X32}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'\"" }
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'080000000000000000000000000000000'\"" }
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
                    new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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

            // BigInteger
          , new FieldExpect<BigInteger>(0, "")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0\"" }
            }
          , new FieldExpect<BigInteger>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0x7D00" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"0x7D00\"" }
               ,
                {
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "$32" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"$32\"" }
            }
          , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\""
                }
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
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'E700000000000000000000000000000000'"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'E700000000000000000000000000000000'\"" }
               ,
                {
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
          , new FieldExpect<BigInteger>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Json | Compact | Pretty)
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "0" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"0\"" }
            }
          , new FieldExpect<BigInteger?>(null, "", true)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0" }
              , { new EK(IsContentType | CallsViaMatch), "\"0\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString 
                          | DefaultBecomesFallbackValue , Log | Compact | Pretty) , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                         | DefaultBecomesZero)
                  , "\"0\""
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "null"
                }
            }
          , new FieldExpect<BigInteger?>(32000, "0x{0:X}")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0x7D00" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"0x7D00\"" }
               ,
                {
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "$32" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"$32\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "$32" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "\"$32\"" }
            }
          , new FieldExpect<BigInteger?>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'\""
                }
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
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "'E700000000000000000000000000000000'"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'E700000000000000000000000000000000'\"" }
               ,
                {
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
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
                  , "\"\\u002234,028,236,692,093,846,346,337,460,743,176,821,145,500  \\u0022\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
                }
            }
        };
}
