namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

[Flags]
public enum QuoteLayerInstantBehaviorFlags : byte
{
    None                                     = 0x00
  , PublishQuoteInstantBehaviorFlags         = 0x01
  , EnforceBookUniqueSourceOrderIdEntryCheck = 0x02
  , OnlyUpdateBookOrdersWithSameSource       = 0x04
  , InsertNewBookOrdersOnCreatedTimeOrder    = 0x08
  , DisableBookOrdersApplyListShifts         = 0x10
  , DisableBookOrdersCalculateShifts         = 0x20
  , DisableBookLayerApplyListShifts          = 0x40
  , DisableBookLayerCalculateShifts          = 0x80
  , DisableAllBookShifts                     = 0xF0
}

[Flags]
public enum QuoteInstantBehaviorFlags : ushort
{
    None = 0x00_00

    // Includes All QuoteLayerInstantBehaviorFlags
  , PublishQuoteInstantBehaviorFlags         = QuoteLayerInstantBehaviorFlags.PublishQuoteInstantBehaviorFlags
  , EnforceBookUniqueSourceOrderIdEntryCheck = QuoteLayerInstantBehaviorFlags.EnforceBookUniqueSourceOrderIdEntryCheck
  , OnlyUpdateBookOrdersWithSameSource       = QuoteLayerInstantBehaviorFlags.OnlyUpdateBookOrdersWithSameSource
  , InsertNewBookOrdersOnCreatedTimeOrder    = QuoteLayerInstantBehaviorFlags.InsertNewBookOrdersOnCreatedTimeOrder
  , DisableBookOrdersApplyListShifts         = QuoteLayerInstantBehaviorFlags.DisableBookOrdersApplyListShifts
  , DisableBookOrdersCalculateShifts         = QuoteLayerInstantBehaviorFlags.DisableBookOrdersCalculateShifts
  , DisableBookLayerApplyListShifts          = QuoteLayerInstantBehaviorFlags.DisableBookLayerApplyListShifts
  , DisableBookLayerCalculateShifts          = QuoteLayerInstantBehaviorFlags.DisableBookLayerCalculateShifts
  , DisableAllBookShifts                     = QuoteLayerInstantBehaviorFlags.DisableAllBookShifts
    // End of QuoteInstantBehaviorFlags

  , EnforceBookUniquePriceSourceLayerEntryCheck = 0x01_00
  , SquashBookMultiSourceLayers                 = 0x02_00
  , DisableUpgradeLayer                         = 0x04_00
  , NoBookMaxAllowedSizeUpdates                 = 0x08_00
  , IgnoreSideDateTimesCompare                  = 0x10_00
  , NoSideDateTimesUpdates                      = 0x30_00
  , IgnoreValidDateTimesCompare                 = 0x40_00
  , NoValidDateTimeUpdates                      = 0xC0_00
}

[Flags]
public enum PublishableQuoteInstantBehaviorFlags : uint
{
    None = 0x00_00_00_00

    // Includes All QuoteInstantBehaviorFlags
    // Includes All QuoteLayerInstantBehaviorFlags
  , PublishQuoteInstantBehaviorFlags         = QuoteLayerInstantBehaviorFlags.PublishQuoteInstantBehaviorFlags
  , EnforceBookUniqueSourceOrderIdEntryCheck = QuoteLayerInstantBehaviorFlags.EnforceBookUniqueSourceOrderIdEntryCheck
  , OnlyUpdateBookOrdersWithSameSource       = QuoteLayerInstantBehaviorFlags.OnlyUpdateBookOrdersWithSameSource
  , InsertNewBookOrdersOnCreatedTimeOrder    = QuoteLayerInstantBehaviorFlags.InsertNewBookOrdersOnCreatedTimeOrder
  , DisableBookOrdersApplyListShifts         = QuoteLayerInstantBehaviorFlags.DisableBookOrdersApplyListShifts
  , DisableBookOrdersCalculateShifts         = QuoteLayerInstantBehaviorFlags.DisableBookOrdersCalculateShifts
  , DisableBookLayerApplyListShifts          = QuoteLayerInstantBehaviorFlags.DisableBookLayerApplyListShifts
  , DisableBookLayerCalculateShifts          = QuoteLayerInstantBehaviorFlags.DisableBookLayerCalculateShifts
  , DisableAllBookShifts                     = QuoteLayerInstantBehaviorFlags.DisableAllBookShifts
    // End of QuoteInstantBehaviorFlags

