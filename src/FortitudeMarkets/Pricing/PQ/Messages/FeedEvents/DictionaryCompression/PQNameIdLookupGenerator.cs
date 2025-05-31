// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

public interface IPQNameIdLookupGenerator : INameIdLookupGenerator,
    IPQSupportsStringUpdates<INameIdLookup>
{
    new IPQNameIdLookupGenerator Clone();

    void Clear();
}

public class PQNameIdLookupGenerator : NameIdLookupGenerator, IPQNameIdLookupGenerator
{
    private const uint ReservedForStringSerializedSize = 0;

    private readonly   PQFeedFields dictionaryFieldKey;
    protected readonly List<int>     IdsUpdated = new();

    protected int HighestIdSerialized;

    protected uint SequenceId = 0;

    public PQNameIdLookupGenerator(PQFeedFields dictionaryFieldKey) => this.dictionaryFieldKey = dictionaryFieldKey;

    public PQNameIdLookupGenerator(INameIdLookup toClone, PQFeedFields? dictionaryFieldKey = null) : base(toClone)
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
            IdsUpdated.Clear(); // either all will be marked or none if false
            if (value)
            {
                IdsUpdated.AddRange(Cache.Keys);
                HighestIdSerialized = 0;
            }
            else
            {
                HighestIdSerialized = Count;
            }
        }
    }

    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates)
        {
            SequenceId++;
            HasUpdates = false;
        }
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags) =>
        from kvp in this
        where (messageFlags & StorageFlags.Complete) > 0 || IsIdUpdated(kvp.Key) || kvp.Key > HighestIdSerialized
        let sideEffect = HighestIdSerialized = Math.Max(HighestIdSerialized, kvp.Key)
        select new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(dictionaryFieldKey, CrudCommand.Upsert.ToPQSubFieldId(), ReservedForStringSerializedSize)
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


    public override void Clear()
    {
        IdsUpdated.Clear();
        HighestIdSerialized = 0;

        base.Clear();
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

    protected override void AddedIdAndName(int id, string? name)
    {
        IdsUpdated.Add(id);
    }

    public bool IsIdUpdated(int id) => IdsUpdated.Contains(id);

    public override string ToString() =>
        $"PQNameIdLookupGenerator {{ {nameof(dictionaryFieldKey)}: {dictionaryFieldKey}, " +
        $"{nameof(Cache)}: {Cache.ListDictionaryContents()}";
}
