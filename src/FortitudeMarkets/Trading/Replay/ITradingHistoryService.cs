namespace FortitudeMarkets.Trading.Replay;

public interface ITradingHistoryService
{
    void FetchHistory(DateTime from, DateTime to);
    event Action<IReplayMessage> ReplayMessage;
}
