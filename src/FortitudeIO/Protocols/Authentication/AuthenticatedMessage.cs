#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IAuthenticatedMessage : IVersionedMessage
{
    IMutableString? SenderName { get; set; }
    DateTime SendTime { get; set; }
    IMutableString? Info { get; set; }
    IUserData? UserData { get; set; }
}

public abstract class AuthenticatedMessage : VersionedMessage, IAuthenticatedMessage
{
    protected AuthenticatedMessage() { }

    protected AuthenticatedMessage(IAuthenticatedMessage toClone) : base(toClone)
    {
        SenderName = toClone.SenderName != null ? new MutableString(toClone.SenderName) : null;
        SendTime = toClone.SendTime;
        UserData = toClone.UserData != null ? new UserData(toClone.UserData) : null;
        Info = toClone.Info != null ? new MutableString(toClone.Info) : null;
    }

    protected AuthenticatedMessage(byte version, IMutableString? senderName = null, DateTime? sendTime = null
        , IUserData? userData = null, IMutableString? info = null)
        : base(version)
    {
        SenderName = senderName;
        SendTime = sendTime ?? DateTimeConstants.UnixEpoch;
        UserData = userData;
        Info = info;
    }

    public IMutableString? SenderName { get; set; }
    public DateTime SendTime { get; set; }
    public IUserData? UserData { get; set; }
    public IMutableString? Info { get; set; }

    public override void Reset()
    {
        SenderName?.DecrementRefCount();
        SenderName = null;
        UserData?.DecrementRefCount();
        UserData = null;
        Info?.DecrementRefCount();
        Info = null;
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IAuthenticatedMessage authenticatedMessage)
        {
            SenderName = authenticatedMessage.SenderName?.SyncOrRecycle(SenderName as MutableString);
            SendTime = authenticatedMessage.SendTime;
            UserData = authenticatedMessage.UserData?.CopyOrClone(UserData as UserData);
            Info = authenticatedMessage.Info?.CopyOrClone(Info as MutableString);
        }

        return this;
    }

    public abstract override IAuthenticatedMessage Clone();

    protected bool Equals(IAuthenticatedMessage other) =>
        base.Equals(other) && Equals(SenderName, other.SenderName) &&
        SendTime.Equals(other.SendTime) && Equals(UserData, other.UserData) && Equals(Info, other.Info);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IAuthenticatedMessage)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SenderName != null ? SenderName.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ SendTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (Info != null ? Info.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (UserData != null ? UserData.GetHashCode() : 0);
            return hashCode;
        }
    }
}
