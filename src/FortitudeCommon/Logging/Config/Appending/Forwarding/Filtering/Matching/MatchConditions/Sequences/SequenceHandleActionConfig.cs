// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface ISequenceHandleActionConfig : IFLogConfig, IConfigCloneTo<ISequenceHandleActionConfig>
  , IInterfacesComparable<ISequenceHandleActionConfig>, IStringBearer
{
    ILogMessageTemplateConfig? SendMessage { get; }

    IAppenderReferenceConfig? SendToAppender { get; }

    TriggeringLogEntries SendTriggeringLogEntries { get; }
}

public interface IMutableSequenceHandleActionConfig : ISequenceHandleActionConfig, IMutableFLogConfig
{
    new IMutableLogMessageTemplateConfig? SendMessage { get; set; }

    new IAppenderReferenceConfig? SendToAppender { get; set; }

    new TriggeringLogEntries SendTriggeringLogEntries { get; set; }
}

public class SequenceHandleActionConfig : FLogConfig, IMutableSequenceHandleActionConfig
{
    public SequenceHandleActionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public SequenceHandleActionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public SequenceHandleActionConfig
    (IMutableLogMessageTemplateConfig? sendMessage = null, IAppenderReferenceConfig? sendToAppender = null
      , TriggeringLogEntries sendTriggeringLogEntries = TriggeringLogEntries.All)
        : this(InMemoryConfigRoot, InMemoryPath, sendMessage, sendToAppender, sendTriggeringLogEntries) { }

    public SequenceHandleActionConfig
    (IConfigurationRoot root, string path, IMutableLogMessageTemplateConfig? sendMessage = null, IAppenderReferenceConfig? sendToAppender = null
      , TriggeringLogEntries sendTriggeringLogEntries = TriggeringLogEntries.All)
        : base(root, path)
    {
        SendMessage              = sendMessage;
        SendToAppender           = sendToAppender;
        SendTriggeringLogEntries = sendTriggeringLogEntries;
    }

    public SequenceHandleActionConfig(ISequenceHandleActionConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        SendMessage              = toClone.SendMessage as IMutableLogMessageTemplateConfig;
        SendToAppender           = toClone.SendToAppender;
        SendTriggeringLogEntries = toClone.SendTriggeringLogEntries;
    }

    public SequenceHandleActionConfig(ISequenceHandleActionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    ILogMessageTemplateConfig? ISequenceHandleActionConfig.SendMessage => SendMessage;

    public IMutableLogMessageTemplateConfig? SendMessage
    {
        get
        {
            if (GetSection(nameof(SendMessage)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new LogMessageTemplateConfig(ConfigRoot, $"{Path}{Split}{nameof(SendMessage)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new LogMessageTemplateConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SendMessage)}");

                value.ParentConfig = this;
            }
        }
    }

    public IAppenderReferenceConfig? SendToAppender
    {
        get
        {
            if (GetSection(nameof(SendToAppender)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new AppenderReferenceConfig(ConfigRoot, $"{Path}{Split}{nameof(SendToAppender)}");
            return null;
        }
        set =>
            _ = value != null
                ? new AppenderReferenceConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SendToAppender)}")
                : null;
    }

    public TriggeringLogEntries SendTriggeringLogEntries
    {
        get =>
            Enum.TryParse<TriggeringLogEntries>(this[nameof(SendTriggeringLogEntries)], out var logLevel)
                ? logLevel
                : TriggeringLogEntries.All;
        set => this[nameof(SendTriggeringLogEntries)] = value.ToString();
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    ISequenceHandleActionConfig IConfigCloneTo<ISequenceHandleActionConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public SequenceHandleActionConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    ISequenceHandleActionConfig ICloneable<ISequenceHandleActionConfig>.Clone() => Clone();

    public virtual SequenceHandleActionConfig Clone() => new(this);

    public virtual bool AreEquivalent(ISequenceHandleActionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var sendMessageSame         = SendMessage?.AreEquivalent(other.SendMessage, exactTypes) ?? other.SendMessage == null;
        var sendAppenderSame        = SendToAppender?.AreEquivalent(other.SendToAppender, exactTypes) ?? other.SendToAppender == null;
        var sendTriggerMessagesSame = SendTriggeringLogEntries == other.SendTriggeringLogEntries;

        var allAreSame = sendMessageSame && sendAppenderSame && sendTriggerMessagesSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISequenceHandleActionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = SendMessage?.GetHashCode() ?? 0;
        hashCode = (hashCode * 397) ^ (SendToAppender?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (int)SendTriggeringLogEntries;
        return hashCode;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddStyled(nameof(SendMessage), SendMessage)
           .Field.WhenNonNullAddStyled(nameof(SendToAppender), SendToAppender)
           .Field.AlwaysAdd(nameof(SendTriggeringLogEntries), SendTriggeringLogEntries)
           .Complete();
}
