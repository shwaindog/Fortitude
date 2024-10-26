// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Signals;

public struct PostDecisionAccuracy
{
    public BidAskInstantPair MarketTopOfBook { get; set; }
    public decimal           Accuracy        { get; set; }
}

public struct PostSignalReceiverAccuracy
{
    public long         SignalId           { get; set; }
    public int          PublisherId        { get; set; }
    public SignalAction OriginalAction     { get; set; }
    public Prediction   Prediction         { get; set; }
    public TimeSpan     LifeLeftAtDecision { get; set; }

    public List<PostDecisionAccuracy> FibanacciAccuracyToEndTime { get; set; }
}
