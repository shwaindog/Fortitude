// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
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
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongNoFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongNoFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnum.NDLNFE_1)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.NDLNFE_1" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongNoFlagsEnum.NDLNFE_1\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLNFE_1\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.NDLNFE_1" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "\"NDLNFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum>(NoDefaultLongNoFlagsEnum.NDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongNoFlagsEnum.     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                , "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
                , "\"     8589934592\"" }
            }
            
            // Nullable No Default No Flags Long Enum
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1.Default(), "")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongNoFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(null)
            {
              { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
            , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultLongNoFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultLongNoFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                     , CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "\"NoDefaultLongNoFlagsEnum.0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
            , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites) , "null" }
            , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites) , "null" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.NDLNFE_1" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongNoFlagsEnum.NDLNFE_1\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLNFE_1\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongNoFlagsEnum.NDLNFE_1" 
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"NDLNFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongNoFlagsEnum?>(NoDefaultLongNoFlagsEnum.NDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
              { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "NoDefaultLongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongNoFlagsEnum.     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
              , "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
              , "\"     8589934592\"" }
            }
            
            // No Default No Flags ULong Enum
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), "")
            {
                { 
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultULongNoFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultULongNoFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_4, "D")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.4" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultULongNoFlagsEnum.4\"" 
              }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "4" }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"4\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "NoDefaultULongNoFlagsEnum.4" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "4" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum>(NoDefaultULongNoFlagsEnum.NDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultULongNoFlagsEnum.     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                , "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
                , "\"     8589934592\"" }
            }
            
            // Nullable No Default No Flags ULong Enum
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1.Default(), "")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultULongNoFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(null)
            {
              { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
            , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultULongNoFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultULongNoFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                     , CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "\"NoDefaultULongNoFlagsEnum.0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
            , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites) , "null" }
            , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites) , "null" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.NDUNFE_1" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultULongNoFlagsEnum.NDUNFE_1\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDUNFE_1\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultULongNoFlagsEnum.NDUNFE_1" 
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"NDUNFE_1\"" }
            }
          , new FieldExpect<NoDefaultULongNoFlagsEnum?>(NoDefaultULongNoFlagsEnum.NDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
              { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "NoDefaultULongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultULongNoFlagsEnum.     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
              , "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultULongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
              , "\"     8589934592\"" }
            }
            
            // With Default No Flags Long Enum
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_1.Default(), "")
            {
                { 
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultLongNoFlagsEnum.Default" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultLongNoFlagsEnum.Default\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"Default\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.Default" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_11, "0x{0[^1..]:X}")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.0xB" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultLongNoFlagsEnum.0xB\"" 
              }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"0xB\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "WithDefaultLongNoFlagsEnum.0xB" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"0xB\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum>(WithDefaultLongNoFlagsEnum.WDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultLongNoFlagsEnum.     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                , "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
                , "\"     8589934592\"" }
            }
            
            // Nullable With Default No Flags Long Enum
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_1.Default(), "")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.Default" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultLongNoFlagsEnum.Default\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"Default\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.Default" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(null)
            {
              { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
              , "WithDefaultLongNoFlagsEnum.Default" }
            , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
              , "Default" }
            , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
            , { new EK(SimpleType | CallsViaMatch), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "WithDefaultLongNoFlagsEnum.Default" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , CompactLog | Pretty) , "Default" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                       , CompactLog | Pretty) , "\"WithDefaultLongNoFlagsEnum.Default\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString)
              , "\"Default\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "null" }
            , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites) , "null" }
            , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites) , "null" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_7, "F")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.WDLNFE_7" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultLongNoFlagsEnum.WDLNFE_7\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"WDLNFE_7\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "WithDefaultLongNoFlagsEnum.WDLNFE_7" 
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"WDLNFE_7\"" }
            }
          , new FieldExpect<WithDefaultLongNoFlagsEnum?>(WithDefaultLongNoFlagsEnum.WDLNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
              { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "WithDefaultLongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"WithDefaultLongNoFlagsEnum.     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
              , "\"     8589934592\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "WithDefaultLongNoFlagsEnum.\"     8589934592\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
              , "\"     8589934592\"" }
            }
            
            // With Default No Flags ULong Enum
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_1.Default(), "")
            {
                { 
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "WithDefaultULongNoFlagsEnum.Default" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultULongNoFlagsEnum.Default\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"Default\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.Default" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_11, "0x{0[^1..]:X}")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.0xB" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultULongNoFlagsEnum.0xB\"" 
              }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"0xB\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty), "WithDefaultULongNoFlagsEnum.0xB" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"0xB\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum>(WithDefaultULongNoFlagsEnum.WDUNFE_1.JustUnnamed(), "\"{0,15}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultULongNoFlagsEnum.     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                , "\"     8589934592\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022     8589934592\\u0022\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.\"     8589934592\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
                , "\"     8589934592\"" }
            }
            
            // Nullable With Default No Flags ULong Enum
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_1.Default(), "")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.Default" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultULongNoFlagsEnum.Default\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"Default\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.Default" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "\"Default\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(null)
            {
              { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue, CompactLog | Pretty)
              , "WithDefaultULongNoFlagsEnum.Default" }
            , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty)
              , "Default" }
            , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackString), "\"Default\"" }
            , { new EK(SimpleType | CallsViaMatch), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "WithDefaultULongNoFlagsEnum.Default" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                       , CompactLog | Pretty) , "Default" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                       , CompactLog | Pretty) , "\"WithDefaultULongNoFlagsEnum.Default\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString)
              , "\"Default\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "null" }
            , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites) , "null" }
            , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites) , "null" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_7, "F")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.WDUNFE_7" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"WithDefaultULongNoFlagsEnum.WDUNFE_7\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"WDUNFE_7\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "WithDefaultULongNoFlagsEnum.WDUNFE_7" 
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"WDUNFE_7\"" }
            }
          , new FieldExpect<WithDefaultULongNoFlagsEnum?>(WithDefaultULongNoFlagsEnum.WDUNFE_1.JustUnnamed(), "\"{0,-15}\"")
            {
              { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "WithDefaultULongNoFlagsEnum.\"8589934592     \"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"WithDefaultULongNoFlagsEnum.8589934592     \"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "\"8589934592     \"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
              , "\"8589934592     \"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u00228589934592     \\u0022\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "WithDefaultULongNoFlagsEnum.\"8589934592     \"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
              , "\"8589934592     \"" }
            }
            
            // No Default With Flags Long Enum
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "")
            {
                { 
                    new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "NoDefaultLongWithFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongWithFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "0" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_1" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongWithFlagsEnum.NDLWFE_1\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLWFE_1\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_1" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "\"NDLWFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First4Mask())
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLWFE_First4Mask\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"  }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "\"NDLWFE_First4Mask\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\"]" }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"[\\u0022NDLWFE_First8Mask\\u0022]\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"  }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\"]" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                , "\"[\\u0022NDLWFE_First8Mask\\u0022, \\u0022NDLWFE_LastTwoMask\\u0022]\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"   }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                  "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'\""
              }
              , { new EK(SimpleType | AcceptsSpanFormattable, CompactJson | Pretty)
              , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"   
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
              , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable, CompactJson | Pretty)
              , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"   
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
              , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum>(NoDefaultLongWithFlagsEnum.NDLWFE_1.JustUnnamed(), "\"{0,-25}\"")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.\"9223372028264841216      \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongWithFlagsEnum.9223372028264841216      \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "\"9223372028264841216      \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
                , "\"9223372028264841216      \"" }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u00229223372028264841216      \\u0022\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.\"9223372028264841216      \"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
                , "\"9223372028264841216      \"" }
            }
            
            // Nullable No Default With Flags Long Enum
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.Default(), "")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.0" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongWithFlagsEnum.0\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "0" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.0" 
                }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(null)
            {
              { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "0" }
            , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultLongWithFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackString)
              , "\"0\"" }
            , { new EK(SimpleType | DefaultTreatedAsValueOut | DefaultBecomesFallbackString, CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "NoDefaultLongWithFlagsEnum.0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackString
                     , CompactLog | Pretty), "0" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "\"NoDefaultLongWithFlagsEnum.0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty), "\"0\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
            , { new EK(AcceptsSpanFormattable | CallsUsingObject | AlwaysWrites) , "null" }
            , { new EK(AcceptsSpanFormattable | NeverWhenCallingViaObject | AlwaysWrites | NonDefaultWrites) , "null" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_1" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
                , "\"NoDefaultLongWithFlagsEnum.NDLWFE_1\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLWFE_1\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_1" 
                }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) , "\"NDLWFE_1\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First4Mask())
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask\"" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"NDLWFE_First4Mask\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.NDLWFE_First4Mask"  }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "\"NDLWFE_First4Mask\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Mask(), "[\"{0}\"]")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\"]" }
              , { new EK(SimpleType | AcceptsSpanFormattable ), "\"[\\u0022NDLWFE_First8Mask\\u0022]\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\"]"  }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\"]" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8AndLast2Mask(), "[\"{0,/, /\", \"/}\"]"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]" }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactJson | Pretty)
                , "\"[\\u0022NDLWFE_First8Mask\\u0022, \\u0022NDLWFE_LastTwoMask\\u0022]\"" }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.[\"NDLWFE_First8Mask\" | NoDefaultLongWithFlagsEnum.\"NDLWFE_LastTwoMask\"]"   }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
                , "[\"NDLWFE_First8Mask\", \"NDLWFE_LastTwoMask\"]" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8MinusFlag2Mask(), "'{0}'"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                  "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'\""
              }
              , { new EK(SimpleType | AcceptsSpanFormattable, CompactJson | Pretty)
              , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\""
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.'NDLWFE_1 | NoDefaultLongWithFlagsEnum.NDLWFE_3 | NoDefaultLongWithFlagsEnum.NDLWFE_4 | " +
                "NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask'"   
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
              , "\"'NDLWFE_1, NDLWFE_3, NDLWFE_4, NDLWFE_Second4Mask'\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.First8Last2MaskMinusFlag1(), "{0,/, /, /[^3..]}"
                                                       ,  contentHandling: ReformatMultiLine)
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask\"" }
              , { new EK(SimpleType | AcceptsSpanFormattable, CompactJson | Pretty)
              , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\""
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.NDLWFE_4 |·NoDefaultLongWithFlagsEnum.NDLWFE_Second4Mask |·NoDefaultLongWithFlagsEnum.NDLWFE_LastTwoMask"   
              }
              , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty)
              , "\"NDLWFE_4, NDLWFE_Second4Mask, NDLWFE_LastTwoMask\"" }
            }
          , new FieldExpect<NoDefaultLongWithFlagsEnum?>(NoDefaultLongWithFlagsEnum.NDLWFE_1.JustUnnamed(), "\"{0,25}\"")
            {
              { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.\"      9223372028264841216\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut, CompactLog | Pretty)
              , "\"NoDefaultLongWithFlagsEnum.      9223372028264841216\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                     | DefaultBecomesFallbackString), "\"      9223372028264841216\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesZero)
              , "\"      9223372028264841216\"" }
            , { new EK(SimpleType | AcceptsSpanFormattable), "\"\\u0022      9223372028264841216\\u0022\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactLog | Pretty)
              , "NoDefaultLongWithFlagsEnum.\"      9223372028264841216\"" }
            , { new EK(AcceptsSpanFormattable | AllOutputConditionsMask, CompactJson | Pretty) 
              , "\"      9223372028264841216\"" }
            }
        };
}
