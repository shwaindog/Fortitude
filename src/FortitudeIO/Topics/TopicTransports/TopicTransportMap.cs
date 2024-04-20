#region

using System.Collections;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicTransports;

public class TopicTransportMap<T> : IMap<ITopicEndpointInfo, T> where T : ITransportTopicConversation
{
    private readonly IMap<ITopicEndpointInfo, T> backingMap = new GarbageAndLockFreeMap<ITopicEndpointInfo, T>(
        (lhs, rhs) => lhs.EquivalentEndpoint(rhs));

    public IEnumerator<KeyValuePair<ITopicEndpointInfo, T>> GetEnumerator() => backingMap.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T? this[ITopicEndpointInfo key]
    {
        get => backingMap[key];
        set => backingMap[key] = value;
    }

    public int Count => backingMap.Count;

    public IEnumerable<ITopicEndpointInfo> Keys => backingMap.Keys;
    public IEnumerable<T> Values => backingMap.Values;

    public T? GetValue(ITopicEndpointInfo key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public bool TryGetValue(ITopicEndpointInfo key, out T? value) => backingMap.TryGetValue(key, out value);

    public T GetOrPut(ITopicEndpointInfo key, Func<ITopicEndpointInfo, T> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            backingMap.Add(key, value);
        }

        return value!;
    }

    public T AddOrUpdate(ITopicEndpointInfo key, T value) => backingMap.AddOrUpdate(key, value);

    public bool Add(ITopicEndpointInfo key, T value) => backingMap.Add(key, value);

    public bool Remove(ITopicEndpointInfo key) => backingMap.Remove(key);

    public void Clear()
    {
        backingMap.Clear();
    }

    object ICloneable.Clone() => Clone();

    public IMap<ITopicEndpointInfo, T> Clone() => throw new NotImplementedException();

    public bool ContainsKey(ITopicEndpointInfo key) => backingMap.ContainsKey(key);

    public event Action<IEnumerable<KeyValuePair<ITopicEndpointInfo, T>>> OnUpdate
    {
        add => backingMap.OnUpdate += value;
        remove => backingMap.OnUpdate -= value;
    }
}
