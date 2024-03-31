#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Transports.Network;

[TestClassNotRequired]
public class SocketBufferTooFullException : Exception
{
    public SocketBufferTooFullException(string message) : base(message) { }
}
