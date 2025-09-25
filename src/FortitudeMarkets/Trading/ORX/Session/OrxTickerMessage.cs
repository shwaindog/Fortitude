// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeMarkets.Trading.ORX.Session;

public sealed class OrxTickerMessage : OrxTradingMessage
{
    public OrxTickerMessage(string exchange, string ticker)
    {
        Exchange = exchange;
        Ticker   = ticker;
    }

    public OrxTickerMessage() { }

    private OrxTickerMessage(OrxTickerMessage toClone)
    {
        CopyFrom(this);
    }

    public override uint MessageId => (uint)TradingMessageIds.Ticker;

    [OrxMandatoryField(10)] public MutableString? Exchange { get; set; }

    [OrxMandatoryField(11)] public MutableString? Ticker { get; set; }

    [OrxMandatoryField(12)] public long ContractSize { get; set; }

    [OrxMandatoryField(13)] public long MinimumSize { get; set; }

    [OrxMandatoryField(14)] public long SizeIncrement { get; set; }

    [OrxMandatoryField(15)] public long MaximumSize { get; set; }

    [OrxMandatoryField(16)] public decimal PriceIncrement { get; set; }

    [OrxMandatoryField(17)] public uint Mql { get; set; }

    [OrxMandatoryField(18)] public bool Tradeable { get; set; }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxTickerMessage message)
        {
            Exchange       = message.Exchange?.SyncOrRecycle(Exchange);
            Ticker         = message.Ticker?.SyncOrRecycle(Ticker);
            ContractSize   = message.ContractSize;
            MinimumSize    = message.MinimumSize;
            SizeIncrement  = message.SizeIncrement;
            MaximumSize    = message.MaximumSize;
            PriceIncrement = message.PriceIncrement;
            Mql            = message.Mql;
            Tradeable      = message.Tradeable;
        }

        return this;
    }

    public override void StateReset()
    {
        Exchange?.DecrementRefCount();
        Exchange = null;
        Ticker?.DecrementRefCount();
        Ticker         = null;
        ContractSize   = 0;
        MinimumSize    = 0;
        SizeIncrement  = 0;
        MaximumSize    = 0;
        PriceIncrement = 0;
        Mql            = 0;
        Tradeable      = false;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxTickerMessage>().CopyFrom(this) ?? new OrxTickerMessage(this);

    public override bool Equals(object? obj) => obj is OrxTickerMessage m && m.Exchange == Exchange && m.Ticker == Ticker;

    public override int GetHashCode()
    {
        var hash = 13;
        hash = hash * 7 + Exchange?.GetHashCode() ?? 0;
        hash = hash * 7 + Ticker?.GetHashCode() ?? 0;
        return hash;
    }

    internal void UpdateData
    (long contractSize, long minSize, long sizeInc, long maxSize,
        decimal priceInc,
        uint mql)
    {
        ContractSize   = contractSize;
        MinimumSize    = minSize;
        SizeIncrement  = sizeInc;
        MaximumSize    = maxSize;
        PriceIncrement = priceInc;
        Mql            = mql;
    }

    internal void UpdateStatus(bool tradeable)
    {
        Tradeable = tradeable;
    }

    public override string ToString() =>
        new StringBuilder(1024)
            .Append(Exchange).Append(',')
            .Append(Ticker).Append(',')
            .Append(ContractSize).Append(',')
            .Append(MinimumSize).Append(',')
            .Append(SizeIncrement).Append(',')
            .Append(MaximumSize).Append(',')
            .Append(PriceIncrement).Append(',')
            .Append(Mql).Append(',')
            .Append(Tradeable).ToString();
}
