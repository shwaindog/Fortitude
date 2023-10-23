using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.OSWrapper.NetworkingWrappers;

namespace FortitudeTests.TestEnvironment
{
    public class TestMachineConfig
    {
        public static string LoopBackIpAddress => "169.254.80.243";
        public static string NetworkSubAddress => "239.0.0.222";
        public static int ServerSnapshotPort => 4000;
        public static int ServerUpdatePort => 4001;

        public static int TradingServerPort => 5000;
        public static int TradingTickerInfoPort => 5001;
    }
}
