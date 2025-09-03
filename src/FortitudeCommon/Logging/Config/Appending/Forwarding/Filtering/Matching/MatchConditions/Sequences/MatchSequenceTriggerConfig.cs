// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface IMatchSequenceTriggerConfig : IFLogConfig, IConfigCloneTo<IMatchSequenceTriggerConfig>
  , IInterfacesComparable<IMatchSequenceTriggerConfig>
  , IStyledToStringObject
{
    IMatchOperatorExpressionConfig? TriggeredWhenEntry { get; } // If triggered

    IExtractedMessageKeyValuesConfig? OnTriggerExtract { get; } // next step is to extract values

    IMatchOperatorExpressionConfig? AbortWhenEntry { get; } // next check if sequence should abort

    IMatchOperatorExpressionConfig? CompletedWhenEntry { get; } // next check if this triggerred instance should complete

    IMatchSequenceTriggerConfig? NextTriggerStep { get; } // extracted keys are available to chained 

    IMatchSequenceTriggerConfig? SequenceFinalTrigger { get; }

    ITimeSpanConfig? TimeOut { get; } // On StartSequence this is time it is triggered.  For chained NextTriggerStep sequence
}

public interface IMutableMatchSequenceTriggerConfig : IMatchSequenceTriggerConfig, IMutableFLogConfig
{
    new IMutableMatchOperatorExpressionConfig? TriggeredWhenEntry { get; set; } // If triggered

    new IAppendableExtractedMessageKeyValuesConfig? OnTriggerExtract { get; set; } // next step is to extract values

    new IMutableMatchOperatorExpressionConfig? AbortWhenEntry { get; set; } // next check if sequence should abort

    new IMutableMatchOperatorExpressionConfig? CompletedWhenEntry { get; set; } // next check if this triggerred instance should complete

    new IMutableMatchSequenceTriggerConfig? NextTriggerStep { get; set; } // extracted keys are available to chained 

    new IMutableMatchSequenceTriggerConfig? SequenceFinalTrigger { get; set; }

    new ITimeSpanConfig? TimeOut { get; set; } // On StartSequence this is time it is triggered.  For chained NextTriggerStep sequence
}

public class MatchSequenceTriggerConfig : FLogConfig, IMutableMatchSequenceTriggerConfig
{
    public MatchSequenceTriggerConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MatchSequenceTriggerConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchSequenceTriggerConfig
    (IMutableMatchOperatorExpressionConfig? triggeredWhenEntry, IMutableMatchSequenceTriggerConfig? nextTriggerStep = null
      , IAppendableExtractedMessageKeyValuesConfig? onTriggerExtract = null, IMutableMatchOperatorExpressionConfig? completedWhenEntry = null
      , IMutableMatchOperatorExpressionConfig? abortWhenEntry = null, IMutableMatchSequenceTriggerConfig? sequenceFinalTrigger = null
      , ITimeSpanConfig? timeOut = null)
        : this(InMemoryConfigRoot, InMemoryPath, triggeredWhenEntry, nextTriggerStep, onTriggerExtract, completedWhenEntry
             , abortWhenEntry, sequenceFinalTrigger, timeOut) { }

    public MatchSequenceTriggerConfig
    (IConfigurationRoot root, string path, IMutableMatchOperatorExpressionConfig? triggeredWhenEntry
      , IMutableMatchSequenceTriggerConfig? nextTriggerStep = null
      , IAppendableExtractedMessageKeyValuesConfig? onTriggerExtract = null, IMutableMatchOperatorExpressionConfig? completedWhenEntry = null
      , IMutableMatchOperatorExpressionConfig? abortWhenEntry = null, IMutableMatchSequenceTriggerConfig? sequenceFinalTrigger = null
      , ITimeSpanConfig? timeOut = null)
        : base(root, path)
    {
        TriggeredWhenEntry   = triggeredWhenEntry;
        NextTriggerStep      = nextTriggerStep;
        OnTriggerExtract     = onTriggerExtract;
        CompletedWhenEntry   = completedWhenEntry;
        AbortWhenEntry       = abortWhenEntry;
        SequenceFinalTrigger = sequenceFinalTrigger;

        TimeOut = timeOut;
    }

    public MatchSequenceTriggerConfig(IMatchSequenceTriggerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TriggeredWhenEntry   = toClone.TriggeredWhenEntry as IMutableMatchOperatorExpressionConfig;
        CompletedWhenEntry   = toClone.CompletedWhenEntry as IMutableMatchOperatorExpressionConfig;
        AbortWhenEntry       = toClone.AbortWhenEntry as IMutableMatchOperatorExpressionConfig;
        NextTriggerStep      = toClone.NextTriggerStep as IMutableMatchSequenceTriggerConfig;
        OnTriggerExtract     = toClone.OnTriggerExtract as IAppendableExtractedMessageKeyValuesConfig;
        SequenceFinalTrigger = toClone.SequenceFinalTrigger as IMutableMatchSequenceTriggerConfig;

        TimeOut = toClone.TimeOut;
    }

