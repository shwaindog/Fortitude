// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class BoolTestData
{

    public static readonly IFormatExpectation[] AllBoolExpectations =
    [
        // bool
        new FieldExpect<bool>(false, "") { { AcceptsStruct | AlwaysWrites | NonNullWrites, "false" } }
      , new FieldExpect<bool>(true) { { AcceptsStruct |  AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "true" } }
      , new FieldExpect<bool>( false, "'{0}'", true, true) 
            { { AcceptsStruct |  AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'false'" } }
      , new FieldExpect<bool>(true, "\"{0,-10}\"")
        {
            { AcceptsStruct |  AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"true      \"" }
        }

        // bool?
      , new NullableStructFieldExpect<bool>(false, "{0}") { { AcceptsNullableStruct | AlwaysWrites | NonNullWrites, "false" } }
      , new NullableStructFieldExpect<bool>(null, "null", true) { { AcceptsNullableStruct |  AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullableStructFieldExpect<bool>( false, "'{0}'", true, true) 
            { { AcceptsNullableStruct |  AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'false'" } }
      , new NullableStructFieldExpect<bool>(true, "\"{0,-10}\"")
        {
            { AcceptsNullableStruct |  AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"true      \"" }
        }
    ];
}
