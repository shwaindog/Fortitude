namespace FortitudeBusRules.Messaging;

public struct RequestResponse<T>
{
    public RequestResponse(IDispatchResult? dispatchResult, T? response)
    {
        DispatchResult = dispatchResult;
        Response = response;
    }

    public IDispatchResult? DispatchResult { get; set; }
    public T? Response { get; set; }
}
