// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeFieldPrettyJsonTests : SelectTypeFieldTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullFmt(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullFmtStruct(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharArrayExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharArray(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharSequence(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringBuilderExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringBuilder(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullStringBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpect), typeof(SelectTypeFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullStringBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(StringBearerTestData.AllStringBearerExpectations[24], ScaffoldingRegistry.AllScaffoldingTypes[901]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string prettyJsonTemplate = "{{{0}{1}{2}{0}}}";

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            expectValue  = "\"" + propertyName + "\": " + expectValue;
        }

        else { expectValue = ""; }

        return string.Format(prettyJsonTemplate, maybeNewLine, maybeIndent, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string prettyJsonTemplate = "{{{0}{1}{1}{2}{0}{1}}}";

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            expectValue  = "\"" + propertyName + "\": " + expectValue.IndentSubsequentLines();
        }

        else { expectValue = ""; }

        return string.Format(prettyJsonTemplate, maybeNewLine, maybeIndent, expectValue);
    }
}
