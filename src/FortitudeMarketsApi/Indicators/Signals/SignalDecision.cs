// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Signals;

public interface ISignalDecision
{
    Signal       Signal     { get; }
    int          ReceiverId { get; }
    SignalAction Action     { get; }

    TimeSpan      LifeLeftAtDecision         { get; }
    SignalHistory ReceiverSignalHistory      { get; }
    decimal       AppliedStrengthScaling     { get; }
    Prediction?   ReceiverPrediction         { get; }
    long?         TriggeredPublicationSignal { get; }

    List<KeyPricePoints> KeyPricePoints { get; }

    List<BidAskInstantPair>    FibanacciSampleTopOfBook                  { get; }
    List<VolatilityPeriod>     InstrumentFibancciSampleVolatilityPeriods { get; }
    List<MovingAveragePeriods> FibancciSampleMovingAveragePeriods        { get; }
}

public struct SignalDecision : ISignalDecision
{
    public Signal       Signal     { get; set; }
    public int          ReceiverId { get; set; }
    public SignalAction Action     { get; set; }

    public TimeSpan      LifeLeftAtDecision         { get; set; }
    public SignalHistory ReceiverSignalHistory      { get; set; }
    public decimal       AppliedStrengthScaling     { get; set; }
    public Prediction?   ReceiverPrediction         { get; set; }
    public long?         TriggeredPublicationSignal { get; set; }

    public List<KeyPricePoints> KeyPricePoints { get; set; }

    public List<BidAskInstantPair>    FibanacciSampleTopOfBook                  { get; set; }
    public List<VolatilityPeriod>     InstrumentFibancciSampleVolatilityPeriods { get; set; }
    public List<MovingAveragePeriods> FibancciSampleMovingAveragePeriods        { get; set; }
}
