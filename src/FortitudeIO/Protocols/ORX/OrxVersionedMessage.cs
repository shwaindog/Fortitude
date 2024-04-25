#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols.ORX;

public abstract class OrxVersionedMessage : ReusableObject<IVersionedMessage>, IVersionedMessage
{
    protected OrxVersionedMessage() { }

    protected OrxVersionedMessage(IVersionedMessage toClone) => Version = toClone.Version;

    protected OrxVersionedMessage(byte versionNumber) => Version = versionNumber;

    public abstract uint MessageId { get; }

    public byte Version { get; set; }

    public override void StateReset()
    {
        Version = 0;
        base.StateReset();
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage tradingMessage
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Version = tradingMessage.Version;
        return this;
    }

    public virtual void Configure(byte version)
    {
        Version = version;
    }

    protected bool Equals(IVersionedMessage other) => Version == other.Version && MessageId == other.MessageId;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IVersionedMessage)obj);
    }

    public override int GetHashCode() => Version.GetHashCode();
}
