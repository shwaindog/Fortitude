#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public interface IParty : IReusableObject<IParty>
{
    IMutableString PartyId { get; set; }
    IMutableString Name { get; set; }
    IParty? ParentParty { get; set; }
    IMutableString ClientPartyId { get; set; }
    IBookingInfo Portfolio { get; set; }
}
