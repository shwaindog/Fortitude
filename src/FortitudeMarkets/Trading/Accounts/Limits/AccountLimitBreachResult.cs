using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Trading.Limits;

public readonly struct AccountLimitBreachResult(uint accountId, decimal currentLevel, decimal proposedLevel, LimitKey limitKey)
{
    public uint AccountId { get; } = accountId;

    public decimal CurrentLevel { get; } = currentLevel;

    public decimal ProposedLevel { get; } = proposedLevel;

    public LimitKey LimitKey { get; } = limitKey;
}