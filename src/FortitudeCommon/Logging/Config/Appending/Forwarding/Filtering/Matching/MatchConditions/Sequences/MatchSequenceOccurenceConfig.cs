// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface IMatchSequenceOccurenceConfig : IMatchConditionConfig, IConfigCloneTo<IMatchSequenceOccurenceConfig>
{
    IMatchSequenceTriggerConfig StartSequence { get; }

    ISequenceHandleActionConfig OnSequenceComplete { get; }

    ISequenceHandleActionConfig OnSequenceAbort { get; }

    ISequenceHandleActionConfig OnSequenceTimeout { get; }

    new IMatchSequenceOccurenceConfig Clone();

    new IMatchSequenceOccurenceConfig CloneConfigTo(IConfigurationRoot configRoot, string path);
}

public interface IMutableMatchSequenceOccurenceConfig : IMatchSequenceOccurenceConfig, IMutableMatchConditionConfig
{
    new IMutableMatchSequenceTriggerConfig StartSequence { get; set; }

    new IMutableSequenceHandleActionConfig OnSequenceComplete { get; set; }

    new IMutableSequenceHandleActionConfig OnSequenceAbort { get; set; }

    new IMutableSequenceHandleActionConfig OnSequenceTimeout { get; set; }
}

public class MatchSequenceOccurenceConfig : MatchConditionConfig, IMutableMatchSequenceOccurenceConfig
{
    public MatchSequenceOccurenceConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MatchSequenceOccurenceConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchSequenceOccurenceConfig
    (IMutableSequenceHandleActionConfig? onSequenceComplete = null
      , IMutableSequenceHandleActionConfig? onSequenceAbort = null
      , IMutableSequenceHandleActionConfig? onSequenceTimeout = null
      , IMutableMatchSequenceTriggerConfig? sendTriggeringLogEntries = null)
        : this(InMemoryConfigRoot, InMemoryPath, onSequenceComplete, onSequenceAbort, onSequenceTimeout, sendTriggeringLogEntries) { }

