// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public static class StringTestData
{
    private static PositionUpdatingList<IStringLikeExpectation>? allStringLikeExpectations;

    public static PositionUpdatingList<IStringLikeExpectation> AllStringExpectations => allStringLikeExpectations ??=
        new PositionUpdatingList<IStringLikeExpectation>(typeof(StringTestData))
        {
            // string
            new StringLikeExpect<string>("", "", true, "0")
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | EmptyBecomesNull | DefaultBecomesNull)
                  , "null"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesEmpty)
                  , "\"\""
                }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut), "" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut)
                  , "\"\""
                }

               ,
                {
                    new EK(IsComplexType | AcceptsChars | AlwaysWrites | NonDefaultWrites | NonDefaultWrites | NonNullWrites
                         | NonNullAndPopulatedWrites)
                  , "\"\""
                }
              , { new EK(IsComplexType | AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new StringLikeExpect<string>(null, "", true, "")
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "" }
              , { new EK(IsContentType | CallsViaMatch), "null" }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsValueOut
                         | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString), "" }
            }
          , new StringLikeExpect<string>(null, "", false, "", 10, 50)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "null" },
            }
          , new StringLikeExpect<string>(null, "", true, "0", -1, -10)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "0" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null" },
            }
          , new StringLikeExpect<string>("It", "[{0}]", false, "0", 3, 2)
            {
                { new EK(AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue), "[0]" }
              , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "[]" }
              , { new EK(AcceptsChars | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallbackValue), "\"[0]\"" }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , """
                    "[]"
                    """
                }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , """
                    "[]"
                    """
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsStringOut)
                  , """
                    "[]"
                    """
                }
               ,
            }
          , new StringLikeExpect<string>("began", "[{0[8..10]}]", false, "0", 10, 5)
            {
                { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut), "[]" }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                  , """
                    "[]"
                    """
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsStringOut)
                  , """
                    "[]"
                    """
                }
            }
          , new StringLikeExpect<string>("with", "\"{0[8..10]}\"")
            {
                { new EK(IsContentType | CallsViaMatch | AcceptsString, Log | Compact | Pretty), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsString | CallsAsReadOnlySpan, Log | Compact | Pretty), "\"\"" }

              , { new EK(AcceptsChars | AcceptsString | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "\"\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsString | CallsAsReadOnlySpan)
                  , "\"\\u0022\\u0022\""
                }
               , { new EK(AcceptsChars | CallsAsReadOnlySpan | AllOutputConditionsMask) , "\"\"" }
            }
          , new StringLikeExpect<string>("the", "{0}", true, "", -1, -10)
            {
                { new EK(AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
              , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
              , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut)
                  , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsStringOut)
                  , "\"\""
                }
               ,
            }
          , new StringLikeExpect<string>("forging", "{0,10}", true, "orging", 1)
            {
                { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "    orging" }
              , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"    orging\"" }
            }
          , new StringLikeExpect<string>("It began with the forging of the Great Strings.", "[{0}]")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
          , new StringLikeExpect<string>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                                       , "3{0[5..]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                }
            }
          , new
                StringLikeExpect<string>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                                       , "'{0,30}'", fromIndex: -1, length: 24)
                {
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                      , "'      Seven to the Cobol-Lords'"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut)
                      , "\"'      Seven to the Cobol-Lords'\""
                    }
                }
          , new
                StringLikeExpect<string>("And nine, nine strings were gifted to the race of C++ coders, " +
                                         "who above all else desired unchecked memory access power. ", "***\"{0[1..^1]}\"###"
                                       , fromIndex: 9, length: 41)
                {
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                      , "***\\u0022nine strings were gifted to the race of\\u0022###"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                      , "\"***\"nine strings were gifted to the race of\"###\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                      , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
                    }
                }
          , new StringLikeExpect<string>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                                       , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsValueOut
                         , Log | Compact | Pretty)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsStringOut
                         , Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsValueOut)
                  , """
                    within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsStringOut
                         , Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AcceptsString | DefaultTreatedAsValueOut)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AcceptsString | AlwaysWrites | NonDefaultWrites | NonNullWrites
                         | NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                    operators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
            }
          , new StringLikeExpect<string>("But they were all of them deceived, for another string was made.", "{0,0/,//[1..]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                  , " for another string was made."
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , "\" for another string was made.\""
                }
            }
          , new StringLikeExpect<string>
                ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                 "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                 "languages with.", "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
                {
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                      , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut)
                      , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                    }
                }
          , new StringLikeExpect<string>
                ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                 "time confine them", "{0[^40..^0]}")
                {
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
                      , "and in the dustbins of time confine them"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut)
                      , "\"and in the dustbins of time confine them\""
                    }
                }
        };
}
