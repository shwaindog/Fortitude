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
public class ContentTypeMoldPrettyLogAsStringTests : ContentTypeMoldAsStringTests
{
    public override StringStyle TestStyle => Pretty | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(StringBearerTestData.AllStringBearerExpectations[34], ScaffoldingRegistry.AllScaffoldingTypes[1217]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var prettyLogTemplate = 
            condition.HasComplexTypeFlag() 
         || expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{0} {{{1}{2}{3}{1}}}" : "{0} {{ {2} }}")
                :  "{0}= {3}";

        var maybeNewLine  = "";
        var maybeIndent   = "";
        var maybeProperty = propertyName.IsNotEmpty() && condition.HasComplexTypeFlag() ? $"{propertyName}: " : "";
        var expectValue   = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            expectValue  = 
                maybeProperty 
              + (condition.HasComplexTypeFlag() && expectValue.HasAnyPairedBrc() 
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
        const string prettyLogTemplate = "\"{0} {{{1}{2}{3}{1}}}\"";

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
