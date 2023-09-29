using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.DictionaryCompression
{
    public class PQNameIdLookupGenerator : NameIdLookupGenerator, IPQNameIdLookupGenerator
    {
        protected readonly List<int> IdsUpdated = new List<int>();
        private readonly byte dictionaryFieldKey;
        private readonly byte subDictionaryId;
        private const uint ReservedForStringSerializedSize = 0;

        public PQNameIdLookupGenerator(byte dictionaryFieldKey, byte subDictionaryId = 0)
        {
            this.dictionaryFieldKey = dictionaryFieldKey;
            this.subDictionaryId = subDictionaryId;
        }

        public PQNameIdLookupGenerator(INameIdLookup toClone) : base(toClone)
        {
            if (toClone is PQNameIdLookupGenerator pqToClone)
            {
                dictionaryFieldKey = pqToClone.dictionaryFieldKey;
                subDictionaryId = pqToClone.subDictionaryId;
                IdsUpdated = new List<int>(pqToClone.IdsUpdated);
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
                }
            }
        }

        protected override void AddNewEntry(int id, string name)
        {
            base.AddNewEntry(id, name);
            IdsUpdated.Add(id);
        }

        public bool IsIdUpdated(int id)
        {
            return IdsUpdated.Contains(id);
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
        {
            return from kvp in this
                where updatedStyle == UpdateStyle.FullSnapshot || IsIdUpdated(kvp.Key) 
                select new PQFieldStringUpdate
                            {
                                Field = new PQFieldUpdate(dictionaryFieldKey, ReservedForStringSerializedSize, 
                                (byte)(subDictionaryId | PQFieldFlags.IsUpdate)),
                                StringUpdate = new PQStringUpdate
                                {
                                    DictionaryId = kvp.Key,
                                    Value = kvp.Value,
                                    Command = CrudCommand.Update
                                }
                            };
        }

        public bool UpdateFieldString(PQFieldStringUpdate updates)
        {
            if (updates.Field.Id != dictionaryFieldKey
                || (updates.Field.Flag & 0x0F) != subDictionaryId) return false;
            var id = updates.StringUpdate.DictionaryId;
            var stringUpdateCommand = updates.StringUpdate.Command;
            var stringValue = updates.StringUpdate.Value;
            if (stringUpdateCommand != CrudCommand.Insert && stringUpdateCommand != CrudCommand.Update) return false;
            SetIdToName(id, stringValue);
            return true;
        }

        public override void CopyFrom(INameIdLookup source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (ReferenceEquals(this, source)) return;
            if ((copyMergeFlags & CopyMergeFlags.NoAppendLookupValues) != 0)
            {
                Cache.Clear();
                ReverseLookup.Clear();
            }
            if (source is PQNameIdLookupGenerator pqNameIdLookupGenerator)
            {
                bool shouldCopyNonUpdate = (copyMergeFlags & CopyMergeFlags.CopyNonUpdated) != 0;

                if (!shouldCopyNonUpdate)
                {
                    foreach (var updatedId in pqNameIdLookupGenerator.IdsUpdated)
                    {
                        Cache[updatedId] = pqNameIdLookupGenerator[updatedId];
                        ReverseLookup[pqNameIdLookupGenerator[updatedId]] = updatedId;
                        if (IdsUpdated.Contains(updatedId)) continue;
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
                            Cache[kvp.Key] = kvp.Value;
                            ReverseLookup[kvp.Value] = kvp.Key;
                        }
                        LargestAddedId = Math.Max(LargestAddedId, pqNameIdLookupGenerator.LargestAddedId);
                    }
                }
                foreach (var updatedId in pqNameIdLookupGenerator.IdsUpdated)
                {
                    if (IdsUpdated.Contains(updatedId)) continue;
                    IdsUpdated.Add(updatedId);
                }
            }
            else
            {
                base.CopyFrom(source, copyMergeFlags);
            }
        }

        IPQNameIdLookupGenerator IPQNameIdLookupGenerator.Clone()
        {
            return (IPQNameIdLookupGenerator)Clone();
        }
        
        public override object Clone()
        {
            return new PQNameIdLookupGenerator(this);
        }

        public override bool AreEquivalent(IIdLookup<string> other, bool exactTypes = false)
        {
            if (!(other is IPQNameIdLookupGenerator)) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var baseSame = base.AreEquivalent(other, exactTypes);
            var dictionaryIdSame = true;
            var subDictionaryIdSame = true;
            if (exactTypes)
            {
                var pqNameIdLookupGen = other as PQNameIdLookupGenerator;

                dictionaryIdSame = dictionaryFieldKey == pqNameIdLookupGen?.dictionaryFieldKey;
                subDictionaryIdSame = subDictionaryId == pqNameIdLookupGen?.subDictionaryId;
            }

            return baseSame && dictionaryIdSame && subDictionaryIdSame;
        }

        public override string ToString()
        {
            return $"PQNameIdLookupGenerator {{ {nameof(dictionaryFieldKey)}: {dictionaryFieldKey}, " +
                   $"{nameof(subDictionaryId)}: {subDictionaryId}, {nameof(Cache)}: {Cache.ListDictionaryContents()}";
        }
    }
}