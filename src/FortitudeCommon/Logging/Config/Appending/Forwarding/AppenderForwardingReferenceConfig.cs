// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IAppenderForwardingReferenceConfig : IAppenderReferenceConfig
{
    ushort AppendProcessingOrder { get; }  // If Synchronised will send from lowest to highest order
}

public interface IMutableAppenderForwardingReferenceConfig : IAppenderForwardingReferenceConfig, IMutableAppenderReferenceConfig
{
    new ushort AppendProcessingOrder { get; set; } 
}

public class AppenderForwardingReferenceConfig : AppenderReferenceConfig, IMutableAppenderForwardingReferenceConfig
{
    public AppenderForwardingReferenceConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AppenderForwardingReferenceConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AppenderForwardingReferenceConfig(string appenderName, string appenderType, bool deactivateHere = false, ushort processingOrder = 0)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, deactivateHere, processingOrder) { }

    public AppenderForwardingReferenceConfig
        (IConfigurationRoot root, string path, string appenderName, string appenderType, bool deactivateHere = false, ushort processingOrder = 0) : base(root, path, appenderName, appenderType, deactivateHere)
    {
        AppendProcessingOrder = processingOrder;
    }

    public AppenderForwardingReferenceConfig(IAppenderForwardingReferenceConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        AppendProcessingOrder = toClone.AppendProcessingOrder;
    }

    public AppenderForwardingReferenceConfig(IAppenderForwardingReferenceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort AppendProcessingOrder
    {
        get => ushort.TryParse(this[nameof(AppendProcessingOrder)], out var procOrder) ? procOrder : (ushort)0;
        set => this[nameof(AppendProcessingOrder)] = value.ToString();
    }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? $"{nameof(FLoggerBuiltinAppenderType.ForwardToRef)}";
        set => this[nameof(AppenderType)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override AppenderForwardingReferenceConfig Clone() => new(this);

    public override IAppenderReferenceConfig CloneConfigTo(IConfigurationRoot configRoot, string path)
    {
        return new AppenderForwardingReferenceConfig(configRoot, path);
    }

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IAppenderForwardingReferenceConfig forwardingRefConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var procOrderSame = AppendProcessingOrder == forwardingRefConfig.AppendProcessingOrder;

        var allAreSame = baseSame && procOrderSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAppenderForwardingReferenceConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ AppendProcessingOrder;
            return hashCode;
        }
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(AppenderForwardingReferenceConfig))
               .AddTypeStart()
               .AddNonNullOrEmptyField(nameof(AppenderName), AppenderName)
               .AddNonNullOrEmptyField(nameof(AppenderType), AppenderType)
               .AddField(nameof(DeactivateHere), DeactivateHere)
               .AddField(nameof(AppendProcessingOrder), AppendProcessingOrder)
               .AddTypeEnd();
    }
}