    public MatchSequenceOccurenceConfig
    (IConfigurationRoot root, string path, IMutableSequenceHandleActionConfig? onSequenceComplete = null
      , IMutableSequenceHandleActionConfig? onSequenceAbort = null
      , IMutableSequenceHandleActionConfig? onSequenceTimeout = null
      , IMutableMatchSequenceTriggerConfig? sendTriggeringLogEntries = null)
        : base(root, path)
    {
        OnSequenceComplete = onSequenceComplete ??
                             new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceComplete)}");

        OnSequenceAbort = onSequenceAbort ??
                          new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceAbort)}");
        
        OnSequenceTimeout = onSequenceTimeout ??
                            new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceTimeout)}");

        StartSequence = sendTriggeringLogEntries ??
                        new MatchSequenceTriggerConfig(ConfigRoot, $"{Path}{Split}{nameof(StartSequence)}");
    }

    public MatchSequenceOccurenceConfig(IMatchSequenceOccurenceConfig toClone, IConfigurationRoot root, string path)
        : base(root, path, FLoggerEntryMatchType.MessageSequenceCompletes)
    {
        OnSequenceComplete = (IMutableSequenceHandleActionConfig)toClone.OnSequenceComplete;
        OnSequenceAbort    = (IMutableSequenceHandleActionConfig)toClone.OnSequenceAbort;
        OnSequenceTimeout  = (IMutableSequenceHandleActionConfig)toClone.OnSequenceTimeout;
        StartSequence      = (IMutableMatchSequenceTriggerConfig)toClone.StartSequence;
    }

    public MatchSequenceOccurenceConfig(IMatchSequenceOccurenceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    ISequenceHandleActionConfig IMatchSequenceOccurenceConfig.OnSequenceComplete => OnSequenceComplete;

    public IMutableSequenceHandleActionConfig OnSequenceComplete
    {
        get => new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceComplete)}");
        set => _ = new SequenceHandleActionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OnSequenceComplete)}");
    }

    ISequenceHandleActionConfig IMatchSequenceOccurenceConfig.OnSequenceAbort => OnSequenceAbort;

    public IMutableSequenceHandleActionConfig OnSequenceAbort
    {
        get
        {
            if (GetSection(nameof(OnSequenceAbort)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceAbort)}")
                {
                    ParentConfig = this
                };
            }
            return new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceAbort)}",
                                                  sendTriggeringLogEntries: TriggeringLogEntries.None)
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new SequenceHandleActionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OnSequenceAbort)}");

            value.ParentConfig = this;
        }
    }

    ISequenceHandleActionConfig IMatchSequenceOccurenceConfig.OnSequenceTimeout => OnSequenceTimeout;

    public IMutableSequenceHandleActionConfig OnSequenceTimeout
    {
        get
        {
            if (GetSection(nameof(OnSequenceTimeout)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceTimeout)}")
                {
                    ParentConfig = this
                };
            }
            return new SequenceHandleActionConfig(ConfigRoot, $"{Path}{Split}{nameof(OnSequenceTimeout)}",
                                                  sendTriggeringLogEntries: TriggeringLogEntries.None)
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new SequenceHandleActionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OnSequenceTimeout)}");

            
            value.ParentConfig = this;
        }
    }

    IMatchSequenceTriggerConfig IMatchSequenceOccurenceConfig.StartSequence => StartSequence;

    public IMutableMatchSequenceTriggerConfig StartSequence
    {
        get => new MatchSequenceTriggerConfig(ConfigRoot, $"{Path}{Split}{nameof(StartSequence)}");
        set => _ = new MatchSequenceTriggerConfig(value, ConfigRoot, $"{Path}{Split}{nameof(StartSequence)}");
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IMatchConditionConfig ICloneable<IMatchConditionConfig>.Clone() => Clone();

    IMatchConditionConfig IConfigCloneTo<IMatchConditionConfig>.CloneConfigTo
        (IConfigurationRoot configRoot, string path) =>
        new MatchSequenceOccurenceConfig(this, configRoot, path);

    IMatchSequenceOccurenceConfig ICloneable<IMatchSequenceOccurenceConfig>.Clone() => Clone();

    IMatchSequenceOccurenceConfig IMatchSequenceOccurenceConfig.Clone() => Clone();

    public override MatchSequenceOccurenceConfig Clone() => new(this);

    IMatchSequenceOccurenceConfig IConfigCloneTo<IMatchSequenceOccurenceConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    IMatchSequenceOccurenceConfig IMatchSequenceOccurenceConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public override MatchSequenceOccurenceConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override bool AreEquivalent(IMatchConditionConfig? other, bool exactTypes = false)
    {
        if (other is not IMatchSequenceOccurenceConfig sequenceOccurence) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var seqCompleteSame  = OnSequenceComplete.AreEquivalent(sequenceOccurence.OnSequenceComplete, exactTypes);
        var seqAbortSame     = OnSequenceAbort.AreEquivalent(sequenceOccurence.OnSequenceAbort, exactTypes);
        var seqTimeoutSame   = OnSequenceTimeout.AreEquivalent(sequenceOccurence.OnSequenceTimeout, exactTypes);
        var sendAppenderSame = StartSequence.AreEquivalent(sequenceOccurence.StartSequence, exactTypes);

        var allAreSame = baseSame && seqCompleteSame && seqAbortSame && seqTimeoutSame && sendAppenderSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchSequenceOccurenceConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ OnSequenceComplete.GetHashCode();
        hashCode = (hashCode * 397) ^ OnSequenceAbort.GetHashCode();
        hashCode = (hashCode * 397) ^ OnSequenceTimeout.GetHashCode();
        hashCode = (hashCode * 397) ^ StartSequence.GetHashCode();
        return hashCode;
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(MatchSequenceOccurenceConfig))
               .AddTypeStart()
               .AddField(nameof(CheckConditionType), CheckConditionType, FLoggerEntryMatchTypeExtensions.FLoggerEntryMatchTypeFormatter)
               .AddField(nameof(OnSequenceAbort), OnSequenceAbort)
               .AddField(nameof(OnSequenceTimeout), OnSequenceTimeout)
               .AddField(nameof(StartSequence), StartSequence)
               .AddTypeEnd();
    }
}
