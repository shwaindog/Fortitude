#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public abstract class OrxAuthenticatedMessage : OrxVersionedMessage, IAuthenticatedMessage
{
    private int refCount;
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

    public override void CopyFrom(IVersionedMessage versionedMessage
        , CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IAuthenticatedMessage authMessage)
        {
            SenderName = authMessage.SenderName != null ?
                Recycler!.Borrow<MutableString>().Clear().Append(authMessage.SenderName) :
                null;
            SendTime = authMessage.SendTime;
            Info = authMessage.Info != null ?
                Recycler!.Borrow<MutableString>().Clear().Append(authMessage.Info) :
                null;
            if (authMessage.UserData != null)
            {
                var orxUserData = Recycler!.Borrow<OrxUserData>();
                orxUserData.CopyFrom(authMessage.UserData, Recycler);
                UserData = orxUserData;
            }
        }
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((IVersionedMessage)source, copyMergeFlags);
    }

    public virtual void Configure(byte version, DateTime sendTime,
        MutableString? senderName, MutableString? info, OrxUserData? userData, IRecycler recyclerFactory)
    {
        base.Configure(version);
        SendTime = sendTime;
        SenderName = senderName != null ? recyclerFactory.Borrow<MutableString>().Clear().Append(senderName) : null;
        Info = info != null ? recyclerFactory.Borrow<MutableString>().Clear().Append(info) : null;
        UserData = userData != null ? recyclerFactory.Borrow<OrxUserData>().Configure(userData, recyclerFactory) : null;
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
