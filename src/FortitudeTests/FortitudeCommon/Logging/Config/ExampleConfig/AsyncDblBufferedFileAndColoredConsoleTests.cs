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
public class AsyncDblBufferedFileAndColoredConsoleTests
{
    private const string AppLogFileAppenderAppenderName = "AppLogFileAppender";
    private const string ColoredConsoleAppenderName     = "ColoredConsole";

    [TestMethod]
    public void AsyncDailyDblBufferedFileLoadsAndLogsToFile()
    {
        using var wd = GetType().GetTemporaryWorkingDirectoryFor();
        FLogConfigExamples.AsyncDblBufferedFileAndColoredConsoleExample.ExtractExampleTo();
        var context =
            FLogContext
                .NewUninitializedContext
                .InitializeContextFromWorkingDirFilePath(Environment.CurrentDirectory, FLogConfigFile.DefaultConfigFullFilePath)
                .StartFlogSetAsCurrentContext();

        var fileManualResetEvent    = new ManualResetEvent(false);
        var consoleManualResetEvent = new ManualResetEvent(false);
        context.AppenderRegistry.WhenAppenderProcessedCountRun(AppLogFileAppenderAppenderName, 100, (_, _) => fileManualResetEvent.Set());
        context.AppenderRegistry.WhenAppenderProcessedCountRun(ColoredConsoleAppenderName, 100, (_, _) => consoleManualResetEvent.Set());
        var testLogger = FLog.FLoggerForType.As<IVersatileFLogger>();

        for (var i = 0; i < 20; i++)
        {
            testLogger.TrcApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.DbgApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.InfApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.WrnApnd("Testing")?.Args(" 1,", " 2,", " 3.");
            testLogger.ErrApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        }
        fileManualResetEvent.WaitOne();
        consoleManualResetEvent.WaitOne();
    }
}
