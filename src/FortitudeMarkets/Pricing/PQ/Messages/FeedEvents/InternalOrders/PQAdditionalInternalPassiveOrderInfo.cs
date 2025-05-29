using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

public interface IPQAdditionalInternalPassiveOrderInfo : IMutableAdditionalInternalPassiveOrderInfo, ISupportsPQNameIdLookupGenerator
  , IPQSupportsStringUpdates<IPQAdditionalInternalPassiveOrderInfo>, IPQSupportsNumberPrecisionFieldUpdates<IPQAdditionalInternalPassiveOrderInfo>
  , ICloneable<IPQAdditionalInternalPassiveOrderInfo>
{
    bool IsOrderSequenceIdUpdated      { get; set; }
    bool IsParentOrderIdUpdated        { get; set; }
    bool IsClosingOrderIdUpdated       { get; set; }
    bool IsClosingOrderPriceUpdated    { get; set; }
    bool IsDivisionIdUpdated           { get; set; }
    int  DivisionNameId                { get; set; }
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


    new IPQAdditionalInternalPassiveOrderInfo Clone();
}

[Flags]
public enum PQAdditionalInternalPassiveOrderInfoUpdatedFlags : uint
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

public class PQAdditionalInternalPassiveOrderInfo : ReusableObject<IAdditionalInternalPassiveOrderInfo>, IPQAdditionalInternalPassiveOrderInfo
{
    protected int NumUpdatesSinceEmpty = -1;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    protected PQAdditionalInternalPassiveOrderInfoUpdatedFlags UpdatedFlags;

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

    public PQAdditionalInternalPassiveOrderInfo()
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAdditionalInternalPassiveOrderInfo(IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAdditionalInternalPassiveOrderInfo
        (IPQNameIdLookupGenerator lookupDict, int orderId = 0, DateTime createdTime = default, decimal orderVolume = 0m
          , OrderGenesisFlags genesisFlags = OrderGenesisFlags.None, OrderType orderType = OrderType.None
          , OrderLifeCycleState orderLifeCycleState = OrderLifeCycleState.None, DateTime? updatedTime = null, decimal? remainingVolume = null
          , uint trackingId = 0)
        //: base(orderId, createdTime, orderVolume, orderType, genesisFlags, orderLifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        NameIdLookup = lookupDict;
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQAdditionalInternalPassiveOrderInfo(IAdditionalInternalPassiveOrderInfo? toClone, IPQNameIdLookupGenerator? pqNameIdLookupGenerator = null)
        //: base(toClone)
    {
        NameIdLookup = pqNameIdLookupGenerator ?? new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
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

            SetFlagsSame(toClone);
        }

        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) NumUpdatesSinceEmpty = 0;
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

    public uint DeskId
    {
        get => deskId;
        set
        {
            IsDeskIdUpdated |= value != deskId || NumUpdatesSinceEmpty == 0;
            deskId          =  value;
        }
    }

