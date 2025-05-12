namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates
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

      , TrackingAdapterLifeTimeSequenceId       = 10 // 0x0A
      , TrackingSignalProcessingSequenceId      = 11 // 0x0B
      , TrackingSignalProcessingAtDate          = 12 // 0x0C
      , TrackingSignalProcessingAtSub2MinTime   = 13 // 0x0D
      , TrackingStrategySequenceId              = 14 // 0x0E
      , TrackingStrategyProcessingAtDate        = 15 // 0x0F
      , TrackingStrategyProcessingAtSub2MinTime = 16 // 0x10
      , TrackingAlgoSequenceId                  = 17 // 0x11
      , TrackingAlgoProcessingAtDate            = 18 // 0x12
      , TrackingAlgoProcessingAtSub2MinTime     = 19 // 0x13
      , TrackingStoragePersistSequenceId        = 20 // 0x14

      , TickerRegionName                            = 25 // 0x19
      , TickerRegionCurrency                        = 26 // 0x1A
      , TickerRegionDailyCashRate                   = 27 // 0x1B
      , TickerRegionDailyInflation                  = 28 // 0x1C
      , TickerRegionLifeTimeAccumulatedInterestRate = 29 // 0x1D
      , TickerRegionLifeTimeAccumulatedInflation    = 30 // 0x1E
      , TickerRegionPreviousTradingDayDate          = 31 // 0x1F
      , TickerRegionNextTradingDayDate              = 32 // 0x20

      , ConversionRateFromType           = 33 // 0x21
      , ConversionRateToType             = 34 // 0x22
      , ConversionRateCurrency           = 35 // 0x23
      , ConversionRateValue              = 36 // 0x24
      , ConversionRateDailyCashRateDelta = 37 // 0x25
      , ConversionRateUpdatedDate        = 38 // 0x26
      , ConversionRateUpdatedSub2MinTime = 39 // 0x27

      , PnLReportedCurrency               = 45 // 0x2D
      , PnLVolumeConversionConversionRate = 46 // 0x2E
      , PnLPriceConversionRate            = 47 // 0x2F

      , MarginTotal                        = 50 // 0x32
      , MarginVolumeConversionRate         = 51 // 0x33
      , MarginPriceConversionRate          = 52 // 0x34
      , MarginMinimumRequiredBySource      = 53 // 0x35
      , MarginReportedAvailable            = 54 // 0x36
      , MarginEstimatedAvailable           = 55 // 0x37
      , MarginSourceOpenPositionCarryCosts = 56 // 0x38

      , PriceAdjustmentAtDate            = 64 // 0x40
      , PriceAdjustmentAtSub2MinTime     = 65 // 0x41
      , PriceAdjustmentFromPrice         = 66 // 0x42
      , PriceAdjustmentToPrice           = 67 // 0x43
      , PriceAdjustmentDaysApart         = 68 // 0x44
      , PriceAdjustmentDailyInterestRate = 69 // 0x45
      , PriceAdjustmentPriceMultiplier   = 70 // 0x46
      , PriceAdjustmentPriceShift        = 71 // 0x47

      , ContinuousPriceAdjustmentType                    = 74 // 0x4A
      , ContinuousPriceAdjustmentReason                  = 75 // 0x4B
      , ContinuousPriceAdjustmentToNext                  = 76 // 0x4C
      , ContinuousPriceAdjustmentFromPrevious            = 77 // 0x4D
      , ContinuousPriceAdjustmentPriceLifeTimeMultiplier = 78 // 0x4E
      , ContinuousPriceAdjustmentPriceLifeTimeShift      = 79 // 0x4F
      , ContinuousPriceAdjustmentContractId              = 80 // 0x50
      , ContinuousPriceAdjustmentContractExpiryDate      = 81 // 0x51
      , ContinuousPriceAdjustmentContractNameId          = 82 // 0x52
      , ContinuousPriceAdjustmentDeltaIssued             = 83 // 0x53
      , ContinuousPriceAdjustmentLifeTimeTotalIssued     = 84 // 0x54

      , CandlePeriod    = 90  // 0x5A
      , CandleStartDateTime    = 91  // 0x5B
      , CandleStartSub2MinTime = 92  // 0x5C
      , CandleStartPrice       = 93  // 0x5D
      , CandleEndDateTime      = 94  // 0x5E
      , CandleEndSub2MinTime   = 95  // 0x5F
      , CandleEndPrice         = 96  // 0x60
      , CandleHighestPrice     = 97  // 0x61
      , CandleLowestPrice      = 98  // 0x62
      , CandleTickCount        = 99  // 0x63
      , CandleVolume           = 100 // 0x64
      , CandleSummaryFlags     = 101 // 0x65
      , CandleAveragePrice     = 102 // 0x66
      , CandleVolatility       = 103 // 0x67

      , IndicatorMovingAveragePeriod           = 110 // 0x6E
      , IndicatorMovingAverageStartDate        = 111 // 0x6F
      , IndicatorMovingAverageStartSub2MinTime = 112 // 0x70
      , IndicatorMovingAverageMidPrice         = 113 // 0x71
      , IndicatorMovingAverageAskPrice         = 114 // 0x72
      , IndicatorMovingAverageBidPrice         = 115 // 0x73
      , IndicatorMovingAverageTickCount        = 116 // 0x74
      , IndicatorMovingAveragePeriodVolatility = 117 // 0x75
      , IndicatorMovingAverageGradient         = 118 // 0x76

      , IndicatorVolatilityPeriod           = 120 // 0x78
      , IndicatorVolatilityStartDate        = 121 // 0x79
      , IndicatorVolatilityStartSub2MinTime = 122 // 0x7A
      , IndicatorVolatilityEndDate          = 123 // 0x7B
      , IndicatorVolatilityEndSub2MinTime   = 124 // 0x7C
      , IndicatorVolatilityPriceValue       = 125 // 0x7D
      , IndicatorVolatilityNormalisedValue  = 126 // 0x7E
      , IndicatorVolatilityTickCount        = 127 // 0x7F
      , IndicatorVolatilityFromMidPrice     = 128 // 0x80

      , IndicatorPricePointId                 = 130 // 0x82
      , IndicatorPricePointSourceId           = 131 // 0x83
      , IndicatorPricePointDescriptionTextId  = 132 // 0x84
      , IndicatorPricePointCreatedDate        = 133 // 0x85
      , IndicatorPricePointCreatedSub2MinTime = 134 // 0x86
      , IndicatorPricePointDate               = 135 // 0x87
      , IndicatorPriceSub2MinTime             = 136 // 0x88
      , IndicatorPricePointPrice              = 137 // 0x89
      , IndicatorPricePointScalarValue        = 138 // 0x8A

      , IndicatorPivotId                       = 150 // 0x96
      , IndicatorPivotPeriodType               = 151 // 0x97
      , IndicatorPivotType                     = 152 // 0x98
      , IndicatorPivotLinkedMarketEventId      = 153 // 0x99
      , IndicatorPivotState                    = 154 // 0x9A
      , IndicatorPivotPricePoint               = 155 // 0x9B
      , IndicatorPivotMinimumPriceDistance     = 156 // 0x9C
      , IndicatorPivotMinimumTimeDistanceMs    = 157 // 0x9D
      , IndicatorPivotStrength                 = 158 // 0x9E
      , IndicatorPivotPreviousCrossDate        = 159 // 0x9F
      , IndicatorPivotPreviousCrossSub2MinTime = 160 // 0xA0
      , IndicatorPivotBreachDate               = 161 // 0xA1
      , IndicatorPivotBreachSub2MinTime        = 162 // 0xA2

      , IndicatorPriceBoundaryLineId                       = 170 // 0xAA
      , IndicatorPriceBoundaryLinePeriodType               = 171 // 0xAB
      , IndicatorPriceBoundaryLineType                     = 172 // 0xAC
      , IndicatorPriceBoundaryLineBreachState              = 173 // 0xAD
      , IndicatorPriceBoundaryLineStrength                 = 174 // 0xAE
      , IndicatorPriceBoundaryLineSourceType               = 175 // 0xAF
      , IndicatorPriceBoundaryLineSource                   = 176 // 0xB0
      , IndicatorPriceBoundaryLineSourceNameId             = 177 // 0xB1
      , IndicatorPriceBoundaryLineDescriptionTextId        = 178 // 0xB2
      , IndicatorPriceBoundaryLineNameId                   = 179 // 0xB3
      , IndicatorPriceBoundaryLineMaxBreachPriceThreshold  = 180 // 0xB4
      , IndicatorPriceBoundaryLineMaxBreachPeriod          = 181 // 0xB5
      , IndicatorPriceBoundaryLinePricePoints              = 182 // 0xB6
      , IndicatorPriceBoundaryLinePreviousCrossDate        = 183 // 0xB7
      , IndicatorPriceBoundaryLinePreviousCrossSub2MinTime = 184 // 0xB8
      , IndicatorPriceBoundaryLineBreachDateDate           = 185 // 0xB9
      , IndicatorPriceBoundaryLineBreachSub2MinTime        = 186 // 0xBA

      , MarketEventId                          = 200 // 0xC8
      , MarketEventSource                      = 201 // 0xC9
      , MarketEventSourceNameId                = 202 // 0xCA
      , MarketEventType                        = 203 // 0xCB
      , MarketEventDescriptionTextId           = 204 // 0xCC
      , MarketEventStartDate                   = 205 // 0xCD
      , MarketEventStartSub2MinTime            = 206 // 0xCE
      , MarketEventEstimateReceivedDate        = 207 // 0xCF
      , MarketEventEstimateReceivedSub2MinTime = 208 // 0xD0
      , MarketEventEstimateSource              = 209 // 0xD1
      , MarketEventEstimateDescriptionTextId   = 210 // 0xD2
      , MarketEventEstimateSeverity            = 211 // 0xD3
      , MarketEventEstimatedActiveTimeMs       = 212 // 0xD4
      , MarketEventEstimatedValue              = 213 // 0xD5
      , MarketEventEstimatedProbability        = 214 // 0xD6
      , MarketEventOutcome                     = 215 // 0xD7
      , MarketEventActualSeverity              = 216 // 0xD8
      , MarketEventOutcomeDescriptionTextId    = 217 // 0xD9
      , MarketEventEndAtDate                   = 218 // 0xDA
      , MarketEventEndAtSub2MinTime            = 219 // 0xDB
      , MarketEventActualValue                 = 220 // 0xDC
      , MarketEventFeedState                   = 221 // 0xDD

        // Used by OpenInterest and Daily Traded Volume
      , MarketAggregateSource            = 230 // 0xE6
      , MarketAggregateUpdateDate        = 231 // 0xE7
      , MarketAggregateUpdateSub2MinTime = 232 // 0xE8
      , MarketAggregateVolume            = 233 // 0xE9
      , MarketAggregateVwap              = 234 // 0xEA
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

      , SignalPublishId                      = 10 // 0x0A
      , SignalSourceId                       = 11 // 0x0B
      , SignalSourceNameId                   = 12 // 0x0C
      , SignalType                           = 13 // 0x0D
      , SignalCategory                       = 14 // 0x0E
      , SignalDescriptionTextId              = 15 // 0x0F
      , SignalUpdatedDate                    = 16 // 0x10
      , SignalUpdatedSub2MinTime             = 17 // 0x11
      , SignalEstimateTimeToLiveMs           = 18 // 0x12
      , SignalTargetPrice                    = 19 // 0x13
      , SignalTargetPriceBoundaryId          = 20 // 0x14
      , SignalIsFalseKillPrice               = 21 // 0x15
      , SignalIsFalsePriceBoundaryId         = 22 // 0x16
      , SignalDirection                      = 23 // 0x17
      , SignalValue                          = 24 // 0x18
      , SignalTriggerThresholdValue          = 25 // 0x19
      , SignalStrength                       = 26 // 0x1A
      , SignalAccuracyConfidence             = 27 // 0x1B
      , SignalLinkedStrategyWeighting        = 28 // 0x1C
      , SignalRecentAccuracy                 = 29 // 0x1D
      , SignalAggregateWeighting             = 30 // 0x1E
      , SignalState                          = 31 // 0x1F
      , SignalLastDecision                   = 32 // 0x20
      , SignalLastDecisionDate               = 33 // 0x21
      , SignalLastDecisionSub2MinTime        = 34 // 0x22
      , SignalStateDecisionDescriptionTextId = 35 // 0x23
      , SignalStateDecisionReasonTextId      = 36 // 0x24

      , StrategyDecisionId                          = 49 // 0x31
      , StrategyId                                  = 50 // 0x32
      , StrategyNameId                              = 51 // 0x33
      , StrategyDecisionIntentType                  = 52 // 0x34
      , StrategyDecisionIntentNameId                = 53 // 0x35
      , StrategyDecisionFinalType                   = 54 // 0x36
      , StrategyDecisionFinalNameId                 = 55 // 0x37
      , StrategyDecisionFinalReasonTextId           = 56 // 0x38
      , StrategyStateType                           = 57 // 0x39
      , StrategyStateDescriptionTextId              = 58 // 0x3A
      , StrategyDecisionDate                        = 59 // 0x3B
      , StrategyDecisionSub2MinTime                 = 60 // 0x3C
      , StrategyDecisionOrderId                     = 61 // 0x3D
      , StrategyDecisionIntentSignalValue           = 62 // 0x3E
      , StrategyDecisionIntentSignalThreshold       = 63 // 0x3F
      , StrategyDecisionIntentSourceNameId          = 64 // 0x40
      , StrategyDecisionIntentStrength              = 65 // 0x41
      , StrategyDecisionIntentAccuracyConfidence    = 66 // 0x42
      , StrategyDecisionIntentDirection             = 67 // 0x43
      , StrategyDecisionIntentTargetPrice           = 68 // 0x44
      , StrategyDecisionIntentTargetPriceBoundaryId = 69 // 0x45
      , StrategyDecisionEstimatedTimeToLive         = 70 // 0x46
      , StrategyDecisionKillPrice                   = 71 // 0x47
      , StrategyDecisionKillPriceBoundaryId         = 73 // 0x48
      , StrategyDecisionOutcome                     = 74 // 0x49
      , StrategyDecisionOutcomeAtDate               = 75 // 0x4A
      , StrategyDecisionOutcomeAtSub2MinTime        = 76 // 0x4B
      , StrategyDecisionOutcomeSuccessRating        = 77 // 0x4C

      , ExecutionSummaryPeriod                 = 90  // 0x5A
      , ExecutionPeriodStartDateTime           = 91  // 0x5B
      , ExecutionPeriodStartSub2MinTime        = 92  // 0x5C
      , ExecutionPeriodSlippageDelta           = 93  // 0x5D
      , ExecutionPeriodInternalOrderFillCount  = 94  // 0x5E
      , ExecutionPeriodSourceTradeCount        = 95  // 0x5F
      , ExecutionPeriodSourcePaidCount         = 96  // 0x60
      , ExecutionPeriodSourceGivenCount        = 97  // 0x61
      , ExecutionPeriodAdapterTradeCount       = 98  // 0x62
      , ExecutionPeriodInternalFillRatio       = 99  // 0x63
      , ExecutionPeriodInternalFillPriceDelta  = 100 // 0x64
      , ExecutionPeriodTargetToActualDelta     = 101 // 0x65
      , ExecutionPeriodPlacedConfirmTimeUs     = 102 // 0x66
      , ExecutionPeriodAvgAggressiveFillTimeMs = 103 // 0x67
      , ExecutionPeriodEffectivePriceDelta     = 104 // 0x68


      , LimitNameId                    = 111 // 0x6F
      , LimitType                      = 112 // 0x70
      , LimitCategory                  = 113 // 0x71
      , LimitPortfolioId               = 114 // 0x72
      , LimitStrategyId                = 115 // 0x73
      , LimitTraderId                  = 116 // 0x74
      , LimitAppliedToNameId           = 117 // 0x75
      , LimitLastUpdateDate            = 118 // 0x76
      , LimitLastUpdateSub2MinTime     = 119 // 0x77
      , LimitApplyPeriod               = 120 // 0x78
      , LimitCurrentValue              = 121 // 0x79
      , LimitBreachThreshold           = 122 // 0x7A
      , LimitAlertApproachingThreshold = 123 // 0x7B
      , LimitStatus                    = 124 // 0x7C


      , TradingAccountId                           = 130 // 0x82
      , TradingAccountNameId                       = 131 // 0x83
      , TradingAccountType                         = 132 // 0x84
      , TradingAccountTradingStatus                = 133 // 0x85
      , TradingAccountTickerNetOpenDirection       = 134 // 0x86
      , TradingAccountTickerNetOpenMarketAggregate = 135 // 0x87
      , TradingAccountTickerNetOpenPnl             = 136 // 0x88
      , TradingAccountTickerClosedDailyPnl         = 137 // 0x89

      , LimitBreachPublishId               = 144 // 0x90
      , LimitBreachType                    = 145 // 0x91
      , LimitBreachNameId                  = 146 // 0x92
      , LimitBreachState                   = 147 // 0x93
      , LimitBreachOutcome                 = 148 // 0x94
      , LimitBreachTradingState            = 149 // 0x95
      , LimitBreachExpectedRecoveryDate    = 150 // 0x96
      , LimitBreachExpectedRecoverySub2Min = 151 // 0x97
      , LimitBreachStateAsOfDate           = 152 // 0x98
      , LimitBreachStateAsOfSub2MinTime    = 153 // 0x99
      , LimitBreachCurrentValue            = 154 // 0x9A
      , LimitBreachAttemptedValue          = 155 // 0x9B
      , LimitBreachAttemptedOrderId        = 156 // 0x9C

      , OrderId                                = 160 // 0xA0
      , OrderSequenceId                        = 161 // 0xA1
      , OrderType                              = 162 // 0xA2
      , OrderFlags                             = 163 // 0xA3
      , OrderCreatedDate                       = 164 // 0xA4
      , OrderCreatedSub2MinTime                = 165 // 0xA5
      , OrderSubmitPrice                       = 166 // 0xA6
      , OrderUpdatedDate                       = 167 // 0xA7
      , OrderUpdatedSub2MinTime                = 168 // 0xA8
      , OrderTrackingId                        = 169 // 0xA9
      , OrderVolume                            = 170 // 0xAA
      , OrderRemainingVolume                   = 171 // 0xAB
      , OrderInternalTargetPrice               = 172 // 0xAC
      , OrderInternalParentOrderId             = 173 // 0xAD
      , OrderInternalClosingOrderId            = 174 // 0xAE
      , OrderInternalClosingOrderOpenPrice     = 175 // 0xAF
      , OrderInternalDecisionCreateDate        = 176 // 0xB0
      , OrderInternalDecisionCreateSub2MinTime = 177 // 0xB1
      , OrderInternalDecisionAmendDate         = 178 // 0xB2
      , OrderInternalDecisionAmendSub2MinTime  = 179 // 0xB3
      , OrderInternalDivisionId                = 180 // 0xB4
      , OrderInternalDivisionNameId            = 181 // 0xB5
      , OrderInternalDeskId                    = 182 // 0xB6
      , OrderInternalDeskNameId                = 183 // 0xB7
      , OrderInternalStrategyId                = 184 // 0xB8
      , OrderInternalStrategyNameId            = 185 // 0xB9
      , OrderInternalStrategyDecisionId        = 186 // 0xBA
      , OrderInternalStrategyDecisionNameId    = 187 // 0xBB
      , OrderInternalPortfolioId               = 188 // 0xBC
      , OrderInternalPortfolioNameId           = 189 // 0xBD
      , OrderInternalTraderId                  = 190 // 0xBE
      , OrderInternalTraderNameId              = 191 // 0xBF
      , OrderInternalMarginConsumed            = 192 // 0xC0
      , OrderExternalCounterPartyId            = 193 // 0xC1
      , OrderExternalCounterPartyNameId        = 194 // 0xC2
      , OrderExternalTraderId                  = 195 // 0xC3
      , OrderExternalTraderNameId              = 196 // 0xC4

      , LastTradedSummaryPeriod                            = 210 // 0xD2
      , LastTradedTradeId                                  = 211 // 0xD3
      , LastTradedOrderId                                  = 212 // 0xD4
      , LastTradedBatchId                                  = 213 // 0xD5
      , LastTradedActionCommand                            = 214 // 0xD6
      , LastTradedAtPrice                                  = 215 // 0xD7
      , LastTradedTradeTimeDate                            = 216 // 0xD8
      , LastTradedTradeSub2MinTime                         = 217 // 0xD9
      , LastTradedOrderVolume                              = 218 // 0xDA
      , LastTradedBooleanFlags                             = 219 // 0xDB
      , LastTradedInternalTargetPrice                      = 220 // 0xDC
      , LastTradedInternalSubmitPrice                      = 221 // 0xDD
      , LastTradedInternalTradeFillRefId                   = 222 // 0xDE
      , LastTradedInternalRemainingOrderVolume             = 223 // 0xDF
      , LastTradedInternalLinkedClosingOrderId             = 224 // 0xC0
      , LastTradedInternalDivisionId                       = 225 // 0xC1
      , LastTradedInternalDivisionNameId                   = 226 // 0xC2
      , LastTradedInternalDeskId                           = 227 // 0xC3
      , LastTradedInternalDeskNameId                       = 228 // 0xC4
      , LastTradedInternalStrategyId                       = 229 // 0xC5
      , LastTradedInternalStrategyNameId                   = 230 // 0xC6
      , LastTradedInternalPortfolioId                      = 231 // 0xC7
      , LastTradedInternalPortfolioNameId                  = 232 // 0xC8
      , LastTradedInternalTraderId                         = 233 // 0xC9
      , LastTradedInternalTraderNameId                     = 234 // 0xCA
      , LastTradedInternalNetOpenPositionDeltaPnl          = 235 // 0xCB
      , LastTradedInternalPortfolioNetOpenPositionDeltaPnl = 236 // 0xCC
      , LastTradedInternalStrategyNetOpenPositionDeltaPnl  = 237 // 0xCD
      , LastTradedInternalTraderNetOpenPositionDeltaPnl    = 238 // 0xCE
      , LastTradedInternalSimulatorNetOpenPositionDeltaPnl = 239 // 0xCF
      , LastTradedInternalTargetPricePnl                   = 240 // 0xD0
      , LastTradedInternalClosingOrderPnl                  = 241 // 0xD1
      , LastTradedInternalClosingOrderDeltaMarginReleased  = 242 // 0xD2
      , LastTradedInternalTotalDeltaMarginReleased         = 243 // 0xD3
      , LastTradedCounterPartyId                           = 244 // 0xD4
      , LastTradedCounterPartyNameId                       = 245 // 0xD5
      , LastTradedTraderId                                 = 246 // 0xD6
      , LastTradedTraderNameId                             = 247 // 0xD7
    }

    public static class PQSubFieldKeyExtensions
    {
        public static string ToSubIdString(this byte subId, PQFeedFields parentId)
        {
            return parentId.IsPricingSubKey() switch
                   {
                       true  => ((PQPricingSubFieldKeys)subId).ToString()
                     , false => ((PQTradingSubFieldKeys)subId).ToString()
                   };
        }

        public static bool IsPricingSubKey(this PQFeedFields parentId)
        {
            return parentId switch
                   {
                       _ when parentId <= PQFeedFields.MarketEventsUpcoming           => true
                     , _ when parentId <= PQFeedFields.PortfolioLimitApproachingAlert => false
                     , _ when parentId <= PQFeedFields.QuoteLayerOrdersCount          => true
                     , _ when parentId <= PQFeedFields.QuoteLayerOrders               => false
                     , _ when parentId <= PQFeedFields.QuoteLayersRangeEnd            => true
                     , _ when parentId <= PQFeedFields.QuoteExpiryDate                => false
                     , _                                                              => true
                   };
        }

        public static bool IsTradingSubKey(this PQFeedFields parentId)
        {
            return !parentId.IsPricingSubKey();
        }
    }
}
