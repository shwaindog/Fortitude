using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading;

namespace FortitudeMarketsCore.Trading.ORX
{
    public abstract class OrxTradingMessage : OrxAuthenticatedMessage, ITradingMessage
    {
        protected OrxTradingMessage()
        {
        }

        protected OrxTradingMessage(ITradingMessage toClone)
        : base(toClone)
        {
            SequenceNumber = toClone.SequenceNumber;
            IsReplay = toClone.IsReplay;
            MachineName = toClone.MachineName != null ? new MutableString(toClone.MachineName) : null;
            OriginalSendTime = toClone.OriginalSendTime;
        }

        protected OrxTradingMessage(byte versionNumber, int sequenceNumber, bool isReplay, DateTime sendTime, 
            string machineName, string senderName, DateTime originalSendTime, string info, OrxUserData userData)
        : this(versionNumber, sequenceNumber, isReplay, sendTime, (MutableString)machineName, 
            senderName, originalSendTime, info, userData)
        { }

        protected OrxTradingMessage(byte versionNumber, int sequenceNumber, bool isReplay, DateTime sendTime, 
            MutableString machineName, MutableString senderName, DateTime originalSendTime, MutableString info,
            OrxUserData userData)
        : base(versionNumber, sendTime, senderName, info, userData)
        {
            SequenceNumber = sequenceNumber;
            IsReplay = isReplay;
            MachineName = machineName;
            OriginalSendTime = originalSendTime;
        }

        [OrxMandatoryField(5)]
        public int SequenceNumber { get; set; }

        [OrxMandatoryField(6)]
        public bool IsReplay { get; set; }

        [OrxOptionalField(7)]
        public MutableString MachineName { get; set; }

        IMutableString ITradingMessage.MachineName
        {
            get => MachineName;
            set => MachineName = (MutableString)value;
        }
        
        [OrxOptionalField(8)]
        public DateTime OriginalSendTime { get; set; } = DateTimeConstants.UnixEpoch;

        public override void CopyFrom(IVersionedMessage source, IRecycler orxRecyclingFactory)
        {
            base.CopyFrom(source, orxRecyclingFactory);
            if (source is ITradingMessage tradingMessage)
            {
                SequenceNumber = tradingMessage.SequenceNumber;
                IsReplay = tradingMessage.IsReplay;
                MachineName = tradingMessage.MachineName != null
                    ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(tradingMessage.MachineName)
                    : null;
                OriginalSendTime = tradingMessage.OriginalSendTime;
            }
        }

        public virtual void Configure()
        {
            base.Configure(TradingVersionInfo.CurrentVersion);
            MachineName = null;
            IsReplay = false;
            OriginalSendTime = DateTimeConstants.UnixEpoch;
        }
    }
}
