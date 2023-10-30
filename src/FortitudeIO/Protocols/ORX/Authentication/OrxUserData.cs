#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public class OrxUserData : IUserData
{
    public OrxUserData() { }

    public OrxUserData(IUserData userData)
    {
        UserId = userData.UserId != null ? new MutableString(userData.UserId) : null;
        if (userData.AuthData != null) AuthData = new OrxAuthenticationData(userData.AuthData);
    }

    public OrxUserData(MutableString userId, OrxAuthenticationData? authData = null)
    {
        AuthData = authData;
        UserId = userId;
    }

    [OrxMandatoryField(0)] public MutableString? UserId { get; set; }

    [OrxOptionalField(1)] public OrxAuthenticationData? AuthData { get; set; }

    IMutableString? IUserData.UserId
    {
        get => UserId;
        set => UserId = value as MutableString;
    }

    IAuthenticationData? IUserData.AuthData
    {
        get => AuthData;
        set => AuthData = value as OrxAuthenticationData;
    }

    public IUserData Clone() => new OrxUserData(this);

    public void CopyFrom(IUserData userData, IRecycler orxRecyclingFactory)
    {
        UserId = userData.UserId != null ?
            orxRecyclingFactory.Borrow<MutableString>().Clear().Append(userData.UserId) :
            null;
        if (userData.AuthData != null)
        {
            var orxAuthData = orxRecyclingFactory.Borrow<OrxAuthenticationData>();
            orxAuthData.CopyFrom(userData.AuthData, orxRecyclingFactory);
            AuthData = orxAuthData;
        }
    }

    public OrxUserData Configure(IUserData userData, IRecycler recyclerFactory)
    {
        AuthData = userData.AuthData != null ?
            recyclerFactory.Borrow<OrxAuthenticationData>().Configure(userData.AuthData, recyclerFactory) :
            null;
        UserId = userData.UserId != null ?
            recyclerFactory.Borrow<MutableString>().Clear().Append(userData.UserId) :
            null;
        return this;
    }

    protected bool Equals(OrxUserData other)
    {
        var userIdSame = Equals(UserId, other.UserId);
        var authDataSame = Equals(AuthData, other.AuthData);
        return userIdSame && authDataSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxUserData)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((UserId?.GetHashCode() ?? 0) * 397) ^
                   (AuthData?.GetHashCode() ?? 0);
        }
    }
}
