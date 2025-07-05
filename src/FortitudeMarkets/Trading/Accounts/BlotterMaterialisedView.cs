// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders.SpotOrders;

namespace FortitudeMarkets.Trading.Accounts;

public readonly struct BlotterViewKey(AccountViewType accountViewType, TimeBoundaryPeriod minRetainPeriod, int minRetainSize, ushort? tickerId = null)
{
    public AccountViewType AccountViewType { get; } = accountViewType;

    public int MinRetainSize { get; } = minRetainSize;

    public TimeBoundaryPeriod MinRetainPeriod { get; } = minRetainPeriod;

    public ushort? TickerId { get; } = tickerId;
}

public interface IBlotterView : IOrderBlotter
{
    BlotterViewKey BlotterViewKey { get; }

    IAccountBlotter ParentBlotter { get; }
}

public class BlotterMaterialisedView : OrderBlotter, IBlotterView
{
    protected BlotterViewKey   ViewKey;
    protected IAccountBlotter? ParentAccountBlotter;

    public BlotterMaterialisedView() { }

    public BlotterMaterialisedView(IBlotterView toClone)
    {
        foreach (var orderBlotter in toClone)
        {
            SortedEntries.Add(orderBlotter.OrderId, orderBlotter);
        }
    }

    public BlotterMaterialisedView WithViewKeyAndParent(BlotterViewKey blotterViewKey, IAccountBlotter parentBlotter)
    {
        ViewKey = blotterViewKey;

        ParentAccountBlotter = parentBlotter;

        SubscribeToParentEvents(parentBlotter);

        return this;
    }

    protected void SubscribeToParentEvents(IAccountBlotter parentBlotter)
    {
        parentBlotter.OrderUpdates += ParentUpdate;
    }

    protected virtual void ParentUpdate(IOrderBlotterEntry updated, IOrderBlotterEntry? previous, IRecyclableSet<IBlotterProperty> changedValues)
    {
        if (!AppliesToBlotter(updated) || !AppliesToView(updated, previous, changedValues)) return;
        LastReceivedUpdateTime = updated.LastUpdateTime.Max(LastReceivedUpdateTime);
        SortedEntries.Add(updated.OrderId, updated);
    }

    public BlotterViewKey BlotterViewKey => ViewKey;

    public IAccountBlotter ParentBlotter => ParentAccountBlotter!;

    private bool AppliesToView(IOrderBlotterEntry updated, IOrderBlotterEntry? previous, IRecyclableSet<IBlotterProperty> changedValues)
    {
        if (ViewKey.TickerId != null && updated.TickerId != ViewKey.TickerId)
        {
            return false;
        }
        switch (ViewKey.AccountViewType)
        {
            case AccountViewType.AggressiveOrders:
                if ((updated.Type & (OrderType.Market | OrderType.AggressiveLimit | OrderType.Stop)) == 0)
                {
                    return false;
                }
                break;
            case AccountViewType.DoneOrders:
                if (updated.IsComplete != true)
                {
                    return false;
                }
                break;
            case AccountViewType.OpenOrders:
                if (updated.IsComplete)
                {
                    SortedEntries.Remove(updated.OrderId);
                    return false;
                }
                break;
            case AccountViewType.PassiveOrders:
                if ((updated.Type & (OrderType.PassiveLimit)) == 0)
                {
                    return false;
                }
                break;
            case AccountViewType.OpenPosition:
                if (changedValues.Contains(OrderBlotterEntry.ExecutedSizeProperty))
                {
                    var previousPosition =
                        SortedEntries.Values
                                     .Where(obe => !obe.OrderId.Equals(updated.OrderId))
                                     .Sum(obe => obe.ExecutedPrice) ?? 0m;
                    var currentPosition = previousPosition + updated.ExecutedPrice;
                    if (currentPosition == 0m)
                    {
                        SortedEntries.Clear();
                    }
                }
                break;
        }

        return true;
    }

    public override BlotterMaterialisedView Clone() =>
        Recycler?.Borrow<BlotterMaterialisedView>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new BlotterMaterialisedView(this);

    public override BlotterMaterialisedView CopyFrom(IOrderBlotter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SortedEntries.Clear();

        if (source is BlotterMaterialisedView blotterView)
        {
            if (!ReferenceEquals(ParentAccountBlotter, blotterView.ParentAccountBlotter))
            {
                if (ParentAccountBlotter != null) { }
            }
            SortBy = blotterView.SortBy;
            SortedEntries.CopyFrom(blotterView.SortedEntries);
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
