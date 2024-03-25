#region

#endregion

namespace FortitudeTests.TestEnvironment;

public class TestMachineConfig
{
    public static string LoopBackIpAddress => "169.254.224.238";
    public static string NetworkSubAddress => "224.1.0.222";
    public static ushort ServerSnapshotPort => 4000;
    public static ushort ServerUpdatePort => 4001;

    public static ushort TradingServerPort => 5000;
    public static ushort TradingTickerInfoPort => 5001;
}
