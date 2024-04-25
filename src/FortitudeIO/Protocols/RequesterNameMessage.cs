namespace FortitudeIO.Protocols;

public class RequesterNameMessage : VersionedMessage
{
    public const uint RequesterNameMessageId = uint.MaxValue - 2;
    public RequesterNameMessage() => Version = 1;

    public RequesterNameMessage(RequesterNameMessage toClone) : base(toClone)
    {
        Version = toClone.Version;
        RequesterConnectionName = toClone.RequesterConnectionName;
    }

    public RequesterNameMessage(string requesterConnectionName) : base(1) => RequesterConnectionName = requesterConnectionName;

    public string RequesterConnectionName { get; set; } = null!;

    public override uint MessageId => RequesterNameMessageId;

    public override IVersionedMessage Clone() => Recycler?.Borrow<RequesterNameMessage>() ?? new RequesterNameMessage(this);
}
