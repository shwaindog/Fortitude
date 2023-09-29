using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeIO.Topics.Config.ConnectionConfig
{
    public enum TransportType : byte
    {
        Unknown,
        Sockets,
        MemoryMappedFiles
    }
}
