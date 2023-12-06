#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols;

public interface IVersionedMessage : IReusableObject<IVersionedMessage>
{
    uint MessageId { get; }
    byte Version { get; }
}
