using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX.Authentication
{
    public sealed class OrxLoggedOutMessage : OrxVersionedMessage
    {
        public override uint MessageId => (uint)AuthenticationMessages.LoggedOutResponse;

        [OrxMandatoryField(10)]
        public MutableString Reason { get; set; }

        public OrxLoggedOutMessage() { }

        public OrxLoggedOutMessage(MutableString reason)
        {
            Reason = reason;
        }

        public void Configure(byte version, MutableString reason)
        { 
            Configure(version);
            Reason = reason;
        }
    }
}
