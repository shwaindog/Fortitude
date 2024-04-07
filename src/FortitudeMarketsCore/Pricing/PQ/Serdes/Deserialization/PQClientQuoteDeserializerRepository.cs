#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQClientMessageStreamDecoderFactory : IMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply();
}

public interface IPQClientQuoteDeserializerRepository : IMessageDeserializationRepository, IPQClientMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply();

    IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig streamPubConfig)
        where T : PQLevel0Quote, new();

    IPQDeserializer? GetDeserializer(IUniqueSourceTickerIdentifier identifier);

    bool UnregisterDeserializer(IUniqueSourceTickerIdentifier identifier);
}

public sealed class PQClientQuoteDeserializerRepository : MessageDeserializationRepository, IPQClientQuoteDeserializerRepository
{
    private readonly PQFeedType feedType;

    public PQClientQuoteDeserializerRepository(IRecycler recycler, PQFeedType feed
        , IMessageDeserializationRepository? fallbackCoalasingDeserializer = null) : base(recycler, fallbackCoalasingDeserializer) =>
        feedType = feed;

    public IPQClientMessageStreamDecoder Supply() => new PQClientMessageStreamDecoder(this, feedType);

    IMessageStreamDecoder IMessageStreamDecoderFactory.Supply() => Supply();

    public override bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null)
    {
        if (messageDeserializer == null) return false;
        var instanceOfTypeToSerialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            if (CascadingFallbackDeserializationRepo == null ||
                !CascadingFallbackDeserializationRepo.IsRegisteredWithType<INotifyingMessageDeserializer<TM>, TM>(msgId))
            {
                RegisteredDeserializers.Add(msgId, messageDeserializer);
                return true;
            }

            return CascadingFallbackDeserializationRepo.IsRegistered(msgId);
        }

        if (existingMessageDeserializer as INotifyingMessageDeserializer<TM> == null)
            throw new Exception("Two different message types cannot be registered to the same Id");

        RegisteredDeserializers.AddOrUpdate(msgId, messageDeserializer);
        return true;
    }

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
}

public sealed class PQClientQuoteSerializerRepository :
    FactorySerializationRepository
{
    public PQClientQuoteSerializerRepository(IRecycler recycler, IMessageSerializationRepository? fallbackCoalasingDeserializer = null) : base(
        recycler, fallbackCoalasingDeserializer)
    {
        RegisterSerializer<PQSnapshotIdsRequest>();
    }

    protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId)
    {
        if (typeof(TM) == typeof(PQSnapshotIdsRequest)) return new PQSnapshotIdsRequestSerializer();
        throw new NotSupportedException();
    }
}
