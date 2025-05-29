// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

public interface IPQOrderBookSide : IMutableOrderBookSide, IPQSupportsNumberPrecisionFieldUpdates<IOrderBookSide>
  , IPQSupportsStringUpdates<IOrderBookSide>, ICloneable<IPQOrderBookSide>
  , IRelatedItems<LayerFlags, ushort>, IRelatedItems<INameIdLookupGenerator>
  , ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQOrderBookSide>
  , IMutableTracksShiftsList<IPQPriceVolumeLayer, IPriceVolumeLayer>

{
    new IPQPriceVolumeLayer this[int index] { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new bool IsReadOnly { get; }

    new ListShiftCommand ClearAll();

    new ListShiftCommand DeleteAt(int index);

    new ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply);

    new int Capacity { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new int Count { get; set; }

    IReadOnlyList<IPQPriceVolumeLayer> AllLayers { get; }

    new bool HasUpdates { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQMarketAggregate? OpenInterestSide { get; set; }

    bool IsDailyTickUpdateCountUpdated { get; set; }

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection);

    new ListShiftCommand ShiftElements(int byElements);

    new ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    new void ClearShiftCommands();

    new void Clear();

    new void RemoveAt(int index);

    new IPQOrderBookSide Clone();

    new IPQOrderBookSide ResetWithTracking();

    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBookSide : ReusableObject<IOrderBookSide>, IPQOrderBookSide
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBookSide));

    private readonly IList<IPQPriceVolumeLayer> allLayers;

    private readonly TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>? elementShiftRegistry;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;
    private IPQMarketAggregate?      pqOpenInterestSide;
    private LayerFlags               layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;

    protected OrderBookUpdatedFlags UpdatedFlags;

    protected uint   SequenceId    = uint.MaxValue;

    private   ushort maxPublishDepth = 1;
    private   uint   dailyTickUpdateCount;

    public PQOrderBookSide()
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide              = BookSide.Unknown;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator();
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers             = [new PQPriceVolumeLayer()];

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide              =  bookSide;
        LayerSupportedFlags   =  layerType.SupportedLayerFlags();
        LayerSupportedFlags   |= LayerSupportedType.SupportedLayerFlags();
        IsLadder              =  isLadder;
        nameIdLookupGenerator =  nameIdLookup ?? InitializeNewIdLookupGenerator();
        LayerSelector         =  new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        MaxAllowedSize        =  (ushort)numBookLayers;
        allLayers             =  new List<IPQPriceVolumeLayer>(MaxAllowedSize);
        for (var i = 0; i < MaxAllowedSize; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide
        (BookSide bookSide, ISourceTickerInfo srcTickerInfo, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide            =  bookSide;
        LayerSupportedFlags =  srcTickerInfo.LayerFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();
        MaxAllowedSize      =  srcTickerInfo.MaximumPublishedLayers;
        nameIdLookupGenerator = nameIdLookup ??
                                (srcTickerInfo is IPQSourceTickerInfo pqSrcTkrInfo
                                    ? InitializeNewIdLookupGenerator(pqSrcTkrInfo.NameIdLookup)
                                    : InitializeNewIdLookupGenerator());
        LayerSelector = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        allLayers = new List<IPQPriceVolumeLayer>(MaxAllowedSize);
        for (var i = 0; i < MaxAllowedSize; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            allLayers.Add(pqLayer);
        }
        EnsureRelatedItemsAreConfigured(nameIdLookup);

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide            = bookSide;
        LayerSupportedFlags = bookLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        IsLadder            = isLadder;
        MaxAllowedSize      = (ushort)(bookLayers?.Count() ?? 1);

        nameIdLookupGenerator = nameIdLookup ?? SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers =
            (bookLayers?.Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, pvl))
                       .OfType<IPQPriceVolumeLayer>()
                       .ToList() ?? [LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup)]
            );

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide(IOrderBookSide toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        elementShiftRegistry.CopyFrom(toClone.ShiftCommands);

        BookSide = toClone.BookSide;

        LayerSupportedFlags =  toClone.LayerSupportedFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();

        if (toClone.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide = new PQMarketAggregate(toClone.OpenInterestSide);
        }

        MaxAllowedSize        = toClone.MaxAllowedSize;
        DailyTickUpdateCount  = toClone.DailyTickUpdateCount;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator(nameIdLookup, toClone);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        var size = toClone.Capacity;
        allLayers = new List<IPQPriceVolumeLayer>(size);
        Capacity  = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var sourceLayer = toClone[i];
            var pqLayer     = LayerSelector.CreateExpectedImplementation(sourceLayer.LayerType, NameIdLookup, sourceLayer);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }


    protected static Func<IPriceVolumeLayer, IPriceVolumeLayer, bool> SamePrice = (lhs, rhs) => lhs.Price == rhs.Price;

    protected Func<IPQPriceVolumeLayer> NewElementFactory => () => LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);

    ushort IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    ushort IMutableTracksShiftsList<IPQPriceVolumeLayer, IPriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    ushort IPQOrderBookSide.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    ushort IMutableCappedCapacityList<IMutablePriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }
    ushort IMutableCappedCapacityList<IPQPriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    public ushort MaxAllowedSize
    {
        get => maxPublishDepth;
        private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; }

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public LayerType LayerSupportedType => LayerSupportedFlags.MostCompactLayerType();

    public LayerFlags LayerSupportedFlags
    {
        get => layerFlags;
        set => layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;
    }

    public bool IsLadder
    {
        get => layerFlags.HasLadder();
        set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
    }

    public BookSide BookSide { get; }

    IMutablePriceVolumeLayer IReadOnlyList<IMutablePriceVolumeLayer>.this[int index] => this[index];

    IPriceVolumeLayer IReadOnlyList<IPriceVolumeLayer>.this[int level] => this[level];

    IMutablePriceVolumeLayer IMutableOrderBookSide.this[int level]
    {
        get => this[level];
        set => this[level] = (IPQPriceVolumeLayer)value;
    }

    IPriceVolumeLayer IOrderBookSide.this[int index] => this[index];

    IMutablePriceVolumeLayer IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQPriceVolumeLayer)value;
    }

    IMutablePriceVolumeLayer IList<IMutablePriceVolumeLayer>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQPriceVolumeLayer)value;
    }

    IMutablePriceVolumeLayer IMutableCapacityList<IMutablePriceVolumeLayer>.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQPriceVolumeLayer)value;
    }


    public IPQPriceVolumeLayer this[int level]
    {
        get => level < AllLayers.Count && level >= 0 ? AllLayers[level] : AllLayers[0];
        set
        {
            if (value is ISupportsPQNameIdLookupGenerator setNameIdLookup)
            {
                setNameIdLookup.NameIdLookup = NameIdLookup;
            }
            HasUnreliableListTracking |= ShiftCommands.Any() && !ReferenceEquals(allLayers[level], value);
            allLayers[level]          = value;
        }
    }
    public IReadOnlyList<IPQPriceVolumeLayer> AllLayers => allLayers.AsReadOnly();

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            if (nameIdLookupGenerator == value) return;
            nameIdLookupGenerator = value;
            LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

            foreach (var pqPriceVolumeLayer in allLayers.OfType<ISupportsPQNameIdLookupGenerator>()) pqPriceVolumeLayer.NameIdLookup = value;
        }
    }

    public int Capacity
    {
        get => Math.Min(MaxAllowedSize, AllLayers.Count);
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected PQOrderBook Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth);

            for (var i = AllLayers.Count; i < value && AllLayers.Count > 0; i++)
            {
                var clonedFirstLayer = AllLayers[0].Clone();
                clonedFirstLayer.StateReset();
                allLayers.Add(clonedFirstLayer);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = Math.Min(MaxAllowedSize, AllLayers.Count - 1); i >= 0; i--)
            {
                var layerAtLevel = AllLayers[i];
                if ((layerAtLevel.Price) > 0) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = Capacity; i >= value; i--)
            {
                var layerAtLevel = AllLayers[i];
                layerAtLevel.IsEmpty = true;
            }
        }
    }

    public bool IsEmpty
    {
        get => AllLayers.All(pvl => pvl.IsEmpty);
        set
        {
            foreach (var priceVolumeLayer in AllLayers)
            {
                priceVolumeLayer.IsEmpty = value;
            }
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry!.ShiftCommands;
        set => elementShiftRegistry!.ShiftCommands = (List<ListShiftCommand>)value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry!.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry!.ClearRemainingElementsFromIndex = (ushort?)value;
    }

    public bool HasUnreliableListTracking { get; set; }

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection) =>
        elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IPQPriceVolumeLayer, IPriceVolumeLayer>.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IPQOrderBookSide.AppendShiftCommand(ListShiftCommand toAppendAtEnd) => elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableOrderBookSide.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    void IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.ClearShiftCommands() => elementShiftRegistry!.ClearShiftCommands();

    void IMutableTracksShiftsList<IPQPriceVolumeLayer, IPriceVolumeLayer>.ClearShiftCommands() => elementShiftRegistry!.ClearShiftCommands();

    void IMutableOrderBookSide.ClearShiftCommands() => elementShiftRegistry!.ClearShiftCommands();

    void IPQOrderBookSide.ClearShiftCommands() => elementShiftRegistry!.ClearShiftCommands();

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.InsertAtStart
        (IMutablePriceVolumeLayer toInsertAtStart) =>
        elementShiftRegistry!.InsertAtStart((IPQPriceVolumeLayer)toInsertAtStart);

    bool IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.AppendAtEnd
        (IMutablePriceVolumeLayer toAppendAtEnd) =>
        elementShiftRegistry!.AppendAtEnd((IPQPriceVolumeLayer)toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.InsertAt
        (int index, IMutablePriceVolumeLayer toInsertAtStart) =>
        elementShiftRegistry!.InsertAt(index, (IPQPriceVolumeLayer)toInsertAtStart);

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.Delete
        (IMutablePriceVolumeLayer toDelete) =>
        elementShiftRegistry!.Delete((IPQPriceVolumeLayer)toDelete);

    public ListShiftCommand ApplyListShiftCommand
        (ListShiftCommand shiftCommandToApply) =>
        elementShiftRegistry!.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementShiftRegistry!.ClearAll();

    public ListShiftCommand ShiftElements(int byElements) => elementShiftRegistry!.ShiftElements(byElements);

    public ListShiftCommand InsertAtStart(IPQPriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IPQPriceVolumeLayer toAppendAtEnd) => elementShiftRegistry!.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IPQPriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    public ListShiftCommand Delete(IPQPriceVolumeLayer toDelete) => elementShiftRegistry!.Delete(toDelete);

    public void Add(IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Add(item);
    }

    public bool Contains(IPQPriceVolumeLayer item) => allLayers.Contains(item);

    public void CopyTo(IPQPriceVolumeLayer[] array, int arrayIndex)
    {
        for (int i = 0; i < allLayers.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = allLayers[i];
        }
    }

    public bool Remove(IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        return allLayers.Remove(item);
    }

    public int IndexOf(IPQPriceVolumeLayer item) => allLayers.IndexOf(item);

    public void Insert(int index, IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Insert(index, item);
    }

    public void Add(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Add((IPQPriceVolumeLayer)item);
    }

    public void Clear()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Clear();
    }

    public bool Contains(IMutablePriceVolumeLayer item) => allLayers.Contains((IPQPriceVolumeLayer)item);

    public void CopyTo(IMutablePriceVolumeLayer[] array, int arrayIndex)
    {
        for (int i = 0; i < allLayers.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = allLayers[i];
        }
    }

    public bool Remove(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        return allLayers.Remove((IPQPriceVolumeLayer)item);
    }

    public bool IsReadOnly => false;

    public int IndexOf(IMutablePriceVolumeLayer item) => allLayers.IndexOf((IPQPriceVolumeLayer)item);

    public void Insert(int index, IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Insert(index, (IPQPriceVolumeLayer)item);
    }

    public void RemoveAt(int index)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.RemoveAt(index);
    }

    IEnumerator<IMutablePriceVolumeLayer> IMutableOrderBookSide.                GetEnumerator() => GetEnumerator();

    IEnumerator<IMutablePriceVolumeLayer> IEnumerable<IMutablePriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public bool HasNonEmptyOpenInterest
    {
        get => pqOpenInterestSide is { IsEmpty: false };
        set
        {
            if (value) return;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBookSide.OpenInterestSide => OpenInterestSide!;
    IMutableMarketAggregate? IMutableOrderBookSide.OpenInterestSide
    {
        get => OpenInterestSide;
        set => OpenInterestSide = (IPQMarketAggregate?)value;
    }

    public IPQMarketAggregate? OpenInterestSide
    {
        get
        {
            if (HasNonEmptyOpenInterest && pqOpenInterestSide is { DataSource: not (MarketDataSource.None or MarketDataSource.Published) })
                return pqOpenInterestSide;

            var vwapResult = this.CalculateVwap();

            pqOpenInterestSide            ??= new PQMarketAggregate();
            pqOpenInterestSide.DataSource =   MarketDataSource.Published;
            pqOpenInterestSide.UpdateTime =   DateTime.Now;
            pqOpenInterestSide.Volume     =   vwapResult.VolumeAchieved;
            pqOpenInterestSide.Vwap       =   vwapResult.AchievedVwap;
            return pqOpenInterestSide;
        }
        set
        {
            if (value != null)
            {
                pqOpenInterestSide ??= new PQMarketAggregate();

                pqOpenInterestSide.DataSource = value.DataSource;
                pqOpenInterestSide.UpdateTime = value.UpdateTime;
                pqOpenInterestSide.Volume     = value.Volume;
                pqOpenInterestSide.Vwap       = value.Vwap;
            }
            else if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.IsEmpty = true;
            }
        }
    }

    public uint DailyTickUpdateCount
    {
        get => dailyTickUpdateCount;
        set
        {
            IsDailyTickUpdateCountUpdated |= value != dailyTickUpdateCount || SequenceId == 0;
            dailyTickUpdateCount          =  value;
        }
    }

    public bool IsDailyTickUpdateCountUpdated
    {
        get => (UpdatedFlags & OrderBookUpdatedFlags.IsDailyTickCountUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookUpdatedFlags.IsDailyTickCountUpdated;

            else if (IsDailyTickUpdateCountUpdated) UpdatedFlags ^= OrderBookUpdatedFlags.IsDailyTickCountUpdated;
        }
    }

    public bool HasUpdates
    {
        get
        {
            return UpdatedFlags != OrderBookUpdatedFlags.None
                || (HasNonEmptyOpenInterest
                 && pqOpenInterestSide is { DataSource: not (MarketDataSource.None or MarketDataSource.Published), HasUpdates: true })
                || AllLayers.Any(pqpvl => pqpvl.HasUpdates);
        }
        set
        {
            foreach (var pqPvLayer in AllLayers) pqPvLayer.HasUpdates = value;
            NameIdLookup.HasUpdates = value;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.HasUpdates = value;
            }
            if (value) return;
            UpdatedFlags = OrderBookUpdatedFlags.None;
        }
    }

    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
        foreach (var priceVolumeLayer in AllLayers)
        {
            priceVolumeLayer.UpdateStarted(updateSequenceId);
        }
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        foreach (var pqPriceVolumeLayer in AllLayers) pqPriceVolumeLayer.UpdateComplete();
        elementShiftRegistry!.ClearShiftCommands();
        NameIdLookup.UpdateComplete(updateSequenceId);
        pqOpenInterestSide?.UpdateComplete(updateSequenceId);
        if (HasUpdates)
        {
            SequenceId++;
            HasUpdates = false;
        }
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsDailyTickUpdateCountUpdated)
        {
            yield return new PQFieldUpdate(PQFeedFields.QuoteDailySidedTickCount, DailyTickUpdateCount);
        }
        if (pqOpenInterestSide != null)
        {
            foreach (var oiUpdates in pqOpenInterestSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
            {
                yield return oiUpdates.WithFieldId(PQFeedFields.QuoteOpenInterestSided);
            }
        }
        for (var i = 0; i < AllLayers.Count; i++)
            if (this[i] is { } currentLayer)
                foreach (var layerFields in currentLayer.GetDeltaUpdateFields(snapShotTime,
                                                                              messageFlags, quotePublicationPrecisionSetting))
                {
                    var positionUpdate = layerFields.AtDepth((ushort)i);
                    yield return positionUpdate;
                }
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFeedFields.QuoteDailySidedTickCount)
        {
            IsDailyTickUpdateCountUpdated = true;

            DailyTickUpdateCount = pqFieldUpdate.Payload;
            return 0;
        }
        if (pqFieldUpdate.Id is PQFeedFields.QuoteOpenInterestSided)
        {
            pqOpenInterestSide ??= new PQMarketAggregate();
            return pqOpenInterestSide.UpdateField(pqFieldUpdate);
        }
        if (pqFieldUpdate.Id is >= PQFeedFields.QuoteLayerPrice and < PQFeedFields.QuoteLayersRangeEnd)
        {
            var index              = pqFieldUpdate.DepthIndex();
            var pqPriceVolumeLayer = this[index];
            return pqPriceVolumeLayer.UpdateField(pqFieldUpdate);
        }
        return -1;
    }

    object ICloneable.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    public override IPQOrderBookSide Clone() => (IPQOrderBookSide?)Recycler?.Borrow<PQOrderBookSide>().CopyFrom(this) ?? new PQOrderBookSide(this);

    public override IOrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!copyMergeFlags.HasSkipReferenceLookups() && source is PQOrderBookSide sourcePqOrderBook)
            NameIdLookup.CopyFrom(sourcePqOrderBook.NameIdLookup, copyMergeFlags);
        copyMergeFlags = copyMergeFlags.AddSkipReferenceLookups();
        if (source.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide ??= new PQMarketAggregate();
            pqOpenInterestSide.CopyFrom(source.OpenInterestSide, copyMergeFlags);
        }
        else if (pqOpenInterestSide != null)
        {
            pqOpenInterestSide.IsEmpty = true;
        }
        DailyTickUpdateCount = source.DailyTickUpdateCount;
        LayerSupportedFlags  = source.LayerSupportedFlags;
        MaxAllowedSize       = source.MaxAllowedSize;
        var originalCount = Count;

        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer = source[i];
            if (sourceLayer is null or { IsEmpty: true } && i >= originalCount) continue;
            var destinationLayer = this[i];

            if (i < AllLayers.Count)
                allLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, NameIdLookup, LayerSupportedType, sourceLayer, copyMergeFlags);
            else
                allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, sourceLayer, copyMergeFlags));
            if (sourceLayer is { IsEmpty: false }) continue;
            if (destinationLayer is IMutablePriceVolumeLayer mutableDestinationLayer) mutableDestinationLayer.IsEmpty = true;
        }

        for (var i = source.Count; i < AllLayers.Count; i++)
            if (AllLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    public int AppendEntryAtEnd()
    {
        var index = AllLayers.Count;
        allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup));
        return index;
    }

    public void EnsureRelatedItemsAreConfigured(INameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null && !ReferenceEquals(NameIdLookup, nameIdLookupGenerator))
        {
            NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
            if (NameIdLookup.Count != otherNameIdLookupGenerator.Count) NameIdLookup.CopyFrom(otherNameIdLookupGenerator, CopyMergeFlags.FullReplace);
        }
    }

    public void EnsureRelatedItemsAreConfigured(LayerFlags newLayerFlags, ushort bookDepth)
    {
        LayerSupportedFlags = newLayerFlags;
        MaxAllowedSize      = bookDepth;

        var layerFactory = LayerSelector.FindForLayerFlags(LayerSupportedFlags);

        for (var i = 0; i < MaxAllowedSize; i++)
            if (i >= AllLayers.Count)
                allLayers.Add(layerFactory.CreateNewLayer());
            else if (i < AllLayers.Count && AllLayers[i] is { } currentLayer
                                         && !LayerSelector.OriginalCanWhollyContain(LayerSupportedFlags
                                                                                  , currentLayer.SupportsLayerFlags))
                allLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = MaxAllowedSize; i < AllLayers.Count; i++) allLayers.RemoveAt(i);
    }

    public virtual bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var layerFlagsSame    = LayerSupportedFlags == other.LayerSupportedFlags;
        var deepestPricedSame = Count == other.Count;
        var maxPubDepthSame   = MaxAllowedSize == other.MaxAllowedSize;
        var openInterestSame  = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = pqOpenInterestSide?.AreEquivalent(other.OpenInterestSide, exactTypes) ?? false;
        }
        var bookLayersSame = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer.AreEquivalent(otherLayer, exactTypes);
            bookLayersSame &= layerIsSame;
        }

        var deepestPossibleSame = true;

        // if (other is IMutableOrderBookSide mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        var allAreSame = layerFlagsSame && deepestPossibleSame && deepestPricedSame
                      && maxPubDepthSame && bookLayersSame && openInterestSame;
        return allAreSame;
    }

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    IPQOrderBookSide ITrackableReset<IPQOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    IPQOrderBookSide IPQOrderBookSide.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutablePriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IMutablePriceVolumeLayer>>.
        ResetWithTracking() =>
        ResetWithTracking();

    ITracksResetCappedCapacityList<IPQPriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IPQPriceVolumeLayer>>.ResetWithTracking() =>
        ResetWithTracking();

    IMutableOrderBookSide IMutableOrderBookSide.ResetWithTracking() => ResetWithTracking();

    public PQOrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i].ResetWithTracking();
        pqOpenInterestSide?.ResetWithTracking();
        DailyTickUpdateCount = 0;

        return this;
    }

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i].StateReset();
        DailyTickUpdateCount = 0;
        NameIdLookup.Clear();
        pqOpenInterestSide?.StateReset();
        SequenceId = 0;
        UpdatedFlags = OrderBookUpdatedFlags.None;
        base.StateReset();
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (AllLayers.Count <= 0) return [];
        // All layers share same dictionary or should do anyway
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        // All layers share same dictionary or should do anyway
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
    {
        IPQNameIdLookupGenerator thisLayDict;
        if (source is IPQOrderBookSide pqOrderBook)
            thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
        else
            thisLayDict = InitializeNewIdLookupGenerator
                (
                 source?.OfType<ISupportsPQNameIdLookupGenerator>()
                       .Select(pvl => pvl.NameIdLookup)
                       .FirstOrDefault());

        return thisLayDict;
    }

    public IPQNameIdLookupGenerator InitializeNewIdLookupGenerator(IPQNameIdLookupGenerator? optionalExisting = null, IOrderBookSide? clone = null)
    {
        IPQNameIdLookupGenerator thisBookNameIdLookupGenerator = optionalExisting != null
            ? new PQNameIdLookupGenerator(optionalExisting, PQFeedFields.QuoteLayerStringUpdates)
            : clone is IPQOrderBookSide pqClone
                ? new PQNameIdLookupGenerator(pqClone.NameIdLookup, PQFeedFields.QuoteLayerStringUpdates)
                : new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        return thisBookNameIdLookupGenerator;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBookSide, true);

    public override int GetHashCode() => AllLayers.GetHashCode();

    protected string PQOrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(MaxAllowedSize)}: {MaxAllowedSize}, {nameof(Count)}: {Count}, {nameof(IsLadder)}: {IsLadder}, " +
        $"{nameof(OpenInterestSide)}: {OpenInterestSide}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    public override string ToString() => $"{GetType().Name}({PQOrderBookSideToStringMembers})";
}
