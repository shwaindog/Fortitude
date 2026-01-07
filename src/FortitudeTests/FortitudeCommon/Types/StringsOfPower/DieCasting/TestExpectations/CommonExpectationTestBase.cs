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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public abstract class CommonExpectationTestBase
{
    protected static IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;
    
    protected static IVersatileFLogger logger = null!;

    public static void AllDerivedShouldCallThisInClassInitialize(TestContext testContext)
    {
        if (logger == null!)
        {
            FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

            logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        }
    }
    

    public static string GenerateScaffoldExpectationTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }
    
    public abstract string TestsCommonDescription { get; }
    
    public abstract StringStyle TestStyle { get; }

    protected abstract string BuildExpectedRootOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation);

    protected virtual string BuildExpectedChildOutput(ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) =>
        BuildExpectedRootOutput(tos, className, propertyName, condition, expectation);


    public abstract void RunExecuteIndividualScaffoldExpectation();
    
    protected void ExecuteIndividualScaffoldExpectation(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        ExecuteTestScaffoldingWithExpectation(formatExpectation, scaffoldingToCall);
    }

    protected void ExecuteTestScaffoldingWithExpectation(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend(TestsCommonDescription)?
              .Append(" In String Style - ")
              .Append(TestStyle)
              .AppendLine("  using Scaffolding Class - ")
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(GetType().Name).Append(".").Append(nameof(RunExecuteIndividualScaffoldExpectation)).AppendLine("\n")
              .Append(nameof(ExecuteIndividualScaffoldExpectation))
              .Append("(")
              .Append(formatExpectation.ItemCodePath)
              .Append(", ")
              .Append(scaffoldingToCall.ItemCodePath)
              .FinalAppend(");");
        
        var tos = new TheOneString().Initialize(TestStyle);
        tos.Settings.NewLineStyle = "\n";

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildExpectedChildOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
        stringBearer.RevealState(tos);
        var buildExpectedOutput =
            BuildExpectedRootOutput
                (tos, stringBearer.GetType().CachedCSharpNameNoConstraints()
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
        Assert.AreEqual(buildExpectedOutput, result);
    }
}
