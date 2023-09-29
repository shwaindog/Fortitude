using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Configuration;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig
{
    public class MarketServerConfig<T> : IMarketServerConfig<T> where T : class, IMarketServerConfig<T>
    {
        protected ISubject<IMarketServerConfigUpdate<T>> UpdateStream;
        protected ISubject<IConnectionUpdate> ConnectionUpdateStream;
        private string name;
        private MarketServerType marketServerType;
        private IEnumerable<IConnectionConfig> serverConnections;
        private ITimeTable availabilityTimeTable;
        private readonly IObservable<IMarketServerConfigUpdate<T>> repoUpdateStream;
        private IDisposable updateStreamSub;

        public MarketServerConfig(string name, MarketServerType marketServerType, IEnumerable<IConnectionConfig> serverConnections , ITimeTable availabilityTimeTable,
            IObservable<IMarketServerConfigUpdate<T>> repoUpdateStream = null)
        {
            Id = IdGenLong.Next();
            ConnectionUpdateStream = new Subject<IConnectionUpdate>();
            this.name = name;
            this.marketServerType = marketServerType;
            this.serverConnections = serverConnections;
            foreach (var serverConnectionConfig in serverConnections)
            {
                serverConnectionConfig.Updates = ConnectionUpdateStream;
            }
            this.availabilityTimeTable = availabilityTimeTable;
            this.repoUpdateStream = repoUpdateStream;
            UpdateStream = new Subject<IMarketServerConfigUpdate<T>>();
            updateStreamSub = repoUpdateStream?.Subscribe(UpdateStream);
            UpdateStream.Subscribe(ListenForUpdates);
        }

        private void ListenForUpdates(IMarketServerConfigUpdate<T> scu )
        {
            if (scu.ServerConfig.Id != Id) return;
            if (scu.EventType == EventType.Updated ||
                scu.EventType == EventType.Retrieved)
            {
                name = scu.ServerConfig.Name;
                marketServerType = scu.ServerConfig.MarketServerType;
                UpdateConnectionsList(scu.ServerConfig.ServerConnections);
                availabilityTimeTable = scu.ServerConfig.AvailabilityTimeTable;
                UpdateFields(scu.ServerConfig);
            }
            else if (scu.EventType == EventType.Deleted)
            {
                updateStreamSub?.Dispose();
            }
            if (ReferenceEquals(this, scu.ServerConfig)) return;
            UpdateStream.OnNext(new MarketServerConfigUpdate<T>(this as T, scu.EventType));
        }

        public MarketServerConfig(MarketServerConfig<T> toClone)
        {
            Id = toClone.Id;
            ConnectionUpdateStream = new Subject<IConnectionUpdate>();
            name = toClone.Name;
            marketServerType = toClone.MarketServerType;
            serverConnections = toClone.ServerConnections.Select(sc => sc.Clone()).ToArray();
            foreach (var serverConnectionConfig in serverConnections)
            {
                serverConnectionConfig.Updates = ConnectionUpdateStream;
            }
            availabilityTimeTable = toClone.AvailabilityTimeTable;
            repoUpdateStream = toClone.repoUpdateStream;
            UpdateStream = new Subject<IMarketServerConfigUpdate<T>>();
            updateStreamSub = repoUpdateStream?.Subscribe(UpdateStream);
            UpdateStream.Subscribe(ListenForUpdates);
        }
        
        protected virtual void UpdateFields(T updatedServerConfig)
        {   
        }

        private void UpdateConnectionsList(IEnumerable<IConnectionConfig> newConnectionsList)
        {
            var originalServerConnection = serverConnections;
            serverConnections = newConnectionsList.Select(scc =>
            {
                var clonedScc = scc.Clone();
                clonedScc.Updates = ConnectionUpdateStream;
                return clonedScc;
            }).ToList();
            var diffResults = originalServerConnection.Diff(serverConnections, scc => scc.Id);
            foreach (var scc in diffResults.NewItems)
            {
                ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Created));
            }
            foreach (var scc in diffResults.UpdatedItems)
            {
                ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Updated));
            }
            foreach (var scc in diffResults.DeletedItems)
            {
                ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Deleted));
            }
        }

        public long Id { get; private set; }

        public string Name
        {
            get => name;
            protected set
            {
                if (name == value) return;
                name = value;
                UpdateStream.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
            }
        }

        public MarketServerType MarketServerType
        {
            get => marketServerType;
            protected set
            {
                if (marketServerType == value) return;
                marketServerType = value;
                UpdateStream.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
            }
        }

        public IEnumerable<IConnectionConfig> ServerConnections
        {
            get => serverConnections;
            protected set
            {
                if (Equals(serverConnections, value)) return;
                serverConnections = value;
                UpdateStream.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
            }
        }

        public ITimeTable AvailabilityTimeTable
        {
            get => availabilityTimeTable;
            protected set
            {
                if (availabilityTimeTable == value) return;
                availabilityTimeTable = value;
                UpdateStream.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
            }
        }

        public IObservable<IMarketServerConfigUpdate<T>> Updates
        {
            get => UpdateStream.AsObservable();
            set
            {
                if (ReferenceEquals(value, UpdateStream)) return;
                updateStreamSub?.Dispose();
                if (value != null)
                {
                    updateStreamSub = value.Subscribe(UpdateStream);
                }
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public IMarketServerConfig<T> Clone()
        {
            return new MarketServerConfig<T>(this);
        }

        protected bool Equals(MarketServerConfig<T> other)
        {
            return string.Equals(name, other.name) && marketServerType == other.marketServerType 
                && serverConnections.SequenceEqual(other.ServerConnections) 
                && Equals(availabilityTimeTable, other.availabilityTimeTable) && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MarketServerConfig<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) marketServerType;
                hashCode = (hashCode * 397) ^ (serverConnections != null ? serverConnections.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (availabilityTimeTable != null ? availabilityTimeTable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id.GetHashCode();
                return hashCode;
            }
        }
    }
}