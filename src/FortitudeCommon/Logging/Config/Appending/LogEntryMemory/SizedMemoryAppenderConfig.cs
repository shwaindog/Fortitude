using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.LogEntryMemory;

public interface ISizedMemoryAppenderConfig : IAppenderDefinitionConfig, IConfigCloneTo<ISizedMemoryAppenderConfig>
{
    long MemorySize { get; }

    new ISizedMemoryAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    new ISizedMemoryAppenderConfig Clone();
}

public interface IMutableSizedMemoryAppenderConfig : ISizedMemoryAppenderConfig, IMutableAppenderDefinitionConfig
{
    new long MemorySize { get; set; }
}

internal class SizedMemoryAppenderConfig : AppenderDefinitionConfig, ISizedMemoryAppenderConfig
{
    private const string SizedMemoryAppenderConfigType = $"{nameof(FLoggerBuiltinAppenderType.FLogEntryListAppender)}";

    public SizedMemoryAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public SizedMemoryAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public SizedMemoryAppenderConfig(string appenderName, long memorySize = 16
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, memorySize, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public SizedMemoryAppenderConfig(IConfigurationRoot root, string path, string appenderName
      , long memorySize = 16, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : base(root, path, appenderName, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        MemorySize = memorySize;
    }

    public SizedMemoryAppenderConfig(ISizedMemoryAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MemorySize = toClone.MemorySize;
    }

    public SizedMemoryAppenderConfig(ISizedMemoryAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public long MemorySize
    {
        get =>
            long.TryParse(this[nameof(MemorySize)], out var dropInterval)
                ? dropInterval
                : IFLogEntryQueueConfig.DefaultQueueDropInterval;
        set => this[nameof(MemorySize)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    ISizedMemoryAppenderConfig ISizedMemoryAppenderConfig.Clone() => Clone();

    ISizedMemoryAppenderConfig ISizedMemoryAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public override SizedMemoryAppenderConfig Clone() => new(this);

    ISizedMemoryAppenderConfig ICloneable<ISizedMemoryAppenderConfig>.Clone() => Clone();

    ISizedMemoryAppenderConfig IConfigCloneTo<ISizedMemoryAppenderConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public override SizedMemoryAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);


    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not ISizedMemoryAppenderConfig sizedMemoryAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var appendersSame = MemorySize == sizedMemoryAppenderConfig.MemorySize;

        var allAreSame = baseSame && appendersSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IForwardingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ MemorySize.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        using var tb = sbc.StartComplexType(nameof(ForwardingAppenderConfig))
                          .AddBaseFieldsStart();
        base.ToString(sbc);
        tb.Field.AlwaysAdd(nameof(MemorySize), MemorySize);
        return tb;
    }
}
