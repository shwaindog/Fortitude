#region

using System.Collections;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public class PQHeartBeatQuotesMessage : IPQHeartBeatQuotesMessage, IEnumerable<IPQLevel0Quote>
{
    public PQHeartBeatQuotesMessage(IList<IPQLevel0Quote> quotesToSendHeartBeats) =>
        QuotesToSendHeartBeats = quotesToSendHeartBeats;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQLevel0Quote> GetEnumerator() => QuotesToSendHeartBeats.GetEnumerator();

    public uint MessageId => (uint)PricingMessageIds.HeartBeatMessage;

    public byte Version => 1;
    public IList<IPQLevel0Quote> QuotesToSendHeartBeats { get; set; }

    protected bool Equals(PQHeartBeatQuotesMessage other) =>
        Equals(QuotesToSendHeartBeats, other.QuotesToSendHeartBeats);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PQHeartBeatQuotesMessage)obj);
    }

    public override int GetHashCode() => QuotesToSendHeartBeats != null ? QuotesToSendHeartBeats.GetHashCode() : 0;
}