    public int DeskNameId
    {
        get => deskNameId;
        set
        {
            IsDeskNameUpdated |= deskNameId != value || NumUpdatesSinceEmpty == 0;
            deskNameId        =  value;
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

    public uint StrategyId
    {
        get => strategyId;
        set
        {
            IsStrategyIdUpdated |= value != strategyId || NumUpdatesSinceEmpty == 0;
            strategyId          =  value;
        }
    }

    public int StrategyNameId
    {
        get => strategyNameId;
        set
        {
            IsStrategyNameUpdated |= strategyNameId != value || NumUpdatesSinceEmpty == 0;
            strategyNameId        =  value;
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

    public uint StrategyDecisionId
    {
        get => strategyDecisionId;
        set
        {
            IsStrategyDecisionIdUpdated |= strategyDecisionId != value || NumUpdatesSinceEmpty == 0;
            strategyDecisionId          =  value;
        }
    }

    public int StrategyDecisionNameId
    {
        get => strategyDecisionNameId;
        set
        {
            IsStrategyDecisionNameUpdated |= strategyDecisionNameId != value || NumUpdatesSinceEmpty == 0;
            strategyDecisionNameId        =  value;
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

    public uint PortfolioId
    {
        get => portfolioId;
        set
        {
            IsPortfolioIdUpdated |= portfolioId != value || NumUpdatesSinceEmpty == 0;
            portfolioId          =  value;
        }
    }

    public int PortfolioNameId
    {
        get => portfolioNameId;
        set
        {
            IsPortfolioNameUpdated |= portfolioNameId != value || NumUpdatesSinceEmpty == 0;
            portfolioNameId        =  value;
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
    public uint InternalTraderId
    {
        get => internalTraderId;
        set
        {
            IsInternalTraderIdUpdated |= internalTraderId != value || NumUpdatesSinceEmpty == 0;
            internalTraderId          =  value;
        }
    }

    public int InternalTraderNameId
    {
        get => internalTraderNameId;
        set
        {
            IsInternalTraderNameUpdated |= internalTraderNameId != value || NumUpdatesSinceEmpty == 0;
            internalTraderNameId        =  value;
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
            marginConsumed          =  value;
        }
    }


    public bool IsOrderSequenceIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.OrderSequenceIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.OrderSequenceIdFlag;

            else if (IsOrderSequenceIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.OrderSequenceIdFlag;
        }
    }

    public bool IsParentOrderIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ParentOrderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ParentOrderIdFlag;

            else if (IsParentOrderIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ParentOrderIdFlag;
        }
    }
    public bool IsClosingOrderIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderIdFlag;

            else if (IsClosingOrderIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderIdFlag;
        }
    }
    public bool IsClosingOrderPriceUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderPriceFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderPriceFlag;

            else if (IsClosingOrderPriceUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.ClosingOrderPriceFlag;
        }
    }
    public bool IsDivisionIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionIdFlag;

            else if (IsDivisionIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionIdFlag;
        }
    }
    public bool IsDivisionNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionNameFlag;

            else if (IsDivisionNameUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DivisionNameFlag;
        }
    }
    public bool IsDeskIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskIdFlag;

            else if (IsDeskIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskIdFlag;
        }
    }
    public bool IsDeskNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskNameFlag;

            else if (IsDeskNameUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DeskNameFlag;
        }
    }

    public bool IsStrategyIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyIdFlag;

