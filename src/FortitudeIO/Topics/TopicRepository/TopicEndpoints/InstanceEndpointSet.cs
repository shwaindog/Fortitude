using System;
using System.Collections;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Lists;
using FortitudeIO.Topics.Config.ConnectionConfig;

namespace FortitudeIO.Topics.TopicRepository.TopicEndpoints
{
    public interface IInstanceEndpointSet : ICollection<ITopicEndpointInfo>
    {
    }

    public class InstanceEndpointSet : IInstanceEndpointSet
    {
        private ReuseIteratorList<ITopicEndpointInfo> backingList;

        public InstanceEndpointSet()
        {
            backingList = new ReuseIteratorList<ITopicEndpointInfo>();
        }

        public InstanceEndpointSet(int capacity)
        {
            backingList = new ReuseIteratorList<ITopicEndpointInfo>(capacity);
        }

        public InstanceEndpointSet(IEnumerable<ITopicEndpointInfo> collection)
        {
            backingList = new ReuseIteratorList<ITopicEndpointInfo>(collection);
        }

        public IEnumerator<ITopicEndpointInfo> GetEnumerator()
        {
            return backingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ITopicEndpointInfo item)
        {
            if (Contains(item))
            {
                throw new ArgumentException("Trying to add duplicate InstanceConnectionDetails");
            }
            backingList.Add(item);
        }

        public void Clear()
        {
            backingList.Clear();
        }

        public bool Contains(ITopicEndpointInfo item)
        {
            return backingList.Contains(item);
        }

        public void CopyTo(ITopicEndpointInfo[] array, int arrayIndex)
        {
            backingList.CopyTo(array, arrayIndex);
        }

        public bool Remove(ITopicEndpointInfo item)
        {
            return backingList.Remove(item);
        }

        public int Count => backingList.Count;
        public bool IsReadOnly => false;
    }
}
