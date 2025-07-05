using FortitudeMarkets.Trading.Accounts;

namespace FortitudeMarkets.Trading.Limits;

public class LimitCheck
{
    protected ILimitCheckingAccountBlotterView OrderHistory;

    protected LimitKey Limits;

    public LimitCheck(LimitKey limits, ILimitCheckingAccountBlotterView orderHistory)
    {
        Limits       = limits;
        OrderHistory = orderHistory;

        VerifyViewMeetsLimitPeriod();
    }

    private void VerifyViewMeetsLimitPeriod()
    {
    }

    public AccountLimitBreachResult? CheckForLimitBreach(IOrderBlotterEntry orderBlotterEntry)
    {
        return null;
    }
}
