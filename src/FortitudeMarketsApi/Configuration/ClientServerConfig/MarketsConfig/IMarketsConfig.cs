using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.MarketsConfig
{
    interface IMarketsConfig
    {
        IList<IVenueConfig> VenueConfigs { get; set; }
    }

    public interface IVenueConfig
    {
    }
}
