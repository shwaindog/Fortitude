#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Construction;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

public interface IOrxSerializationRepository : IStreamEncoderFactory
{
    IOrxSerializationRepository RegisterSerializer<TM>()
        where TM : class, IVersionedMessage, new();

    IOrxSerializationRepository RegisterSerializer<TM>(uint msgId)
        where TM : class, IVersionedMessage, new();
}

public class OrxSerializationRepository : SocketStreamMessageEncoderFactory, IOrxSerializationRepository
{
    private readonly IRecycler recycler;
    private readonly IMessageIdSerializationRepository serializationRepository;
    private readonly ISyncLock serializerLock = new SpinLockLight();
    private readonly IDictionary<uint, uint> serializersCallbackCount = new Dictionary<uint, uint>();

    public OrxSerializationRepository(
        IDictionary<uint, IMessageSerializer> serializerMap,
        IMessageIdSerializationRepository serializationRepository, IRecycler recycler) : base(serializerMap)
    {
        this.serializationRepository = serializationRepository;
        this.recycler = recycler;
    }

    public IOrxSerializationRepository RegisterSerializer<T>() where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = recycler.Borrow<T>();
        var serializer = serializationRepository.GetSerializer<T>(instanceOfTypeToSerialize.MessageId)!;
        RegisterMessageSerializer(instanceOfTypeToSerialize.MessageId, serializer);
        instanceOfTypeToSerialize.DecrementRefCount();
        return this;
    }

    public IOrxSerializationRepository RegisterSerializer<TM>(uint msgId)
        where TM : class, IVersionedMessage, new()
    {
        IMessageSerializer? mu;
        if (!SerializerMap.TryGetValue(msgId, out var u))
        {
            SerializerMap.Add(msgId, mu = serializationRepository.GetSerializer<TM>(msgId)!);
            lock (serializersCallbackCount)
            {
                serializersCallbackCount[msgId] = 0;
            }
        }
        else if ((mu = u as IMessageSerializer<TM>) == null)
        {
            throw new Exception("Two different message types cannot be registered to the same Id");
        }

        return this;
    }
}
