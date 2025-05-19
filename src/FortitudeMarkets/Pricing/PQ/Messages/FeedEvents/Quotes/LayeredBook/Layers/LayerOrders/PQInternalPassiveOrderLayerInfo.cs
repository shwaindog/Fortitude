using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

public interface IPQInternalPassiveOrderLayerInfo : IMutableInternalPassiveOrderLayerInfo, ISupportsPQNameIdLookupGenerator
  , IPQSupportsStringUpdates<IPQInternalPassiveOrderLayerInfo>, ICloneable<IPQInternalPassiveOrderLayerInfo>
{
    bool IsOrderSequenceIdUpdated      { get; set; }
    bool IsParentOrderIdUpdated        { get; set; }
    bool IsClosingOrderIdUpdated       { get; set; }
    bool IsClosingOrderPriceUpdated    { get; set; }
    bool IsDivisionIdUpdated           { get; set; }
    int  DivisionNameId            { get; set; }
    bool IsDivisionNameUpdated         { get; set; }
    bool IsDeskIdUpdated               { get; set; }
    int  DeskNameId                    { get; set; }
    bool IsDeskNameUpdated             { get; set; }
    bool IsStrategyIdUpdated           { get; set; }
    int  StrategyNameId                { get; set; }
    bool IsStrategyNameUpdated         { get; set; }
    bool IsStrategyDecisionIdUpdated   { get; set; }
    int  StrategyDecisionNameId        { get; set; }
    bool IsStrategyDecisionNameUpdated { get; set; }
    bool IsPortfolioIdUpdated          { get; set; }
    int  PortfolioNameId               { get; set; }
    bool IsPortfolioNameUpdated        { get; set; }
    bool IsInternalTraderIdUpdated     { get; set; }
    int  InternalTraderNameId          { get; set; }
    bool IsInternalTraderNameUpdated   { get; set; }
    bool IsMarginConsumedUpdated       { get; set; }

    bool IsDecisionCreatedDateUpdated        { get; set; }
    bool IsDecisionCreatedSub2MinTimeUpdated { get; set; }
    bool IsDecisionAmendDateUpdated          { get; set; }
    bool IsDecisionAmendSub2MinTimeUpdated   { get; set; }


    new IPQInternalPassiveOrderLayerInfo Clone();
}

[Flags]
public enum InternalPassiveOrderLayerInfoUpdatedFlags : uint
{
    None                           = 0x00_00_00_00
  , OrderSequenceIdFlag            = 0x00_00_00_01
  , ParentOrderIdFlag              = 0x00_00_00_02
  , ClosingOrderIdFlag             = 0x00_00_00_04
  , ClosingOrderPriceFlag          = 0x00_00_00_08
  , DecisionCreatedDateFlag        = 0x00_00_00_10
  , DecisionCreatedSub2MinTimeFlag = 0x00_00_00_20
  , DecisionAmendDateFlag          = 0x00_00_00_40
  , DecisionAmendSub2MinTimeFlag   = 0x00_00_00_80
  , DivisionIdFlag                 = 0x00_00_01_00
  , DivisionNameFlag               = 0x00_00_02_00
  , DeskIdFlag                     = 0x00_00_04_00
  , DeskNameFlag                   = 0x00_00_08_00
  , StrategyIdFlag                 = 0x00_00_10_00
  , StrategyNameFlag               = 0x00_00_20_00
  , StrategyDecisionIdFlag         = 0x00_00_40_00
  , StrategyDecisionNameFlag       = 0x00_00_80_00
  , PortfolioIdFlag                = 0x00_01_00_00
  , PortfolioNameFlag              = 0x00_02_00_00
  , InternalTraderIdFlag           = 0x00_04_00_00
  , InternalTraderNameFlag         = 0x00_08_00_00
  , MarginConsumedFlag             = 0x00_10_00_00
}

public class PQInternalPassiveOrderLayerInfo : PQAnonymousOrderLayerInfo, IPQInternalPassiveOrderLayerInfo
{
    private IPQNameIdLookupGenerator nameIdLookup = null!;

    protected InternalPassiveOrderLayerInfoUpdatedFlags IPOUpdatedFlags;

    private uint     orderSequenceId;
    private uint     parentOrderId;
    private uint     closingOrderId;
    private decimal  closingOrderPrice;
    private DateTime decisionCreatedTime;
    private DateTime decisionAmendTime;
    private uint     divisionId;
    private int      divisionNameId;
    private uint     deskId;
    private int      deskNameId;
    private uint     strategyId;
    private int      strategyNameId;
    private uint     strategyDecisionId;
    private int      strategyDecisionNameId;
    private uint     portfolioId;
    private int      portfolioNameId;
    private uint     internalTraderId;
    private int      internalTraderNameId;
    private decimal  marginConsumed;

