#region

using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeMarketsApi.Trading;

public interface ITradingMessage : IAuthenticatedMessage
{
    int SequenceNumber { get; set; }
    bool IsReplay { get; set; }
    DateTime OriginalSendTime { get; set; }
    IMutableString? MachineName { get; set; }
}
