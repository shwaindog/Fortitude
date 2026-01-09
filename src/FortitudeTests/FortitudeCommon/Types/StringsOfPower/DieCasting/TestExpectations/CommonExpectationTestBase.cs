// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
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

public abstract class CommonExpectationTestBase
{
    protected static IReadOnlyList<ScaffoldingPartEntry> ScafReg = ScaffoldingRegistry.AllScaffoldingTypes;

    protected static IVersatileFLogger Logger       = null!;
    private static   Recycler          recycler     = null!;
    private static   ITheOneString     theOneString = null!;

    private static StringBuilderType lastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;

    public static void AllDerivedShouldCallThisInClassInitialize(TestContext testContext)
    {
        if (Logger == null!)
        {
            FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

            Logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        }
        var bufferSize = 256;
        recycler = new Recycler($"Base2SizingArrayPool({bufferSize})")
                   .RegisterFactory(() => new RecyclingCharArray(bufferSize))
                   .RegisterFactory(() => new RecyclingByteArray(bufferSize))
                   .RegisterFactory(() => new MutableString(bufferSize));
        theOneString = new TheOneString().ReInitialize(new CharArrayStringBuilder());

        theOneString.Settings.NewLineStyle = "\n";
    }


    public static string GenerateScaffoldExpectationTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }

    private IStringBuilder GetComparisonBuilder(IStringBuilder subjectOneStringWriteBuffer)
    {
        if (subjectOneStringWriteBuffer is CharArrayStringBuilder) { return recycler.Borrow<MutableString>(); }
        return recycler.Borrow<CharArrayStringBuilder>();
    }

    private IStringBuilder SourceTheOnStringStringBuilder(StringBuilderType usingStringBuilder)
    {
        if (usingStringBuilder is StringBuilderType.Alternating)
        {
            if (lastRetrievedStringBuilderType is StringBuilderType.CharArrayStringBuilder)
            {
                lastRetrievedStringBuilderType = StringBuilderType.MutableString;
                return recycler.Borrow<MutableString>();
            }
            lastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;
            return recycler.Borrow<CharArrayStringBuilder>();
        }
        if (usingStringBuilder is StringBuilderType.Both or StringBuilderType.MutableString)
        {
            lastRetrievedStringBuilderType = StringBuilderType.MutableString;
            return recycler.Borrow<MutableString>();
        }
        lastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;
        return recycler.Borrow<CharArrayStringBuilder>();
    }

    public abstract string TestsCommonDescription { get; }

    public abstract StringStyle TestStyle { get; }

    protected abstract IStringBuilder BuildExpectedRootOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
      , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation);

    protected virtual IStringBuilder BuildExpectedChildOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
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

        theOneString.ReInitialize(sb, TestStyle);

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
              .Append(lastRetrievedStringBuilderType)
              .FinalAppend(");");

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildExpectedChildOutput;
        }
        var stringBearer                = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, theOneString.Settings);
        var resultWithVisibleWhiteSpace = recycler.Borrow<CharArrayStringBuilder>();
        stringBearer.RevealState(theOneString);
        theOneString.WriteBuffer.CopyAndMakeWhiteSpaceVisible(resultWithVisibleWhiteSpace);
        var buildExpectedWithVisibleWhiteSpace = recycler.Borrow<CharArrayStringBuilder>();
        var buildExpectedOutput =
            BuildExpectedRootOutput
                (recycler, theOneString, stringBearer.GetType().CachedCSharpNameNoConstraints()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation);
        buildExpectedOutput.CopyAndMakeWhiteSpaceVisible(buildExpectedWithVisibleWhiteSpace);
        if (!theOneString.WriteBuffer.SequenceMatches(buildExpectedOutput))
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
            var toCall = lastRetrievedStringBuilderType == StringBuilderType.MutableString
                ? StringBuilderType.CharArrayStringBuilder
                : StringBuilderType.MutableString;
            ExecuteTestScaffoldingWithExpectation(formatExpectation, scaffoldingToCall, toCall);
        }
    }
}
