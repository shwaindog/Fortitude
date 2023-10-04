#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IAuthenticationData
{
    AuthenticationType AuthenticationType { get; set; }
    IList<byte>? AuthenticationBytes { get; set; }
    IAuthenticationData Clone();
}

public class AuthenticationData : IAuthenticationData
{
    public AuthenticationData(IAuthenticationData toClone)
    {
        AuthenticationType = toClone.AuthenticationType;
        AuthenticationBytes = toClone.AuthenticationBytes;
    }

    public AuthenticationData(AuthenticationType authenticationType, byte[] authenticationBytes)
    {
        AuthenticationType = authenticationType;
        AuthenticationBytes = authenticationBytes;
    }

    public AuthenticationType AuthenticationType { get; set; }
    public IList<byte>? AuthenticationBytes { get; set; }

    public IAuthenticationData Clone() => new AuthenticationData(this);

    public IAuthenticationData Configure(IAuthenticationData authData, IRecycler recyclerFactory)
    {
        AuthenticationType = authData.AuthenticationType;
        if (authData.AuthenticationBytes == null) return this;
        var authBytes = recyclerFactory.Borrow<List<byte>>();
        authBytes.Clear();
        for (var i = 0; i < authData.AuthenticationBytes.Count; i++) authBytes.Add(authData.AuthenticationBytes[i]);
        return this;
    }
}
