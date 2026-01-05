// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public class CharArrayTestData
{
    private static PositionUpdatingList<IStringLikeExpectation>? allCharArrayExpectations;

    public static PositionUpdatingList<IStringLikeExpectation> AllCharArrayExpectations => allCharArrayExpectations ??=
        new PositionUpdatingList<IStringLikeExpectation>(typeof(CharArrayTestData))
        {
            // char[]

            new StringLikeExpect<string>("", "", true, "0")
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
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
                         | DefaultBecomesFallbackValue)
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
          , new StringLikeExpect<char[], char[]>("".ToCharArray(), "", true, ['0']
                                               , formatFlags: AsCollection)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(IsContentType | CallsViaMatch), "\"\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
                  , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | EmptyBecomesNull | DefaultBecomesNull)
                  , "null"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "" }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AllOutputConditionsMask
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesNull,
                           AnyLog)
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
                    new EK(AcceptsChars | AcceptsCharArray | AllOutputConditionsMask
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesNull, AnyJson)
                  , "[]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | DefaultBecomesZero | DefaultBecomesNull, AnyJson)
                  , "null"
                }
            }
          , new StringLikeExpect<char[], char[]>(null, "", true, [])
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
              , { 
                    new EK(IsContentType | CallsViaMatch | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                       | DefaultBecomesFallbackString) , "" 
                }
              , { new EK(IsContentType | CallsViaMatch), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsCharArray | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsCharArray | DefaultTreatedAsStringOut 
                         | DefaultBecomesFallbackValue | DefaultBecomesFallbackString) , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                         | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { 
                    new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue 
                         | DefaultBecomesFallbackString) , "" 
                }
              , {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut 
                         | DefaultBecomesNull) , "null"
                }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | DefaultTreatedAsValueOut), "" }
            }
          , new StringLikeExpect<char[], char[]>(null, "", false, [], 10, 50)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue), "\"\"" }
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
          , new StringLikeExpect<char[], char[]>(null, "", true, ['0'], -1, -10)
            {
                { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "0" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0\""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "0" }
              , { new EK(AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites), "null" },
            }
          , new StringLikeExpect<char[], char[]>("It".ToCharArray(), "[{0}]", false, ['0'], 3, 2)
            {
                { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero | DefaultBecomesFallbackValue), "[0]" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut), "[]" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsStringOut | DefaultBecomesZero | DefaultBecomesFallbackValue), "\"[0]\"" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsStringOut), "\"[]\"" }
              , { new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask, AnyLog) , "[]" }
               ,
                {
                    new EK(AcceptsChars | AllOutputConditionsMask)
                  , """
                    "[]"
                    """
                }
            }
          , new StringLikeExpect<char[], char[]>("It".ToCharArray(), "\"{0}\"", false, ['0'], 3, 2
                                               , formatFlags: AsCollection)
            {
                {
                    new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue, Log | Compact | Pretty)
                  , "\"0\""
                }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue)
                  , "\"0\""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue), "\"\\u00220\\u0022\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultBecomesFallbackValue | DefaultBecomesZero
                         , Log | Compact | Pretty)
                  , "\"0\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                         | DefaultBecomesZero)
                  , "\"0\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultBecomesFallbackValue | DefaultBecomesZero)
                  , "\"\\u00220\\u0022\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultBecomesEmpty | DefaultBecomesNull
                         , Log | Compact | Pretty)
                  , "\"\""
                }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut, AnyLog), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut, AnyJson), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut), "\"\\u0022\\u0022\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Log | Compact | Pretty), "[]" }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "[\"\"]"
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
                    new EK(AcceptsChars | AcceptsCharArray | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites
                         | NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites
                         | NonNullAndPopulatedWrites | DefaultTreatedAsStringOut, Json | Compact | Pretty)
                  , "\"\\u0022\\u0022\""
                }
            }
          , new StringLikeExpect<char[], char[]>("began".ToCharArray(), "[{0[8..10]}]", false, ['0'], 10, 5)
            {
                { new EK(IsContentType | AcceptsChars | AllOutputConditionsMask | DefaultTreatedAsValueOut), "[]" }
              , { new EK(IsContentType | AcceptsChars), "\"[]\"" }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask, AnyLog)
                  , """
                    []
                    """
                }
              , {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask)
                  , """
                    "[]"
                    """
                }
            }
          , new StringLikeExpect<char[], char[]>("began".ToCharArray(), "'{0[8..10]}'", false, [], 10, 5
                                               , AsCollection)
            {
                { new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut), "''" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut), "\"''\"" }
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

          , new StringLikeExpect<char[]>("with".ToCharArray(), "\"{0[8..10]}\"")
            {
                { new EK(IsContentType | CallsViaMatch | AcceptsString, Log | Compact | Pretty), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsString | CallsAsSpan, Log | Compact | Pretty), "\"\"" }

              , { new EK(AcceptsChars | AcceptsString | CallsAsSpan | DefaultTreatedAsValueOut), "\"\"" }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsString | CallsAsSpan)
                  , "\"\\u0022\\u0022\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask)
                  , "\"\""
                }
            }
          , new StringLikeExpect<char[]>("with".ToCharArray(), "\"{0[8..10]}\"", formatFlags: AsCollection)
            {
                { new EK(IsContentType | CallsViaMatch | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(IsContentType | CallsViaMatch | AcceptsCharArray | DefaultTreatedAsStringOut, AnyJson), "\"\\u0022\\u0022\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan, AnyLog), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut), "\"\"" }
              , {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut, AnyJson)
                  , """""
                    "\u0022\u0022"
                    """""
                }
              , { new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Log | Compact | Pretty), "[]" }
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
                    new EK(AcceptsChars | AcceptsCharArray | CallsAsSpan | AllOutputConditionsMask)
                  , "\"\""
                }
            }
          , new StringLikeExpect<char[], char[]>("the".ToCharArray(), "\"{0}\"", true, [], -1, -10)
            {
                { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "\"0\"" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut, AnyLog), "\"\"" }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut, AnyJson)
                  , """
                    "\u0022\u0022"
                    """
                }
              ,
                {
                    new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut)
                  , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | DefaultTreatedAsStringOut)
                  , "\"\""
                }
            }
          , new StringLikeExpect<char[], char[]>("the".ToCharArray(), "{0}", true, [], -1, -10
                                                     , AsCollection)
            {
                { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
              , { new EK(IsContentType | AcceptsChars | DefaultTreatedAsValueOut), "" }
              , { new EK(IsContentType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "[]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                  , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "[]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
                  , "\"\""
                }
            }
          , new StringLikeExpect<char[], char[]>("forging".ToCharArray(), "{0,10}", true, "orging".ToCharArray()
                                               , 1)
            {
                { new EK(IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "    orging" }
              , { new EK(IsContentType | AcceptsChars | CallsAsSpan), "\"    orging\"" }
              , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites, AnyLog), "    orging" }
              , { new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"    orging\"" }
            }
          , new StringLikeExpect<char[], char[]>("forging".ToCharArray(), "[{0,10}]", true, "orging".ToCharArray()
                                               , 1, formatFlags: AsCollection)
            {
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "[    orging]"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"[    orging]\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, AnyLog)
                  , "[[    orging]]"
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
          , new StringLikeExpect<char[]>("It began with the forging of the Great Strings.".ToCharArray(), "[{0}]")
            {
                {
                    new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,{
                    new EK( IsContentType | AcceptsChars | CallsAsSpan)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
               ,
                {
                    new EK( AcceptsChars | AllOutputConditionsMask, AnyLog)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask | DefaultTreatedAsStringOut)
                  , "\"[It began with the forging of the Great Strings.]\""
                }
            }
          , new StringLikeExpect<char[]>("It began with the forging of the Great Strings.".ToCharArray(), "[{0}]", formatFlags: AsCollection)
            {
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "[It began with the forging of the Great Strings.]"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
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
          , new StringLikeExpect<char[]>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.".ToCharArray() 
                                       , "3{0[5..]}")
            {
                {
                    new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,{
                    new EK( IsContentType | AcceptsChars | CallsAsSpan)
                  , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                }
               ,
                {
                    new EK(AcceptsChars | AllOutputConditionsMask, AnyLog)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask | DefaultTreatedAsStringOut)
                  , "\"3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.\""
                }
            }
          , new StringLikeExpect<char[]>("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                                             .ToCharArray()
                                       , "3{0[5..]}", formatFlags: AsCollection)
            {
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , "3 were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
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
          , new StringLikeExpect<char[]>
                ("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.".ToCharArray()
               , "'{0,30}'", fromIndex: -1, length: 24)
                {
                    { new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut), "'      Seven to the Cobol-Lords'" }
                   ,{ new EK( IsContentType | AcceptsChars | CallsAsSpan), "\"'      Seven to the Cobol-Lords'\"" }
                   , { new EK(AcceptsChars | AllOutputConditionsMask, AnyLog) , "'      Seven to the Cobol-Lords'" }
                   , {
                        new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask | DefaultTreatedAsStringOut)
                      , "\"'      Seven to the Cobol-Lords'\""
                    }
                }
          , new
                StringLikeExpect<char[]>("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
                                             .ToCharArray(), "'{0,30}'", fromIndex: -1, length: 24, formatFlags: AsCollection)
                {
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                      , "'      Seven to the Cobol-Lords'"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
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
                StringLikeExpect<char[]>
                (("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. ").ToCharArray(), "***\"{0[1..^1]}\"###"
               , fromIndex: 9, length: 41)
                {
                    {
                        new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut, AnyLog)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,{
                        new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut, AnyLog)
                      , "\"***\"nine strings were gifted to the race of\"###\""
                    }
                   ,
                    {
                        new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut, AnyJson)
                      , "***\\u0022nine strings were gifted to the race of\\u0022###"
                    }
                  , {
                        new EK(IsContentType | AcceptsChars | CallsAsSpan, AnyJson)
                      , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask, AnyLog)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask | DefaultTreatedAsStringOut, AnyJson)
                      , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
                    }
                }
          , new
                StringLikeExpect<char[]>
                (("And nine, nine strings were gifted to the race of C++ coders, " +
                  "who above all else desired unchecked memory access power. ").ToCharArray(), "***\"{0[1..^1]}\"###"
               , fromIndex: 9, length: 41, formatFlags: AsCollection)
                {
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                      , "\"***\"nine strings were gifted to the race of\"###\""
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                      , "***\\u0022nine strings were gifted to the race of\\u0022###"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | DefaultTreatedAsStringOut, Json | Compact | Pretty)
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
                               NonNullAndPopulatedWrites)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,
                }
          , new StringLikeExpect<char[]>
                ("For within these strings was bound the flexibility, mutability and the operators to govern each language".ToCharArray()
               , "{0,0/ /\n/[1..^1]}")
                {
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsString | DefaultTreatedAsValueOut, AnyLog)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsString | DefaultTreatedAsStringOut , AnyLog)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsString | DefaultTreatedAsValueOut)
                  , """
                    within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsString, AnyJson)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | CallsAsSpan | AcceptsString | DefaultTreatedAsStringOut , AnyLog)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | AllOutputConditionsMask | DefaultTreatedAsValueOut, AnyLog)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask, AnyLog)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask , AnyJson)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe\u000a
                    operators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
          }
          , new StringLikeExpect<char[]>
                ("For within these strings was bound the flexibility, mutability and the operators to govern each language".ToCharArray()
               , "{0,0/ /\n/[1..^1]}", formatFlags: AsCollection)
                {
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut
                             , Log | Compact | Pretty)
                      , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut
                             , Log | Compact | Pretty)
                      , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                      , """
                        within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                        \u000aoperators\u000ato\u000agovern\u000aeach
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan)
                      , """
                        "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                        \u000aoperators\u000ato\u000agovern\u000aeach"
                        """.RemoveLineEndings()
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut
                             , Log | Compact | Pretty)
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
          , new StringLikeExpect<char[]>("But they were all of them deceived, for another string was made.".ToCharArray()
                                       , "{0,0/,//[1..]}")
                {
                    {
                        new EK(IsContentType | AcceptsChars | CallsAsSpan  | DefaultTreatedAsValueOut)
                      , " for another string was made."
                    }
                   ,
                    {
                        new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsStringOut, AnyLog)
                      , "\" for another string was made.\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | AllOutputConditionsMask, AnyLog)
                      , " for another string was made."
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask | DefaultTreatedAsStringOut)
                      , "\" for another string was made.\""
                    }
                }
          , new StringLikeExpect<char[]>("But they were all of them deceived, for another string was made.".ToCharArray()
                                       , "{0,0/,//[1..]}", formatFlags: AsCollection)
            {
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                  , " for another string was made."
                }
               ,
                {
                    new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan)
                  , "\" for another string was made.\""
                }
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
          , new StringLikeExpect<char[]>
                (("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                 "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                 "languages with.").ToCharArray(), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
                {
                    {
                        new EK( IsContentType | AcceptsChars | DefaultTreatedAsValueOut)
                      , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                    }
                   ,{
                        new EK( IsContentType | AcceptsChars)
                      , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                    }
                   ,{
                        new EK(AcceptsChars | AllOutputConditionsMask, AnyLog)
                      , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsSpan | AllOutputConditionsMask)
                      , "\" after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String\""
                    }
                }
          , new StringLikeExpect<char[]>
                (("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                  "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                  "languages with.").ToCharArray(), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100, formatFlags: AsCollection)
                {
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                      , " after many Moons of playing Doom! the Dotnet Lord Hejlsberg forged a master String"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
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
          , new StringLikeExpect<char[]>
                (("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                 "time confine them").ToCharArray(), "{0[^40..^0]}")
                {
                    {
                        new EK( IsContentType | AcceptsChars | CallsAsSpan | DefaultTreatedAsValueOut)
                      , "and in the dustbins of time confine them"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | CallsAsSpan)
                      , "\"and in the dustbins of time confine them\""
                    }
                   ,
                    {
                        new EK(AcceptsChars |  AllOutputConditionsMask, AnyLog)
                      , "and in the dustbins of time confine them"
                    }
                   ,
                    {
                        new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             | DefaultTreatedAsStringOut)
                      , "\"and in the dustbins of time confine them\""
                    }
                }
          , new StringLikeExpect<char[]>
                (("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                  "time confine them").ToCharArray(), "{0[^40..^0]}", formatFlags: AsCollection)
                {
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut)
                      , "and in the dustbins of time confine them"
                    }
                   ,
                    {
                        new EK(IsContentType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut)
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
        };
}
