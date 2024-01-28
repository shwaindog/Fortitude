#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ;

public interface IPQQuoteSerializerRepository : IMessageIdSerializationRepository, IMessageIdDeserializationRepository
{
    bool HasPictureDeserializers { get; }

    IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig streamp)
        where T : PQLevel0Quote, new();

    IPQDeserializer? GetQuoteDeserializer(IUniqueSourceTickerIdentifier identifier);

    void RemoveQuoteDeserializer(IUniqueSourceTickerIdentifier identifier);
}
