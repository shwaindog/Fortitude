// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;


[NoMatchingProductionClass]
[TestClass]
public class ContentTypeMoldPrettyJsonAsValueTests : ContentTypeMoldAsValueTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)=> 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    // [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(StringBearerTestData.AllStringBearerExpectations[26], ScaffoldingRegistry.AllScaffoldingTypes[1367]);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        { expectValue.Clear(); }
        return expectValue;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var prettyJsonTemplate = "{{{0}{1}\"{2}\": {3}{0}}}";
        
        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            if(condition.HasComplexTypeFlag() && expectValue.IsBrcBounded()) expectValue.IndentSubsequentLines(tos.Settings.NewLineStyle);
        }

        else { expectValue.Clear(); }

        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyJsonTemplate, maybeNewLine, maybeIndent, propertyName, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}
