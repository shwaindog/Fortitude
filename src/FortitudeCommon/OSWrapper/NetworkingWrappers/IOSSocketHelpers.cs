using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public static class OSSocketHelpers
    {
        public static IPEndPoint RemoteOrLocalIPEndPoint(this IOSSocket socket)
        {
            return socket.Connected ? socket.RemoteEndPoint as IPEndPoint 
                : socket.LocalEndPoint as IPEndPoint;
        }
    }
}
