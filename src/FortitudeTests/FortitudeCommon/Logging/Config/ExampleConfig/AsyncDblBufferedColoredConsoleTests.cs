// // Licensed under the MIT license.
// // Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.TestHelpers;

namespace FortitudeTests.FortitudeCommon.Logging.Config.ExampleConfig;

[TestClass]
[NoMatchingProductionClass]
public class AsyncDblBufferedColoredConsoleTests
{
    private const string DblBufferedColoredConsoleName = "ColoredConsole";

    [TestMethod]
    [Timeout(20_000)]
    public void AsyncDailyDblBufferedFileLoadsAndLogsToFile()
    {
        using var wd = GetType().GetTemporaryWorkingDirectoryFor();
        FLogConfigExamples.AsyncDblBufferedColoredConsoleExample.ExtractExampleTo();
        var context =
            FLogContext
                .NewUninitializedContext
                .InitializeContextFromWorkingDirFilePath(Environment.CurrentDirectory, FLogConfigFile.DefaultConfigFullFilePath)
                .StartFlogSetAsCurrentContext();

        var manualResetEvent = new ManualResetEvent(false);
        context.AppenderRegistry.WhenAppenderProcessedCountRun(DblBufferedColoredConsoleName, 100, (_, _) => manualResetEvent.Set());
        var testLogger = FLog.FLoggerForType.As<IVersatileFLogger>();

        for (var i = 0; i < 20; i++)
        {
            testLogger.TrcApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.DbgApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.InfApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.WrnApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.ErrApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        }
        manualResetEvent.WaitOne();
    }
}
