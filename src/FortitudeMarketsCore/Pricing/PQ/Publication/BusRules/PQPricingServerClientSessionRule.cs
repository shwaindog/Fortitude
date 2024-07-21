// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public class PQPricingServerClientSessionRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerClientSessionRule));

    private readonly string feedName;

    private readonly IConversationRequester snapshotResponderClient;

    public PQPricingServerClientSessionRule(string feedName, IConversationRequester snapshotResponderClient)
        : base(feedName + "_" + nameof(PQPricingServerClientSessionRule))
    {
        this.feedName = feedName;

        this.snapshotResponderClient = snapshotResponderClient;
    }

    public override ValueTask StartAsync()
    {
        Logger.Info("New PQSnapshot Client Session Rule started {0}", snapshotResponderClient);
        var clientSocketReceiver = (ISocketReceiver)snapshotResponderClient.StreamListener!;
        var clientDecoder        = (IPQServerMessageStreamDecoder)clientSocketReceiver.Decoder!;
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSnapshotIdsRequest>()
                     .AddDeserializedNotifier
                         (new PassThroughDeserializedNotifier<PQSnapshotIdsRequest>
                             ($"{nameof(PQPricingServerClientSessionRule)}.{nameof(OnSnapshotIdsRequest)}"
                            , DeserializeNotifyTypeFlags.JustMessage,
                              new InvokeRuleCallbackListenContext<PQSnapshotIdsRequest>
                                  ($"{nameof(OnSnapshotIdsRequest)}", this, OnSnapshotIdsRequest)));
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSourceTickerInfoRequest>()
                     .AddDeserializedNotifier
                         (new PassThroughDeserializedNotifier<PQSourceTickerInfoRequest>
                             ($"{nameof(PQPricingServerClientSessionRule)}.{nameof(OnSourceTickerInfoRequest)}"
                            , DeserializeNotifyTypeFlags.JustMessage,
                              new InvokeRuleCallbackListenContext<PQSourceTickerInfoRequest>
                                  ($"{nameof(OnSourceTickerInfoRequest)}", this, OnSourceTickerInfoRequest)));
        snapshotResponderClient.Stopped += ReceivedClientStopped;
        snapshotResponderClient.Start(); // not started automatically waits for rule to set deserializers before registering for listen.
        IncrementLifeTimeCount();
        return ValueTask.CompletedTask;
    }

    public override ValueTask StopAsync()
    {
        Logger.Info($"Closing {snapshotResponderClient}");
        return base.StopAsync();
    }

    private async ValueTask OnSnapshotIdsRequest(PQSnapshotIdsRequest snapshotIdsRequestMsg)
    {
        var reusableList = Context.PooledRecycler.Borrow<ReusableList<uint>>();
        reusableList.AddRange(snapshotIdsRequestMsg.RequestSourceTickerIds);
        var lastPublishedQuotesResponse = await this.RequestAsync<IList<uint>, IList<IPQTickInstant>>(
             feedName.FeedTickerLastPublishedQuotesRequestAddress(), reusableList, new DispatchOptions());
        foreach (var pqTickInstant in lastPublishedQuotesResponse) snapshotResponderClient.Send(pqTickInstant);
    }

    private async ValueTask OnSourceTickerInfoRequest(PQSourceTickerInfoRequest sourceTickerInfoRequest)
    {
        var lastPublishedQuotesResponse = await this.RequestAsync<string, FeedSourceTickerInfoUpdate>(
             feedName.FeedPricingConfiguredTickersRequestAddress(), feedName, new DispatchOptions());
        var clientResponse = Context.PooledRecycler.Borrow<PQSourceTickerInfoResponse>();
        clientResponse.SourceTickerInfos.AddRange(lastPublishedQuotesResponse.SourceTickerInfos);
        clientResponse.RequestId  = sourceTickerInfoRequest.RequestId;
        clientResponse.ResponseId = sourceTickerInfoRequest.RequestId;
        snapshotResponderClient.Send(clientResponse);
    }

    private void ReceivedClientStopped()
    {
        Logger.Info($"Received Client Session Rule stopped event {snapshotResponderClient}");
        Timer.RunIn(2_000, () => DecrementLifeTimeCount());
    }
}
