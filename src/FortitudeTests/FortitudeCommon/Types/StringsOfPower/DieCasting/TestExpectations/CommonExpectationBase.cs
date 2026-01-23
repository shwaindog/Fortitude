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
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public abstract class CommonExpectationBase
{
    protected static IVersatileFLogger Logger       = null!;
    protected static Recycler          Recycler     = null!;
    protected static ITheOneString     MyTheOneString = null!;

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
        MyTheOneString = new TheOneString().ReInitialize(new CharArrayStringBuilder());

        MyTheOneString.Settings.NewLineStyle = "\n";
        TheOneString.DefaultSettings.NewLineStyle = "\n";
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

    public bool IsLogIgnoredTypeName(StyleOptions styleOptions, Type? checkIsAutoIgnore)
    {
        return styleOptions.LogSuppressDisplayTypeNames.Any(s => checkIsAutoIgnore != null && checkIsAutoIgnore.FullName!.StartsWith(s));
    }
}
