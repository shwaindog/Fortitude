#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Converters;

public static class PQQuoteExtensions
{
    public static PQLevel0Quote ToL0PQQuote(this Level0PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel0Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel0Quote(vanillaQuote);
    }

    public static PQLevel1Quote ToL1PQQuote(this Level1PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel1Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel1Quote(vanillaQuote);
    }

    public static PQLevel2Quote ToL2PQQuote(this Level2PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel2Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel2Quote(vanillaQuote);
    }

    public static PQLevel3Quote ToL3PQQuote(this Level3PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel3Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel3Quote(vanillaQuote);
    }

    public static Level0PriceQuote ToL0PriceQuote(this Level0PriceQuote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level0PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level0PriceQuote(pqQuote);
    }

    public static Level1PriceQuote ToL1PriceQuote(this PQLevel1Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level1PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level1PriceQuote(pqQuote);
    }

    public static Level2PriceQuote ToL2PriceQuote(this PQLevel2Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level2PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level2PriceQuote(pqQuote);
    }

    public static Level3PriceQuote ToL3PriceQuote(this PQLevel3Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level3PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level3PriceQuote(pqQuote);
    }
}
