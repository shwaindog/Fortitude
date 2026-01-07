// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeKeyedCollectionFieldCompactLogTests : SelectTypeKeyedCollectionFieldTests
{
    public override StringStyle TestStyle => Compact | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);


    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredCompactLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredCompactLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[636]);
    }

    protected override string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactLogTemplate = "{0} {{{1}{2}{1}}}";

        var maybePadding = "";
        var expectValue  = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue)
        {
            maybePadding = expectValue.Length > 0 ? " " : "";
            if (expectValue != "null" &&  expectation is IOrderedListExpect orderedListExpectation 
                                       && orderedListExpectation.ElementCallType.IsEnumOrNullable())
            {
                expectValue = propertyName + ": (" + orderedListExpectation.CollectionCallType.ShortNameInCSharpFormat() + ")" + 
                              expectValue;    
            }
            else
            {
                expectValue = propertyName + ": " + expectValue;
            }
        }
        else { expectValue = ""; }
        return string.Format(compactLogTemplate, className, maybePadding,  expectValue);
    }
    
    protected override string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var compactLogTemplate = className.IsNotEmpty() ? "({0}){1}" : "{1}";

        var expectValue = expectation.GetExpectedOutputFor(condition, tos, expectation.ValueFormatString);
        if (expectValue == IFormatExpectation.NoResultExpectedValue)
        {
            expectValue = "";
        }
        return string.Format(compactLogTemplate, className, expectValue);
    }
}