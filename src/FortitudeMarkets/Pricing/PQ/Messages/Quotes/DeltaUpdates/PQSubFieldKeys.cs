namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates
{
    public enum PQSubFieldKeys : byte
    {
        None = 0

        // Aligns with CrudCommand enum values
      , CommandInsert = 1 // Create
      , CommandRead   = 2 // Read
      , CommandUpdate = 3 // Update
      , CommandDelete = 4 // Delete
      , CommandUpsert = 5 // Create or Update

      , ContinuousPriceAdjustmentDate        = 6
      , ContinuousPriceAdjustmentType        = 7
      , ContinuousPriceAdjustmentSub2MinTime = 8
      , ContinuousPriceAdjustmentMultiplier  = 9

      , SourceSequenceId           = 12 // 0x0C
      , AdapterSequenceId          = 13 // 0x0D
      , ClientSequenceId           = 14 // 0x0E
      , SignalProcessingSequenceId = 15 // 0x0F
      , StrategySequenceId         = 16 // 0x10
      , AlgoSequenceId             = 17 // 0x11
      , StoragePersistSequenceId   = 18 // 0x12

      , PriceSummaryPeriod              = 20 // 0x14
      , PricePeriodStartDateTime        = 21 // 0x15
      , PricePeriodStartSub2MinTime     = 22 // 0x16
      , PricePeriodStartPrice           = 23 // 0x17
      , PricePeriodEndDateTime          = 24 // 0x18
      , PricePeriodEndSub2MinTime       = 25 // 0x19
      , PricePeriodEndPrice             = 26 // 0x1A
      , PricePeriodHighestPrice         = 27 // 0x1B
      , PricePeriodLowestPrice          = 28 // 0x1C
      , PricePeriodTickCount            = 29 // 0x1D
      , PricePeriodVolume               = 30 // 0x1E
      , PricePeriodSummaryFlags         = 31 // 0x1F
      , PricePeriodAveragePrice         = 32 // 0x20
      , PricePeriodNormalisedVolatility = 33 // 0x21

      , MovingAveragePeriod           = 37 // 0x25
      , MovingAverageStartDate        = 38 // 0x26
      , MovingAverageStartSub2MinTime = 39 // 0x27
      , MovingAverageMidPrice         = 40 // 0x28
      , MovingAverageAskPrice         = 41 // 0x29
      , MovingAverageBidPrice         = 42 // 0x2A
      , MovingAverageTickCount        = 43 // 0x2B 
      , MovingAveragePeriodVolatility = 44 // 0x2C
      , MovingAverageGradient         = 45 // 0x2D

      , VolatilityPeriod           = 46 // 0x2E
      , VolatilityStartDate        = 47 // 0x2F
      , VolatilityStartSub2MinTime = 48 // 0x30
      , VolatilityEndDate          = 49 // 0x31
      , VolatilityEndSub2MinTime   = 50 // 0x32
      , VolatilityPriceValue       = 51 // 0x33
      , VolatilityNormalisedValue  = 52 // 0x34
      , VolatilityTickCount        = 53 // 0x35
      , VolatilityFromMidPrice     = 54 // 0x36

      , MarketEventId                          = 55 // 0x37
      , MarketEventSource                      = 56 // 0x38
      , MarketEventSourceNameId                = 57 // 0x39
      , MarketEventType                        = 58 // 0x3A
      , MarketEventDescriptionTextId           = 59 // 0x3B
      , MarketEventStartDate                   = 60 // 0x3C
      , MarketEventStartSub2MinTime            = 61 // 0x3D
      , MarketEventEstimateReceivedDate        = 62 // 0x3E
      , MarketEventEstimateReceivedSub2MinTime = 63 // 0x3F
      , MarketEventEstimateSource              = 64 // 0x40
      , MarketEventEstimateDescriptionTextId   = 65 // 0x41
      , MarketEventEstimateSeverity            = 66 // 0x42
      , MarketEventEstimatedActiveTimeMs       = 67 // 0x43
      , MarketEventEstimatedValue              = 68 // 0x44
      , MarketEventEstimatedProbability        = 69 // 0x45
      , MarketEventOutcome                     = 70 // 0x46
      , MarketEventActualSeverity              = 71 // 0x47
      , MarketEventOutcomeDescriptionTextId    = 72 // 0x48
      , MarketEventEndAtDate                   = 73 // 0x49
      , MarketEventEndAtSub2MinTime            = 74 // 0x4A
      , MarketEventActualValue                 = 75 // 0x4B
      , MarketEventFeedState                   = 76 // 0x4C

      , PricePointId                 = 80 // 0x50
      , PricePointSourceId           = 81 // 0x51
      , PricePointDescriptionTextId  = 82 // 0x52
      , PricePointCreatedDate        = 83 // 0x53
      , PricePointCreatedSub2MinTime = 84 // 0x54
      , PricePointDate               = 85 // 0x55
      , PriceSub2MinTime             = 86 // 0x56
      , PricePointPrice              = 87 // 0x57
      , PricePointScalarValue        = 88 // 0x58

      , PivotId                       = 90  // 0x5A
      , PivotPeriodType               = 91  // 0x5B
      , PivotType                     = 92  // 0x5C
      , PivotLinkedMarketEventId      = 93  // 0x5D
      , PivotState                    = 94  // 0x5E
      , PivotPricePoint               = 95  // 0x5F
      , PivotMinimumPriceDistance     = 96  // 0x60
      , PivotMinimumTimeDistanceMs    = 97  // 0x61
      , PivotStrength                 = 98  // 0x62
      , PivotPreviousCrossDate        = 99  // 0x63
      , PivotPreviousCrossSub2MinTime = 100 // 0x64
      , PivotBreachDate               = 101 // 0x65
      , PivotBreachSub2MinTime        = 102 // 0x66

      , PriceBoundaryLineId                       = 103 // 0x67 
      , PriceBoundaryLinePeriodType               = 104 // 0x68 
      , PriceBoundaryLineType                     = 105 // 0x69 
      , PriceBoundaryLineBreachState              = 106 // 0x6A 
      , PriceBoundaryLineStrength                 = 107 // 0x6B 
      , PriceBoundaryLineSourceType               = 108 // 0x6C 
      , PriceBoundaryLineSource                   = 109 // 0x6D 
      , PriceBoundaryLineSourceNameId             = 110 // 0x6E 
      , PriceBoundaryLineDescriptionTextId        = 111 // 0x6F 
      , PriceBoundaryLineNameId                   = 112 // 0x70
      , PriceBoundaryLineMaxBreachPriceThreshold  = 113 // 0x71
      , PriceBoundaryLineMaxBreachPeriod          = 114 // 0x72
      , PriceBoundaryLinePricePoints              = 115 // 0x73
      , PriceBoundaryLinePreviousCrossDate        = 116 // 0x74
      , PriceBoundaryLinePreviousCrossSub2MinTime = 117 // 0x75
      , PriceBoundaryLineBreachDateDate           = 118 // 0x76
      , PriceBoundaryLineBreachSub2MinTime        = 119 // 0x77

      , StrategyDecisionId                          = 126 // 0x7E
      , StrategyId                                  = 127 // 0x7F
      , StrategyNameId                              = 128 // 0x80
      , StrategyDecisionIntentType                  = 129 // 0x81
      , StrategyDecisionIntentNameId                = 130 // 0x82
      , StrategyDecisionFinalType                   = 131 // 0x83
      , StrategyDecisionFinalNameId                 = 132 // 0x84
      , StrategyDecisionFinalReasonTextId           = 133 // 0x85
      , StrategyStateType                           = 134 // 0x86
      , StrategyStateDescriptionTextId              = 135 // 0x87
      , StrategyDecisionDate                        = 136 // 0x88
      , StrategyDecisionSub2MinTime                 = 137 // 0x89
      , StrategyDecisionOrderId                     = 138 // 0x8A
      , StrategyDecisionIntentSignalValue           = 139 // 0x8B
      , StrategyDecisionIntentSignalThreshold       = 140 // 0x8C
      , StrategyDecisionIntentSourceNameId          = 141 // 0x8D
      , StrategyDecisionIntentStrength              = 142 // 0x8E
      , StrategyDecisionIntentAccuracyConfidence    = 143 // 0x8F
      , StrategyDecisionIntentDirection             = 144 // 0x90
      , StrategyDecisionIntentTargetPrice           = 145 // 0x91
      , StrategyDecisionIntentTargetPriceBoundaryId = 146 // 0x92
      , StrategyDecisionEstimatedTimeToLive         = 147 // 0x93
      , StrategyDecisionKillPrice                   = 148 // 0x94
      , StrategyDecisionKillPriceBoundaryId         = 149 // 0x95
      , StrategyDecisionOutcome                     = 150 // 0x96
      , StrategyDecisionOutcomeAtDate               = 151 // 0x97
      , StrategyDecisionOutcomeAtSub2MinTime        = 152 // 0x98
      , StrategyDecisionOutcomeSuccessRating        = 153 // 0x99

      , SignalPublishId               = 159 // 0x9F
      , SignalSourceId                = 160 // 0xA0
      , SignalCategory                = 161 // 0xA1
      , SignalCreatedDate             = 162 // 0xA2
      , SignalCreatedSub2MinTime      = 163 // 0xA3
      , SignalEstimateTimeToLiveMs    = 164 // 0xA4
      , SignalTargetPrice             = 165 // 0xA5
      , SignalTargetPriceBoundaryId   = 166 // 0xA6
      , SignalIsFalseKillPrice        = 167 // 0xA7
      , SignalIsFalsePriceBoundaryId  = 168 // 0xA8
      , SignalDirection               = 169 // 0xA9
      , SignalStrength                = 170 // 0xAA
      , SignalAccuracyConfidence      = 171 // 0xAB
      , SignalLinkedStrategyWeighting = 172 // 0xAC

      , ExecutionSummaryPeriod                 = 178 // 0xB2
      , ExecutionPeriodStartDateTime           = 179 // 0xB3
      , ExecutionPeriodStartSub2MinTime        = 180 // 0xB4
      , ExecutionPeriodSlippageDelta           = 181 // 0xB5
      , ExecutionPeriodInternalOrderFillCount  = 182 // 0xB6
      , ExecutionPeriodSourceTradeCount        = 183 // 0xB7
      , ExecutionPeriodSourcePaidCount         = 184 // 0xB8
      , ExecutionPeriodSourceGivenCount        = 185 // 0xB9
      , ExecutionPeriodAdapterTradeCount       = 186 // 0xBA
      , ExecutionPeriodInternalFillRatio       = 187 // 0xBB
      , ExecutionPeriodInternalFillPriceDelta  = 188 // 0xBC
      , ExecutionPeriodTargetToActualDelta     = 189 // 0xBD
      , ExecutionPeriodPlacedConfirmTimeUs     = 190 // 0xBE
      , ExecutionPeriodAvgAggressiveFillTimeMs = 191 // 0xBF
      , ExecutionPeriodEffectivePriceDelta     = 192 // 0xC0

      , OrderId                         = 195 // 0xC3
      , OrderSequenceId                 = 196 // 0xC4
      , OrderType                       = 197 // 0xC5
      , OrderFlags                      = 198 // 0xC6
      , OrderCreatedDate                = 199 // 0xC7
      , OrderCreatedSub2MinTime         = 200 // 0xC8
      , OrderSubmitPrice                = 201 // 0xC9
      , OrderUpdatedDate                = 202 // 0xCA
      , OrderUpdatedSub2MinTime         = 203 // 0xCB
      , OrderVolume                     = 204 // 0xCC
      , OrderRemainingVolume            = 205 // 0xCD
      , OrderInternalTargetPrice        = 206 // 0xCE
      , OrderInternalTrackingId         = 207 // 0xCF
      , OrderInternalDeskNameId         = 208 // 0xD0
      , OrderInternalDivisionNameId     = 209 // 0xD1
      , OrderInternalStrategyDecisionId = 210 // 0xD2
      , OrderInternalStrategyNameId     = 211 // 0xD3
      , OrderInternalPortfolioNameId    = 212 // 0xD4
      , OrderCounterPartyNameId         = 213 // 0xD5
      , OrderTraderNameId               = 214 // 0xD6

        // Used by OpenInterest and Daily Traded Volume
      , MarketAggregateSource            = 216 // 0xD8
      , MarketAggregateUpdateDate        = 217 // 0xD9
      , MarketAggregateUpdateSub2MinTime = 218 // 0xDA
      , MarketAggregateVolume            = 219 // 0xDB
      , MarketAggregateVwap              = 220 // 0xDC

      , LastTradedSummaryPeriod    = 221 // 0xDD
      , LastTradedTradeId          = 222 // 0xDE
      , LastTradedOrderId          = 223 // 0xDF
      , LastTradedBatchId          = 224 // 0xE0
      , LastTradedActionCommand    = 225 // 0xE1
      , LastTradedAtPrice          = 226 // 0xE2
      , LastTradedTradeTimeDate    = 227 // 0xE3
      , LastTradedTradeSub2MinTime = 228 // 0xE4
      , LastTradedOrderVolume      = 229 // 0xE5
      , LastTradedBooleanFlags     = 230 // 0xE6

      , LastTradedInternalTargetPrice     = 231 // 0xE7
      , LastTradedInternalSubmitPrice     = 232 // 0xE8
      , LastTradedInternalLastFillNameId  = 233 // 0xE9
      , LastTradedInternalOrderIdNameId   = 234 // 0xEA
      , LastTradedInternalPlacerNameId    = 235 // 0xEB
      , LastTradedInternalDeskNameId      = 236 // 0xEC
      , LastTradedInternalDivisionNameId  = 237 // 0xED
      , LastTradedInternalTrackingNameId  = 238 // 0xEE
      , LastTradedInternalStrategyNameId  = 239 // 0xEF
      , LastTradedInternalPortfolioNameId = 240 // 0xF0
      , LastTradedCounterPartyId          = 241 // 0xF1
      , LastTradedTraderId                = 242 // 0xF2

      , LimitBreachPublishId            = 245
      , LimitBreachType                 = 246
      , LimitBreachNameId               = 247
      , LimitBreachState                = 248
      , LimitBreachStateAsOfDate        = 249
      , LimitBreachStateAsOfSub2MinTime = 250
      , LimitBreachCurrentValue         = 251
      , LimitBreachAttemptedValue       = 252
      , LimitBreachAttemptedOrderId     = 253
      , LimitBreachThreshold            = 254
      , LimitBreachSlidingWindowPeriod  = 255
    }
}
