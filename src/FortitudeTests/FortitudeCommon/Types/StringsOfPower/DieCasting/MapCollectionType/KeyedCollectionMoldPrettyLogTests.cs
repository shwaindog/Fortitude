// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

[NoMatchingProductionClass]
[TestClass]
public class KeyedCollectionMoldPrettyLogTests : KeyedCollectionMoldTests
{
    public override StringStyle TestStyle => Pretty | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);
    
    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredPrettyLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(SimpleDictTestData.AllSubListFilteredDictExpectations[13], ScaffoldingRegistry.AllScaffoldingTypes[991]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactLogTemplate = "{0} {1}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue == IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = "";
        }
        return string.Format(compactLogTemplate, className, expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue == IFormatExpectation.NoResultExpectedValue)
        { expectValue = ""; }
        return expectValue;
    }
}
