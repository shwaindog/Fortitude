// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication.BusRules.BusMessages;

public class PublishQuoteEvent : RecyclableObject
{
    public IPublishableTickInstant PublishQuote { get; set; } = null!;

    public PQMessageFlags? MessageFlags { get; set; }

    public uint? OverrideSequenceNumber { get; set; }

    public override void StateReset()
    {
        PublishQuote = null!;
        MessageFlags = null;

        OverrideSequenceNumber = null;
        base.StateReset();
    }

    public override string ToString() =>
        $"{nameof(PublishQuoteEvent)}({nameof(MessageFlags)}: {MessageFlags}, {nameof(OverrideSequenceNumber)}: {OverrideSequenceNumber}, " +
        $"{nameof(PublishQuote)}: {PublishQuote})";
}
