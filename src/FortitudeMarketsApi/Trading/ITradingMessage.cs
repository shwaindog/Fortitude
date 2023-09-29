using System;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;

namespace FortitudeMarketsApi.Trading
{
    public interface ITradingMessage : IAuthenticatedMessage
    {
        int SequenceNumber { get; set; }
        bool IsReplay { get; set; }
        DateTime OriginalSendTime { get; set; }
        IMutableString MachineName { get; set; }
    }
}
