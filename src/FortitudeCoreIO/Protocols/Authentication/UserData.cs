#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IUserData
{
    IAuthenticationData? AuthData { get; set; }
    IMutableString? UserId { get; set; }
    IUserData Clone();
}

public class UserData : IUserData
{
    public UserData(IMutableString userId, IAuthenticationData? authData = null)
    {
        AuthData = authData;
        UserId = userId;
    }

    public UserData(IUserData toClone)
    {
        AuthData = toClone.AuthData?.Clone();
        UserId = toClone.UserId?.Clone();
    }

    public IAuthenticationData? AuthData { get; set; }
    public IMutableString? UserId { get; set; }

    public IUserData Clone() => new UserData(this);

    public IUserData Configure(IUserData userData, IRecycler recyclerFactory)
    {
        AuthData = userData.AuthData != null ?
            recyclerFactory.Borrow<AuthenticationData>().Configure(userData.AuthData, recyclerFactory) :
            null;
        UserId = userData.UserId != null ?
            recyclerFactory.Borrow<MutableString>().Clear().Append(userData.UserId) :
            null;
        return this;
    }
}
