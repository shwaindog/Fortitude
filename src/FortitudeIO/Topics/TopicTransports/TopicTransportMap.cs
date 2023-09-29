using System;
using System.Collections;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Topics.Config.ConnectionConfig;

namespace FortitudeIO.Topics.TopicTransports
{
    public class TopicTransportMap<T> : IMap<ITopicEndpointInfo, T> where T : ITransportTopicConversation
    {

        private IMap<ITopicEndpointInfo, T> backingMap = new GarbageAndLockFreeMap<ITopicEndpointInfo, T>(
            (lhs, rhs) => lhs.EquivalentEndpoint(rhs));

        public IEnumerator<FortitudeCommon.DataStructures.Maps.KeyValuePair<ITopicEndpointInfo, T>> GetEnumerator()
        {
            return backingMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[ITopicEndpointInfo key]
        {
            get => backingMap[key];
            set => backingMap[key] = value;
        }

        public int Count => backingMap.Count;
        public bool TryGetValue(ITopicEndpointInfo key, out T value)
        {
            return backingMap.TryGetValue(key, out value);
        }

        public void Add(ITopicEndpointInfo key, T value)
        {
            backingMap.Add(key, value);
        }

        public FortitudeCommon.DataStructures.Maps.KeyValuePair<ITopicEndpointInfo, T> Remove(ITopicEndpointInfo key)
        {
            return backingMap.Remove(key);
        }

        public void Clear()
        {
            backingMap.Clear();
        }

        public bool ContainsKey(ITopicEndpointInfo key)
        {
            return backingMap.ContainsKey(key);
        }

        public event Action<IEnumerable<FortitudeCommon.DataStructures.Maps.KeyValuePair<ITopicEndpointInfo, T>>> OnUpdate
        {
            add => backingMap.OnUpdate += value;
            remove => backingMap.OnUpdate -= value;
        }
    }
}
