#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols;

public interface IRequestMessage : IVersionedMessage
{
    int RequestId { get; set; }
}

public abstract class RequestMessage : VersionedMessage, IRequestMessage
{
    private static int lastRequestId;
    protected RequestMessage() { }

    protected RequestMessage(IRequestMessage toClone) : base(toClone) => RequestId = toClone.RequestId;

    protected RequestMessage(byte version) => Version = version;


    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);
        if (source is IRequestMessage requestMessage) RequestId = requestMessage.RequestId;
        return this;
    }

    public int RequestId { get; set; }

    public int NewRequestId()
    {
        return RequestId = Interlocked.Increment(ref lastRequestId);
    }
}
