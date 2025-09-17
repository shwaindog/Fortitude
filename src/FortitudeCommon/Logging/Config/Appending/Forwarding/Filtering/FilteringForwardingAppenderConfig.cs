// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IFilteringForwardingAppenderConfig : IForwardingAppenderConfig, ICloneable<IFilteringForwardingAppenderConfig>
{
    IMatchOperatorExpressionConfig When { get; }

    new IFilteringForwardingAppenderConfig Clone();
}

public interface IMutableFilteringForwardingAppenderConfig : IFilteringForwardingAppenderConfig, IMutableForwardingAppenderConfig
{
    new IMutableMatchOperatorExpressionConfig When { get; set; }
}

public class FilteringForwardingAppenderConfig : ForwardingAppenderConfig, IMutableFilteringForwardingAppenderConfig
{
    public FilteringForwardingAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FilteringForwardingAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FilteringForwardingAppenderConfig
    (string appenderName, IMutableMatchOperatorExpressionConfig whenCondition
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, whenCondition, forwardToAppendersConfig
             , runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FilteringForwardingAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , IMutableMatchOperatorExpressionConfig whenCondition
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, forwardToAppendersConfig, runOnAsyncQueueNumber, inheritFromAppenderName
             , isTemplateOnlyDefinition, deactivateHere) =>
        When = whenCondition;

    public FilteringForwardingAppenderConfig(IFilteringForwardingAppenderConfig toClone, IConfigurationRoot root, string path) :
        base(toClone, root, path) =>
        When = (IMutableMatchOperatorExpressionConfig)toClone.When;

    public FilteringForwardingAppenderConfig(IFilteringForwardingAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    IMatchOperatorExpressionConfig IFilteringForwardingAppenderConfig.When => When;

    public IMutableMatchOperatorExpressionConfig When
    {
        get
        {
            if (GetSection(nameof(When)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(When)}")
                {
                    ParentConfig = this
                };
            return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(When)}")
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(When)}");

            value.ParentConfig = this;
        }
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IFilteringForwardingAppenderConfig IFilteringForwardingAppenderConfig.Clone() => Clone();

    IFilteringForwardingAppenderConfig ICloneable<IFilteringForwardingAppenderConfig>.Clone() => Clone();

    public override FilteringForwardingAppenderConfig Clone() => new(this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IFilteringForwardingAppenderConfig filteringAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var inboundQueueSame = When.AreEquivalent(filteringAppender.When, exactTypes);

        var allAreSame = baseSame && inboundQueueSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFilteringForwardingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();

        return hashCode;
    }

    public override StateExtractStringRange RevealState(ITheOneString stsa) =>
        stsa.StartComplexType(this)
           .AddBaseStyledToStringFields(this)
           .Field.AlwaysAdd(nameof(When), When)
           .Complete();
}
