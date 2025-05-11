// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication;

public interface IPQServerHeartBeatSender
{
    bool HasStarted { get; }

    IDoublyLinkedList<IPQPublishableTickInstant>? ServerLinkedQuotes { get; set; }

    ISyncLock?       ServerLinkedLock { get; set; }
    IPQUpdateServer? UpdateServer     { get; set; }

    void StartSendingHeartBeats();
    void StopAndWaitUntilFinished();
}
