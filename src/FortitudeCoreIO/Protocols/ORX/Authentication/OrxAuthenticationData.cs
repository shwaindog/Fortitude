#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public class OrxAuthenticationData : IAuthenticationData
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

    public IAuthenticationData Clone() => new OrxAuthenticationData(this);

    public void CopyFrom(IAuthenticationData authData, IRecycler orxRecyclingFactory)
    {
        AuthenticationType = authData.AuthenticationType;
        if (authData.AuthenticationBytes != null)
        {
            var orxAuthData = orxRecyclingFactory.Borrow<List<byte>>();
            orxAuthData.Clear();
            orxAuthData.AddRange(authData.AuthenticationBytes);
            AuthenticationBytes = orxAuthData;
        }
    }

    public OrxAuthenticationData Configure(IAuthenticationData authData, IRecycler recyclerFactory)
    {
        AuthenticationType = authData.AuthenticationType;
        if (authData.AuthenticationBytes == null) return this;
        var authBytes = recyclerFactory.Borrow<List<byte>>();
        authBytes.Clear();
        for (var i = 0; i < authData.AuthenticationBytes.Count; i++) authBytes.Add(authData.AuthenticationBytes[i]);
        return this;
    }

    protected bool Equals(OrxAuthenticationData other)
    {
        var authTypeSame = AuthenticationType == other.AuthenticationType;
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
