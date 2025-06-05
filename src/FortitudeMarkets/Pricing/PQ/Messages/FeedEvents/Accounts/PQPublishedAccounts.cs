using FortitudeMarkets.Pricing.FeedEvents.Accounts;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Accounts;

public interface IPQPublishedAccounts : IMutablePublishedAccounts, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}


public class PQPublishedAccounts
{
}