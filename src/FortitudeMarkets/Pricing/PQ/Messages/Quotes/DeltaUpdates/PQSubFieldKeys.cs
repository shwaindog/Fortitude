namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates
{
    public enum PQPricingSubFieldKeys : byte
    {
        None = 0

        // Aligns with CrudCommand enum values
      , CommandInsert        = 1 // Create
      , CommandRead          = 2 // Read
      , CommandUpdate        = 3 // Update
      , CommandDelete        = 4 // Delete or remove element
      , CommandUpsert        = 5 // Create or Update
      , CommandReset         = 6 // Clear/Wipe keep position
      , CommandElementsShift = 7 // Similar to bit shift but for list elements
        // ( to include a list virtual fix/drop index and will shift above or below the point)


        // Used by OpenInterest and Daily Traded Volume
      , MarketAggregateSource            = 20 // 0x14
      , MarketAggregateUpdateDate        = 21 // 0x15
      , MarketAggregateUpdateSub2MinTime = 22 // 0x16
      , MarketAggregateVolume            = 23 // 0x17
      , MarketAggregateVwap              = 24 // 0x18

      , SourceSequenceId           = 30 // 0x1E
      , AdapterSequenceId          = 31 // 0x1F
      , AdapterLifeTimeSequenceId  = 32 // 0x20
      , ClientSequenceId           = 33 // 0x21
      , SignalProcessingSequenceId = 34 // 0x22
      , StrategySequenceId         = 35 // 0x23
      , AlgoSequenceId             = 36 // 0x24
      , StoragePersistSequenceId   = 37 // 0x25

      , CandleSummaryPeriod    = 40 // 0x28
      , CandleStartDateTime    = 41 // 0x29
      , CandleStartSub2MinTime = 42 // 0x2A
      , CandleStartPrice       = 43 // 0x2B
      , CandleEndDateTime      = 44 // 0x2C
      , CandleEndSub2MinTime   = 45 // 0x2D
      , CandleEndPrice         = 46 // 0x2E
      , CandleHighestPrice     = 47 // 0x2F
      , CandleLowestPrice      = 48 // 0x30
      , CandleTickCount        = 49 // 0x31
      , CandleVolume           = 50 // 0x32
      , CandleSummaryFlags     = 51 // 0x33
      , CandleAveragePrice     = 52 // 0x34
      , CandleVolatility       = 53 // 0x35

      , MovingAveragePeriod           = 60 // 0x3C
      , MovingAverageStartDate        = 61 // 0x3D
      , MovingAverageStartSub2MinTime = 62 // 0x3E
      , MovingAverageMidPrice         = 63 // 0x3F
      , MovingAverageAskPrice         = 64 // 0x40
      , MovingAverageBidPrice         = 65 // 0x41
      , MovingAverageTickCount        = 66 // 0x42
      , MovingAveragePeriodVolatility = 67 // 0x43
      , MovingAverageGradient         = 68 // 0x44

      , VolatilityPeriod           = 70 // 0x46
      , VolatilityStartDate        = 71 // 0x47
      , VolatilityStartSub2MinTime = 72 // 0x48
      , VolatilityEndDate          = 73 // 0x49
      , VolatilityEndSub2MinTime   = 74 // 0x4A
      , VolatilityPriceValue       = 75 // 0x4B
      , VolatilityNormalisedValue  = 76 // 0x4C
      , VolatilityTickCount        = 77 // 0x4D
      , VolatilityFromMidPrice     = 78 // 0x4E

      , MarketEventId                          = 80  // 0x50
      , MarketEventSource                      = 81  // 0x51
      , MarketEventSourceNameId                = 82  // 0x52
      , MarketEventType                        = 83  // 0x53
      , MarketEventDescriptionTextId           = 84  // 0x54
      , MarketEventStartDate                   = 85  // 0x55
      , MarketEventStartSub2MinTime            = 86  // 0x56
      , MarketEventEstimateReceivedDate        = 87  // 0x57
      , MarketEventEstimateReceivedSub2MinTime = 88  // 0x58
      , MarketEventEstimateSource              = 89  // 0x59
      , MarketEventEstimateDescriptionTextId   = 90  // 0x5A
      , MarketEventEstimateSeverity            = 91  // 0x5B
      , MarketEventEstimatedActiveTimeMs       = 92  // 0x5C
      , MarketEventEstimatedValue              = 93  // 0x5D
      , MarketEventEstimatedProbability        = 94  // 0x5E
      , MarketEventOutcome                     = 95  // 0x5F
      , MarketEventActualSeverity              = 96  // 0x60
      , MarketEventOutcomeDescriptionTextId    = 97  // 0x61
      , MarketEventEndAtDate                   = 98  // 0x62
      , MarketEventEndAtSub2MinTime            = 99  // 0x63
      , MarketEventActualValue                 = 100 // 0x64
      , MarketEventFeedState                   = 101 // 0x65

      , PricePointId                 = 110 // 0x6E
      , PricePointSourceId           = 111 // 0x6F
      , PricePointDescriptionTextId  = 112 // 0x70
      , PricePointCreatedDate        = 113 // 0x71
      , PricePointCreatedSub2MinTime = 114 // 0x72
      , PricePointDate               = 115 // 0x73
      , PriceSub2MinTime             = 116 // 0x74
      , PricePointPrice              = 117 // 0x75
      , PricePointScalarValue        = 118 // 0x77

      , PivotId                       = 120 // 0x78
      , PivotPeriodType               = 122 // 0x79
      , PivotType                     = 123 // 0x7A
      , PivotLinkedMarketEventId      = 124 // 0x7B
      , PivotState                    = 125 // 0x7C
      , PivotPricePoint               = 126 // 0x7D
      , PivotMinimumPriceDistance     = 127 // 0x7E
      , PivotMinimumTimeDistanceMs    = 128 // 0x7F
      , PivotStrength                 = 129 // 0x80
      , PivotPreviousCrossDate        = 130 // 0x81
      , PivotPreviousCrossSub2MinTime = 131 // 0x82
      , PivotBreachDate               = 132 // 0x83
      , PivotBreachSub2MinTime        = 133 // 0x84

      , PriceBoundaryLineId                       = 140 // 0x8C
      , PriceBoundaryLinePeriodType               = 141 // 0x8D
      , PriceBoundaryLineType                     = 142 // 0x8E
      , PriceBoundaryLineBreachState              = 143 // 0x8F
      , PriceBoundaryLineStrength                 = 144 // 0x90
      , PriceBoundaryLineSourceType               = 145 // 0x91
      , PriceBoundaryLineSource                   = 146 // 0x92
      , PriceBoundaryLineSourceNameId             = 147 // 0x93
      , PriceBoundaryLineDescriptionTextId        = 148 // 0x94
      , PriceBoundaryLineNameId                   = 149 // 0x95
      , PriceBoundaryLineMaxBreachPriceThreshold  = 150 // 0x96
      , PriceBoundaryLineMaxBreachPeriod          = 151 // 0x97
      , PriceBoundaryLinePricePoints              = 152 // 0x98
      , PriceBoundaryLinePreviousCrossDate        = 153 // 0x99
      , PriceBoundaryLinePreviousCrossSub2MinTime = 154 // 0x9A
      , PriceBoundaryLineBreachDateDate           = 155 // 0x9B
      , PriceBoundaryLineBreachSub2MinTime        = 156 // 0x9C

      , PriceAdjustmentAtDate          = 160 // 0xA0
      , PriceAdjustmentAtSub2MinTime   = 161 // 0xA1
      , PriceAdjustmentFromPrice       = 162 // 0xA2
      , PriceAdjustmentToPrice         = 163 // 0xA3
      , PriceAdjustmentPriceMultiplier = 164 // 0xA4
      , PriceAdjustmentPriceShift      = 165 // 0xA5

      , ContinuousPriceAdjustmentType                    = 170  // 0xAA
      , ContinuousPriceAdjustmentReason                  = 171  // 0xAB
      , ContinuousPriceAdjustmentToNext                  = 172  // 0xAC
      , ContinuousPriceAdjustmentFromPrevious            = 173  // 0xAD
      , ContinuousPriceAdjustmentPriceLifeTimeMultiplier = 174  // 0xAE
      , ContinuousPriceAdjustmentPriceLifeTimeShift      = 175  // 0xAF
      , ContinuousPriceAdjustmentContractId              = 176  // 0xB0
      , ContinuousPriceAdjustmentContractExpiryDate      = 177  // 0xB1
      , ContinuousPriceAdjustmentContractNameId          = 178  // 0xB2
      , ContinuousPriceAdjustmentDeltaIssued             = 179  // 0xB3
      , ContinuousPriceAdjustmentLiftTimeTotalIssued     = 180  // 0xB4
    }

    public enum PQTradingSubFieldKeys : byte
    {
        None = 0

        // Aligns with CrudCommand enum values
      , CommandInsert        = 1 // Create
      , CommandRead          = 2 // Read
      , CommandUpdate        = 3 // Update
      , CommandDelete        = 4 // Delete or remove element
      , CommandUpsert        = 5 // Create or Update
      , CommandReset         = 6 // Clear/Wipe keep position
      , CommandElementsShift = 7 // Similar to bit shift but for list elements
        // ( to include a list virtual fix/drop index and will shift above or below the point)


      , StrategyDecisionId                          = 20 // 0x14
      , StrategyId                                  = 21 // 0x15
      , StrategyNameId                              = 22 // 0x16
      , StrategyDecisionIntentType                  = 23 // 0x17
      , StrategyDecisionIntentNameId                = 24 // 0x18
      , StrategyDecisionFinalType                   = 25 // 0x19
      , StrategyDecisionFinalNameId                 = 26 // 0x1A
      , StrategyDecisionFinalReasonTextId           = 27 // 0x1B
      , StrategyStateType                           = 28 // 0x1C
      , StrategyStateDescriptionTextId              = 29 // 0x1D
      , StrategyDecisionDate                        = 30 // 0x1E
      , StrategyDecisionSub2MinTime                 = 31 // 0x1F
      , StrategyDecisionOrderId                     = 32 // 0x20
      , StrategyDecisionIntentSignalValue           = 33 // 0x21
      , StrategyDecisionIntentSignalThreshold       = 34 // 0x22
      , StrategyDecisionIntentSourceNameId          = 35 // 0x23
      , StrategyDecisionIntentStrength              = 36 // 0x24
      , StrategyDecisionIntentAccuracyConfidence    = 37 // 0x25
      , StrategyDecisionIntentDirection             = 38 // 0x26
      , StrategyDecisionIntentTargetPrice           = 39 // 0x27
      , StrategyDecisionIntentTargetPriceBoundaryId = 40 // 0x28
      , StrategyDecisionEstimatedTimeToLive         = 41 // 0x29
      , StrategyDecisionKillPrice                   = 42 // 0x2A
      , StrategyDecisionKillPriceBoundaryId         = 43 // 0x2B
      , StrategyDecisionOutcome                     = 44 // 0x2C
      , StrategyDecisionOutcomeAtDate               = 45 // 0x2D
      , StrategyDecisionOutcomeAtSub2MinTime        = 46 // 0x2E
      , StrategyDecisionOutcomeSuccessRating        = 47 // 0x2F

      , SignalPublishId                      = 53  // 0x35
      , SignalSourceId                       = 54  // 0x36
      , SignalSourceNameId                   = 55  // 0x37
      , SignalType                           = 56  // 0x38
      , SignalCategory                       = 57  // 0x39
      , SignalDescriptionTextId              = 58  // 0x3A
      , SignalUpdatedDate                    = 59  // 0x3B
      , SignalUpdatedSub2MinTime             = 60  // 0x3C
      , SignalEstimateTimeToLiveMs           = 61  // 0x3D
      , SignalTargetPrice                    = 62  // 0x3E
      , SignalTargetPriceBoundaryId          = 63  // 0x3F
      , SignalIsFalseKillPrice               = 64  // 0x40
      , SignalIsFalsePriceBoundaryId         = 65  // 0x41
      , SignalDirection                      = 66  // 0x42
      , SignalValue                          = 67  // 0x43
      , SignalTriggerThresholdValue          = 68  // 0x44
      , SignalStrength                       = 69  // 0x45
      , SignalAccuracyConfidence             = 70  // 0x46
      , SignalLinkedStrategyWeighting        = 71  // 0x47
      , SignalRecentAccuracy                 = 72  // 0x48
      , SignalAggregateWeighting             = 73  // 0x49
      , SignalState                          = 74  // 0x4A
      , SignalLastDecision                   = 75  // 0x4B
      , SignalLastDecisionDate               = 76  // 0x4C
      , SignalLastDecisionSub2MinTime        = 77  // 0x4D
      , SignalStateDecisionDescriptionTextId = 78  // 0x4E
      , SignalStateDecisionReasonTextId      = 79  // 0x4F

      , LimitNameId                    = 90   // 0x5A
      , LimitType                      = 91   // 0x5B
      , LimitCategory                  = 92   // 0x5C
      , LimitPortfolioId               = 93   // 0x5D
      , LimitStrategyId                = 94   // 0x5E
      , LimitTraderId                  = 95   // 0x5F
      , LimitAppliedToNameId           = 96   // 0x60
      , LimitLastUpdateDate            = 98   // 0x61
      , LimitLastUpdateSub2MinTime     = 99   // 0x62
      , LimitApplyPeriod               = 100  // 0x63
      , LimitCurrentValue              = 101  // 0x64
      , LimitBreachThreshold           = 102  // 0x65
      , LimitAlertApproachingThreshold = 103  // 0x66
      , LimitStatus                    = 104  // 0x67

      , LimitBreachPublishId               = 115  // 0x73
      , LimitBreachType                    = 116  // 0x74
      , LimitBreachNameId                  = 117  // 0x75
      , LimitBreachState                   = 118  // 0x76
      , LimitBreachOutcome                 = 119  // 0x77
      , LimitBreachTradingState            = 120  // 0x78
      , LimitBreachExpectedRecoveryDate    = 121  // 0x79
      , LimitBreachExpectedRecoverySub2Min = 122  // 0x7A
      , LimitBreachStateAsOfDate           = 123  // 0x7B
      , LimitBreachStateAsOfSub2MinTime    = 124  // 0x7C
      , LimitBreachCurrentValue            = 125  // 0x7D
      , LimitBreachAttemptedValue          = 126  // 0x7E
      , LimitBreachAttemptedOrderId        = 127  // 0x7F

      , TradingAccountId                           = 140 // 0x8C
      , TradingAccountNameId                       = 141 // 0x8D
      , TradingAccountTradingStatus                = 142 // 0x8E
      , TradingAccountTickerNetOpenDirection       = 143 // 0x8F
      , TradingAccountTickerNetOpenMarketAggregate = 144 // 0x90
      , TradingAccountTickerNetOpenPnl             = 145 // 0x91
      , TradingAccountTickerClosedDailyPnl         = 146 // 0x92

      , ExecutionSummaryPeriod                 = 160 // 0xA0
      , ExecutionPeriodStartDateTime           = 161 // 0xA1
      , ExecutionPeriodStartSub2MinTime        = 162 // 0xA2
      , ExecutionPeriodSlippageDelta           = 163 // 0xA3
      , ExecutionPeriodInternalOrderFillCount  = 164 // 0xA4
      , ExecutionPeriodSourceTradeCount        = 165 // 0xA5
      , ExecutionPeriodSourcePaidCount         = 166 // 0xA6
      , ExecutionPeriodSourceGivenCount        = 167 // 0xA7
      , ExecutionPeriodAdapterTradeCount       = 168 // 0xA8
      , ExecutionPeriodInternalFillRatio       = 169 // 0xA9
      , ExecutionPeriodInternalFillPriceDelta  = 170 // 0xAA
      , ExecutionPeriodTargetToActualDelta     = 171 // 0xAB
      , ExecutionPeriodPlacedConfirmTimeUs     = 172 // 0xAC
      , ExecutionPeriodAvgAggressiveFillTimeMs = 173 // 0xAD
      , ExecutionPeriodEffectivePriceDelta     = 174 // 0xAE

      , OrderId                                    = 180 // 0xB4
      , OrderSequenceId                            = 181 // 0xB5
      , OrderType                                  = 182 // 0xB6
      , OrderFlags                                 = 183 // 0xB7
      , OrderCreatedDate                           = 184 // 0xB8
      , OrderCreatedSub2MinTime                    = 185 // 0xB9
      , OrderSubmitPrice                           = 186 // 0xBA
      , OrderUpdatedDate                           = 187 // 0xBB
      , OrderUpdatedSub2MinTime                    = 188 // 0xBC
      , OrderVolume                                = 189 // 0xBD
      , OrderRemainingVolume                       = 190 // 0xBE
      , OrderInternalTargetPrice                   = 191 // 0xBF
      , OrderInternalDecisionSourceSentDate        = 192 // 0xC0
      , OrderInternalDecisionSourceSentSub2MinTime = 193 // 0xC1
      , OrderInternalTrackingId                    = 194 // 0xC2
      , OrderInternalDeskNameId                    = 195 // 0xC3
      , OrderInternalDivisionNameId                = 196 // 0xC4
      , OrderInternalStrategyDecisionId            = 197 // 0xC5
      , OrderInternalStrategyNameId                = 198 // 0xC6
      , OrderInternalPortfolioNameId               = 199 // 0xC7
      , OrderInternalMarginConsumed                = 200 // 0xC8
      , OrderCounterPartyNameId                    = 201 // 0xC9
      , OrderTraderNameId                          = 202 // 0xCA

      , LastTradedSummaryPeriod           = 220 // 0xDC
      , LastTradedTradeId                 = 221 // 0xDD
      , LastTradedOrderId                 = 222 // 0xDE
      , LastTradedBatchId                 = 223 // 0xDF
      , LastTradedActionCommand           = 224 // 0xE0
      , LastTradedAtPrice                 = 225 // 0xE1
      , LastTradedTradeTimeDate           = 226 // 0xE2
      , LastTradedTradeSub2MinTime        = 227 // 0xE3
      , LastTradedOrderVolume             = 228 // 0xE4
      , LastTradedBooleanFlags            = 229 // 0xE5
      , LastTradedInternalTargetPrice     = 230 // 0xE6
      , LastTradedInternalSubmitPrice     = 231 // 0xE7
      , LastTradedInternalLastFillNameId  = 232 // 0xE8
      , LastTradedInternalOrderIdNameId   = 233 // 0xE9
      , LastTradedInternalPlacerNameId    = 234 // 0xEA
      , LastTradedInternalDeskNameId      = 235 // 0xEB
      , LastTradedInternalDivisionNameId  = 236 // 0xEC
      , LastTradedInternalTrackingNameId  = 237 // 0xED
      , LastTradedInternalStrategyNameId  = 238 // 0xEE
      , LastTradedInternalPortfolioNameId = 239 // 0xEF
      , LastTradedInternalPnl             = 240 // 0xF0
      , LastTradedCounterPartyId          = 241 // 0xF1
      , LastTradedTraderId                = 242 // 0xF2
    }

    public static class PQSubFieldKeyExtensions
    {
        public static string ToSubIdString(this byte subId, PQQuoteFields parentId)
        {
            return parentId.IsPricingSubKey() switch
                   {
                       true  => ((PQPricingSubFieldKeys)subId).ToString()
                     , false => ((PQTradingSubFieldKeys)subId).ToString()
                   };
        }

        public static bool IsPricingSubKey(this PQQuoteFields parentId)
        {
            return parentId switch
                   {
                       _ when parentId <= PQQuoteFields.PriceBoundaryLinesRecentlyBreached => true
                     , _ when parentId <= PQQuoteFields.PortfolioLimitApproachingAlert     => false
                     , _ when parentId <= PQQuoteFields.OrdersCount                        => true
                     , _ when parentId <= PQQuoteFields.LayerOrders                        => false
                     , _ when parentId <= PQQuoteFields.AllLayersRangeEnd                  => true
                     , _ when parentId <= PQQuoteFields.QuoteExpiryDate                    => false
                     , _                                                                  => true
                   };
        }

        public static bool IsTradingSubKey(this PQQuoteFields parentId)
        {
            return !parentId.IsPricingSubKey();
        }
    }
}
