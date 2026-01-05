// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.MapCollectionsFieldsTypes;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public partial class KeyedCollectionMoldTests
{

    [TestMethod]
    [DynamicData(nameof(SimpleUnfilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleUnfilteredCompactJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(SimplePredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimplePredicateFilteredCompactJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(SimpleSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void SimpleSubListFilteredCompactJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(ValueRevealerUnfilteredDict), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerUnfilteredCompactJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(ValueRevealerPredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerPredicateFilteredCompactJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(ValueRevealerSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void ValueRevealerSubListFilteredCompactJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(BothRevealersUnfilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersUnfilteredCompactJsonDict(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(BothRevealersPredicateFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersPredicateFilteredCompactJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(BothRevealersSubListFilteredDictExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void BothRevealersSubListFilteredCompactJsonDict
        (IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    public void CompactJsonDictionaryTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactJson(BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[976]);
    }

    private void SharedCompactJson(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Keyed Collection Type  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(nameof(SharedCompactJson)).Append("()\n\n")
              .Append("SharedCompactJson(")
              .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        
        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Json);
        
        string BuildExpectedOutput(string _, string _1
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return expectValue;
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
        Assert.AreEqual(buildExpectedOutput, result, $"Difference at i={buildExpectedOutput.DiffPosition(result)}");
    }
}


//KeyedFromDictionaryFormatStringsAddFilteredStringBearer<IPAddress,·Uri,·IPAddress,·Uri>·{············0.0.0.0:·==>·http://first-null.com/,··········127.0.0.1:·==>·tcp://localhost/,····255.255.255.255:·==>·http://unknown.com/,········192.168.1.1:·==>·tcp://default-gateway/·}