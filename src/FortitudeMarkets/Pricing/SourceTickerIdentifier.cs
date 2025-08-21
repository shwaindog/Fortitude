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
    [JsonIgnore] uint SourceInstrumentId { get; }

    ushort SourceId       { get; }
    ushort InstrumentId   { get; }
    string SourceName     { get; }
    string InstrumentName { get; }
}


public class SourceTickerId : ReusableObject<ISourceTickerId>, ISourceTickerId, IInterfacesComparable<ISourceTickerId>
{
    public SourceTickerId() { }

    public SourceTickerId(ushort sourceId, ushort tickerId, string sourceName, string ticker)
    {
        SourceId       = sourceId;
        InstrumentId   = tickerId;
        SourceName     = sourceName;
        InstrumentName = ticker;
    }

    public SourceTickerId(ISourceTickerId toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;
    }

    public SourceTickerId(SourceTickerIdentifier toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;
    }

    public virtual bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        var sourceIdSame = SourceId == other?.SourceId;
        var tickerIdSame = InstrumentId == other?.InstrumentId;
        var sourceSame   = SourceName == other?.SourceName;
        var tickerSame   = InstrumentName == other?.InstrumentName;


        return sourceIdSame && tickerIdSame && sourceSame && tickerSame;
    }

    public uint   SourceInstrumentId => (uint)((SourceId << 16) | InstrumentId);
    public ushort SourceId           { get; set; }
    public ushort InstrumentId       { get; set; }
    public string SourceName         { get; set; } = null!;
    public string InstrumentName     { get; set; } = null!;

    public override void StateReset()
    {
        SourceId       = 0;
        InstrumentId   = 0;
        SourceName     = null!;
        InstrumentName = null!;
        base.StateReset();
    }

    public override ISourceTickerId Clone() => Recycler?.Borrow<SourceTickerId>().CopyFrom(this) ?? new SourceTickerId((ISourceTickerId)this);

    public override ISourceTickerId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId       = source.SourceId;
        InstrumentId   = source.InstrumentId;
        SourceName     = source.SourceName;
        InstrumentName = source.InstrumentName;
        return this;
    }

    protected bool Equals(SourceTickerId other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SourceTickerId)obj);
    }

    protected string SourceTickerIdToStringMembers =>
        $"{nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, {nameof(InstrumentId)}: {InstrumentId}, " +
        $"{nameof(InstrumentName)}: {InstrumentName}";

    protected string SourceInstrumentIdToString => $"{nameof(SourceInstrumentId)}: {SourceInstrumentId}";

    public override string ToString() => $"{nameof(SourceTickerId)}{{{SourceTickerIdToStringMembers}, {SourceInstrumentIdToString}}}";

    public override int GetHashCode() => HashCode.Combine(SourceId, InstrumentId, SourceName, InstrumentName);

    public static implicit operator SourceTickerIdentifier(SourceTickerId sourceTickerId) => new(sourceTickerId);
}

public readonly struct SourceTickerIdentifier // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public SourceTickerIdentifier(ushort sourceId, ushort instrumentId)
    {
        SourceId     = sourceId;
        InstrumentId = instrumentId;
    }

    public SourceTickerIdentifier(ISourceTickerId sourceTickerId)
    {
        SourceId     = sourceTickerId.SourceId;
        InstrumentId = sourceTickerId.InstrumentId;
    }

    public uint   SourceInstrumentId => (uint)((SourceId << 16) | InstrumentId);
    public ushort SourceId           { get; }
    public ushort InstrumentId       { get; }
    public string InstrumentName     => SourceTickerIdentifierExtensions.GetRegisteredInstrumentName(SourceInstrumentId);
    public string SourceName         => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);
}

