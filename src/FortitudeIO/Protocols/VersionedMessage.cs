using FortitudeCommon.DataStructures.Memory;

namespace FortitudeIO.Protocols
{
    public abstract class VersionedMessage : IVersionedMessage
    {
        protected VersionedMessage()
        {
        }

        protected VersionedMessage(IVersionedMessage toClone)
        {
            Version = toClone.Version;
        }

        protected VersionedMessage(byte version)
        {
            Version = version;
        }

        public abstract uint MessageId { get; }
        public byte Version { get; set; }

        public virtual void CopyFrom(IVersionedMessage source, IRecycler recyclerFactory)
        {
            Version = source.Version;
        }

        protected bool Equals(IVersionedMessage other)
        {
            return Version == other.Version && MessageId == other.MessageId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IVersionedMessage) obj);
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }
    }
}