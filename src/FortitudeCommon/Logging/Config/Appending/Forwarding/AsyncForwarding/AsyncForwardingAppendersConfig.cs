// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;

public interface IAsyncForwardingAppendersConfig : IQueueingAppenderConfig
{
    const bool DefaultShouldBroadcast        = false;
    const int  DefaultMaxDispatchUnconfirmed = 128;

    const FLogAsyncAppenderDispatchType DefaultAppenderDispatchType     = FLogAsyncAppenderDispatchType.SingleBackgroundThread;
    const AsyncReceiveQueueFullHandling DefaultReceiveQueueFullHandling = AsyncReceiveQueueFullHandling.BackPressureBlock;

    // can have the values FLoggerBuiltinAppenderType 
    FLogAsyncAppenderDispatchType AsyncType { get; }

    bool Broadcast { get; }

    AsyncReceiveQueueFullHandling AsyncQueueFullHandling { get; }

    int MaxDispatchUnconfirmed { get; }

    IFLogEntryAsyncReceiveConfig AsyncReceiveQueue { get; }
}

public interface IMutableAsyncForwardingAppendersConfig : IAsyncForwardingAppendersConfig, IMutableQueueingAppenderConfig
{
    // can have the values FLoggerBuiltinAppenderType 
    new FLogAsyncAppenderDispatchType AsyncType { get; set; }

    new bool Broadcast { get; set; }

    new AsyncReceiveQueueFullHandling AsyncQueueFullHandling { get; set; }

    new int MaxDispatchUnconfirmed { get; set; }

    new IMutableFLogEntryAsyncReceiveConfig AsyncReceiveQueue { get; set; }
}

