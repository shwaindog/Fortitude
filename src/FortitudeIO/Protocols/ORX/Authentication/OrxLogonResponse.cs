#region

using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public sealed class OrxLogonResponse : OrxVersionedMessage
{
    public OrxLogonResponse() { }

    private OrxLogonResponse(OrxLogonResponse toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)AuthenticationMessages.LoggedInResponse;

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<OrxLogonResponse>().CopyFrom(this) ?? new OrxLogonResponse(this);
}
