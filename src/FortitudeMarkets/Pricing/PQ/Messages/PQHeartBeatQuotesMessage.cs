// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages;

public interface IPQHeartBeatQuotesMessage : IVersionedMessage
{
    List<IPQMessage> QuotesToSendHeartBeats { get; set; }
}

public class PQHeartBeatQuotesMessage : ReusableObject<IVersionedMessage>, IPQHeartBeatQuotesMessage
  , IEnumerable<IPQMessage>
{
    public PQHeartBeatQuotesMessage() => QuotesToSendHeartBeats = new List<IPQMessage>();

    public PQHeartBeatQuotesMessage(List<IPQMessage> quotesToSendHeartBeats) => QuotesToSendHeartBeats = quotesToSendHeartBeats;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQMessage> GetEnumerator() => QuotesToSendHeartBeats.GetEnumerator();

    public uint MessageId => (uint)PQMessageIds.HeartBeat;

    public byte Version => 1;

    public List<IPQMessage> QuotesToSendHeartBeats { get; set; }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQHeartBeatQuotesMessage heartBeatQuotesMessage)
        {
            QuotesToSendHeartBeats.Clear();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < heartBeatQuotesMessage.QuotesToSendHeartBeats.Count; i++)
                QuotesToSendHeartBeats.Add(heartBeatQuotesMessage.QuotesToSendHeartBeats[i]);
        }

        return this;
    }

    public override void StateReset()
    {
        QuotesToSendHeartBeats.Clear();
        base.StateReset();
    }

    public override IVersionedMessage Clone() => throw new NotImplementedException();

    protected bool Equals(PQHeartBeatQuotesMessage other) => Equals(QuotesToSendHeartBeats, other.QuotesToSendHeartBeats);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PQHeartBeatQuotesMessage)obj);
    }

    public override int GetHashCode() => QuotesToSendHeartBeats != null ? QuotesToSendHeartBeats.GetHashCode() : 0;
}
