// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Numerics;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.UnitFieldsContentTypes;

public class DecimalNumberTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? decimalNumberExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> DecimalNumberExpectations => decimalNumberExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(DecimalNumberTestData))
        {
          
        // Half
        new FieldExpect<Half>(Half.Zero)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<Half>(Half.MinValue / (Half)2.0, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-32750\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-32750"
            }
        }
      , new FieldExpect<Half>(Half.One, "", true, Half.One)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<Half>(Half.NaN, "", true, Half.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<Half>(Half.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "\"NaN\"" }
        }
      , new FieldExpect<Half>(Half.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'65500'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'65500'"
            }
        }
      , new FieldExpect<Half>(Half.MinValue, "'{0:c}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$65,504.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$65,504.00'"
            }
        }
      , new FieldExpect<Half>((Half)(Math.E * 10.0), "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"27\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "27"
            }
        }
      , new FieldExpect<Half>((Half)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14                \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14                \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.14                \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14                \""
            }
        }

        // Half?
      , new FieldExpect<Half?>(Half.Zero)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<Half?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<Half?>(Half.MinValue / (Half)2.0, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-32750\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-32750"
            }
        }
      , new FieldExpect<Half?>(Half.One, "", true, Half.One)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<Half?>(Half.NaN, "", true, Half.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<Half?>(Half.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites), "\"NaN\"" }
        }
      , new FieldExpect<Half?>(Half.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'65500'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'65500'"
            }
        }
      , new FieldExpect<Half?>(Half.MinValue, "'{0:c}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$65,504.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$65,504.00'"
            }
        }
      , new FieldExpect<Half?>((Half)(Math.E * 10.0), "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"27\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "27"
            }
        }
      , new FieldExpect<Half?>((Half)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14                \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14                \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.14                \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"3.14                \""
            }
        }
        
        // float
      , new FieldExpect<float>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<float>(1 - float.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.4028235E+38\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "3.4028235E+38"
            }
        }
      , new FieldExpect<float>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<float>(float.NaN, "", true, float.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites), "\"NaN\"" }
        }
      , new FieldExpect<float>(float.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u0022NaN\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<float>(float.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'3.4028235E+38'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'3.4028235E+38'"
            }
        }
      , new FieldExpect<float>(float.MinValue, "'{0:c}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , "\"'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float>((float)Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.1415927           \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.1415927           \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.1415927           \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.1415927           \""
            }
        }

        // float?
      , new FieldExpect<float?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<float?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<float?>(1 - float.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.4028235E+38\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "3.4028235E+38"
            }
        }
      , new FieldExpect<float?>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<float?>(float.NaN, "", true, float.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty) , "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites) , "\"NaN\"" }
        }
      , new FieldExpect<float?>(float.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<float?>(float.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'3.4028235E+38'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'3.4028235E+38'"
            }
        }
      , new FieldExpect<float?>(float.MinValue, "'{0:c}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , "\"'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FieldExpect<float?>((float)Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<float?>((float)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.1415927           \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.1415927           \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.1415927           \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.1415927           \""
            }
        }
        
        // double
      , new FieldExpect<double>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<double>(1 - double.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1.7976931348623157E+308\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "1.7976931348623157E+308"
            }
        }
      , new FieldExpect<double>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<double>(double.NaN, "", true, double.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<double>(double.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<double>(double.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.7976931348623157E+308'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1.7976931348623157E+308'"
            }
        }
      , new FieldExpect<double>(double.MinValue, "'{0:c}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , """
                '-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<double>(Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<double>(Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.141592653589793   \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.141592653589793   \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.141592653589793   \u0022"
                """ 
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.141592653589793   \""
            }
        }

        // double?
      , new FieldExpect<double?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<double?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<double?>(1 - double.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1.7976931348623157E+308\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "1.7976931348623157E+308"
            }
        }
      , new FieldExpect<double?>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<double?>(double.NaN, "", true, double.NaN)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "NaN" }
          , { new EK(IsContentType | AcceptsSpanFormattable), "\"NaN\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "NaN" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"NaN\"" }
        }
      , new FieldExpect<double?>(double.NaN, "\"{0}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty), "\"NaN\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable), 
                """
                "\u0022NaN\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"NaN\""
            }
        }
      , new FieldExpect<double?>(double.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.7976931348623157E+308'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'1.7976931348623157E+308'"
            }
        }
      , new FieldExpect<double?>(double.MinValue, "'{0:c}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , """
                '-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,
                766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,
                328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,
                881,250,404,026,184,124,858,368.00'
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<double?>(Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<double?>(Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.141592653589793   \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.141592653589793   \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.141592653589793   \u0022"
                """ 
            }
         ,  {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.141592653589793   \""
            }
        }

        // decimal
      , new FieldExpect<decimal>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<decimal>(decimal.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-79228162514264337593543950335\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-79228162514264337593543950335"
            }
        }
      , new FieldExpect<decimal>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<decimal>(decimal.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'79228162514264337593543950335'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'79228162514264337593543950335'"
            }
        }
      , new FieldExpect<decimal>(decimal.MinValue, "'{0:c}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$79,228,162,514,264,337,593,543,950,335.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal>((decimal)Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<decimal>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14159265358979    \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"3.14159265358979    \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.14159265358979    \u0022"
                """
          }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14159265358979    \""
            }
        }

        // decimal?
      , new FieldExpect<decimal?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "0" }
        }
      , new FieldExpect<decimal?>(null, "", true)
        {
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "0" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"0\"" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<decimal?>(decimal.MinValue, "R")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"-79228162514264337593543950335\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "-79228162514264337593543950335"
            }
        }
      , new FieldExpect<decimal?>(1, "", true, 1)
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut), "1" }
        }
      , new FieldExpect<decimal?>(decimal.MaxValue, "'{0:G}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'79228162514264337593543950335'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'79228162514264337593543950335'"
            }
        }
      , new FieldExpect<decimal?>(decimal.MinValue, "'{0:c}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'-$79,228,162,514,264,337,593,543,950,335.00'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "'-$79,228,162,514,264,337,593,543,950,335.00'"
            }
        }
      , new FieldExpect<decimal?>((decimal)Math.E * 1_000_000, "N0")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"2,718,282\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "2,718,282"
            }
        }
      , new FieldExpect<decimal?>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"3.14159265358979    \"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Json | Compact | Pretty)
              , "\"3.14159265358979    \"" 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u00223.14159265358979    \u0022"
                """ 
            }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"3.14159265358979    \"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut)
              , "\"3.14159265358979    \""
            }
        }

        // Complex
      , new FieldExpect<Complex>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<0; 0>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c0; 0\u003e"
                """
            }
           ,{ new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<0; 0>" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c0; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(32000, "{0:N0}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32,000; 0>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32,000; 0>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "<32,000; 0>"
            }
         , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(32.0d, 1), "N0", true
                               , new Complex(32.0d, 1))
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32; 1>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32; 1>\"" }
          , {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u003c32; 1\u003e"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<32; 1>" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c32; 1\u003e"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'<1,000,000.00; 1,000,000.00>'\"" }
          , {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
        }
      , new FieldExpect<Complex>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , """
                "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """ 
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """ 
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<Complex>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<3.141592653589793; 2.718281828459045>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "\u0022\u003c3.141592653589793; 2.718281828459045\u003e\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
        }

        // Complex?
      , new FieldExpect<Complex?>(0, "")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<0; 0>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c0; 0\u003e"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     , Log | Compact | Pretty)
              , "<0; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut
                     , Json | Compact | Pretty)
              , """
                "\u003c0; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(null, "", true)
        {  
            { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesNull), "null" }
          , { new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "<0; 0>" }
          , { new EK(IsContentType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(IsContentType | CallsViaMatch |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                   , Json | Compact | Pretty), "\"\\u003c0; 0\\u003e\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesZero, Log | Compact | Pretty), "0" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, Log | Compact | Pretty), "<0; 0>" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut |  DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue, Log | Compact | Pretty), "\"<0; 0>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable |  DefaultBecomesFallbackString | DefaultBecomesFallbackValue)
              , "\"\\u003c0; 0\\u003e\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero)
              , "<0; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "null"
            }
        }
      , new FieldExpect<Complex?>(32000, "{0:N0}")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32,000; 0>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32,000; 0>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "<32,000; 0>"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c32,000; 0\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "<32; 1>" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<32; 1>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "\u003c32; 1\u003e"
                """
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "<32; 1>" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , """
                "\u003c32; 1\u003e"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(999999.999, 999999.999), "'{0:N2}'")
        {
            { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'" }
          , { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"'<1,000,000.00; 1,000,000.00>'\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'<1,000,000.00; 1,000,000.00>'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c1,000,000.00; 1,000,000.00\u003e'"
                """
            }
        }
      , new FieldExpect<Complex?>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty)
              , """
                "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(IsContentType | AcceptsSpanFormattable)
              , """
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , """
                '<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'
                """.RemoveLineEndings()
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "'\u003c-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632
                ,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389
                ,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299
                ,881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476
                ,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986
                ,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797
                ,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000\u003e'"
                """.RemoveLineEndings()
            }
        }
      , new FieldExpect<Complex?>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            { new EK(IsContentType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"<3.141592653589793; 2.718281828459045>\"" }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
          , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut)
              , """
                "\u0022\u003c3.141592653589793; 2.718281828459045\u003e\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , """
                "\u003c3.141592653589793; 2.718281828459045\u003e"
                """
            }
        }
    };
}
