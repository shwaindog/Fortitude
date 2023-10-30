namespace FortitudeIO.Protocols
{
    public interface IVersionedMessage
    {
        uint MessageId { get; }
        byte Version { get; }
    }
}
