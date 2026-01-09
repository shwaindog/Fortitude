// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

[NoMatchingProductionClass]
[TestClass]
public class KeyedCollectionMoldPrettyJsonTests : KeyedCollectionMoldTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);
    
    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredPrettyJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), typeof(KeyedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredPrettyJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(ValueRevealerDictTestData.AllValueRevealerSubListFilteredDictExpectations[4], ScaffoldingRegistry.AllScaffoldingTypes[995]);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            expectValue.Clear();
        }
        return expectValue;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            expectValue.Clear();
        }
        return expectValue;
    }
}
