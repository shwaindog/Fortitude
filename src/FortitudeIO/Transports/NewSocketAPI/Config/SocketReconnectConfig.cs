#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Config;

public interface ISocketReconnectConfig : ICloneable<ISocketReconnectConfig>
{
    uint StartReconnectIntervalMs { get; set; }
    uint MaxReconnectIntervalMs { get; set; }
    uint IncrementReconnectIntervalMs { get; set; }
    uint NextReconnectIntervalMs { get; set; }
}

public class SocketReconnectConfig : ISocketReconnectConfig
{
    private uint nextReconnectInterval = 1_000;

    public SocketReconnectConfig(uint nextReconnectInterval)
    {
        this.nextReconnectInterval = nextReconnectInterval;
        StartReconnectIntervalMs = nextReconnectInterval;
    }

    public SocketReconnectConfig(ISocketReconnectConfig toClone)
    {
        StartReconnectIntervalMs = toClone.StartReconnectIntervalMs;
        MaxReconnectIntervalMs = toClone.MaxReconnectIntervalMs;
        IncrementReconnectIntervalMs = toClone.IncrementReconnectIntervalMs;
    }

    public SocketReconnectConfig() => StartReconnectIntervalMs = nextReconnectInterval;

    public uint StartReconnectIntervalMs { get; set; }
    public uint MaxReconnectIntervalMs { get; set; } = 60_000;
    public uint IncrementReconnectIntervalMs { get; set; } = 10_000;

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

    public override int GetHashCode() =>
        HashCode.Combine(StartReconnectIntervalMs, MaxReconnectIntervalMs, IncrementReconnectIntervalMs);

    public override string ToString() =>
        $"SocketConnectionConfig({nameof(StartReconnectIntervalMs)}: {StartReconnectIntervalMs}, " +
        $"{nameof(MaxReconnectIntervalMs)}: {MaxReconnectIntervalMs}, {nameof(IncrementReconnectIntervalMs)}: " +
        $"{IncrementReconnectIntervalMs}, {nameof(NextReconnectIntervalMs)}: {NextReconnectIntervalMs})";
}
