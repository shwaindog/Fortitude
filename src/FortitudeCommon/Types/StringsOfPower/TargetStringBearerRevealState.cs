// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static System.Reflection.BindingFlags;

namespace FortitudeCommon.Types.StringsOfPower;

public static class TargetStringBearerRevealState
{
    private static ConcurrentDictionary<Type, Delegate> nonVirtualToString = new();

    private static readonly Type       TheOneStringType = typeof(ITheOneString);

    private static MethodInfo? noOpGenericMethodInfo;

    public static void CallBaseStyledToString<T, TRevealBase>(T baseHasStyledToString, ITheOneString stsa)
        where T : TRevealBase 
        where TRevealBase : class, IStringBearer
    {
        TRevealBase asBase = baseHasStyledToString;

        var callBaseToString = 
            (PalantírReveal<TRevealBase>)
            nonVirtualToString.GetOrAdd(typeof(TRevealBase), _ => CreateInvokeMethod<TRevealBase>());

        callBaseToString(asBase, stsa);
    }

    public static void CallBaseStyledToStringIfSupported<T>(T baseHasStyledToString, ITheOneString stsa)
        where T : IStringBearer
    {
        Type baseType = typeof(T).BaseType!;

        var callBaseToString = (PalantírReveal<T>)nonVirtualToString.GetOrAdd(baseType, bType => CreateInvokeMethod(bType));

        callBaseToString(baseHasStyledToString, stsa);
    }

    private record TypeCallCount(Type _)
    {
        public int CallCount { get; set; }
    };

    private static ConcurrentDictionary<Type, TypeCallCount> noOpCalls = new();

    private static AppendSummary NoOpBaseDoesntSupportStyledToString<T>(T _, ITheOneString _1)
    {
        var callCount = noOpCalls.GetOrAdd(typeof(T), new TypeCallCount(typeof(T)));
        callCount.CallCount++;
        return new AppendSummary();
    }

    public static PalantírReveal<TBase> CreateInvokeMethod<TBase>() where TBase : class, IStringBearer
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
        var baseTypeCustomStyler = typeof(PalantírReveal<>).MakeGenericType(baseType);
        if (methodToCall == null || methodToCall.IsAbstract)
        {
            methodToCall = NoOpGenericMethodInfo;
            var genericMethod        = methodToCall!.MakeGenericMethod(baseType);
            return GetNonVirtualDispatchStyledToString(baseType, genericMethod, baseTypeCustomStyler);
        }

        var methodInvoker = GetNonVirtualDispatchStyledToString(baseType, methodToCall, baseTypeCustomStyler);
        return methodInvoker;
    }

    private static MethodInfo NoOpGenericMethodInfo
    {
        get
        {
            if (noOpGenericMethodInfo != null) return noOpGenericMethodInfo;
            var allMethods = typeof(TargetStringBearerRevealState).GetMethods(Static | NonPublic);
            for (var i = 0; i < allMethods.Length; i++)
            {
                var methodInfo = allMethods[i];
                Console.Out.WriteLine(methodInfo.Name);
                if (!methodInfo.IsGenericMethodDefinition) continue;
                if (!methodInfo.Name.StartsWith(nameof(NoOpBaseDoesntSupportStyledToString))) continue;
                noOpGenericMethodInfo = methodInfo;
                break;
            }
            return noOpGenericMethodInfo ?? throw new InvalidOperationException("Could not find the generic definition for " 
                                                                              + nameof(NoOpBaseDoesntSupportStyledToString)) ;
        }
    }

    // Created with help from https://blog.adamfurmanek.pl/2020/01/11/net-inside-out-part-14-calling-virtual-method-without-dynamic-dispatch/index.html
    // Full credit and thanks for posting 


    public static Delegate GetNonVirtualDispatchStyledToString(Type styledToStringType, MethodInfo methodToCall, Type customStyleType)
    {
        var helperMethod =
            new DynamicMethod($"{styledToStringType.Name}_Styled_ToString", typeof(AppendSummary),
                              [styledToStringType, typeof(ITheOneString)], styledToStringType.Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(customStyleType);
        return methodInvoker;
    }

    public static PalantírReveal<T> GetNonVirtualDispatchStyledToString<T>(MethodInfo methodToCall)
    where T : notnull
    {
        var methodInvoker = (PalantírReveal<T>)GetNonVirtualDispatchStyledToString(typeof(T), methodToCall, typeof(PalantírReveal<T>));
        return methodInvoker;
    }
}
