// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public interface ILevel3Quote : ILevel2Quote, ICloneable<ILevel3Quote>
{
    // ILastTradedList? TickLastTraded { get; }

    uint     BatchId              { get; }
    uint     SourceQuoteReference { get; }
    DateTime ValueDate            { get; }
    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);


    new ILevel3Quote Clone();
}

public interface IMutableLevel3Quote : ILevel3Quote, IMutableLevel2Quote, ITrackableReset<IMutableLevel3Quote>
{
    new uint BatchId { get; set; }

    new uint SourceQuoteReference { get; set; }

    new DateTime ValueDate { get; set; }


    new IMutableLevel3Quote ResetWithTracking();

    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new IMutableLevel3Quote Clone();
}

public interface IPublishableLevel3Quote : IPublishableLevel2Quote, ILevel3Quote, ICloneable<IPublishableLevel3Quote>
  , IDoublyLinkedListNode<IPublishableLevel3Quote>
{
    IOnTickLastTraded? OnTickLastTraded { get; }

    new ILevel3Quote AsNonPublishable { get; }

    new IPublishableLevel3Quote? Next     { get; set; }
    new IPublishableLevel3Quote? Previous { get; set; }

    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IPublishableLevel3Quote Clone();
}

public interface IMutablePublishableLevel3Quote : IPublishableLevel3Quote, IMutableLevel3Quote, IMutablePublishableLevel2Quote,
    ITrackableReset<IMutablePublishableLevel3Quote>
{
    new IMutableOnTickLastTraded? OnTickLastTraded { get; set; }

    new IMutableLevel3Quote AsNonPublishable { get; }

    new IMutablePublishableLevel3Quote Clone();

    new IMutablePublishableLevel3Quote ResetWithTracking();

    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);
}
