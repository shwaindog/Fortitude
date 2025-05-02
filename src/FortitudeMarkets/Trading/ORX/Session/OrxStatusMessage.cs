// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeMarkets.Trading.ORX.Session;

public sealed class OrxStatusMessage : OrxTradingMessage
{
    public OrxStatusMessage() { }

    public OrxStatusMessage(OrxExchangeStatus orxExchangeStatus, MutableString exchangeName)
    {
        ExchangeStatus = orxExchangeStatus;
        ExchangeName   = exchangeName;
    }

    private OrxStatusMessage(OrxStatusMessage toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.StatusUpdate;

    [OrxMandatoryField(10)] public OrxExchangeStatus ExchangeStatus { get; set; }

    [OrxMandatoryField(11)] public MutableString? ExchangeName { get; set; }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxStatusMessage>().CopyFrom(this) ?? new OrxStatusMessage(this);

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxStatusMessage statusMessage)
        {
            ExchangeStatus = statusMessage.ExchangeStatus;
            ExchangeName   = statusMessage.ExchangeName?.CopyOrClone(ExchangeName);
        }

        return this;
    }

    public override void StateReset()
    {
        ExchangeStatus = OrxExchangeStatus.Down;
        ExchangeName?.DecrementRefCount();
        ExchangeName = null;
        base.StateReset();
    }

    public void Configure
        (OrxExchangeStatus orxExchangeStatus, string exchangeName, IRecycler orxRecyclingFactory)
    {
        Configure();
        ExchangeStatus = orxExchangeStatus;
        var mutableExchangeNameString = orxRecyclingFactory.Borrow<MutableString>();
        mutableExchangeNameString.Clear().Append(exchangeName);
        ExchangeName = mutableExchangeNameString;
    }
}
