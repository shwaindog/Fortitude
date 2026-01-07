// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;


[NoMatchingProductionClass]
[TestClass]
public class ContentTypeMoldTestsCompactJsonAsStringTests : ContentTypeMoldCompactLogAsStringTests
{
    public override StringStyle TestStyle => Compact | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(CloakedBearerTestData.AllCloakedBearerExpectations[19], ScaffoldingRegistry.AllScaffoldingTypes[1213]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var compactJsonTemplate =
            expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>() && condition.HasAnyOf(DefaultTreatedAsStringOut)
                ? "\"{0}\""
                : "{0}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
            
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            if (expectValue == "null" && condition.HasAnyOf(DefaultBecomesNull) ) return "null";
        }
        else { expectValue = ""; }
        return string.Format(compactJsonTemplate, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        string compactJsonTemplate = 
            condition.HasAnyOf(DefaultTreatedAsStringOut)
            ? (expectation.CoreType.ImplementsInterfaceOrIs<IStringBearer>() 
                ?  "{{\\u0022{0}\\u0022:{1}}}"
                :  "\"{{\\u0022{0}\\u0022:{1}}}\"")
            : "{{\"{0}\":{1}}}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = expectValue.Replace("\"", "\\u0022");
        }
        else { expectValue = ""; }
        
        return string.Format(compactJsonTemplate, propertyName, expectValue);
    }
    //
    // private void SharedCompactJsonAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    // {
    //     logger.InfoAppend("Simple Value Type Single Value Field  Scaffolding Class - ")?
    //           .AppendLine(scaffoldingToCall.Name)
    //           .AppendLine()
    //           .AppendLine("Scaffolding Flags -")
    //           .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString().Replace(",", " |"))
    //           .FinalAppend("\n");
    //
    //     logger.WarnAppend("FormatExpectation - ")?
    //           .AppendLine(formatExpectation.ToString())
    //           .FinalAppend("");
    //         
    //     logger.InfoAppend("To Debug Test past the following code into ")?
    //           .Append(nameof(CompactJsonSingleTest)).Append("()\n\n")
    //           .Append("SharedCompactJsonAsValue(")
    //           .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
    //
    //     // ReSharper disable once RedundantArgumentDefaultValue
    //     var tos = new TheOneString().Initialize(Compact | Json);
    //     tos.Settings.NewLineStyle = "\n";
    //
    //     string BuildExpectedOutput(ITheOneString tos, string _, string propertyName
    //       , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    //     {
    //         var compactJsonTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
    //             ? "{{\"{0}\":{1}}}"
    //             : "{1}";
    //
    //         var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
    //         if (expectValue == IFormatExpectation.NoResultExpectedValue)
    //         {
    //             expectValue = "";
    //         }
    //         return string.Format(compactJsonTemplate, propertyName, expectValue);
    //     }
    //
    //     string BuildChildExpectedOutput(ITheOneString tos, string className, string propertyName
    //       , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    //     {
    //         const string compactJsonTemplate = "{{\"{0}\":{1}}}";
    //
    //         var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
    //         return string.Format(compactJsonTemplate, propertyName, expectValue);
    //     }
    //
    //     if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
    //     {
    //         complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
    //     }
    //     tos.Clear();
    //     var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
    //     stringBearer.RevealState(tos);
    //     var buildExpectedOutput =
    //         BuildExpectedOutput
    //             (tos, stringBearer.GetType().CachedCSharpNameNoConstraints()
    //            , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
    //            , scaffoldingToCall.ScaffoldingFlags
    //            , formatExpectation).MakeWhiteSpaceVisible();
    //     var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
    //     if (buildExpectedOutput != result)
    //     {
    //         logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
    //               .Append(result).AppendLine()
    //               .AppendLine("Expected it to match -")
    //               .AppendLine(buildExpectedOutput)
    //               .FinalAppend("");
    //     }
    //     else
    //     {
    //         logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
    //               .Append(result).AppendLine()
    //               .FinalAppend("");
    //     }
    //     Assert.AreEqual(buildExpectedOutput, result);
    // }
    //
    // private void SharedCompactJsonAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    // {
    //     logger.InfoAppend("Simple Value Type Single Value Field  Scaffolding Class - ")?
    //           .AppendLine(scaffoldingToCall.Name)
    //           .AppendLine()
    //           .AppendLine("Scaffolding Flags -")
    //           .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString().Replace(",", " |"))
    //           .FinalAppend("\n");
    //
    //     logger.WarnAppend("FormatExpectation - ")?
    //           .AppendLine(formatExpectation.ToString())
    //           .FinalAppend("");
    //         
    //     logger.InfoAppend("To Debug Test past the following code into ")?
    //           .Append(nameof(CompactJsonSingleTest)).Append("()\n\n")
    //           .Append("SharedCompactJsonAsString(")
    //           .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
    //
    //     // ReSharper disable once RedundantArgumentDefaultValue
    //     var tos = new TheOneString().Initialize(Compact | Json);
    //     tos.Settings.NewLineStyle = "\n";
    //
    //     string BuildExpectedOutput(ITheOneString tos, string className, string propertyName
    //       , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    //     {
    //         var compactJsonTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
    //             ? "{{\"{0}\":{1}}}"
    //             : "{1}";
    //
    //         var maybeSpace  = "";
    //         var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
    //         
    //         if (expectValue != IFormatExpectation.NoResultExpectedValue)
    //         {
    //             maybeSpace = expectValue.Trim().Length > 0 ? " " : "";
    //             if (maybeSpace.Length == 0)
    //             {
    //                 expectValue = "";
    //             }
    //         }
    //         else { expectValue = ""; }
    //         return string.Format(compactJsonTemplate, propertyName, expectValue);
    //     }
    //
    //     string BuildChildExpectedOutput(ITheOneString tos, string className, string propertyName
    //       , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    //     {
    //         const string compactJsonTemplate = "{{\"{0}\":{1}}}";
    //
    //         var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
    //         if (expectValue != IFormatExpectation.NoResultExpectedValue)
    //         {
    //             expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
    //         }
    //         else { expectValue = ""; }
    //         return string.Format(compactJsonTemplate, propertyName, expectValue);
    //     }
    //
    //     if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
    //     {
    //         complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
    //     }
    //     tos.Clear();
    //     var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
    //     stringBearer.RevealState(tos);
    //     var buildExpectedOutput =
    //         BuildExpectedOutput
    //             (tos, stringBearer.GetType().CachedCSharpNameNoConstraints()
    //            , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
    //            , scaffoldingToCall.ScaffoldingFlags
    //            , formatExpectation).MakeWhiteSpaceVisible();
    //     var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
    //     if (buildExpectedOutput != result)
    //     {
    //         logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
    //               .Append(result).AppendLine()
    //               .AppendLine("Expected it to match -")
    //               .AppendLine(buildExpectedOutput)
    //               .FinalAppend("");
    //     }
    //     else
    //     {
    //         logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
    //               .Append(result).AppendLine()
    //               .FinalAppend("");
    //     }
    //     Assert.AreEqual(buildExpectedOutput, result);
    // }
}
