// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

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
            ,{ new EK(IsContentType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"true\"" }
        }
      , new FieldExpect<bool>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'false'"
            }
           ,
            { new EK(IsContentType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool>(true, "\"{0,-10}\"")
        {
            { new EK(IsContentType | AcceptsStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(IsContentType | AcceptsStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(IsContentType | AcceptsStruct) , 
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
           ,{ new EK(IsContentType | AcceptsNullableStruct  | DefaultTreatedAsStringOut), "\"false\"" }
        }
      , new FieldExpect<bool?>(true, "{0}", true, true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "true" }
           , { new EK(IsContentType | AcceptsNullableStruct | DefaultTreatedAsStringOut), "\"true\"" }
        }
      , new FieldExpect<bool?>(null, "{0}",true, false)
        {
            { new EK( IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
           ,{ new EK( IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "false" }
           ,{ new EK( IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"false\"" }
           ,{ new EK( IsContentType | CallsViaMatch), "null" }
          , { new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<bool?>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "'false'"
            }
           ,
            { new EK(IsContentType | AcceptsNullableStruct | DefaultTreatedAsStringOut) , "\"'false'\"" }
        }
      , new FieldExpect<bool?>(true, "\"{0,-10}\"")
        {
            { new EK(IsContentType | AcceptsNullableStruct, Log | Compact | Pretty) , "\"true      \"" }
          , { new EK(IsContentType | AcceptsNullableStruct | DefaultTreatedAsValueOut) , "\"true      \"" }
          , { new EK(IsContentType | AcceptsNullableStruct) , 
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
