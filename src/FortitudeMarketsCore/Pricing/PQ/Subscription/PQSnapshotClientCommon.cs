// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClientCommon
{
    IList<ISourceTickerInfo> LastPublishedSourceTickerInfos { get; }

    ValueTask<PQSourceTickerInfoResponse?> RequestSourceTickerInfoListAsync
        (int timeout = 10_000, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);

    ValueTask<bool> RequestSnapshots
    (IList<ISourceTickerInfo> sourceTickerIds, int timeout = 10_000
      , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);
}
