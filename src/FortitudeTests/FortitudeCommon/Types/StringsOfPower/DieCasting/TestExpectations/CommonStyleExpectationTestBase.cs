// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public abstract class CommonStyleExpectationTestBase : CommonExpectationBase
{
    protected void ExecuteIndividualScaffoldExpectation(IInputBearerFormatExpectation formatExpectation, StringStyle stringStyle
      , StringBuilderType usingStringBuilder = StringBuilderType.Alternating)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        ExecuteStringStyleTestFormatExpectation(formatExpectation, stringStyle, usingStringBuilder);
    }

    protected void ExecuteStringStyleTestFormatExpectation(IInputBearerFormatExpectation formatExpectation, StringStyle stringStyle
      , StringBuilderType usingStringBuilder = StringBuilderType.Alternating)
    {
        Logger.InfoAppend(TestsCommonDescription)?
              .Append(" In String Style - ")
              .Append(stringStyle)
              .FinalAppend("\n");

        Logger.WarnAppend("FormatExpectation - ")?
              .Append(formatExpectation.ToString())
              .FinalAppend("");

        var sb = SourceTheOnStringStringBuilder(usingStringBuilder);

        TheOneString.ReInitialize(sb, stringStyle);

        var stringBearer                = formatExpectation.TestStringBearer;
        var resultWithVisibleWhiteSpace = Recycler.Borrow<CharArrayStringBuilder>();
        stringBearer.RevealState(TheOneString);
        TheOneString.WriteBuffer.CopyAndMakeWhiteSpaceVisible(resultWithVisibleWhiteSpace);
        var buildExpectedWithVisibleWhiteSpace = Recycler.Borrow<CharArrayStringBuilder>();
        var buildExpectedOutput                = formatExpectation.GetExpectedOutputFor(Recycler, stringStyle);
        buildExpectedOutput.CopyAndMakeWhiteSpaceVisible(buildExpectedWithVisibleWhiteSpace);
        if (!TheOneString.WriteBuffer.SequenceMatches(buildExpectedOutput))
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
            ExecuteStringStyleTestFormatExpectation(formatExpectation, stringStyle, toCall);
        }
    }
}
