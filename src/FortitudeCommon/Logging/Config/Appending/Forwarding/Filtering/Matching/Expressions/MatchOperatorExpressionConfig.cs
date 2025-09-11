// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;

public interface IMatchOperatorExpressionConfig : IFLogConfig, IConfigCloneTo<IMatchOperatorExpressionConfig>
  , IInterfacesComparable<IMatchOperatorExpressionConfig>, IStyledToStringObject
{
    ushort EvaluateOrder { get; }

    IMatchOperatorCollectionConfig? Any { get; }

    IMatchOperatorCollectionConfig? All { get; }

    IMatchOperatorExpressionConfig? And { get; }

    IMatchOperatorExpressionConfig? Or { get; }

    IMatchConditionConfig? IsTrue { get; }

    IMatchConditionConfig? IsFalse { get; }
}

public interface IMutableMatchOperatorExpressionConfig : IMatchOperatorExpressionConfig, IMutableFLogConfig
{
    new ushort EvaluateOrder { get; set; }

    new IAppendableMatchOperatorLookupConfig? Any { get; set; }

    new IAppendableMatchOperatorLookupConfig? All { get; set; }

    new IMutableMatchOperatorExpressionConfig? And { get; set; }

    new IMutableMatchOperatorExpressionConfig? Or { get; set; }

    new IMutableMatchConditionConfig? IsTrue { get; set; }

    new IMutableMatchConditionConfig? IsFalse { get; set; }
}

public class MatchOperatorExpressionConfig : FLogConfig, IMutableMatchOperatorExpressionConfig
{
    public MatchOperatorExpressionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MatchOperatorExpressionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchOperatorExpressionConfig
    (ushort evalOrder, IAppendableMatchOperatorLookupConfig? all = null, IAppendableMatchOperatorLookupConfig? any = null
      , IMutableMatchOperatorExpressionConfig? and = null, IMutableMatchOperatorExpressionConfig? or = null
      , IMutableMatchConditionConfig? isTrueEval = null, IMutableMatchConditionConfig? isFalseEval = null)
        : this(InMemoryConfigRoot, InMemoryPath, evalOrder, all, any, and, or, isTrueEval, isFalseEval) { }

    public MatchOperatorExpressionConfig
    (IConfigurationRoot root, string path, ushort evalOrder, IAppendableMatchOperatorLookupConfig? all = null
      , IAppendableMatchOperatorLookupConfig? any = null, IMutableMatchOperatorExpressionConfig? and = null
      , IMutableMatchOperatorExpressionConfig? or = null, IMutableMatchConditionConfig? isTrueEval = null
      , IMutableMatchConditionConfig? isFalseEval = null)
        : base(root, path)
    {
        EvaluateOrder = evalOrder;

        All = all;
        Any = any;
        And = and;
        Or  = or;

        IsFalse = isFalseEval;
        IsTrue  = isTrueEval;
    }

    public MatchOperatorExpressionConfig(IMatchOperatorExpressionConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        All = toClone.All as IAppendableMatchOperatorLookupConfig;
        Any = toClone.Any as IAppendableMatchOperatorLookupConfig;

        And = toClone.And as IMutableMatchOperatorExpressionConfig;
        Or  = toClone.Or as IMutableMatchOperatorExpressionConfig;

        IsFalse = toClone.IsFalse as IMutableMatchConditionConfig;
        IsTrue  = toClone.IsTrue as IMutableMatchConditionConfig;

        EvaluateOrder = toClone.EvaluateOrder;
    }

