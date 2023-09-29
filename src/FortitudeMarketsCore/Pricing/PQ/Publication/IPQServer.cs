using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQServer<T> : IDisposable where T : IPQLevel0Quote
    {
        void StartServices();
        T Register(string ticker);
        void Unregister(T quote);
        void Publish(T quote);
        bool IsStarted { get; }
    }
}
