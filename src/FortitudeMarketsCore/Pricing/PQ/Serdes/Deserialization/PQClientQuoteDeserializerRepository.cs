#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQClientMessageStreamDecoderFactory : IMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply();
}

public interface IPQClientQuoteDeserializerRepository : IConversationDeserializationRepository, IPQClientMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply();

    IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig streamPubConfig)
        where T : PQLevel0Quote, new();

    IPQDeserializer? GetDeserializer(IUniqueSourceTickerIdentifier identifier);

    bool UnregisterDeserializer(IUniqueSourceTickerIdentifier identifier);
}

public sealed class PQClientQuoteDeserializerRepository : ConversationDeserializationRepository, IPQClientQuoteDeserializerRepository
{
    private readonly PQFeedType feedType;

    public PQClientQuoteDeserializerRepository(IRecycler recycler, PQFeedType feed
        , IMessageDeserializationRepository? fallbackCoalasingDeserializer = null) : base(recycler, fallbackCoalasingDeserializer) =>
        feedType = feed;

    public override IPQClientMessageStreamDecoder Supply() => new PQClientMessageStreamDecoder(this, feedType);

    IMessageStreamDecoder IMessageStreamDecoderFactory.Supply() => Supply();

    public IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig streamPubConfig) where T : PQLevel0Quote, new()
    {
        PQDeserializerBase quoteDeserializer = new PQQuoteDeserializer<T>(streamPubConfig);
        RegisteredDeserializers.Add(streamPubConfig.Id, quoteDeserializer);
        return quoteDeserializer;
    }

    public IPQDeserializer? GetDeserializer(IUniqueSourceTickerIdentifier identifier) =>
        TryGetDeserializer(identifier.Id, out var quoteDeserializer) ?
            quoteDeserializer as IPQDeserializer :
            CascadingFallbackDeserializationRepo?.GetDeserializer(identifier.Id) as IPQDeserializer;

    public bool UnregisterDeserializer(IUniqueSourceTickerIdentifier identifier) => UnregisterDeserializer(identifier.Id);

    protected override IMessageDeserializer? SourceMessageDeserializer<TM>(uint msgId)
    {
        return msgId switch
        {
            (uint)PQMessageIds.SourceTickerInfoResponse => new PQSourceTickerInfoResponseDeserializer(Recycler)
            , _ => null
        };
    }
}
