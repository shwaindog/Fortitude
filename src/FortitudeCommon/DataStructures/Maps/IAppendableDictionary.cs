using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.DataStructures.Maps;

public interface IStickyKeyValueDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TValue : notnull
{
}


public interface IAppendableDictionary<TKey, TValue> : IStickyKeyValueDictionary<TKey, TValue>
where TValue : notnull
{
    new TValue this[TKey loggerName] { get; set; }

    void Add(TKey key, TValue value);

    void Add(KeyValuePair<TKey, TValue> item);

    new ICollection<TKey> Keys { get; }

    new ICollection<TValue> Values { get; }
}