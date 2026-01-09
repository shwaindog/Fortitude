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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

[NoMatchingProductionClass]
[TestClass]
public class SelectTypeKeyedCollectionFieldPrettyLogTests : SelectTypeKeyedCollectionFieldTests
{
    public override StringStyle TestStyle => Pretty | Log;
    
    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) => 
        AllDerivedShouldCallThisInClassInitialize(testContext);
    
    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredPrettyLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredPrettyLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredPrettyLogDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredPrettyLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), typeof(SelectTypeKeyedCollectionFieldTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredPrettyLogDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) => 
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod] 
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[622]);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        const string compactLogTemplate = "{0} {{{1}{2}{3}{1}}}";
        
        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            if (!expectValue.SequenceMatches("null")
             && expectation is IOrderedListExpect orderedListExpectation
             && orderedListExpectation.ElementCallType.IsEnumOrNullable())
            {
                var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
                nextExpect.Append(propertyName).Append(": (");
                orderedListExpectation.CollectionCallType.AppendShortNameInCSharpFormat(nextExpect).Append(")")
                                      .Append(expectValue.IndentSubsequentLines());
                expectValue.DecrementRefCount();
                expectValue = nextExpect;
            }
            else
            {
                var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
                nextExpect.Append(propertyName).Append(": ").Append(expectValue.IndentSubsequentLines());
                expectValue.DecrementRefCount();
                expectValue = nextExpect;
            }
        }

        else { expectValue.Clear(); }

        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactLogTemplate, className, maybeNewLine, maybeIndent, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
    
    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) 
    {
        var prettyLogTemplate = className.IsNotEmpty() ? "({0}){1}" : "{1}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue)) { expectValue.Clear(); }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyLogTemplate, className, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}