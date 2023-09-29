using FortitudeMarketsApi.Trading.Counterparties;

namespace FortitudeMarketsCore.Trading.Counterparties
{
    public class Parties : IParties
    {
        public Parties(IParties toClone)
        {
            BuySide = toClone.BuySide?.Clone();
            SellSide = toClone.SellSide?.Clone();
        }

        public Parties(IParty buySide, IParty sellSide)
        {
            BuySide = buySide;
            SellSide = sellSide;
        }

        public IParty BuySide { get; set; }
        public IParty SellSide { get; set; }
        public IParties Clone()
        {
            return new Parties(this);
        }
    }
}