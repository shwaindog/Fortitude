#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public class OrxUserData : ReusableObject<IUserData>, IUserData
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

    public override IUserData Clone() => Recycler?.Borrow<OrxUserData>().CopyFrom(this) ?? new OrxUserData(this);

    public override void StateReset()
    {
        UserId?.DecrementRefCount();
        UserId = null;
        AuthData?.DecrementRefCount();
        AuthData = null;
        base.StateReset();
    }

    public override IUserData CopyFrom(IUserData userData, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        UserId = userData.UserId?.CopyOrClone(UserId);
        AuthData = userData.AuthData?.CopyOrClone(AuthData);
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
