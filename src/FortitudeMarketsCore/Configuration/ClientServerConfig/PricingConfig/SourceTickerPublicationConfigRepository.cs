using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig
{
    public class SourceTickerPublicationConfigRepository : ISourceTickerPublicationConfigRepository
    {
        private readonly Dictionary<string, ISourceTickerPublicationConfig> defs = new Dictionary<string, ISourceTickerPublicationConfig>();



        public ISourceTickerPublicationConfig this[string symbol]
        {
            get
            {
                ISourceTickerPublicationConfig def;
                return defs.TryGetValue(symbol, out def) ? def : null;
            }
        }

        public SourceTickerPublicationConfigRepository(IEnumerable<ISourceTickerPublicationConfig> initialise)
        {
            foreach (var ISourceTickerPublicationConfig in initialise)
            {
                defs.Add(ISourceTickerPublicationConfig.Ticker, ISourceTickerPublicationConfig);
            }
        }

        public IEnumerator<ISourceTickerPublicationConfig> GetEnumerator()
        {
            return defs.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}