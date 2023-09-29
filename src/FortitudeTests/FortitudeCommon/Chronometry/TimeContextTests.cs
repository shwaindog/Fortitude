using System;
using FortitudeCommon.Chronometry;

namespace FortitudeTests.FortitudeCommon.Chronometry
{
    public class TimeContextTests
    {
        public class StubTimeContext : ITimeContext
        {
            public DateTime UtcNow { get; set; }
            public DateTime LocalTimeNow { get; set; }
        }
    }
}