    public MatchSequenceTriggerConfig(IMatchSequenceTriggerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    IMatchOperatorExpressionConfig? IMatchSequenceTriggerConfig.TriggeredWhenEntry => TriggeredWhenEntry;

    public IMutableMatchOperatorExpressionConfig? TriggeredWhenEntry
    {
        get
        {
            if (GetSection(nameof(TriggeredWhenEntry)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(TriggeredWhenEntry)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TriggeredWhenEntry)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchOperatorExpressionConfig? IMatchSequenceTriggerConfig.AbortWhenEntry => AbortWhenEntry;

    public IMutableMatchOperatorExpressionConfig? AbortWhenEntry
    {
        get
        {
            if (GetSection(nameof(AbortWhenEntry)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(AbortWhenEntry)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AbortWhenEntry)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchOperatorExpressionConfig? IMatchSequenceTriggerConfig.CompletedWhenEntry => CompletedWhenEntry;

    public IMutableMatchOperatorExpressionConfig? CompletedWhenEntry
    {
        get
        {
            if (GetSection(nameof(CompletedWhenEntry)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(CompletedWhenEntry)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(CompletedWhenEntry)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchSequenceTriggerConfig? IMatchSequenceTriggerConfig.NextTriggerStep => NextTriggerStep;

    public IMutableMatchSequenceTriggerConfig? NextTriggerStep
    {
        get
        {
            if (GetSection(nameof(NextTriggerStep)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchSequenceTriggerConfig(ConfigRoot, $"{Path}{Split}{nameof(NextTriggerStep)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchSequenceTriggerConfig(value, ConfigRoot, $"{Path}{Split}{nameof(NextTriggerStep)}");

                value.ParentConfig = this;
            }
        }
    }

    IExtractedMessageKeyValuesConfig? IMatchSequenceTriggerConfig.OnTriggerExtract => OnTriggerExtract;

    public IAppendableExtractedMessageKeyValuesConfig? OnTriggerExtract
    {
        get
        {
            if (GetSection(nameof(OnTriggerExtract)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new ExtractedMessageKeyValuesConfig(ConfigRoot, $"{Path}{Split}{nameof(OnTriggerExtract)}")
                {
                    ParentConfig = this
                };
            return new ExtractedMessageKeyValuesConfig(ConfigRoot, $"{Path}{Split}{nameof(OnTriggerExtract)}")
            {
                ParentConfig = this
            };
        }
        set
        {
            if (value != null)
            {
                _ = new ExtractedMessageKeyValuesConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OnTriggerExtract)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchSequenceTriggerConfig? IMatchSequenceTriggerConfig.SequenceFinalTrigger => SequenceFinalTrigger;

    public IMutableMatchSequenceTriggerConfig? SequenceFinalTrigger
    {
        get
        {
            if (GetSection(nameof(SequenceFinalTrigger)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchSequenceTriggerConfig(ConfigRoot, $"{Path}{Split}{nameof(SequenceFinalTrigger)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchSequenceTriggerConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SequenceFinalTrigger)}");

                value.ParentConfig = this;
            }
        }
    }

    public ITimeSpanConfig? TimeOut
    {
        get
        {
            if (GetSection(nameof(TimeOut)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(TimeOut)}");
            return null;
        }
        set =>
            _ = value != null
                ? new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TimeOut)}")
                : null;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IMatchSequenceTriggerConfig IConfigCloneTo<IMatchSequenceTriggerConfig>.CloneConfigTo
        (IConfigurationRoot configRoot, string path) =>
        new MatchSequenceTriggerConfig(this, configRoot, path);

    public MatchSequenceTriggerConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IMatchSequenceTriggerConfig ICloneable<IMatchSequenceTriggerConfig>.Clone() => Clone();

    public virtual MatchSequenceTriggerConfig Clone() => new(this);

    public virtual bool AreEquivalent(IMatchSequenceTriggerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var triggeredWhenSame  = TriggeredWhenEntry?.AreEquivalent(other.TriggeredWhenEntry, exactTypes) ?? other.TriggeredWhenEntry == null;
        var nextTriggerSame    = NextTriggerStep?.AreEquivalent(other.NextTriggerStep, exactTypes) ?? other.NextTriggerStep == null;
        var triggerExtractSame = OnTriggerExtract?.AreEquivalent(other.OnTriggerExtract, exactTypes) ?? other.OnTriggerExtract == null;
        var completedWhenSame  = CompletedWhenEntry?.AreEquivalent(other.CompletedWhenEntry, exactTypes) ?? other.CompletedWhenEntry == null;
        var abortWhenSame      = AbortWhenEntry?.AreEquivalent(other.AbortWhenEntry, exactTypes) ?? other.AbortWhenEntry == null;
        var sequenceFinalSame  = SequenceFinalTrigger?.AreEquivalent(other.SequenceFinalTrigger, exactTypes) ?? other.SequenceFinalTrigger == null;
        var timeoutSame        = TimeOut?.AreEquivalent(other.TimeOut, exactTypes) ?? other.TimeOut == null;

        var allAreSame = triggeredWhenSame && nextTriggerSame && triggerExtractSame && completedWhenSame
                      && abortWhenSame && sequenceFinalSame && timeoutSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchSequenceTriggerConfig, true);

    public override int GetHashCode()
    {
        var hashCode = TriggeredWhenEntry?.GetHashCode() ?? 0;
        hashCode = (hashCode * 397) ^ (NextTriggerStep?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (OnTriggerExtract?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (CompletedWhenEntry?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (AbortWhenEntry?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (SequenceFinalTrigger?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (TimeOut?.GetHashCode() ?? 0);
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.WhenNonNullAddStyled(nameof(TriggeredWhenEntry), TriggeredWhenEntry)
           .Field.WhenNonNullAddStyled(nameof(NextTriggerStep), NextTriggerStep)
           .Field.WhenNonNullAddStyled(nameof(OnTriggerExtract), OnTriggerExtract)
           .Field.WhenNonNullAddStyled(nameof(CompletedWhenEntry), CompletedWhenEntry)
           .Field.WhenNonNullAddStyled(nameof(AbortWhenEntry), AbortWhenEntry)
           .Field.WhenNonNullAddStyled(nameof(TimeOut), TimeOut)
           .Complete();
}
