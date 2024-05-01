namespace FortitudeIO.Protocols;

public enum CloseReason : byte
{
    Unknown
    , Completed
    , StartConnectionFailed
    , RemoteRequestedClose
    , RemoteDisconnecting
    , RemoteClosed
    , AuthorizationTimeout
    , AuthorizationFailed
}

public class ExpectSessionCloseMessage : VersionedMessage
{
    public const uint ExpectSessionCloseMessageId = uint.MaxValue - 1;
    public ExpectSessionCloseMessage() => Version = 1;

    public ExpectSessionCloseMessage(ExpectSessionCloseMessage toClone) : base(toClone)
    {
        Version = 1;
        CloseReason = toClone.CloseReason;
        ReasonText = toClone.ReasonText;
    }

    public ExpectSessionCloseMessage(CloseReason closeReason, string? reasonText = null)
    {
        Version = 1;
        CloseReason = closeReason;
        ReasonText = reasonText;
    }

    public CloseReason CloseReason { get; set; }

    public string? ReasonText { get; set; }

    public override uint MessageId => ExpectSessionCloseMessageId;

    public override IVersionedMessage Clone() => Recycler?.Borrow<ExpectSessionCloseMessage>() ?? new ExpectSessionCloseMessage();
}
