// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IBufferingAppenderConfig : IQueueingAppenderConfig
  , IConfigCloneTo<IBufferingAppenderConfig>
{
    const int DefaultBufferTimeMs = 2_000;

    const FLogLevel DefaultFlushLogLevel = FLogLevel.Error;

    int MaxBufferTimeMs { get; }

    FLogLevel FlushLogLevel { get; }

    new IBufferingAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    new IBufferingAppenderConfig Clone();
}

public interface IMutableBufferingAppenderConfig : IBufferingAppenderConfig, IMutableQueueingAppenderConfig
{
    new int MaxBufferTimeMs { get; set; }

    new FLogLevel FlushLogLevel { get; set; }
}

public class BufferingAppenderConfig : QueueingAppenderConfig, IMutableBufferingAppenderConfig
{
    public BufferingAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public BufferingAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public BufferingAppenderConfig
    (string appenderName
      , IAppendableForwardingAppendersLookupConfig? forwardToAppendersConfig = null
      , IMutableFLogEntryQueueConfig? inboundQueue = null
      , int maxBufferTimeMs = IBufferingAppenderConfig.DefaultBufferTimeMs
      , FLogLevel flushLogLevel = IBufferingAppenderConfig.DefaultFlushLogLevel
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, forwardToAppendersConfig, inboundQueue, maxBufferTimeMs
             , flushLogLevel, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public BufferingAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , IAppendableForwardingAppendersLookupConfig? forwardToAppendersConfig = null
      , IMutableFLogEntryQueueConfig? inboundQueue = null
      , int maxBufferTimeMs = IBufferingAppenderConfig.DefaultBufferTimeMs
      , FLogLevel flushLogLevel = IBufferingAppenderConfig.DefaultFlushLogLevel
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, forwardToAppendersConfig, inboundQueue, runOnAsyncQueueNumber, inheritFromAppenderName
             , isTemplateOnlyDefinition, deactivateHere)
    {
        MaxBufferTimeMs = maxBufferTimeMs;
        FlushLogLevel   = flushLogLevel;
    }

    public BufferingAppenderConfig(IBufferingAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MaxBufferTimeMs = toClone.MaxBufferTimeMs;
        FlushLogLevel   = toClone.FlushLogLevel;
    }

    public BufferingAppenderConfig(IBufferingAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLogLevel FlushLogLevel
    {
        get =>
            Enum.TryParse<FLogLevel>(this[nameof(FlushLogLevel)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : FLogLevel.None;
        set => this[nameof(FlushLogLevel)] = value.ToString();
    }

    public int MaxBufferTimeMs
    {
        get => int.TryParse(this[nameof(MaxBufferTimeMs)], out var timePart) ? timePart : 0;
        set => this[nameof(MaxBufferTimeMs)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override BufferingAppenderConfig Clone() => new(this);

    IBufferingAppenderConfig ICloneable<IBufferingAppenderConfig>.Clone() => Clone();

    IBufferingAppenderConfig IBufferingAppenderConfig.Clone() => Clone();

    IBufferingAppenderConfig IConfigCloneTo<IBufferingAppenderConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    IBufferingAppenderConfig IBufferingAppenderConfig.CloneConfigTo
        (IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    public override BufferingAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new(this, configRoot, path);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IBufferingAppenderConfig bufferingAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var bufferTimeSame = MaxBufferTimeMs == bufferingAppenderConfig.MaxBufferTimeMs;
        var flushLevelSame = FlushLogLevel == bufferingAppenderConfig.FlushLogLevel;

        var allAreSame = baseSame && bufferTimeSame && flushLevelSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IQueueingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ (int)FlushLogLevel;
        hashCode = (hashCode * 397) ^ MaxBufferTimeMs;
        return hashCode;
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(BufferingAppenderConfig))
           .AddTypeStart()
           .AddBaseFieldsStart();
        base.ToString(sbc)
            .AddBaseFieldsEnd()
            .AddField(nameof(MaxBufferTimeMs), MaxBufferTimeMs)
            .AddField(nameof(FlushLogLevel), FlushLogLevel, FLogLevelExtensions.FLogLevelFormatter)
            .AddTypeEnd();
        return sbc;
    }
}
