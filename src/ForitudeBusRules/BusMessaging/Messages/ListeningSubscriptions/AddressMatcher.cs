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

    public bool IsMatch(string address) => address.Contains(AddressMatchPattern);

    public static bool IsMatcherPattern(string checkAddress) => checkAddress.Contains("*");
}
