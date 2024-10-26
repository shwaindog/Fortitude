// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Signals;

public struct Prediction
{
    public BidAskInstantPair? MarketAtPrediction    { get; set; }
    public LifeTime           LifeTime              { get; set; }
    public PredictedDirection PredictedDirection    { get; set; }
    public decimal            TargetPredictionPrice { get; set; }
}
