// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public interface IPQClientMessageStreamDecoderFactory : IMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply(string name);
}

public interface IPQClientQuoteDeserializerRepository : IConversationDeserializationRepository, IPQClientMessageStreamDecoderFactory
{
    new IPQClientMessageStreamDecoder Supply(string name);

    IPQMessageDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig)
        where T : class, IPQMutableMessage;

    IPQMessageDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType);

    IPQMessageDeserializer? GetDeserializer(ISourceTickerInfo identifier);

    bool UnregisterDeserializer(ISourceTickerInfo identifier);
}

public sealed class PQClientQuoteDeserializerRepository
    (string name, IRecycler recycler, IMessageDeserializationRepository? fallbackCoalasingDeserializer = null)
    : ConversationDeserializationRepository(name, recycler, fallbackCoalasingDeserializer), IPQClientQuoteDeserializerRepository
{
    public PQSourceTickerInfoResponseDeserializer StatelessPQSourceTickerInfoResponseDeserializer = new(recycler);

    public override IPQClientMessageStreamDecoder Supply(string name) =>
        new PQClientMessageStreamDecoder(new PQClientQuoteDeserializerRepository(name, Recycler, this));

    IMessageStreamDecoder IMessageStreamDecoderFactory.Supply(string name) => Supply(name);

    public IPQMessageDeserializer? GetDeserializer(ISourceTickerInfo identifier) =>
        TryGetDeserializer(identifier.SourceTickerId, out var quoteDeserializer)
            ? quoteDeserializer as IPQMessageDeserializer
            : CascadingFallbackDeserializationRepo?.GetDeserializer(identifier.SourceTickerId) as IPQMessageDeserializer;

    public bool UnregisterDeserializer(ISourceTickerInfo identifier) => UnregisterDeserializer(identifier.SourceTickerId);

    public IPQMessageDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig) where T : class, IPQMutableMessage
    {
        IPQMessageDeserializer quoteDeserializer = new PQMessageDeserializer<T>(streamPubConfig);
        RegisteredDeserializers.Add(streamPubConfig.SourceTickerInfo.SourceTickerId, quoteDeserializer);
        return quoteDeserializer;
    }

    public IPQMessageDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType)
    {
        return pqLevelQuoteMessageType switch
               {
                   Type when pqLevelQuoteMessageType == typeof(PQPublishableTickInstant) => CreateQuoteDeserializer<PQPublishableTickInstant>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQPublishableLevel1Quote) => CreateQuoteDeserializer<PQPublishableLevel1Quote>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQPublishableLevel2Quote) => CreateQuoteDeserializer<PQPublishableLevel2Quote>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQPublishableLevel3Quote) => CreateQuoteDeserializer<PQPublishableLevel3Quote>(streamPubConfig)

                 , _ => null
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
                   (uint)PQMessageIds.SourceTickerInfoResponse => StatelessPQSourceTickerInfoResponseDeserializer
                 , _ => CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType)
               };
    }

    public override uint? ResolveExpectedMessageIdForMessageType(Type messageType)
    {
        if (messageType == typeof(PQSourceTickerInfoResponse)) return (uint)PQMessageIds.SourceTickerInfoResponse;
        return CascadingFallbackDeserializationFactoryRepo?.ResolveExpectedMessageIdForMessageType(messageType);
    }
}
