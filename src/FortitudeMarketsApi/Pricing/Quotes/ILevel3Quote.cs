// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel3Quote : ILevel2Quote, ICloneable<ILevel3Quote>, IDoublyLinkedListNode<ILevel3Quote>
{
    IRecentlyTraded? RecentlyTraded { get; }

    uint     BatchId              { get; }
    uint     SourceQuoteReference { get; }
    DateTime ValueDate            { get; }

    new ILevel3Quote? Next     { get; set; }
    new ILevel3Quote? Previous { get; set; }

    new ILevel3Quote Clone();
}

public interface IMutableLevel3Quote : ILevel3Quote, IMutableLevel2Quote
{
    new uint BatchId { get; set; }

    new uint SourceQuoteReference { get; set; }

    new DateTime ValueDate { get; set; }

    new IMutableRecentlyTraded? RecentlyTraded { get; set; }
    new IMutableLevel3Quote     Clone();
}
