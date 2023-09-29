using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX.Authentication
{
    public sealed class OrxLogonRequest : OrxVersionedMessage
    {
        public override uint MessageId => (uint)AuthenticationMessages.LogonRequest;
        [OrxMandatoryField(10)]
        public MutableString Login { get; set; }
        [OrxMandatoryField(11)]
        public MutableString Password { get; set; }
        [OrxMandatoryField(12)]
        public MutableString Account { get; set; }
        [OrxMandatoryField(13)]
        public MutableString ClientVersion { get; set; }
        [OrxMandatoryField(14)]
        public uint HbInterval { get; set; }

        public OrxLogonRequest(ILoginCredentials loginCredentials, MutableString account, MutableString clientVersion, uint hbinterval)
        {
            Login = loginCredentials.LoginId;
            Password = loginCredentials.Password;
            Account = account;
            ClientVersion = clientVersion;
            HbInterval = hbinterval;
        }

        public OrxLogonRequest() { }
    }
}
