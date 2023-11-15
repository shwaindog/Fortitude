#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeIO.Protocols.Authentication;

public interface IAuthenticatedMessage : IVersionedMessage, IRecyclableObject<IVersionedMessage>
{
    IMutableString? SenderName { get; set; }
    DateTime SendTime { get; set; }
    IMutableString? Info { get; set; }
    IUserData? UserData { get; set; }
}

public abstract class AuthenticatedMessage : VersionedMessage, IAuthenticatedMessage
{
    private int refCount;
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
    public int RefCount => refCount;
    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) == 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool RecycleOnRefCountZero { get; set; }
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }

    public bool Recycle()
    {
        if (!AutoRecycledByProducer && !IsInRecycler && (refCount == 0 || !RecycleOnRefCountZero))
            Recycler!.Recycle(this);
        return IsInRecycler;
    }

    public override void CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IAuthenticatedMessage authMessage)
        {
            SenderName = authMessage.SenderName != null ?
                Recycler!.Borrow<MutableString>().Clear().Append(authMessage.SenderName) :
                null;
            SendTime = authMessage.SendTime;
            UserData = authMessage.UserData != null ?
                Recycler!.Borrow<UserData>().Configure(authMessage.UserData, Recycler) :
                null;
            Info = authMessage.Info != null ?
                Recycler!.Borrow<MutableString>().Clear().Append(authMessage.Info) :
                null;
        }
    }

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
