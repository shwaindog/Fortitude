// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

public class PQPricingClientRequestResponseRegistrationRule : RemoteRequestResponseRegistrationRule
{
    private const    uint     PQSourceTickerInfoResponseMessageId = (uint)PQMessageIds.SourceTickerInfoResponse;
    private readonly string   feedName;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingClientRequestResponseRegistrationRule));

    public PQPricingClientRequestResponseRegistrationRule
    (string feedName
      , ISocketSessionContext socketSessionContext, string? registrationRepoName = null)
        : base(feedName.FeedRegisterRemoteResponseRuleName(), socketSessionContext, feedName.FeedRequestResponseRegistrationAddress(),
               null, registrationRepoName) =>
        this.feedName = feedName;

    public override async ValueTask StartAsync()
    {
        logger.Info("PQPricingClientRequestResponseRegistrationRule for feedName {0} deployed on {1}", feedName, Context.RegisteredOn.Name);
        await LauncherRequestIdResponseListener();
    }

    protected override string ExtractSubscriptionPostfix(string fullMessageAddressDestination) =>
        fullMessageAddressDestination.ExtractTickerFromAmendPublicationAddress(feedName)
                                     .Replace(feedName.FeedAmendTickerPublicationAddress(), "");

    protected override void RuleOverrideDeserializerResolverNoMessageId(MessageDeserializerResolveRun messageDeserializerResolveRun)
    {
        if (CapturedAnyRootDeserializationRepository is not PQClientQuoteDeserializerRepository pqClientQuoteDeserializerRepository)
        {
            messageDeserializerResolveRun.FailureMessage
                = "Expected to have a root MessageDeserializationRepository that is of Type PQClientQuoteDeserializerRepository " +
                  "to be able look up ticker names to SourceTickerInfo and resolve a message deserializer.  Will not be able to register tickers requests.  " +
                  $"Will not be able to update ticker subscription requests for ticker {messageDeserializerResolveRun.SubscribePostFix}";
            logger.Warn(messageDeserializerResolveRun.FailureMessage);
            return;
        }

        if (messageDeserializerResolveRun.DeserializedType == typeof(PQSourceTickerInfoResponse))
        {
            messageDeserializerResolveRun.MessageId = PQSourceTickerInfoResponseMessageId;
            var pqSourceTickerInfoResponseDeserializer = pqClientQuoteDeserializerRepository
                .SourceNotifyingMessageDeserializerFromMessageId<PQSourceTickerInfoResponse>(PQSourceTickerInfoResponseMessageId);
            pqSourceTickerInfoResponseDeserializer!.RemoveOnZeroNotifiers = false;
            messageDeserializerResolveRun.MessageDeserializer             = pqSourceTickerInfoResponseDeserializer;
        }
    }
}
