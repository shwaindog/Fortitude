// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface ISourceTickerId : IReusableObject<ISourceTickerId>
{
    uint   Id       { get; }
    ushort SourceId { get; }
    ushort TickerId { get; }
    string Source   { get; }
    string Ticker   { get; }
}

public class SourceTickerIdentifier : ReusableObject<ISourceTickerId>, ISourceTickerId
{
    public SourceTickerIdentifier() { }

    public SourceTickerIdentifier(ushort sourceId, ushort tickerId, string source, string ticker)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;
    }

    public SourceTickerIdentifier(ISourceTickerId toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source   = toClone.Source;
        Ticker   = toClone.Ticker;
    }

    public uint   Id       => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; set; }
    public ushort TickerId { get; set; }
    public string Source   { get; set; } = null!;
    public string Ticker   { get; set; } = null!;

    public override ISourceTickerId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = source.SourceId;
        TickerId = source.TickerId;
        Source   = source.Source;
        Ticker   = source.Ticker;
        return this;
    }

    public override void StateReset()
    {
        SourceId = 0;
        TickerId = 0;
        Source   = null!;
        Ticker   = null!;
        base.StateReset();
    }

    public override ISourceTickerId Clone() => Recycler?.Borrow<SourceTickerIdentifier>().CopyFrom(this) ?? new SourceTickerIdentifier(this);

    public static implicit operator SourceTickerId(SourceTickerIdentifier sourceTickerId) => new(sourceTickerId);
}

public readonly struct SourceTickerId // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public SourceTickerId(ushort sourceId, ushort tickerId)
    {
        SourceId = sourceId;
        TickerId = tickerId;
    }

    public SourceTickerId(ISourceTickerId sourceTickerId)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
    }

    public uint   Id       => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; }
    public ushort TickerId { get; }
    public string Ticker   => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceId, TickerId);
    public string Source   => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);
}

public readonly struct SourceTickerIdValue // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public SourceTickerIdValue(ushort sourceId, ushort tickerId, string source, string ticker)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;
    }

    public uint   Id       => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; }
    public ushort TickerId { get; }
    public string Source   { get; }
    public string Ticker   { get; }


    public static implicit operator SourceTickerId(SourceTickerIdValue sourceTickerId) => new(sourceTickerId.SourceId, sourceTickerId.TickerId);
}

public static class SourceTickerIdentifierExtensions
{
    public const string NoSourceNameValue = "Source Name Not Registered";
    public const string NoTickerNameValue = "Ticker Name Not Registered";

    private static readonly ConcurrentMap<ushort, IMap<ushort, string>> SingleStringShortNameLookup = new();
    private static readonly ConcurrentMap<ushort, string>               IdToSourceNameLookup        = new();
    private static readonly ConcurrentMap<ushort, IMap<ushort, string>> IdsToTickerNameLookup       = new();

    public static string ShortName(this ISourceTickerId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceId, out var tickerMap))
        {
            tickerMap = new ConcurrentMap<ushort, string>();
            SingleStringShortNameLookup.Add(id.SourceId, tickerMap);
        }
        if (!tickerMap!.TryGetValue(id.TickerId, out var shortName))
        {
            if (id.Source != NoSourceNameValue && id.Ticker != NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}";
                tickerMap.Add(id.TickerId, shortName);
            }
            else
            {
                shortName = $"{id.SourceId}-{id.TickerId}";
            }
        }
        return shortName!;
    }

    public static bool Register(this ISourceTickerId id)
    {
        if (!IdToSourceNameLookup.ContainsKey(id.SourceId)) IdToSourceNameLookup.AddOrUpdate(id.SourceId, id.Source);
        if (!IdsToTickerNameLookup.TryGetValue(id.SourceId, out var tickerMap))
        {
            tickerMap = new ConcurrentMap<ushort, string>();
            IdsToTickerNameLookup.Add(id.SourceId, tickerMap);
        }
        if (tickerMap!.ContainsKey(id.TickerId)) return false;
        tickerMap.Add(id.TickerId, id.Ticker);
        return true;
    }

    public static string GetRegisteredSourceName(ushort sourceId)
    {
        if (IdToSourceNameLookup.ContainsKey(sourceId)) return IdToSourceNameLookup[sourceId]!;
        return NoSourceNameValue;
    }

    public static string GetRegisteredTickerName(ushort sourceId, ushort tickerId)
    {
        if (IdsToTickerNameLookup.TryGetValue(sourceId, out var tickerMap))
            if (tickerMap!.TryGetValue(tickerId, out var tickerName))
                return tickerName!;
        return NoTickerNameValue;
    }

    public static string ShortName(this SourceTickerId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceId, out var tickerMap))
        {
            tickerMap = new ConcurrentMap<ushort, string>();
            SingleStringShortNameLookup.Add(id.SourceId, tickerMap);
        }
        if (!tickerMap!.TryGetValue(id.TickerId, out var shortName))
        {
            shortName = $"{id.Source}-{id.Ticker}";
            tickerMap.Add(id.TickerId, shortName);
        }
        return shortName!;
    }

    public static string ShortName(this SourceTickerIdValue id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceId, out var tickerMap))
        {
            tickerMap = new ConcurrentMap<ushort, string>();
            SingleStringShortNameLookup.Add(id.SourceId, tickerMap);
        }
        if (!tickerMap!.TryGetValue(id.TickerId, out var shortName))
        {
            shortName = $"{id.Source}-{id.Ticker}";
            tickerMap.Add(id.TickerId, shortName);
        }
        return shortName!;
    }
}
