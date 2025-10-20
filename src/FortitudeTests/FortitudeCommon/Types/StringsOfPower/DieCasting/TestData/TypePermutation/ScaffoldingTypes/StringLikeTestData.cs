// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
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
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars |  AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"\"" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, Json | Compact | Pretty), "\"\"" }
        }
      , new FieldExpect<string>(null, "", true, "")
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<string>(null, "", true, "", 10, 50)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<string>(null, "", true, "", -1, -10)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<string>("It", "[{0}]", false, "", 3, 2)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "[]" }
          , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "[]"
                """
            }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "[]" },
        }
      , new FieldExpect<string>("began", "[{0[8..10]}]", false, "", 10, 5)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "[]" }
          ,  { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
               , """
                 "[]"
                 """ 
            }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "[]" },
        }
      , new FieldExpect<string>("with", "\"{0[8..10]}\"")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "\"\""
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<string>("the", "{0}", true, "", -1, -10)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          ,  { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"\"" }
          , { new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites), "" },
        }
      , new FieldExpect<string>("forging", "{0,10}", true, "orging", 1)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "    orging" }
           , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"    orging\"" },
        }
      , new FieldExpect<string>("It began with the forging of the Great Strings.", "[{0}]")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[It began with the forging of the Great Strings.]"
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"[It began with the forging of the Great Strings.]\""
            }
        }
      , new FieldExpect<string>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                              , "3{0[5..]}")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
            }
        }
      , new FieldExpect<string>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                              , "'{0,30}'", fromIndex: -1, length: 24)
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'      Seven to the Cobol-Lords'"
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'      Seven to the Cobol-Lords'\""
            }
        }
      , new
            FieldExpect<string>("And nine, nine strings were gifted to the race of C++ coders, " +
                                "who above all else desired unchecked memory access power. ", "***\"{0[1..^1]}\"***"
                              , fromIndex: 9, length: 41)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "***\"nine strings were gifted to the race of\"***"
                }
                ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"***\"nine strings were gifted to the race of\"***\""
                }
            }
      , new FieldExpect<string>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                              , "{0,0/ /\n/[1..^1]}")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                operators\u000ato\u000agovern\u000aeach"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<string>("But they were all of them deceived, for another string was made.", "{0,0/,//[1..]}")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , " for another string was made."
            }
            ,
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\" for another string was made.\""
            }
        }
      , new FieldExpect<string>
            ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
             "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
             "languages with.", "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
                ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                }
            }
      , new FieldExpect<string>
            ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
             "time confine them", "{0[^40..^0]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "and in the dustbins of time confine them"
                }
                ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"and in the dustbins of time confine them\""
                }
            }

        // char[]
      , new FieldExpect<char[]>("".ToCharArray(), "", true, [])
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "[]" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Json | Compact | Pretty)
              , """
                ""
                """
            }
        }
      , new FieldExpect<char[]>(null, "", true, [])
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<char[]>(null, "", true, [], 10, 50)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<char[]>(null, "", true, [], -1, -10)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites), "null" },
        }
      , new FieldExpect<char[]>("It".ToCharArray(), "\"{0}\"", false, [], 3, 2)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\"\"" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Log | Compact | Pretty), "\"\"" }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Json | Compact | Pretty)
              , """""
                """" 
                """""
            }
        }
      , new FieldExpect<char[]>("began".ToCharArray(), "'{0[8..10]}'", false, [], 10, 5)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "''" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Log | Compact | Pretty)
              , "''"
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["'","'"]"""
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
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, Log | Compact | Pretty)
              , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """""
                """"
                """""
            }
        }
      , new FieldExpect<char[]>("the".ToCharArray(), "{0}", true, [], -1, -10)
        {
            { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "[]" }
          , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Json | Compact | Pretty), "\"\"" }
        }
      , new FieldExpect<char[]>("forging".ToCharArray(), "[{0,10}]"
                              , true, "orging".ToCharArray(), 1)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "[    orging]" }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites, Log | Compact | Pretty)
              , "[    orging]"
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["["," "," "," "," ","o","r","g","i","n","g","]"]"""
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
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[It began with the forging of the Great Strings.]"
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                ["[","I","t"," ","b","e","g","a","n"," ","w","i","t","h"," ","t","h","e"," ","f","o","r","g","i","n","g"," ","o","f"," ","t","h","e"
                ," ","G","r","e","a","t"," ","S","t","r","i","n","g","s",".","]"]
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"[It began with the forging of the Great Strings.]\""
            }
        }
      , new FieldExpect<char[]>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.".ToCharArray()
                              , "3{0[5..]}")
        {
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                ["3"," ","w","e","r","e"," ","g","i","v","e","n"," ","t","o"," ","t","h","e"," ","A","s","s","e","m","b","l","y"," ","P","r","o"
                ,"g","r","a","m","m","e","r","s",","," ","i","m","p","r","a","c","t","i","c","a","l",","," ","w","a","c","k","i","e","s","t"
                ," ","a","n","d"," ","h","a","i","r","i","e","s","t"," ","o","f"," ","a","l","l"," ","b","e","i","n","g","s","."]
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
            }
        }
      , new FieldExpect<char[]>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                                    .ToCharArray(), "'{0,30}'", fromIndex: -1, length: 24)
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'      Seven to the Cobol-Lords'"
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """["'"," "," "," "," "," "," ","S","e","v","e","n"," ","t","o"," ","t","h","e"," ","C","o","b","o","l","-","L","o","r","d","s","'"]"""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'    Seven to the Cobol-Lords'\""
            }
        }
      , new
            FieldExpect<char[]>
            (("And nine, nine strings were gifted to the race of C++ coders, " +
              "who above all else desired unchecked memory access power. ").ToCharArray(), "***\"{0[1..^1]}\"***"
           , fromIndex: 9, length: 41)
            {
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "***\"nine strings were gifted to the race of\"***"
                }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["*","*","*","\u0022","n","i","n","e"," ","s","t","r","i","n","g","s"," ","w","e","r","e"," ","g","i","f","t","e","d",
                    " ","t","o"," ","t","h","e"," ","r","a","c","e"," ","o","f","\u0022","*","*","*"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , "***\"nine strings were gifted to the race of\"***"
                }
               ,
            }
      , new FieldExpect<char[]>
            ("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                 .ToCharArray(), "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["w","i","t","h","i","n","\\","u","0","0","0","a","t","h","e","s","e","\\","u","0","0","0","a","s","t","r","i","n","g","s"
                    ,"\\","u","0","0","0","a","w","a","s","\\","u","0","0","0","a","b","o","u","n","d","\\","u","0","0","0","a","t","h","e","\\"
                    ,"u","0","0","0","a","f","l","e","x","i","b","i","l","i","t","y",",","\\","u","0","0","0","a","m","u","t","a","b","i","l","i"
                    ,"t","y","\\","u","0","0","0","a","a","n","d","\\","u","0","0","0","a","t","h","e","\\","u","0","0","0","a","o","p","e","r","a"
                    ,"t","o","r","s","\\","u","0","0","0","a","t","o","\\","u","0","0","0","a","g","o","v","e","r","n","\\","u","0","0","0","a","e"
                    ,"a","c","h"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
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
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Log | Compact | Pretty)
              , " for another string was made."
            }
           ,
            {
                new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Compact | Pretty)
              , """[" ","f","o","r"," ","a","n","o","t","h","e","r"," ","s","t","r","i","n","g"," ","w","a","s"," ","m","a","d","e","."]"""
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Compact | Pretty)
              , "\" for another string was made.\""
            }
        }
      , new FieldExpect<char[]>
            (("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
              "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
              "languages with.").ToCharArray(), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    [" ","a","f","t","e","r"," ","m","a","n","y"," ","M","o","o","n","s"," ","o","f"," ","p","l","a","y","i","n","g"," ",
                    "D","o","o","m","!"," ","t","h","e"," ","D","o","t","n","e","t"," ","L","o","r","d"," ","H","e","j","l","s","b","e","r","g"," "
                    ,"f","o","r","g","e","d"," ","a"," ","m","a","s","t","e","r"," ","S","t","r","i","n","g"]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<char[]>
            (("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
              "time confine them").ToCharArray(), "{0[^40..^0]}")
            {
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "and in the dustbins of time confine them"
                }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["a","n","d"," ","i","n"," ","t","h","e"," ","d","u","s","t","b","i","n","s"," ","o","f"," ","t","i","m","e"," ",
                    "c","o","n","f","i","n","e"," ","t","h","e","m"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , "\"and in the dustbins of time confine them\""
                }
            }

        // ICharSequence
      , new FieldExpect<MutableString>(new MutableString(""), "", true, [])
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "[]" }
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, [])
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<MutableString>(null, "", true, [], 10, 50)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<CharArrayStringBuilder>(null, "", true, [], -1, -10)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<MutableString>(new MutableString("It"), "\"{0}\"", false, [], 3, 2)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "\"\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["\u0022","\u0022"]"""
            }
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("began"), "'{0[8..10]}'"
                                              , false, [], 10, 5)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "''" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["'","'"]"""
            }
        }
      , new FieldExpect<MutableString>(new MutableString("with"), "\"{0[8..10]}\"")
        {
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "\"\""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """["\u0022","\u0022"]"""
            }
        }
      , new FieldExpect<CharArrayStringBuilder>(new CharArrayStringBuilder("the"), "{0}", true, [], -1, -10)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "" }
          , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "[]" }
        }
      , new FieldExpect<MutableString>(new MutableString("forging"), "[{0,10}]", true, new MutableString("orging"), 1)
        {
            { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "[    orging]" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """["["," "," "," "," ","o","r","g","i","n","g","]"]"""
            }
        }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder("It began with the forging of the Great Strings."), "[{0}]")
            {
                {
                    new EK(AcceptsChars |  AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    ["[","I","t"," ","b","e","g","a","n"," ","w","i","t","h"," ","t","h","e"," ","f","o","r","g","i","n","g"," ","o","f"," ","t","h","e"
                    ," ","G","r","e","a","t"," ","S","t","r","i","n","g","s",".","]"]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """
                    ["3"," ","w","e","r","e"," ","g","i","v","e","n"," ","t","o"," ","t","h","e"," ","A","s","s","e","m","b","l","y"," ","P","r","o"
                    ,"g","r","a","m","m","e","r","s",","," ","i","m","p","r","a","c","t","i","c","a","l",","," ","w","a","c","k","i","e","s","t"
                    ," ","a","n","d"," ","h","a","i","r","i","e","s","t"," ","o","f"," ","a","l","l"," ","b","e","i","n","g","s","."]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new
                 CharArrayStringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
           , "'{0,30}'", fromIndex: -1, length: 24)
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence  | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "'      Seven to the Cobol-Lords'"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , """["'"," "," "," "," "," "," ","S","e","v","e","n"," ","t","o"," ","t","h","e"," ","C","o","b","o","l","-","L","o","r","d","s","'"]"""
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString
                 ("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"***"
           , fromIndex: 9, length: 41)
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "***\"nine strings were gifted to the race of\"***"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["*","*","*","\u0022","n","i","n","e"," ","s","t","r","i","n","g","s"," ","w","e","r","e"," ","g","i","f","t","e","d",
                    " ","t","o"," ","t","h","e"," ","r","a","c","e"," ","o","f","\u0022","*","*","*"]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["w","i","t","h","i","n","\\","u","0","0","0","a","t","h","e","s","e","\\","u","0","0","0","a","s","t","r","i","n","g","s"
                    ,"\\","u","0","0","0","a","w","a","s","\\","u","0","0","0","a","b","o","u","n","d","\\","u","0","0","0","a","t","h","e","\\"
                    ,"u","0","0","0","a","f","l","e","x","i","b","i","l","i","t","y",",","\\","u","0","0","0","a","m","u","t","a","b","i","l","i"
                    ,"t","y","\\","u","0","0","0","a","a","n","d","\\","u","0","0","0","a","t","h","e","\\","u","0","0","0","a","o","p","e","r","a"
                    ,"t","o","r","s","\\","u","0","0","0","a","t","o","\\","u","0","0","0","a","g","o","v","e","r","n","\\","u","0","0","0","a","e"
                    ,"a","c","h"]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , " for another string was made."
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """[" ","f","o","r"," ","a","n","o","t","h","e","r"," ","s","t","r","i","n","g"," ","w","a","s"," ","m","a","d","e","."]"""
                }
            }
      , new FieldExpect<CharArrayStringBuilder>
            (new CharArrayStringBuilder
                 ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                  "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                  "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    [" ","a","f","t","e","r"," ","m","a","n","y"," ","M","o","o","n","s"," ","o","f"," ","p","l","a","y","i","n","g"," ",
                    "D","o","o","m","!"," ","t","h","e"," ","D","o","t","n","e","t"," ","L","o","r","d"," ","H","e","j","l","s","b","e","r","g"," "
                    ,"f","o","r","g","e","d"," ","a"," ","m","a","s","t","e","r"," ","S","t","r","i","n","g"]
                    """.RemoveLineEndings()
                }
            }
      , new FieldExpect<MutableString>
            (new MutableString
                 ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                  "time confine them"), "{0[^40..^0]}")
            {
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "and in the dustbins of time confine them"
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact | Pretty)
                  , """
                    ["a","n","d"," ","i","n"," ","t","h","e"," ","d","u","s","t","b","i","n","s"," ","o","f"," ","t","i","m","e"," ",
                    "c","o","n","f","i","n","e"," ","t","h","e","m"]
                    """.RemoveLineEndings()
                }
            }

        // StringBuilder
      , new FieldExpect<StringBuilder>(new StringBuilder(""), "", true, new StringBuilder())
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "" }
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder())
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder(), 10, 50)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<StringBuilder>(null, "", true, new StringBuilder(), -1, -10)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites), "null" }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It"), "\"{0}\"", false, new StringBuilder(), 3, 2)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "\"\"" }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("began"), "'{0[8..10]}'", false, new StringBuilder(), 10, 5)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "''" }
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("with"), "\"{0[8..10]}\"")
        {
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"\""
            }
           ,
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("the"), "{0}", true, new StringBuilder(), -1, -10)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "" }, { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites), "" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("forging"), "[{0,10}]", true, new StringBuilder("orging"), 1)
        {
            { new EK(AcceptsChars | AlwaysWrites | NonNullWrites), "[    orging]" },
        }
      , new FieldExpect<StringBuilder>(new StringBuilder("It began with the forging of the Great Strings."), "[{0}]")
        {
            {
                new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "[It began with the forging of the Great Strings.]"
            }
        }
      , new
            FieldExpect<StringBuilder>
            (new StringBuilder("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
           , "3{0[5..]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
            }
      , new
            FieldExpect<StringBuilder>
            (new StringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
           , "{0,30}", fromIndex: -1, length: 26)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "    Seven to the Cobol-Lords"
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
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "***\"nine strings were gifted to the race of\"***"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder
                 ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
           , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , " for another string was made."
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder
                 ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                  "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                  "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                }
            }
      , new FieldExpect<StringBuilder>
            (new StringBuilder
                 ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                  "time confine them"), "{0[^40..^0]}")
            {
                {
                    new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                           NonNullAndPopulatedWrites)
                  , "and in the dustbins of time confine them"
                }
            }
    ];
}
