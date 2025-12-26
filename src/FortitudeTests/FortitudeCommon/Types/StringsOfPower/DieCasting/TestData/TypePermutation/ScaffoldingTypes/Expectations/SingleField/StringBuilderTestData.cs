// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class StringBuilderTestData
{
    private static PositionUpdatingList<IStringLikeExpectation>? allStringBuilderExpectations;

    public static PositionUpdatingList<IStringLikeExpectation> AllStringBuilderExpectations => allStringBuilderExpectations ??=
        new PositionUpdatingList<IStringLikeExpectation>(typeof(StringBuilderTestData))
        {
            // StringBuilder
            new StringLikeExpect<StringBuilder, StringBuilder>
                (new StringBuilder(""), "", true, new StringBuilder("0"))
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
                  , "0"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | EmptyBecomesNull | DefaultBecomesNull)
                  , "null"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesEmpty)
                  , "\"\""
                }
              , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut)
                  , "\"\""
                }

               ,
                {
                    new EK(ComplexType | AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites
                         | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"\""
                }
              , { new EK(ComplexType | AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites), "null" }
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (null, "", true, new StringBuilder(""))
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut |
                           DefaultTreatedAsStringOut
                         | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | DefaultTreatedAsValueOut
                         | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
            }
          , new StringLikeExpect<StringBuilder,  StringBuilder>
                (null, "", false, new StringBuilder(""), 10, 50)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites), "null" },
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (null, "", true, new StringBuilder("0"), -1, -10)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "0" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites), "null" },
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (new StringBuilder("It"), "\"{0}\"", false, new StringBuilder(), 3, 2)
            {
                {
                    new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "\"\""
                }
               ,
                {
                    new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)
                  , "\"\""
                }
              , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultBecomesZero
                         , Log | Compact | Pretty)
                  , "\"0\""
                }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "\"0\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultBecomesZero)
                  , "\"\\u00220\\u0022\""
                }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence), "\"\\u0022\\u0022\"" }
              , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "\"\"" }
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (new StringBuilder("began"), "'{0[8..10]}'", false, new StringBuilder("0"), 10, 5)
            {
                { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "''" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , "\"''\""
                }
            }
          , new StringLikeExpect<StringBuilder>(new StringBuilder("with"), "\"{0[8..10]}\"")
            {
                { new EK(SimpleType | CallsViaMatch | AcceptsStringBuilder, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | AcceptsStringBuilder | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | AcceptsStringBuilder | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder)
                  , "\"\\u0022\\u0022\""
                }
               , { new EK(AcceptsChars | AcceptsStringBuilder | AllOutputConditionsMask ) , "\"\"" }
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (new StringBuilder("the"), "{0}", true, new StringBuilder(""), -1, -10)
            {
                { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut |
                           DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "" }
              , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut)
                  , "\"\""
                }
            }
          , new StringLikeExpect<StringBuilder, StringBuilder>
                (new StringBuilder("forging"), "[{0,10}]", true, new StringBuilder("orging"), 1)
            {
                { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "[    orging]" }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut)
                  , "\"[    orging]\""
                }
            }
          , new StringLikeExpect<StringBuilder>(new StringBuilder("It began with the forging of the Great Strings."), "[{0}]")
            {
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
          , new
                StringLikeExpect<StringBuilder>(new
                                                    StringBuilder("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
                                              , "3{0[5..]}")
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                      , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                      , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                    }
                }
          , new
                StringLikeExpect<StringBuilder>
                (new StringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
               , "'{0,30}'", fromIndex: -1, length: 24)
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                      , "'      Seven to the Cobol-Lords'"
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                      , "\"'      Seven to the Cobol-Lords'\""
                    }
                }
          , new
                StringLikeExpect<StringBuilder>
                (new StringBuilder
                     ("And nine, nine strings were gifted to the race of C++ coders, " +
                      "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"***"
               , fromIndex: 9, length: 41)
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "***\"nine strings were gifted to the race of\"***"
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                      , "\"***\"nine strings were gifted to the race of\"***\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                      , "***\\u0022nine strings were gifted to the race of\\u0022***"
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                      , "\"***\\u0022nine strings were gifted to the race of\\u0022***\""
                    }
                }
          , new StringLikeExpect<StringBuilder>
                (new StringBuilder
                     ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
               , "{0,0/ /\n/[1..^1]}")
                {
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | CallsAsSpan | DefaultTreatedAsValueOut
                             , Log | Compact | Pretty)
                      , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | CallsAsSpan | DefaultTreatedAsStringOut
                             , Log | Compact | Pretty)
                      , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | CallsAsSpan | DefaultTreatedAsValueOut)
                      , """
                        within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                        \u000aoperators\u000ato\u000agovern\u000aeach
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | CallsAsSpan)
                      , """
                        "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                        \u000aoperators\u000ato\u000agovern\u000aeach"
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | CallsAsSpan | DefaultTreatedAsStringOut
                             , Log | Compact | Pretty)
                      , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                      , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                      , """
                        "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                        operators\u000ato\u000agovern\u000aeach"
                        """.RemoveLineEndings()
                    }
                }
          , new StringLikeExpect<StringBuilder>
                (new StringBuilder("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                      , " for another string was made."
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                      , """
                        " for another string was made."
                        """
                    }
                }
          , new StringLikeExpect<StringBuilder>
                (new StringBuilder
                     ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                      "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                      "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                      , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                      , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                    }
                }
          , new StringLikeExpect<StringBuilder>
                (new StringBuilder
                     ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                      "time confine them"), "{0[^40..^0]}")
                {
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                      , "and in the dustbins of time confine them"
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                               NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
                      , "\"and in the dustbins of time confine them\""
                    }
                }
        };
}
