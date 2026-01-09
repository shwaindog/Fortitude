// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
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
public class OrderedCollectionMoldPrettyLogTests : OrderedCollectionMoldTests
{
    public override StringStyle TestStyle => Pretty | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => 
        GenerateScaffoldExpectationTestName(methodInfo, data);


    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), typeof(OrderedCollectionMoldTests) , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), typeof(OrderedCollectionMoldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations[1], ScaffoldingRegistry.AllScaffoldingTypes[1049]);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string prettyLogTemplate = "({0}){1}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            expectValue.Clear();
        }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyLogTemplate, className, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
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
