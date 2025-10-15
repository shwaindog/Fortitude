// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
            { AcceptsChars | AlwaysWrites | NonNullWrites, "" },
            { AcceptsChars | CallsAsSpan | AlwaysWrites, "" },
            { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "" }
        }
      , new FieldExpect<string>(null, "", true, "")
        {
            { AcceptsChars | AlwaysWrites | NonEmptyWrites, "null" },
            { AcceptsChars | CallsAsSpan | AlwaysWrites, "null" },
            { AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites, "null" }
        }
       , new FieldExpect<string>("It began with the forging of the Great Strings.", "[{0}]")
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "[It began with the forging of the Great Strings.]" }
        }
      , new FieldExpect<string>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                              , "3{0[5..]}")
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings." }
        }
      , new FieldExpect<string>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                              , "{0,30}", FromIndex: -1, Length: 26)
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "    Seven to the Cobol-Lords" }
        }
      , new
            FieldExpect<string>("And nine, nine strings were gifted to the race of C++ coders, " +
                                "who above all else desired unchecked memory access power. " , "***\"{0[1..^1]}\"***", FromIndex: 9, Length: 41)
            {
                { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
                  , "***\"nine strings were gifted to the race of\"***" }
            }
      , new FieldExpect<string>("For within these strings was bound the flexibility, mutability and the operators to govern each language"
                              , "{0,0/ /\n/[1..^1]}")
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "within\nthese\nstrings\nwas\nbound\nthe\nflexibility," +
                                            "\nmutability\nand\nthe\noperators\nto\ngovern\neach" }
        }
      , new FieldExpect<string>("But they were all of them deceived, for another string was made.", "{0,0/,//[1..]}")
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " for another string was made." }
        }
      , new FieldExpect<string>("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                                "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                                "languages with.", "{0,/,/!/[1..3]}", FromIndex: 16, Length:100)
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String" }
            
        }
      , new FieldExpect<string>("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                                "time confine them", "{0[^40..^0]}")
        {
            { AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
      , "and in the dustbins of time confine them" }
        }
    ];
}
