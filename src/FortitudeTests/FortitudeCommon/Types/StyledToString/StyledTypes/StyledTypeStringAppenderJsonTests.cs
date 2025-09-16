// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.Options;

namespace FortitudeTests.FortitudeCommon.Types.StyledToString.StyledTypes;

[TestClass]
[NoMatchingProductionClass]
public class StyledTypeStringAppenderJsonTests
{


    [TestMethod]
    public void SerializingCompactAccreditedInstructorDeliversMatchesJsonSerializer()
    {
        var objectGraph = new SchoolForUnrelatedStudies();
        
        var shakespeare = objectGraph.Shakespeare;
        
        var shakeJsonSer = JsonSerializer.Serialize(shakespeare, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });

        Console.Out.WriteLine("Json Serializer");
        Console.Out.WriteLine(shakeJsonSer);
        
        
        var styledStringBuilder = new StyledTypeStringAppender();
        styledStringBuilder.ClearAndReinitialize(StringBuildingStyle.Json | StringBuildingStyle.Compact);
        styledStringBuilder.Settings = new StyleOptions
        {
            WriteKeyValuePairsAsCollection = true
        };
        shakespeare.ToString(styledStringBuilder);
        var shakeStyled = styledStringBuilder.WriteBuffer.ToString();
        
        Console.Out.WriteLine("StyledTypeStringAppender");
        Console.Out.WriteLine(shakeStyled);
        
    }


    [TestMethod]
    public void SerializingPrettyAccreditedInstructorDeliversMatchesJsonSerializer()
    {
        var objectGraph = new SchoolForUnrelatedStudies();
        
        var shakespeare = objectGraph.Shakespeare;
        
        var shakeJsonSer = JsonSerializer.Serialize(shakespeare, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
            , WriteIndented = true
        });

        Console.Out.WriteLine("Json Serializer");
        Console.Out.WriteLine(shakeJsonSer);
        
        
        var styledStringBuilder = new StyledTypeStringAppender();
        styledStringBuilder.ClearAndReinitialize(StringBuildingStyle.Json | StringBuildingStyle.Pretty);
        styledStringBuilder.Settings = new StyleOptions
        {
            WriteKeyValuePairsAsCollection = true
           ,  EnableColumnContentWidthWrap = false
        };
        shakespeare.ToString(styledStringBuilder);
        var shakeStyled = styledStringBuilder.WriteBuffer.ToString();
        
        Console.Out.WriteLine("StyledTypeStringAppender");
        Console.Out.WriteLine(shakeStyled);
        
    }
}
