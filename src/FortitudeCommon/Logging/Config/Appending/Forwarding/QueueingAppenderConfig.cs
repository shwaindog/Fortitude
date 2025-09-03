// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IQueueingAppenderConfig : IForwardingAppenderConfig
{
    IFLogEntryQueueConfig InboundQueue { get; }

    new IQueueingAppenderConfig Clone();
}

public interface IMutableQueueingAppenderConfig : IQueueingAppenderConfig, IMutableForwardingAppenderConfig
{
    new IMutableFLogEntryQueueConfig InboundQueue { get; set; }
}

public class QueueingAppenderConfig : ForwardingAppenderConfig, IMutableQueueingAppenderConfig
{
    public QueueingAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public QueueingAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public QueueingAppenderConfig
    (string appenderName
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null, IMutableFLogEntryQueueConfig? inboundQueue = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, forwardToAppendersConfig, inboundQueue
             , runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public QueueingAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null
      , IMutableFLogEntryQueueConfig? inboundQueue = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, forwardToAppendersConfig, runOnAsyncQueueNumber, inheritFromAppenderName
             , isTemplateOnlyDefinition, deactivateHere) =>
        InboundQueue = inboundQueue ?? new FLogEntryQueueConfig(IFLogEntryQueueConfig.DefaultQueueSize);

    public QueueingAppenderConfig(IQueueingAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) =>
        InboundQueue = (IMutableFLogEntryQueueConfig)toClone.InboundQueue;

    public QueueingAppenderConfig(IQueueingAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    IFLogEntryQueueConfig IQueueingAppenderConfig.InboundQueue => InboundQueue;

    public IMutableFLogEntryQueueConfig InboundQueue
    {
        get
        {
            if (GetSection(nameof(InboundQueue)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new FLogEntryQueueConfig(ConfigRoot, $"{Path}{Split}{nameof(InboundQueue)}")
                {
                    ParentConfig = this
                };
            return new FLogEntryQueueConfig(IFLogEntryQueueConfig.DefaultQueueSize)
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new FLogEntryQueueConfig(value, ConfigRoot, $"{Path}{Split}{nameof(InboundQueue)}");

            value.ParentConfig = this;
        }
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IQueueingAppenderConfig IQueueingAppenderConfig.Clone() => Clone();

    public override QueueingAppenderConfig Clone() => new(this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IQueueingAppenderConfig queueingAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var inboundQueueSame = InboundQueue.AreEquivalent(queueingAppenderConfig.InboundQueue, exactTypes);

        var allAreSame = baseSame && inboundQueueSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IQueueingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ InboundQueue.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .AddBaseStyledToStringFields(this).Complete();
}
