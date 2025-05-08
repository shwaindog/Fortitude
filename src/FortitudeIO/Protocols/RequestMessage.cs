// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

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

    public int RequestId { get; set; }


    public override IVersionedMessage CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);
        if (source is IRequestMessage requestMessage) RequestId = requestMessage.RequestId;
        return this;
    }

    public int NewRequestId()
    {
        return RequestId = Interlocked.Increment(ref lastRequestId);
    }
}
