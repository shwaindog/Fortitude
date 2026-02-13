// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

[NoMatchingProductionClass]
[TestClass]
public class ContentTypeMoldPrettyLogAsStringTests : ContentTypeMoldAsStringTests
{
    public override StringStyle TestStyle => Pretty | Log;

    [ClassInitialize]
    public static void EnsureBaseClassInitialized(TestContext testContext) =>
        AllDerivedShouldCallThisInClassInitialize(testContext);

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data) => GenerateScaffoldExpectationTestName(methodInfo, data);

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), typeof(ContentTypeMoldAsStringTests), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullCloakedBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsString), typeof(ContentTypeMoldAsStringTests)
                  , DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullStringBearerAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall) =>
        ExecuteIndividualScaffoldExpectation(formatExpectation, scaffoldingToCall);

    // [TestMethod]
    public override void RunExecuteIndividualScaffoldExpectation()
    {
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        ExecuteIndividualScaffoldExpectation(EnumTestData.EnumExpectations[33], ScaffoldingRegistry.AllScaffoldingTypes[1310]
                                           , StringBuilderType.CharArrayStringBuilder);
    }

    protected override IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var prettyLogTemplate = 
            condition.HasComplexTypeFlag() && className.IsStringBearerOrNullableCached() 
                ? "({0}) {{{1}{2}{3}{1}}}"
                : "({0}) {3}" ;

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
            if (propertyName.IsNotEmpty() && condition.HasComplexTypeFlag()) nextExpect.Append(propertyName).Append(": ");
            if (condition.HasComplexTypeFlag() && expectValue.HasAnyPairedBrc()) expectValue.IndentSubsequentLines(tos.Settings.NewLineStyle);
            nextExpect.Append(expectValue);
            expectValue.DecrementRefCount();
            expectValue = nextExpect;
        }
        else { expectValue.Clear(); }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyLogTemplate, className?.CachedCSharpNameNoConstraints() ?? "", maybeNewLine, maybeIndent, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }

    protected override IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
    {
        var prettyLogTemplate =
            IsLogIgnoredTypeName(tos.Settings, className)
                ? "\"{{{1}{2}{3}{1}}}\""
                : "\"{0} {{{1}{2}{3}{1}}}\"";

        var maybeNewLine = "";
        var maybeIndent  = "";
        var expectValue  = expectation.GetExpectedOutputFor(sbFactory, condition, tos, expectation.ValueFormatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue))
        {
            maybeNewLine = "\n";
            maybeIndent  = "  ";
            var nextExpect = sbFactory.Borrow<CharArrayStringBuilder>();
            nextExpect.Append(propertyName).Append(": ");
            if (condition.HasComplexTypeFlag() && expectValue.HasAnyPairedBrc()) expectValue.IndentSubsequentLines(tos.Settings.NewLineStyle);
            nextExpect.Append(expectValue);
            expectValue.DecrementRefCount();
            expectValue = nextExpect;
        }
        else { expectValue.Clear(); }
        var fmtExpect = sbFactory.Borrow<CharArrayStringBuilder>();
        fmtExpect.AppendFormat(prettyLogTemplate, className?.CachedCSharpNameNoConstraints() ?? "", maybeNewLine, maybeIndent, expectValue);
        expectValue.DecrementRefCount();
        return fmtExpect;
    }
}
