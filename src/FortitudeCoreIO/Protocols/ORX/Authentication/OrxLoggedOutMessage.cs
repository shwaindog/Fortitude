#region

using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public sealed class OrxLoggedOutMessage : OrxVersionedMessage
{
    public OrxLoggedOutMessage() { }

    public OrxLoggedOutMessage(MutableString reason) => Reason = reason;

    public override uint MessageId => (uint)AuthenticationMessages.LoggedOutResponse;

    [OrxMandatoryField(10)] public MutableString? Reason { get; set; }

    public void Configure(byte version, MutableString? reason)
    {
        Configure(version);
        Reason = reason;
    }
}