public class AsyncForwardingAppendersConfig : QueueingAppenderConfig, IMutableAsyncForwardingAppendersConfig
{
    public AsyncForwardingAppendersConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AsyncForwardingAppendersConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AsyncForwardingAppendersConfig
    (string appenderName
      , FLogAsyncAppenderDispatchType asyncType
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null
      , IMutableFLogEntryQueueConfig? inboundQueue = null
      , bool broadcast = IAsyncForwardingAppendersConfig.DefaultShouldBroadcast
      , AsyncReceiveQueueFullHandling asyncQueueFullHandling = IAsyncForwardingAppendersConfig.DefaultReceiveQueueFullHandling
      , IMutableFLogEntryAsyncReceiveConfig? asyncReceiveQueue = null
      , int maxDispatchUnconfirmed = IAsyncForwardingAppendersConfig.DefaultMaxDispatchUnconfirmed
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, asyncType, forwardToAppendersConfig, inboundQueue
             , broadcast, asyncQueueFullHandling, asyncReceiveQueue, runOnAsyncQueueNumber
             , maxDispatchUnconfirmed, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public AsyncForwardingAppendersConfig
    (IConfigurationRoot root, string path, string appenderName
      , FLogAsyncAppenderDispatchType asyncType
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null
      , IMutableFLogEntryQueueConfig? inboundQueue = null
      , bool broadcast = IAsyncForwardingAppendersConfig.DefaultShouldBroadcast
      , AsyncReceiveQueueFullHandling asyncQueueFullHandling = IAsyncForwardingAppendersConfig.DefaultReceiveQueueFullHandling
      , IMutableFLogEntryAsyncReceiveConfig? asyncReceiveQueue = null
      , int maxDispatchUnconfirmed = IAsyncForwardingAppendersConfig.DefaultMaxDispatchUnconfirmed
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, forwardToAppendersConfig, inboundQueue, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        AsyncType = asyncType;
        Broadcast = broadcast;

        AsyncQueueFullHandling = asyncQueueFullHandling;
        AsyncReceiveQueue      = asyncReceiveQueue ?? new FLogEntryAsyncReceiveConfig(IFLogEntryQueueConfig.DefaultQueueSize);
        MaxDispatchUnconfirmed = maxDispatchUnconfirmed;
    }

    public AsyncForwardingAppendersConfig
        (IAsyncForwardingAppendersConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        AsyncType = toClone.AsyncType;
        Broadcast = toClone.Broadcast;

        AsyncQueueFullHandling = toClone.AsyncQueueFullHandling;
        AsyncReceiveQueue      = (IMutableFLogEntryAsyncReceiveConfig)toClone.AsyncReceiveQueue;
        MaxDispatchUnconfirmed = toClone.MaxDispatchUnconfirmed;
    }

    public AsyncForwardingAppendersConfig(IAsyncForwardingAppendersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public AsyncReceiveQueueFullHandling AsyncQueueFullHandling
    {
        get =>
            Enum.TryParse<AsyncReceiveQueueFullHandling>(this[nameof(AsyncQueueFullHandling)], out var fullQueueHandling)
                ? fullQueueHandling
                : IAsyncForwardingAppendersConfig.DefaultReceiveQueueFullHandling;
        set => this[nameof(AsyncQueueFullHandling)] = value.ToString();
    }

    IFLogEntryAsyncReceiveConfig IAsyncForwardingAppendersConfig.AsyncReceiveQueue => AsyncReceiveQueue;

    public IMutableFLogEntryAsyncReceiveConfig AsyncReceiveQueue
    {
        get
        {
            if (GetSection(nameof(AsyncReceiveQueue)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryAsyncReceiveConfig(ConfigRoot, $"{Path}{Split}{nameof(AsyncReceiveQueue)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryAsyncReceiveConfig(IFLogEntryQueueConfig.DefaultQueueSize)
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new FLogEntryAsyncReceiveConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AsyncReceiveQueue)}");

            value.ParentConfig = this;
        }
    }

    public FLogAsyncAppenderDispatchType AsyncType
    {
        get =>
            Enum.TryParse<FLogAsyncAppenderDispatchType>(this[nameof(AsyncType)], out var fullQueueHandling)
                ? fullQueueHandling
                : IAsyncForwardingAppendersConfig.DefaultAppenderDispatchType;
        set => this[nameof(AsyncType)] = value.ToString();
    }

    public bool Broadcast
    {
        get => bool.TryParse(this[nameof(DeactivateHere)], out var disabled) && disabled;
        set => this[nameof(DeactivateHere)] = value.ToString();
    }

    public int MaxDispatchUnconfirmed
    {
        get =>
            int.TryParse(this[nameof(MaxDispatchUnconfirmed)], out var timePart)
                ? timePart
                : IAsyncForwardingAppendersConfig.DefaultMaxDispatchUnconfirmed;
        set => this[nameof(MaxDispatchUnconfirmed)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override AsyncForwardingAppendersConfig Clone() => new(this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IAsyncForwardingAppendersConfig asyncAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var asyncTypeSame   = AsyncType == asyncAppenderConfig.AsyncType;
        var broadcastSame   = Broadcast == asyncAppenderConfig.Broadcast;
        var receiveFullSame = AsyncQueueFullHandling == asyncAppenderConfig.AsyncQueueFullHandling;
        var unconfirmedSame = MaxDispatchUnconfirmed == asyncAppenderConfig.MaxDispatchUnconfirmed;
        var asyncQueueSame  = AsyncReceiveQueue.AreEquivalent(asyncAppenderConfig.AsyncReceiveQueue, exactTypes);

        var allAreSame = baseSame && asyncTypeSame && broadcastSame && receiveFullSame && unconfirmedSame && asyncQueueSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IQueueingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = ForwardToAppenders.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        using var tb = sbc.StartComplexType(nameof(AsyncForwardingAppendersConfig))
           .Field.AlwaysAdd(nameof(AsyncType), AsyncType, FLogAsyncAppenderDispatchTypeExtensions.FLogAsyncAppenderDispatchTypeFormatter)
           .AddBaseFieldsStart();
        base.ToString(sbc);
            tb.Field.AlwaysAdd(nameof(AsyncQueueFullHandling), AsyncQueueFullHandling
                    , AsyncReceiveQueueFullHandlingExtensions.AsyncReceiveQueueFullHandlingFormatter)
            .Field.AlwaysAdd(nameof(Broadcast), Broadcast)
            .Field.AlwaysAdd(nameof(MaxDispatchUnconfirmed), MaxDispatchUnconfirmed)
            .Field.AlwaysAdd(nameof(AsyncReceiveQueue), AsyncReceiveQueue);
        return tb;
    }
}
