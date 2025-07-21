// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IFilterToAppenderConfig : IForwardingAppenderConfig, ICloneable<IFilterToAppenderConfig>
{
    IMatchOperatorExpressionConfig When { get; }

    new IFilterToAppenderConfig Clone();
}

public interface IMutableFilterToAppenderConfig : IFilterToAppenderConfig, IMutableForwardingAppenderConfig
{
    new IMutableMatchOperatorExpressionConfig When { get; set; }
}

public class FilterToAppenderConfig : ForwardingAppenderConfig, IMutableFilterToAppenderConfig
{
    public FilterToAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FilterToAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FilterToAppenderConfig
    (string appenderName, IMutableMatchOperatorExpressionConfig whenCondition
      , IAppendableForwardingAppendersLookupConfig? forwardToAppendersConfig = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, whenCondition, forwardToAppendersConfig
             , runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FilterToAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , IMutableMatchOperatorExpressionConfig whenCondition
      , IAppendableForwardingAppendersLookupConfig? forwardToAppendersConfig = null
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, forwardToAppendersConfig, runOnAsyncQueueNumber, inheritFromAppenderName
             , isTemplateOnlyDefinition, deactivateHere)
    {
        When = whenCondition;
    }

    public FilterToAppenderConfig(IFilterToAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        When = (IMutableMatchOperatorExpressionConfig)toClone.When;
    }

    public FilterToAppenderConfig(IFilterToAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    IMatchOperatorExpressionConfig IFilterToAppenderConfig.When => When;

    public IMutableMatchOperatorExpressionConfig When
    {
        get
        {
            if (GetSection(nameof(When)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(When)}")
                {
                    ParentConfig = this
                };
            }
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

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IFilterToAppenderConfig IFilterToAppenderConfig.Clone() => Clone();

    IFilterToAppenderConfig ICloneable<IFilterToAppenderConfig>.Clone() => Clone();

    public override FilterToAppenderConfig Clone() => new(this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IFilterToAppenderConfig filteringAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var inboundQueueSame = When.AreEquivalent(filteringAppender.When, exactTypes);

        var allAreSame = baseSame && inboundQueueSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFilterToAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();

        return hashCode;
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(FilterToAppenderConfig))
           .AddTypeStart()
           .AddBaseFieldsStart();
        return base.ToString(sbc)
                   .AddBaseFieldsEnd()
                   .AddField(nameof(When), When)
                   .AddTypeEnd();
    }
}
