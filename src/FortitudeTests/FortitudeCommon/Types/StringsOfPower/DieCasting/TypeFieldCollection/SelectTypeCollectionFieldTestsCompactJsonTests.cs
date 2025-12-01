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
    public void UnfilteredCompactJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactJsonStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    public void CompactJsonListTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactJson(BoolCollectionsTestData.AllBoolCollectionExpectations[0], ScaffoldingRegistry.AllScaffoldingTypes[332]);
    }

    private void SharedCompactJson(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
        
        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Json);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactLogTemplate = "{{{0}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "\"" + propertyName + "\":" + expectValue;
            }
            else { expectValue = ""; }
            return string.Format(compactLogTemplate, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            { expectValue = ""; }
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
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(nameof(CompactJsonListTest)).Append("()\n\n")
              .Append("SharedCompactJson(")
              .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        Assert.AreEqual(buildExpectedOutput, result);
    }
}
