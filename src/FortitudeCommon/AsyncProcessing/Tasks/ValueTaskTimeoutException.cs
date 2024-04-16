namespace FortitudeCommon.AsyncProcessing.Tasks;

public class ValueTaskTimeoutException : Exception
{
    public ValueTaskTimeoutException(string? message) : base(message) { }
}
