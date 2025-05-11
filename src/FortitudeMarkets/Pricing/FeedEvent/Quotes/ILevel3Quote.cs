// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public interface ILevel3Quote : ILevel2Quote, ICloneable<ILevel3Quote>
{
    // ILastTradedList? TickLastTraded { get; }
    IRecentlyTraded? RecentlyTraded { get; }

    uint     BatchId              { get; }
    uint     SourceQuoteReference { get; }
    DateTime ValueDate            { get; }
    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);


    new ILevel3Quote Clone();
}

public interface IMutableLevel3Quote : ILevel3Quote, IMutableLevel2Quote
{
    new uint BatchId { get; set; }

    new uint SourceQuoteReference { get; set; }

    new DateTime ValueDate { get; set; }

    new IMutableRecentlyTraded? RecentlyTraded { get; set; }

    new bool                    AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new IMutableLevel3Quote     Clone();
}

public interface IPublishableLevel3Quote : IPublishableLevel2Quote, ILevel3Quote, ICloneable<IPublishableLevel3Quote>
  , IDoublyLinkedListNode<IPublishableLevel3Quote>
{
    new IPublishableLevel3Quote? Next     { get; set; }
    new IPublishableLevel3Quote? Previous { get; set; }

    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IPublishableLevel3Quote Clone();
}

public interface IMutablePublishableLevel3Quote : IPublishableLevel3Quote, IMutableLevel3Quote, IMutablePublishableLevel2Quote
{
    new IMutablePublishableLevel3Quote Clone();

    
    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);
}
