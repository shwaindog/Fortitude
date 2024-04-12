#region

using FortitudeBusRules.Rules;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public class PQClientSnapshotRule : Rule
{
    private readonly ISocketDispatcherResolver dispatcherResolver;
    private IMarketConnectionConfig marketConnectionConfig;

    public PQClientSnapshotRule(IMarketConnectionConfig marketConnectionConfig, ISocketDispatcherResolver dispatcherResolver) : base(
        "PQClientSnapshotRule" + marketConnectionConfig.Name)
    {
        this.marketConnectionConfig = marketConnectionConfig;
        this.dispatcherResolver = dispatcherResolver;
    }

    public override ValueTask StartAsync() => base.StartAsync();
}
