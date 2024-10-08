﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IListenSubscribeInterceptor : IInterceptor
{
    ValueTask Intercept(IMessageListenerRegistration messageListenerRegistration);
    bool      ShouldRunIntercept(IMessageListenerRegistration messageListenerRegistration);
}

public abstract class AddressListenSubscribeInterceptor : IListenSubscribeInterceptor
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AddressListenSubscribeInterceptor));

    protected AddressListenSubscribeInterceptor(string name, IAddressMatcher addressMatcher)
    {
        Name           = name;
        AddressMatcher = addressMatcher;
    }

    public IAddressMatcher AddressMatcher { get; }

    public string Name { get; }

    public async ValueTask Intercept(IMessageListenerRegistration messageListenerRegistration)
    {
        if (ShouldRunIntercept(messageListenerRegistration))
        {
            messageListenerRegistration.AddRunListenSubscriptionInterceptor(this);
            await RunInterceptorAction(messageListenerRegistration);
        }
    }

    public bool ShouldRunIntercept(IMessageListenerRegistration messageListenerRegistration) =>
        AddressMatcher.IsMatch(messageListenerRegistration.PublishAddress)
     && messageListenerRegistration.ActiveListenSubscribeInterceptors.All(lsi => lsi.Name != Name);

    public abstract ValueTask RunInterceptorAction(IMessageListenerRegistration messageListenerRegistration);
}
