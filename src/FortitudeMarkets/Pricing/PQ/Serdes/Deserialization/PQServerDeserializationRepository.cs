#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public sealed class PQServerDeserializationRepository : ConversationDeserializationRepository, IConversationDeserializationRepository
{
    public PQServerDeserializationRepository(string name, IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(name, recycler, cascadingFallbackDeserializationRepo) { }

    public override IMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) =>
        SourceDeserializerFromMessageId(msgId, typeof(TM)) as IMessageDeserializer<TM>;

    public override INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId) =>
        SourceDeserializerFromMessageId(msgId, typeof(TM)) as INotifyingMessageDeserializer<TM>;

    public override IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType)
    {
        switch (msgId)
        {
            case (uint)PQMessageIds.SnapshotIdsRequest: return new PQSnapshotIdsRequestDeserializer(Recycler);
            case (uint)PQMessageIds.SourceTickerInfoRequest: return new PQSourceTickerInfoRequestDeserializer(Recycler);
        }

        return CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType);
    }

    public override uint? ResolveExpectedMessageIdForMessageType(Type messageType)
    {
        if (messageType == typeof(PQSnapshotIdsRequest)) return (uint)PQMessageIds.SnapshotIdsRequest;
        if (messageType == typeof(PQSourceTickerInfoRequest)) return (uint)PQMessageIds.SourceTickerInfoRequest;

        return CascadingFallbackDeserializationFactoryRepo?.ResolveExpectedMessageIdForMessageType(messageType);
    }

    public override IPQServerMessageStreamDecoder Supply(string name) =>
        new PQServerMessageStreamDecoder(new PQServerDeserializationRepository(name, Recycler, this));
}
