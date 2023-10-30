using System;
using System.Threading;
using FortitudeCommon.Chronometry;

namespace FortitudeCommon.Types
{
    public static class IdGen
    {
        private static long uid;
        public static string Next(string rad)
        {
            return rad + DateTime.Now.ToString("yyyyMMddHHmmss") + "X" + Interlocked.Increment(ref uid);
        }
    }

    public static class IdGenLong
    {
        private static long previousId;
        public static long Next()
        {
            long testGen = TimeContext.UtcNow.Ticks;
            if (testGen == previousId)
            {
                testGen++;
            }
            previousId = testGen;
            return previousId;
        }
    }

    public class IdGenMod
    {
        public IdGenMod(string rad, long length)
        {
            this.rad = rad;
            length = length - rad.Length - 15;
            if (length <= 0)
            {
                throw new Exception("Invalid ID length");
            }
            mod = 1;
            while (length-- > 0)
            {
                mod *= 10;
            }
        }

        private readonly string rad;
        private readonly long mod;
        private long uid;
        public string Next => rad + DateTime.Now.ToString("yyyyMMddHHmmss") + "X" + (uid = ++uid % mod);
    }
}
