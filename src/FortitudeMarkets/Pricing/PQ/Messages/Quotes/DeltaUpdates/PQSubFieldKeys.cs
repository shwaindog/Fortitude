using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates
{
    public enum PQSubFieldKeys : byte
    {
        None           = 0
            
        // Aligns with CrudCommand enum values
     ,  CommandInsert  = 1  // Create
     ,  CommandRead    = 2  // Read
     ,  CommandUpdate = 3   // Update
     ,  CommandDelete = 4   // Delete
     ,  CommandUpsert = 5   // Create or Update

      , PriceSummaryPeriod              = 10
      , PricePeriodStartDateTime        = 11
      , PricePeriodStartSub2MinTime     = 12
      , PricePeriodStartPrice           = 13
      , PricePeriodEndDateTime          = 14
      , PricePeriodEndSub2MinTime       = 15
      , PricePeriodEndPrice             = 16
      , PricePeriodHighestPrice         = 17
      , PricePeriodLowestPrice          = 18
      , PricePeriodTickCount            = 19 // 0x0A
      , PricePeriodVolume               = 20 // 0x0B
      , PricePeriodSummaryFlags         = 21 // 0x0C
      , PricePeriodAveragePrice         = 22 // 0x0D
      , PricePeriodNormalisedVolatility = 23 // 0x0D

      , OrderId                      = 50 // 0x32
      , OrderSequenceId              = 51 // 0x33
      , OrderFlags                   = 52 // 0x34
      , OrderCreatedDate             = 53 // 0x35
      , OrderCreatedSub2MinTime      = 54 // 0x36
      , OrderUpdatedDate             = 55 // 0x37
      , OrderUpdatedSub2MinTime      = 56 // 0x38
      , OrderVolume                  = 57 // 0x39
      , OrderRemainingVolume         = 58 // 0x3A
      , OrderInternalTrackingId      = 61 // 0x3D
      , OrderInternalDeskNameId      = 62 // 0x3E
      , OrderInternalDivisionNameId  = 63 // 0x3F
      , OrderInternalStrategyNameId  = 64 // 0x40
      , OrderInternalPortfolioNameId = 65 // 0x41
      , OrderCounterPartyNameId      = 59 // 0x3B
      , OrderTraderNameId            = 60 // 0x3C

      , PivotSummaryPeriod            = 80 // 0x42
      , PivotType                     = 81 // 0x43
      , PivotId                       = 82 // 0x44
      , PivotState                    = 82 // 0x45
      , PivotPrice                    = 83 // 0x46
      , PivotTimeDate                 = 84 // 0x47
      , PivotSub2MinTime              = 85 // 0x48
      , PivotStrength                 = 86 // 0x49
      , PivotPrecedingTimespanMs      = 87 // 0x4A
      , PivotTerminatedTimespanMs     = 88 // 0x4B
      , PivotPreviousCrossDate        = 89 // 0x4C
      , PivotPreviousCrossSub2MinTime = 90 // 0x4D

      , ExecutionSummaryPeriod                 = 100
      , ExecutionPeriodStartDateTime           = 101
      , ExecutionPeriodStartSub2MinTime        = 102
      , ExecutionPeriodSlippageDelta           = 103
      , ExecutionPeriodInternalOrderFillCount  = 104
      , ExecutionPeriodSourceTradeCount        = 105
      , ExecutionPeriodSourcePaidCount         = 106
      , ExecutionPeriodSourceGivenCount        = 107
      , ExecutionPeriodAdapterTradeCount       = 108
      , ExecutionPeriodInternalFillRatio       = 109
      , ExecutionPeriodTargetToActualDelta     = 110
      , ExecutionPeriodPlacedConfirmTimeUs     = 111
      , ExecutionPeriodAvgAggressiveFillTimeMs = 112
      , ExecutionPeriodEffectivePriceDelta     = 113

      , StrategyPublishId               = 130
      , StrategyDecisionId              = 134
      , StrategyId                      = 131
      , StrategyDecisionType            = 132
      , StrategyStateType               = 133
      , StrategyDecisionDate            = 135
      , StrategyDecisionSub2MinTime     = 136
      , StrategyDecisionPortfolioNameId = 137
      , StrategyAlgoId                  = 138
      , StrategyAlgoDecisionType        = 139
      , StrategyAlgoDecisionStateType   = 140
      , StrategyAlgoDecisionId          = 141

      , SignalPublishId               = 150
      , SignalId                      = 151
      , SignalCreatedDate             = 152
      , SignalCreatedSub2MinTime      = 153
      , SignalValue1                  = 154
      , SignalValue2                  = 155
      , SignalValue3                  = 156
      , SignalValue4                  = 157
      , SignalLinkedStrategyPublishId = 158
      , SignalLinkedStrategyAlgoId    = 159
      , SignalLinkedStrategyIndex     = 160

      , LastTradedSummaryPeriod    = 201 // 0xC9
      , LastTradedTradeId          = 202 // 0xCA
      , LastTradedOrderId          = 203 // 0xCB
      , LastTradedBatchId          = 204 // 0xCC
      , LastTradedActionCommand    = 205 // 0xCD
      , LastTradedAtPrice          = 206 // 0xCE
      , LastTradedTradeTimeDate    = 207 // 0xCF
      , LastTradedTradeSub2MinTime = 208 // 0xD0
      , LastTradedOrderVolume      = 209 // 0xD1
      , LastTradedBooleanFlags     = 210 // 0xD2

      , LastTradedInternalLastFillNameId  = 211 // 0xD3
      , LastTradedInternalOrderIdNameId   = 212 // 0xD4
      , LastTradedInternalPlacerNameId    = 213 // 0xD5
      , LastTradedInternalDeskNameId      = 214 // 0xD6
      , LastTradedInternalDivisionNameId  = 215 // 0xD7
      , LastTradedInternalTrackingNameId  = 216 // 0xD8
      , LastTradedInternalStrategyNameId  = 217 // 0xD9
      , LastTradedInternalPortfolioNameId = 218 // 0xDA
      , LastTradedCounterPartyId          = 219 // 0xDB
      , LastTradedTraderId                = 220 // 0xDC
    }
}