  , EnforceBookUniquePriceSourceLayerEntryCheck = QuoteInstantBehaviorFlags.EnforceBookUniquePriceSourceLayerEntryCheck
  , SquashBookMultiSourceLayers                 = QuoteInstantBehaviorFlags.SquashBookMultiSourceLayers
  , DisableUpgradeLayer                         = QuoteInstantBehaviorFlags.DisableUpgradeLayer
  , NoBookMaxAllowedSizeUpdates                 = QuoteInstantBehaviorFlags.NoBookMaxAllowedSizeUpdates
  , IgnoreSideDateTimesCompare                  = QuoteInstantBehaviorFlags.IgnoreSideDateTimesCompare
  , NoSideDateTimesUpdates                      = QuoteInstantBehaviorFlags.NoSideDateTimesUpdates
  , IgnoreValidDateTimesCompare                 = QuoteInstantBehaviorFlags.IgnoreValidDateTimesCompare
  , NoValidDateTimeUpdates                      = QuoteInstantBehaviorFlags.NoValidDateTimeUpdates
    // End of QuoteInstantBehaviorFlags

  , IgnoreAdapterReceiveTimeCompare             = 0x00_01_00_00
  , IgnoreAdapterSentTimeCompare                = 0x00_02_00_00
  , NoAdapterReceiveTimeUpdates                 = 0x00_05_00_00
  , NoAdapterSentTimeUpdates                    = 0x00_0A_00_00
  , NoAdapterTimesUpdates                       = 0x00_0F_00_00
  , IgnoreClientTimesCompare                    = 0x00_10_00_00
  , NoClientInboundSocketTimeUpdates            = 0x00_30_00_00
  , NoClientProcessedTimeUpdates                = 0x00_70_00_00
  , NoClientReceiveTimeUpdates                  = 0x00_90_00_00
  , NoClientTimesUpdates                        = 0x00_F0_00_00
  , DefaultAdapterFlags                         = 0x00_F0_00_01
  , NoPublishableQuoteUpdates                   = 0x01_FF_00_00
  , JustChartableQuoteFieldUpdates              = 0x01_FF_F0_00
  , SuppressPublishOriginalQuoteFlags           = 0x02_00_00_00
  , RestoreOriginalQuoteFlags                   = 0x04_00_00_00
  , RestoreAndOverlayOriginalQuoteFlags         = 0x0C_00_00_00
  , PublishPublishableQuoteInstantBehaviorFlags = 0x10_00_00_00
  , InheritAdditionalPublishedFlags             = 0x20_00_00_00
  , DefaultClientFlags                          = 0x3C_00_00_01
}

public static class QuoteLayerInstantBehaviorFlagsExtensions
{
    public static bool HasPublishQuoteInstantBehaviorFlagsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.PublishQuoteInstantBehaviorFlags) > 0;

    public static bool HasEnforceBookUniqueSourceOrderIdEntryCheckFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.EnforceBookUniqueSourceOrderIdEntryCheck) > 0;

    public static bool HasOnlyUpdateBookOrdersWithSameSourceFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.OnlyUpdateBookOrdersWithSameSource) > 0;

    public static bool HasInsertNewBookOrdersOnCreatedTimeOrderFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.InsertNewBookOrdersOnCreatedTimeOrder) > 0;

    public static bool HasDisableBookOrdersApplyListShiftsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.DisableBookOrdersApplyListShifts) > 0;

    public static bool HasDisableBookOrdersCalculateShiftsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.DisableBookOrdersCalculateShifts) > 0;

    public static bool HasDisableBookLayerApplyListShiftsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.DisableBookLayerApplyListShifts) > 0;

    public static bool HasDisableBookLayerCalculateShiftsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.DisableBookLayerCalculateShifts) > 0;

    public static bool HasDisableAllBookShiftsFlag(this QuoteLayerInstantBehaviorFlags flags) =>
        (flags & QuoteLayerInstantBehaviorFlags.DisableAllBookShifts) > 0;
}

