// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages;

public class PQSourceTickerInfoResponse : ResponseMessage
{
    public PQSourceTickerInfoResponse() => Version = 1;

    public PQSourceTickerInfoResponse(IEnumerable<ISourceTickerInfo> requestSourceTickerIds) : this() =>
        SourceTickerInfos.AddRange(requestSourceTickerIds);

    private PQSourceTickerInfoResponse(PQSourceTickerInfoResponse toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)PQMessageIds.SourceTickerInfoResponse;

    public List<ISourceTickerInfo> SourceTickerInfos { get; } = new();

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is PQSourceTickerInfoResponse pqSourceTickerInfoResponse)
        {
            SourceTickerInfos.Clear();
            SourceTickerInfos.AddRange(pqSourceTickerInfoResponse.SourceTickerInfos);
        }

        return this;
    }

    public override void StateReset()
    {
        SourceTickerInfos.Clear();
        base.StateReset();
    }


    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<PQSourceTickerInfoResponse>().CopyFrom(this) ?? new PQSourceTickerInfoResponse(this);

    protected bool Equals(PQSourceTickerInfoResponse other)
    {
        var arraysSame = SourceTickerInfos.SequenceEqual(other.SourceTickerInfos);
        return arraysSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IPQSnapshotIdsRequest)obj);
    }

    public override int GetHashCode() => SourceTickerInfos.GetHashCode();

    public override string ToString() =>
        $"{nameof(PQSourceTickerInfoResponse)} ({MembersToString}, {nameof(SourceTickerInfos)}: [{SourceTickerInfos.JoinToString()}])";
}
