#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public class OrxLoggedOutMessage : OrxVersionedMessage
{
    public OrxLoggedOutMessage() { }

    public OrxLoggedOutMessage(MutableString reason) => Reason = reason;

    private OrxLoggedOutMessage(OrxLoggedOutMessage toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)AuthenticationMessages.LoggedOutResponse;

    [OrxMandatoryField(10)] public MutableString? Reason { get; set; }

    public override void Reset()
    {
        Reason?.DecrementRefCount();
        Reason = null;
        base.Reset();
    }


    public override OrxLoggedOutMessage Clone() =>
        (OrxLoggedOutMessage?)Recycler?.Borrow<OrxLoggedOutMessage>().CopyFrom(this) ?? new OrxLoggedOutMessage(this);

    public void Configure(byte version, MutableString? reason)
    {
        Configure(version);
        Reason = reason;
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is OrxLoggedOutMessage loggedOutMessage) Reason = loggedOutMessage.Reason.SyncOrRecycle(Reason);
        return this;
    }
}
