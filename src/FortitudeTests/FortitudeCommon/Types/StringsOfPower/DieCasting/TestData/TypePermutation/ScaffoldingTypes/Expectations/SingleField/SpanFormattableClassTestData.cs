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
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "0.0" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"0.0\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty) , "0.0"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty) , "\"0.0\""
            }
        }
      , new FieldExpect<Version>(null, "{0}", true, new Version())
        {
            { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString |  DefaultBecomesFallbackValue
                     , CompactLog | Pretty), "0.0" }
          , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"0.0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                     , CompactLog | Pretty) , "0.0" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString) 
            , "\"0.0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                     , CompactLog | Pretty), "0" }
         ,  { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
          , { new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<Version, string>(null, "", true, "")
        {
            { new EK(ContentType | CallsViaMatch | DefaultBecomesNull | DefaultBecomesNull), "null" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut |  DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                   , CompactLog | Pretty), "" }
          , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                   | DefaultBecomesFallbackString, CompactLog | Pretty), "" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                   | DefaultBecomesFallbackString), "\"\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                   , CompactLog | Pretty), "0" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero
                     | DefaultBecomesNull)
              , "null"
            }
        }
      , new FieldExpect<Version>(new Version(1, 1))
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "1.1" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"1.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty) , "1.1"
            }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact | Pretty)
              , "\"1.1\""
            }
        }
      , new FieldExpect<Version>(new Version("1.2.3.4"), "'{0}'")
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'1.2.3.4'" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.2.3.4'\"" }
           , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'1.0'" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"'1.0'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty), "'1.0'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'1.0'\"" }
        }
      , new FieldExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
        {
            { new EK(ContentType | AcceptsSpanFormattable, CompactLog | Pretty), "\"          5.6.7.8\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          5.6.7.8\"" }
          , { new EK(ContentType | AcceptsSpanFormattable)
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
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut,  CompactLog | Pretty), "0.0.0.0" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"0.0.0.0\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
            { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                     , CompactLog | Pretty), "" }
          , { new EK(ContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                   , CompactLog | Pretty) , "0" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero )
              , "\"0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                   , CompactLog | Pretty) , "" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Loopback)
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "127.0.0.1" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"127.0.0.1\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'192.168.0.1'" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"'192.168.0.1'\"" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty), "'192.168.0.1'" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'192.168.0.1'\"" }
        }
      , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'")
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'255.255.255.254'" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"'255.255.255.254'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
            { new EK(ContentType | AcceptsSpanFormattable, CompactLog | Pretty ) , "\"      255.255.0.0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut) , "\"      255.255.0.0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable)
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
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                , "https://learn.microsoft.com/en-us/dotnet/api" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"https://learn.microsoft.com/en-us/dotnet/api\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
            { new EK(ContentType | CallsViaMatch | DefaultBecomesNull), "null" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                     , CompactLog | Pretty), "" }
          , { new EK(ContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
            // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
            // So the following will only match when both the scaffold and the following have both.
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                    , CompactLog | Pretty), "0" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero), "\"0\"" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                        , CompactLog | Pretty), "" }
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackString 
                   | DefaultBecomesFallbackValue), "\"\"" }
            // The following covers the others that would return null.
          , { new EK(ContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
          , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
          , { new EK(AcceptsSpanFormattable), "null" }
        }
      , new FieldExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'"
                           , true, new Uri("https://github.com/shwaindog/Fortitude"))
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "'https://github.com/shwaindog/Fortitude'" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"'https://github.com/shwaindog/Fortitude'\"" }
           ,
            {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty)
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
                { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "https://github.com/shwaindog/Fortitude" 
                }
              , { new EK(ContentType | AcceptsSpanFormattable), "\"https://github.com/shwaindog/Fortitude\"" }
              , {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty) , "https://github.com/shwaindog/Fortitude"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty) , "\"https://github.com/shwaindog/Fortitude\""
                }
            }
      , new FieldExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
        {
            { new EK(ContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
              , "'https://en.wikipedia.org/wiki/Rings_of_Power'" }
          , { new EK(ContentType | AcceptsSpanFormattable), "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\"" }
          , {
                new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , CompactLog | Pretty)
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
