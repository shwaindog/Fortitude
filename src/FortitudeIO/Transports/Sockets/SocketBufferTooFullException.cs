using System;
using FortitudeCommon.Types;

namespace FortitudeIO.Transports.Sockets
{
    [TestClassNotRequired]
    public class SocketBufferTooFullException : Exception
    {
        public SocketBufferTooFullException(string message) : base(message) {}
    }
}