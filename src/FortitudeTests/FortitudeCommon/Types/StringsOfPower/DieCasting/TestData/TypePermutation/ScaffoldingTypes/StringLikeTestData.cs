// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public static class StringLikeTestData
{
    public static readonly IFormatExpectation[] AllStringLikeExpectations =
    [
        // string
        new FieldExpect<string>("", "", true, "0")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "\"\"" }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "0"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | EmptyBecomesNull | DefaultBecomesNull)
              , "null"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "\"0\""
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesEmpty)
              , "\"\""
            }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "" }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut)
              , "\"\""
            }

           ,
            {
                new EK(ComplexType | AcceptsChars | AlwaysWrites | NonDefaultWrites | NonDefaultWrites | NonNullWrites
                     | NonNullAndPopulatedWrites)
              , "\"\""
            }
          , { new EK(ComplexType | AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null" }
        }
      , new FieldExpect<string>(null, "", true, "")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsValueOut
                     | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
        }
      , new FieldExpect<string>(null, "", false, "", 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
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
      , new FieldExpect<string>(null, "", true, "0", -1, -10)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "0" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites), "null" },
        }
      , new FieldExpect<string>("It", "[{0}]", false, "0", 3, 2)
        {
            { new EK(AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "[0]" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "[]" }
          , { new EK(AcceptsChars | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback), "\"[0]\"" }
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
      , new FieldExpect<string>("began", "[{0[8..10]}]", false, "0", 10, 5)
        {
            { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "[]" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites | DefaultTreatedAsStringOut)
              , """
                "[]"
                """
            }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites |  DefaultTreatedAsStringOut)
              , """
                "[]"
                """
            }
        }
      , new FieldExpect<string>("with", "\"{0[8..10]}\"")
        {
            { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "\"\"" }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut)
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<string>("the", "{0}", true, "", -1, -10)
        {
            { new EK(AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonNullWrites  | DefaultTreatedAsStringOut)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites |  DefaultTreatedAsStringOut)
              , "\"\""
            }
           ,
        }
      , new FieldExpect<string>("forging", "{0,10}", true, "orging", 1)
        {
            { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "    orging" }
          , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"    orging\"" }
        }
      , new FieldExpect<string>("It began with the forging of the Great Strings.", "[{0}]")
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
      , new FieldExpect<string>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
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
      , new FieldExpect<string>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
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
            FieldExpect<string>("And nine, nine strings were gifted to the race of C++ coders, " +
                                "who above all else desired unchecked memory access power. ", "***\"{0[1..^1]}\"###"
                              , fromIndex: 9, length: 41)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut, Log | Compact | Pretty )
                  , "***\"nine strings were gifted to the race of\"###"
                }
              , {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut, Json | Compact | Pretty )
                  , "***\\u0022nine strings were gifted to the race of\\u0022###"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut, Log | Compact | Pretty )
                  , "\"***\"nine strings were gifted to the race of\"###\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut, Json | Compact | Pretty )
                  , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
                }
            }
      , new FieldExpect<string>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                              , "{0,0/ /\n/[1..^1]}")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut)
              , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Log | Compact | Pretty)
              , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Json | Compact | Pretty)
              , """
                "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                operators\u000ato\u000agovern\u000aeach"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<string>("But they were all of them deceived, for another string was made.", "{0,0/,//[1..]}")
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
      , new FieldExpect<string>
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
      , new FieldExpect<string>
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

        // char[]
      , new FieldExpect<char[]>("".ToCharArray(), "", true, ['0'])
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "\"\"" }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "0"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | EmptyBecomesNull | DefaultBecomesNull)
              , "null"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "\"0\""
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut | DefaultBecomesEmpty)
              , "\"\""
            }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "" }
           ,
            {
                new EK(SimpleType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesNull,
                       Log | Compact | Pretty)
              , "[]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesZero | DefaultBecomesNull, Log | Compact | Pretty)
              , "null"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesNull, Json | Compact | Pretty)
              , "[]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesZero | DefaultBecomesNull, Json | Compact | Pretty)
              , "null"
            }
        }
      , new FieldExpect<char[]>(null, "", true, [])
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | DefaultTreatedAsValueOut
                     | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
        }
      , new FieldExpect<char[]>(null, "", false, [], 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<char[]>(null, "", true, ['0'], -1, -10)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback ), "0" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback ), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "0" }
          , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites), "null" },
        }
      , new FieldExpect<char[]>("It".ToCharArray(), "\"{0}\"", false, ['0'], 3, 2)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback ), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback ), "\"\"0\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallback), "\"0\"" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback), "\"\"0\"\"" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "\"\"" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsStringOut )
              , """""
                """"
                """""
            }
           ,
            {
                new EK( AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Log | Compact | Pretty)
              , "[\"\"]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Log | Compact | Pretty)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Json | Compact)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Json | Pretty)
              , """
                [
                    "\u0022",
                    "\u0022"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut, Json | Compact | Pretty)
              , """""
                """" 
                """""
            }
        }
      , new FieldExpect<char[]>("began".ToCharArray(), "'{0[8..10]}'", false, [], 10, 5)
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut), "''" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut), "\"''\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                     , Log | Compact | Pretty)
              , "['']"
            }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Log | Compact | Pretty), "\"''\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Json | Compact)
              , """["'","'"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Json | Pretty)
              , """
                [
                    "'",
                    "'"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Json | Compact | Pretty)
              , """
                "''"
                """
            }
        }
      , new FieldExpect<char[]>("with".ToCharArray(), "\"{0[8..10]}\"")
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut), "\"\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut, Log | Compact | Pretty),
                """""
                """"
                """""
            }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut, Json | Compact | Pretty),
                """""
                "\u0022\u0022"
                """""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[\"\"]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Pretty)
              , """
                [
                    "\u0022",
                    "\u0022"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<char[]>("the".ToCharArray(), "{0}", true, [], -1, -10)
        {
            { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
          , {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites |  NonNullWrites, Log | Compact | Pretty)
              , "[]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites |  NonNullWrites, Log | Compact | Pretty)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites  | NonNullWrites, Json | Compact | Pretty)
              , "[]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites  | NonNullWrites, Json | Compact | Pretty)
              , "\"\""
            }
        }
      , new FieldExpect<char[]>("forging".ToCharArray(), "[{0,10}]"
                              , true, "orging".ToCharArray(), 1)
        {
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
              , "[    orging]"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
              , "\"[    orging]\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "[[    orging]]"
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , """
                "[    orging]"
                """
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Json | Compact)
              , """["["," "," "," "," ","o","r","g","i","n","g","]"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Json | Pretty)
              , """
                [
                    "[",
                    " ",
                    " ",
                    " ",
                    " ",
                    "o",
                    "r",
                    "g",
                    "i",
                    "n",
                    "g",
                    "]"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Json | Compact | Pretty)
              , """
                "[    orging]"
                """
            }
        }
      , new FieldExpect<char[]>("It began with the forging of the Great Strings.".ToCharArray(), "[{0}]")
        {
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
              , "[It began with the forging of the Great Strings.]"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
              , "\"[It began with the forging of the Great Strings.]\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[[It began with the forging of the Great Strings.]]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """
                ["[","I","t"," ","b","e","g","a","n"," ","w","i","t","h"," ","t","h","e"," ","f","o","r","g","i","n","g"," ","o","f"," ","t","h","e"
                ," ","G","r","e","a","t"," ","S","t","r","i","n","g","s",".","]"]
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                [
                    "[",
                    "I",
                    "t",
                    " ",
                    "b",
                    "e",
                    "g",
                    "a",
                    "n",
                    " ",
                    "w",
                    "i",
                    "t",
                    "h",
                    " ",
                    "t",
                    "h",
                    "e",
                    " ",
                    "f",
                    "o",
                    "r",
                    "g",
                    "i",
                    "n",
                    "g",
                    " ",
                    "o",
                    "f",
                    " ",
                    "t",
                    "h",
                    "e",
                    " ",
                    "G",
                    "r",
                    "e",
                    "a",
                    "t",
                    " ",
                    "S",
                    "t",
                    "r",
                    "i",
                    "n",
                    "g",
                    "s",
                    ".",
                    "]"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"[It began with the forging of the Great Strings.]\""
            }
        }
      , new FieldExpect<char[]>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.".ToCharArray()
                              , "3{0[5..]}")
        {
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
              , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """
                ["3"," ","w","e","r","e"," ","g","i","v","e","n"," ","t","o"," ","t","h","e"," ","A","s","s","e","m","b","l","y"," ","P","r","o"
                ,"g","r","a","m","m","e","r","s",","," ","i","m","p","r","a","c","t","i","c","a","l",","," ","w","a","c","k","i","e","s","t"
                ," ","a","n","d"," ","h","a","i","r","i","e","s","t"," ","o","f"," ","a","l","l"," ","b","e","i","n","g","s","."]
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Pretty)
              , """
                [
                    "3",
                    " ",
                    "w",
                    "e",
                    "r",
                    "e",
                    " ",
                    "g",
                    "i",
                    "v",
                    "e",
                    "n",
                    " ",
                    "t",
                    "o",
                    " ",
                    "t",
                    "h",
                    "e",
                    " ",
                    "A",
                    "s",
                    "s",
                    "e",
                    "m",
                    "b",
                    "l",
                    "y",
                    " ",
                    "P",
                    "r",
                    "o",
                    "g",
                    "r",
                    "a",
                    "m",
                    "m",
                    "e",
                    "r",
                    "s",
                    ",",
                    " ",
                    "i",
                    "m",
                    "p",
                    "r",
                    "a",
                    "c",
                    "t",
                    "i",
                    "c",
                    "a",
                    "l",
                    ",",
                    " ",
                    "w",
                    "a",
                    "c",
                    "k",
                    "i",
                    "e",
                    "s",
                    "t",
                    " ",
                    "a",
                    "n",
                    "d",
                    " ",
                    "h",
                    "a",
                    "i",
                    "r",
                    "i",
                    "e",
                    "s",
                    "t",
                    " ",
                    "o",
                    "f",
                    " ",
                    "a",
                    "l",
                    "l",
                    " ",
                    "b",
                    "e",
                    "i",
                    "n",
                    "g",
                    "s",
                    "."
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
            }
        }
      , new FieldExpect<char[]>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                                    .ToCharArray(), "'{0,30}'", fromIndex: -1, length: 24)
        {
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
              , "'      Seven to the Cobol-Lords'"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
              , "\"'      Seven to the Cobol-Lords'\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "['      Seven to the Cobol-Lords']"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """["'"," "," "," "," "," "," ","S","e","v","e","n"," ","t","o"," ","t","h","e"," ","C","o","b","o","l","-","L","o","r","d","s","'"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Pretty)
              , """
                [
                    "'",
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    "S",
                    "e",
                    "v",
                    "e",
                    "n",
                    " ",
                    "t",
                    "o",
                    " ",
                    "t",
                    "h",
                    "e",
                    " ",
                    "C",
                    "o",
                    "b",
                    "o",
                    "l",
                    "-",
                    "L",
                    "o",
                    "r",
                    "d",
                    "s",
                    "'"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"'    Seven to the Cobol-Lords'\""
            }
        }
      , new
            FieldExpect<char[]>
            (("And nine, nine strings were gifted to the race of C++ coders, " +
              "who above all else desired unchecked memory access power. ").ToCharArray(), "***\"{0[1..^1]}\"###"
           , fromIndex: 9, length: 41)
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "***\"nine strings were gifted to the race of\"###"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "\"***\"nine strings were gifted to the race of\"###\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "***\\u0022nine strings were gifted to the race of\\u0022###"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                  , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "[***\"nine strings were gifted to the race of\"###]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["*","*","*","\u0022","n","i","n","e"," ","s","t","r","i","n","g","s"," ","w","e","r","e"," ","g","i","f","t","e","d",
                    " ","t","o"," ","t","h","e"," ","r","a","c","e"," ","o","f","\u0022","#","#","#"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "*",
                        "*",
                        "*",
                        "\u0022",
                        "n",
                        "i",
                        "n",
                        "e",
                        " ",
                        "s",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        "s",
                        " ",
                        "w",
                        "e",
                        "r",
                        "e",
                        " ",
                        "g",
                        "i",
                        "f",
                        "t",
                        "e",
                        "d",
                        " ",
                        "t",
                        "o",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "r",
                        "a",
                        "c",
                        "e",
                        " ",
                        "o",
                        "f",
                        "\u0022",
                        "#",
                        "#",
                        "#"
                      ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites) , "***\"nine strings were gifted to the race of\"###"
                }
               ,
            }
      , new FieldExpect<char[]>
            ("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                 .ToCharArray(), "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "[within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["w","i","t","h","i","n","\u000a","t","h","e","s","e","\u000a","s","t","r","i","n","g","s","\u000a","w","a","s","\u000a",
                    "b","o","u","n","d","\u000a","t","h","e","\u000a","f","l","e","x","i","b","i","l","i","t","y",",","\u000a","m","u","t","a",
                    "b","i","l","i","t","y","\u000a","a","n","d","\u000a","t","h","e","\u000a","o","p","e","r","a","t","o","r","s","\u000a",
                    "t","o","\u000a","g","o","v","e","r","n","\u000a","e","a","c","h"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "w",
                        "i",
                        "t",
                        "h",
                        "i",
                        "n",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "s",
                        "e",
                        "\u000a",
                        "s",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        "s",
                        "\u000a",
                        "w",
                        "a",
                        "s",
                        "\u000a",
                        "b",
                        "o",
                        "u",
                        "n",
                        "d",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "\u000a",
                        "f",
                        "l",
                        "e",
                        "x",
                        "i",
                        "b",
                        "i",
                        "l",
                        "i",
                        "t",
                        "y",
                        ",",
                        "\u000a",
                        "m",
                        "u",
                        "t",
                        "a",
                        "b",
                        "i",
                        "l",
                        "i",
                        "t",
                        "y",
                        "\u000a",
                        "a",
                        "n",
                        "d",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "\u000a",
                        "o",
                        "p",
                        "e",
                        "r",
                        "a",
                        "t",
                        "o",
                        "r",
                        "s",
                        "\u000a",
                        "t",
                        "o",
                        "\u000a",
                        "g",
                        "o",
                        "v",
                        "e",
                        "r",
                        "n",
                        "\u000a",
                        "e",
                        "a",
                        "c",
                        "h"
                      ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                    operators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<char[]>("But they were all of them deceived, for another string was made.".ToCharArray()
                              , "{0,0/,//[1..]}")
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut), " for another string was made." }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut), "\" for another string was made.\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Log | Compact | Pretty)
              , "[ for another string was made.]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Compact)
              , """[" ","f","o","r"," ","a","n","o","t","h","e","r"," ","s","t","r","i","n","g"," ","w","a","s"," ","m","a","d","e","."]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Pretty)
              , """
                [
                    " ",
                    "f",
                    "o",
                    "r",
                    " ",
                    "a",
                    "n",
                    "o",
                    "t",
                    "h",
                    "e",
                    "r",
                    " ",
                    "s",
                    "t",
                    "r",
                    "i",
                    "n",
                    "g",
                    " ",
                    "w",
                    "a",
                    "s",
                    " ",
                    "m",
                    "a",
                    "d",
                    "e",
                    "."
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites)
              , "\" for another string was made.\""
            }
        }
      , new FieldExpect<char[]>
            (("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
              "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
              "languages with.").ToCharArray(), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "[ after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    [" ","a","f","t","e","r"," ","m","a","n","y"," ","M","o","o","n","s"," ","o","f"," ","p","l","a","y","i","n","g"," ",
                    "D","o","o","m","!"," ","t","h","e"," ","D","o","t","n","e","t"," ","L","o","r","d"," ","H","e","j","l","s","b","e","r","g"," "
                    ,"f","o","r","g","e","d"," ","a"," ","m","a","s","t","e","r"," ","S","t","r","i","n","g"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        " ",
                        "a",
                        "f",
                        "t",
                        "e",
                        "r",
                        " ",
                        "m",
                        "a",
                        "n",
                        "y",
                        " ",
                        "M",
                        "o",
                        "o",
                        "n",
                        "s",
                        " ",
                        "o",
                        "f",
                        " ",
                        "p",
                        "l",
                        "a",
                        "y",
                        "i",
                        "n",
                        "g",
                        " ",
                        "D",
                        "o",
                        "o",
                        "m",
                        "!",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "D",
                        "o",
                        "t",
                        "n",
                        "e",
                        "t",
                        " ",
                        "L",
                        "o",
                        "r",
                        "d",
                        " ",
                        "H",
                        "e",
                        "j",
                        "l",
                        "s",
                        "b",
                        "e",
                        "r",
                        "g",
                        " ",
                        "f",
                        "o",
                        "r",
                        "g",
                        "e",
                        "d",
                        " ",
                        "a",
                        " ",
                        "m",
                        "a",
                        "s",
                        "t",
                        "e",
                        "r",
                        " ",
                        "S",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g"
                      ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
            }
      , new FieldExpect<char[]>
            (("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
              "time confine them").ToCharArray(), "{0[^40..^0]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "and in the dustbins of time confine them"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
                  , "\"and in the dustbins of time confine them\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "[and in the dustbins of time confine them]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["a","n","d"," ","i","n"," ","t","h","e"," ","d","u","s","t","b","i","n","s"," ","o","f"," ","t","i","m","e"," ",
                    "c","o","n","f","i","n","e"," ","t","h","e","m"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "a",
                        "n",
                        "d",
                        " ",
                        "i",
                        "n",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "d",
                        "u",
                        "s",
                        "t",
                        "b",
                        "i",
                        "n",
                        "s",
                        " ",
                        "o",
                        "f",
                        " ",
                        "t",
                        "i",
                        "m",
                        "e",
                        " ",
                        "c",
                        "o",
                        "n",
                        "f",
                        "i",
                        "n",
                        "e",
                        " ",
                        "t",
                        "h",
                        "e",
                        "m"
                      ]
                    """.Dos2Unix()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
                  , "\"and in the dustbins of time confine them\""
                }
            }

        // ICharSequence
      , new FieldExpect<MutableString>(new MutableString(""), "", true, new MutableString("0"))
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "\"\"" }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "0"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | EmptyBecomesNull | DefaultBecomesNull)
              , "null"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
              , "\"0\""
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesEmpty)
              , "\"\""
            }
          , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut), "" }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                      | DefaultTreatedAsStringOut , Log | Compact | Pretty)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites |  DefaultTreatedAsStringOut
                     , Log | Compact | Pretty)
              , "null"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "[]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsStringOut, Json | Compact | Pretty)
              , "null"
            }
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, new CharArrayStringBuilder(""))
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | DefaultTreatedAsValueOut
                     | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
        }
      , new FieldExpect<MutableString>(null, "", true, new MutableString(""))
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | DefaultTreatedAsValueOut
                     | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", false, [], 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites), "null" },
        }
      , new FieldExpect<MutableString>(null, "", false, [], 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites), "null" },
        }
      , new FieldExpect<MutableString>(null, "", false, new MutableString(""), 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites), "null" },
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, new CharArrayStringBuilder("0"), -1, -10)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites), "null" },
        }
      , new FieldExpect<MutableString>(null, "", true, new MutableString("0"), -1, -10)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "0" }
          , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites), "null" },
        }
      , new FieldExpect<MutableString>(new MutableString("It"), "\"{0}\"", false, new MutableString(), 3, 2)
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero ), "\"0\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "\"\"" }
          , { new EK(AcceptsChars | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback), "\"\"\"\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                     , Log | Compact | Pretty)
              , """""
                """"
                """""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                     , Json | Compact)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                     , Json | Pretty)
              , """
                [
                    "\u0022",
                    "\u0022"
                  ]
                """.Dos2Unix()
            }
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("began"), "'{0[8..10]}'"
                                              , false, [], 10, 5)
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "''" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"''\"" }
           , {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut 
                     , Log | Compact | Pretty)
              , "\"''\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact)
              , """["'","'"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Pretty)
              , """
                [
                    "'",
                    "'"
                  ]
                """.Dos2Unix()
            }
        }
      , new FieldExpect<MutableString>(new MutableString("with"), "\"{0[8..10]}\"")
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "\"\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut, Log | Compact | Pretty)
              , """""
                """"
                """"" 
            }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut, Json | Compact | Pretty)
              , """""
                "\u0022\u0022"
                """"" 
            }
           ,{
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , """""
                """"
                """""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Pretty)
              , """
                [
                    "\u0022",
                    "\u0022"
                  ]
                """.Dos2Unix()
            }
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("the"), "{0}", true
                                              , new CharArrayStringBuilder(""), -1, -10)
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
          , {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut 
                     , Json | Compact | Pretty), "[]"
            }
        }
      , new FieldExpect<MutableString>(new MutableString("forging"), "[{0,10}]", true
                                     , new MutableString("orging"), 1)
        {
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
              , "[    orging]"
            }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
              , "\"[    orging]\""
            }
           ,{
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "\"[    orging]\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact)
              , """["["," "," "," "," ","o","r","g","i","n","g","]"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Pretty)
              , """
                [
                    "[",
                    " ",
                    " ",
                    " ",
                    " ",
                    "o",
                    "r",
                    "g",
                    "i",
                    "n",
                    "g",
                    "]"
                  ]
                """.Dos2Unix()
            }
        }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder("It began with the forging of the Great Strings."), "[{0}]")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact)
                  , """
                    ["[","I","t"," ","b","e","g","a","n"," ","w","i","t","h"," ","t","h","e"," ","f","o","r","g","i","n","g"," ","o","f"," ","t","h","e"
                    ," ","G","r","e","a","t"," ","S","t","r","i","n","g","s",".","]"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    [
                        "[",
                        "I",
                        "t",
                        " ",
                        "b",
                        "e",
                        "g",
                        "a",
                        "n",
                        " ",
                        "w",
                        "i",
                        "t",
                        "h",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "f",
                        "o",
                        "r",
                        "g",
                        "i",
                        "n",
                        "g",
                        " ",
                        "o",
                        "f",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "G",
                        "r",
                        "e",
                        "a",
                        "t",
                        " ",
                        "S",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        "s",
                        ".",
                        "]"
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["3"," ","w","e","r","e"," ","g","i","v","e","n"," ","t","o"," ","t","h","e"," ","A","s","s","e","m","b","l","y"," ","P","r","o"
                    ,"g","r","a","m","m","e","r","s",","," ","i","m","p","r","a","c","t","i","c","a","l",","," ","w","a","c","k","i","e","s","t"
                    ," ","a","n","d"," ","h","a","i","r","i","e","s","t"," ","o","f"," ","a","l","l"," ","b","e","i","n","g","s","."]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Pretty)
                  , """
                    [
                        "3",
                        " ",
                        "w",
                        "e",
                        "r",
                        "e",
                        " ",
                        "g",
                        "i",
                        "v",
                        "e",
                        "n",
                        " ",
                        "t",
                        "o",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "A",
                        "s",
                        "s",
                        "e",
                        "m",
                        "b",
                        "l",
                        "y",
                        " ",
                        "P",
                        "r",
                        "o",
                        "g",
                        "r",
                        "a",
                        "m",
                        "m",
                        "e",
                        "r",
                        "s",
                        ",",
                        " ",
                        "i",
                        "m",
                        "p",
                        "r",
                        "a",
                        "c",
                        "t",
                        "i",
                        "c",
                        "a",
                        "l",
                        ",",
                        " ",
                        "w",
                        "a",
                        "c",
                        "k",
                        "i",
                        "e",
                        "s",
                        "t",
                        " ",
                        "a",
                        "n",
                        "d",
                        " ",
                        "h",
                        "a",
                        "i",
                        "r",
                        "i",
                        "e",
                        "s",
                        "t",
                        " ",
                        "o",
                        "f",
                        " ",
                        "a",
                        "l",
                        "l",
                        " ",
                        "b",
                        "e",
                        "i",
                        "n",
                        "g",
                        "s",
                        "."
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
           , "'{0,30}'", fromIndex: -1, length: 24)
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "'      Seven to the Cobol-Lords'"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"'      Seven to the Cobol-Lords'\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"'      Seven to the Cobol-Lords'\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact)
                  , """["'"," "," "," "," "," "," ","S","e","v","e","n"," ","t","o"," ","t","h","e"," ","C","o","b","o","l","-","L","o","r","d","s","'"]"""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                            , Json | Pretty)
                  , """
                    [
                        "'",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        "S",
                        "e",
                        "v",
                        "e",
                        "n",
                        " ",
                        "t",
                        "o",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "C",
                        "o",
                        "b",
                        "o",
                        "l",
                        "-",
                        "L",
                        "o",
                        "r",
                        "d",
                        "s",
                        "'"
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString
                 ("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"###"
           , fromIndex: 9, length: 41)
            {
                {
                    new EK(SimpleType | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "***\"nine strings were gifted to the race of\"###"
                }
               ,
                {
                    new EK(SimpleType | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"***\"nine strings were gifted to the race of\"###\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"***\"nine strings were gifted to the race of\"###\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["*","*","*","\u0022","n","i","n","e"," ","s","t","r","i","n","g","s"," ","w","e","r","e"," ","g","i","f","t","e","d",
                    " ","t","o"," ","t","h","e"," ","r","a","c","e"," ","o","f","\u0022","#","#","#"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "*",
                        "*",
                        "*",
                        "\u0022",
                        "n",
                        "i",
                        "n",
                        "e",
                        " ",
                        "s",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        "s",
                        " ",
                        "w",
                        "e",
                        "r",
                        "e",
                        " ",
                        "g",
                        "i",
                        "f",
                        "t",
                        "e",
                        "d",
                        " ",
                        "t",
                        "o",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "r",
                        "a",
                        "c",
                        "e",
                        " ",
                        "o",
                        "f",
                        "\u0022",
                        "#",
                        "#",
                        "#"
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["w","i","t","h","i","n","\u000a","t","h","e","s","e","\u000a","s","t","r","i","n","g","s","\u000a","w","a","s","\u000a",
                    "b","o","u","n","d","\u000a","t","h","e","\u000a","f","l","e","x","i","b","i","l","i","t","y",",","\u000a","m","u","t","a"
                    ,"b","i","l","i","t","y","\u000a","a","n","d","\u000a","t","h","e","\u000a","o","p","e","r","a","t","o","r","s","\u000a","t"
                    ,"o","\u000a","g","o","v","e","r","n","\u000a","e","a","c","h"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "w",
                        "i",
                        "t",
                        "h",
                        "i",
                        "n",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "s",
                        "e",
                        "\u000a",
                        "s",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        "s",
                        "\u000a",
                        "w",
                        "a",
                        "s",
                        "\u000a",
                        "b",
                        "o",
                        "u",
                        "n",
                        "d",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "\u000a",
                        "f",
                        "l",
                        "e",
                        "x",
                        "i",
                        "b",
                        "i",
                        "l",
                        "i",
                        "t",
                        "y",
                        ",",
                        "\u000a",
                        "m",
                        "u",
                        "t",
                        "a",
                        "b",
                        "i",
                        "l",
                        "i",
                        "t",
                        "y",
                        "\u000a",
                        "a",
                        "n",
                        "d",
                        "\u000a",
                        "t",
                        "h",
                        "e",
                        "\u000a",
                        "o",
                        "p",
                        "e",
                        "r",
                        "a",
                        "t",
                        "o",
                        "r",
                        "s",
                        "\u000a",
                        "t",
                        "o",
                        "\u000a",
                        "g",
                        "o",
                        "v",
                        "e",
                        "r",
                        "n",
                        "\u000a",
                        "e",
                        "a",
                        "c",
                        "h"
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), " for another string was made." }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\" for another string was made.\"" }
               , {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\" for another string was made.\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """[" ","f","o","r"," ","a","n","o","t","h","e","r"," ","s","t","r","i","n","g"," ","w","a","s"," ","m","a","d","e","."]"""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        " ",
                        "f",
                        "o",
                        "r",
                        " ",
                        "a",
                        "n",
                        "o",
                        "t",
                        "h",
                        "e",
                        "r",
                        " ",
                        "s",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g",
                        " ",
                        "w",
                        "a",
                        "s",
                        " ",
                        "m",
                        "a",
                        "d",
                        "e",
                        "."
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder
                 ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                  "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                  "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    [" ","a","f","t","e","r"," ","m","a","n","y"," ","M","o","o","n","s"," ","o","f"," ","p","l","a","y","i","n","g"," ",
                    "D","o","o","m","!"," ","t","h","e"," ","D","o","t","n","e","t"," ","L","o","r","d"," ","H","e","j","l","s","b","e","r","g"," "
                    ,"f","o","r","g","e","d"," ","a"," ","m","a","s","t","e","r"," ","S","t","r","i","n","g"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        " ",
                        "a",
                        "f",
                        "t",
                        "e",
                        "r",
                        " ",
                        "m",
                        "a",
                        "n",
                        "y",
                        " ",
                        "M",
                        "o",
                        "o",
                        "n",
                        "s",
                        " ",
                        "o",
                        "f",
                        " ",
                        "p",
                        "l",
                        "a",
                        "y",
                        "i",
                        "n",
                        "g",
                        " ",
                        "D",
                        "o",
                        "o",
                        "m",
                        "!",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "D",
                        "o",
                        "t",
                        "n",
                        "e",
                        "t",
                        " ",
                        "L",
                        "o",
                        "r",
                        "d",
                        " ",
                        "H",
                        "e",
                        "j",
                        "l",
                        "s",
                        "b",
                        "e",
                        "r",
                        "g",
                        " ",
                        "f",
                        "o",
                        "r",
                        "g",
                        "e",
                        "d",
                        " ",
                        "a",
                        " ",
                        "m",
                        "a",
                        "s",
                        "t",
                        "e",
                        "r",
                        " ",
                        "S",
                        "t",
                        "r",
                        "i",
                        "n",
                        "g"
                      ]
                    """.Dos2Unix()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString
                 ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                  "time confine them"), "{0[^40..^0]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "and in the dustbins of time confine them"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"and in the dustbins of time confine them\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"and in the dustbins of time confine them\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["a","n","d"," ","i","n"," ","t","h","e"," ","d","u","s","t","b","i","n","s"," ","o","f"," ","t","i","m","e"," ",
                    "c","o","n","f","i","n","e"," ","t","h","e","m"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "a",
                        "n",
                        "d",
                        " ",
                        "i",
                        "n",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "d",
                        "u",
                        "s",
                        "t",
                        "b",
                        "i",
                        "n",
                        "s",
                        " ",
                        "o",
                        "f",
                        " ",
                        "t",
                        "i",
                        "m",
                        "e",
                        " ",
                        "c",
                        "o",
                        "n",
                        "f",
                        "i",
                        "n",
                        "e",
                        " ",
                        "t",
                        "h",
                        "e",
                        "m"
                      ]
                    """.Dos2Unix()
                }
            }

        // StringBuilder
      , new FieldExpect<StringBuilder>(new StringBuilder(""), "", true, new StringBuilder("0"))
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut), "" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "\"\"" }
           ,
            {
                new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero
                     | DefaultBecomesFallback)
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
                     | DefaultBecomesFallback)
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
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder(""))
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
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
      , new FieldExpect<StringBuilder>(null, "", false, new StringBuilder(""), 10, 50)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
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
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder("0"), -1, -10)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut), "null" }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
              , "null"
            }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"0\"" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "0" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites), "null" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It"), "\"{0}\"", false, new StringBuilder(), 3, 2)
        {
            { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesZero ), "\"0\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesZero ), "\"\"0\"\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder |DefaultTreatedAsValueOut | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesZero), "\"\"0\"\"" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "\"\"" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallback), "\"\"\"\"" }
           ,{
                new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut )
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("began"), "'{0[8..10]}'", false, new StringBuilder("0"), 10, 5)
        {
            { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "''" }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites 
                     | DefaultTreatedAsStringOut) , "\"''\""
            }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("with"), "\"{0[8..10]}\"")
        {
            { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "\"\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites 
                     | DefaultTreatedAsStringOut )
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("the"), "{0}", true, new StringBuilder(""), -1, -10)
        {
            { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
          , { new EK(SimpleType | AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "" }
          , { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsStringOut), "\"\"" }
          , {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites |  NonNullWrites | DefaultTreatedAsStringOut )
              , "\"\""
            }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("forging"), "[{0,10}]", true, new StringBuilder("orging"), 1)
        {
            { new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut), "[    orging]" }
          , {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut )
              , "\"[    orging]\""
            }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It began with the forging of the Great Strings."), "[{0}]")
        {
            {
                new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
              , "[It began with the forging of the Great Strings.]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsStringOut )
              , "\"[It began with the forging of the Great Strings.]\""
            }
        }
      , new FieldExpect<StringBuilder> (new StringBuilder("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
        {
            {
                new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
           ,
            {
                new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites | DefaultTreatedAsStringOut )
              , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
            }
        }
      , new
            FieldExpect<StringBuilder>
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
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut )
                  , "\"'      Seven to the Cobol-Lords'\""
                }
            }
      , new
            FieldExpect<StringBuilder>
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
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Log | Compact | Pretty )
                  , "\"***\"nine strings were gifted to the race of\"***\""
                }
              , {
                    new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                  , "***\\u0022nine strings were gifted to the race of\\u0022***"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Json | Compact | Pretty )
                  , "\"***\\u0022nine strings were gifted to the race of\\u0022***\""
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut , Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut , Json | Compact | Pretty)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                    operators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | DefaultTreatedAsValueOut)
                  , " for another string was made."
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsStringBuilder | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut )
                  , """
                    " for another string was made."
                    """
                }
            }
      , new FieldExpect<StringBuilder>
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
                           NonNullAndPopulatedWrites | DefaultTreatedAsStringOut )
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
            }
      , new FieldExpect<StringBuilder>
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
                           NonNullAndPopulatedWrites  | DefaultTreatedAsStringOut )
                  , "\"and in the dustbins of time confine them\""
                }
            }
    ];
}
