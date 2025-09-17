// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void FLogStringSerializer<in T>(T toSerialize, IStringBuilder toAppendTo);

public record RegisteredStringSerializer
(
    Type RegisteredForType
  , Delegate Serializer
  , FLogCallLocation RegisteredLocation
  , bool UsesStyleAppender = false)
{
    private void Invoke<T>(T toSerialize, IStringBuilder toAppendTo)
    {
        if (Serializer is FLogStringSerializer<T> flogSerializer)
            flogSerializer.Invoke(toSerialize, toAppendTo);
        else if (Serializer is Action<T, IStringBuilder> actionSerializer) actionSerializer.Invoke(toSerialize, toAppendTo);
    }

    public void Invoke<T>(T toSerialize, ITheOneString toAppendTo)
    {
        if (!UsesStyleAppender)
        {
            Invoke(toSerialize, toAppendTo.WriteBuffer);
            return;
        }
        if (Serializer is Action<T, ITheOneString> actionSerializer) actionSerializer.Invoke(toSerialize, toAppendTo);
    }
}

public static class FLogStringSerializerRegistry
{
    private static readonly ConcurrentDictionary<Type, RegisteredStringSerializer> ExternalRegisteredSerializer = new();

    private static readonly ConcurrentDictionary<Type, RegisteredStringSerializer> FlogAutoRegisteredSerializers = new();

    public static void RegisterTypeSerializer<T>(FLogStringSerializer<T> toRegister,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var registeredFrom = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var typeOfT        = typeof(T);
        ExternalRegisteredSerializer.TryRemove(typeOfT, out _);
        ExternalRegisteredSerializer.TryAdd(typeOfT, new RegisteredStringSerializer(typeOfT, toRegister, registeredFrom));
    }

    public static void RegisterTypeSerializer<T>(Action<T, ITheOneString> toRegister,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var registeredFrom = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var typeOfT        = typeof(T);
        ExternalRegisteredSerializer.TryRemove(typeOfT, out _);
        ExternalRegisteredSerializer.TryAdd(typeOfT, new RegisteredStringSerializer(typeOfT, toRegister, registeredFrom));
    }

    public static void AutoRegisterSerializerFor<T>(Action<T, ITheOneString> autoCreatedSerializer, FLogCallLocation callLocation)
    {
        var typeOfT = typeof(T);

        FlogAutoRegisteredSerializers.TryRemove(typeOfT, out _);
        FlogAutoRegisteredSerializers.TryAdd
            (typeOfT, new RegisteredStringSerializer(typeOfT, autoCreatedSerializer, callLocation, true));
    }

    public static bool TryGetSerializerFor<T>(T toSerialize, [NotNullWhen(true)] out RegisteredStringSerializer? value)
    {
        var typeOfT = typeof(T);
        value = null;
        if (ExternalRegisteredSerializer.TryGetValue(typeOfT, out value)) return true;
        if (FlogAutoRegisteredSerializers.TryGetValue(typeOfT, out value)) return true;
        return false;
    }
}
