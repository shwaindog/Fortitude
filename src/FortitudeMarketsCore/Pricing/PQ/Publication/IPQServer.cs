#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQServer<T> : IDisposable where T : IPQLevel0Quote
{
    bool IsStarted { get; }
    void StartServices();
    T? Register(string ticker);
    void Unregister(T quote);
    void Publish(T quote);
}
