// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;

public enum QuoteStorageType
{
    MessageSerdes
  , CompressedFieldsDiffArray // Todo implement
  , StructArray               // Todo implement
  , CompressedStructArray     // Todo implement
}

public interface IPriceQuoteFileHeader : IFileSubHeader
{
    ISourceTickerQuoteInfo SourceTickerQuoteInfo          { get; }
    IMessageDeserializer   DefaultMessageDeserializer     { get; }
    IMessageSerializer     IndexEntryMessageSerializer    { get; }
    IMessageSerializer     RepeatedEntryMessageSerializer { get; }
}

public interface IPriceQuoteFileHeader<TEntry> : IPriceQuoteFileHeader where TEntry : class, IVersionedMessage
{
    new IMessageDeserializer<TEntry> DefaultMessageDeserializer     { get; }
    new IMessageSerializer<TEntry>   IndexEntryMessageSerializer    { get; }
    new IMessageSerializer<TEntry>   RepeatedEntryMessageSerializer { get; }
}

public interface IMutablePriceQuoteFileHeader<TEntry> : IPriceQuoteFileHeader<TEntry> where TEntry : class, IVersionedMessage
{
    new ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; set; }
}
