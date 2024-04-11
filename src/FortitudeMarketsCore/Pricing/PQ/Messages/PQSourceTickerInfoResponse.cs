#region

using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public class PQSourceTickerInfoResponse : VersionedMessage
{
    public PQSourceTickerInfoResponse() => Version = 1;

    public PQSourceTickerInfoResponse(IEnumerable<ISourceTickerQuoteInfo> requestSourceTickerIds) : this() =>
        SourceTickerQuoteInfos.AddRange(requestSourceTickerIds);

    private PQSourceTickerInfoResponse(PQSourceTickerInfoResponse toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)PQMessageIds.SourceTickerInfoResponse;

    public List<ISourceTickerQuoteInfo> SourceTickerQuoteInfos { get; } = new();

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is PQSourceTickerInfoResponse pqSourceTickerInfoResponse)
        {
            SourceTickerQuoteInfos.Clear();
            SourceTickerQuoteInfos.AddRange(pqSourceTickerInfoResponse.SourceTickerQuoteInfos);
        }

        return this;
    }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<PQSourceTickerInfoResponse>().CopyFrom(this) ?? new PQSourceTickerInfoResponse(this);

    protected bool Equals(PQSourceTickerInfoResponse other)
    {
        var arraysSame = SourceTickerQuoteInfos.SequenceEqual(other.SourceTickerQuoteInfos);
        return arraysSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IPQSnapshotIdsRequest)obj);
    }

    public override int GetHashCode() => SourceTickerQuoteInfos.GetHashCode();
}
