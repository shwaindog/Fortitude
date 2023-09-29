using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.DataStructures.Maps
{
    public class LinkedListCache<Tk, Tv> : LinkedListMap<Tk, Tv>
    {
        private IEnumerable<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> values = new FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>[0];

        public LinkedListCache()
        {
            OnUpdate += v => { values = v.ToArray(); };
        }

        public override IEnumerator<FortitudeCommon.DataStructures.Maps.KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}