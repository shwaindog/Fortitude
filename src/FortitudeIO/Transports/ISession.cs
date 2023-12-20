#region

using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeIO.Transports;

public interface ISession
{
    IUserData? AuthData { get; set; }
    bool Active { get; set; }
    string? SessionDescription { get; }
}
