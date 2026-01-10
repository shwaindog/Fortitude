// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestCollections;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

public class StringCollectionsTestData
{

    private static PositionUpdatingList<IOrderedListExpect>? allStringCollectionExpectations;

    public static PositionUpdatingList<IOrderedListExpect> AllStringCollectionExpectations => allStringCollectionExpectations ??=
        new PositionUpdatingList<IOrderedListExpect>(typeof(StringCollectionsTestData))
        {
            
            // string Collections 
            new OrderedListExpect<string>([],  "", name: "Empty")
            {
                { new EK(   IsOrderedCollectionType | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
               ,{ new EK(   AcceptsChars | AlwaysWrites | NonNullWrites), "[]" }
               ,{ new EK(   AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<string>(null,  "")
            {
                { new EK( IsOrderedCollectionType | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsChars | AlwaysWrites), "null" }
              , { new EK(AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<string>(StringList,  "", name: "All_NoFilter")
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
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287
                    \u1d09\u2534\u003e"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList, name: "All_NullFmtString")
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
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287
                    \u1d09\u2534\u003e"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList,  "{0[..50]}", name: "All_LenCapped50")
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     <Title author="A RR Token">Origin of the 𝄞trings ,
                     It began with the forging of the Great Strings.,
                     Three were given to the Assembly Programmers, impr,
                     Seven to the Cobol-Lords, eventually great Bitcoin,
                     And nine, nine strings were gifted to the race of ,
                     For within these strings was bound the flexibility,
                     But they were all of them deceived, for another st,
                     Deep in the land of Redmond, after many Moons of p,
                     One string to use in all, one string to find text ,
                     <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ 
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      <Title author="A RR Token">Origin of the 𝄞trings ,
                      It began with the forging of the Great Strings.,
                      Three were given to the Assembly Programmers, impr,
                      Seven to the Cobol-Lords, eventually great Bitcoin,
                      And nine, nine strings were gifted to the race of ,
                      For within these strings was bound the flexibility,
                      But they were all of them deceived, for another st,
                      Deep in the land of Redmond, after many Moons of p,
                      One string to use in all, one string to find text ,
                      <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ 
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings ",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impr",
                    "Seven to the Cobol-Lords, eventually great Bitcoin",
                    "And nine, nine strings were gifted to the race of ",
                    "For within these strings was bound the flexibility",
                    "But they were all of them deceived, for another st",
                    "Deep in the land of Redmond, after many Moons of p",
                    "One string to use in all, one string to find text ",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 "
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings ",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impr",
                      "Seven to the Cobol-Lords, eventually great Bitcoin",
                      "And nine, nine strings were gifted to the race of ",
                      "For within these strings was bound the flexibility",
                      "But they were all of them deceived, for another st",
                      "Deep in the land of Redmond, after many Moons of p",
                      "One string to use in all, one string to find text ",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 "
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
                      '                                                     It began with the forging of the Great Strings.',
                      '     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.',
                      '                                    But they were all of them deceived, for another string was made.',
                      '                              <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "'                               \u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin
                     offs\u003c/Title\u003e'",
                    "'                                                     It began with the forging of the Great Strings.'",
                    "'     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                    "'                                    But they were all of them deceived, for another string was made.'",
                    "'                              \u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b
                     s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279
                     \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e'"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "'                               \u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e'",
                      "'                                                     It began with the forging of the Great Strings.'",
                      "'     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                      "'                                    But they were all of them deceived, for another string was made.'",
                      "'                              \u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e'"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList,  "{0,0/ /_/}", () => String_First_5)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     <Title_author="A_RR_Token">Origin_of_the_𝄞trings_&_spin_offs</Title>,
                     It_began_with_the_forging_of_the_Great_Strings.,
                     Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.,
                     Seven_to_the_Cobol-Lords,_eventually_great_Bitcoin_miners_and_great_cardigan_wearers_of_the_mainframe_halls.,
                     And_nine,_nine_strings_were_gifted_to_the_race_of_C++_coders,_who_above_all_else_desired_unchecked_memory_access_power. 
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      <Title_author="A_RR_Token">Origin_of_the_𝄞trings_&_spin_offs</Title>,
                      It_began_with_the_forging_of_the_Great_Strings.,
                      Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.,
                      Seven_to_the_Cobol-Lords,_eventually_great_Bitcoin_miners_and_great_cardigan_wearers_of_the_mainframe_halls.,
                      And_nine,_nine_strings_were_gifted_to_the_race_of_C++_coders,_who_above_all_else_desired_unchecked_memory_access_power. 
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "\u003cTitle_author=\u0022A_RR_Token\u0022\u003eOrigin_of_the_\ud834\udd1etrings_&_spin_offs\u003c/Title\u003e",
                    "It_began_with_the_forging_of_the_Great_Strings.",
                    "Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.",
                    "Seven_to_the_Cobol-Lords,_eventually_great_Bitcoin_miners_and_great_cardigan_wearers_of_the_mainframe_halls.",
                    "And_nine,_nine_strings_were_gifted_to_the_race_of_C++_coders,_who_above_all_else_desired_unchecked_memory_access_power. "
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "\u003cTitle_author=\u0022A_RR_Token\u0022\u003eOrigin_of_the_\ud834\udd1etrings_&_spin_offs\u003c/Title\u003e",
                      "It_began_with_the_forging_of_the_Great_Strings.",
                      "Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.",
                      "Seven_to_the_Cobol-Lords,_eventually_great_Bitcoin_miners_and_great_cardigan_wearers_of_the_mainframe_halls.",
                      "And_nine,_nine_strings_were_gifted_to_the_race_of_C++_coders,_who_above_all_else_desired_unchecked_memory_access_power. "
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList,  "", () => String_Second_5)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
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
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo
                     u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250
                     \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string>(StringList,  "", () => String_Skip_Odd_Index)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                     And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                     But they were all of them deceived, for another string was made.,
                     One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                      Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                      And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                      But they were all of them deceived, for another string was made.,
                      One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "But they were all of them deceived, for another string was made.",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "But they were all of them deceived, for another string was made.",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them"
                    ]
                    """.Dos2Unix()
                }
            }
            
            // string? (collections with null strings) 
          , new OrderedListExpect<string?>([],  "", name: "NullEmpty")
            {
                { new EK(   IsOrderedCollectionType | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan), "[]" }
               ,{ new EK(   AcceptsChars | AlwaysWrites | NonNullWrites), "[]" }
               ,{ new EK(   AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<string?>(null,  "")
            {
                { new EK( IsOrderedCollectionType | AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
              , { new EK(AcceptsChars | AlwaysWrites), "null" }
              , { new EK(AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AlwaysWrites), "[]" }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "", name: "All_NullNoFilter")
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                     It began with the forging of the Great Strings.,
                     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                     null,
                     null,
                     Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                     And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                     For within these strings was bound the flexibility, mutability and the operators to govern each language,
                     But they were all of them deceived, for another string was made.,
                     Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, 
                     and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                     null,
                     One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                     <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                      It began with the forging of the Great Strings.,
                      Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                      null,
                      null,
                      Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                      And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                      For within these strings was bound the flexibility, mutability and the operators to govern each language,
                      But they were all of them deceived, for another string was made.,
                      Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                      null,
                      One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                      <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    null,
                    null,
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    null,
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl
                    \u0287\u1d09\u2534\u003e"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      null,
                      null,
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      null,
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value, name: "All_NullFmtString")
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                     It began with the forging of the Great Strings.,
                     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                     null,
                     null,
                     Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                     And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                     For within these strings was bound the flexibility, mutability and the operators to govern each language,
                     But they were all of them deceived, for another string was made.,
                     Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, 
                     and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                     null,
                     One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                     <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      <Title author="A RR Token">Origin of the 𝄞trings & spin offs</Title>,
                      It began with the forging of the Great Strings.,
                      Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.,
                      null,
                      null,
                      Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                      And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                      For within these strings was bound the flexibility, mutability and the operators to govern each language,
                      But they were all of them deceived, for another string was made.,
                      Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                      null,
                      One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them,
                      <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                    null,
                    null,
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made.",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    null,
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl
                    \u0287\u1d09\u2534\u003e"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.",
                      null,
                      null,
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made.",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      null,
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "{0[..50]}", name: "All_NullLenCapped50")
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     <Title author="A RR Token">Origin of the 𝄞trings ,
                     It began with the forging of the Great Strings.,
                     Three were given to the Assembly Programmers, impr,
                     null,
                     null,
                     Seven to the Cobol-Lords, eventually great Bitcoin,
                     And nine, nine strings were gifted to the race of ,
                     For within these strings was bound the flexibility,
                     But they were all of them deceived, for another st,
                     Deep in the land of Redmond, after many Moons of p,
                     null,
                     One string to use in all, one string to find text ,
                     <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ 
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      <Title author="A RR Token">Origin of the 𝄞trings ,
                      It began with the forging of the Great Strings.,
                      Three were given to the Assembly Programmers, impr,
                      null,
                      null,
                      Seven to the Cobol-Lords, eventually great Bitcoin,
                      And nine, nine strings were gifted to the race of ,
                      For within these strings was bound the flexibility,
                      But they were all of them deceived, for another st,
                      Deep in the land of Redmond, after many Moons of p,
                      null,
                      One string to use in all, one string to find text ,
                      <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ 
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings ",
                    "It began with the forging of the Great Strings.",
                    "Three were given to the Assembly Programmers, impr",null,null,
                    "Seven to the Cobol-Lords, eventually great Bitcoin",
                    "And nine, nine strings were gifted to the race of ",
                    "For within these strings was bound the flexibility",
                    "But they were all of them deceived, for another st",
                    "Deep in the land of Redmond, after many Moons of p",
                    null,
                    "One string to use in all, one string to find text ",
                    "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287
                     \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 "
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "\u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings ",
                      "It began with the forging of the Great Strings.",
                      "Three were given to the Assembly Programmers, impr",
                      null,
                      null,
                      "Seven to the Cobol-Lords, eventually great Bitcoin",
                      "And nine, nine strings were gifted to the race of ",
                      "For within these strings was bound the flexibility",
                      "But they were all of them deceived, for another st",
                      "Deep in the land of Redmond, after many Moons of p",
                      null,
                      "One string to use in all, one string to find text ",
                      "\u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 "
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "'{0,100}'", () => NullString_LenLt_100)
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
                      '                                                     It began with the forging of the Great Strings.',
                      '     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.',
                      '                                    But they were all of them deceived, for another string was made.',
                      '                              <ǝlʇᴉ┴/>sɟɟo uᴉds ⅋ sƃuᴉɹʇS ǝɥʇ ɟo uᴉƃᴉɹO<,,uǝʞo┴ ɹɹ ∀,,=ɹoɥʇnɐ ǝlʇᴉ┴>'
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    "'                               \u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin
                     offs\u003c/Title\u003e'",
                    "'                                                     It began with the forging of the Great Strings.'",
                    "'     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                    "'                                    But they were all of them deceived, for another string was made.'",
                    "'                              \u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b
                     s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279
                     \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e'"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      "'                               \u003cTitle author=\u0022A RR Token\u0022\u003eOrigin of the \ud834\udd1etrings & spin offs\u003c/Title\u003e'",
                      "'                                                     It began with the forging of the Great Strings.'",
                      "'     Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.'",
                      "'                                    But they were all of them deceived, for another string was made.'",
                      "'                              \u003c\u01ddl\u0287\u1d09\u2534/\u003es\u025f\u025fo u\u1d09ds \u214b s\u0183u\u1d09\u0279\u0287S \u01dd\u0265\u0287 \u025fo u\u1d09\u0183\u1d09\u0279O\u003c,,u\u01dd\u029eo\u2534 \u0279\u0279 \u2200,,=\u0279o\u0265\u0287n\u0250 \u01ddl\u0287\u1d09\u2534\u003e'"
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "{0,0/ /_/}", () => NullString_First_5)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     <Title_author="A_RR_Token">Origin_of_the_𝄞trings_&_spin_offs</Title>,
                     It_began_with_the_forging_of_the_Great_Strings.,
                     Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.,
                     null
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      <Title_author="A_RR_Token">Origin_of_the_𝄞trings_&_spin_offs</Title>,
                      It_began_with_the_forging_of_the_Great_Strings.,
                      Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.,
                      null
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "\u003cTitle_author=\u0022A_RR_Token\u0022\u003eOrigin_of_the_\ud834\udd1etrings_&_spin_offs\u003c/Title\u003e",
                    "It_began_with_the_forging_of_the_Great_Strings.",
                    "Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.",
                    null
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "\u003cTitle_author=\u0022A_RR_Token\u0022\u003eOrigin_of_the_\ud834\udd1etrings_&_spin_offs\u003c/Title\u003e",
                      "It_began_with_the_forging_of_the_Great_Strings.",
                      "Three_were_given_to_the_Assembly_Programmers,_impractical,_wackiest_and_hairiest_of_all_beings.",
                      null
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "", () => NullString_Second_5)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                     And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                     For within these strings was bound the flexibility, mutability and the operators to govern each language,
                     But they were all of them deceived, for another string was made.
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                      And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ,
                      For within these strings was bound the flexibility, mutability and the operators to govern each language,
                      But they were all of them deceived, for another string was made.
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "But they were all of them deceived, for another string was made."
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. ",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "But they were all of them deceived, for another string was made."
                    ]
                    """.Dos2Unix()
                }
            }
          , new OrderedListExpect<string?>(NullStringList.Value,  "", () => NullString_Skip_Odd_Index)
            {
                { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactLog), 
                    """
                    [
                     null,
                     It began with the forging of the Great Strings.,
                     null,
                     Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                     For within these strings was bound the flexibility, mutability and the operators to govern each language,
                     Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String, 
                     and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                     One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them
                     ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyLog), 
                    """
                    [
                      null,
                      It began with the forging of the Great Strings.,
                      null,
                      Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.,
                      For within these strings was bound the flexibility, mutability and the operators to govern each language,
                      Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ,
                      One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them
                    ]
                    """.Dos2Unix()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, CompactJson), 
                    """
                    [
                    null,
                    "It began with the forging of the Great Strings.",
                    null,
                    "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                    "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                    "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,
                      and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                    "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them"
                    ]
                    """.RemoveLineEndings()
                }
              , { new EK( AcceptsChars | CallsAsSpan | CallsAsReadOnlySpan | AllOutputConditionsMask, PrettyJson), 
                    """
                    [
                      null,
                      "It began with the forging of the Great Strings.",
                      null,
                      "Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.",
                      "For within these strings was bound the flexibility, mutability and the operators to govern each language",
                      "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master String,  and into this string he poured his unambiguity, his immutability desires and his will to replace all ",
                      "One string to use in all, one string to find text in, One string to replace them all and in the dustbins of time confine them"
                    ]
                    """.Dos2Unix()
                }
            }
        };
}
