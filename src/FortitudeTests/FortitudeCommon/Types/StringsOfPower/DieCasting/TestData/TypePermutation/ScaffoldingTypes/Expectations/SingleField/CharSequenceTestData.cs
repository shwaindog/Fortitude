// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public class CharSequenceTestData
{
    private static PositionUpdatingList<IStringLikeExpectation>? allCharSequenceExpectations;

    public static PositionUpdatingList<IStringLikeExpectation> AllCharSequenceExpectations => allCharSequenceExpectations ??=
        new PositionUpdatingList<IStringLikeExpectation>(typeof(CharSequenceTestData))
        {
            // ICharSequence
            // string
            new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>( new CharArrayStringBuilder(""), "", true, new CharArrayStringBuilder("0"))
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
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
                         | DefaultBecomesFallbackValue)
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
          , new StringLikeExpect<MutableString, MutableString>
                (new MutableString(""), "", true, new MutableString("0"), formatFlags: AsCollection)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut), "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero
                         | DefaultBecomesFallbackValue)
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
                         | DefaultBecomesFallbackValue)
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
                         | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                  , "\"\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsStringOut
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                (null, "", true, new CharArrayStringBuilder(""))
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue  | DefaultBecomesFallbackString)
                  , "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
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
          , new StringLikeExpect<MutableString, MutableString>
                (null, "", true, new MutableString(""))
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"\"" }
              , { new EK(SimpleType | CallsViaMatch), "null" }
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                (null, "", false, [], 10, 50)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
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
          , new StringLikeExpect<MutableString, MutableString>
                (null, "", false, [], 10, 50)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
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
          , new StringLikeExpect<MutableString, MutableString>
                (null, "", false, new MutableString(""), 10, 50)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                (null, "", true, new CharArrayStringBuilder("0"), -1, -10)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue), "" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "0" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites), "null" },
            }
          , new StringLikeExpect<MutableString, MutableString>
                (null, "", true, new MutableString("0"), -1, -10)
            {
                { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut), "null" }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull)
                  , "null"
                }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"0\"" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "0" }
              , { new EK(AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
              , { new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites), "null" },
            }
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                ("It", "[{0}]", false, "0", 3, 2)
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
            }
          , new StringLikeExpect<MutableString, MutableString>
                (new MutableString("It"), "\"{0}\"", false, new MutableString(), 3, 2, AsCollection)
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
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence)
                  , "\"\\u0022\\u0022\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                         , Log | Compact | Pretty)
                  , "\"\""
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
          , new StringLikeExpect<MutableString>("began", "[{0[8..10]}]", false, "0", 10, 5
                                                )
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
                    new EK(AcceptsChars | CallsAsReadOnlySpan | AlwaysWrites | DefaultTreatedAsStringOut)
                  , """
                    "[]"
                    """
                }
            }
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                (new CharArrayStringBuilder("began"), "'{0[8..10]}'", false, [], 10, 5
               , formatFlags: AsCollection)
            {
                { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "''" }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"''\"" }
               ,
                {
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>( new CharArrayStringBuilder( "with"), "\"{0[8..10]}\"")
            {
                { new EK(SimpleType | CallsViaMatch | AcceptsString, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(SimpleType | CallsViaMatch | AcceptsString | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsString | CallsAsReadOnlySpan, Log | Compact | Pretty), "\"\"" }

              , { new EK(AcceptsChars | AcceptsString | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsString | CallsAsReadOnlySpan)
                  , "\"\\u0022\\u0022\""
                }
               , { new EK(AcceptsChars | CallsAsReadOnlySpan | AllOutputConditionsMask ) , "\"\"" }
            }
          , new StringLikeExpect<MutableString>(new MutableString("with"), "\"{0[8..10]}\"", formatFlags: AsCollection)
            {
                { new EK(SimpleType | AcceptsChars | CallsViaMatch | CallsAsSpan, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | CallsViaMatch | DefaultBecomesFallbackValue | DefaultTreatedAsValueOut), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | CallsViaMatch | DefaultBecomesFallbackValue), "\"\\u0022\\u0022\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | CallsAsSpan, Log | Compact | Pretty), "\"\"" }
              , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | CallsAsSpan | DefaultTreatedAsValueOut), "\"\"" }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | CallsAsSpan), """""
                                                                                           "\u0022\u0022"
                                                                                           """""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty)
                  , "\"\""
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
          , new StringLikeExpect<MutableString, MutableString>( new MutableString("the"), "{0}", true, new MutableString(""), -1, -10)
            {
                { new EK(AcceptsChars | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
              , { new EK(SimpleType | AcceptsChars | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                (new CharArrayStringBuilder("the"), "{0}", true, new CharArrayStringBuilder(""), -1, -10, AsCollection)
                {
                    { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultBecomesZero), "0" }
                   ,
                    {
                        new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut |
                               DefaultBecomesNull)
                      , "null"
                    }
                  , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), "" }
                  , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\"\"" }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
                      , "\"\""
                    }
                   ,
                    {
                        new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut
                             , Json | Compact | Pretty)
                      , "[]"
                    }
                }
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>("forging", "{0,10}", true,
                                                                                 new CharArrayStringBuilder("orging"), 1)
            {
                { new EK(AcceptsChars | CallsAsReadOnlySpan | DefaultTreatedAsValueOut), "    orging" }
              , { new EK(AcceptsChars | AlwaysWrites | NonNullWrites | DefaultTreatedAsStringOut), "\"    orging\"" }
            }
          , new StringLikeExpect<MutableString, MutableString>
                (new MutableString("forging"), "[{0,10}]", true, new MutableString("orging"), 1
               , formatFlags: AsCollection)
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
               ,
                {
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
          , new StringLikeExpect<MutableString>("It began with the forging of the Great Strings.", "[{0}]")
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
          , new StringLikeExpect<CharArrayStringBuilder>
                (new CharArrayStringBuilder("It began with the forging of the Great Strings."), "[{0}]"
               , formatFlags: AsCollection)
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
          , new StringLikeExpect<CharArrayStringBuilder>
                ( "Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings."
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
          , new StringLikeExpect<MutableString>
                (new MutableString("Three were given to the Assembly Programmers, impractical, wackiest and hairiest of all beings.")
               , "3{0[5..]}", formatFlags: AsCollection)
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
                        new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             , Json | Compact)
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
          , new StringLikeExpect<MutableString>
                ("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls."
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
          , new StringLikeExpect<CharArrayStringBuilder>
                (new CharArrayStringBuilder("Seven to the Cobol-Lords, eventually great Bitcoin miners and great cardigan wearers of the mainframe halls.")
               , "'{0,30}'", fromIndex: -1, length: 24, formatFlags: AsCollection)
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
          , new StringLikeExpect<CharArrayStringBuilder>
                ("And nine, nine strings were gifted to the race of C++ coders, who above all else desired unchecked memory access power. "
               , "***\"{0[1..^1]}\"###" , fromIndex: 9, length: 41)
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
          , new StringLikeExpect<MutableString>
                (new MutableString
                     ("And nine, nine strings were gifted to the race of C++ coders, " +
                      "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"###"
               , fromIndex: 9, length: 41, formatFlags: AsCollection)
                {
                    {
                        new EK(SimpleType | AcceptsCharSequence | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                      , "***\"nine strings were gifted to the race of\"###"
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsCharSequence | DefaultTreatedAsStringOut, Log | Compact | Pretty)
                      , "\"***\"nine strings were gifted to the race of\"###\""
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsCharSequence | DefaultTreatedAsValueOut, Json | Compact | Pretty)
                      , "***\\u0022nine strings were gifted to the race of\\u0022###"
                    }
                   ,
                    {
                        new EK(SimpleType | AcceptsCharSequence)
                      , "\"***\\u0022nine strings were gifted to the race of\\u0022###\""
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
          , new StringLikeExpect<MutableString>
                (new MutableString("For within these strings was bound the flexibility, mutability and the operators to govern each language")
                                       , "{0,0/ /\n/[1..^1]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsValueOut
                         , Log | Compact | Pretty)
                  , "within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsStringOut
                         , Log | Compact | Pretty)
                  , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsValueOut)
                  , """
                    within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan)
                  , """
                    "within\u000athese\u000astrings\u000awas\u000abound\u000athe\u000aflexibility,\u000amutability\u000aand\u000athe
                    \u000aoperators\u000ato\u000agovern\u000aeach"
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | CallsAsReadOnlySpan | AcceptsString | CallsAsSpan | DefaultTreatedAsStringOut
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
          , new StringLikeExpect<CharArrayStringBuilder>
                (new CharArrayStringBuilder
                     ("For within these strings was bound the flexibility, mutability and the operators to govern each language")
               , "{0,0/ /\n/[1..^1]}", formatFlags: AsCollection)
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
                   ,
                    {
                        new EK(AcceptsChars | CallsAsReadOnlySpan | AcceptsString | AlwaysWrites | NonDefaultWrites | NonNullWrites
                             | NonNullAndPopulatedWrites, Log | Compact | Pretty)
                      , "\"within\nthese\nstrings\nwas\nbound\nthe\nflexibility,\nmutability\nand\nthe\noperators\nto\ngovern\neach\""
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
          , new StringLikeExpect<CharArrayStringBuilder>
                ( new CharArrayStringBuilder("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}")
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
          , new StringLikeExpect<MutableString>
                (new MutableString("But they were all of them deceived, for another string was made."), "{0,0/,//[1..]}"
               , formatFlags: AsCollection)
                {
                    { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut), " for another string was made." }
                  , { new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut), "\" for another string was made.\"" }
                   ,
                    {
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
          , new StringLikeExpect<MutableString, MutableString>
                ( new MutableString( "Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                 "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                 "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100)
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
          , new StringLikeExpect<CharArrayStringBuilder>
                (new CharArrayStringBuilder
                     ("Deep in the land of Redmond, after many Moons of playing Doom, the Dotnet Lord Hejlsberg forged a master " +
                      "String, and into this string he poured his unambiguity, his immutability desires and his will to replace all " +
                      "languages with."), "{0,/,/!/[1..3]}", fromIndex: 16, length: 100
               , formatFlags: AsCollection)
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
                        new EK(AcceptsChars | AcceptsCharSequence | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonDefaultWrites |
                               NonNullWrites |
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
          , new StringLikeExpect<CharArrayStringBuilder, CharArrayStringBuilder>
                ( new CharArrayStringBuilder("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                 "time confine them"), "{0[^40..^0]}")
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
          , new StringLikeExpect<MutableString>
                (new MutableString
                     ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                      "time confine them"), "{0[^40..^0]}"
               , formatFlags: AsCollection)
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
        };

}
