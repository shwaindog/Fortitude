// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeIO.Transports.Network.Config;

public interface ISocketReconnectConfig : ICloneable<ISocketReconnectConfig>
{
    const uint DefaultStartReconnectIntervalMs = 1_000;
    const uint DefaultMaxReconnectIntervalMs   = 60_000;
    const uint DefaultIncrementReconnectIntervalMs    = 10_000;
    const uint DefaultNextReconnectIntervalMs  = 1_000;

    uint StartReconnectIntervalMs     { get; set; }
    uint MaxReconnectIntervalMs       { get; set; }
    uint IncrementReconnectIntervalMs { get; set; }
    uint NextReconnectIntervalMs      { get; set; }
}

public class SocketReconnectConfig : ConfigurationSection, ISocketReconnectConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { nameof(StartReconnectIntervalMs), ISocketReconnectConfig.DefaultStartReconnectIntervalMs.ToString() }
      , { nameof(MaxReconnectIntervalMs), ISocketReconnectConfig.DefaultMaxReconnectIntervalMs.ToString() }
      , { nameof(IncrementReconnectIntervalMs), ISocketReconnectConfig.DefaultIncrementReconnectIntervalMs.ToString() }
    };

    private uint nextReconnectInterval = ISocketReconnectConfig.DefaultNextReconnectIntervalMs;

    public SocketReconnectConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public SocketReconnectConfig(uint nextReconnectInterval) : base(new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), "")
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
        this.nextReconnectInterval = nextReconnectInterval;
        StartReconnectIntervalMs   = nextReconnectInterval;
    }

    public SocketReconnectConfig(ISocketReconnectConfig toClone) : 
        this(toClone, new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), "") { }

    public SocketReconnectConfig(ISocketReconnectConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        StartReconnectIntervalMs     = toClone.StartReconnectIntervalMs;
        MaxReconnectIntervalMs       = toClone.MaxReconnectIntervalMs;
        IncrementReconnectIntervalMs = toClone.IncrementReconnectIntervalMs;
        nextReconnectInterval        = StartReconnectIntervalMs;
    }

    public SocketReconnectConfig() : base(new ConfigurationBuilder().AddInMemoryCollection(Defaults).Build(), "")
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
        nextReconnectInterval = StartReconnectIntervalMs;
    }

    public uint StartReconnectIntervalMs
    {
        get
        {
            var checkValue = this[nameof(StartReconnectIntervalMs)];
            return checkValue.IsNotNullOrEmpty() ? uint.Parse(checkValue!) : ISocketReconnectConfig.DefaultStartReconnectIntervalMs;
        }
        set => this[nameof(StartReconnectIntervalMs)] = value.ToString();
    }

    public uint MaxReconnectIntervalMs
    {
        get
        {
            var checkValue = this[nameof(MaxReconnectIntervalMs)];
            return checkValue.IsNotNullOrEmpty()  ? uint.Parse(checkValue!) : ISocketReconnectConfig.DefaultMaxReconnectIntervalMs;
        }
        set => this[nameof(MaxReconnectIntervalMs)] = value.ToString();
    }

    public uint IncrementReconnectIntervalMs
    {
        get
        {
            var checkValue = this[nameof(IncrementReconnectIntervalMs)];
            return checkValue != null ? uint.Parse(checkValue) : ISocketReconnectConfig.DefaultIncrementReconnectIntervalMs;
        }
        set => this[nameof(IncrementReconnectIntervalMs)] = value.ToString();
    }

    public uint NextReconnectIntervalMs
    {
        get
        {
            var returnValue = nextReconnectInterval;
            nextReconnectInterval
                = Math.Max(MaxReconnectIntervalMs, nextReconnectInterval + IncrementReconnectIntervalMs);
            return returnValue;
        }
        set => nextReconnectInterval = value;
    }

    object ICloneable.Clone() => Clone();

    public ISocketReconnectConfig Clone() => new SocketReconnectConfig(this);

    protected bool Equals(SocketReconnectConfig other) =>
        StartReconnectIntervalMs == other.StartReconnectIntervalMs &&
        MaxReconnectIntervalMs == other.MaxReconnectIntervalMs &&
        IncrementReconnectIntervalMs == other.IncrementReconnectIntervalMs;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SocketReconnectConfig)obj);
    }

    public override int GetHashCode() => HashCode.Combine(StartReconnectIntervalMs, MaxReconnectIntervalMs, IncrementReconnectIntervalMs);

    public override string ToString() =>
        $"SocketConnectionConfig({nameof(StartReconnectIntervalMs)}: {StartReconnectIntervalMs}, " +
        $"{nameof(MaxReconnectIntervalMs)}: {MaxReconnectIntervalMs}, {nameof(IncrementReconnectIntervalMs)}: " +
        $"{IncrementReconnectIntervalMs}, {nameof(NextReconnectIntervalMs)}: {NextReconnectIntervalMs})";
}
