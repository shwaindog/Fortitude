#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Conflation;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQLevel1Quote : IPQLevel0Quote, IMutableLevel1Quote
{
    new IPQPeriodSummary? PeriodSummary { get; set; }
    new IPQLevel1Quote Clone();
}
