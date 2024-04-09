#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClassNotRequired]
public static class PQBooleanValues
{
    public const byte IsReplayFlag = 0x01;
    public const byte IsExecutableFlag = 0x02;
    public const byte IsStaleQuote = 0x04;
}
