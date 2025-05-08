// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing;

public interface ISourceTickerId : IReusableObject<ISourceTickerId>
{
    [JsonIgnore] uint SourceTickerId { get; }
    ushort            SourceId       { get; }
    ushort            InstrumentId   { get; }
    string            SourceName     { get; }
    string            InstrumentName { get; }
}

public class SourceTicker : ReusableObject<ISourceTickerId>, ISourceTickerId, IInterfacesComparable<ISourceTickerId>
{
    public SourceTicker() { }

    public SourceTicker(ushort sourceId, ushort tickerId, string sourceName, string ticker)
    {
        SourceId       = sourceId;
        InstrumentId   = tickerId;
        SourceName     = sourceName;
        InstrumentName = ticker;
    }

    public SourceTicker(ISourceTickerId toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;
    }

    public SourceTicker(SourceTickerIdentifier toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.TickerId;
        SourceName     = toClone.Source;
        InstrumentName = toClone.Ticker;
    }

    public virtual bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        var sourceIdSame = SourceId == other?.SourceId;
        var tickerIdSame = InstrumentId == other?.InstrumentId;
        var sourceSame   = SourceName == other?.SourceName;
        var tickerSame   = InstrumentName == other?.InstrumentName;


        return sourceIdSame && tickerIdSame && sourceSame && tickerSame;
    }

    public uint   SourceTickerId => (uint)((SourceId << 16) | InstrumentId);
    public ushort SourceId       { get; set; }
    public ushort InstrumentId   { get; set; }
    public string SourceName     { get; set; } = null!;
    public string InstrumentName { get; set; } = null!;

    public override ISourceTickerId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId       = source.SourceId;
        InstrumentId   = source.InstrumentId;
        SourceName     = source.SourceName;
        InstrumentName = source.InstrumentName;
        return this;
    }

    public override void StateReset()
    {
        SourceId       = 0;
        InstrumentId   = 0;
        SourceName     = null!;
        InstrumentName = null!;
        base.StateReset();
    }

    public override ISourceTickerId Clone() => Recycler?.Borrow<SourceTicker>().CopyFrom(this) ?? new SourceTicker((ISourceTickerId)this);

    protected bool Equals(SourceTicker other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SourceTicker)obj);
    }

    public override int GetHashCode() => HashCode.Combine(SourceId, InstrumentId, SourceName, InstrumentName);

    public static implicit operator SourceTickerIdentifier(SourceTicker sourceTickerId) => new(sourceTickerId);
}

public readonly struct SourceTickerIdentifier // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public SourceTickerIdentifier(ushort sourceId, ushort tickerId)
    {
        SourceId = sourceId;
        TickerId = tickerId;
    }

    public SourceTickerIdentifier(ISourceTickerId sourceTickerId)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.InstrumentId;
    }

    public uint   SourceTickerId => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId       { get; }
    public ushort TickerId       { get; }
    public string Ticker         => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceTickerId);
    public string Source         => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);
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

    public uint   SourceTickerId => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId       { get; }
    public ushort TickerId       { get; }
    public string Source         { get; }
    public string Ticker         { get; }


    public static implicit operator SourceTickerIdentifier
        (SourceTickerIdValue sourceTickerId) =>
        new(sourceTickerId.SourceId, sourceTickerId.TickerId);
}

public static class SourceTickerIdentifierExtensions
{
    public const string NoSourceNameValue            = "Source Name Not Registered";
    public const string NoTickerNameValue            = "Ticker Name Not Registered";
    public const string NoSourceTickerShortNameValue = "Source Ticker Not Registered";

    private static readonly ConcurrentMap<uint, string>   SingleStringShortNameLookup = new();
    private static readonly ConcurrentMap<ushort, string> IdToSourceNameLookup        = new();
    private static readonly ConcurrentMap<uint, string>   IdsToTickerNameLookup       = new();

    public static string SourceTickerShortName(this ISourceTickerId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
            if (id.SourceName != NoSourceNameValue && id.InstrumentName != NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
            }
        return shortName!;
    }

    public static bool Register(this ISourceTickerId id)
    {
        if (!IdToSourceNameLookup.ContainsKey(id.SourceId)) IdToSourceNameLookup.AddOrUpdate(id.SourceId, id.SourceName);
        if (!IdsToTickerNameLookup.TryGetValue(id.SourceTickerId, out var tickerMap))
        {
            IdsToTickerNameLookup.Add(id.SourceTickerId, id.InstrumentName);
            return true;
        }
        return false;
    }

    public static bool Register(this SourceTickerIdValue id)
    {
        if (!IdToSourceNameLookup.ContainsKey(id.SourceId)) IdToSourceNameLookup.AddOrUpdate(id.SourceId, id.Source);
        if (!IdsToTickerNameLookup.TryGetValue(id.SourceTickerId, out var tickerMap))
        {
            IdsToTickerNameLookup.Add(id.SourceTickerId, id.Ticker);
            return true;
        }
        return false;
    }

    public static string GetRegisteredSourceName(ushort sourceId)
    {
        if (IdToSourceNameLookup.ContainsKey(sourceId)) return IdToSourceNameLookup[sourceId]!;
        return NoSourceNameValue;
    }

    public static string GetRegisteredTickerName(uint sourceTickerId)
    {
        if (IdsToTickerNameLookup.TryGetValue(sourceTickerId, out var tickerName)) return tickerName!;
        return NoTickerNameValue;
    }

    public static string SourceTickerShortName(uint sourceTickerId)
    {
        if (!SingleStringShortNameLookup.TryGetValue(sourceTickerId, out var shortName)) shortName = NoSourceTickerShortNameValue;
        return shortName!;
    }

    public static string SourceTickerShortName(this SourceTickerIdentifier id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
        {
            if (id.Source != NoSourceNameValue && id.Ticker != NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
            }
            else
            {
                shortName = NoSourceTickerShortNameValue;
            }
        }
        return shortName!;
    }

    public static string SourceTickerShortName(this SourceTickerIdValue id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
        {
            if (id.Source != NoSourceNameValue && id.Ticker != NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
            }
            else
            {
                shortName = NoSourceTickerShortNameValue;
            }
        }
        return shortName!;
    }
}
