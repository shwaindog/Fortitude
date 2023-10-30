namespace FortitudeTests.TestEnvironment;

public class TestMachineConfig
{
    public static string LoopBackIpAddress => "169.254.224.238";
    public static string NetworkSubAddress => "224.1.0.222";
    public static int ServerSnapshotPort => 4000;
    public static int ServerUpdatePort => 4001;

    public static int TradingServerPort => 5000;
    public static int TradingTickerInfoPort => 5001;
}
