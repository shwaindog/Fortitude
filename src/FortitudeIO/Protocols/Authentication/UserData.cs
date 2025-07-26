#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IUserData : IReusableObject<IUserData>
{
    IAuthenticationData? AuthData { get; set; }
    IMutableString? UserId { get; set; }
}

public class UserData : ReusableObject<IUserData>, IUserData
{
    public UserData() { }

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

    public override IUserData CopyFrom(IUserData source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        AuthData = source.AuthData?.CopyOrClone(AuthData as AuthenticationData);
        UserId = source.UserId?.CopyOrClone(UserId as MutableString);
        return this;
    }

    public override void StateReset()
    {
        AuthData?.DecrementRefCount();
        AuthData = null;
        UserId?.DecrementRefCount();
        UserId = null;
        base.StateReset();
    }

    public override IUserData Clone() => Recycler?.Borrow<UserData>().CopyFrom(this) ?? new UserData(this);

    [Obsolete]
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
