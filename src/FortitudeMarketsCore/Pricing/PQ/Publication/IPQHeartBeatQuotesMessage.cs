﻿#region

using FortitudeIO.Protocols;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQHeartBeatQuotesMessage : IVersionedMessage
{
    IList<IPQLevel0Quote> QuotesToSendHeartBeats { get; set; }
}
