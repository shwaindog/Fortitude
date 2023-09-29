using System.Collections.Generic;
using System.Linq;

namespace FortitudeIO.Topics.Config
{
    public interface ITopicConfigRepository
    {
        void UpsertConfig(ITopicConfig upsertTopicConfig);
        IEnumerable<ITopicConfig> AllInstanceConfig { get; }
        ITopicConfig GetConfigForTopic(string topicName);
    }

    public class TopicConfigRepository : ITopicConfigRepository
    {
        private List<ITopicConfig> configs = new List<ITopicConfig>();
       

        public void UpsertConfig(ITopicConfig upsertTopicConfig)
        {
            foreach (var topicConfig in configs)
            {
                if (topicConfig.TopicName == upsertTopicConfig.TopicName)
                {
                    configs.Remove(topicConfig);
                    break;
                }
            }
            configs.Add(upsertTopicConfig);
        }

        public IEnumerable<ITopicConfig> AllInstanceConfig => configs;

        public ITopicConfig GetConfigForTopic(string topicName)
        {
            return AllInstanceConfig.FirstOrDefault(topicConfig => topicConfig.TopicName == topicName);
        }
    }
}
