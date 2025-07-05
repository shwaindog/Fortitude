using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Trading.Accounts;

public enum AccountType
{
    None
   , Venue  // takes the opposite position of the other accounts
   , Adapter
   , Division
   , Desk
   , Trader
   , Strategy
   , Client
   , Portfolio
}

public enum AccountViewType
{
    None
  , All
  , LimitReserved
  , Rejected
  , OpenPosition
  , OpenOrders
  , PassiveOrders
  , AggressiveOrders
  , DoneOrders
}
