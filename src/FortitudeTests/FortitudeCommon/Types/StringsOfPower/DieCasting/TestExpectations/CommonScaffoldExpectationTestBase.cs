// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public enum StringBuilderType
{
    Both
  , Alternating
  , MutableString
  , CharArrayStringBuilder
}

public abstract class CommonScaffoldExpectationTestBase : CommonExpectationBase
{
    protected static IReadOnlyList<ScaffoldingPartEntry> ScafReg = ScaffoldingRegistry.AllScaffoldingTypes;
    
    public static string GenerateScaffoldExpectationTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }

    public abstract StringStyle TestStyle { get; }

    protected abstract IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation);

    protected virtual IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, Type? className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation) =>
        BuildExpectedRootOutput(sbFactory, tos, className, propertyName, condition, expectation);


    public abstract void RunExecuteIndividualScaffoldExpectation();

    protected void ExecuteIndividualScaffoldExpectation(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall
      , StringBuilderType usingStringBuilder = StringBuilderType.Alternating)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        ExecuteTestScaffoldingWithExpectation(formatExpectation, scaffoldingToCall, usingStringBuilder);
    }

    protected void ExecuteTestScaffoldingWithExpectation(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall
      , StringBuilderType usingStringBuilder = StringBuilderType.Alternating)
    {
        Logger.InfoAppend(TestsCommonDescription)?
              .Append(" In String Style - ")
              .Append(TestStyle)
              .AppendLine("  using Scaffolding Class - ")
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        Logger.WarnAppend("FormatExpectation - ")?
              .Append(formatExpectation.ToString())
              .FinalAppend("");

        var sb = SourceTheOnStringStringBuilder(usingStringBuilder);

        ResetOneStringWithSettings(MyTheOneString);
        MyTheOneString.ReInitialize(sb, TestStyle);

        Logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(GetType().Name).Append(".").Append(nameof(RunExecuteIndividualScaffoldExpectation)).AppendLine("\n")
              .Append(nameof(ExecuteIndividualScaffoldExpectation))
              .Append("(")
              .Append(formatExpectation.ItemCodePath)
              .Append(", ")
              .Append(scaffoldingToCall.ItemCodePath)
              .Append(", ")
              .Append(nameof(StringBuilderType))
              .Append(".")
              .Append(LastRetrievedStringBuilderType)
              .FinalAppend(");");

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildExpectedChildOutput;
        }
        var stringBearer                = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, MyTheOneString.Settings);
        var resultWithVisibleWhiteSpace = Recycler.Borrow<CharArrayStringBuilder>();
        stringBearer.RevealState(MyTheOneString);
        MyTheOneString.WriteBuffer.CopyAndMakeWhiteSpaceVisible(resultWithVisibleWhiteSpace);
        var buildExpectedWithVisibleWhiteSpace = Recycler.Borrow<CharArrayStringBuilder>();
        var buildExpectedOutput =
            BuildExpectedRootOutput
                (Recycler, MyTheOneString, stringBearer.GetType()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation);
        buildExpectedOutput.CopyAndMakeWhiteSpaceVisible(buildExpectedWithVisibleWhiteSpace);
        if (!MyTheOneString.WriteBuffer.SequenceMatches(buildExpectedOutput))
        {
            Logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
                  .Append(resultWithVisibleWhiteSpace).AppendLine()
                  .AppendLine("Expected it to match -")
                  .AppendLine(buildExpectedWithVisibleWhiteSpace)
                  .FinalAppend("");
        }
        else
        {
            Logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(resultWithVisibleWhiteSpace).AppendLine()
                  .FinalAppend("");
        }
        Assert.IsTrue(sb.SequenceMatches(buildExpectedOutput)
                    , "Expected does not equal result.  First difference found at " + sb.DiffPosition(buildExpectedOutput) );
        resultWithVisibleWhiteSpace.DecrementRefCount();
        buildExpectedOutput.DecrementRefCount();
        buildExpectedWithVisibleWhiteSpace.DecrementRefCount();
        if (usingStringBuilder == StringBuilderType.Both)
        {
            var toCall = LastRetrievedStringBuilderType == StringBuilderType.MutableString
                ? StringBuilderType.CharArrayStringBuilder
                : StringBuilderType.MutableString;
            ExecuteTestScaffoldingWithExpectation(formatExpectation, scaffoldingToCall, toCall);
        }
    }
}
