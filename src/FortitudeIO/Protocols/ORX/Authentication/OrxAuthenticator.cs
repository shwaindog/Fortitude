﻿#region

using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public delegate bool OrxAuthenticator(
    ISessionConnection clientSessionConnection, MutableString? usr, MutableString? pwd,
    out IUserData? authData, out MutableString? message);
