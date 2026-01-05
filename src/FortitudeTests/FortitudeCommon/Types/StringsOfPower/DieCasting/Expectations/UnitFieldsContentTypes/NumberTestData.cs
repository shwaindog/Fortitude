// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public static class NumberTestData
{
    public static Lazy<List<ISingleFieldExpectation>> AllNumberExpectations =
        new(() =>
            [
                ..UnsignedIntegerTestData.UnsignedIntegerExpectations
              , ..SignedIntegerTestData.SignedIntegerExpectations
              , ..DecimalNumberTestData.DecimalNumberExpectations
            ]
           );
}
