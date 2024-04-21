#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization;

public class RemoteNotificationRegistration : RecyclableObject
{
    public uint? MessageId { get; set; }

    public IConverter? Converter { get; set; }

    public Type? DeserializedType { get; set; }

    protected virtual string StateToString =>
        $"{nameof(MessageId)}: {MessageId}, {nameof(Converter)}: {Converter}, {nameof(DeserializedType)}: {DeserializedType?.Name}";

    public override string ToString() => $"{GetType().Name}({StateToString})";
}

public class RemoteMessageBusPublishRegistration : RemoteNotificationRegistration
{
    public string PublishAddress { get; set; } = null!;
    public AddRemoveCommand AddRemoveRegistration { get; set; }
    public Type PublishType { get; set; } = null!;
    public IQueueContext? QueueContext { get; set; }
    public IRule? Rule { get; set; }

    protected override string StateToString =>
        $"{base.StateToString}, {nameof(PublishAddress)}: {PublishAddress}, {nameof(AddRemoveRegistration)}: {AddRemoveRegistration}, " +
        $"{nameof(PublishType)}: {PublishType.Name}, {nameof(QueueContext)}: {QueueContext}, {nameof(Rule)}: {Rule}";

    public override void StateReset()
    {
        QueueContext = null;
        Rule = null;
        PublishType = null!;
        base.StateReset();
    }
}

public class RemoteRequestIdResponseRegistration : RemoteNotificationRegistration
{
    public IAsyncResponseSource ResponseSource { get; set; } = null!;

    public int RequestId { get; set; }

    public int TimeoutTimespan { get; set; }

    protected override string StateToString =>
        $"{base.StateToString}, {nameof(ResponseSource)}: {ResponseSource}, {nameof(RequestId)}: {RequestId}, " +
        $"{nameof(TimeoutTimespan)}: {TimeoutTimespan}";

    public override void StateReset()
    {
        ResponseSource = null!;
        RequestId = -1;
        base.StateReset();
    }
}

public class RemoteRegistrationResponse : RecyclableObject
{
    public bool Succeeded { get; set; }
    public string FailureReason { get; set; } = null!;

    public override void StateReset()
    {
        Succeeded = false;
        FailureReason = null!;
        base.StateReset();
    }

    public override string ToString() => $"{GetType().Name}({nameof(Succeeded)}: {Succeeded}, {nameof(FailureReason)}: {FailureReason})";
}

public class RemoteMessageBusPublishRegistrationResponse : RecyclableObject
{
    public bool Succeeded { get; set; }
    public string FailureReason { get; set; } = null!;

    public string? UnsubscribeAddress { get; set; }
    public RemoteMessageBusPublishRegistration? UnsubscribeRequest { get; set; }

    public override void StateReset()
    {
        Succeeded = false;
        FailureReason = null!;
        UnsubscribeAddress = null!;
        UnsubscribeRequest?.DecrementRefCount();
        UnsubscribeRequest = null!;
        base.StateReset();
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(Succeeded)}: {Succeeded}, {nameof(FailureReason)}: {FailureReason}, " +
        $"{nameof(UnsubscribeAddress)}: {UnsubscribeAddress}, {nameof(UnsubscribeRequest)}: {UnsubscribeRequest})";
}
