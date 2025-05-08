// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

[Flags]
public enum SourceTickerInfoBooleanFlags : uint
{
    None                 = 0
  , SubscribeToPricesSet = 1
  , TradingEnabledSet    = 2

  , SubscribeToPricesUpdated = 0x01_00_00
  , TradingEnabledUpdated    = 0x02_00_00

  , IsUpdatedMask = 0xFF_00_00
}

public static class SourceTickerInfoBooleanFlagsExtensions
{
    public static bool HasSubscribeToPricesFlag
        (this SourceTickerInfoBooleanFlags flags) =>
        (flags & SourceTickerInfoBooleanFlags.SubscribeToPricesSet) > 0;

    public static bool HasTradingEnabledFlag
        (this SourceTickerInfoBooleanFlags flags) =>
        (flags & SourceTickerInfoBooleanFlags.TradingEnabledSet) > 0;

    public static SourceTickerInfoBooleanFlags GetSourceTickerInfoBooleanFlagsEnum
        (this ISourceTickerInfo sourceTickerInfo) =>
        (sourceTickerInfo.SubscribeToPrices ? SourceTickerInfoBooleanFlags.SubscribeToPricesSet : SourceTickerInfoBooleanFlags.None)
      | (sourceTickerInfo.TradingEnabled ? SourceTickerInfoBooleanFlags.TradingEnabledSet : SourceTickerInfoBooleanFlags.None);
}
