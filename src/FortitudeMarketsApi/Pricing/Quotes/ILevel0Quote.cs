#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel0Quote : IReusableObject<ILevel0Quote>, IInterfacesComparable<ILevel0Quote>
{
    bool IsReplay { get; }
    DateTime SourceTime { get; }
    DateTime ClientReceivedTime { get; }
    ISourceTickerQuoteInfo? SourceTickerQuoteInfo { get; }
    decimal SinglePrice { get; }
    new ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}
