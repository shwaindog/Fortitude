// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeKeyedCollectionFieldPrettyJsonTests : SelectTypeKeyedCollectionFieldTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);


    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[634]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactJsonTemplate = "{{{0}{1}{2}{0}}}";
            
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

        return string.Format(compactJsonTemplate, maybeNewLine, maybeIndent, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue == IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = "";
        }
        return expectValue;
    }
}