    public MatchOperatorExpressionConfig(IMatchOperatorExpressionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public ushort EvaluateOrder
    {
        get =>
            ushort.TryParse(this[nameof(EvaluateOrder)], out var evalOrder)
                ? evalOrder
                : (ushort)0;
        set => this[nameof(EvaluateOrder)] = value.ToString();
    }

    IMatchOperatorCollectionConfig? IMatchOperatorExpressionConfig.All => All;

    public IAppendableMatchOperatorLookupConfig? All
    {
        get
        {
            if (GetSection(nameof(All)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new AppendableMatchOperatorLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(All)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new AppendableMatchOperatorLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(All)}");

                value.ParentConfig = this;
            }
        }
    }
    IMatchOperatorCollectionConfig? IMatchOperatorExpressionConfig.Any => Any;

    public IAppendableMatchOperatorLookupConfig? Any
    {
        get
        {
            if (GetSection(nameof(Any)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new AppendableMatchOperatorLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(Any)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new AppendableMatchOperatorLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Any)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchOperatorExpressionConfig? IMatchOperatorExpressionConfig.And => And;

    public IMutableMatchOperatorExpressionConfig? And
    {
        get
        {
            if (GetSection(nameof(And)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(And)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(And)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchOperatorExpressionConfig? IMatchOperatorExpressionConfig.Or => Or;

    public IMutableMatchOperatorExpressionConfig? Or
    {
        get
        {
            if (GetSection(nameof(Or)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{nameof(Or)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new MatchOperatorExpressionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Or)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchConditionConfig? IMatchOperatorExpressionConfig.IsFalse => IsFalse;

    public IMutableMatchConditionConfig? IsFalse
    {
        get
        {
            if (GetSection(nameof(IsFalse)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return ConfigRoot.GetMatchConditionConfig($"{Path}{Split}{nameof(IsFalse)}", this);
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{nameof(IsFalse)}");

                value.ParentConfig = this;
            }
        }
    }

    IMatchConditionConfig? IMatchOperatorExpressionConfig.IsTrue => IsTrue;

    public IMutableMatchConditionConfig? IsTrue
    {
        get
        {
            if (GetSection(nameof(IsTrue)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return ConfigRoot.GetMatchConditionConfig($"{Path}{Split}{nameof(IsTrue)}", this);
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{nameof(IsTrue)}");

                value.ParentConfig = this;
            }
        }
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public IMatchOperatorExpressionConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new MatchOperatorExpressionConfig(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IMatchOperatorExpressionConfig ICloneable<IMatchOperatorExpressionConfig>.Clone() => Clone();

    public virtual MatchOperatorExpressionConfig Clone() => new(this);

    public virtual bool AreEquivalent(IMatchOperatorExpressionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var evalOrderSame = EvaluateOrder == other.EvaluateOrder;
        var allSame       = All?.AreEquivalent(other.All, exactTypes) ?? other.All == null;
        var anySame       = Any?.AreEquivalent(other.Any, exactTypes) ?? other.Any == null;
        var andSame       = And?.AreEquivalent(other.And, exactTypes) ?? other.And == null;
        var orSame        = Or?.AreEquivalent(other.Or, exactTypes) ?? other.Or == null;
        var isTrueSame    = IsTrue?.AreEquivalent(other.IsTrue, exactTypes) ?? other.Or == null;
        var isFalseSame   = IsFalse?.AreEquivalent(other.IsFalse, exactTypes) ?? other.Or == null;

        var allAreSame = evalOrderSame && allSame && anySame && andSame && orSame && isTrueSame && isFalseSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchOperatorExpressionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)EvaluateOrder;
        hashCode = (hashCode * 397) ^ (All?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (Any?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (And?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (Or?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (IsTrue?.GetHashCode() ?? 0);
        hashCode = (hashCode * 397) ^ (IsFalse?.GetHashCode() ?? 0);
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(EvaluateOrder), EvaluateOrder)
           .Field.WhenNonNullAddStyled(nameof(All), All)
           .Field.WhenNonNullAddStyled(nameof(Any), Any)
           .Field.WhenNonNullAddStyled(nameof(And), And)
           .Field.WhenNonNullAddStyled(nameof(Or), Or)
           .Field.WhenNonNullAddStyled(nameof(IsTrue), IsTrue)
           .Field.WhenNonNullAddStyled(nameof(IsFalse), IsFalse)
           .Complete();
}
