// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class StringLikeTestData
{
    public static readonly IFormatExpectation[] AllStringLikeExpectations =
    [
        // string
        new FieldExpect<string>("", "", true, "")
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "" },
        }
      , new FieldExpect<string>(null, "", true, "")
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<string>(null, "", true, "", 10, 50)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<string>(null, "", true, "", -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<string>("It", "[{0}]", false, "", 3, 2)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "[]" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "[]" },
        }
      , new FieldExpect<string>("began", "[{0[8..10]}]", false, "", 10, 5)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "[]" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "[]" },
        }
      , new FieldExpect<string>("with", "\"{0[8..10]}\"")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"\""
            }
           ,
        }
      , new FieldExpect<string>("the", "{0}", true, "", -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "" },
        }
      , new FieldExpect<string>("forging", "{0,10}", true, "orging", 1)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "    orging" },
        }
      , new FieldExpect<string>("It began with the forging of the Great Strings.", "[{0}]")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]"
            }
        }
      , new FieldExpect<string>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                              , "3{0[5..]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
        }
      , new FieldExpect<string>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                              , "{0,30}", FromIndex: -1, Length: 26)
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "    Seven to the Cobol-Lords"
            }
        }
      , new
            FieldExpect<string>("And nine, nine strings were gifted to the race of C++ coders, " +
                                "who above all else desired unchecked memory access power. ", "***\"{0[1..^1]}\"***"
                              , FromIndex: 9, Length: 41)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "***\"nine strings were gifted to the race of\"***"
                }
            }
      , new FieldExpect<string>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                              , "{0,0/ /\n/[1..^1]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
            }
        }
      , new FieldExpect<string>("But they were all of them deceived, for another string was made.", "{0,0/,//[1..]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " for another string was made."
            }
        }
      , new FieldExpect<string>("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                                "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                                "languages with.", "{0,/,/!/[1..3]}", FromIndex: 16, Length: 100)
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
            }
        }
      , new FieldExpect<string>("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                                "time confine them", "{0[^40..^0]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "and in the dustbins of time confine them"
            }
        }

        // char[]
      , new FieldExpect<char[]>("".ToCharArray(), "", true, [])
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<char[]>(null, "", true, [])
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<char[]>(null, "", true, [], 10, 50)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<char[]>(null, "", true, [], -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<char[]>("It".ToCharArray(), "\"{0}\"", false, [], 3, 2)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "\"\"" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "\"\"" },
        }
      , new FieldExpect<char[]>("began".ToCharArray(), "'{0[8..10]}'", false, [], 10, 5)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "''" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "''" },
        }
      , new FieldExpect<char[]>("with".ToCharArray(), "\"{0[8..10]}\"")
        {
            {
                AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"\""
            }
           ,
        }
      , new FieldExpect<char[]>("the".ToCharArray(), "{0}", true, [], -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<char[]>("forging".ToCharArray(), "[{0,10}]", true, "orging".ToCharArray(), 1)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "[    orging]" },
        }
      , new FieldExpect<char[]>("It began with the forging of the Great Strings.".ToCharArray(), "[{0}]")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]"
            }
        }
      , new FieldExpect<char[]>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.".ToCharArray()
                              , "3{0[5..]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
        }
      , new FieldExpect<char[]>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                                    .ToCharArray(), "{0,30}", FromIndex: -1, Length: 26)
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "    Seven to the Cobol-Lords"
            }
        }
      , new
            FieldExpect<char[]>(("And nine, nine strings were gifted to the race of C++ coders, " +
                                 "who above all else desired unchecked memory access power. ").ToCharArray(), "***\"{0[1..^1]}\"***"
                              , FromIndex: 9, Length: 41)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "***\"nine strings were gifted to the race of\"***"
                }
            }
      , new FieldExpect<char[]>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                                    .ToCharArray(), "{0,0/ /\n/[1..^1]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
            }
        }
      , new FieldExpect<char[]>("But they were all of them deceived, for another string was made.".ToCharArray(), "{0,0/,//[1..]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " for another string was made."
            }
        }
      , new FieldExpect<char[]>(("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                                 "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                                 "languages with.").ToCharArray(), "{0,/,/!/[1..3]}", FromIndex: 16, Length: 100)
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
            }
        }
      , new FieldExpect<char[]>(("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                                 "time confine them").ToCharArray(), "{0[^40..^0]}")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "and in the dustbins of time confine them"
            }
        }

        // ICharSequence
      , new FieldExpect<MutableString>(new MutableString(""), "", true, [])
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, [])
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<MutableString>(null, "", true, [], 10, 50)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, [], -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<MutableString>(new MutableString("It"), "\"{0}\"", false, [], 3, 2)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "\"\"" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "\"\"" },
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("began"), "'{0[8..10]}'", false, [], 10, 5)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "''" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "''" },
        }
      , new FieldExpect<MutableString>(new MutableString("with"), "\"{0[8..10]}\"")
        {
            {
                AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"\""
            }
           ,
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("the"), "{0}", true, [], -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<MutableString>(new MutableString("forging"), "[{0,10}]", true, new MutableString("orging"), 1)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "[    orging]" },
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("It began with the forging of the Great Strings."), "[{0}]")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]"
            }
        }
      , new
            FieldExpect<MutableString>
            (new MutableString("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
            }
      , new
            FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
           , "{0,30}", FromIndex: -1, Length: 26)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "    Seven to the Cobol-Lords"
                }
            }
      , new
            FieldExpect<MutableString>
            (new MutableString
                 ("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"***"
           , FromIndex: 9, Length: 41)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "***\"nine strings were gifted to the race of\"***"
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , " for another string was made."
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                                        "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                                        "languages with."), "{0,/,/!/[1..3]}", FromIndex: 16, Length: 100)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                               "time confine them"), "{0[^40..^0]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "and in the dustbins of time confine them"
                }
            }

        // StringBuilder
      , new FieldExpect<StringBuilder>(new StringBuilder(""), "", true, new StringBuilder())
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder())
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder(), 10, 50)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder(), -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It"), "\"{0}\"", false, new StringBuilder(), 3, 2)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "\"\"" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "\"\"" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("began"), "'{0[8..10]}'", false, new StringBuilder(), 10, 5)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "''" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "''" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("with"), "\"{0[8..10]}\"")
        {
            {
                AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"\""
            }
           ,
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("the"), "{0}", true, new StringBuilder(), -1, -10)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" }, { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("forging"), "[{0,10}]", true, new StringBuilder("orging"), 1)
        {
            { AcceptsChars | AlwaysWrites | NonNullWrites, "[    orging]" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It began with the forging of the Great Strings."), "[{0}]")
        {
            {
                AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]"
            }
        }
      , new
            FieldExpect<StringBuilder>
            (new StringBuilder("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
            }
      , new
            FieldExpect<StringBuilder>
            (new StringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
           , "{0,30}", FromIndex: -1, Length: 26)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "    Seven to the Cobol-Lords"
                }
            }
      , new
            FieldExpect<StringBuilder>
            (new StringBuilder
                 ("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"***"
           , FromIndex: 9, Length: 41)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "***\"nine strings were gifted to the race of\"***"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , " for another string was made."
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                               "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                               "languages with."), "{0,/,/!/[1..3]}", FromIndex: 16, Length: 100)
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                               "time confine them"), "{0[^40..^0]}")
            {
                {
                    AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "and in the dustbins of time confine them"
                }
            }
    ];
}
