using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX
{
    public abstract class OrxVersionedMessage : IVersionedMessage
    {
        protected OrxVersionedMessage()
        {
        }

        protected OrxVersionedMessage(IVersionedMessage toClone)
        {
            Version = toClone.Version;
        }
        
        protected OrxVersionedMessage(byte versionNumber)
        {
            Version = versionNumber;
        }

        public abstract uint MessageId { get; }

        [OrxMandatoryField(0)]
        public byte Version { get; set; }

        public virtual void CopyFrom(IVersionedMessage tradingMessage, IRecycler orxRecyclingFactory)
        {
            Version = tradingMessage.Version;
        }

        public virtual void Configure(byte version)
        {
            Version = version;
        }

        protected bool Equals(IVersionedMessage other)
        {
            return Version == other.Version && MessageId == other.MessageId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IVersionedMessage)obj);
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }
    }
}
