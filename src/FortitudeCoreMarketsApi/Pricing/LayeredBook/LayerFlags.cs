using System;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    [Flags]
    public enum LayerFlags : uint
    {
        None = 0x00,
        Price = 0x01,
        Volume = 0x02,
        SourceQuoteReference = 0x04,
        SourceName = 0x08,
        TraderCount = 0x10,
        TraderName = 0x30,
        TraderSize = 0x50,
        ValueDate = 0x80,
        Executable = 0x1_00,
        Reserved = 0x0E_00,
        UserExtensibile = 0xF0_00
    }
}