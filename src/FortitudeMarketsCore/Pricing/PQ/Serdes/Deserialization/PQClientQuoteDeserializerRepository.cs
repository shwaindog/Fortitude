// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
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

    IPQQuoteDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig)
        where T : PQTickInstant, new();

    IPQQuoteDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType);

    IPQQuoteDeserializer? GetDeserializer(ISourceTickerInfo identifier);

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

    public IPQQuoteDeserializer? GetDeserializer(ISourceTickerInfo identifier) =>
        TryGetDeserializer(identifier.SourceTickerId, out var quoteDeserializer)
            ? quoteDeserializer as IPQQuoteDeserializer
            : CascadingFallbackDeserializationRepo?.GetDeserializer(identifier.SourceTickerId) as IPQQuoteDeserializer;

    public bool UnregisterDeserializer(ISourceTickerInfo identifier) => UnregisterDeserializer(identifier.SourceTickerId);

    public IPQQuoteDeserializer CreateQuoteDeserializer<T>(ITickerPricingSubscriptionConfig streamPubConfig) where T : PQTickInstant, new()
    {
        IPQQuoteDeserializer quoteDeserializer = new PQQuoteDeserializer<T>(streamPubConfig);
        RegisteredDeserializers.Add(streamPubConfig.SourceTickerInfo.SourceTickerId, quoteDeserializer);
        return quoteDeserializer;
    }

    public IPQQuoteDeserializer? CreateQuoteDeserializer(ITickerPricingSubscriptionConfig streamPubConfig, Type pqLevelQuoteMessageType)
    {
        return pqLevelQuoteMessageType switch
               {
                   Type when pqLevelQuoteMessageType == typeof(PQTickInstant) => CreateQuoteDeserializer<PQTickInstant>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQLevel1Quote) => CreateQuoteDeserializer<PQLevel1Quote>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQLevel2Quote) => CreateQuoteDeserializer<PQLevel2Quote>(streamPubConfig)
                 , Type when pqLevelQuoteMessageType == typeof(PQLevel3Quote) => CreateQuoteDeserializer<PQLevel3Quote>(streamPubConfig)

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
