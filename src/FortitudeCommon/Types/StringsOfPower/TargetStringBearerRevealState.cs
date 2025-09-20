// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Types.StringsOfPower;

public static class TargetStringBearerRevealState
{
    private static ConcurrentDictionary<Type, Delegate> nonVirtualToString = new();

    private static readonly Type TheOneStringType = typeof(ITheOneString);

    public static void CallBaseStyledToString<T, TBase>(T baseHasStyledToString, ITheOneString stsa)
        where T : TBase where TBase : class, IStringBearer
    {
        TBase asBase = baseHasStyledToString;

        var callBaseToString = (StringBearerRevealState<TBase>)nonVirtualToString.GetOrAdd(typeof(TBase), bType => CreateInvokeMethod<TBase>());

        callBaseToString(asBase, stsa);
    }

    public static void CallBaseStyledToStringIfSupported<T>(T baseHasStyledToString, ITheOneString stsa)
        where T : IStringBearer
    {
        Type baseType = typeof(T).BaseType!;

        var callBaseToString = (StringBearerRevealState<T>)nonVirtualToString.GetOrAdd(baseType, bType => CreateInvokeMethod(bType));

        callBaseToString(baseHasStyledToString, stsa);
    }

    private record TypeCallCount(Type ForType)
    {
        public int CallCount { get; set; }
    };

    private static ConcurrentDictionary<Type, TypeCallCount> noOpCalls = new();

    private static StateExtractStringRange NoOpBaseDoesntSupportStyledToString<T>(T noSupporting, ITheOneString stsa)
    {
        var callCount = noOpCalls.GetOrAdd(typeof(T), new TypeCallCount(typeof(T)));
        callCount.CallCount++;
        return new StateExtractStringRange();
    }

    public static StringBearerRevealState<TBase> CreateInvokeMethod<TBase>() where TBase : class, IStringBearer
    {
        var methodToCall = typeof(TBase).GetMethod(nameof(IStringBearer.RevealState), [TheOneStringType]);

        var methodInvoker = GetNonVirtualDispatchStyledToString<TBase>(methodToCall!);
        return methodInvoker;
    }

    public static Delegate CreateInvokeMethod(Type baseType)
    {
        var methodToCall = baseType.GetMethod(nameof(IStringBearer.RevealState), [TheOneStringType]);

        if (methodToCall?.MethodImplementationFlags == MethodImplAttributes.ForwardRef && baseType.BaseType != null)
        {
            return CreateInvokeMethod(baseType.BaseType);
        }
        var baseTypeCustomStyler = typeof(StringBearerRevealState<>).MakeGenericType(baseType);
        if (methodToCall == null || methodToCall.IsAbstract)
        {
            var myType         = typeof(TargetStringBearerRevealState);
            methodToCall = myType.GetMethod(nameof(NoOpBaseDoesntSupportStyledToString), [baseType, TheOneStringType]);
            var genericMethod        = methodToCall!.MakeGenericMethod(baseType);
            return GetNonVirtualDispatchStyledToString(baseType, genericMethod, baseTypeCustomStyler);
        }

        var methodInvoker = GetNonVirtualDispatchStyledToString(baseType, methodToCall!, baseTypeCustomStyler);
        return methodInvoker;
    }


    // Created with help from https://blog.adamfurmanek.pl/2020/01/11/net-inside-out-part-14-calling-virtual-method-without-dynamic-dispatch/index.html
    // Full credit and thanks for posting 


    public static Delegate GetNonVirtualDispatchStyledToString(Type styledToStringType, MethodInfo methodToCall, Type customStyleType)
    {
        var helperMethod =
            new DynamicMethod($"{styledToStringType.Name}_Styled_ToString", typeof(StateExtractStringRange),
                              [styledToStringType, typeof(ITheOneString)], styledToStringType.Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(customStyleType);
        return methodInvoker;
    }

    public static StringBearerRevealState<T> GetNonVirtualDispatchStyledToString<T>(MethodInfo methodToCall)
    {
        var methodInvoker = (StringBearerRevealState<T>)GetNonVirtualDispatchStyledToString(typeof(T), methodToCall, typeof(StringBearerRevealState<T>));
        return methodInvoker;
    }
}
