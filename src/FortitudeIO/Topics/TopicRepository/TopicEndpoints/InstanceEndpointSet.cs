#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicRepository.TopicEndpoints;

public interface IInstanceEndpointSet : ICollection<ITopicEndpointInfo> { }

public class InstanceEndpointSet : IInstanceEndpointSet
{
    private AutoRecycleEnumerator<ITopicEndpointInfo> singleEnumerator = new();
    private List<ITopicEndpointInfo> topicEndpoints;

    public InstanceEndpointSet() => topicEndpoints = new List<ITopicEndpointInfo>();

    public InstanceEndpointSet(int capacity) => topicEndpoints = new List<ITopicEndpointInfo>(capacity);

    public InstanceEndpointSet(IEnumerable<ITopicEndpointInfo> collection) => topicEndpoints = collection.ToList();

    public IEnumerator<ITopicEndpointInfo> GetEnumerator()
    {
        singleEnumerator.Clear();
        singleEnumerator.AddRange(topicEndpoints);
        return singleEnumerator;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(ITopicEndpointInfo item)
    {
        if (Contains(item)) throw new ArgumentException("Trying to add duplicate InstanceConnectionDetails");
        topicEndpoints.Add(item);
    }

    public void Clear()
    {
        topicEndpoints.Clear();
    }

    public bool Contains(ITopicEndpointInfo item) => topicEndpoints.Contains(item);

    public void CopyTo(ITopicEndpointInfo[] array, int arrayIndex)
    {
        topicEndpoints.CopyTo(array, arrayIndex);
    }

    public bool Remove(ITopicEndpointInfo item) => topicEndpoints.Remove(item);

    public int Count => topicEndpoints.Count;
    public bool IsReadOnly => false;
}
