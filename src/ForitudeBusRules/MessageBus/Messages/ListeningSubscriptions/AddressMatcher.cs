namespace FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;

public interface IAddressMatcher
{
    bool IsMatch(string address);
}

public class AddressMatcher : IAddressMatcher
{
    private readonly string addressMatchPattern;

    public AddressMatcher(string addressPattern) => addressMatchPattern = addressPattern.Replace("*", "");

    public bool IsMatch(string address) => addressMatchPattern.Contains(address);

    public static bool IsMatcherPattern(string checkAddress) => checkAddress.Contains("*");
}
