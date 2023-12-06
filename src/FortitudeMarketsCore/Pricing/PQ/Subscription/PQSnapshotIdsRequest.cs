#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQSnapshotIdsRequest : ReusableObject<IVersionedMessage>, IPQSnapshotIdsRequest
{
    public PQSnapshotIdsRequest() => RequestSourceTickerIds = Array.Empty<uint>();

    public PQSnapshotIdsRequest(IList<uint> requestSourceTickerIds) =>
        RequestSourceTickerIds = requestSourceTickerIds.ToArray();

    public PQSnapshotIdsRequest(uint[] requestSourceTickerIds) => RequestSourceTickerIds = requestSourceTickerIds;

    private PQSnapshotIdsRequest(PQSnapshotIdsRequest toClone)
    {
        RequestSourceTickerIds = Array.Empty<uint>();
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public uint MessageId => 0;

    public byte Version => 1;

    public uint[] RequestSourceTickerIds { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQSnapshotIdsRequest pqSnapshotIdsRequest)
        {
            if (pqSnapshotIdsRequest.RequestSourceTickerIds.Any())
            {
                RequestSourceTickerIds = new uint[pqSnapshotIdsRequest.RequestSourceTickerIds.Length];
                Array.Copy(pqSnapshotIdsRequest.RequestSourceTickerIds, 0, RequestSourceTickerIds, 0
                    , pqSnapshotIdsRequest.RequestSourceTickerIds.Length);
            }
            else
            {
                RequestSourceTickerIds = Array.Empty<uint>();
            }
        }
        else
        {
            RequestSourceTickerIds = Array.Empty<uint>();
        }

        return this;
    }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<PQSnapshotIdsRequest>().CopyFrom(this) ?? new PQSnapshotIdsRequest(this);

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
}
