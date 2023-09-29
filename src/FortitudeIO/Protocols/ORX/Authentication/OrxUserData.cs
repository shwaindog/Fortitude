using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX.Authentication
{
    public class OrxUserData : IUserData
    {
        public OrxUserData()
        {
        }
        
        public OrxUserData(IUserData userData)
        {
            UserId = userData.UserId != null ? new MutableString(userData.UserId) : null ;
            if(userData.AuthData != null)
            {
                AuthData = new OrxAuthenticationData(userData.AuthData);
            }
        }

        public OrxUserData(MutableString userId, OrxAuthenticationData authData = null)
        {
            AuthData = authData;
            UserId = userId;
        }

        [OrxMandatoryField(0)]
        public MutableString UserId { get; set; }

        IMutableString IUserData.UserId
        {
            get => UserId;
            set => UserId = (MutableString)value;
        }

        [OrxOptionalField(1)]
        public OrxAuthenticationData AuthData { get; set; }

        IAuthenticationData IUserData.AuthData
        {
            get => AuthData;
            set => AuthData = (OrxAuthenticationData)value;
        }

        public void CopyFrom(IUserData userData, IRecycler orxRecyclingFactory)
        {
            UserId = userData.UserId != null
                ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(userData.UserId) as MutableString
                : null;
            if (userData.AuthData != null)
            {
                var orxAuthData = orxRecyclingFactory.Borrow<OrxAuthenticationData>();
                orxAuthData.CopyFrom(userData.AuthData, orxRecyclingFactory);
                AuthData = orxAuthData;
            }
        }

        public OrxUserData Configure(IUserData userData, IRecycler recyclerFactory)
        {
            AuthData = userData.AuthData != null
                ? recyclerFactory.Borrow<OrxAuthenticationData>().Configure(userData.AuthData, recyclerFactory)
                : null;
            UserId = userData.UserId != null
                ? recyclerFactory.Borrow<MutableString>().Clear().Append(userData.UserId)
                : null;
            return this;
        }

        public IUserData Clone()
        {
            return new OrxUserData(this);
        }

        protected bool Equals(OrxUserData other)
        {
            var userIdSame = string.Equals(UserId, other.UserId);
            var authDataSame = Equals(AuthData, other.AuthData);
            return userIdSame && authDataSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrxUserData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserId != null ? UserId.GetHashCode() : 0) * 397) ^
                       (AuthData != null ? AuthData.GetHashCode() : 0);
            }
        }
    }
}
