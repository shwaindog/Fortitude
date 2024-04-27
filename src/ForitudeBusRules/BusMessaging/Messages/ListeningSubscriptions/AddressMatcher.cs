namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IAddressMatcher
{
    string AddressMatchPattern { get; }
    bool IsMatch(string address);
}

public class AddressMatcher : IAddressMatcher
{
    public AddressMatcher(string addressPattern) => AddressMatchPattern = addressPattern.Replace("*", "");

    public string AddressMatchPattern { get; }

    public bool IsMatch(string address) => AddressMatchPattern.Contains(address);

    public static bool IsMatcherPattern(string checkAddress) => checkAddress.Contains("*");
}
