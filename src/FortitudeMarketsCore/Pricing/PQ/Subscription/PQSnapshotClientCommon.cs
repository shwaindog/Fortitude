#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClientCommon
{
    IList<ISourceTickerQuoteInfo> LastPublishedSourceTickerQuoteInfos { get; }

    ValueTask<PQSourceTickerInfoResponse?> RequestSourceTickerQuoteInfoListAsync(int timeout = 10_000
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);

    ValueTask<bool> RequestSnapshots(IList<ISourceTickerQuoteInfo> sourceTickerIds, int timeout = 10_000
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);
}
