using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Counterparties
{
    public interface IParty
    {
        IMutableString PartyId { get; set; }
        IMutableString Name { get; set; }
        IParty ParentParty { get; set; }
        IMutableString ClientPartyId { get; set; }
        IBookingInfo Portfolio { get; set; }
        IParty Clone();
    }
}