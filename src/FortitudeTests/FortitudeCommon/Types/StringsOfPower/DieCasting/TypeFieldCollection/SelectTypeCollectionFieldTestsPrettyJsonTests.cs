// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionFieldTests
{

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredPrettyJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    public void PrettyJsonListTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactJson(BoolCollectionsTestData.AllBoolCollectionExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[476]);
    }

    private void SharedPrettyJson(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Ordered Collection Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(nameof(PrettyJsonListTest)).Append("()\n\n")
              .Append("SharedPrettyJson(")
              .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        
        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Pretty | Json);
        tos.Settings.NewLineStyle = "\n";

        string BuildExpectedOutput(string _, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactJsonTemplate = "{{{0}{1}{2}{0}}}";


            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
                expectValue  = "\"" + propertyName + "\": " + expectValue.IndentSubsequentLines();
            }

            else { expectValue = ""; }

            return string.Format(compactJsonTemplate, maybeNewLine, maybeIndent, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return expectValue;
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
        stringBearer.RevealState(tos);
        var buildExpectedOutput =
            BuildExpectedOutput
                (stringBearer.GetType().CachedCSharpNameNoConstraints()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation).MakeWhiteSpaceVisible();
        var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
        if (buildExpectedOutput != result)
        {
            logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .AppendLine("Expected it to match -")
                  .AppendLine(buildExpectedOutput)
                  .FinalAppend("");
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
        Assert.AreEqual(buildExpectedOutput, result, $"Difference at i={buildExpectedOutput.DiffPosition(result)}");
    }
}