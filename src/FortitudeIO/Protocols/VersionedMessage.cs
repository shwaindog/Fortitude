#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols;

public abstract class VersionedMessage : IVersionedMessage, IStoreState<IVersionedMessage>
{
    protected VersionedMessage() { }

    protected VersionedMessage(IVersionedMessage toClone) => Version = toClone.Version;

    protected VersionedMessage(byte version) => Version = version;

    public virtual void CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags)
    {
        Version = source.Version;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((IVersionedMessage)source, copyMergeFlags);
    }

    public abstract uint MessageId { get; }
    public byte Version { get; set; }

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
