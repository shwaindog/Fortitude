#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQClientMessageStreamDecoderFactory : IMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply(string name);
}

public interface IPQClientQuoteDeserializerRepository : IConversationDeserializationRepository, IPQClientMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply(string name);

    IPQDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig)
        where T : PQLevel0Quote, new();

    IPQDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType);

    IPQDeserializer? GetDeserializer(ISourceTickerQuoteInfo identifier);

    bool UnregisterDeserializer(ISourceTickerQuoteInfo identifier);
}

public sealed class PQClientQuoteDeserializerRepository : ConversationRepository, IPQClientQuoteDeserializerRepository
{
    public PQSourceTickerInfoResponseDeserializer statelessPQSourceTickerInfoResponseDeserializer;

    public PQClientQuoteDeserializerRepository(string name, IRecycler recycler
        , IMessageDeserializationRepository? fallbackCoalasingDeserializer = null) : base(name,
        recycler, fallbackCoalasingDeserializer) =>
        statelessPQSourceTickerInfoResponseDeserializer = new PQSourceTickerInfoResponseDeserializer(recycler);

    public override IPQClientMessageStreamDecoder Supply(string name) =>
        new PQClientMessageStreamDecoder(new PQClientQuoteDeserializerRepository(name, Recycler, this));

    IMessageStreamDecoder IMessageStreamDecoderFactory.Supply(string name) => Supply(name);

    public IPQDeserializer? GetDeserializer(ISourceTickerQuoteInfo identifier) =>
        TryGetDeserializer(identifier.Id, out var quoteDeserializer) ?
            quoteDeserializer as IPQDeserializer :
            CascadingFallbackDeserializationRepo?.GetDeserializer(identifier.Id) as IPQDeserializer;

    public bool UnregisterDeserializer(ISourceTickerQuoteInfo identifier) => UnregisterDeserializer(identifier.Id);

    public IPQDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig) where T : PQLevel0Quote, new()
    {
        PQDeserializerBase quoteDeserializer = new PQQuoteDeserializer<T>(streamPubConfig);
        RegisteredDeserializers.Add(streamPubConfig.SourceTickerQuoteInfo.Id, quoteDeserializer);
        return quoteDeserializer;
    }

    public IPQDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType)
    {
        return pqLevelQuoteMessageType switch
        {
            Type when pqLevelQuoteMessageType == typeof(PQLevel0Quote) => CreateQuoteDeserializer<PQLevel0Quote>(streamPubConfig)
            , Type when pqLevelQuoteMessageType == typeof(PQLevel1Quote) => CreateQuoteDeserializer<PQLevel1Quote>(streamPubConfig)
            , Type when pqLevelQuoteMessageType == typeof(PQLevel2Quote) => CreateQuoteDeserializer<PQLevel2Quote>(streamPubConfig)
            , Type when pqLevelQuoteMessageType == typeof(PQLevel3Quote) => CreateQuoteDeserializer<PQLevel3Quote>(streamPubConfig), _ => null
        };
    }

    public override INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId) =>
        SourceDeserializerFromMessageId(msgId, typeof(TM)) as INotifyingMessageDeserializer<TM>;

    public override IMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) =>
        SourceDeserializerFromMessageId(msgId, typeof(TM)) as IMessageDeserializer<TM>;

    public override IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType)
    {
        return msgId switch
        {
            (uint)PQMessageIds.SourceTickerInfoResponse => statelessPQSourceTickerInfoResponseDeserializer
            , _ => CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType)
        };
    }

    public override uint? ResolveExpectedMessageIdForMessageType(Type messageType)
    {
        if (messageType == typeof(PQSourceTickerInfoResponse)) return (uint)PQMessageIds.SourceTickerInfoResponse;
        return CascadingFallbackDeserializationFactoryRepo?.ResolveExpectedMessageIdForMessageType(messageType);
        ;
    }
}
