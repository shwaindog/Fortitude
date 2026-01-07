// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;


[NoMatchingProductionClass]
[TestClass]
public class ContentTypeMoldPrettyLogAsValueTests : ContentTypeMoldAsValueTests
{
    public override StringStyle TestStyle => Pretty | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);


    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(StringBuilderTestData.AllStringBuilderExpectations[5], ScaffoldingRegistry.AllScaffoldingTypes[1380]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var prettyLogTemplate = 
            condition.HasComplexTypeFlag() 
         || expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{0} {{{1}{2}{3}{1}}}" : "{0} {{ {2} }}")
                :  "{0}= {3}";

        var maybeNewLine = "";
        var maybeIndent  = "";

        var maybeProperty = propertyName.IsNotEmpty() && condition.HasComplexTypeFlag() ? $"{propertyName}: " : "";
        var expectValue   = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            expectValue  = 
                maybeProperty + 
                (condition.HasComplexTypeFlag() && expectValue.HasAnyPairedBrc() 
                    ? expectValue.IndentSubsequentLines() 
                    : expectValue);
        }
        else
        {
            expectValue = "";
        }

        return string.Format(prettyLogTemplate, className, maybeNewLine, maybeIndent, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string prettyLogTemplate = "{0} {{{1}{2}{3}{1}}}";

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            expectValue  = propertyName + ": " + (condition.HasComplexTypeFlag() && expectValue.HasAnyPairedBrc() 
                ? expectValue.IndentSubsequentLines() 
                : expectValue);
        }

        else { expectValue = ""; }

        return string.Format(prettyLogTemplate, className, maybeNewLine, maybeIndent, expectValue);
    }
}
