#region

using FortitudeCommon.Types;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public class PQSourceTickerInfoRequest : RequestMessage
{
    public PQSourceTickerInfoRequest() => Version = 1;

    private PQSourceTickerInfoRequest(PQSourceTickerInfoRequest toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)PQMessageIds.SourceTickerInfoRequest;

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        return this;
    }

    public override IVersionedMessage Clone() => Recycler?.Borrow<PQSourceTickerInfoRequest>().CopyFrom(this) ?? new PQSourceTickerInfoRequest(this);

    protected bool Equals(IPQSnapshotIdsRequest other) => true;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IPQSnapshotIdsRequest)obj);
    }

    public override int GetHashCode() => 0;
}
