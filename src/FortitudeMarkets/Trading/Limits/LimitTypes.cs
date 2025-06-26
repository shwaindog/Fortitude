using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Limits;

public enum LimitTypes : uint
{
    None                                           = 0
  , AccountAllOpenPositionLimit                    = 0x00_00_00_01
  , AccountTickerOpenPositionLimit                 = 0x00_00_00_02
  , AccountTickerRegionLimit                       = 0x00_00_00_04
  , AccountAssetCategoryOpenPositionLimit          = 0x00_00_00_08
  , AdapterAllOpenPositionLimit                    = 0x00_00_00_10
  , AdapterTickerOpenPositionLimit                 = 0x00_00_00_20
  , AdapterTickerOrderSizeLimit                    = 0x00_00_00_40
  , AdapterOrdersSubmitThroughputLimit             = 0x00_00_00_80
  , AdapterOrdersSubmitVolumeThroughputLimit       = 0x00_00_01_00
  , AdapterTickerOrdersSubmitThroughputLimit       = 0x00_00_02_00
  , AdapterTickerOrdersSubmitVolumeThroughputLimit = 0x00_00_04_00
  , SourceAllOpenPositionLimit                     = 0x00_00_08_00
  , SourceTickerOpenPositionLimit                  = 0x00_00_08_00
  , SourceMarginLimit                              = 0x00_00_08_00
}
