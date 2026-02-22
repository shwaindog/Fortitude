// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

[NoMatchingProductionClass]
[TestClass]
public class OrderedCollectionMoldPrettyJsonTests : OrderedCollectionMoldTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => 
        GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BoolCollectionsTestData.AllBoolCollectionExpectations[0], ScaffoldingRegistry.AllScaffoldingTypes[1003], StringBuilderType.CharArrayStringBuilder);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            expectValue.Clear();
        }
        return expectValue;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
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
