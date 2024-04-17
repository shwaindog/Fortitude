#region

using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public struct ConversationMessageNotification<TR>
{
    public ConversationMessageNotification(TR message, MessageHeader header, IConversation conversation)
    {
        Message = message;
        Header = header;
        Conversation = conversation;
    }

    public TR Message { get; }

    public MessageHeader Header { get; }

    public IConversation Conversation { get; }
}

public interface IReceiverListenContext : IStoreState<IReceiverListenContext>
{
    string Name { get; }
    int UsageCount { get; set; }
    Type ExpectedType { get; }
    int IncrementUsage();
    int DecrementUsage();
}

public interface IReceiverListenContext<TR> : IReceiverListenContext, ICloneable<IReceiverListenContext<TR>>
{
    void SendToReceiver(ConversationMessageNotification<TR> conversationMessageNotification);
    void SendToReceiver(TR conversationMessageNotification);
}

public abstract class ReceiverListenContext<TR> : IReceiverListenContext<TR>
{
    protected ReceiverListenContext(string name) => Name = name;

    protected ReceiverListenContext(ReceiverListenContext<TR> toClone)
    {
        Name = toClone.Name;
        UsageCount = toClone.UsageCount;
    }

    public string Name { get; }
    public int UsageCount { get; set; }
    public int IncrementUsage() => ++UsageCount;
    public int DecrementUsage() => --UsageCount;
    public Type ExpectedType => typeof(TR);
    public abstract void SendToReceiver(ConversationMessageNotification<TR> conversationMessageNotification);
    public abstract void SendToReceiver(TR conversationMessageNotification);

    public abstract IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags);

    public abstract IReceiverListenContext CopyFrom(IReceiverListenContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    object ICloneable.Clone() => Clone();

    public abstract IReceiverListenContext<TR> Clone();
}
