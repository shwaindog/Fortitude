using System.Collections.Generic;
using FortitudeIO.Protocols;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQHeartBeatQuotesMessage : IVersionedMessage
    {
        IList<IPQLevel0Quote> QuotesToSendHeartBeats { get; set; }
    }
}
