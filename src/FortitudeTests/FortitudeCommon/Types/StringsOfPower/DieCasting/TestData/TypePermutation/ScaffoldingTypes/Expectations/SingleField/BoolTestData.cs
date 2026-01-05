// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class BoolTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? allBoolExpectations;  
    
    public static PositionUpdatingList<ISingleFieldExpectation> AllBoolExpectations => allBoolExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(BoolTestData))
            {
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
            ,{ new EK(ContentType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"true\"" }
        }
      , new FieldExpect<bool>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'false'"
            }
           ,
            { new EK(ContentType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool>(true, "\"{0,-10}\"")
        {
            { new EK(ContentType | AcceptsStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(ContentType | AcceptsStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(ContentType | AcceptsStruct) , 
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
           ,{ new EK(ContentType | AcceptsNullableStruct  | DefaultTreatedAsStringOut), "\"false\"" }
        }
      , new FieldExpect<bool?>(true, "{0}", true, true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "true" }
           , { new EK(ContentType | AcceptsNullableStruct | DefaultTreatedAsStringOut), "\"true\"" }
        }
      , new FieldExpect<bool?>(null, "{0}",true, false)
        {
            { new EK( ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
           ,{ new EK( ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "false" }
           ,{ new EK( ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"false\"" }
           ,{ new EK( ContentType | CallsViaMatch), "null" }
          , { new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<bool?>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "'false'"
            }
           ,
            { new EK(ContentType | AcceptsNullableStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool?>(true, "\"{0,-10}\"")
        {
            { new EK(ContentType | AcceptsNullableStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(ContentType | AcceptsNullableStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(ContentType | AcceptsNullableStruct) , 
                """
                "\u0022true      \u0022"
                """
            }
          , {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "\"true      \""
            }
        }
    };
}
