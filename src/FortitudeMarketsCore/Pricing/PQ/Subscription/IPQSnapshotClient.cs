#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClient : IConversationRequester
{
    IMessageStreamDecoder MessageStreamDecoder { get; }

    void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds);
    void Send(IVersionedMessage versionedMessage);
}
