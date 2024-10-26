// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Indicators.Signals;

public struct SignalState
{
    public DateTime              ReceivedCreatedAt     { get; set; }
    public StrengthRating        StrengthRating        { get; set; }
    public decimal               StrengthValue         { get; set; }
    public SignalPublicationType SignalPublicationType { get; set; }
    public PredictedDirection    PredictedDirection    { get; set; }
}

public struct LifeTime
{
    public DateTime  CreatedTime       { get; set; }
    public DateTime  TimeAtDeath       { get; set; }
    public DecayType StrengthDecayType { get; set; }
}

public interface ISignal
{
    long   SignalId        { get; }
    int    SignalDetailsId { get; }
    int    SenderId        { get; }
    ushort ProcessedCount  { get; }

    SignalIndicatorHistory       SenderState               { get; }
    SignalIndicatorHistory?      OriginatorState           { get; }
    List<SignalIndicatorHistory> SignalModificationHistory { get; }

    long? ReplaceSignalPublicationId { get; }

    uint        InstrumentId              { get; }
    SignalState CurrentState              { get; }
    Prediction  SenderPrediction          { get; }
    decimal?    SenderInvalidateThreshold { get; }
}

public struct Signal : ISignal
{
    public long   SignalId        { get; set; }
    public int    SignalDetailsId { get; set; }
    public int    SenderId        { get; set; }
    public ushort ProcessedCount  { get; set; }

    public SignalIndicatorHistory       SenderState               { get; set; }
    public SignalIndicatorHistory?      OriginatorState           { get; set; }
    public List<SignalIndicatorHistory> SignalModificationHistory { get; set; }

    public long? ReplaceSignalPublicationId { get; set; }

    public uint        InstrumentId              { get; set; }
    public SignalState CurrentState              { get; set; }
    public Prediction  SenderPrediction          { get; set; }
    public decimal?    SenderInvalidateThreshold { get; set; }
}
