using System.Globalization;

namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public interface IOSThread
    {
        void Start();
        void Start(object parameterized);
        string Name { get; set; }
        CultureInfo CurrentCultureInfo { get; }
        void Join();
        void Join(int milliseconds);
        bool IsAlive { get; }
        bool IsBackground { get; set; }
        void Abort();
    }
}