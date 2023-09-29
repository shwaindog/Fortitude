using System.Collections;
using System.Collections.Generic;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public class PQHeartBeatQuotesMessage : IPQHeartBeatQuotesMessage, IEnumerable<IPQLevel0Quote>
    {
        public uint MessageId => (uint)PricingMessageIds.HeartBeatMessage;

        public PQHeartBeatQuotesMessage(IList<IPQLevel0Quote> quotesToSendHeartBeats)
        {
            QuotesToSendHeartBeats = quotesToSendHeartBeats;
        }

        public byte Version => 1;
        public IList<IPQLevel0Quote> QuotesToSendHeartBeats { get; set; }

        protected bool Equals(PQHeartBeatQuotesMessage other)
        {
            return Equals(QuotesToSendHeartBeats, other.QuotesToSendHeartBeats);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PQHeartBeatQuotesMessage) obj);
        }

        public override int GetHashCode()
        {
            return (QuotesToSendHeartBeats != null ? QuotesToSendHeartBeats.GetHashCode() : 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IPQLevel0Quote> GetEnumerator()
        {
            return QuotesToSendHeartBeats.GetEnumerator();
        }
    }
}