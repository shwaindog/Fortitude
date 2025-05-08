// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeBusRules.Connectivity.Cluster.Messages;

public interface IBusRulesMessage : IVersionedMessage
{
    DateTime SendTime { get; }

    uint MessageSequenceNumber { get; }
}

public abstract class BusRulesMessage : OrxVersionedMessage, IBusRulesMessage
{
    protected BusRulesMessage() { }

    protected BusRulesMessage(IBusRulesMessage toClone) : base(toClone)
    {
        SendTime              = toClone.SendTime;
        MessageSequenceNumber = toClone.MessageSequenceNumber;
    }

    [OrxMandatoryField(1)] public DateTime SendTime              { get; set; } = DateTimeConstants.UnixEpoch;
    [OrxMandatoryField(2)] public uint     MessageSequenceNumber { get; set; }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage versionedMessage
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is BusRulesMessage busRulesMessage)
        {
            SendTime              = busRulesMessage.SendTime;
            MessageSequenceNumber = busRulesMessage.MessageSequenceNumber;
        }

        return this;
    }
}
