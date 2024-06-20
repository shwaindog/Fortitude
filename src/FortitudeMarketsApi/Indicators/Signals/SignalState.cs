// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Indicators.Signals;

public enum PredictedDirection : byte
{
    None = 0
  , Sideways
  , Up   // long/buy 
  , Down // short/sell
}

public enum SignalPublicationType : byte
{
    Directional
  , Informational
  , StatusChange
}

public enum StrengthRating : byte
{
    None = 0
  , VeryWeak        // 100% encompassing published strength values
  , Weak            // 80%
  , WeakToModerate  // 60%
  , Moderate        // 40%
  , Average         // 30%
  , ModerateStrong  // 20%
  , Firm            // 10%
  , Sturdy          // 5%
  , Solid           // 2%
  , Strong          // 1%
  , VeryStrong      // 0.5% 
  , ExtremelyStrong // 0.1%
}

public enum SignalAction : byte
{
    Created
  , ReplacedExisting
  , AccumulatedOnExisting
  , Received
  , TriggerOrder
  , Ignored
  , Cancelled
  , Forwarded
  , Filtered
  , Dropped
}

public enum ActionReason : byte
{
    ThresholdNotMet
  , ThresholdMet
  , DisabledConfig
  , AccuracyBelowThreshold
  , HistoryInsufficient
  , ReceiverStateNotNeeded
  , MarketConditionsNotMet
  , ReceiverDeactivated
}

public enum DecayType : byte
{
    Linear
  , HalfLife
  , TrickleDischargeCurve
  , Binary
}

public enum ContributionDirection : byte
{
    Ignored
  , Contributed
  , Detracted
}
