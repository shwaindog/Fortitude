using System;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

namespace FortitudeMarketsCore.Pricing.PQ
{
    public sealed class PQQuoteSerializerFactory : IPQQuoteSerializerFactory
    {
        private readonly IMap<uint, PQDeserializerBase> deserializers = 
            new LinkedListCache<uint, PQDeserializerBase>();

        public bool HasPictureDeserializers => deserializers.Count > 0;

        public IPQDeserializer CreateQuoteDeserializer<T>(ISourceTickerClientAndPublicationConfig stream) 
            where T : class, IPQLevel0Quote
        {
            PQDeserializerBase quoteDeserializer = new PQQuoteDeserializer<T>(stream);
            deserializers.Add(stream.Id, quoteDeserializer);
            return quoteDeserializer;
        }

        public IPQDeserializer GetQuoteDeserializer(IUniqueSourceTickerIdentifier identifier)
        {
            return deserializers.TryGetValue(identifier.Id, out var quoteDeserializer) ? quoteDeserializer : null;
        }

        public void RemoveQuoteDeserializer(IUniqueSourceTickerIdentifier identifier)
        {
            deserializers.Remove(identifier.Id);
        }

        public ICallbackBinaryDeserializer<T> GetDeserializer<T>(uint msgId) where T : class
        {
            if (typeof(T) == typeof(IPQLevel0Quote))
            {
                if (deserializers.TryGetValue(msgId, out var quoteDeserializer))
                {
                    return quoteDeserializer as ICallbackBinaryDeserializer<T>;
                }
            }
            throw new NotSupportedException();
        }

        public IBinarySerializer GetSerializer<T>(uint msgId) where T : class
        {
            if (typeof(T) == typeof(uint[])) 
            {
                return new PQSnapshotIdsRequestSerializer();
            }
            throw new NotSupportedException();
        }
    }
}