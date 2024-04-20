namespace FortitudeBusRules.Messages;

public struct RequestResponse<T>
{
    public RequestResponse(IDispatchResult? dispatchResult, T? response)
    {
        DispatchResult = dispatchResult;
        Response = response;
    }

    public IDispatchResult? DispatchResult { get; set; }
    public T? Response { get; set; }

    public override string ToString() => $"RequestResponse({nameof(DispatchResult)}: {DispatchResult}, {nameof(Response)}: {Response})";
}
