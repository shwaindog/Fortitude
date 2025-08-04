using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeTests.FortitudeCommon.Logging.Core.Hub;

[TestClass]
public class FLogContextTests
{
    [TestMethod]
    public void DefaultContextStartsAndLogsToConsole()
    {
        var context = FLogContext.NullOnUnstartedContext.SetAsNewNewDefaultConfigContext();
        var startedContext = FLogContext.Context;

        var logger = FLog.FLoggerForType;

        logger.Info()?.StringAppender().FinalAppend("Testing 1 2 3, testing");

        Thread.Sleep(5_000);
    }
}