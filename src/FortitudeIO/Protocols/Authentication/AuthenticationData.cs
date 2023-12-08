#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IAuthenticationData : IReusableObject<IAuthenticationData>
{
    AuthenticationType AuthenticationType { get; set; }
    IList<byte>? AuthenticationBytes { get; set; }
}

public class AuthenticationData : ReusableObject<IAuthenticationData>, IAuthenticationData
{
    public AuthenticationData() { }

    public AuthenticationData(IAuthenticationData toClone)
    {
        AuthenticationType = toClone.AuthenticationType;
        AuthenticationBytes = toClone.AuthenticationBytes;
    }

    public AuthenticationData(AuthenticationType authenticationType, byte[]? authenticationBytes)
    {
        AuthenticationType = authenticationType;
        AuthenticationBytes = authenticationBytes;
    }

    public AuthenticationType AuthenticationType { get; set; }
    public IList<byte>? AuthenticationBytes { get; set; }

    public override IAuthenticationData CopyFrom(IAuthenticationData source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        AuthenticationType = source.AuthenticationType;
        if (source.AuthenticationBytes == null) return this;
        var authBytes = Recycler?.Borrow<List<byte>>() ?? new List<byte>();
        authBytes.Clear();
        for (var i = 0; i < source.AuthenticationBytes.Count; i++) authBytes.Add(source.AuthenticationBytes[i]);
        return this;
    }

    public override void Reset()
    {
        AuthenticationType = default;
        if (AuthenticationBytes != null)
        {
            AuthenticationBytes.Clear();
            Recycler?.Recycle(AuthenticationBytes);
        }

        AuthenticationBytes = null;
        base.Reset();
    }

    public override IAuthenticationData Clone() =>
        Recycler?.Borrow<AuthenticationData>().CopyFrom(this) ?? new AuthenticationData(this);

    [Obsolete]
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
