using System;
using System.Collections.Generic;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;

namespace FortitudeIO.Transports.Sockets.Subscription
{
    public abstract class SocketStreamSubscriber : ISocketStreamSubscriber
    {
        protected readonly ISocketDispatcher Dispatcher;
        public readonly IFLogger logger;
        private readonly ISyncLock serializerLock = new SpinLockLight();
        protected readonly IMap<uint, IBinaryDeserializer> deserializers;
        private readonly IDictionary<uint, uint> deserializersCallbackCount = new Dictionary<uint, uint>();

        protected SocketStreamSubscriber(IFLogger logger, ISocketDispatcher dispatcher, string sessionDescription,
            int wholeMessagesPerReceive, IMap<uint, IBinaryDeserializer> map = null)
        {
            this.logger = logger;
            Dispatcher = dispatcher;
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            deserializers = map ?? new LinkedListCache<uint, IBinaryDeserializer>();
            SessionDescription = sessionDescription;
            WholeMessagesPerReceive = wholeMessagesPerReceive;
        }

        public bool ZeroBytesReadIsDisconnection { get; set; } = true;

        public abstract int RecvBufferSize { get; }
        public IFLogger Logger => logger;

        public void StartMessaging()
        {
            Dispatcher.Start();
        }

        public void StopMessaging()
        {
            Dispatcher.Stop();
        }

        public abstract IBinaryStreamPublisher StreamToPublisher { get; }


        public abstract IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> deserializers);
        public string SessionDescription { get; }
        public int WholeMessagesPerReceive { get; }

        public void  Unregister(ISocketSessionConnection sessionConnection)
        {
            Dispatcher.Listener.UnregisterForListen(sessionConnection);
        }

        public int RegisteredDeserializersCount => deserializers.Count;

        public void RegisterDeserializer<TM>(uint msgId, Action<TM, object, ISession> msgHandler) where TM : class
        {
            if (msgHandler == null)
                throw new Exception("Message Handler cannot be null");
            IBinaryDeserializer u;
            ICallbackBinaryDeserializer<TM> mu;
            if (!deserializers.TryGetValue(msgId, out u))
            {
                deserializers.Add(msgId, mu = GetFactory().GetDeserializer<TM>(msgId));
                lock (deserializersCallbackCount)
                {
                    deserializersCallbackCount[msgId] = 0;
                }
            }
            else if ((mu = u as ICallbackBinaryDeserializer<TM>) == null)
            {
                throw new Exception("Two different message types cannot be registered to the same Id");
            }
            else if (mu.IsRegistered(msgHandler))
            {
                throw new Exception("Message Handler already registered");
            }
            mu.Deserialized += msgHandler;
            serializerLock.Acquire();
            try
            {
                deserializersCallbackCount[msgId]++;
            }
            finally
            {
                serializerLock.Release();
            }
        }

        public void UnregisterDeserializer<TM>(uint msgId, Action<TM, object, ISession> msgHandler) where TM : class
        {
            IBinaryDeserializer u;
            ICallbackBinaryDeserializer<TM> mu;
            if (!deserializers.TryGetValue(msgId, out u) || (mu = u as ICallbackBinaryDeserializer<TM>) == null)
                throw new Exception("Message Type could not be matched with the provided Id");
            if (!mu.IsRegistered(msgHandler))
                throw new Exception("Unknown Message Handler");
            mu.Deserialized -= msgHandler;
            serializerLock.Acquire();
            try
            {
                if (--deserializersCallbackCount[msgId] == 0)
                    deserializers.Remove(msgId);
            }
            finally
            {
                serializerLock.Release();
            }
        }

        protected abstract IBinaryDeserializationFactory GetFactory();
        public abstract void OnCxError(ISocketSessionConnection cx, string errorMsg, int proposedReconnect);
    }
}