using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Trading.Accounts;

public interface IAccountBlotter : IOrderBlotter, IList<IOrderBlotterEntry>
{
    new uint AccountId { get; set; }

    new string AccountName { get; set; }
    
    new AccountType AccountType { get; set; }

    new int Count { get; }

    new IOrderBlotterEntry this[int index] { get; set; } 

    ISortOrderComparer<IOrderBlotterEntry>? SortBy { get; set; }

    TimeSpan PurgeCheckInterval { get; set; }

    new Predicate<IOrderBlotterEntry> AppliesToBlotter { get; set; }

    new Predicate<IOrderBlotterEntry> CanPurge { get; set; }

    new int MinRetainSize { get; set; }

    new int PurgeEligibleSize { get; set; }

    new TimeBoundaryPeriod MinRetainPeriod { get; set; }

    new TimeBoundaryPeriod PurgeEligiblePeriod { get; set; }

    new IAccountBlotter Clone();
}

public class AccountBlotter : OrderBlotter, IAccountBlotter
{
    public AccountBlotter()
    {
    }

    public AccountBlotter(IAccountBlotter toClone)
    {
        SortBy = toClone.SortBy;
        foreach (var orderBlotter in toClone)
        {
            SortedEntries.Add(orderBlotter.OrderId, orderBlotter);
        }
    }

    public TimeSpan PurgeCheckInterval { get; set; }

    IAccountBlotter IAccountBlotter.Clone() => Clone();

    public override AccountBlotter Clone() =>
        Recycler?.Borrow<AccountBlotter>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new AccountBlotter(this);


    public override AccountBlotter CopyFrom(IOrderBlotter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SortedEntries.Clear();
        if (source is AccountBlotter accountBlotter)
        {
            SortBy = accountBlotter.SortBy;
            SortedEntries.CopyFrom(accountBlotter.SortedEntries);
        }
        else if(source is IAccountBlotter iAccountBlotter)
        {
            SortBy = iAccountBlotter.SortBy;
            foreach (var orderBlotterEntry in source)
            {
                SortedEntries.Add(orderBlotterEntry.OrderId, orderBlotterEntry);
            }
        } 
        else 
        {
            foreach (var orderBlotterEntry in source)
            {
                SortedEntries.Add(orderBlotterEntry.OrderId, orderBlotterEntry);
            }
        }
        return this;
    }
}
