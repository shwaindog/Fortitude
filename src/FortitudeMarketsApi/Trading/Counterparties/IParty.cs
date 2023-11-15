#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Counterparties;

public interface IParty : IRecyclableObject<IParty>
{
    IMutableString PartyId { get; set; }
    IMutableString Name { get; set; }
    IParty? ParentParty { get; set; }
    IMutableString ClientPartyId { get; set; }
    IBookingInfo Portfolio { get; set; }
    IParty Clone();
}
