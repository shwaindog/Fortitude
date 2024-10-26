#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeMarkets.Trading;

#endregion

namespace FortitudeMarkets.Trading;

public abstract class TradingMessage : AuthenticatedMessage, ITradingMessage
{
    protected TradingMessage() { }

    protected TradingMessage(ITradingMessage toClone)
    {
        SequenceNumber = toClone.SequenceNumber;
        IsReplay = toClone.IsReplay;
        MachineName = toClone.MachineName;
        OriginalSendTime = toClone.OriginalSendTime;
    }

    protected TradingMessage(bool isReplay = false, string? machineName = null,
        DateTime? originalSendTime = null, DateTime? sendTime = null, string? senderName = null,
        byte versionNumber = TradingVersionInfo.CurrentVersion, IUserData? userData = null, string? info = null)
        : this(isReplay, (MutableString?)machineName, originalSendTime, sendTime, (MutableString?)senderName,
            versionNumber, userData, (MutableString)info) { }

    protected TradingMessage(bool isReplay = false, IMutableString? machineName = null,
        DateTime? originalSendTime = null, DateTime? sendTime = null, IMutableString? senderName = null,
        byte versionNumber = TradingVersionInfo.CurrentVersion, IUserData? userData = null,
        IMutableString? info = null) : base(versionNumber, senderName, sendTime, userData, info)
    {
        IsReplay = isReplay;
        MachineName = machineName;
        OriginalSendTime = originalSendTime ?? DateTimeConstants.UnixEpoch;
    }

    public int SequenceNumber { get; set; }
    public bool IsReplay { get; set; }
    public IMutableString? MachineName { get; set; }
    public DateTime OriginalSendTime { get; set; } = DateTimeConstants.UnixEpoch;

    protected bool Equals(TradingMessage other) =>
        base.Equals(other) && SequenceNumber == other.SequenceNumber &&
        IsReplay == other.IsReplay && Equals(MachineName, other.MachineName) &&
        OriginalSendTime.Equals(other.OriginalSendTime);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TradingMessage)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ SequenceNumber;
            hashCode = (hashCode * 397) ^ IsReplay.GetHashCode();
            hashCode = (hashCode * 397) ^ (MachineName != null ? MachineName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ OriginalSendTime.GetHashCode();
            return hashCode;
        }
    }
}
