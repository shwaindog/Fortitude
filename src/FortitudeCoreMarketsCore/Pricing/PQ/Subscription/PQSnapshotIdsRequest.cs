namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQSnapshotIdsRequest : IPQSnapshotIdsRequest
{
    public PQSnapshotIdsRequest(IList<uint> requestSourceTickerIds) =>
        RequestSourceTickerIds = requestSourceTickerIds.ToArray();

    public PQSnapshotIdsRequest(uint[] requestSourceTickerIds) => RequestSourceTickerIds = requestSourceTickerIds;

    public uint MessageId => 0;

    public byte Version => 1;

    public uint[] RequestSourceTickerIds { get; set; }

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
