using System.Collections;
using System.Collections.Generic;

namespace FortitudeTests.TestHelpers
{
    public class DelayEnumerator<T> : IEnumerator<T>
    {
        private IEnumerable<T> delayActualEnumerator;
        private IEnumerator<T> realizedEnumerator;

        public DelayEnumerator(IEnumerable<T> delayActualEnumerator)
        {
            this.delayActualEnumerator = delayActualEnumerator;
        }

        private IEnumerator<T> Enum => realizedEnumerator ?? (
                                          realizedEnumerator = delayActualEnumerator.GetEnumerator());

        public void Dispose()
        {
            Enum.Dispose();
        }

        public bool MoveNext()
        {
            return Enum.MoveNext();
        }

        public void Reset()
        {
            Enum.Reset();
        }

        object IEnumerator.Current => Current;

        public T Current => Enum.Current;
    }
}
