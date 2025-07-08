using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FortitudeIO.Storage.Database.Config;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface ITradingAccountsConfig
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IDbConnectionConfig? DbTradingAccountsConnectionConfig { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IReadOnlyDictionary<uint, IAccountTradingConfig>? DbResolvedTradingAccounts { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IReadOnlyDictionary<uint, IAccountTradingConfig>? ConfigTradingAccountsOverride { get; set; }
    
    [JsonIgnore]
    IReadOnlyDictionary<uint, IAccountTradingConfig>? FinalTradingAccounts { get; }

}


public class TradingAccountsConfig
{
}