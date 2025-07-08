using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Trading.Limits;

namespace FortitudeMarkets.Trading.Accounts;

public readonly struct ReserveResult(AccountLimitBreachResult? limitBreachResult, IOrderBlotterEntry reserveLimitsForBlotterEntry)
{
    public IOrderBlotterEntry ReserveLimitsForBlotterEntry { get; } = reserveLimitsForBlotterEntry;

    public bool PassesAllLimitChecks => LimitBreachResult == null;

    public AccountLimitBreachResult? LimitBreachResult { get; } = limitBreachResult;
}