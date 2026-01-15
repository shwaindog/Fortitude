// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public abstract class CommonExpectationBase
{
    protected static IVersatileFLogger Logger       = null!;
    protected static Recycler          Recycler     = null!;
    protected static ITheOneString     TheOneString = null!;

    protected static StringBuilderType LastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;

    public static void AllDerivedShouldCallThisInClassInitialize(TestContext testContext)
    {
        if (Logger == null!)
        {
            FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

            Logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        }
        var bufferSize = 256;
        Recycler = new Recycler($"Base2SizingArrayPool({bufferSize})")
                   .RegisterFactory(() => new RecyclingCharArray(bufferSize))
                   .RegisterFactory(() => new RecyclingByteArray(bufferSize))
                   .RegisterFactory(() => new MutableString(bufferSize));
        TheOneString = new TheOneString().ReInitialize(new CharArrayStringBuilder());

        TheOneString.Settings.NewLineStyle = "\n";
    }

    protected IStringBuilder GetComparisonBuilder(IStringBuilder subjectOneStringWriteBuffer)
    {
        if (subjectOneStringWriteBuffer is CharArrayStringBuilder) { return Recycler.Borrow<MutableString>(); }
        return Recycler.Borrow<CharArrayStringBuilder>();
    }

    protected IStringBuilder SourceTheOnStringStringBuilder(StringBuilderType usingStringBuilder)
    {
        if (usingStringBuilder is StringBuilderType.Alternating)
        {
            if (LastRetrievedStringBuilderType is StringBuilderType.CharArrayStringBuilder)
            {
                LastRetrievedStringBuilderType = StringBuilderType.MutableString;
                return Recycler.Borrow<MutableString>();
            }
            LastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;
            return Recycler.Borrow<CharArrayStringBuilder>();
        }
        if (usingStringBuilder is StringBuilderType.Both or StringBuilderType.MutableString)
        {
            LastRetrievedStringBuilderType = StringBuilderType.MutableString;
            return Recycler.Borrow<MutableString>();
        }
        LastRetrievedStringBuilderType = StringBuilderType.CharArrayStringBuilder;
        return Recycler.Borrow<CharArrayStringBuilder>();
    }

    public abstract string TestsCommonDescription { get; }
}
