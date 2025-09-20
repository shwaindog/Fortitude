#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public abstract class OrxAuthenticatedMessage : OrxVersionedMessage, IAuthenticatedMessage
{
    protected OrxAuthenticatedMessage() { }

    protected OrxAuthenticatedMessage(IAuthenticatedMessage toClone) : base(toClone)
    {
        SenderName = toClone.SenderName != null ? new MutableString(toClone.SenderName) : null;
        SendTime = toClone.SendTime;
        Info = toClone.Info != null ? new MutableString(toClone.Info) : null;
        UserData = toClone.UserData != null ? new OrxUserData(toClone.UserData) : null;
    }

    protected OrxAuthenticatedMessage(byte versionNumber, DateTime sendTime,
        MutableString senderName, MutableString info, OrxUserData userData) : base(versionNumber)
    {
        SenderName = senderName;
        SendTime = sendTime;
        Info = info;
        UserData = userData;
    }

    [OrxOptionalField(2)] public MutableString? SenderName { get; set; }

    [OrxOptionalField(3)] public MutableString? Info { get; set; }

    [OrxOptionalField(4)] public OrxUserData? UserData { get; set; }

    [OrxMandatoryField(1)] public DateTime SendTime { get; set; } = DateTimeConstants.UnixEpoch;

    IMutableString? IAuthenticatedMessage.SenderName
    {
        get => SenderName;
        set => SenderName = value as MutableString;
    }

    IMutableString? IAuthenticatedMessage.Info
    {
        get => Info;
        set => Info = value as MutableString;
    }

    IUserData? IAuthenticatedMessage.UserData
    {
        get => UserData;
        set => UserData = value as OrxUserData;
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage versionedMessage
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IAuthenticatedMessage authMessage)
        {
            SenderName = authMessage.SenderName?.CopyOrClone(SenderName);
            SendTime = authMessage.SendTime;
            Info = authMessage.Info?.CopyOrClone(Info);
            UserData = authMessage.UserData?.CopyOrClone(UserData);
        }

        return this;
    }

    protected bool Equals(IAuthenticatedMessage other) =>
        base.Equals(other) && SendTime.Equals(other.SendTime) &&
        Equals(SenderName, other.SenderName) && Equals(Info, other.Info) &&
        Equals(UserData, other.UserData);

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
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ SendTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (SenderName != null ? SenderName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Info != null ? Info.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (UserData != null ? UserData.GetHashCode() : 0);
            return hashCode;
        }
    }
}
