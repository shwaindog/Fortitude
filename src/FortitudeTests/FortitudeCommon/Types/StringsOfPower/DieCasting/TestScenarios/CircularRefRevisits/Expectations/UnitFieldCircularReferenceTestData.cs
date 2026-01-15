// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.Expectations;

public static class UnitFieldCircularReferenceTestData
{
    private static PositionUpdatingList<ISingleFieldExpectation>? circularReferenceExpectations;

    public static OrderedBranchNode<IChildNode> SelfReferencing
    {
        get
        {
            var selfReferencing        = new OrderedBranchNode<IChildNode>();
            selfReferencing.Parent = selfReferencing;
            return selfReferencing;
        }
    }
    
    public static OrderedBranchNode<IChildNode> DualReferencingPair
    {
        get
        {
            var child               = new OrderedBranchNode<IChildNode>();
            var dualReferencingPair = new OrderedBranchNode<IChildNode>([child]);
            return dualReferencingPair;
        }
    }
    
    public static BinaryBranchNode<LeafNode> SecondFieldSame
    {
        get
        {
            var child               = new LeafNode("SameChild");
            var secondFieldSame = new BinaryBranchNode<LeafNode>("SameOnLeftAndRight", child, child );
            return secondFieldSame;
        }
    }
    
    public static BinaryBranchNode<BinaryBranchNode<LeafNode>> SeparateObjectsShareSame
    {
        get
        {
            var sameChild       = new LeafNode("SameChild");
            var firstDiffChild  = new LeafNode("FirstDiffChild");
            var secondDiffChild = new LeafNode("SecondDiffChild");
            var firstBranch     = new BinaryBranchNode<LeafNode>("FirstBranch", sameChild, firstDiffChild); 
            var secondBranch    = new BinaryBranchNode<LeafNode>("secondBranch", secondDiffChild, sameChild); 
            var root            = new BinaryBranchNode<BinaryBranchNode<LeafNode>>("SameOnLeftAndRight", firstBranch, secondBranch );
            return root;
        }
    }

