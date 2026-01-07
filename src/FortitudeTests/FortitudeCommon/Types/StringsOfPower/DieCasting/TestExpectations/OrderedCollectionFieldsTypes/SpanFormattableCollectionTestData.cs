// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved


namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

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
