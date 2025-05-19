// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public enum OrderPositionAlterationFlags : uint
{
    None                          = 0x00_00_00_00
  , Long                          = 0x00_00_00_40
  , Short                         = 0x00_00_00_80
  , PositionIncreasing            = 0x00_00_00_10
  , PositionReducing              = 0x00_00_00_20
  , IsEntry                       = 0x00_00_40_00
  , IsExit                        = 0x00_00_80_00
  , Partial                       = 0x00_00_00_02
  , TakeProfit                    = 0x00_00_00_01
  , StopLoss                      = 0x00_00_00_08
  , ExpectedShortTermPosition     = 0x00_00_01_00
  , ExpectedMediumTermPosition    = 0x00_00_02_00
  , ExpectedLongTermPosition      = 0x00_00_04_00
  , ExpectedBreakout              = 0x00_00_08_00
  , ExpectedRangeReversion        = 0x00_00_10_00
  , ExpectedRangeTargetDivergence = 0x00_00_20_00
  , IsPartialFillCompleting       = 0x00_00_20_00

  , ParentPartial  = 0x01_00_00_00
  , ParentComplete = 0x02_00_00_00
  , ParentOpening  = 0x04_00_00_00
  , ParentClosing  = 0x08_00_00_00
  , SlippageEntry  = 0x10_00_00_00
  , SlippageExit   = 0x20_00_00_00
  , SlippageEarly  = 0x40_00_00_00
  , SlippageLate   = 0x80_00_00_00
}
