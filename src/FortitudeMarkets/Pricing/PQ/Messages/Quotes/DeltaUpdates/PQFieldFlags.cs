namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public static class PQFieldFlags
{
    public const byte None = 0;

    // start of context specific flags
    // for order book
    public const byte IsAskSideFlag = 0x20;

    public const byte IsTraderCountOnlyFlag = 0x40;

    // for recentlytraded
    public const byte IsPaidFlag = 0x20;

    public const byte IsGivenFlag = 0x40;


    // for string field Update

    // Insert or Update or if missing is delete
    public const byte IsUpsert = 0x40;

    // end of context specific flags

    public const byte IsExtendedFieldId = 0x80;

    public const byte LayerExecutableFlag = 0x01;

    public static bool IsAsk(this PQFieldUpdate fieldUpdate) => (fieldUpdate.Flag & IsAskSideFlag) == IsAskSideFlag;

    public static bool IsBid(this PQFieldUpdate fieldUpdate) => (fieldUpdate.Flag & IsAskSideFlag) == 0;
}
