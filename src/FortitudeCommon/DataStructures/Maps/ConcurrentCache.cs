using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.DataStructures.Maps
{
    public class ConcurrentCache<Tk, Tv> : ConcurrentMap<Tk, Tv>
    {
        private IEnumerable<KeyValuePair<Tk, Tv>> values = new KeyValuePair<Tk, Tv>[0];

        public ConcurrentCache()
        {
            OnUpdate += v => { values = v.ToArray(); };
        }

        public override IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}