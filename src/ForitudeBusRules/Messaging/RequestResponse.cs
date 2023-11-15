namespace Fortitude.EventProcessing.BusRules.Messaging;

public interface IRequestResponse<T>
{
    public DateTime SentDateTime { get; }
    public DateTime ReceivedDateTime { get; }
    public DateTime ReturnedDateTime { get; }
    public T Body { get; }
}

class RequestResponse<T> : IRequestResponse<T>
{
    public DateTime SentDateTime { get; set; }
    public DateTime ReceivedDateTime { get; set; }
    public DateTime ReturnedDateTime { get; set; }
    public T Body { get; set; }
}
