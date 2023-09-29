using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.LayeredBook
{
    public class TraderPriceVolumeLayer : PriceVolumeLayer, IMutableTraderPriceVolumeLayer
    {
        public const string TraderCountTraderName = "Only Trader Count Provided";
        public TraderPriceVolumeLayer(decimal price = 0m, decimal volume = 0m) : base(price, volume)
        {
            TraderDetails = new List<IMutableTraderLayerInfo>();
        }

        public TraderPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
        {
            if (toClone is ITraderPriceVolumeLayer trdToClone)
            {
                TraderDetails = new List<IMutableTraderLayerInfo>(trdToClone.Count);
                foreach (var traderLayerInfo in trdToClone.Where(tli => tli != null))
                {
                    TraderDetails.Add(traderLayerInfo is TraderLayerInfo
                        ? (IMutableTraderLayerInfo)traderLayerInfo.Clone()
                        : new TraderLayerInfo(traderLayerInfo.TraderName, traderLayerInfo.TraderVolume));
                }
            }
        }

        protected IList<IMutableTraderLayerInfo> TraderDetails { get; }

        public IMutableTraderLayerInfo this[int i]
        {
            get
            {
                AssertMaxTraderSizeNotExceeded(i);
                for (var j = TraderDetails.Count; j <= i; j++)
                {
                    TraderDetails.Add(new TraderLayerInfo());
                }
                return TraderDetails[i];
            }
            set
            {
                AssertMaxTraderSizeNotExceeded(i);
                for (var j = TraderDetails.Count; j <= i ; j++)
                {
                    TraderDetails.Add(j < i - 1 ? new TraderLayerInfo() : null);
                }
                TraderDetails[i] = value;
            }
        }

        ITraderLayerInfo ITraderPriceVolumeLayer.this[int i] => this[i];
        
        public int Count
        {
            get
            {
                for (var i = TraderDetails?.Count - 1 ?? 0; i >= 0; i--)
                {
                    var traderLayerInfo = TraderDetails?[i];
                    if (!traderLayerInfo?.IsEmpty ?? false)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
        }

        public override bool IsEmpty => base.IsEmpty && TraderDetails.All(tli => tli.IsEmpty);

        public bool IsTraderCountOnly
        {
            get
            {
                return TraderDetails
                    .All(mutableTraderLayerInfo => mutableTraderLayerInfo.IsEmpty
                         || mutableTraderLayerInfo.TraderName == TraderCountTraderName); 
            }
        }

        public void Add(string traderName, decimal volume)
        {
            int indexToUpdate = Count;
            AssertMaxTraderSizeNotExceeded(indexToUpdate);
            if (indexToUpdate == TraderDetails.Count)
            {
                TraderDetails.Add(new TraderLayerInfo(traderName, volume));
            }
            else
            {
                var entryToUpdate = TraderDetails[indexToUpdate];
                entryToUpdate.TraderName = traderName;
                entryToUpdate.TraderVolume = volume;
            }
        }

        public bool RemoveAt(int index)
        {
            TraderDetails[index].Reset();
            return true;
        }

        public void SetTradersCountOnly(int numberOfTraders)
        {
            AssertMaxTraderSizeNotExceeded(numberOfTraders);
            Reset();
            for (int i = 0; i < numberOfTraders; i++)
            {
                this[i].TraderName = TraderCountTraderName;
                this[i].TraderVolume = 0m;
            }
        }

        public override void Reset()
        {
            base.Reset();
            foreach (var traderLayerInfo in TraderDetails)
            {
                traderLayerInfo?.Reset();
            }
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void AssertMaxTraderSizeNotExceeded(int proposedNewIndex)
        {
            if (proposedNewIndex > byte.MaxValue)
                throw new ArgumentOutOfRangeException($"Max Traders represented is {byte.MaxValue}");
        }

        public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source, copyMergeFlags);
            if (source is IMutableTraderPriceVolumeLayer sourceTraderPriceVolumeLayer)
            {
                for (int i = 0; i < sourceTraderPriceVolumeLayer.Count; i++)
                {
                    if (i < TraderDetails.Count)
                    {
                        if (TraderDetails[i] != null)
                        {
                            TraderDetails[i]?.CopyFrom(sourceTraderPriceVolumeLayer[i]);
                        }
                        else if (sourceTraderPriceVolumeLayer[i] != null)
                        {
                            TraderDetails[i] = new TraderLayerInfo(sourceTraderPriceVolumeLayer[i]);
                        }
                    }
                    else
                    {
                        TraderDetails.Add(new TraderLayerInfo(sourceTraderPriceVolumeLayer[i]));
                    }
                }
                for (int i = TraderDetails.Count - 1; i >= sourceTraderPriceVolumeLayer.Count; i--)
                {
                    TraderDetails[i].Reset();
                }
            }
        }

        ITraderPriceVolumeLayer ICloneable<ITraderPriceVolumeLayer>.Clone()
        {
            return (ITraderPriceVolumeLayer)Clone();
        }

        ITraderPriceVolumeLayer ITraderPriceVolumeLayer.Clone()
        {
            return (ITraderPriceVolumeLayer)Clone();
        }

        IMutableTraderPriceVolumeLayer IMutableTraderPriceVolumeLayer.Clone()
        {
            return (IMutableTraderPriceVolumeLayer)Clone();
        }

        public override IPriceVolumeLayer Clone()
        {
            return new TraderPriceVolumeLayer(this);
        }

        public override bool AreEquivalent(IPriceVolumeLayer other, bool exactTypes = false)
        {
            if (!(other is ITraderPriceVolumeLayer otherTvl)) return false;
            var baseSame = base.AreEquivalent(other, exactTypes);
            var traderDetailsSame = TraderDetails.Zip(otherTvl, (ftd, std) => ftd.AreEquivalent(std, exactTypes))
                .All(same => same);
            return baseSame && traderDetailsSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((ITraderPriceVolumeLayer)obj, true);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ TraderDetails.GetHashCode();
        }

        public override string ToString()
        {
            return $"TraderPriceVolumeLayer {{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
                   $"{Volume:N2}, {nameof(TraderDetails)}:[ {String.Join(", ", TraderDetails)}]," +
                   $" {nameof(Count)}: {Count} }}";
        }

        IEnumerator<ITraderLayerInfo> IEnumerable<ITraderLayerInfo>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IMutableTraderLayerInfo> GetEnumerator()
        {
            return TraderDetails.Take(Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}