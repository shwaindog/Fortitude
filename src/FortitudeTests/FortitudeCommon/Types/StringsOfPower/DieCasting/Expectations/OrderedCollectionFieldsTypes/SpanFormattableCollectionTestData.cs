// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.OrderedCollectionFieldsTypes;

public class SpanFormattableCollectionTestData
{
    public static readonly Lazy<List<IOrderedListExpect>> AllSpanFormattableCollectionExpectations =
        new(() =>
        [
            ..NumberCollectionsTestData.AllNumberCollectionsExpectations
          , ..SpanFormattableClassCollectionTestData.SpanFormattableClassCollectionsExpectations
          , ..EnumCollectionsTestData.AllEnumCollectionsExpectations
        ]);
    
}
