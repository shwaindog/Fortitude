#region

using FortitudeMarkets.TimeSeriesData.Requests;

#endregion

namespace FortitudeMarkets.TimeSeriesData.Responses;

public interface IPagedCollectionResult<T>
{
    IList<HistoricalEntry<T>> NextPage(IList<HistoricalEntry<T>> toClearAndPopulate);
    IEnumerable<HistoricalEntry<T>[]> AsArrayEnumerable();
    IEnumerable<HistoricalEntry<T>> NextPageAsEnumerable();
    IObservable<HistoricalEntry<T>> NextPageAsObservable(ReplyResultsType replyResultsType);
}

public abstract class PagedCollectionResult<T> : IPagedCollectionResult<T>
{
    public abstract IList<HistoricalEntry<T>> NextPage(IList<HistoricalEntry<T>> toClearAndPopulate);
    public abstract IEnumerable<HistoricalEntry<T>[]> AsArrayEnumerable();
    public abstract IEnumerable<HistoricalEntry<T>> NextPageAsEnumerable();
    public abstract IObservable<HistoricalEntry<T>> NextPageAsObservable(ReplyResultsType replyResultsType);
}
