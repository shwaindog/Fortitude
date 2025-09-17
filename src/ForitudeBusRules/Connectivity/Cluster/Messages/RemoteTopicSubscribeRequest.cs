// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeBusRules.Connectivity.Cluster.Messages;

public interface IRemoteTopicSubscribeRequest : IBusRulesMessage
{
    int SubscriberSubscriptionId { get; }

    MutableString? RemoteName { get; set; }

    MutableString? RemoteDestinationAddress { get; set; }

    MutableString? ExpectedMessageType { get; set; }
}

public class RemoteTopicSubscribeRequest : BusRulesMessage, IRemoteTopicSubscribeRequest
{
    public RemoteTopicSubscribeRequest() { }

    public RemoteTopicSubscribeRequest(IRemoteTopicSubscribeRequest toClone) : base(toClone)
    {
        RemoteName               = toClone.RemoteName?.Clone();
        RemoteDestinationAddress = toClone.RemoteDestinationAddress?.Clone();
        ExpectedMessageType      = toClone.ExpectedMessageType?.Clone();
    }

    public override uint MessageId => (uint)InterClusterMessageIds.RemoteTopicSubscribeRequest;

    [OrxMandatoryField(10)] public int SubscriberSubscriptionId { get; set; }

    [OrxMandatoryField(11)] public MutableString? RemoteName               { get; set; }
    [OrxMandatoryField(12)] public MutableString? RemoteDestinationAddress { get; set; }
    [OrxMandatoryField(13)] public MutableString? ExpectedMessageType      { get; set; }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<RemoteTopicSubscribeRequest>().CopyFrom(this) ?? new RemoteTopicSubscribeRequest(this);


    public override IVersionedMessage CopyFrom
        (IVersionedMessage versionedMessage, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IRemoteTopicSubscribeRequest remoteTopicSubscribeRequest)
        {
            RemoteName               = remoteTopicSubscribeRequest.RemoteName.CopyOrClone(RemoteName);
            RemoteDestinationAddress = remoteTopicSubscribeRequest.RemoteDestinationAddress.CopyOrClone(RemoteDestinationAddress);
            ExpectedMessageType      = remoteTopicSubscribeRequest.ExpectedMessageType.CopyOrClone(ExpectedMessageType);
        }

        return this;
    }

    public override void StateReset()
    {
        SubscriberSubscriptionId = 0;
        RemoteName?.DecrementRefCount();
        RemoteName = null;
        RemoteDestinationAddress?.DecrementRefCount();
        RemoteDestinationAddress = null;
        ExpectedMessageType?.DecrementRefCount();
        ExpectedMessageType = null;
        base.StateReset();
    }
}
