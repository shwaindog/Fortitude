// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class BoolTestData
{
    public static readonly IFormatExpectation[] AllBoolExpectations =
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
            {
                new EK(AcceptsStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"true      \""
            }
           ,
            { new EK( SimpleType | AcceptsStruct | DefaultTreatedAsStringOut) , "\"\"true      \"\"" }
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
      , new FieldExpect<bool?>(null, "null", true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
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
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut) , "\"true      \""
            }
           ,
            { new EK(SimpleType | AcceptsNullableStruct  | DefaultTreatedAsStringOut) , "\"\"true      \"\"" }
        }
    ];
}
