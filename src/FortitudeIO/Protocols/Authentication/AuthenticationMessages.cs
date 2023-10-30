using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeIO.Protocols.ORX.Authentication
{
    public enum AuthenticationMessages
    {
        LogonRequest = 1,
        LoggedInResponse = 2,
        LoggedOutResponse = 3,
    }
}
