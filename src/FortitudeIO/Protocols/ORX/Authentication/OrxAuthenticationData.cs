// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public class OrxAuthenticationData : ReusableObject<IAuthenticationData>, IAuthenticationData
{
    public OrxAuthenticationData() { }

    public OrxAuthenticationData(IAuthenticationData authData)
    {
        AuthenticationType = authData.AuthenticationType;
        AuthenticationBytes
            = authData.AuthenticationBytes != null ? new List<byte>(authData.AuthenticationBytes) : null;
    }

    [OrxOptionalField(1)] public List<byte>? AuthenticationBytes { get; set; }

    [OrxMandatoryField(0)] public AuthenticationType AuthenticationType { get; set; }

    IList<byte>? IAuthenticationData.AuthenticationBytes
    {
        get => AuthenticationBytes;
        set => AuthenticationBytes = (List<byte>)value!;
    }

    public override IAuthenticationData Clone() => Recycler?.Borrow<OrxAuthenticationData>().CopyFrom(this) ?? new OrxAuthenticationData(this);


    public override void StateReset()
    {
        if (AuthenticationBytes != null)
        {
            AuthenticationBytes.Clear();
            Recycler?.Recycle(AuthenticationBytes);
        }

        AuthenticationBytes = null;
        AuthenticationType  = AuthenticationType.None;
        base.StateReset();
    }

    public override IAuthenticationData CopyFrom
    (IAuthenticationData authData
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        AuthenticationType = authData.AuthenticationType;
        if (authData.AuthenticationBytes != null)
        {
            var orxAuthData = Recycler?.Borrow<List<byte>>() ?? new List<byte>();
            orxAuthData.Clear();
            orxAuthData.AddRange(authData.AuthenticationBytes);
            AuthenticationBytes = orxAuthData;
        }

        return this;
    }

    protected bool Equals(OrxAuthenticationData other)
    {
        var authTypeSame   = AuthenticationType == other.AuthenticationType;
        var tokenBytesSame = Equals(AuthenticationBytes, other.AuthenticationBytes);
        return authTypeSame && tokenBytesSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxAuthenticationData)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)AuthenticationType * 397) ^
                   (AuthenticationBytes != null ? AuthenticationBytes.GetHashCode() : 0);
        }
    }
}
