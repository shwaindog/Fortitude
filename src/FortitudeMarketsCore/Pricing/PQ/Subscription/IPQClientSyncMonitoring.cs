using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public interface IPQClientSyncMonitoring
    {
        void RegisterNewDeserializer(IPQDeserializer quoteDeserializer);
        void UnregisterSerializer(IPQDeserializer quoteDeserializer);
        void CheckStartMonitoring();
        void CheckStopMonitoring();
    }
}