public static class QuoteInstantBehaviorFlagsExtensions
{
    public static bool HasPublishQuoteInstantBehaviorFlagsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.PublishQuoteInstantBehaviorFlags) > 0;

    public static bool HasEnforceBookUniqueSourceOrderIdEntryCheckFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.EnforceBookUniqueSourceOrderIdEntryCheck) > 0;

    public static bool HasOnlyUpdateBookOrdersWithSameSourceFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.OnlyUpdateBookOrdersWithSameSource) > 0;

    public static bool HasInsertNewBookOrdersOnCreatedTimeOrderFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.InsertNewBookOrdersOnCreatedTimeOrder) > 0;

    public static bool HasDisableBookOrdersApplyListShiftsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableBookOrdersApplyListShifts) > 0;

    public static bool HasDisableBookOrdersCalculateShiftsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableBookOrdersCalculateShifts) > 0;

    public static bool HasDisableBookLayerApplyListShiftsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableBookLayerApplyListShifts) > 0;

    public static bool HasDisableBookLayerCalculateShiftsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableBookLayerCalculateShifts) > 0;

    public static bool HasDisableAllBookShiftsFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableAllBookShifts) > 0;

    public static bool HasEnforceBookUniquePriceSourceLayerEntryCheckFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.EnforceBookUniquePriceSourceLayerEntryCheck) > 0;

    public static bool HasSquashBookMultiSourceLayersFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.SquashBookMultiSourceLayers) > 0;

    public static bool HasDisableUpgradeLayerFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.DisableUpgradeLayer) > 0;

    public static bool HasNoBookMaxAllowedSizeUpdatesFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.NoBookMaxAllowedSizeUpdates) > 0;

    public static bool HasIgnoreSideDateTimesCompareFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.IgnoreSideDateTimesCompare) > 0;

    public static bool HasNoSideDateTimesUpdatesFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.NoSideDateTimesUpdates) == QuoteInstantBehaviorFlags.NoSideDateTimesUpdates;

    public static bool HasIgnoreValidDateTimesCompareFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.IgnoreValidDateTimesCompare) > 0;

    public static bool HasNoValidDateTimeUpdatesFlag(this QuoteInstantBehaviorFlags flags) =>
        (flags & QuoteInstantBehaviorFlags.NoValidDateTimeUpdates) == QuoteInstantBehaviorFlags.NoValidDateTimeUpdates;
}

public static class PublishableQuoteInstantBehaviorFlagsExtensions
{
    public static PublishableQuoteInstantBehaviorFlags ToQuoteInstantBehaviorMask(this PublishableQuoteInstantBehaviorFlags flags) =>
        (PublishableQuoteInstantBehaviorFlags)((uint)flags & 0xFF_FF);

    public static PublishableQuoteInstantBehaviorFlags JustPublishableBehaviorMask(this PublishableQuoteInstantBehaviorFlags flags) =>
        (PublishableQuoteInstantBehaviorFlags)((uint)flags & 0xFF_FF_00_00);

