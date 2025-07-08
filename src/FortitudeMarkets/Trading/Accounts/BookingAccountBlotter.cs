using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeMarkets.Trading.Limits;

namespace FortitudeMarkets.Trading.Accounts;


public delegate long BookingHandler(IOrderBlotterEntry toBook);

public interface ILimitCheckingAccountBlotterView : IBlotterView
{
    ReserveResult LimitCheck(IOrderBlotterEntry checkForLimitBreaches);

    LimitKey[] CheckingLimits { get; }

    new IBookingAccountBlotter ParentBlotter { get; }
}


public interface IBookingAccountBlotter : IAccountBlotter, ILimitCheckingAccountBlotterView
{
    IBlotterView LimitReservedOrders { get; } // PendingExecution to the venue

    IBlotterView RejectedOrders { get; } // either by limit breaches or from the venue

    void AddToLimitReservedOrders(IOrderBlotterEntry passedLocalLimits);

    ReserveResult LimitCheckReserve(IOrderBlotterEntry checkForLimitBreaches);

    void CommitReserved(IOrderBlotterEntry checkForLimitBreaches);

    void RollbackReserved(IOrderBlotterEntry checkForLimitBreaches);

    IBookingAccountBlotter DefaultBookingPortfolio { get; set; }

    IReadOnlyDictionary<uint, IBookingAccountBlotter> AlternativeBookingPortfolios { get; set; }

    uint[]     AuthorisedOnBehalfOfIds { get; set; }

    LimitKey[] ConfiguredLimits { get; set; }

    IReadOnlyDictionary<LimitKey, ILimitCheckingAccountBlotterView> GetLimitApplicableViewsMap { get; }

    BookingHandler? BookingHandler { get; set; }
}


public class BookingAccountBlotter
{
}