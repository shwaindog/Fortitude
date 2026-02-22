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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeCollectionFieldPrettyJsonTests : SelectTypeCollectionFieldTests
{
    public override StringStyle TestStyle => Pretty | Json;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => 
        GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);
    
    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BoolCollectionsTestData.AllBoolCollectionExpectations[3], ScaffoldingRegistry.AllScaffoldingTypes[5]
                                           , StringBuilderType.MutableString);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactJsonTemplate = "{{{0}{1}{2}{0}}}";
            
        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();

            nextExpect.Append("\"" ).Append(propertyName).Append("\": ").Append(expectValue.IndentSubsequentLines(tos.Settings.NewLineStyle));
            expectValue.DecrementRefCount();
            expectValue = nextExpect;
        }

        else { expectValue.Clear(); }

        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactJsonTemplate, maybeNewLine, maybeIndent, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
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