// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

public interface IPQNameIdLookupGenerator : INameIdLookupGenerator,
    IPQSupportsStringUpdates<INameIdLookup>
{
    new IPQNameIdLookupGenerator Clone();

    void Clear();
}

public class PQNameIdLookupGenerator : NameIdLookupGenerator, IPQNameIdLookupGenerator
{
    private const uint ReservedForStringSerializedSize = 0;

    private readonly   byte      dictionaryFieldKey;
    protected readonly List<int> IdsUpdated = new();

    protected int HighestIdSerialized;

    public PQNameIdLookupGenerator(byte dictionaryFieldKey) => this.dictionaryFieldKey = dictionaryFieldKey;

    public PQNameIdLookupGenerator(INameIdLookup toClone, byte? dictionaryFieldKey = null) : base(toClone)
    {
        if (toClone is PQNameIdLookupGenerator pqToClone)
        {
            this.dictionaryFieldKey = dictionaryFieldKey ?? pqToClone.dictionaryFieldKey;
            IdsUpdated              = new List<int>(pqToClone.IdsUpdated);
        }
    }

    public bool HasUpdates
    {
        get => IdsUpdated.Any();
        set
        {
            IdsUpdated.Clear();
            if (value)
            {
                IdsUpdated.AddRange(Cache.Keys);
                HighestIdSerialized = 0;
            }
            else
            {
                HighestIdSerialized = LargestAddedId;
            }
        }
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags) =>
        from kvp in this
        where (messageFlags & StorageFlags.Complete) > 0 || IsIdUpdated(kvp.Key) || kvp.Key > HighestIdSerialized
        let sideEffect = HighestIdSerialized = Math.Max(HighestIdSerialized, kvp.Key)
        select new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(dictionaryFieldKey, ReservedForStringSerializedSize,
                                      PQFieldFlags.IsUpsert)
          , StringUpdate = new PQStringUpdate
            {
                DictionaryId = kvp.Key, Value = kvp.Value, Command = CrudCommand.Upsert
            }
        };

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != dictionaryFieldKey) return false;

        var id = stringUpdate.StringUpdate.DictionaryId;

        var stringUpdateCommand = stringUpdate.StringUpdate.Command;
        var stringValue         = stringUpdate.StringUpdate.Value;

        if (stringUpdateCommand != CrudCommand.Upsert) return false;
        SetIdToName(id, stringValue);
        return true;
    }

    public override INameIdLookup CopyFrom(INameIdLookup source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ReferenceEquals(this, source)) return this;
        var hasFullReplace = copyMergeFlags.HasFullReplace();
        if (hasFullReplace) Clear();

        if (source is PQNameIdLookupGenerator pqNameIdLookupGenerator)
        {
            if (!hasFullReplace)
            {
                foreach (var updatedId in pqNameIdLookupGenerator.IdsUpdated)
                {
                    Cache[updatedId]                                   = pqNameIdLookupGenerator[updatedId]!;
                    ReverseLookup[pqNameIdLookupGenerator[updatedId]!] = updatedId;
                    if (IdsUpdated.Contains(updatedId) || updatedId <= HighestIdSerialized) continue;
                    IdsUpdated.Add(updatedId);
                }
                LargestAddedId = Math.Max(LargestAddedId, pqNameIdLookupGenerator.LargestAddedId);
            }
            else
            {
                if (Cache.Count != pqNameIdLookupGenerator.Cache.Count ||
                    LargestAddedId != pqNameIdLookupGenerator.LargestAddedId)
                {
                    foreach (var kvp in pqNameIdLookupGenerator.Cache)
                    {
                        Cache[kvp.Key]           = kvp.Value;
                        ReverseLookup[kvp.Value] = kvp.Key;
                        IdsUpdated.Add(kvp.Key);
                    }

                    LargestAddedId      = pqNameIdLookupGenerator.LargestAddedId;
                    HighestIdSerialized = pqNameIdLookupGenerator.HighestIdSerialized;
                }
            }
        }
        else
        {
            base.CopyFrom(source, copyMergeFlags);
        }

        return this;
    }

    public void Clear()
    {
        Cache.Clear();
        ReverseLookup.Clear();
        IdsUpdated.Clear();
        LargestAddedId      = 0;
        HighestIdSerialized = 0;
    }

    IPQNameIdLookupGenerator IPQNameIdLookupGenerator.Clone() => (IPQNameIdLookupGenerator)Clone();

    public override object Clone() => new PQNameIdLookupGenerator(this);

    public override bool AreEquivalent(IIdLookup<string>? other, bool exactTypes = false)
    {
        if (!(other is IPQNameIdLookupGenerator)) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var dictionaryIdSame    = true;
        var subDictionaryIdSame = true;

        if (exactTypes)
        {
            var pqNameIdLookupGen = other as PQNameIdLookupGenerator;

            dictionaryIdSame = dictionaryFieldKey == pqNameIdLookupGen?.dictionaryFieldKey;
        }

        return baseSame && dictionaryIdSame && subDictionaryIdSame;
    }

    protected override void AddNewEntry(int id, string name)
    {
        base.AddNewEntry(id, name);
        IdsUpdated.Add(id);
    }

    public bool IsIdUpdated(int id) => IdsUpdated.Contains(id);

    public override string ToString() =>
        $"PQNameIdLookupGenerator {{ {nameof(dictionaryFieldKey)}: {dictionaryFieldKey}, " +
        $"{nameof(Cache)}: {Cache.ListDictionaryContents()}";
}
