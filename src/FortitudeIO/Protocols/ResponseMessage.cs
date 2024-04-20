#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols;

public interface IResponseMessage : IVersionedMessage
{
    int RequestId { get; set; }
    int ResponseId { get; set; }
}

public abstract class ResponseMessage : VersionedMessage, IResponseMessage
{
    private static int lastResponseId;
    protected ResponseMessage() { }

    protected ResponseMessage(IResponseMessage toClone) : base(toClone)
    {
        RequestId = toClone.RequestId;
        ResponseId = toClone.ResponseId;
    }

    protected ResponseMessage(byte version) => Version = version;

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);
        if (source is IResponseMessage responseMessage)
        {
            RequestId = responseMessage.RequestId;
            ResponseId = responseMessage.ResponseId;
        }

        return this;
    }

    public int RequestId { get; set; }

    public int ResponseId { get; set; }

    public int NewResponseId()
    {
        return ResponseId = Interlocked.Increment(ref lastResponseId);
    }
}
