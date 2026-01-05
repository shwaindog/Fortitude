// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class EnumTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? enumExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> EnumExpectations => enumExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(EnumTestData))
        {
            // No Default No Flags Long Enum
            new FieldExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnum.NDLNFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.NDLNFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.NDLNFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLNFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.NDLNFE_1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLNFE_1\""
                }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnum.NDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // Nullable No Default No Flags Long Enum
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(null)
            {
                { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.NDLNFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.NDLNFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLNFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.NDLNFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"NDLNFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // No Default No Flags ULong Enum
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_4, "D")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.4"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.4\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "4" }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"4\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "NoDefaultULongNoFlagsEnum.4" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "4" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // Nullable No Default No Flags ULong Enum
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(null)
            {
                { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.NDUNFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.NDUNFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDUNFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.NDUNFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"NDUNFE_1\"" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // With Default No Flags Long Enum
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_11, "0x{0[^1..]:X}")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.0xB"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.0xB\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"0xB\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "WithDefaultLongNoFlagsEnum.0xB" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"0xB\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // Nullable With Default No Flags Long Enum
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(null)
            {
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
                  , "Default"
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
              , { new EK(ContentType | CallsViaMatch), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.Default\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable), "null" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_7, "F")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.WDLNFE_7"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.WDLNFE_7\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDLNFE_7\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.WDLNFE_7"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"WDLNFE_7\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // With Default No Flags ULong Enum
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_11, "0x{0[^1..]:X}")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.0xB"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.0xB\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"0xB\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "WithDefaultULongNoFlagsEnum.0xB" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"0xB\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"     8589934592\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"     8589934592\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.\"     8589934592\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"     8589934592\""
                }
            }

            // Nullable With Default No Flags ULong Enum
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(null)
            {
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
                  , "Default"
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
              , { new EK(ContentType | CallsViaMatch), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.Default\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable), "null" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_7, "F")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.WDUNFE_7"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.WDUNFE_7\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDUNFE_7\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.WDUNFE_7"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"WDUNFE_7\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_1.JustUnnamed(), "\"{0,-15}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.\"8589934592     \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongNoFlagsEnum.8589934592     \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"8589934592     \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"8589934592     \""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u00228589934592     \\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.\"8589934592     \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"8589934592     \""
                }
            }

            // No Default With Flags Long Enum
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLWFE_1\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLWFE_First4Mask\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022NDLWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                      , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022NDLWFE_First8Mask\\u0022, \\u0022NDLWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                      , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                      , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.JustUnnamed(), "\"{0,-25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"9223372028264841216      \""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u00229223372028264841216      \\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"9223372028264841216      \""
                }
            }

            // Nullable No Default With Flags Long Enum
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(null)
            {
                { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"NDLWFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDLWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLWFE_First4Mask\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022NDLWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022NDLWFE_First8Mask\\u0022, \\u0022NDLWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                    "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.JustUnnamed(), "\"{0,25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultLongWithFlagsEnum.      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"      9223372028264841216\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022      9223372028264841216\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"      9223372028264841216\""
                }
            }


            // No Default With Flags ULong Enum
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDUWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDUWFE_1\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDUWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDUWFE_First4Mask\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022NDUWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\", \"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022NDUWFE_First8Mask\\u0022, \\u0022NDUWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\", \"NDUWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                       , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"NDUWFE_4, NDUWFE_Second4Mask, NDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDUWFE_4, NDUWFE_Second4Mask, NDUWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum>(NoDefaultULongWithFlagsEnum.NDUWFE_1.JustUnnamed(), "\"{0,-25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"9223372028264841216      \""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u00229223372028264841216      \\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"9223372028264841216      \""
                }
            }


            // Nullable No Default With Flags ULong Enum
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(null)
            {
                { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(ContentType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.0\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDUWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"NDUWFE_1\"" }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"NDUWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDUWFE_First4Mask\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022NDUWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\", \"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022NDUWFE_First8Mask\\u0022, \\u0022NDUWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.[\"NDUWFE_First8Mask\" | NoDefaultULongWithFlagsEnum.\"NDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"NDUWFE_First8Mask\", \"NDUWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.'NDUWFE_1 | NoDefaultULongWithFlagsEnum.NDUWFE_3 | NoDefaultULongWithFlagsEnum.NDUWFE_4 | " +
                    "NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'NDUWFE_1, NDUWFE_3, NDUWFE_4, NDUWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"NDUWFE_4, NDUWFE_Second4Mask, NDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.NDUWFE_4 |·NoDefaultULongWithFlagsEnum.NDUWFE_Second4Mask |·NoDefaultULongWithFlagsEnum.NDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"NDUWFE_4, NDUWFE_Second4Mask, NDUWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<NoDefaultULongWithFlagsEnum?>(NoDefaultULongWithFlagsEnum.NDUWFE_1.JustUnnamed(), "\"{0,25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"NoDefaultULongWithFlagsEnum.      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"      9223372028264841216\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022      9223372028264841216\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "NoDefaultULongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"      9223372028264841216\""
                }
            }


            // With Default With Flags Long Enum
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.Default(), "")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.Default"
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDLWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDLWFE_1\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDLWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDLWFE_First4Mask\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022WDLWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\", \"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022WDLWFE_First8Mask\\u0022, \\u0022WDLWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\", \"WDLWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                        , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum>(WithDefaultLongWithFlagsEnum.WDLWFE_1.JustUnnamed(), "\"{0,-25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"9223372028264841216      \""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u00229223372028264841216      \\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"9223372028264841216      \""
                }
            }

            // Nullable No Default With Flags Long Enum
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.Default(), "")
          {
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultLongWithFlagsEnum.Default"
            }
           ,
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
            , "\"WithDefaultLongWithFlagsEnum.Default\""
            }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
           ,
            {
              new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultLongWithFlagsEnum.Default"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
          }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(null)
            {
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
                  , "Default"
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
              , { new EK(ContentType | CallsViaMatch), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.Default\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable), "null" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDLWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"WDLWFE_1\"" }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDLWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDLWFE_First4Mask\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022WDLWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\", \"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022WDLWFE_First8Mask\\u0022, \\u0022WDLWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.[\"WDLWFE_First8Mask\" | WithDefaultLongWithFlagsEnum.\"WDLWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDLWFE_First8Mask\", \"WDLWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.'WDLWFE_1 | WithDefaultLongWithFlagsEnum.WDLWFE_3 | WithDefaultLongWithFlagsEnum.WDLWFE_4 | " +
                    "WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'WDLWFE_1, WDLWFE_3, WDLWFE_4, WDLWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.WDLWFE_4 |·WithDefaultLongWithFlagsEnum.WDLWFE_Second4Mask |·WithDefaultLongWithFlagsEnum.WDLWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDLWFE_4, WDLWFE_Second4Mask, WDLWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<WithDefaultLongWithFlagsEnum?>(WithDefaultLongWithFlagsEnum.WDLWFE_1.JustUnnamed(), "\"{0,25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultLongWithFlagsEnum.      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"      9223372028264841216\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022      9223372028264841216\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultLongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"      9223372028264841216\""
                }
            }

            // With Default With Flags Long Enum
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.Default(), "")
          {
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultULongWithFlagsEnum.Default"
            }
           ,
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
            , "\"WithDefaultULongWithFlagsEnum.Default\""
            }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
           ,
            {
              new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultULongWithFlagsEnum.Default"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
          }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDUWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDUWFE_1\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDUWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDUWFE_First4Mask\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022WDUWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\", \"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022WDUWFE_First8Mask\\u0022, \\u0022WDUWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\", \"WDUWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                         , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"WDUWFE_4, WDUWFE_Second4Mask, WDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDUWFE_4, WDUWFE_Second4Mask, WDUWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum>(WithDefaultULongWithFlagsEnum.WDUWFE_1.JustUnnamed(), "\"{0,-25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"9223372028264841216      \""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"9223372028264841216      \""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u00229223372028264841216      \\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.\"9223372028264841216      \""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"9223372028264841216      \""
                }
            }


            // Nullable No Default With Flags ULong Enum
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.Default(), "")
          {
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultULongWithFlagsEnum.Default"
            }
           ,
            {
              new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
            , "\"WithDefaultULongWithFlagsEnum.Default\""
            }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"Default\"" }
           ,
            {
              new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
            , "WithDefaultULongWithFlagsEnum.Default"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
          }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(null)
            {
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
                  , "Default"
                }
              , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
              , { new EK(ContentType | CallsViaMatch), "null" }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "Default"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.Default\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"Default\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(ContentType | AcceptsSpanFormattable), "null" }
              , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_1"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_1\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDUWFE_1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_1"
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty), "\"WDUWFE_1\"" }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First4Mask())
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"WDUWFE_First4Mask\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_First4Mask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDUWFE_First4Mask\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8Mask(), "[\"{0}\"]")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\"]"
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"[\\u0022WDUWFE_First8Mask\\u0022]\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\"]"
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                          , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\", \"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                  , "\"[\\u0022WDUWFE_First8Mask\\u0022, \\u0022WDUWFE_LastTwoMask\\u0022]\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.[\"WDUWFE_First8Mask\" | WithDefaultULongWithFlagsEnum.\"WDUWFE_LastTwoMask\"]"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "[\"WDUWFE_First8Mask\", \"WDUWFE_LastTwoMask\"]"
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                          , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"'WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask'\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.'WDUWFE_1 | WithDefaultULongWithFlagsEnum.WDUWFE_3 | WithDefaultULongWithFlagsEnum.WDUWFE_4 | " +
                    "WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"'WDUWFE_1, WDUWFE_3, WDUWFE_4, WDUWFE_Second4Mask'\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                          , formatFlags: ReformatMultiLine)
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable, CompactJson | Pretty)
                  , "\"WDUWFE_4, WDUWFE_Second4Mask, WDUWFE_LastTwoMask\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.WDUWFE_4 |·WithDefaultULongWithFlagsEnum.WDUWFE_Second4Mask |·WithDefaultULongWithFlagsEnum.WDUWFE_LastTwoMask"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"WDUWFE_4, WDUWFE_Second4Mask, WDUWFE_LastTwoMask\""
                }
            }
          , new FieldExpect<WithDefaultULongWithFlagsEnum?>(WithDefaultULongWithFlagsEnum.WDUWFE_1.JustUnnamed(), "\"{0,25}\"")
            {
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                  , "\"WithDefaultULongWithFlagsEnum.      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"      9223372028264841216\""
                }
               ,
                {
                    new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                  , "\"      9223372028264841216\""
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"\\u0022      9223372028264841216\\u0022\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                  , "WithDefaultULongWithFlagsEnum.\"      9223372028264841216\""
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                  , "\"      9223372028264841216\""
                }
            }
        };
}
