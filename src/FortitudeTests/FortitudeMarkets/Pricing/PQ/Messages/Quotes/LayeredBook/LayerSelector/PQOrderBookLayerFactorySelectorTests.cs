﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQOrderBookLayerFactorySelectorTests
{
    private const decimal ExpectedPrice    = 2.3456m;
    private const decimal ExpectedVolume   = 42_000_100m;
    private const uint    ExpectedQuoteRef = 41_111_2222u;

    private static IPQNameIdLookupGenerator nameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);

    private readonly LayerType  pqPriceVolumeLayerType      = new PQPriceVolumeLayer().LayerType;
    private readonly LayerFlags pqPriceVolumeSupportedFlags = new PQPriceVolumeLayer().SupportsLayerFlags;

    private readonly LayerType  pqSourcePriceVolumeLayerType              = new PQSourcePriceVolumeLayer(nameIdGenerator).LayerType;
    private readonly LayerFlags pqSourcePriceVolumeSupportedFlags         = new PQSourcePriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private readonly LayerType  pqSourceQuoteRefPriceVolumeLayerType      = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator).LayerType;
    private readonly LayerFlags pqSourceQuoteRefPriceVolumeSupportedFlags = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;

    private readonly LayerType  pqTraderPriceVolumeLayerType            = new PQTraderPriceVolumeLayer(nameIdGenerator).LayerType;
    private readonly LayerFlags pqTraderPriceVolumeSupportedFlags       = new PQTraderPriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private readonly LayerType  pqValueDatePriceVolumeLayerType         = new PQValueDatePriceVolumeLayer().LayerType;
    private readonly LayerFlags pqValueDatePriceVolumeSupportedFlags    = new PQValueDatePriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  priceVolumeLayerType                    = new PriceVolumeLayer().LayerType;
    private readonly LayerFlags priceVolumeSupportedFlags               = new PriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  sourcePriceVolumeLayerType              = new SourcePriceVolumeLayer().LayerType;
    private readonly LayerFlags sourcePriceVolumeSupportedFlags         = new SourcePriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  sourceQuoteRefPriceVolumeLayerType      = new SourceQuoteRefPriceVolumeLayer().LayerType;
    private readonly LayerFlags sourceQuoteRefPriceVolumeSupportedFlags = new SourceQuoteRefPriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  traderPriceVolumeLayerType              = new TraderPriceVolumeLayer().LayerType;
    private readonly LayerFlags traderPriceVolumeSupportedFlags         = new TraderPriceVolumeLayer().SupportsLayerFlags;
    private readonly LayerType  valueDatePriceVolumeLayerType           = new ValueDatePriceVolumeLayer().LayerType;
    private readonly LayerFlags valueDatePriceVolumeSupportedFlags      = new ValueDatePriceVolumeLayer().SupportsLayerFlags;

    private string   expectedSourceName = null!;
    private string   expectedTraderName = null!;
    private DateTime expectedValueDate;

    private IPQSourceTickerInfo ipqSourceTickerInfo = new PQSourceTickerInfo();

    private PQOrderBookLayerFactorySelector layerSelector = null!;

    private PQPriceVolumeLayer               pqPriceVolumeLayer               = null!;
    private PQSourcePriceVolumeLayer         pqSourcePriceVolumeLayer         = null!;
    private PQSourceQuoteRefPriceVolumeLayer pqSourceQuoteRefPriceVolumeLayer = null!;

    private PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSrcQtRefTrdrVlDtPvl = null!;

    private LayerType pqSrcQtRefTrdrVlDtPvlType =
        new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdGenerator).LayerType;

    private LayerFlags pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags =
        new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdGenerator).SupportsLayerFlags;
    private PQTraderPriceVolumeLayer    pqTraderPriceVolumeLayer    = null!;
    private PQValueDatePriceVolumeLayer pqValueDatePriceVolumeLayer = null!;


    private PriceVolumeLayer               priceVolumeLayer               = null!;
    private SourcePriceVolumeLayer         sourcePriceVolumeLayer         = null!;
    private SourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer = null!;

    private SourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl = null!;

    private LayerType srcQtRefTrdrVlDtPvlType =
        new SourceQuoteRefTraderValueDatePriceVolumeLayer().LayerType;

    private LayerFlags srcQtRefTrdrVlDtPvlTypeSupportedFlags =
        new SourceQuoteRefTraderValueDatePriceVolumeLayer().SupportsLayerFlags;
    private TraderPriceVolumeLayer    traderPriceVolumeLayer    = null!;
    private ValueDatePriceVolumeLayer valueDatePriceVolumeLayer = null!;


    [TestInitialize]
    public void SetUp()
    {
        nameIdGenerator    = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        layerSelector      = new PQOrderBookLayerFactorySelector(nameIdGenerator);
        expectedTraderName = "TraderName-Leila";
        expectedSourceName = "SourceName-Wattle";
        expectedValueDate  = new DateTime(2018, 01, 9, 22, 0, 0);
        priceVolumeLayer   = new PriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        sourcePriceVolumeLayer = new SourcePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                            expectedSourceName, true);
        sourceQuoteRefPriceVolumeLayer = new SourceQuoteRefPriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                            expectedSourceName, true, ExpectedQuoteRef);
        valueDatePriceVolumeLayer = new ValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                  expectedValueDate);
        traderPriceVolumeLayer = new TraderPriceVolumeLayer(ExpectedPrice, ExpectedVolume)
        {
            [0] = new TraderLayerInfo(expectedTraderName, ExpectedVolume)
        };
        srcQtRefTrdrVlDtPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                                expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new TraderLayerInfo(expectedTraderName, ExpectedVolume)
        };

        pqPriceVolumeLayer = new PQPriceVolumeLayer(ExpectedPrice, ExpectedVolume);
        pqSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                expectedSourceName, true);
        pqSourceQuoteRefPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume,
                                                                                expectedSourceName, true, ExpectedQuoteRef);
        pqValueDatePriceVolumeLayer = new PQValueDatePriceVolumeLayer(ExpectedPrice, ExpectedVolume,
                                                                      expectedValueDate);
        pqTraderPriceVolumeLayer = new PQTraderPriceVolumeLayer(nameIdGenerator.Clone(), ExpectedPrice, ExpectedVolume)
        {
            [0] = new PQTraderLayerInfo(nameIdGenerator.Clone(), expectedTraderName, ExpectedVolume)
        };
        pqSrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nameIdGenerator, ExpectedPrice, ExpectedVolume,
                                                                                    expectedValueDate, expectedSourceName, true, ExpectedQuoteRef)
        {
            [0] = new PQTraderLayerInfo(nameIdGenerator.Clone(), expectedTraderName, ExpectedVolume)
        };


        ipqSourceTickerInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));
        ipqSourceTickerInfo.NameIdLookup = nameIdGenerator;
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.None;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.Executable | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourcePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.IsInstanceOfType(pqRecentlyTradedFactory, typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.SourceName |
                                         LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable |
                                         LayerFlags.SourceName | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQSourceQuoteRefPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnValueDatePriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(ValueDatePriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnTraderPriceVolumeLayerFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.TraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.TraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Volume | LayerFlags.TraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                         LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(), typeof(PQTraderPriceVolumeLayerFactory));
    }

    [TestMethod]
    public void VariosLayerFlags_Select_ReturnsSrcQtRefTrdrVlDtPvlFactory()
    {
        ipqSourceTickerInfo.LayerFlags = LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        var pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.ValueDate | LayerFlags.TraderName;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate | LayerFlags.SourceName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags = LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags = LayerFlags.TraderName | LayerFlags.Executable | LayerFlags.ValueDate;
        pqRecentlyTradedFactory        = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));

        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference | LayerFlags.ValueDate;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
        ipqSourceTickerInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable | LayerFlags.SourceName | LayerFlags.SourceQuoteReference
          | LayerFlags.ValueDate | LayerFlags.TraderName;
        pqRecentlyTradedFactory = layerSelector.FindForLayerFlags(ipqSourceTickerInfo);
        Assert.AreEqual(pqRecentlyTradedFactory.GetType(),
                        typeof(PQSourceQuoteRefPQTraderValueDatePriceVolumeLayerFactory));
    }


    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_UpgradeExistingLayer_ConvertsToPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.UpgradeExistingLayer(priceVolumeLayer, priceVolumeLayerType, priceVolumeLayer);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourcePriceVolumeLayerType, sourcePriceVolumeLayer);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.UpgradeExistingLayer(sourcePriceVolumeLayer, sourceQuoteRefPriceVolumeLayerType, sourcePriceVolumeLayer)
                             .CopyFrom(sourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.UpgradeExistingLayer(valueDatePriceVolumeLayer, valueDatePriceVolumeLayerType, valueDatePriceVolumeLayer);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.UpgradeExistingLayer(traderPriceVolumeLayer, traderPriceVolumeLayerType, traderPriceVolumeLayer);
        var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pqPvl = layerSelector.UpgradeExistingLayer(srcQtRefTrdrVlDtPvl, srcQtRefTrdrVlDtPvlType, srcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_CreateExpectedImplementation_ClonesPQPriceVolumeLayerType()
    {
        var pqPvl = layerSelector.CreateExpectedImplementation(priceVolumeLayerType, nameIdGenerator).CopyFrom(pqPriceVolumeLayer);
        Assert.AreEqual(pqPvl.GetType(), typeof(PQPriceVolumeLayer));
        Assert.AreNotSame(pqPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);

        pqPvl = layerSelector.CreateExpectedImplementation(sourcePriceVolumeLayerType, nameIdGenerator).CopyFrom(pqSourcePriceVolumeLayer);
        var pqSrcPvl = pqPvl as PQSourcePriceVolumeLayer;
        Assert.IsNotNull(pqSrcPvl);
        Assert.AreNotSame(pqSourcePriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcPvl.SourceName);
        Assert.AreEqual(true, pqSrcPvl.Executable);

        pqPvl = layerSelector.CreateExpectedImplementation(sourceQuoteRefPriceVolumeLayerType, nameIdGenerator)
                             .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        var pqSrcQtRefPvl = pqPvl as PQSourceQuoteRefPriceVolumeLayer;
        Assert.IsNotNull(pqSrcQtRefPvl);
        Assert.AreNotSame(pqSourceQuoteRefPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, pqSrcQtRefPvl.SourceName);
        Assert.AreEqual(true, pqSrcQtRefPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, pqSrcQtRefPvl.SourceQuoteReference);

        pqPvl = layerSelector.CreateExpectedImplementation(valueDatePriceVolumeLayerType, nameIdGenerator).CopyFrom(pqValueDatePriceVolumeLayer);
        var pqVlDtPvl = pqPvl as PQValueDatePriceVolumeLayer;
        Assert.IsNotNull(pqVlDtPvl);
        Assert.AreNotSame(pqValueDatePriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedValueDate, pqVlDtPvl.ValueDate);

        pqPvl = layerSelector.CreateExpectedImplementation(traderPriceVolumeLayerType, nameIdGenerator).CopyFrom(pqTraderPriceVolumeLayer);
        var pqTrdrPvl = pqPvl as PQTraderPriceVolumeLayer;
        Assert.IsNotNull(pqTrdrPvl);
        Assert.AreNotSame(pqTraderPriceVolumeLayer, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedTraderName, pqTrdrPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, pqTrdrPvl[0]!.TraderVolume);

        pqPvl = layerSelector.CreateExpectedImplementation(srcQtRefTrdrVlDtPvlType, nameIdGenerator).CopyFrom(pqSrcQtRefTrdrVlDtPvl);
        var convertedPqSrcQtRefTrdrVlDtPvl = pqPvl as PQSourceQuoteRefTraderValueDatePriceVolumeLayer;
        Assert.IsNotNull(convertedPqSrcQtRefTrdrVlDtPvl);
        Assert.AreNotSame(pqSrcQtRefTrdrVlDtPvl, pqPvl);
        Assert.AreNotSame(srcQtRefTrdrVlDtPvl, pqPvl);
        Assert.AreEqual(ExpectedPrice, pqPvl.Price);
        Assert.AreEqual(ExpectedVolume, pqPvl.Volume);
        Assert.AreEqual(expectedSourceName, convertedPqSrcQtRefTrdrVlDtPvl.SourceName);
        Assert.AreEqual(true, convertedPqSrcQtRefTrdrVlDtPvl.Executable);
        Assert.AreEqual(ExpectedQuoteRef, convertedPqSrcQtRefTrdrVlDtPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, convertedPqSrcQtRefTrdrVlDtPvl.ValueDate);
        Assert.AreEqual(expectedTraderName, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderName);
        Assert.AreEqual(ExpectedVolume, convertedPqSrcQtRefTrdrVlDtPvl[0]!.TraderVolume);
    }

    [TestMethod]
    public void NonPQPriceVolumeLayerTypes_TypeCanWhollyContain_ReturnsAsExpected()
    {
        var ltPriceVolumeLayerFlags               = LayerType.PriceVolume.SupportedLayerFlags();
        var ltSourcePriceVolumeLayerFlags         = LayerType.SourcePriceVolume.SupportedLayerFlags();
        var ltSourceQuoteRefPriceVolumeLayerFlags = LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags();
        var ltValueDatePriceVolumeLayerFlags      = LayerType.ValueDatePriceVolume.SupportedLayerFlags();
        var ltTraderPriceVolumeLayerFlags         = LayerType.TraderPriceVolume.SupportedLayerFlags();
        var ltSrcQtRefTrdrVlDtPvlFlags            = LayerType.SourceQuoteRefTraderValueDatePriceVolume.SupportedLayerFlags();


        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, priceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, priceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, sourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, sourcePriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, sourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, sourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, valueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, valueDatePriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, traderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, traderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, traderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, traderPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, traderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, traderPriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, srcQtRefTrdrVlDtPvlTypeSupportedFlags));
    }

    [TestMethod]
    public void PQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected()
    {
        var ltPriceVolumeLayerFlags               = LayerType.PriceVolume.SupportedLayerFlags();
        var ltSourcePriceVolumeLayerFlags         = LayerType.SourcePriceVolume.SupportedLayerFlags();
        var ltSourceQuoteRefPriceVolumeLayerFlags = LayerType.SourceQuoteRefPriceVolume.SupportedLayerFlags();
        var ltValueDatePriceVolumeLayerFlags      = LayerType.ValueDatePriceVolume.SupportedLayerFlags();
        var ltTraderPriceVolumeLayerFlags         = LayerType.TraderPriceVolume.SupportedLayerFlags();
        var ltSrcQtRefTrdrVlDtPvlFlags            = LayerType.SourceQuoteRefTraderValueDatePriceVolume.SupportedLayerFlags();

        Assert.IsTrue(layerSelector.OriginalCanWhollyContain(ltPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourcePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse(layerSelector.OriginalCanWhollyContain(ltSourceQuoteRefPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqPriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqSourcePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqSourcePriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqSourceQuoteRefPriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqValueDatePriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqValueDatePriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, pqTraderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, pqTraderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, pqTraderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqTraderPriceVolumeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqTraderPriceVolumeSupportedFlags));
        Assert.IsFalse
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqTraderPriceVolumeSupportedFlags));

        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourcePriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSourceQuoteRefPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltValueDatePriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltTraderPriceVolumeLayerFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
        Assert.IsTrue
            (layerSelector.OriginalCanWhollyContain
                (ltSrcQtRefTrdrVlDtPvlFlags, pqSrcQtRefTrdrVlDtPvlTypeSupportedFlags));
    }


    [TestMethod]
    public void NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToMostCompactSupportedType()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, priceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, sourcePriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator,
                                                    sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqTraderPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, valueDatePriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqTraderPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, valueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQTraderPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, traderPriceVolumeLayerType);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, traderPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqSourcePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqSourceQuoteRefPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqValueDatePriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, srcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, srcQtRefTrdrVlDtPvlType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain()
    {
        IPriceVolumeLayer result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourcePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourcePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourcePriceVolumeLayerType)
                              .CopyFrom(pqSrcQtRefTrdrVlDtPvl);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefPriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSourceQuoteRefPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSourceQuoteRefPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqValueDatePriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqValueDatePriceVolumeLayerType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqValueDatePriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayerType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQTraderPriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayerType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayerType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayerType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqTraderPriceVolumeLayerType);
        Assert.AreSame(result, pqTraderPriceVolumeLayer);
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqTraderPriceVolumeLayerType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);

        result = layerSelector.UpgradeExistingLayer(pqPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType).CopyFrom(pqPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourcePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourcePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourcePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSourceQuoteRefPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqSourceQuoteRefPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqSourceQuoteRefPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqValueDatePriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqValueDatePriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqValueDatePriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqTraderPriceVolumeLayer, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType)
                              .CopyFrom(pqTraderPriceVolumeLayer);
        Assert.AreNotSame(result, pqPriceVolumeLayer);
        Assert.IsInstanceOfType(result, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
        Assert.IsTrue(pqTraderPriceVolumeLayer.AreEquivalent(result));
        result = layerSelector.UpgradeExistingLayer(pqSrcQtRefTrdrVlDtPvl, nameIdGenerator, pqSrcQtRefTrdrVlDtPvlType);
        Assert.AreSame(result, pqSrcQtRefTrdrVlDtPvl);
    }

    [TestMethod]
    public void NullPriceVolumeLayerEntries_SelectPriceDesiredLayerType_HandlesEmptyValues()
    {
        var result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, priceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, sourcePriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQSourcePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, sourceQuoteRefPriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQSourceQuoteRefPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, valueDatePriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQValueDatePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, traderPriceVolumeLayerType)!;
        Assert.AreEqual(typeof(PQTraderPriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
        result = layerSelector.UpgradeExistingLayer(null, nameIdGenerator, srcQtRefTrdrVlDtPvlType)!;
        Assert.AreEqual(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer), result.GetType());
        Assert.IsTrue(result.IsEmpty);
    }
}
