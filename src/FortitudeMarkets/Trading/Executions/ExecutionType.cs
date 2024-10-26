namespace FortitudeMarkets.Trading.Executions;

public enum ExecutionType
{
    Unknown = 0
    , OrderPaid
    , // Paid the asking price. i.e. Aggressed the offer
    OrderGave
    , // Gave the bidder the bid price. i.e. Aggressed the bid
    CounterPartyPaid
    , // Counterparty paid the orders offer price
    CounterPartyGave
    , // Counterparty gave the order its bid price
    MatchedAtMidNoWait
    , MatchedAtMidWithWait
}
