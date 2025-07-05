// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;

namespace FortitudeMarkets.Trading.Limits;


public enum LimitBreachOperator
{
    GreaterThan
  , LessThan
}

public readonly struct LimitKey
(
    LimitType limitType
  , decimal limitLevel
  , TimeBoundaryPeriod limitPeriod = TimeBoundaryPeriod.Eternity
  , decimal? alertLevel = null
  , int? limitCount = null
  , LimitBreachOperator breachOperator = LimitBreachOperator.GreaterThan) 
    : IEquatable<LimitKey>
{
    public LimitType LimitType { get; } = limitType;

    public decimal LimitLevel { get; } = limitLevel;

    public decimal AlertLevel { get; } = alertLevel ?? limitLevel;

    public int? LimitCount { get; } = limitCount;

    public TimeBoundaryPeriod? LimitPeriod { get; } = limitPeriod;

    public LimitBreachOperator BreachOperator { get; } = breachOperator;

    public bool Equals(LimitKey other) => 
        LimitType == other.LimitType && LimitLevel == other.LimitLevel && LimitCount == other.LimitCount 
     && LimitPeriod == other.LimitPeriod && BreachOperator == other.BreachOperator;

    public override bool Equals(object? obj) => obj is LimitKey other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)LimitType;
            hashCode = (hashCode * 397) ^ LimitLevel.GetHashCode();
            hashCode = (hashCode * 397) ^ LimitCount.GetHashCode();
            hashCode = (hashCode * 397) ^ LimitPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)BreachOperator;
            return hashCode;
        }
    }

    public override string ToString() => 
        $"{nameof(LimitType)}: {LimitType}, {nameof(LimitLevel)}: {LimitLevel}, {nameof(LimitCount)}: {LimitCount}, " +
        $"{nameof(LimitPeriod)}: {LimitPeriod}, {nameof(BreachOperator)}: {BreachOperator}";
}
