// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ICanCarrySocketReceiverPayload : ICanCarryTaskCallbackPayload
{
    bool IsSocketReceiverItem { get; }
    bool IsSocketAdd          { get; }

    ISocketReceiver? SocketReceiver { get; }

    public void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd)
    {
        // default implementation do nothing
    }
}
