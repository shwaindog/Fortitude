#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages;

public interface IPQSnapshotIdsRequest : IVersionedMessage
{
    List<uint> RequestSourceTickerIds { get; }
}

public class PQSnapshotIdsRequest : VersionedMessage, IPQSnapshotIdsRequest // doesn't need to be a RequestMessage as the response is QuoteSnapshots
{
    public PQSnapshotIdsRequest() => Version = 1;

    public PQSnapshotIdsRequest(IList<uint> requestSourceTickerIds) : this() => RequestSourceTickerIds.AddRange(requestSourceTickerIds);

    public PQSnapshotIdsRequest(uint[] requestSourceTickerIds) : this() => RequestSourceTickerIds.AddRange(requestSourceTickerIds);

    private PQSnapshotIdsRequest(PQSnapshotIdsRequest toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)PQMessageIds.SnapshotIdsRequest;

    public List<uint> RequestSourceTickerIds { get; } = new();

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQSnapshotIdsRequest pqSnapshotIdsRequest)
        {
            RequestSourceTickerIds.Clear();
            RequestSourceTickerIds.AddRange(pqSnapshotIdsRequest.RequestSourceTickerIds);
        }

        return this;
    }

    public override IVersionedMessage Clone() => Recycler?.Borrow<PQSnapshotIdsRequest>().CopyFrom(this) ?? new PQSnapshotIdsRequest(this);

    protected bool Equals(IPQSnapshotIdsRequest other)
    {
        var arraysSame = RequestSourceTickerIds.SequenceEqual(other.RequestSourceTickerIds);
        return arraysSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IPQSnapshotIdsRequest)obj);
    }

    public override int GetHashCode() => RequestSourceTickerIds != null ? RequestSourceTickerIds.GetHashCode() : 0;

    public override string ToString() =>
        $"{nameof(PQSnapshotIdsRequest)}({nameof(RequestSourceTickerIds)}: [{RequestSourceTickerIds.JoinToString()}])";
}
