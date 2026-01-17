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
public class SelectTypeCollectionFieldPrettyLogTests : SelectTypeCollectionFieldTests
{
    public override StringStyle TestStyle => Pretty | Log;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) =>
        GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), typeof(SelectTypeCollectionFieldTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations[9]
                                           , ScaffoldingRegistry.AllScaffoldingTypes[75], StringBuilderType.CharArrayStringBuilder);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var compactLogTemplate =
            IsLogIgnoredTypeName(tos.Settings, className)
                ? "{{{1}{2}{3}{1}}}"
                : "{0} {{{1}{2}{3}{1}}}";


        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            if (!expectValue.SequenceMatches("null")
             && expectation is IOrderedListExpect orderedListExpectation
             && (orderedListExpectation.ElementCallType.IsEnumOrNullable()
                // || !(IsLogIgnoredTypeName(tos.Settings, orderedListExpectation.CollectionCallType)
                //     && IsLogIgnoredTypeName(tos.Settings, orderedListExpectation.ElementCallType))
                ))
            {
                var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
                nextExpect.Append(propertyName).Append(": (");
                orderedListExpectation.CollectionCallType.AppendShortNameInCSharpFormat(nextExpect).Append(") ");
                nextExpect.Append(expectValue.IndentSubsequentLines());
                expectValue.DecrementRefCount();
                expectValue = nextExpect;
            }
            else
            {
                var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
                nextExpect.Append(propertyName).Append(": ")
                          .Append(expectValue.IndentSubsequentLines());
                expectValue.DecrementRefCount();
                expectValue = nextExpect;
            }
        }

        else { expectValue.Clear(); }

        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactLogTemplate, className?.CachedCSharpNameNoConstraints() ?? "", maybeNewLine, maybeIndent, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }

    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var maybeElementType = className?.GetIterableElementType();
        var compactLogTemplate =
            className != null
            && (!IsLogIgnoredTypeName(tos.Settings, className) 
         || (maybeElementType != null 
         || !IsLogIgnoredTypeName(tos.Settings, maybeElementType!.IfNullableGetUnderlyingTypeOrThis())))
                ? "({0}) {1}"
                : "{1}";

        var expectValue = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue)) { expectValue.Clear(); }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(compactLogTemplate, className?.ShortNameInCSharpFormat() ?? "", expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}
