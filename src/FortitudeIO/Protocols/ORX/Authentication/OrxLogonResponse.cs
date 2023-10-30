
namespace FortitudeIO.Protocols.ORX.Authentication
{
    public sealed class OrxLogonResponse : OrxVersionedMessage
    {
        public override uint MessageId => (uint)AuthenticationMessages.LoggedInResponse;
    }
}
