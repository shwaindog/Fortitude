// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Signals;

public struct DecisionFormingState
{
    private List<decimal>              StrengthThresholds { get; set; }
    private decimal                    CalculatedValue    { get; set; }
    public  List<SenderSignalDecision> AggregatingSignals { get; set; }
}

public struct SenderSignalDecision
{
    public int?  SignalId            { get; set; }
    public long? SignalPublicationId { get; set; }
    public uint  InstrumentId        { get; set; }

    public SignalState?  DecisionSignalState { get; set; }
    public SignalHistory SignalHistory       { get; set; }
    public ActionReason  Reason              { get; set; }

    public ContributionDirection ContributionDirection { get; set; }
    public decimal               Contribution          { get; set; }
}

public struct SignalHistory
{
    public DateTime? LastSignalAtOrAboveStrength { get; set; }
    public TimeSpan? RecentPeriodMs              { get; set; }
    public int?      RecentPublications          { get; set; }
    public double?   RecentAccuracy              { get; set; }
    public double?   RecentAccuracyAtStrength    { get; set; }
    public int       StrengthOrAboveOccurrence   { get; set; }
    public int       TotalOccurrence             { get; set; }
    public int       TotalOccurrenceAtStrength   { get; set; }
}

public struct SignalIndicatorHistory
{
    public int                IndicatorId    { get; set; }
    public IndicatorType      IndicatorType  { get; set; }
    public BidAskInstantPair? BidAskSnapshot { get; set; }
    public SignalState        SentState      { get; set; }
    public SignalState?       ReceivedState  { get; set; }
    public SignalAction       Action         { get; set; }

    public SignalHistory? IndicatorSignalHistory { get; set; }

    public decimal? TargetPrediction { get; set; }
    public int?     TargetEtaMs      { get; set; }

    public List<DecisionFormingState> DecisionState { get; set; }
}
