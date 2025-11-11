// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class SpanFormattableCollectionTestData
{
        public static readonly Lazy<IOrderedListExpect[]> AllSpanFormattableCollectionExpectations = new (() =>
    [
        // bool Collections
        new OrderedListExpect<bool>([],  "")
        {
            { new EK( CollectionCardinality | AcceptsStruct | CallsAsSpan | CallsAsReadOnlySpan |  AllOutputConditionsMask
                   , CompactLog), "[]" }
          , { new EK(OrderedCollectionType | CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, CompactJson), "[]" }
          , { new EK(OrderedCollectionType | CollectionCardinality  | AcceptsStruct | AllOutputConditionsMask, Pretty), "[]" 
            }
        }
    ]);
        
}
