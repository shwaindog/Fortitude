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
public class SyncFileAndColoredConsoleTests
{
    [TestMethod]
    public void SyncFileAndColoredConsoleLoadsAndLogsToBothAppenders()
    {
        using var wd = GetType().GetTemporaryWorkingDirectoryFor();
        FLogConfigExamples.SyncFileAndColoredConsoleExample.ExtractExampleTo();
        var context =
            FLogContext
                .NewUninitializedContext
                .InitializeContextFromWorkingDirFilePath(Environment.CurrentDirectory, FLogConfigFile.DefaultConfigFullFilePath)
                .StartFlogSetAsCurrentContext();

        var testLogger = FLog.FLoggerForType.As<IVersatileFLogger>();
        
        testLogger.TrcApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        testLogger.DbgApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        testLogger.InfApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        testLogger.WrnApnd("Testing")?.Args(" 1,", " 2,", " 3.");
        testLogger.ErrApnd("Testing")?.Args(" 1,", " 2,", " 3.");
    }
}