    public PQInternalPassiveOrderLayerInfo()
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQInternalPassiveOrderLayerInfo(IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQInternalPassiveOrderLayerInfo
    (IPQNameIdLookupGenerator lookupDict, int orderId = 0, DateTime createdTime = default, decimal orderVolume = 0m
      , OrderFlags typeFlags = OrderFlags.None, OrderType orderType = OrderType.None, LayerOrderFlags orderFlags = LayerOrderFlags.None
      , OrderLifeCycleState orderLifeCycleState = OrderLifeCycleState.None, DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
        : base(orderId, createdTime, orderVolume, orderFlags, orderType, typeFlags, orderLifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        NameIdLookup = lookupDict;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQInternalPassiveOrderLayerInfo(IAnonymousOrderLayerInfo toClone, IPQNameIdLookupGenerator pqNameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (toClone is IInternalPassiveOrder internalPassiveOrder)
        {
            OrderSequenceId      = internalPassiveOrder.OrderSequenceId;
            ParentOrderId        = internalPassiveOrder.ParentOrderId;
            ClosingOrderId       = internalPassiveOrder.ClosingOrderId;
            ClosingOrderPrice    = internalPassiveOrder.ClosingOrderPrice;
            DecisionCreatedTime  = internalPassiveOrder.DecisionCreatedTime;
            DecisionAmendTime    = internalPassiveOrder.DecisionAmendTime;
            DivisionId           = internalPassiveOrder.DivisionId;
            DivisionName         = internalPassiveOrder.DivisionName;
            DeskId               = internalPassiveOrder.DeskId;
            DeskName             = internalPassiveOrder.DeskName;
            StrategyId           = internalPassiveOrder.StrategyId;
            StrategyName         = internalPassiveOrder.StrategyName;
            StrategyDecisionId   = internalPassiveOrder.StrategyDecisionId;
            StrategyDecisionName = internalPassiveOrder.StrategyDecisionName;
            PortfolioId          = internalPassiveOrder.PortfolioId;
            PortfolioName        = internalPassiveOrder.PortfolioName;
            InternalTraderId     = internalPassiveOrder.InternalTraderId;
            InternalTraderName   = internalPassiveOrder.InternalTraderName;
            MarginConsumed       = internalPassiveOrder.MarginConsumed;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }


    public uint OrderSequenceId
    {
        get => orderSequenceId;
        set
        {
            IsOrderSequenceIdUpdated |= value != orderSequenceId || NumUpdatesSinceEmpty == 0;
            orderSequenceId          =  value;
        }
    }
    public uint ParentOrderId
    {
        get => parentOrderId;
        set
        {
            IsParentOrderIdUpdated |= value != parentOrderId || NumUpdatesSinceEmpty == 0;
            parentOrderId          =  value;
        }
    }
    public uint ClosingOrderId
    {
        get => closingOrderId;
        set
        {
            IsClosingOrderIdUpdated |= value != closingOrderId || NumUpdatesSinceEmpty == 0;
            closingOrderId          =  value;
        }
    }
    public decimal ClosingOrderPrice
    {
        get => closingOrderPrice;
        set
        {
            IsClosingOrderPriceUpdated |= value != closingOrderPrice || NumUpdatesSinceEmpty == 0;
            closingOrderPrice          =  value;
        }
    }
    public DateTime DecisionCreatedTime
    {
        get => decisionCreatedTime;
        set
        {
            IsDecisionCreatedDateUpdated |= value.Get2MinIntervalsFromUnixEpoch() != decisionCreatedTime.Get2MinIntervalsFromUnixEpoch() ||
                                            NumUpdatesSinceEmpty == 0;
            IsDecisionCreatedSub2MinTimeUpdated
                |= value.GetSub2MinComponent() != decisionCreatedTime.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            decisionCreatedTime = value;
        }
    }
    public DateTime DecisionAmendTime
    {
        get => decisionAmendTime;
        set
        {
            IsDecisionAmendDateUpdated |= value.Get2MinIntervalsFromUnixEpoch() != decisionAmendTime.Get2MinIntervalsFromUnixEpoch() ||
                                          NumUpdatesSinceEmpty == 0;
            IsDecisionAmendSub2MinTimeUpdated |= value.GetSub2MinComponent() != decisionAmendTime.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            decisionAmendTime                 =  value;
        }
    }
    public uint DivisionId
    {
        get => divisionId;
        set
        {
            IsDivisionIdUpdated |= value != divisionId || NumUpdatesSinceEmpty == 0;
            divisionId          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int DivisionNameId
    {
        get => divisionNameId;
        set
        {
            IsDivisionNameUpdated |= divisionNameId != value || NumUpdatesSinceEmpty == 0;
            divisionNameId        =  value;
        }
    }

    public string? DivisionName
    {
        get => NameIdLookup[divisionNameId];
        set
        {
            var convertedDivisionNameId = NameIdLookup.GetOrAddId(value);
            if (convertedDivisionNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Division Name to something " +
                                    "not defined in the source name to Id table.");
            DivisionNameId = convertedDivisionNameId;
        }
    }

    public uint    DeskId
    {
        get => deskId;
        set
        {
            IsDeskIdUpdated |= value != deskId || NumUpdatesSinceEmpty == 0;
            deskId              =  value;
        }
    }

    public int DeskNameId
    {
        get => deskNameId;
        set
        {
            IsDeskNameUpdated |= deskNameId != value || NumUpdatesSinceEmpty == 0;
            deskNameId            =  value;
        }
    }

    public string? DeskName
    {
        get => NameIdLookup[deskNameId];
        set
        {
            var convertedDeskNameId = NameIdLookup.GetOrAddId(value);
            if (convertedDeskNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Desk Name to something " +
                                    "not defined in the source name to Id table.");
            DeskNameId = convertedDeskNameId;
        }
    }

    public uint    StrategyId
    {
        get => strategyId;
        set
        {
            IsStrategyIdUpdated |= value != strategyId || NumUpdatesSinceEmpty == 0;
            strategyId      =  value;
        }
    }

    public int StrategyNameId
    {
        get => strategyNameId;
        set
        {
            IsStrategyNameUpdated |= strategyNameId != value || NumUpdatesSinceEmpty == 0;
            strategyNameId    =  value;
        }
    }

    public string? StrategyName 
    {
        get => NameIdLookup[strategyNameId];
        set
        {
            var convertedStrategyNameId = NameIdLookup.GetOrAddId(value);
            if (convertedStrategyNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Strategy Name to something " +
                                    "not defined in the source name to Id table.");
            StrategyNameId = convertedStrategyNameId;
        }
    }

    public uint    StrategyDecisionId
    {
        get => strategyDecisionId;
        set
        {
            IsStrategyDecisionIdUpdated |= strategyDecisionId != value || NumUpdatesSinceEmpty == 0;
            strategyDecisionId    =  value;
        }
    }

    public int StrategyDecisionNameId
    {
        get => strategyDecisionNameId;
        set
        {
            IsStrategyDecisionNameUpdated  |= strategyDecisionNameId != value || NumUpdatesSinceEmpty == 0;
            strategyDecisionNameId =  value;
        }
    }
    public string? StrategyDecisionName
    {
        get => NameIdLookup[strategyDecisionNameId];
        set
        {
            var convertedStrategyDecisionNameId = NameIdLookup.GetOrAddId(value);
            if (convertedStrategyDecisionNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Strategy Decision Name to something " +
                                    "not defined in the source name to Id table.");
            StrategyDecisionNameId = convertedStrategyDecisionNameId;
        }
    }

    public uint    PortfolioId
    {
        get => portfolioId;
        set
        {
            IsPortfolioIdUpdated |= portfolioId != value || NumUpdatesSinceEmpty == 0;
            portfolioId                 =  value;
        }
    }

    public int PortfolioNameId
    {
        get => portfolioNameId;
        set
        {
            IsPortfolioNameUpdated |= portfolioNameId != value || NumUpdatesSinceEmpty == 0;
            portfolioNameId               =  value;
        }
    }
    public string? PortfolioName
    {
        get => NameIdLookup[portfolioNameId];
        set
        {
            var convertedPortfolioNameId = NameIdLookup.GetOrAddId(value);
            if (convertedPortfolioNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Portfolio Name to something " +
                                    "not defined in the source name to Id table.");
            PortfolioNameId = convertedPortfolioNameId;
        }
    }
    public uint    InternalTraderId
    {
        get => internalTraderId;
        set
        {
            IsInternalTraderIdUpdated |= internalTraderId != value || NumUpdatesSinceEmpty == 0;
            internalTraderId     =  value;
        }
    }

    public int InternalTraderNameId
    {
        get => internalTraderNameId;
        set
        {
            IsInternalTraderNameUpdated |= internalTraderNameId != value || NumUpdatesSinceEmpty == 0;
            internalTraderNameId   =  value;
        }
    }

    public string? InternalTraderName
    {
        get => NameIdLookup[internalTraderNameId];
        set
        {
            var convertedInternalTraderNameId = NameIdLookup.GetOrAddId(value);
            if (convertedInternalTraderNameId <= 0 && value != null)
                throw new Exception("Error attempted to set the Internal Trader Name to something " +
                                    "not defined in the source name to Id table.");
            InternalTraderNameId = convertedInternalTraderNameId;
        }
    }
    public decimal MarginConsumed
    {
        get => marginConsumed;
        set
        {
            IsMarginConsumedUpdated |= marginConsumed != value || NumUpdatesSinceEmpty == 0;
            marginConsumed            =  value;
        }
    }


    public bool IsOrderSequenceIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.OrderSequenceIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.OrderSequenceIdFlag;

            else if (IsOrderSequenceIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.OrderSequenceIdFlag;
        }
    }

    public bool IsParentOrderIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.ParentOrderIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.ParentOrderIdFlag;

            else if (IsParentOrderIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.ParentOrderIdFlag;
        }
    }
    public bool IsClosingOrderIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderIdFlag;

            else if (IsClosingOrderIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderIdFlag;
        }
    }
    public bool IsClosingOrderPriceUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderPriceFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderPriceFlag;

            else if (IsClosingOrderPriceUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.ClosingOrderPriceFlag;
        }
    }
    public bool IsDivisionIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DivisionIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DivisionIdFlag;

            else if (IsDivisionIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DivisionIdFlag;
        }
    }
    public bool IsDivisionNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DivisionNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DivisionNameFlag;

            else if (IsDivisionNameUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DivisionNameFlag;
        }
    }
    public bool IsDeskIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DeskIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DeskIdFlag;

