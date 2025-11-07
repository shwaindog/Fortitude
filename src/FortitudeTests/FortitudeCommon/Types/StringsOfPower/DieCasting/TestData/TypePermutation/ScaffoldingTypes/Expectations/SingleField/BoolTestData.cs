// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class BoolTestData
{
    public static readonly ISingleFieldExpectation[] AllBoolExpectations =
    [
        // bool
        new FieldExpect<bool>(false, "")
        {
            { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "false" }
            , { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"false\"" }
        }
      , new FieldExpect<bool>(true, "", true, true)
        {
            { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "true" }
            , { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"true\"" }
        }
      , new FieldExpect<bool>(true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "true"
            }
            ,{ new EK(SimpleType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"true\"" }
        }
      , new FieldExpect<bool>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'false'"
            }
           ,
            { new EK(SimpleType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool>(true, "\"{0,-10}\"")
        {
            { new EK(SimpleType | AcceptsStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(SimpleType | AcceptsStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(SimpleType | AcceptsStruct) , 
                """
                "\u0022true      \u0022"
                """
            }
          , {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"true      \""
            }
        }

        // bool?
      , new FieldExpect<bool?>(false, "{0}")
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "false" }
           ,{ new EK(SimpleType | AcceptsNullableStruct  | DefaultTreatedAsStringOut), "\"false\"" }
        }
      , new FieldExpect<bool?>(true, "{0}", true, true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "true" }
           , { new EK(SimpleType | AcceptsNullableStruct | DefaultTreatedAsStringOut), "\"true\"" }
        }
      , new FieldExpect<bool?>(null, "{0}",true, false)
        {
            { new EK( SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallback), "null" }
           ,{ new EK( SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallback), "false" }
           ,{ new EK( SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"false\"" }
           ,{ new EK( SimpleType | CallsViaMatch), "null" }
          , { new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<bool?>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "'false'"
            }
           ,
            { new EK(SimpleType | AcceptsNullableStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool?>(true, "\"{0,-10}\"")
        {
            { new EK(SimpleType | AcceptsNullableStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(SimpleType | AcceptsNullableStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(SimpleType | AcceptsNullableStruct) , 
                """
                "\u0022true      \u0022"
                """
            }
          , {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "\"true      \""
            }
        }
    ];
}
