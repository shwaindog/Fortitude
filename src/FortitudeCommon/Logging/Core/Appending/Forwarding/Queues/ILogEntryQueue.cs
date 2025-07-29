// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;

public interface ILogEntryQueue
{
    int Capacity { get; }

    int Count { get; }

    FullQueueHandling InboundQueueFullHandling { get; }

    bool TryEnqueue(IFLogEntry logEntry);

    int Enqueue(IFLogEntry logEntry);

    int RunRemoveFilter(IFLogEntry blockedLogEntry, Predicate<IFLogEntry> removePredicate);

    IFLogEntry? ForceEnqueue(IFLogEntry logEntry);

    IFLogEntry? ReplaceNewestQueued(IFLogEntry flogEntry);

    (IFLogEntry?, IFLogEntry?) ReplaceNewestQueued(IFLogEntry lastFlogEntry, IFLogEntry secondLastFlogEntry);

    bool QueuedItemsAny(Predicate<IFLogEntry> predicate);

    int DropAll();

    IFLogEntry Poll();

    IFLogEntry? TryPoll();

    IReusableList<IFLogEntry> PollBatch(int maxBatchSize, IReusableList<IFLogEntry> toPopulate);
}
