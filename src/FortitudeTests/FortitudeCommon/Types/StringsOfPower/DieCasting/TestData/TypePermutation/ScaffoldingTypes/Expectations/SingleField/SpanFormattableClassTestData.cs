// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;

public static class SpanFormattableClassTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? spanFormattableClassExpectations;

    public static PositionUpdatingList<ISingleFieldExpectation> SpanFormattableClassExpectations => spanFormattableClassExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(SpanFormattableClassTestData))
        {
            
        // Version and Version?  (Class)
       new FieldExpect<Version>(new Version())
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "0.0"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty) , "\"0.0\""
            }
        }
      , new FieldExpect<Version>(null, "{0}", true, new Version())
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackValue
                     , Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackValue), "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesNull | DefaultBecomesFallbackValue
                     , Log | Compact | Pretty) , "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue) , "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                     , Log | Compact | Pretty), "0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                     , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<Version, string>(null, "", true, "")
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackValue), "\"\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                   , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesNull)
              , "null"
            }
        }
      , new FieldExpect<Version>(new Version(1, 1))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "1.1" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"1.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty) , "1.1"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"1.1\""
            }
        }
      , new FieldExpect<Version>(new Version("1.2.3.4"), "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1.2.3.4'" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.2.3.4'\"" }
           , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'1.2.3.4'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'1.2.3.4'\""
            }
        }
      , new FieldExpect<Version>(new Version(1, 0), "'{0}'", true
                               , new Version(1, 0))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'1.0'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'1.0'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "'1.0'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'1.0'\"" }
        }
      , new FieldExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty), "\"          5.6.7.8\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          5.6.7.8\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022          5.6.7.8\u0022"
                """
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , "\"          5.6.7.8\""
            }
        }

        //  IPAddress and IPAddress?
      , new FieldExpect<IPAddress>(new IPAddress("\0\0\0\0"u8))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "0.0.0.0" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"0.0.0.0\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "0.0.0.0"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"0.0.0.0\""
            }
        }
      , new FieldExpect<IPAddress, string>(null, "", true, "")
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | CallsViaMatch | DefaultBecomesFallbackValue), "\"\"" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                   , Log | Compact | Pretty) , "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero )
              , "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                   , Log | Compact | Pretty) , "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Loopback)
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "127.0.0.1" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"127.0.0.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "127.0.0.1"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"127.0.0.1\""
            }
        }
      , new FieldExpect<IPAddress>(new IPAddress([192, 168, 0, 1]), "'{0}'", true
                                 , new IPAddress([192, 168, 0, 1]))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'192.168.0.1'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'192.168.0.1'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty), "'192.168.0.1'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'192.168.0.1'\"" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty), "'255.255.255.254'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'255.255.255.254'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'255.255.255.254'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'255.255.255.254'\""
            }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.0.0"), "\"{0,17}\"")
        {
            { new EK(SimpleType | AcceptsSpanFormattable, Log | Compact | Pretty ) , "\"      255.255.0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"      255.255.0.0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable)
              , """
                "\u0022      255.255.0.0\u0022"
                """
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
              , "\"      255.255.0.0\""
            }
        }

        //  Uri and Uri?
      , new FieldExpect<Uri>(new Uri("https://learn.microsoft.com/en-us/dotnet/api"))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                , "https://learn.microsoft.com/en-us/dotnet/api" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"https://learn.microsoft.com/en-us/dotnet/api\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "https://learn.microsoft.com/en-us/dotnet/api"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"https://learn.microsoft.com/en-us/dotnet/api\""
            }
        }
      , new FieldExpect<Uri, string>(null, "", false, "")
        {
            { new EK(SimpleType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue
                     , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                    , Log | Compact | Pretty), "0" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero), "\"0\"" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                        , Log | Compact | Pretty), "" }
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable), "null" }
        }
      , new FieldExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'"
                           , true, new Uri("https://github.com/shwaindog/Fortitude"))
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'https://github.com/shwaindog/Fortitude'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'https://github.com/shwaindog/Fortitude'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Log | Compact | Pretty)
              , "'https://github.com/shwaindog/Fortitude'"
            }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty)
              , "\"'https://github.com/shwaindog/Fortitude'\""
            }
        }
      , new
            FieldExpect<Uri>(new
                                 Uri("https://github.com/shwaindog/Fortitude/tree/main/src/FortitudeTests/FortitudeCommon/Types/StringsOfPower/DieCasting/TestData")
                           , "{0[..38]}")
            {
                { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                  , "https://github.com/shwaindog/Fortitude" 
                }
              , { new EK(SimpleType | AcceptsSpanFormattable), "\"https://github.com/shwaindog/Fortitude\"" }
              , {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Log | Compact | Pretty) , "https://github.com/shwaindog/Fortitude"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty) , "\"https://github.com/shwaindog/Fortitude\""
                }
            }
      , new FieldExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
        {
            { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'" }
          , { new EK(SimpleType | AcceptsSpanFormattable), "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'"
            }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\""
            }
        }
        };
}