public readonly struct SourceTickerIdValue // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public SourceTickerIdValue(ushort sourceId, ushort instrumentId, string sourceName, string instrumentName)
    {
        SourceId       = sourceId;
        InstrumentId   = instrumentId;
        SourceName     = sourceName;
        InstrumentName = instrumentName;
    }

    public uint   SourceTickerId => (uint)((SourceId << 16) | InstrumentId);
    public ushort SourceId       { get; }
    public ushort InstrumentId   { get; }
    public string SourceName     { get; }
    public string InstrumentName { get; }


    public static implicit operator SourceTickerIdentifier
        (SourceTickerIdValue sourceTickerId) =>
        new(sourceTickerId.SourceId, sourceTickerId.InstrumentId);
}

public static class SourceTickerIdentifierExtensions
{
    public const string NoSourceNameValue            = "Source Name Not Registered";
    public const string NoTickerNameValue            = "Ticker Name Not Registered";
    public const string NoSourceTickerShortNameValue = "Source Ticker Not Registered";

    private static readonly ConcurrentMap<uint, string>   SingleStringShortNameLookup = new();
    private static readonly ConcurrentMap<ushort, string> IdToSourceNameLookup        = new();
    private static readonly ConcurrentMap<uint, string>   IdsToTickerNameLookup       = new();

    public static string SourceInstrumentShortName(this ISourceTickerId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceInstrumentId, out var shortName))
            if (id.SourceName != NoSourceNameValue && id.InstrumentName != NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}";
                SingleStringShortNameLookup.TryAdd(id.SourceInstrumentId, shortName);
            }
        return shortName!;
    }

    public static bool Register(this ISourceTickerId id)
    {
        if (!IdToSourceNameLookup.ContainsKey(id.SourceId)) IdToSourceNameLookup.AddOrUpdate(id.SourceId, id.SourceName);
        if (!IdsToTickerNameLookup.TryGetValue(id.SourceInstrumentId, out var tickerMap))
        {
            IdsToTickerNameLookup.TryAdd(id.SourceInstrumentId, id.InstrumentName);
            return true;
        }
        return false;
    }

    public static bool Register(this SourceTickerIdValue id)
    {
        if (!IdToSourceNameLookup.ContainsKey(id.SourceId)) IdToSourceNameLookup.AddOrUpdate(id.SourceId, id.SourceName);
        if (!IdsToTickerNameLookup.TryGetValue(id.SourceTickerId, out var tickerMap))
        {
            IdsToTickerNameLookup.TryAdd(id.SourceTickerId, id.InstrumentName);
            return true;
        }
        return false;
    }

    public static string GetRegisteredSourceName(ushort sourceId)
    {
        if (IdToSourceNameLookup.ContainsKey(sourceId)) return IdToSourceNameLookup[sourceId]!;
        return NoSourceNameValue;
    }

    public static string GetRegisteredInstrumentName(uint sourceTickerId)
    {
        if (IdsToTickerNameLookup.TryGetValue(sourceTickerId, out var tickerName)) return tickerName!;
        return NoTickerNameValue;
    }

    public static string SourceInstrumentShortName(uint sourceTickerId)
    {
        if (!SingleStringShortNameLookup.TryGetValue(sourceTickerId, out var shortName)) shortName = NoSourceTickerShortNameValue;
        return shortName!;
    }

    public static string SourceInstrumentShortName(this SourceTickerIdentifier id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceInstrumentId, out var shortName))
        {
            if (id.SourceName != NoSourceNameValue && id.InstrumentName != NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}";
                SingleStringShortNameLookup.TryAdd(id.SourceInstrumentId, shortName);
            }
            else
            {
                shortName = NoSourceTickerShortNameValue;
            }
        }
        return shortName!;
    }

    public static string SourceInstrumentShortName(this SourceTickerIdValue id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
        {
            if (id.SourceName != NoSourceNameValue && id.InstrumentName != NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}";
                SingleStringShortNameLookup.TryAdd(id.SourceTickerId, shortName);
            }
            else
            {
                shortName = NoSourceTickerShortNameValue;
            }
        }
        return shortName!;
    }
}
