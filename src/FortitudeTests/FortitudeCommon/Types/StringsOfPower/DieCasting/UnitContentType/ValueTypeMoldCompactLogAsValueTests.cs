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
public class ContentTypeMoldCompactLogAsValueTests : ContentTypeMoldAsValueTests
{
    public override StringStyle TestStyle => Compact | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);


    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), typeof(ContentTypeMoldAsValueTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(StringBearerTestData.AllStringBearerExpectations[34], ScaffoldingRegistry.AllScaffoldingTypes[1262]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var compactLogTemplate = 
            condition.HasComplexTypeFlag() 
         || expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{0} {{ {1}: {2} }}" : "{0} {{ {2} }}")
                :  "{0}= {2}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue == IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = "";
        }
        return string.Format(compactLogTemplate, className, propertyName, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactLogTemplate = "{0} {{ {1}}}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
        }
        else { expectValue = ""; }
        return string.Format(compactLogTemplate, className, expectValue);
    }
}
