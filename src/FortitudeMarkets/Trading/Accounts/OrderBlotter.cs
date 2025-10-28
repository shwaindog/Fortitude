// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders;

namespace FortitudeMarkets.Trading.Accounts;

public delegate void UpdatedBlotterEntry(IOrderBlotterEntry updated, IOrderBlotterEntry? previous, IRecyclableSet<IBlotterProperty> changedVValues);

public delegate void RemovedBlotterEntry(IOrderBlotterEntry updated);

public interface IOrderBlotter : IReusableObject<IOrderBlotter>, IReadOnlyList<IOrderBlotterEntry>
{
    uint AccountId { get; }

    string AccountName { get; }

    AccountType AccountType { get; }

    Predicate<IOrderBlotterEntry> AppliesToBlotter { get; }

    Predicate<IOrderBlotterEntry> CanPurge { get; }

    int MinRetainSize { get; }

    int PurgeEligibleSize { get; }

    TimeBoundaryPeriod MinRetainPeriod { get; }

    TimeBoundaryPeriod PurgeEligiblePeriod { get; }

    DateTime LastUpdateTime { get; }


    event UpdatedBlotterEntry? OrderUpdates;


    event RemovedBlotterEntry? Removed;
}

public class OrderBlotter : ReusableObject<IOrderBlotter>, IOrderBlotter
{
    protected readonly ResortableDictionary<IOrderId, IOrderBlotterEntry> SortedEntries = new();

    protected DateTime NextPurgeCheckTime     = DateTime.MinValue;
    protected DateTime LastReceivedUpdateTime = DateTime.MinValue;

    private static readonly SortDateTimeOrderComparer<IOrderBlotterEntry> DefaultCreationTimeSort = new(obe => obe!.CreationTime);

    private static readonly Predicate<IOrderBlotterEntry> DefaultAppliesToBlotter = _ => true;

    private static readonly Predicate<IOrderBlotterEntry> DefaultPurgeFromBlotter = blotterOrderEntry =>
    {
        var oneDayAgo = TimeContext.UtcNow.AddDays(-1);
        if (blotterOrderEntry.IsComplete && blotterOrderEntry.DoneTime < oneDayAgo) return true;
        return false;
    };

    public OrderBlotter()
    {
        SortedEntries.SortComparer = DefaultCreationTimeSort;
    }

    public OrderBlotter(IOrderBlotter toClone)
    {
        foreach (var orderBlotter in toClone)
        {
            SortedEntries.Add(orderBlotter.OrderId, orderBlotter);
        }
    }

    public event UpdatedBlotterEntry? OrderUpdates;

    public event RemovedBlotterEntry? Removed;

    public uint AccountId { get; set; }

    public string AccountName { get; set; } = "";

    public AccountType AccountType { get; set; }

    public DateTime LastUpdateTime => LastReceivedUpdateTime;

    public TimeBoundaryPeriod MinRetainPeriod { get; set; }

    public int MinRetainSize { get; set; }

    public TimeBoundaryPeriod PurgeEligiblePeriod { get; set; }

    public int PurgeEligibleSize { get; set; }

    public ISortOrderComparer<IOrderBlotterEntry>? SortBy
    {
        get => SortedEntries.SortComparer;
        set => SortedEntries.SortComparer = value ?? DefaultCreationTimeSort;
    }

    public Predicate<IOrderBlotterEntry> AppliesToBlotter { get; set; } = DefaultAppliesToBlotter;

    public Predicate<IOrderBlotterEntry> CanPurge { get; set; } = DefaultPurgeFromBlotter;

    public void Add(IOrderBlotterEntry item)
    {
        if (AppliesToBlotter(item)) return;
        LastReceivedUpdateTime = item.LastUpdateTime.Max(LastReceivedUpdateTime);
        IOrderBlotterEntry? previous = null;
        if (SortedEntries.TryGetValue(item.OrderId, out var entry))
        {
            previous = entry;
        }
        SortedEntries.Add(item.OrderId, item);
        if (OrderUpdates != null)
        {
            var changes = previous != null ? item.Changes(previous) : item.AllPopulatedFields();
            OrderUpdates?.Invoke(item, previous, changes);
        }
    }

    public void Clear()
    {
        if (Removed != null)
        {
            foreach (var blotterEntry in SortedEntries)
            {
                Removed?.Invoke(blotterEntry.Value);
            }
        }
        SortedEntries.Clear();
    }

    public bool Contains(IOrderBlotterEntry item)
    {
        return SortedEntries.ContainsKey(item.OrderId);
    }

    public void CopyTo(IOrderBlotterEntry[] array, int arrayIndex)
    {
        var i = arrayIndex;
        foreach (var sortedOrderBlotterEntry in SortedEntries)
        {
            if (i < array.Length)
            {
                array[i++] = sortedOrderBlotterEntry.Value;
            }
            else
            {
                break;
            }
        }
    }

    public int Count => SortedEntries.Count;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IOrderBlotterEntry> GetEnumerator() => SortedEntries.Values.GetEnumerator();

    public int IndexOf(IOrderBlotterEntry item) => SortedEntries.IndexOf(item.OrderId);

    public void Insert(int index, IOrderBlotterEntry item)
    {
        if (AppliesToBlotter(item)) return;
        LastReceivedUpdateTime = item.LastUpdateTime.Max(LastReceivedUpdateTime);
        IOrderBlotterEntry? previous = null;
        if (SortedEntries.TryGetValue(item.OrderId, out var entry))
        {
            previous = entry;
        }
        SortedEntries.Add(item.OrderId, item);
        if (OrderUpdates != null)
        {
            var changes = previous != null ? item.Changes(previous) : item.AllPopulatedFields();
            OrderUpdates?.Invoke(item, previous, changes);
        }
    }

    public bool IsReadOnly => false;

    public IOrderBlotterEntry this[int index]
    {
        get => SortedEntries[index];
        set => Add(value);
    }

    public bool Remove(IOrderBlotterEntry item)
    {
        if (AppliesToBlotter(item)) return false;
        IOrderBlotterEntry? previous = null;
        if (SortedEntries.TryGetValue(item.OrderId, out var entry))
        {
            previous = entry;
        }
        var result = SortedEntries.Remove(item.OrderId);
        if (Removed != null && previous != null)
        {
            Removed?.Invoke(previous);
        }
        return result;
    }

    public void RemoveAt(int index)
    {
        var itemAt = SortedEntries[index];
        Remove(itemAt);
    }

    public override OrderBlotter Clone() => Recycler?.Borrow<OrderBlotter>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new OrderBlotter(this);


    public override OrderBlotter CopyFrom(IOrderBlotter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SortedEntries.Clear();
        if (source is OrderBlotter accountBlotter)
        {
            SortBy = accountBlotter.SortBy;
            SortedEntries.CopyFrom(accountBlotter.SortedEntries);
        }
        else if (source is IAccountBlotter iAccountBlotter)
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
