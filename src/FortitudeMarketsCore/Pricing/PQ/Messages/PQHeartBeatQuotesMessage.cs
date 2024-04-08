#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public class PQHeartBeatQuotesMessage : ReusableObject<IVersionedMessage>, IPQHeartBeatQuotesMessage
    , IEnumerable<IPQLevel0Quote>
{
    public PQHeartBeatQuotesMessage() => QuotesToSendHeartBeats = new List<IPQLevel0Quote>();

    public PQHeartBeatQuotesMessage(IList<IPQLevel0Quote> quotesToSendHeartBeats) => QuotesToSendHeartBeats = quotesToSendHeartBeats;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQLevel0Quote> GetEnumerator() => QuotesToSendHeartBeats.GetEnumerator();

    public uint MessageId => (uint)PQMessageIds.HeartBeat;

    public byte Version => 1;
    public IList<IPQLevel0Quote> QuotesToSendHeartBeats { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQHeartBeatQuotesMessage heartBeatQuotesMessage)
        {
            QuotesToSendHeartBeats.Clear();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < heartBeatQuotesMessage.QuotesToSendHeartBeats.Count; i++)
                QuotesToSendHeartBeats.Add(heartBeatQuotesMessage.QuotesToSendHeartBeats[i]);
        }

        return this;
    }

    public override IVersionedMessage Clone() => throw new NotImplementedException();

    protected bool Equals(PQHeartBeatQuotesMessage other) => Equals(QuotesToSendHeartBeats, other.QuotesToSendHeartBeats);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PQHeartBeatQuotesMessage)obj);
    }

    public override int GetHashCode() => QuotesToSendHeartBeats != null ? QuotesToSendHeartBeats.GetHashCode() : 0;
}
