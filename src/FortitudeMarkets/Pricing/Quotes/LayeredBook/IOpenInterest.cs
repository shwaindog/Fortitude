// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public enum MarketDataSource
{
    None
  , Venue     = 1
  , Adapter   = 2
  , Published = 4
}

public interface IOpenInterest : IReusableObject<IOpenInterest>, IInterfacesComparable<IOpenInterest>
{
    MarketDataSource DataSource { get; }

    DateTime UpdateTime { get; }

    decimal Volume  { get; }
    decimal Vwap    { get; }
    bool    IsEmpty { get; }
}

public interface IMutableOpenInterest : IOpenInterest, IInterfacesComparable<IMutableOpenInterest>, ICloneable<IMutableOpenInterest>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new MarketDataSource DataSource { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DateTime UpdateTime { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Volume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Vwap   { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }

    new IMutableOpenInterest Clone();
}