    public static bool HasPublishQuoteInstantBehaviorFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.PublishQuoteInstantBehaviorFlags) > 0;

    public static bool HasEnforceBookUniqueSourceOrderIdEntryCheckFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.EnforceBookUniqueSourceOrderIdEntryCheck) > 0;

    public static bool HasOnlyUpdateBookOrdersWithSameSourceFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.OnlyUpdateBookOrdersWithSameSource) > 0;

    public static bool HasInsertNewBookOrdersOnCreatedTimeOrderFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.InsertNewBookOrdersOnCreatedTimeOrder) > 0;

    public static bool HasDisableBookOrdersApplyListShiftsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableBookOrdersApplyListShifts) > 0;

    public static bool HasDisableBookOrdersCalculateShiftsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableBookOrdersCalculateShifts) > 0;

    public static bool HasDisableBookLayerApplyListShiftsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableBookLayerApplyListShifts) > 0;

    public static bool HasDisableBookLayerCalculateShiftsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableBookLayerCalculateShifts) > 0;

    public static bool HasDisableAllBookShiftsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableAllBookShifts) > 0;

    public static bool HasEnforceBookUniquePriceSourceLayerEntryCheckFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.EnforceBookUniquePriceSourceLayerEntryCheck) > 0;

    public static bool HasNoBookMaxAllowedSizeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoBookMaxAllowedSizeUpdates) > 0;

    public static bool HasDisableUpgradeLayerFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DisableUpgradeLayer) > 0;

    public static bool HasSquashBookMultiSourceLayersFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.SquashBookMultiSourceLayers) > 0;

    public static bool HasIgnoreSideDateTimesCompareFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.IgnoreSideDateTimesCompare) > 0;

    public static bool HasNoSideDateTimesUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoSideDateTimesUpdates) == PublishableQuoteInstantBehaviorFlags.NoSideDateTimesUpdates;

    public static bool HasIgnoreValidDateTimesCompareFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.IgnoreValidDateTimesCompare) > 0;

    public static bool HasNoValidDateTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoValidDateTimeUpdates) == PublishableQuoteInstantBehaviorFlags.NoValidDateTimeUpdates;

    public static bool HasIgnoreAdapterReceiveTimeCompareFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.IgnoreAdapterReceiveTimeCompare) > 0;

    public static bool HasIgnoreAdapterSentTimeCompareFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.IgnoreAdapterSentTimeCompare) > 0;

    public static bool HasNoAdapterReceiveTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoAdapterReceiveTimeUpdates) ==
        PublishableQuoteInstantBehaviorFlags.NoAdapterReceiveTimeUpdates;

    public static bool HasNoAdapterSentTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoAdapterSentTimeUpdates) == PublishableQuoteInstantBehaviorFlags.NoAdapterSentTimeUpdates;

    public static bool HasNoAdapterTimesUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoAdapterTimesUpdates) == PublishableQuoteInstantBehaviorFlags.NoAdapterTimesUpdates;

    public static bool HasIgnoreClientTimesCompareFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.IgnoreClientTimesCompare) > 0;

    public static bool HasNoClientInboundSocketTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoClientInboundSocketTimeUpdates) ==
        PublishableQuoteInstantBehaviorFlags.NoClientInboundSocketTimeUpdates;

    public static bool HasNoClientProcessedTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoClientProcessedTimeUpdates) ==
        PublishableQuoteInstantBehaviorFlags.NoClientProcessedTimeUpdates;

    public static bool HasNoClientReceiveTimeUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoClientReceiveTimeUpdates) == PublishableQuoteInstantBehaviorFlags.NoClientReceiveTimeUpdates;

    public static bool HasNoClientTimesUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoClientTimesUpdates) == PublishableQuoteInstantBehaviorFlags.NoClientTimesUpdates;

    public static bool HasDefaultAdapterFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DefaultAdapterFlags) == PublishableQuoteInstantBehaviorFlags.DefaultAdapterFlags;

    public static bool HasNoPublishableQuoteUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.NoPublishableQuoteUpdates) == PublishableQuoteInstantBehaviorFlags.NoPublishableQuoteUpdates;

    public static bool HasJustChartableQuoteFieldUpdatesFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.JustChartableQuoteFieldUpdates) ==
        PublishableQuoteInstantBehaviorFlags.JustChartableQuoteFieldUpdates;

    public static bool HasSuppressPublishOriginalQuoteFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.SuppressPublishOriginalQuoteFlags) > 0;

    public static bool HasRestoreOriginalQuoteFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.RestoreOriginalQuoteFlags) > 0;

    public static bool HasRestoreAndOverlayOriginalQuoteFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.RestoreAndOverlayOriginalQuoteFlags) ==
        PublishableQuoteInstantBehaviorFlags.RestoreAndOverlayOriginalQuoteFlags;

    public static bool HasPublishPublishableQuoteInstantBehaviorFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.PublishPublishableQuoteInstantBehaviorFlags) > 0;

    public static bool HasInheritAdditionalPublishedFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.InheritAdditionalPublishedFlags) > 0;

    public static bool HasDefaultClientFlagsFlag(this PublishableQuoteInstantBehaviorFlags flags) =>
        (flags & PublishableQuoteInstantBehaviorFlags.DefaultClientFlags) == PublishableQuoteInstantBehaviorFlags.DefaultClientFlags;
}
