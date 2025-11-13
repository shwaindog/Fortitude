// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;

public class StringCollectionsTestData
{

    private static PositionUpdatingList<IOrderedListExpect>? allStringCollectionExpectations;

    public static PositionUpdatingList<IOrderedListExpect> AllStringCollectionExpectations => allStringCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(StringCollectionsTestData))
        {
            
            // string Collections 
            new OrderedListExpect<string>([],  "")
            {
                { new EK(  OrderedCollectionType | AcceptsChars), "[]" }
              , { new EK(   AcceptsChars | AlwaysWrites | NonNullWrites, CompactLog), "[]" }
              , { new EK(   AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactLog), "[]" }
              , { new EK( CollectionCardinality  | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                       , CompactJson), "[]" }
              , { new EK( CollectionCardinality  | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites | NonNullWrites
                       , Pretty), "[]" }
            }
          , new OrderedListExpect<string>(null,  "")
            {
                { new EK( OrderedCollectionType | AcceptsChars | AlwaysWrites), "[]" }
              , { new EK(  AcceptsChars | AlwaysWrites), "null" }
              , { new EK(  AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan |  AlwaysWrites, CompactLog), "[]" }
              , { new EK(  AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites, CompactJson), "null" }
              , { new EK(  AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan  | AlwaysWrites, Pretty), "null" }
            }
          , new OrderedListExpect<string>(StringList,  "")
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                     It began with the forging of the Great Strings.,
                     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                     Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                     And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                     For within these strings was bound the flexibility, mutability and the operators to govern each language,
                     But they were all of them deceived, for another string was made.,
                     Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, 
                     and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                     One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                     <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                      It began with the forging of the Great Strings.,
                      Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                      Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                      And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                      For within these strings was bound the flexibility, mutability and the operators to govern each language,
                      But they were all of them deceived, for another string was made.,
                      Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                      One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                      <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "<Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, 
                     and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "<ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "<Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "<ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList,  "'{0,100}'", () => String_LenLt_100)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     '                               <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>',
                     '                                                     It began with the forging of the Great Strings.',
                     '     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.',
                     '                                    But they were all of them deceived, for another string was made.',
                     '                              <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      '                               <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>',
                      '                                                    It began with the forging of the Great Strings.',
                      '    Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.',
                      '                                   But they were all of them deceived, for another string was made.',
                      '                             <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "'                               <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>'",
                    "'                                                    It began with the forging of the Great Strings.'",
                    "'    Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                    "'                                   But they were all of them deceived, for another string was made.'",
                    "'                             <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "'                               <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>'",
                      "'                                                    It began with the forging of the Great Strings.'",
                      "'    Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                      "'                                   But they were all of them deceived, for another string was made.'",
                      "'                             <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'"
                    ]
                    """.Dos2Unix()
                }
            }
        };
}
