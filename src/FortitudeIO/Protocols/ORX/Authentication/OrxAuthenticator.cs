#region

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public delegate bool OrxAuthenticator(
    ISocketSessionContext clientSessionConnection, MutableString? usr, MutableString? pwd,
    out IUserData? authData, out MutableString? message);
