// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Options;
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
            { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites), "false" }
        }
      , new FieldExpect<bool>(true, "", true, true)
        {
            { new EK(AcceptsStruct | AlwaysWrites | NonNullWrites), "true" }
        }
      , new FieldExpect<bool>(true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "true"
            }
        }
      , new FieldExpect<bool>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Log | StringStyle.Compact | StringStyle.Pretty)
              , "'false'"
            }
           ,
            {
                new EK(AcceptsStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Json | StringStyle.Compact | StringStyle.Pretty)
              , "'false'"
            }
        }
      , new FieldExpect<bool>(true, "\"{0,-10}\"")
        {
            {
                new EK(AcceptsStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Log | StringStyle.Compact | StringStyle.Pretty)
              , "\"true      \""
            }
           ,
            {
                new EK(AcceptsStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Json | StringStyle.Compact | StringStyle.Pretty)
              , "\"true      \""
            }
        }

        // bool?
      , new FieldExpect<bool?>(false, "{0}")
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites), "false" }
        }
      , new FieldExpect<bool?>(true, "{0}", true, true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonNullWrites), "true" }
        }
      , new FieldExpect<bool?>(null, "null", true)
        {
            { new EK(AcceptsNullableStruct | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<bool?>(false, "'{0}'", true, true)
        {
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Log | StringStyle.Compact | StringStyle.Pretty)
              , "'false'"
            }
           ,
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Json | StringStyle.Compact | StringStyle.Pretty)
              , "'false'"
            }
        }
      , new FieldExpect<bool?>(true, "\"{0,-10}\"")
        {
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Log | StringStyle.Compact | StringStyle.Pretty)
              , "\"true      \""
            }
           ,
            {
                new EK(AcceptsNullableStruct | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , StringStyle.Json | StringStyle.Compact | StringStyle.Pretty)
              , "\"true      \""
            }
        }
    ];
}
