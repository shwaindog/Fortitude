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

      , PnLConversionReportedCurrency               = 45 // 0x2D
      , PnLConversionVolumeConversionConversionRate = 46 // 0x2E
      , PnLConversionPriceConversionRate            = 47 // 0x2F

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

      , CandlePeriod           = 90  // 0x5A
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


    public enum PQOrdersSubFieldKeys : byte
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


      , OrderId                                = 10 // 0x0A
      , OrderType                              = 11 // 0x0B
      , OrderGenesisFlags                      = 12 // 0x0C
      , OrderLifecycleStateFlags               = 13 // 0x0D
      , OrderUpdateReasonFlags                 = 15 // 0x1F
      , OrderCreatedDate                       = 16 // 0x10
      , OrderCreatedSub2MinTime                = 17 // 0x11
      , OrderUpdatedDate                       = 18 // 0x12
      , OrderUpdatedSub2MinTime                = 19 // 0x13
      , OrderTrackingId                        = 20 // 0x14
      , OrderDisplayVolume                     = 21 // 0x15 // Negative sell, positive buy
      , OrderRemainingVolume                   = 22 // 0x16
      , OrderInternalTotalVolume               = 23 // 0x17
      , OrderInternalSequenceId                = 24 // 0x18
      , OrderInternalSourceStateFlags          = 25 // 0x19
      , OrderInternalPositionAdjustmentFlags   = 26 // 0x1B
      , OrderInternalTimeInForce               = 27 // 0x1A
      , OrderInternalClientOrderId             = 28 // 0x1C
      , OrderInternalSubmittedPrice            = 29 // 0x1D
      , OrderInternalSubmittedTakeProfitPrice  = 30 // 0x1E
      , OrderInternalSubmittedStopLossPrice    = 31 // 0x1F
      , OrderInternalTargetPrice               = 32 // 0x20
      , OrderInternalOnCreateTargetTakeProfit  = 33 // 0x21
      , OrderInternalOnCreateTargetStopLoss    = 34 // 0x22
      , OrderInternalTakeProfitOrderId         = 35 // 0x23
      , OrderInternalStopLossOrderId           = 36 // 0x24
      , OrderInternalCurrentTargetTakeProfit   = 37 // 0x25
      , OrderInternalCurrentTargetStopLoss     = 38 // 0x26
      , OrderInternalParentOrderId             = 39 // 0x27
      , OrderInternalClosingOrderId            = 40 // 0x28
      , OrderInternalClosingOrderOpenPrice     = 41 // 0x29
      , OrderInternalDecisionCreateDate        = 42 // 0x2A
      , OrderInternalDecisionCreateSub2MinTime = 43 // 0x2B
      , OrderInternalDecisionAmendDate         = 44 // 0x2C
      , OrderInternalDecisionAmendSub2MinTime  = 45 // 0x2D
      , OrderInternalDivisionId                = 46 // 0x2E
      , OrderInternalDivisionNameId            = 47 // 0x2F
      , OrderInternalDeskId                    = 48 // 0x30
      , OrderInternalDeskNameId                = 49 // 0x31
      , OrderInternalStrategyId                = 50 // 0x32
      , OrderInternalStrategyNameId            = 51 // 0x33
      , OrderInternalStrategyDecisionId        = 52 // 0x34
      , OrderInternalStrategyDecisionNameId    = 53 // 0x35
      , OrderInternalPortfolioId               = 54 // 0x36
      , OrderInternalPortfolioNameId           = 55 // 0x37
      , OrderInternalTraderId                  = 56 // 0x38
      , OrderInternalTraderNameId              = 57 // 0x39
      , OrderInternalMarginConsumed            = 58 // 0x3A
      , OrderExternalCounterPartyId            = 59 // 0x3B
      , OrderExternalCounterPartyNameId        = 60 // 0x3C
      , OrderExternalTraderId                  = 61 // 0x3D
      , OrderExternalTraderNameId              = 62 // 0x3E
    }

    public enum PQDecisionsSubFieldKeys : byte
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

      , StrategyDecisionId                          = 130 // 0x32
      , StrategyId                                  = 131 // 0x33
      , StrategyNameId                              = 132 // 0x34
      , StrategyDecisionIntentType                  = 133 // 0x35
      , StrategyDecisionIntentNameId                = 134 // 0x36
      , StrategyDecisionFinalType                   = 135 // 0x37
      , StrategyDecisionFinalNameId                 = 136 // 0x38
      , StrategyDecisionFinalReasonTextId           = 137 // 0x39
      , StrategyStateType                           = 138 // 0x3A
      , StrategyStateDescriptionTextId              = 139 // 0x3B
      , StrategyDecisionDate                        = 140 // 0x3C
      , StrategyDecisionSub2MinTime                 = 141 // 0x3D
      , StrategyDecisionOrderId                     = 142 // 0x3E
      , StrategyDecisionIntentSignalValue           = 143 // 0x3F
      , StrategyDecisionIntentSignalThreshold       = 144 // 0x40
      , StrategyDecisionIntentSourceNameId          = 145 // 0x41
      , StrategyDecisionIntentStrength              = 146 // 0x42
      , StrategyDecisionIntentAccuracyConfidence    = 147 // 0x43
      , StrategyDecisionIntentDirection             = 148 // 0x44
      , StrategyDecisionIntentTargetPrice           = 149 // 0x45
      , StrategyDecisionIntentTargetPriceBoundaryId = 140 // 0x46
      , StrategyDecisionEstimatedTimeToLive         = 151 // 0x47
      , StrategyDecisionKillPrice                   = 153 // 0x48
      , StrategyDecisionKillPriceBoundaryId         = 154 // 0x49
      , StrategyDecisionOutcome                     = 155 // 0x4A
      , StrategyDecisionOutcomeAtDate               = 156 // 0x4B
      , StrategyDecisionOutcomeAtSub2MinTime        = 157 // 0x4C
      , StrategyDecisionOutcomeSuccessRating        = 158 // 0x4D
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

      , ExecutionSummaryPeriod                 = 10 // 0x0A
      , ExecutionPeriodStartDateTime           = 11 // 0x0B
      , ExecutionPeriodStartSub2MinTime        = 12 // 0x0C
      , ExecutionPeriodSlippageDelta           = 13 // 0x0D
      , ExecutionPeriodInternalOrderFillCount  = 14 // 0x0E
      , ExecutionPeriodSourceTradeCount        = 15 // 0x0F
      , ExecutionPeriodSourcePaidCount         = 16 // 0x10
      , ExecutionPeriodSourceGivenCount        = 17 // 0x11
      , ExecutionPeriodAdapterTradeCount       = 18 // 0x12
      , ExecutionPeriodInternalFillRatio       = 19 // 0x13
      , ExecutionPeriodInternalFillPriceDelta  = 20 // 0x14
      , ExecutionPeriodTargetToActualDelta     = 21 // 0x15
      , ExecutionPeriodPlacedConfirmTimeUs     = 22 // 0x16
      , ExecutionPeriodAvgAggressiveFillTimeMs = 23 // 0x17
      , ExecutionPeriodEffectivePriceDelta     = 24 // 0x18


      , LimitNameId                    = 40 // 0x28
      , LimitType                      = 41 // 0x29
      , LimitCategory                  = 42 // 0x2A
      , LimitPortfolioId               = 43 // 0x2B
      , LimitStrategyId                = 44 // 0x2C
      , LimitTraderId                  = 45 // 0x2D
      , LimitAppliedToNameId           = 46 // 0x2E
      , LimitLastUpdateDate            = 47 // 0x2F
      , LimitLastUpdateSub2MinTime     = 48 // 0x30
      , LimitApplyPeriod               = 49 // 0x31
      , LimitCurrentValue              = 50 // 0x32
      , LimitBreachThreshold           = 51 // 0x33
      , LimitAlertApproachingThreshold = 52 // 0x34
      , LimitStatus                    = 53 // 0x35


      , TradingAccountId                                   = 70 // 0x46
      , TradingAccountNameId                               = 71 // 0x47
      , TradingAccountType                                 = 72 // 0x48
      , TradingAccountSubType                              = 73 // 0x49
      , TradingAccountPeriod                               = 74 // 0x4A
      , TradingAccountPeriodStartDate                      = 75 // 0x4B  // Negative Positive indicates Short or Long respectively
      , TradingAccountTradingStatus                        = 76 // 0x4C  // Negative Positive indicates Short or Long respectively
      , TradingAccountTickerNetOpenPosition                = 77 // 0x4D
      , TradingAccountTickerThisPeriodNewDeltaOpenPosition = 78 // 0x4E
      , TradingAccountTickerNetOpenPnl                     = 79 // 0x4F
      , TradingAccountTickerThisPeriodNewDeltaNetOpenPnl   = 80 // 0x50
      , TradingAccountTickerRealisedPnl                    = 81 // 0x51

      , LimitBreachPublishId               = 100 // 0x64
      , LimitBreachType                    = 101 // 0x65
      , LimitBreachNameId                  = 102 // 0x66
      , LimitBreachState                   = 103 // 0x67
      , LimitBreachOutcome                 = 104 // 0x68
      , LimitBreachTradingState            = 105 // 0x69
      , LimitBreachExpectedRecoveryDate    = 106 // 0x6A
      , LimitBreachExpectedRecoverySub2Min = 107 // 0x6B
      , LimitBreachStateAsOfDate           = 108 // 0x6C
      , LimitBreachStateAsOfSub2MinTime    = 109 // 0x6D
      , LimitBreachCurrentValue            = 110 // 0x6E
      , LimitBreachAttemptedValue          = 111 // 0x6F
      , LimitBreachAttemptedOrderId        = 112 // 0x70

      , LastTradedSummaryPeriod                            = 140 // 0x8C
      , LastTradedPeriodUpdateDate                         = 141 // 0x8D
      , LastTradedPeriodUpdateSub2MinTime                  = 142 // 0x8E
      , LastTradedTradeId                                  = 143 // 0x8F
      , LastTradedBatchId                                  = 144 // 0x90
      , LastTradedTypeFlags                                = 145 // 0x91
      , LastTradedLifeCycleStatus                          = 146 // 0x92
      , LastTradedFirstNotifiedDate                        = 147 // 0x93
      , LastTradedFirstNotifiedSub2MinTime                 = 148 // 0x94
      , LastTradedAdapterReceivedDate                      = 149 // 0x95
      , LastTradedAdapterReceivedSub2MinTime               = 150 // 0x96
      , LastTradedUpdateDate                               = 151 // 0x97
      , LastTradedUpdateSub2MinTime                        = 152 // 0x98
      , LastTradedAtPrice                                  = 153 // 0x99
      , LastTradedTradeTimeDate                            = 154 // 0x9A
      , LastTradedTradeSub2MinTime                         = 155 // 0x9B
      , LastTradedOrderId                                  = 156 // 0x9C
      , LastTradedTradeVolume                              = 157 // 0x9D
      , LastTradedBooleanFlags                             = 158 // 0x9E
      , LastTradedInternalTargetPrice                      = 159 // 0x9F
      , LastTradedInternalSubmitPrice                      = 160 // 0xA0
      , LastTradedInternalOrderVolume                      = 161 // 0xA1
      , LastTradedInternalOrderRemainingVolume             = 162 // 0xA2
      , LastTradedInternalTradeFillRefId                   = 163 // 0xA3
      , LastTradedInternalLinkedClosingOrderId             = 164 // 0xA4
      , LastTradedInternalDivisionId                       = 165 // 0xA5
      , LastTradedInternalDivisionNameId                   = 166 // 0xA6
      , LastTradedInternalDeskId                           = 167 // 0xA7
      , LastTradedInternalDeskNameId                       = 168 // 0xA8
      , LastTradedInternalStrategyId                       = 169 // 0xA9
      , LastTradedInternalStrategyNameId                   = 170 // 0xAA
      , LastTradedInternalPortfolioId                      = 171 // 0xAB
      , LastTradedInternalPortfolioNameId                  = 172 // 0xAC
      , LastTradedInternalTraderId                         = 173 // 0xAD
      , LastTradedInternalTraderNameId                     = 174 // 0xAE
      , LastTradedInternalNetOpenPositionDeltaPnl          = 175 // 0xAF
      , LastTradedInternalPortfolioNetOpenPositionDeltaPnl = 176 // 0xB0
      , LastTradedInternalStrategyNetOpenPositionDeltaPnl  = 177 // 0xB1
      , LastTradedInternalTraderNetOpenPositionDeltaPnl    = 178 // 0xB2
      , LastTradedInternalSimulatorNetOpenPositionDeltaPnl = 179 // 0xB3
      , LastTradedInternalTargetPricePnl                   = 180 // 0xB4
      , LastTradedInternalClosingOrderPnl                  = 181 // 0xB5
      , LastTradedInternalClosingOrderDeltaMarginReleased  = 182 // 0xB6
      , LastTradedInternalTotalDeltaMarginReleased         = 183 // 0xB7
      , LastTradedExternalCounterPartyId                   = 184 // 0xB8
      , LastTradedExternalCounterPartyNameId               = 185 // 0xB9
      , LastTradedExternalTraderId                         = 186 // 0xBA
      , LastTradedExternalTraderNameId                     = 187 // 0xBB
    }

    public enum SubKeyType
    {
        PricingSubKey   = 0
      , DecisionsSubKey = 1
      , OrdersSubKey    = 2
      , TradingSubKey   = 3
    }

    public static class PQSubFieldKeyExtensions
    {
        public static string ToSubIdString(this byte subId, PQFeedFields parentId)
        {
            return parentId.GetSubKeyType() switch
                   {
                       SubKeyType.PricingSubKey   => ((PQPricingSubFieldKeys)subId).ToString()
                     , SubKeyType.DecisionsSubKey => ((PQDecisionsSubFieldKeys)subId).ToString()
                     , SubKeyType.OrdersSubKey    => ((PQOrdersSubFieldKeys)subId).ToString()
                     , SubKeyType.TradingSubKey   => ((PQTradingSubFieldKeys)subId).ToString()
                     , _ => throw new ArgumentException($"Unexpected subKey type {parentId.GetSubKeyType()}")
                   };
        }

        public static SubKeyType GetSubKeyType(this PQFeedFields parentId)
        {
            return parentId switch
                   {
                       _ when parentId <= PQFeedFields.MarketEventsUpcoming                 => SubKeyType.PricingSubKey
                     , _ when parentId <= PQFeedFields.StrategyDecisionsWithActivePositions => SubKeyType.DecisionsSubKey
                     , _ when parentId <= PQFeedFields.PortfolioLimitApproachingAlert       => SubKeyType.TradingSubKey
                     , _ when parentId <= PQFeedFields.QuoteLayerOrdersCount                => SubKeyType.PricingSubKey
                     , _ when parentId <= PQFeedFields.QuoteLayerOrders                     => SubKeyType.OrdersSubKey
                     , _ when parentId <= PQFeedFields.QuoteLayersRangeEnd                  => SubKeyType.PricingSubKey
                     , _ when parentId <= PQFeedFields.LastTradedMarketDailyAggregates      => SubKeyType.TradingSubKey
                     , _ when parentId <= PQFeedFields.AdapterInternalOrdersSimulatorOpen   => SubKeyType.OrdersSubKey
                     , _ when parentId <= PQFeedFields.QuoteExpiryDate                      => SubKeyType.PricingSubKey
                     , _                                                                    => SubKeyType.PricingSubKey
                   };
        }

        public static bool IsPricingSubKey(this PQFeedFields parentId)
        {
            return parentId.GetSubKeyType() == SubKeyType.PricingSubKey;
        }

        public static bool IsOrdersSubKey(this PQFeedFields parentId)
        {
            return parentId.GetSubKeyType() == SubKeyType.OrdersSubKey;
        }

        public static bool IsDecisionSubKey(this PQFeedFields parentId)
        {
            return parentId.GetSubKeyType() == SubKeyType.DecisionsSubKey;
        }

        public static bool IsTradingSubKey(this PQFeedFields parentId)
        {
            return parentId.GetSubKeyType() == SubKeyType.TradingSubKey;
        }
    }
}
