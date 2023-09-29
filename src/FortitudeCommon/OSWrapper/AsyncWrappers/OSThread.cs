using System.Globalization;
using System.Threading;

namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public class OSThread : IOSThread
    {
        private readonly Thread thread;

        public OSThread(Thread thread)
        {
            this.thread = thread;
        }

        public void Start()
        {
            thread.Start();
        }
        public void Start(object parameterized)
        {
            thread.Start(parameterized);
        }

        public void Foo()
        {
            thread.Abort();
        }

        public void Join()
        {
            thread.Join();
        }

        public void Join(int milliseconds)
        {
            thread.Join(milliseconds);
        }

        public void Abort()
        {
            thread.Abort();
        }

        public bool IsAlive => thread.IsAlive;
        public bool IsBackground { get { return thread.IsBackground; } set { thread.IsBackground = value; } }

        public CultureInfo CurrentCultureInfo => thread.CurrentCulture;

        public string Name
        {
            get { return thread.Name; }
            set { thread.Name = value; }
        }
    }
}