    public static PositionUpdatingList<ISingleFieldExpectation> CiruclarReferenceExpectations => circularReferenceExpectations ??=
        new PositionUpdatingList<ISingleFieldExpectation>(typeof(UnitFieldCircularReferenceTestData))
        {
            // Version and Version?  (Class)
            new FieldExpect<OrderedBranchNode<IChildNode>>(SelfReferencing)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "0.0" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"0.0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty)
                  , "0.0"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"0.0\""
                }
            }
          , new FieldExpect<Version>(null, "{0}", true, new Version())
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackString | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , "0.0"
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"0.0\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue |
                           DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , "0.0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"0.0\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                         , CompactLog | Pretty)
                  , "0"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
              , { new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
            }
          , new FieldExpect<Version, string>(null, "", true, "")
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , ""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString, CompactLog | Pretty)
                  , ""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                         | DefaultBecomesFallbackString)
                  , "\"\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesZero
                         , CompactLog | Pretty)
                  , "0"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesZero), "\"0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut | DefaultBecomesZero
                         | DefaultBecomesNull)
                  , "null"
                }
            }
          , new FieldExpect<Version>(new Version(1, 1))
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "1.1" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"1.1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty)
                  , "1.1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"1.1\""
                }
            }
          , new FieldExpect<Version>(new Version("1.2.3.4"), "'{0}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'1.2.3.4'" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsStringOut), "\"'1.2.3.4'\"" }
               ,
                {
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'1.0'" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'1.0'\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty), "'1.0'" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'1.0'\"" }
            }
          , new FieldExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
            {
                { new EK(IsContentType | AcceptsSpanFormattable, CompactLog | Pretty), "\"          5.6.7.8\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"          5.6.7.8\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
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
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "0.0.0.0" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"0.0.0.0\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty)
                  , "0.0.0.0"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"0.0.0.0\""
                }
            }
          , new FieldExpect<IPAddress, string>(null, "", true, "")
            {
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , ""
                }
              , { new EK(IsContentType | CallsViaMatch | DefaultBecomesFallbackValue | DefaultBecomesFallbackString), "\"\"" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"\""
                }
                // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
                // So the following will only match when both the scaffold and the following have both.
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                         , CompactLog | Pretty)
                  , "0"
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero)
                  , "\"0\""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , ""
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackValue), "\"\"" }
                // The following covers the others that would return null.
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
            }
          , new FieldExpect<IPAddress>(IPAddress.Loopback)
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "127.0.0.1" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"127.0.0.1\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty)
                  , "127.0.0.1"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"127.0.0.1\""
                }
            }
          , new FieldExpect<IPAddress>(new IPAddress([192, 168, 0, 1]), "'{0}'", true
                                     , new IPAddress([192, 168, 0, 1]))
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'192.168.0.1'" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'192.168.0.1'\"" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, CompactLog | Pretty), "'192.168.0.1'" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites | NonNullWrites, Json | Compact | Pretty), "\"'192.168.0.1'\"" }
            }
          , new FieldExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'")
            {
                { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty), "'255.255.255.254'" }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'255.255.255.254'\"" }
               ,
                {
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
                { new EK(IsContentType | AcceptsSpanFormattable, CompactLog | Pretty), "\"      255.255.0.0\"" }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut), "\"      255.255.0.0\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable)
                  , """
                    "\u0022      255.255.0.0\u0022"
                    """
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                  , "\"      255.255.0.0\""
                }
            }

            //  Uri and Uri?
          , new FieldExpect<Uri>(new Uri("https://learn.microsoft.com/en-us/dotnet/api"))
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "https://learn.microsoft.com/en-us/dotnet/api"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"https://learn.microsoft.com/en-us/dotnet/api\"" }
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
                { new EK(IsContentType | CallsViaMatch | DefaultBecomesNull), "null" }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsValueOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString
                         , CompactLog | Pretty)
                  , ""
                }
               ,
                {
                    new EK(IsContentType | CallsViaMatch | DefaultTreatedAsStringOut | DefaultBecomesFallbackValue | DefaultBecomesFallbackString)
                  , "\"\""
                }
                // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
                // So the following will only match when both the scaffold and the following have both.
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallbackValue), "null" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                         , CompactLog | Pretty)
                  , "0"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero), "\"0\"" }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallbackValue
                         , CompactLog | Pretty)
                  , ""
                }
               ,
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallbackString
                         | DefaultBecomesFallbackValue)
                  , "\"\""
                }
                // The following covers the others that would return null.
              , { new EK(IsContentType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
              , { new EK(AcceptsSpanFormattable | AlwaysWrites), "null" }
              , { new EK(AcceptsSpanFormattable), "null" }
            }
          , new FieldExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'"
                               , true, new Uri("https://github.com/shwaindog/Fortitude"))
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "'https://github.com/shwaindog/Fortitude'"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'https://github.com/shwaindog/Fortitude'\"" }
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
                    {
                        new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                      , "https://github.com/shwaindog/Fortitude"
                    }
                  , { new EK(IsContentType | AcceptsSpanFormattable), "\"https://github.com/shwaindog/Fortitude\"" }
                   ,
                    {
                        new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             , CompactLog | Pretty)
                      , "https://github.com/shwaindog/Fortitude"
                    }
                   ,
                    {
                        new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                             , Json | Compact | Pretty)
                      , "\"https://github.com/shwaindog/Fortitude\""
                    }
                }
          , new FieldExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
            {
                {
                    new EK(IsContentType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, CompactLog | Pretty)
                  , "'https://en.wikipedia.org/wiki/Rings_of_Power'"
                }
              , { new EK(IsContentType | AcceptsSpanFormattable), "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\"" }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , CompactLog | Pretty)
                  , "'https://en.wikipedia.org/wiki/Rings_of_Power'"
                }
               ,
                {
                    new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                         , Json | Compact | Pretty)
                  , "\"'https://en.wikipedia.org/wiki/Rings_of_Power'\""
                }
            }
        };
}
