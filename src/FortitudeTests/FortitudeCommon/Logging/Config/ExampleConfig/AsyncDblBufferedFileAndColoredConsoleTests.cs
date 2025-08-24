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
    private const string DblBufferedForwardingAppenderName = "ForwardingAppender";
    
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
        
        var manualResetEvent = new ManualResetEvent(false);
        context.AppenderRegistry.WhenAppenderProcessedCountRun(DblBufferedForwardingAppenderName, 100, (_, _) => manualResetEvent.Set());
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
