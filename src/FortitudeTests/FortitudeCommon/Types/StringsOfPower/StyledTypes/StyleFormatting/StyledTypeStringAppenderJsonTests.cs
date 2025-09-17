// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StyledToString.StyledTypes.TestData;

namespace FortitudeTests.FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

[TestClass]
[NoMatchingProductionClass]
public class StyledTypeStringAppenderJsonTests
{


    [TestMethod]
    public void SerializingCompactAccreditedInstructorDeliversMatchesJsonSerializer()
    {
        var objectGraph = new SchoolForUnrelatedStudiesTestData();
        
        var shakespeare = objectGraph.Shakespeare;
        
        var shakeJsonSer = JsonSerializer.Serialize(shakespeare, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });

        Console.Out.WriteLine("Json Serializer");
        Console.Out.WriteLine(shakeJsonSer);
        
        
        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(StringStyle.Json | StringStyle.Compact);
        styledStringBuilder.Settings = new StyleOptions
        {
            WriteKeyValuePairsAsCollection = true
        };
        shakespeare.RevealState(styledStringBuilder);
        var shakeStyled = styledStringBuilder.WriteBuffer.ToString();
        
        Console.Out.WriteLine("StyledTypeStringAppender");
        Console.Out.WriteLine(shakeStyled);
        
    }
    
    [TestMethod]
    public void SerializingPrettyAccreditedInstructorDeliversMatchesJsonSerializer()
    {
        var objectGraph = new SchoolForUnrelatedStudiesTestData();
        
        var shakespeare = objectGraph.Shakespeare;
        
        var shakeJsonSer = JsonSerializer.Serialize(shakespeare, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
            , WriteIndented = true
        });

        Console.Out.WriteLine("Json Serializer");
        Console.Out.WriteLine(shakeJsonSer);
        
        
        var styledStringBuilder = new TheOneString();
        styledStringBuilder.ClearAndReinitialize(StringStyle.Json | StringStyle.Pretty);
        styledStringBuilder.Settings = new StyleOptions
        {
            WriteKeyValuePairsAsCollection = true
           ,  EnableColumnContentWidthWrap = false
        };
        shakespeare.RevealState(styledStringBuilder);
        var shakeStyled = styledStringBuilder.WriteBuffer.ToString();
        
        Console.Out.WriteLine("StyledTypeStringAppender");
        Console.Out.WriteLine(shakeStyled);
        
    }
}
