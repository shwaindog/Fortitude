// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class BusRulesMessageDeserializationRepository : IMessageDeserializationRepository
{
    private readonly IMessageDeserializationRepository backingDeserializationRepository;

    private SingleParamActionWrapper<IMessageDeserializer>? registeredActionWrapper;
    private SingleParamActionWrapper<IMessageDeserializer>? unregisteredActionWrapper;

    public BusRulesMessageDeserializationRepository(IMessageDeserializationRepository backingDeserializationRepository)
    {
        this.backingDeserializationRepository                            =  backingDeserializationRepository;
        backingDeserializationRepository.MessageDeserializerRegistered   += FireBusRulesEventMessageDeserializerRegistered;
        backingDeserializationRepository.MessageDeserializerUnregistered += FireBusRulesEventMessageDeserializerUnregistered;
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        backingDeserializationRepository.CopyFrom(source, copyMergeFlags);

    public IMessageSerdesRepository CopyFrom(IMessageSerdesRepository source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        backingDeserializationRepository.CopyFrom(source, copyMergeFlags);

    public IEnumerable<uint> RegisteredMessageIds     => backingDeserializationRepository.RegisteredMessageIds;
    public bool              IsRegistered(uint msgId) => backingDeserializationRepository.IsRegistered(msgId);

    public IMessageDeserializationRepository CopyFrom
    (IMessageDeserializationRepository source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        backingDeserializationRepository.CopyFrom(source, copyMergeFlags);

    public string Name => backingDeserializationRepository.Name;

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> AllRegisteredDeserializers =>
        backingDeserializationRepository.AllRegisteredDeserializers;

    public IMessageDeserializationRepository? CascadingFallbackDeserializationRepo
    {
        get => backingDeserializationRepository.CascadingFallbackDeserializationRepo;
        set => backingDeserializationRepository.CascadingFallbackDeserializationRepo = value;
    }

    public void StateReset()
    {
        backingDeserializationRepository.StateReset();
    }

    public INotifyingMessageDeserializer<TM>?
        RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null, bool forceOverride = false)
        where TM : class, IVersionedMessage, new() =>
        backingDeserializationRepository.RegisterDeserializer<TM>(messageDeserializer, forceOverride);

    public IMessageDeserializer RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer, bool forceOverride = false) =>
        backingDeserializationRepository.RegisterDeserializer(msgId, messageDeserializer, forceOverride);

    public bool UnregisterDeserializer(uint msgId) => backingDeserializationRepository.UnregisterDeserializer(msgId);

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializersOfType(Type messageType) =>
        backingDeserializationRepository.RegisteredDeserializersOfType(messageType);

    public bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer) =>
        backingDeserializationRepository.TryGetDeserializer(msgId, out messageDeserializer);

    public IMessageDeserializer? GetDeserializer(uint msgId) => backingDeserializationRepository.GetDeserializer(msgId);

    public IEnumerable<uint> GetRegisteredMessageIds(IMessageDeserializer messageDeserializer) =>
        backingDeserializationRepository.GetRegisteredMessageIds(messageDeserializer);

    public INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        backingDeserializationRepository.GetDeserializer<TM>(msgId);

    public bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new() =>
        backingDeserializationRepository.IsRegisteredWithType<TS, TM>(msgId);


    event Action<IMessageDeserializer>? IMessageDeserializationRepository.MessageDeserializerRegistered
    {
        add => registeredActionWrapper += value;
        remove => registeredActionWrapper -= value;
    }

    event Action<IMessageDeserializer>? IMessageDeserializationRepository.MessageDeserializerUnregistered
    {
        add => unregisteredActionWrapper += value;
        remove => unregisteredActionWrapper -= value;
    }

    private void FireBusRulesEventMessageDeserializerRegistered(IMessageDeserializer newlyRegistered)
    {
        registeredActionWrapper?.Invoke(newlyRegistered);
    }

    private void FireBusRulesEventMessageDeserializerUnregistered(IMessageDeserializer deregistered)
    {
        unregisteredActionWrapper?.Invoke(deregistered);
    }
}
