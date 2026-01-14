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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeCollectionFieldCompactJsonTests : SelectTypeCollectionFieldTests
{
    public override StringStyle TestStyle => Compact | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => 
        GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BoolCollectionsTestData.AllBoolCollectionExpectations[3], ScaffoldingRegistry.AllScaffoldingTypes[3]
                                           , StringBuilderType.MutableString);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory,  ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactLogTemplate = "{{{0}}}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
            nextExpect.Append("\"").Append(propertyName).Append("\":");
            nextExpect.Append(expectValue);
            expectValue.DecrementRefCount();
            expectValue = nextExpect;
        }
        else { expectValue.Clear(); }
        var fmtSb = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtSb.AppendFormat(compactLogTemplate, expectValue);
        expectValue.DecrementRefCount();
        return fmtSb;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        { expectValue.Clear(); }
        return expectValue;
    }
}
