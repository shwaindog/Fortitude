using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX.Authentication
{
    public abstract class OrxAuthenticatedMessage : OrxVersionedMessage, IAuthenticatedMessage
    {
        protected OrxAuthenticatedMessage()
        {
        }

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

        [OrxMandatoryField(1)]
        public DateTime SendTime { get; set; } = DateTimeConstants.UnixEpoch;

        [OrxOptionalField(2)]
        public MutableString SenderName { get; set; }

        IMutableString IAuthenticatedMessage.SenderName
        {
            get => SenderName;
            set => SenderName = (MutableString)value;
        }

        [OrxOptionalField(3)]
        public MutableString Info { get; set; }

        IMutableString IAuthenticatedMessage.Info
        {
            get => Info;
            set => Info = (MutableString)value;
        }

        [OrxOptionalField(4)] public OrxUserData UserData { get; set; }

        IUserData IAuthenticatedMessage.UserData
        {
            get => UserData;
            set => UserData = (OrxUserData) value;
        }

        public bool ShouldAutoRecycle { get; set; } = true;

        public IRecycler Recycler { get; set; }

        public virtual void Configure(byte version, DateTime sendTime,
            MutableString senderName, MutableString info, OrxUserData userData, IRecycler recyclerFactory)
        {
            base.Configure(version);
            SendTime = sendTime;
            SenderName = senderName != null 
                ? recyclerFactory.Borrow<MutableString>().Clear().Append(senderName) 
                : null;
            Info = info != null ? recyclerFactory.Borrow<MutableString>().Clear().Append(info) : null;
            UserData = userData != null
                ? recyclerFactory.Borrow<OrxUserData>().Configure(userData, recyclerFactory)
                : null;
        }

        public override void CopyFrom(IVersionedMessage versionedMessage, IRecycler orxRecyclingFactory)
        {
            base.CopyFrom(versionedMessage, orxRecyclingFactory);
            if (versionedMessage is IAuthenticatedMessage authMessage)
            {
                SenderName = authMessage.SenderName != null
                    ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(authMessage.SenderName)
                    : null;
                SendTime = authMessage.SendTime;
                Info = authMessage.Info != null
                    ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(authMessage.Info)
                    : null;
                if (authMessage.UserData != null)
                {
                    var orxUserData = orxRecyclingFactory.Borrow<OrxUserData>();
                    orxUserData.CopyFrom(authMessage.UserData, orxRecyclingFactory);
                    UserData = orxUserData;
                }
            }
        }

        protected bool Equals(IAuthenticatedMessage other)
        {
            return base.Equals(other) && SendTime.Equals(other.SendTime) && 
                   Equals(SenderName, other.SenderName) && Equals(Info, other.Info) && 
                   Equals(UserData, other.UserData);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IAuthenticatedMessage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ SendTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (SenderName != null ? SenderName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Info != null ? Info.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserData != null ? UserData.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}