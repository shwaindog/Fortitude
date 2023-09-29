using System;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Sockets;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

namespace FortitudeMarketsCore.Trading.ORX.Publication
{
    public class AdapterOrderSessionTracker
    {
        private readonly IRecycler orxRecyclingFactory;

        private readonly IMap<ISession, IMap<MutableString, OrxOrder>> orderFromSessionCache =
            new GarbageAndLockFreeMap<ISession, IMap<MutableString, OrxOrder>>(ReferenceEquals);
        private readonly GarbageAndLockFreePooledFactory<IMap<MutableString, OrxOrder>> surplusOrderMaps =
            new GarbageAndLockFreePooledFactory<IMap<MutableString, OrxOrder>>(() => 
                new GarbageAndLockFreeMap<MutableString, OrxOrder>(OrderKeyComparison));

        private readonly IMap<MutableString, ISession> sessionFromOrderIdCache =
            new GarbageAndLockFreeMap<MutableString, ISession>(OrderKeyComparison);

        private readonly GarbageAndLockFreePooledFactory<MutableString> mutableStringPool =
            new GarbageAndLockFreePooledFactory<MutableString>(() => new MutableString());

        private static readonly Func<MutableString, MutableString, bool> OrderKeyComparison = (s1, s2) => s1 == s2;

        public AdapterOrderSessionTracker(IRecycler orxRecyclingFactory)
        {
            this.orxRecyclingFactory = orxRecyclingFactory;
        }

        public void RegisterOrderIdWithSession(OrxOrder order, ISession repositorySession)
        {
            lock (orderFromSessionCache)
            {
                if (!orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                {
                    sessionOrders = surplusOrderMaps.Borrow();
                    sessionOrders.Clear();
                    orderFromSessionCache.Add(repositorySession, sessionOrders);
                }
                
                sessionOrders.Add(
                    mutableStringPool.Borrow().Clear().Append(order.OrderId.VenueAdapterOrderId), order);
            }

            lock (sessionFromOrderIdCache)
            {
                sessionFromOrderIdCache.Add(
                    mutableStringPool.Borrow().Clear().Append(order.OrderId.VenueAdapterOrderId), repositorySession);
            }
        }

        public void UnregisterOrderWithSession(IOrder order)
        {
            lock (orderFromSessionCache)
            {
                if (orderFromSessionCache.TryGetValue(order.OrderPublisher.UnderlyingSession, out var sessionOrders))
                {
                    var removedOrderItem = sessionOrders.Remove(order.OrderId.VenueAdapterOrderId as MutableString);
                    if (removedOrderItem.FoundItem)
                    {
                        mutableStringPool.ReturnBorrowed(removedOrderItem.Key);
                        orxRecyclingFactory.Recycle(removedOrderItem.Value);
                    }
                    if (sessionOrders.Count == 0)
                    {
                        var removedSessionMap = orderFromSessionCache.Remove(order.OrderPublisher.UnderlyingSession);
                        if (removedSessionMap.FoundItem)
                        {
                            surplusOrderMaps.ReturnBorrowed(removedSessionMap.Value);
                        }
                    }
                }
            }
            lock (sessionFromOrderIdCache)
            {
                var orderIdSessKvp = sessionFromOrderIdCache.Remove(order.OrderId.VenueAdapterOrderId as MutableString);
                if (orderIdSessKvp.FoundItem)
                {
                    mutableStringPool.ReturnBorrowed(orderIdSessKvp.Key);
                }
            }
        }

        public void UnregisterSession(ISession repositorySession)
        {
            lock (sessionFromOrderIdCache)
            {
                foreach (var orders in ReturnAllOrdersForSession(repositorySession))
                {
                    var removedOrderItem =
                        sessionFromOrderIdCache.Remove(orders.OrderId.VenueAdapterOrderId as MutableString);
                    if (removedOrderItem.FoundItem)
                    {
                        mutableStringPool.ReturnBorrowed(removedOrderItem.Key);
                        orxRecyclingFactory.Recycle(removedOrderItem.Value);
                    }
                }
            }
            lock (orderFromSessionCache)
            {
                var removedSessionMap = orderFromSessionCache.Remove(repositorySession);
                if (removedSessionMap.FoundItem)
                {
                    surplusOrderMaps.ReturnBorrowed(removedSessionMap.Value);
                }
            }
        }

        public IEnumerable<IOrder> ReturnAllOrdersForSession(ISession repositorySession)
        {
            lock (repositorySession)
            {
                if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                {
                    foreach (var orderKvp in sessionOrders)
                    {
                        yield return orderKvp.Value;
                    }
                }
            }
        }

        public void ClearAll()
        {
            lock (orderFromSessionCache)
            {
                foreach (var sessionMaps in orderFromSessionCache)
                {
                    var ordersMap = sessionMaps.Value;
                    foreach (var orderEntry in ordersMap)
                    {
                        var removedEntry = ordersMap.Remove(orderEntry.Key);
                        if (removedEntry.FoundItem)
                        {
                            mutableStringPool.ReturnBorrowed(removedEntry.Key);
                            orxRecyclingFactory.Recycle(removedEntry.Value);
                        }
                    }

                    var removedSessionMap = orderFromSessionCache.Remove(sessionMaps.Key);
                    if (removedSessionMap.FoundItem)
                    {
                        surplusOrderMaps.ReturnBorrowed(removedSessionMap.Value);
                    }
                }
                orderFromSessionCache.Clear();
            }
            lock (sessionFromOrderIdCache)
            {
                foreach (var ordersSession in sessionFromOrderIdCache)
                {
                    var orderIdSessKvp = sessionFromOrderIdCache.Remove(ordersSession.Key);
                    if (orderIdSessKvp.FoundItem)
                    {
                        mutableStringPool.ReturnBorrowed(orderIdSessKvp.Key);
                    }
                }
                sessionFromOrderIdCache.Clear();
            }
        }
        
        public IEnumerable<ISession> AllRegisteredSessions()
        {
            lock(orderFromSessionCache)
            {
                foreach (var session in orderFromSessionCache)
                {
                    yield return session.Key;
                }
            }
        }

        public IOrder FindOrderFromSessionId(IMutableString adapterId, ISession repositorySession)
        {
            OrxOrder order = null;
            lock (orderFromSessionCache)
            {
                if (orderFromSessionCache.TryGetValue(repositorySession, out var sessionOrders))
                {
                    sessionOrders.TryGetValue(adapterId.ToString(), out order);
                }
            }

            return order;
        }

        public ISession FindSessionFromOrderId(IOrderId orderId)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            sessionFromOrderIdCache.TryGetValue(orderId.VenueAdapterOrderId.ToString(), out var session);
            return session;
        }
    }
}