            else if (IsStrategyIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyIdFlag;
        }
    }

    public bool IsStrategyNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyNameFlag;

            else if (IsStrategyNameUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyNameFlag;
        }
    }
    public bool IsStrategyDecisionIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionIdFlag;

            else if (IsStrategyDecisionIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionIdFlag;
        }
    }

    public bool IsStrategyDecisionNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionNameFlag;

            else if (IsStrategyDecisionIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.StrategyDecisionNameFlag;
        }
    }

    public bool IsPortfolioIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioIdFlag;

            else if (IsPortfolioIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioIdFlag;
        }
    }
    public bool IsPortfolioNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioNameFlag;

            else if (IsPortfolioNameUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.PortfolioNameFlag;
        }
    }
    public bool IsInternalTraderIdUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderIdFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderIdFlag;

            else if (IsInternalTraderIdUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderIdFlag;
        }
    }

    public bool IsInternalTraderNameUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderNameFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderNameFlag;

            else if (IsInternalTraderNameUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.InternalTraderNameFlag;
        }
    }
    public bool IsMarginConsumedUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.MarginConsumedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.MarginConsumedFlag;

            else if (IsMarginConsumedUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.MarginConsumedFlag;
        }
    }
    public bool IsDecisionCreatedDateUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedDateFlag;

            else if (IsDecisionCreatedDateUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedDateFlag;
        }
    }
    public bool IsDecisionCreatedSub2MinTimeUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag;

            else if (IsDecisionCreatedSub2MinTimeUpdated)
                UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionCreatedSub2MinTimeFlag;
        }
    }
    public bool IsDecisionAmendDateUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendDateFlag;

            else if (IsDecisionAmendDateUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendDateFlag;
        }
    }
    public bool IsDecisionAmendSub2MinTimeUpdated
    {
        get => (UpdatedFlags & PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag;

            else if (IsDecisionAmendSub2MinTimeUpdated) UpdatedFlags ^= PQAdditionalInternalPassiveOrderInfoUpdatedFlags.DecisionAmendSub2MinTimeFlag;
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
            string? cacheDivisionName                            = null;
            if (divisionNameId > 0) cacheDivisionName            = DivisionName;
            string? cacheDeskName                                = null;
            if (deskNameId > 0) cacheDeskName                    = DeskName;
            string? strategyName                                 = null;
            if (strategyNameId > 0) strategyName                 = StrategyName;
            string? strategyDecisionName                         = null;
            if (strategyDecisionNameId > 0) strategyDecisionName = StrategyDecisionName;
            string? portfolioName                                = null;
            if (portfolioNameId > 0) portfolioName               = PortfolioName;
            string? internalTraderName                           = null;
            if (internalTraderNameId > 0) internalTraderName     = InternalTraderName;
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
    public virtual bool IsEmpty
    {
        get =>
            OrderSequenceId == 0
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
    public uint UpdateCount => (uint)NumUpdatesSinceEmpty;

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != PQAdditionalInternalPassiveOrderInfoUpdatedFlags.None;
        set
        {
            if (value) return;
            NameIdLookup.HasUpdates = value;
            UpdatedFlags            = PQAdditionalInternalPassiveOrderInfoUpdatedFlags.None;
        }
    }

    public virtual void UpdateComplete(uint updateId = 0)
    {
        NameIdLookup.UpdateComplete(updateId);
        HasUpdates = false;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsOrderSequenceIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalSequenceId, OrderSequenceId);
        if (!updatedOnly || IsParentOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalParentOrderId, ParentOrderId);
        if (!updatedOnly || IsClosingOrderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalClosingOrderId, ClosingOrderId);
        if (!updatedOnly || IsClosingOrderPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalClosingOrderOpenPrice, ClosingOrderPrice
                                         , quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1);
        if (!updatedOnly || IsDecisionCreatedDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionCreateDate
                                         , decisionCreatedTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsDecisionCreatedSub2MinTimeUpdated)
        {
            var extended = decisionCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionCreateSub2MinTime, value
                                         , extended);
        }
        if (!updatedOnly || IsDecisionAmendDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionAmendDate
                                         , decisionAmendTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsDecisionAmendSub2MinTimeUpdated)
        {
            var extended = decisionCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalDecisionAmendSub2MinTime, value
                                         , extended);
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
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalStrategyDecisionNameId
                                         , StrategyDecisionNameId);
        if (!updatedOnly || IsPortfolioIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalPortfolioId, PortfolioId);
        if (!updatedOnly || IsPortfolioNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalPortfolioNameId, PortfolioNameId);
        if (!updatedOnly || IsInternalTraderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalTraderId, InternalTraderId);
        if (!updatedOnly || IsInternalTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderInternalTraderNameId, InternalTraderNameId);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.OrdersSubId)
        {
            case PQOrdersSubFieldKeys.OrderInternalSequenceId:
                IsOrderSequenceIdUpdated = true; // in-case of reset and sending 0;
                OrderSequenceId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalParentOrderId:
                IsParentOrderIdUpdated = true; // in-case of reset and sending 0;
                ParentOrderId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalClosingOrderId:
                IsClosingOrderIdUpdated = true; // in=case of reset and sending 0;
                ClosingOrderId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalClosingOrderOpenPrice:
                IsClosingOrderPriceUpdated = true; // in-case of reset and sending 0;
                ClosingOrderPrice          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionCreateDate:
                IsDecisionCreatedDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref decisionCreatedTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionCreateSub2MinTime:
                IsDecisionCreatedSub2MinTimeUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionCreatedTime, pqFieldUpdate.Payload);
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionAmendDate:
                IsDecisionAmendDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref decisionAmendTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDecisionAmendSub2MinTime:
                IsDecisionAmendSub2MinTimeUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionAmendTime, pqFieldUpdate.Payload);
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDivisionId:
                IsDivisionIdUpdated = true; // in-case of reset and sending 0;
                DivisionId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDivisionNameId:
                IsDivisionNameUpdated = true; // in-case of reset and sending 0;
                DivisionNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDeskId:
                IsDeskIdUpdated = true; // in-case of reset and sending 0;
                DeskId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalDeskNameId:
                IsDeskNameUpdated = true; // in-case of reset and sending 0;
                DeskNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyId:
                IsStrategyIdUpdated = true; // in-case of reset and sending 0;
                StrategyId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyNameId:
                IsStrategyNameUpdated = true; // in-case of reset and sending 0;
                StrategyNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyDecisionId:
                IsStrategyDecisionIdUpdated = true; // in-case of reset and sending 0;
                StrategyDecisionId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalStrategyDecisionNameId:
                IsStrategyDecisionNameUpdated = true; // in-case of reset and sending 0;
                StrategyDecisionNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalPortfolioId:
                IsPortfolioIdUpdated = true; // in-case of reset and sending 0;
                StrategyDecisionId   = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalPortfolioNameId:
                IsPortfolioNameUpdated = true; // in-case of reset and sending 0;
                PortfolioNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalTraderId:
                IsInternalTraderIdUpdated = true; // in-case of reset and sending 0;
                InternalTraderId          = pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderInternalTraderNameId:
                IsInternalTraderNameUpdated = true; // in-case of reset and sending 0;
                InternalTraderNameId        = (int)pqFieldUpdate.Payload;
                return 0;
        }

        return -1;
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

        UpdatedFlags = PQAdditionalInternalPassiveOrderInfoUpdatedFlags.None;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQAdditionalInternalPassiveOrderInfo IPQAdditionalInternalPassiveOrderInfo.Clone() => Clone();

    IAdditionalInternalPassiveOrderInfo ICloneable<IAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IPQAdditionalInternalPassiveOrderInfo ICloneable<IPQAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo ICloneable<IMutableAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo IMutableAdditionalInternalPassiveOrderInfo.Clone() => Clone();

    public override PQAdditionalInternalPassiveOrderInfo Clone() =>
        Recycler?.Borrow<PQAdditionalInternalPassiveOrderInfo>().CopyFrom(this) ??
        new PQAdditionalInternalPassiveOrderInfo(this, NameIdLookup);


    public bool AreEquivalent(IMutableInternalPassiveOrder? other, bool exactTypes = false) =>
        AreEquivalent((IAdditionalInternalPassiveOrderInfo?)other, exactTypes);

    public virtual bool AreEquivalent(IAdditionalInternalPassiveOrderInfo? other, bool exactTypes = false)
    {
        if (other is null) return false;

        var orderSequenceIdSame      = OrderSequenceId == other.OrderSequenceId;
        var parentOrderIdSame        = ParentOrderId == other.ParentOrderId;
        var closingOrderIdSame       = ClosingOrderId == other.ClosingOrderId;
        var closingOrderPriceSame    = ClosingOrderPrice == other.ClosingOrderPrice;
        var decisionCreatedTimeSame  = DecisionCreatedTime == other.DecisionCreatedTime;
        var decisionAmendTimeSame    = DecisionAmendTime == other.DecisionAmendTime;
        var divisionIdSame           = DivisionId == other.DivisionId;
        var divisionNameSame         = DivisionName == other.DivisionName;
        var deskIdSame               = DeskId == other.DeskId;
        var deskNameSame             = DeskName == other.DeskName;
        var strategyIdSame           = StrategyId == other.StrategyId;
        var strategyNameSame         = StrategyName == other.StrategyName;
        var strategyDecisionIdSame   = StrategyDecisionId == other.StrategyDecisionId;
        var strategyDecisionNameSame = StrategyDecisionName == other.StrategyDecisionName;
        var portfolioIdSame          = PortfolioId == other.PortfolioId;
        var portfolioNameSame        = PortfolioName == other.PortfolioName;
        var internalTraderIdSame     = InternalTraderId == other.InternalTraderId;
        var internalTraderNameSame   = InternalTraderName == other.InternalTraderName;
        var marginConsumedSame       = MarginConsumed == other.MarginConsumed;

        var updatedSame = true;
        if (exactTypes)
            updatedSame = other is PQAdditionalInternalPassiveOrderInfo pqCounterPartyOther && UpdatedFlags == pqCounterPartyOther.UpdatedFlags;

        var allAreSame = orderSequenceIdSame && parentOrderIdSame && closingOrderIdSame && closingOrderPriceSame && decisionCreatedTimeSame
                      && decisionAmendTimeSame && divisionIdSame && divisionNameSame && deskIdSame && deskNameSame && strategyIdSame
                      && strategyNameSame && strategyDecisionNameSame && strategyDecisionIdSame && portfolioIdSame && portfolioNameSame &&
                         internalTraderIdSame &&
                         internalTraderNameSame && marginConsumedSame && updatedSame;

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

    IPQAdditionalInternalPassiveOrderInfo ITransferState<IPQAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IPQAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableAdditionalInternalPassiveOrderInfo ITransferState<IMutableAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IMutableAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQAdditionalInternalPassiveOrderInfo CopyFrom
        (IAdditionalInternalPassiveOrderInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {

        if (source is IPQAdditionalInternalPassiveOrderInfo pqAddPassive)
        {
            NameIdLookup.CopyFrom(pqAddPassive.NameIdLookup, copyMergeFlags);
            
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pqAddPassive.IsOrderSequenceIdUpdated || isFullReplace)
            {
                IsOrderSequenceIdUpdated = true;

                OrderSequenceId = pqAddPassive.OrderSequenceId;
            }
            if (pqAddPassive.IsParentOrderIdUpdated || isFullReplace)
            {
                IsParentOrderIdUpdated = true;

                ParentOrderId = pqAddPassive.ParentOrderId;
            }
            if (pqAddPassive.IsClosingOrderIdUpdated || isFullReplace)
            {
                IsClosingOrderIdUpdated = true;

                ClosingOrderId = pqAddPassive.ClosingOrderId;
            }
            if (pqAddPassive.IsClosingOrderPriceUpdated || isFullReplace)
            {
                IsClosingOrderPriceUpdated = true;

                ClosingOrderPrice = pqAddPassive.ClosingOrderPrice;
            }
            if (pqAddPassive.IsDecisionCreatedDateUpdated || isFullReplace)
            {
                var originalDecisionCreateTime = decisionCreatedTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionCreatedTime,
                                                                      pqAddPassive.DecisionCreatedTime.Get2MinIntervalsFromUnixEpoch());
                IsDecisionCreatedDateUpdated |= originalDecisionCreateTime != decisionCreatedTime;
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
            }
            if (pqAddPassive.IsDecisionCreatedSub2MinTimeUpdated || isFullReplace)
            {
                var originalDecisionCreateTime = decisionCreatedTime;
                PQFieldConverters.UpdateSub2MinComponent(ref decisionCreatedTime,
                                                         pqAddPassive.DecisionCreatedTime.GetSub2MinComponent());
                IsDecisionCreatedSub2MinTimeUpdated |= originalDecisionCreateTime != decisionCreatedTime;
                if (decisionCreatedTime == DateTime.UnixEpoch) decisionCreatedTime = default;
            }
            if (pqAddPassive.IsDecisionAmendDateUpdated || isFullReplace)
            {
                var originalDecisionAmendTime = decisionAmendTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref decisionAmendTime,
                                                                      pqAddPassive.DecisionAmendTime.Get2MinIntervalsFromUnixEpoch());
                IsDecisionAmendDateUpdated |= originalDecisionAmendTime != decisionAmendTime;
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
            }
            if (pqAddPassive.IsDecisionCreatedSub2MinTimeUpdated || isFullReplace)
            {
                var originalDecisionAmendTime = decisionAmendTime;
                PQFieldConverters.UpdateSub2MinComponent(ref decisionAmendTime,
                                                         pqAddPassive.DecisionAmendTime.GetSub2MinComponent());
                IsDecisionAmendSub2MinTimeUpdated |= originalDecisionAmendTime != decisionAmendTime;
                if (decisionAmendTime == DateTime.UnixEpoch) decisionAmendTime = default;
            }
            if (pqAddPassive.IsDivisionIdUpdated || isFullReplace)
            {
                IsDivisionIdUpdated = true;

                DivisionId = pqAddPassive.DivisionId;
            }
            if (pqAddPassive.IsDivisionNameUpdated || isFullReplace)
            {
                IsDivisionNameUpdated = true;

                DivisionNameId = pqAddPassive.DivisionNameId;
            }
            if (pqAddPassive.IsDeskIdUpdated || isFullReplace)
            {
                IsDeskIdUpdated = true;

                DeskId = pqAddPassive.DeskId;
            }
            if (pqAddPassive.IsDeskNameUpdated || isFullReplace)
            {
                IsDeskNameUpdated = true;

                DeskNameId = pqAddPassive.DeskNameId;
            }
            if (pqAddPassive.IsStrategyIdUpdated || isFullReplace)
            {
                IsStrategyIdUpdated = true;

                StrategyId = pqAddPassive.StrategyId;
            }
            if (pqAddPassive.IsStrategyNameUpdated || isFullReplace)
            {
                IsStrategyNameUpdated = true;

                StrategyNameId = pqAddPassive.StrategyNameId;
            }
            if (pqAddPassive.IsStrategyDecisionIdUpdated || isFullReplace)
            {
                IsStrategyDecisionIdUpdated = true;

                StrategyDecisionId = pqAddPassive.StrategyDecisionId;
            }
            if (pqAddPassive.IsStrategyDecisionNameUpdated || isFullReplace)
            {
                IsStrategyDecisionNameUpdated = true;

                StrategyDecisionNameId = pqAddPassive.StrategyDecisionNameId;
            }
            if (pqAddPassive.IsPortfolioIdUpdated || isFullReplace)
            {
                IsPortfolioIdUpdated = true;

                PortfolioId = pqAddPassive.PortfolioId;
            }
            if (pqAddPassive.IsPortfolioNameUpdated || isFullReplace)
            {
                IsPortfolioNameUpdated = true;

                PortfolioNameId = pqAddPassive.PortfolioNameId;
            }
            if (pqAddPassive.IsInternalTraderIdUpdated || isFullReplace)
            {
                IsInternalTraderIdUpdated = true;

                InternalTraderId = pqAddPassive.InternalTraderId;
            }
            if (pqAddPassive.IsInternalTraderNameUpdated || isFullReplace)
            {
                IsInternalTraderNameUpdated = true;

                InternalTraderNameId = pqAddPassive.InternalTraderNameId;
            }
            if (pqAddPassive.IsMarginConsumedUpdated || isFullReplace)
            {
                IsMarginConsumedUpdated = true;

                MarginConsumed = pqAddPassive.MarginConsumed;
            }

            if (isFullReplace) SetFlagsSame(pqAddPassive);
        }
        else if (source is IInternalPassiveOrder internalPassiveOrder)
        {
            var hasAsNew               = copyMergeFlags.HasAsNew();
            if (hasAsNew)
            {
                UpdatedFlags         = PQAdditionalInternalPassiveOrderInfoUpdatedFlags.None;
                NumUpdatesSinceEmpty = int.MaxValue;
            }

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
            
            if (hasAsNew)
            {
                NumUpdatesSinceEmpty = 0;
            }
        }

        return this;
    }

    protected void SetFlagsSame(IAdditionalInternalPassiveOrderInfo toCopyFlags)
    {
        if (toCopyFlags is PQAdditionalInternalPassiveOrderInfo pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAdditionalInternalPassiveOrderInfo, true);

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
        $"{nameof(OrderSequenceId)}: {OrderSequenceId}, {nameof(ParentOrderId)}: {ParentOrderId}, " +
        $"{nameof(ClosingOrderId)}: {ClosingOrderId}, {nameof(ClosingOrderPrice)}: {ClosingOrderPrice}, {nameof(DecisionCreatedTime)}: {DecisionCreatedTime}, " +
        $"{nameof(DecisionAmendTime)}: {DecisionAmendTime}, {nameof(DivisionId)}: {DivisionId}, {nameof(DivisionName)}: {DivisionName}, " +
        $"{nameof(DeskId)}: {DeskId}, {nameof(DeskName)}: {DeskName}, {nameof(StrategyId)}: {StrategyId}, {nameof(StrategyName)}: {StrategyName}, " +
        $"{nameof(StrategyDecisionId)}: {StrategyDecisionId}, {nameof(StrategyDecisionName)}: {StrategyDecisionName}, {nameof(PortfolioId)}: {PortfolioId}, " +
        $"{nameof(PortfolioName)}: {PortfolioName}, {nameof(InternalTraderId)}: {InternalTraderId}, {nameof(InternalTraderName)}: {InternalTraderName}, " +
        $"{nameof(MarginConsumed)}: {MarginConsumed}";
    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() =>
        $"{nameof(PQAdditionalExternalCounterPartyInfo)}({PQInternalPassiveOrderLayerInfoToStringMembers}, {UpdatedFlagsToString})";
}
