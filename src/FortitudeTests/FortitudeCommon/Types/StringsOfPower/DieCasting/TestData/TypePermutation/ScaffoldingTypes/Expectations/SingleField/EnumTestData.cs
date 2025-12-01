// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
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
            // No Default No Flags Enum
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
            
            // Nullable No Default No Flags Enum
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
            , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue, CompactLog | Pretty), "NoDefaultLongNoFlagsEnum.0" }
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
        };
}