            else if (IsDeskIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DeskIdFlag;
        }
    }
    public bool IsDeskNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DeskNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DeskNameFlag;

            else if (IsDeskNameUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DeskNameFlag;
        }
    }

    public bool IsStrategyIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.StrategyIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyIdFlag;

            else if (IsStrategyIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyIdFlag;
        }
    }

    public bool IsStrategyNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.StrategyNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyNameFlag;

            else if (IsStrategyNameUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyNameFlag;
        }
    }
    public bool IsStrategyDecisionIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionIdFlag;

            else if (IsStrategyDecisionIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionIdFlag;
        }
    }

    public bool IsStrategyDecisionNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionNameFlag;

            else if (IsStrategyDecisionIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.StrategyDecisionNameFlag;
        }
    }

    public bool IsPortfolioIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioIdFlag;

            else if (IsPortfolioIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioIdFlag;
        }
    }
    public bool IsPortfolioNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioNameFlag;

            else if (IsPortfolioNameUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.PortfolioNameFlag;
        }
    }
    public bool IsInternalTraderIdUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderIdFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderIdFlag;

            else if (IsInternalTraderIdUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderIdFlag;
        }
    }

    public bool IsInternalTraderNameUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderNameFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderNameFlag;

            else if (IsInternalTraderNameUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.InternalTraderNameFlag;
        }
    }
    public bool IsMarginConsumedUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.MarginConsumedFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.MarginConsumedFlag;

            else if (IsMarginConsumedUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.MarginConsumedFlag;
        }
    }
    public bool IsDecisionCreatedDateUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedDateFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedDateFlag;

            else if (IsDecisionCreatedDateUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedDateFlag;
        }
    }
    public bool IsDecisionCreatedSub2MinTimeUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag;

            else if (IsDecisionCreatedSub2MinTimeUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag;
        }
    }
    public bool IsDecisionAmendDateUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendDateFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendDateFlag;

            else if (IsDecisionAmendDateUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendDateFlag;
        }
    }
    public bool IsDecisionAmendSub2MinTimeUpdated
    {
        get => (IPOUpdatedFlags & InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag) > 0;
        set
        {
            if (value)
                IPOUpdatedFlags |= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag;

            else if (IsDecisionAmendSub2MinTimeUpdated) IPOUpdatedFlags ^= InternalPassiveOrderLayerInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag;
        }
    }

    [JsonIgnore] INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    [JsonIgnore]
    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            string? cacheDivisionName               = null;
            if (divisionNameId > 0) cacheDivisionName = DivisionName;
            string? cacheDeskName                     = null;
            if (deskNameId > 0) cacheDeskName       = DeskName;
            string? strategyName                     = null;
            if (strategyNameId > 0) strategyName       = StrategyName;
            string? strategyDecisionName                     = null;
            if (strategyDecisionNameId > 0) strategyDecisionName       = StrategyDecisionName;
            string? portfolioName                     = null;
            if (portfolioNameId > 0) portfolioName       = PortfolioName;
            string? internalTraderName                     = null;
            if (internalTraderNameId > 0) internalTraderName       = InternalTraderName;
            nameIdLookup = value;
            if (cacheDivisionName != null && divisionNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(divisionNameId, cacheDivisionName);
                }
                catch
                {
                    DivisionNameId = nameIdLookup.GetOrAddId(cacheDivisionName);
                }
            if (cacheDeskName != null && deskNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(deskNameId, cacheDeskName);
                }
                catch
                {
                    DeskNameId = nameIdLookup.GetOrAddId(cacheDeskName);
                }
            if (strategyName != null && strategyNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(strategyNameId, strategyName);
                }
                catch
                {
                    StrategyNameId = nameIdLookup.GetOrAddId(strategyName);
                }
            if (strategyDecisionName != null && strategyDecisionNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(strategyDecisionNameId, strategyDecisionName);
                }
                catch
                {
                    StrategyDecisionNameId = nameIdLookup.GetOrAddId(strategyDecisionName);
                }
            if (portfolioName != null && portfolioNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(portfolioNameId, portfolioName);
                }
                catch
                {
                    PortfolioNameId = nameIdLookup.GetOrAddId(portfolioName);
                }
            if (internalTraderName != null && internalTraderNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(internalTraderNameId, internalTraderName);
                }
                catch
                {
                    InternalTraderNameId = nameIdLookup.GetOrAddId(internalTraderName);
                }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty
            && OrderSequenceId == 0
            && ParentOrderId == 0
            && ClosingOrderId == 0
            && ClosingOrderPrice == 0
            && DecisionCreatedTime == DateTime.MinValue
            && DecisionAmendTime == DateTime.MinValue
            && DivisionId == 0
            && divisionNameId == 0
            && DeskId == 0
            && deskNameId == 0
            && StrategyId == 0
            && strategyNameId == 0
            && StrategyDecisionId == 0
            && strategyDecisionNameId == 0
            && PortfolioId == 0
            && portfolioNameId == 0
            && InternalTraderId == 0
            && internalTraderNameId == 0
            && MarginConsumed == 0;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrderSequenceId        = 0;
            ParentOrderId          = 0;
            ClosingOrderId         = 0;
            ClosingOrderPrice      = 0;
            DecisionCreatedTime    = DateTime.MinValue;
            DecisionAmendTime      = DateTime.MinValue;
            DivisionId             = 0;
            divisionNameId         = 0;
            DeskId                 = 0;
            deskNameId             = 0;
            StrategyId             = 0;
            strategyNameId         = 0;
            StrategyDecisionId     = 0;
            strategyDecisionNameId = 0;
            PortfolioId            = 0;
            portfolioNameId        = 0;
            InternalTraderId       = 0;
            portfolioNameId        = 0;
            MarginConsumed         = 0;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set
        {
            base.HasUpdates = value;
            if (value) return;
            NameIdLookup.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        NameIdLookup.UpdateComplete();
        base.UpdateComplete();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsOrderSequenceIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalSequenceId, OrderSequenceId);
        if (!updatedOnly || IsParentOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalParentOrderId, ParentOrderId);
        if (!updatedOnly || IsClosingOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalClosingOrderId, ClosingOrderId);
        if (!updatedOnly || IsClosingOrderPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalClosingOrderOpenPrice, ClosingOrderPrice
                                         , quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1 );
        if (!updatedOnly || IsDecisionCreatedDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionCreateDate
                                         , decisionCreatedTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsDecisionCreatedSub2MinTimeUpdated)
        {
            var extended = decisionCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionCreateSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsDecisionAmendDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionAmendDate
                                         , decisionAmendTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsDecisionAmendSub2MinTimeUpdated)
        {
            var extended = decisionCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionAmendSub2MinTime, value, extended);
        }

        if (!updatedOnly || IsDivisionIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDivisionId, DivisionId);
        if (!updatedOnly || IsDivisionNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDivisionNameId, DivisionNameId);
        if (!updatedOnly || IsDeskIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDeskId, DeskId);
        if (!updatedOnly || IsDeskNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDeskNameId, DeskNameId);
        if (!updatedOnly || IsStrategyIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalStrategyId, StrategyId);
        if (!updatedOnly || IsStrategyNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalStrategyNameId, StrategyNameId);
        if (!updatedOnly || IsStrategyDecisionIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalStrategyDecisionId, StrategyDecisionId);
        if (!updatedOnly || IsStrategyDecisionNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalStrategyDecisionNameId, StrategyDecisionNameId);
        if (!updatedOnly || IsPortfolioIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalPortfolioId, PortfolioId);
        if (!updatedOnly || IsPortfolioNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalPortfolioNameId, PortfolioNameId);
        if (!updatedOnly || IsInternalTraderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalTraderId, InternalTraderId);
        if (!updatedOnly || IsInternalTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalTraderNameId, InternalTraderNameId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.OrdersSubId)
        {
            case PQOrdersSubFieldKeys.OrderInternalSequenceId:
                IsOrderSequenceIdUpdated = true; // incase of reset and sending 0;
                OrderSequenceId                 = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalParentOrderId:
                IsParentOrderIdUpdated = true; // incase of reset and sending 0;
                ParentOrderId        = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalClosingOrderId:
                IsClosingOrderIdUpdated = true; // incase of reset and sending 0;
                ClosingOrderId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalClosingOrderOpenPrice:
                IsClosingOrderPriceUpdated = true; // incase of reset and sending 0;
                ClosingOrderPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionCreateDate:
                IsDecisionCreatedDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref decisionCreatedTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionCreateSub2MinTime:
                IsDecisionCreatedSub2MinTimeUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionCreatedTime, pqFieldUpdate.Payload);
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionAmendDate:
                IsDecisionAmendDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref decisionAmendTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionAmendSub2MinTime:
                IsDecisionAmendSub2MinTimeUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionAmendTime, pqFieldUpdate.Payload);
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDivisionId:
                IsDivisionIdUpdated = true; // incase of reset and sending 0;
                DivisionId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDivisionNameId:
                IsDivisionNameUpdated = true; // incase of reset and sending 0;
                DivisionNameId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDeskId:
                IsDeskIdUpdated = true; // incase of reset and sending 0;
                DeskId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDeskNameId:
                IsDeskNameUpdated = true; // incase of reset and sending 0;
                DeskNameId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyId:
                IsStrategyIdUpdated = true; // incase of reset and sending 0;
                StrategyId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyNameId:
                IsStrategyNameUpdated = true; // incase of reset and sending 0;
                StrategyNameId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyDecisionId:
                IsStrategyDecisionIdUpdated = true; // incase of reset and sending 0;
                StrategyDecisionId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyDecisionNameId:
                IsStrategyDecisionNameUpdated = true; // incase of reset and sending 0;
                StrategyDecisionNameId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalPortfolioId:
                IsPortfolioIdUpdated = true; // incase of reset and sending 0;
                StrategyDecisionId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalPortfolioNameId:
                IsPortfolioNameUpdated = true; // incase of reset and sending 0;
                PortfolioNameId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalTraderId:
                IsInternalTraderIdUpdated = true; // incase of reset and sending 0;
                InternalTraderId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalTraderNameId:
                IsInternalTraderNameUpdated = true; // incase of reset and sending 0;
                InternalTraderNameId          = (int)pqFieldUpdate.Payload;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override void StateReset()
    {
        OrderSequenceId        = 0;
        ParentOrderId          = 0;
        ClosingOrderId         = 0;
        ClosingOrderPrice      = 0;
        DecisionCreatedTime    = DateTime.MinValue;
        DecisionAmendTime      = DateTime.MinValue;
        DivisionId             = 0;
        divisionNameId         = 0;
        DeskId                 = 0;
        deskNameId             = 0;
        StrategyId             = 0;
        strategyNameId         = 0;
        StrategyDecisionId     = 0;
        strategyDecisionNameId = 0;
        PortfolioId            = 0;
        portfolioNameId        = 0;
        InternalTraderId       = 0;
        portfolioNameId        = 0;
        MarginConsumed         = 0;

        IPOUpdatedFlags = InternalPassiveOrderLayerInfoUpdatedFlags.None;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQInternalPassiveOrderLayerInfo IPQInternalPassiveOrderLayerInfo.Clone() => Clone();


    IMutableInternalPassiveOrderLayerInfo IMutableInternalPassiveOrderLayerInfo.Clone() => Clone();

    IInternalPassiveOrderLayerInfo ICloneable<IInternalPassiveOrderLayerInfo>.Clone() => Clone();

    IInternalPassiveOrderLayerInfo IInternalPassiveOrderLayerInfo.Clone() => Clone();

    IMutableInternalPassiveOrderLayerInfo ICloneable<IMutableInternalPassiveOrderLayerInfo>.Clone() => Clone();

    IInternalPassiveOrder ICloneable<IInternalPassiveOrder>.                      Clone() => Clone();

    IInternalPassiveOrder IInternalPassiveOrder.                                  Clone() => Clone();

    IMutableInternalPassiveOrder ICloneable<IMutableInternalPassiveOrder>.        Clone() => Clone();

    IMutableInternalPassiveOrder IMutableInternalPassiveOrder.                    Clone() => Clone();

    IPQInternalPassiveOrderLayerInfo ICloneable<IPQInternalPassiveOrderLayerInfo>.Clone() => Clone();

    public override PQInternalPassiveOrderLayerInfo Clone() =>
        Recycler?.Borrow<PQInternalPassiveOrderLayerInfo>().CopyFrom(this) ??
        new PQInternalPassiveOrderLayerInfo(this, NameIdLookup);

    bool IInterfacesComparable<IInternalPassiveOrderLayerInfo>.AreEquivalent(IInternalPassiveOrderLayerInfo? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    public bool AreEquivalent(IMutableInternalPassiveOrderLayerInfo? other, bool exactTypes = false) => 
        AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (!(other is IInternalPassiveOrder internalPassiveOrder)) return false;

        var baseSame                 = base.AreEquivalent(other, exactTypes);
        var orderSequenceIdSame      = OrderSequenceId == internalPassiveOrder.OrderSequenceId;
        var parentOrderIdSame        = ParentOrderId == internalPassiveOrder.ParentOrderId;
        var closingOrderIdSame       = ClosingOrderId == internalPassiveOrder.ClosingOrderId;
        var closingOrderPriceSame    = ClosingOrderPrice == internalPassiveOrder.ClosingOrderPrice;
        var decisionCreatedTimeSame  = DecisionCreatedTime == internalPassiveOrder.DecisionCreatedTime;
        var decisionAmendTimeSame    = DecisionAmendTime == internalPassiveOrder.DecisionAmendTime;
        var divisionIdSame           = DivisionId == internalPassiveOrder.DivisionId;
        var divisionNameSame         = DivisionName == internalPassiveOrder.DivisionName;
        var deskIdSame               = DeskId == internalPassiveOrder.DeskId;
        var deskNameSame             = DeskName == internalPassiveOrder.DeskName;
        var strategyIdSame           = StrategyId == internalPassiveOrder.StrategyId;
        var strategyNameSame         = StrategyName == internalPassiveOrder.StrategyName;
        var strategyDecisionIdSame   = StrategyDecisionId == internalPassiveOrder.StrategyDecisionId;
        var strategyDecisionNameSame = StrategyDecisionName == internalPassiveOrder.StrategyDecisionName;
        var portfolioIdSame          = PortfolioId == internalPassiveOrder.PortfolioId;
        var portfolioNameSame        = PortfolioName == internalPassiveOrder.PortfolioName;
        var internalTraderIdSame     = InternalTraderId == internalPassiveOrder.InternalTraderId;
        var internalTraderNameSame   = InternalTraderName == internalPassiveOrder.InternalTraderName;
        var marginConsumedSame       = MarginConsumed == internalPassiveOrder.MarginConsumed;

        var updatedSame = true;
        if (exactTypes)
            updatedSame = internalPassiveOrder is PQInternalPassiveOrderLayerInfo pqCounterPartyOther && UpdatedFlags == pqCounterPartyOther.UpdatedFlags;

        var allAreSame =  orderSequenceIdSame && parentOrderIdSame && closingOrderIdSame && closingOrderPriceSame && decisionCreatedTimeSame
                       && decisionAmendTimeSame && divisionIdSame && divisionNameSame && deskIdSame && deskNameSame && strategyIdSame
                       && strategyNameSame && strategyDecisionNameSame && strategyDecisionIdSame && portfolioIdSame && portfolioNameSame &&
                          internalTraderIdSame &&
                          internalTraderNameSame && marginConsumedSame && baseSame && updatedSame;

        return allAreSame;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    IPQInternalPassiveOrderLayerInfo ITransferState<IPQInternalPassiveOrderLayerInfo>.CopyFrom
        (IPQInternalPassiveOrderLayerInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQInternalPassiveOrderLayerInfo CopyFrom
        (IAnonymousOrderLayerInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQInternalPassiveOrderLayerInfo pqIntrnlPsivOrdr)
        {
            NameIdLookup.CopyFrom(pqIntrnlPsivOrdr.NameIdLookup, copyMergeFlags);

            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqIntrnlPsivOrdr.IsOrderSequenceIdUpdated || isFullReplace)
            {
                IsOrderSequenceIdUpdated = true;

                OrderSequenceId = pqIntrnlPsivOrdr.OrderSequenceId;
            }
            if (pqIntrnlPsivOrdr.IsParentOrderIdUpdated || isFullReplace)
            {
                IsParentOrderIdUpdated = true;

                ParentOrderId = pqIntrnlPsivOrdr.ParentOrderId;
            }
            if (pqIntrnlPsivOrdr.IsClosingOrderIdUpdated || isFullReplace)
            {
                IsClosingOrderIdUpdated = true;

                ClosingOrderId = pqIntrnlPsivOrdr.ClosingOrderId;
            }
            if (pqIntrnlPsivOrdr.IsClosingOrderPriceUpdated || isFullReplace)
            {
                IsClosingOrderPriceUpdated = true;

                ClosingOrderPrice = pqIntrnlPsivOrdr.ClosingOrderPrice;
            }
            if (pqIntrnlPsivOrdr.IsDecisionCreatedDateUpdated || isFullReplace)
            {
                var originalDecisionCreateTime = decisionCreatedTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionCreatedTime,
                                                                      pqIntrnlPsivOrdr.DecisionCreatedTime.Get2MinIntervalsFromUnixEpoch());
                IsDecisionCreatedDateUpdated |= originalDecisionCreateTime != decisionCreatedTime;
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
            }
            if (pqIntrnlPsivOrdr.IsDecisionCreatedSub2MinTimeUpdated || isFullReplace)
            {
                var originalDecisionCreateTime = decisionCreatedTime;
                PQFieldConverters.UpdateSub2MinComponent(ref decisionCreatedTime,
                                                         pqIntrnlPsivOrdr.DecisionCreatedTime.GetSub2MinComponent());
                IsDecisionCreatedSub2MinTimeUpdated |= originalDecisionCreateTime != decisionCreatedTime;
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
            }
            if (pqIntrnlPsivOrdr.IsDecisionAmendDateUpdated || isFullReplace)
            {
                var originalDecisionAmendTime = decisionAmendTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionAmendTime,
                                                                      pqIntrnlPsivOrdr.DecisionAmendTime.Get2MinIntervalsFromUnixEpoch());
                IsDecisionAmendDateUpdated |= originalDecisionAmendTime != decisionAmendTime;
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
            }
            if (pqIntrnlPsivOrdr.IsDecisionCreatedSub2MinTimeUpdated || isFullReplace)
            {
                var originalDecisionAmendTime = decisionAmendTime;
                PQFieldConverters.UpdateSub2MinComponent(ref decisionAmendTime,
                                                         pqIntrnlPsivOrdr.DecisionAmendTime.GetSub2MinComponent());
                IsDecisionAmendSub2MinTimeUpdated |= originalDecisionAmendTime != decisionAmendTime;
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
            }
            if (pqIntrnlPsivOrdr.IsDivisionIdUpdated || isFullReplace)
            {
                IsDivisionIdUpdated = true;

                DivisionId = pqIntrnlPsivOrdr.DivisionId;
            }
            if (pqIntrnlPsivOrdr.IsDivisionNameUpdated || isFullReplace)
            {
                IsDivisionNameUpdated = true;

                DivisionNameId = pqIntrnlPsivOrdr.DivisionNameId;
            }
            if (pqIntrnlPsivOrdr.IsDeskIdUpdated || isFullReplace)
            {
                IsDeskIdUpdated = true;

                DeskId = pqIntrnlPsivOrdr.DeskId;
            }
            if (pqIntrnlPsivOrdr.IsDeskNameUpdated || isFullReplace)
            {
                IsDeskNameUpdated = true;

                DeskNameId = pqIntrnlPsivOrdr.DeskNameId;
            }
            if (pqIntrnlPsivOrdr.IsStrategyIdUpdated || isFullReplace)
            {
                IsStrategyIdUpdated = true;

                StrategyId = pqIntrnlPsivOrdr.StrategyId;
            }
            if (pqIntrnlPsivOrdr.IsStrategyNameUpdated || isFullReplace)
            {
                IsStrategyNameUpdated = true;

                StrategyNameId = pqIntrnlPsivOrdr.StrategyNameId;
            }
            if (pqIntrnlPsivOrdr.IsStrategyDecisionIdUpdated || isFullReplace)
            {
                IsStrategyDecisionIdUpdated = true;

                StrategyDecisionId = pqIntrnlPsivOrdr.StrategyDecisionId;
            }
            if (pqIntrnlPsivOrdr.IsStrategyDecisionNameUpdated || isFullReplace)
            {
                IsStrategyDecisionNameUpdated = true;

                StrategyDecisionNameId = pqIntrnlPsivOrdr.StrategyDecisionNameId;
            }
            if (pqIntrnlPsivOrdr.IsPortfolioIdUpdated || isFullReplace)
            {
                IsPortfolioIdUpdated = true;

                PortfolioId = pqIntrnlPsivOrdr.PortfolioId;
            }
            if (pqIntrnlPsivOrdr.IsPortfolioNameUpdated || isFullReplace)
            {
                IsPortfolioNameUpdated = true;

                PortfolioNameId = pqIntrnlPsivOrdr.PortfolioNameId;
            }
            if (pqIntrnlPsivOrdr.IsInternalTraderIdUpdated || isFullReplace)
            {
                IsInternalTraderIdUpdated = true;

                InternalTraderId = pqIntrnlPsivOrdr.InternalTraderId;
            }
            if (pqIntrnlPsivOrdr.IsInternalTraderNameUpdated || isFullReplace)
            {
                IsInternalTraderNameUpdated = true;

                InternalTraderNameId = pqIntrnlPsivOrdr.InternalTraderNameId;
            }
            if (pqIntrnlPsivOrdr.IsMarginConsumedUpdated || isFullReplace)
            {
                IsMarginConsumedUpdated = true;

                MarginConsumed = pqIntrnlPsivOrdr.MarginConsumed;
            }

            if (isFullReplace && pqIntrnlPsivOrdr is PQInternalPassiveOrderLayerInfo pqInternalPassiveOrder)
                UpdatedFlags = pqInternalPassiveOrder.UpdatedFlags;
        }
        else if (source is IInternalPassiveOrder internalPassiveOrder)
        {
            OrderSequenceId      = internalPassiveOrder.OrderSequenceId;
            ParentOrderId        = internalPassiveOrder.ParentOrderId;
            ClosingOrderId       = internalPassiveOrder.ClosingOrderId;
            ClosingOrderPrice    = internalPassiveOrder.ClosingOrderPrice;
            DecisionCreatedTime  = internalPassiveOrder.DecisionCreatedTime;
            DecisionAmendTime    = internalPassiveOrder.DecisionAmendTime;
            DivisionId           = internalPassiveOrder.DivisionId;
            DivisionName         = internalPassiveOrder.DivisionName;
            DeskId               = internalPassiveOrder.DeskId;
            DeskName             = internalPassiveOrder.DeskName;
            StrategyId           = internalPassiveOrder.StrategyId;
            StrategyName         = internalPassiveOrder.StrategyName;
            StrategyDecisionId   = internalPassiveOrder.StrategyDecisionId;
            StrategyDecisionName = internalPassiveOrder.StrategyDecisionName;
            PortfolioId          = internalPassiveOrder.PortfolioId;
            PortfolioName        = internalPassiveOrder.PortfolioName;
            InternalTraderId     = internalPassiveOrder.InternalTraderId;
            InternalTraderName   = internalPassiveOrder.InternalTraderName;
            MarginConsumed       = internalPassiveOrder.MarginConsumed;
        }

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IExternalCounterPartyOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((int)OrderSequenceId * 397) ^ hashCode;
            hashCode = ((int)ParentOrderId * 397) ^ hashCode;
            hashCode = ((int)ClosingOrderId * 397) ^ hashCode;
            hashCode = (ClosingOrderPrice.GetHashCode() * 397) ^ hashCode;
            hashCode = (DecisionCreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (DecisionAmendTime.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)DivisionId * 397) ^ hashCode;
            hashCode = ((DivisionName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)DeskId * 397) ^ hashCode;
            hashCode = ((DeskName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)StrategyId * 397) ^ hashCode;
            hashCode = ((StrategyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)StrategyDecisionId * 397) ^ hashCode;
            hashCode = ((StrategyDecisionName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)PortfolioId * 397) ^ hashCode;
            hashCode = ((PortfolioName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)InternalTraderId * 397) ^ hashCode;
            hashCode = ((InternalTraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = (MarginConsumed.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQInternalPassiveOrderLayerInfoToStringMembers =>
        $"{PQAnonymousOrderLayerInfoToStringMembers}, {nameof(OrderSequenceId)}: {OrderSequenceId}, {nameof(ParentOrderId)}: {ParentOrderId}, " +
        $"{nameof(ClosingOrderId)}: {ClosingOrderId}, {nameof(ClosingOrderPrice)}: {ClosingOrderPrice}, {nameof(DecisionCreatedTime)}: {DecisionCreatedTime}, " +
        $"{nameof(DecisionAmendTime)}: {DecisionAmendTime}, {nameof(DivisionId)}: {DivisionId}, {nameof(DivisionName)}: {DivisionName}, " +
        $"{nameof(DeskId)}: {DeskId}, {nameof(DeskName)}: {DeskName}, {nameof(StrategyId)}: {StrategyId}, {nameof(StrategyName)}: {StrategyName}, " +
        $"{nameof(StrategyDecisionId)}: {StrategyDecisionId}, {nameof(StrategyDecisionName)}: {StrategyDecisionName}, {nameof(PortfolioId)}: {PortfolioId}, " +
        $"{nameof(PortfolioName)}: {PortfolioName}, {nameof(InternalTraderId)}: {InternalTraderId}, {nameof(InternalTraderName)}: {InternalTraderName}, " +
        $"{nameof(MarginConsumed)}: {MarginConsumed}";

    public override string ToString() =>
        $"{nameof(PQCounterPartyOrderLayerInfo)}({PQInternalPassiveOrderLayerInfoToStringMembers}, {UpdatedFlagsToString})";
}
