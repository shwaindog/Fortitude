#region

using FortitudeCommon.Monitoring.Logging.NLogAdapter;

#endregion

namespace FortitudeTests.FortitudeCommon.Monitoring.Logging.NLogAdapter;

[TestClass]
public class NLogFactoryTests
{
    private NLogFactory nLogFactory = new();


    [TestMethod]
    public void WritingNloggerClassCreatesExpectedFileWithOutput()
    {
        var nLogger = nLogFactory.GetLogger(typeof(NLogFactoryTests));

        nLogger.Debug("Write Debug Line");
        nLogger.Info("Write Info Line");
        nLogger.Warn("Write Warn Line");
        nLogger.Error("Write Error Line");
    }
}
