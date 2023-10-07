namespace FortitudeIO.Topics.Config;

public interface ITopicConfigRepository
{
    IEnumerable<ITopicConfig> AllInstanceConfig { get; }
    void UpsertConfig(ITopicConfig upsertTopicConfig);
    ITopicConfig? GetConfigForTopic(string topicName);
}

public class TopicConfigRepository : ITopicConfigRepository
{
    private readonly List<ITopicConfig> configs = new();


    public void UpsertConfig(ITopicConfig upsertTopicConfig)
    {
        foreach (var topicConfig in configs)
            if (topicConfig.TopicName == upsertTopicConfig.TopicName)
            {
                configs.Remove(topicConfig);
                break;
            }

        configs.Add(upsertTopicConfig);
    }

    public IEnumerable<ITopicConfig> AllInstanceConfig => configs;

    public ITopicConfig? GetConfigForTopic(string topicName)
    {
        return AllInstanceConfig.FirstOrDefault(topicConfig => topicConfig.TopicName == topicName);
    }
}
