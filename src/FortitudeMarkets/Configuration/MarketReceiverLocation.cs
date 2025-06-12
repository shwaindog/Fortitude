using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Configuration;

namespace FortitudeMarkets.Configuration
{
    internal class MarketReceiverLocation { }
}


public enum MarketExchange
{
    None
  , ASX                // XASX  Australian - Sydney
  , NYSE               // XNYS - US - New York
  , Nasdaq             // XNAS - US - New York
  , LondonSE           // XLON - UK - London
  , SIXSE              // XSWX - Zurich - Switzerland
  , ShanghaiSE         // XSHG - China - Shanghai
  , ShenzhenSE         // XSHE - China - Shenzhen
  , HongKongSE         // XHKG - China - Hong Kong 
  , TaipeiSE           // XTAI - China - Taiwan - Taipei
  , TokyoSE            // XJPX (TYO) Japan - Tokyo
  , NewZealandSE       // NZSX - New Zealand - Wellington
  , TorontoSE          // XTSE - Canada - Toronto
  , IndiaNationalSE    // XNSE - Mumbai - India
  , BombaySE           // XBOM - Mumbai - India
  , EuronextOsloSE     //  XOSL - Norway - Oslo, 
  , EuronextBrusselsSE //  XBRU - Belgium - Brussels, 
  , EuronextAmsterdam  // XAMS - Netherlands - Amsterdam
  , EuronextDublinSE   //  XMSM - Ireland - Dublin, 
  , EuronextLisbonSE   //  XLIS - Portugal -Lisbon,
  , EuronextParisSE    //  XPAR - France - Paris
  , FrankfurtSE        // XFRA - Germany - Frankfurt
  , EuronextMilanSE    //  XMIL - Italy - Milan, 
  , SaudiArabiaSaudiSE // XSAU - Riyadh - Saudi Arabia
  , IranTehranSE       // XTEH - Tehran - Iran
  , ReutersLondon
  , ReutersNewYork
  , EBSLondon
  , EBSNewYork
  , Currenex
  , Hotspot
}


public enum LocationDataCenter
{
    None
  , US_NY_Equinix_NY1 // US - New York (Jersey) - 
  , US_NY_Equinix_NY2
  , US_NY_Equinix_NY3
  , US_NY_Equinix_NY4
  , US_NY_Equinix_NY5
  , US_NY_Equinix_NY6
  , US_NY_Equinix_NY7
  , US_NY_Equinix_NY8
  , US_NY_Equinix_NY9
  , US_NY_Equinix_NY10
  , US_NY_Equinix_NY11
  , US_NY_Equinix_NY12
  , US_NY_Equinix_NY13
  , US_CH_Equinix_CH1 // US - Chicago
  , US_CH_Equinix_CH2
  , US_CH_Equinix_CH3
  , US_CH_Equinix_CH4
  , US_CH_Equinix_CH5
  , US_CH_Equinix_CH6
  , US_CH_Equinix_CH7
  , AU_MEL_NextDC_M1
  , AU_MEL_NextDC_M2
  , AU_MEL_NextDC_M3
  , AU_MEL_NextDC_M4  // in planning
  , AU_GEL_NextDC_GE1 // in planning Geelong
}


public enum Broker
{
    None
  , CMCMarkets
  , InteractiveBrokers
  , IGGroup
  , ComSec
  , StandardCharter
    // , DeutcheBank
    // , Barclays
    // , HSBC
    // , SocGen
    // , BNPParibas
    // , MorganStanley
    // , Citibank
    // , BankOfAmerica
    // , GoldmanSachs
    // , Macquarie
}


public interface IPublisherLocation
{
    MarketExchange PrimaryPricingExchange { get; set; }
    MarketExchange IndexPublishExchange   { get; set; }
}

public interface IBrokerLocation
{
    MarketExchange PrimaryPricingExchange { get; set; }
    Broker         Broker                 { get; set; }
}


public struct MarketPublishAndReceiverLocation { }
