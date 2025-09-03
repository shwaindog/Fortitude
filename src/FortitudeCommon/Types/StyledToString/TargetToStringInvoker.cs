// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Types.StyledToString;

public static class TargetToStringInvoker
{
    private static ConcurrentDictionary<Type, Delegate> nonVirtualToString = new();

    private static readonly Type StyledStringAppenderType = typeof(IStyledTypeStringAppender);

    public static void CallBaseStyledToString<T, TBase>(T baseHasStyledToString, IStyledTypeStringAppender stsa)
        where T : TBase where TBase : class, IStyledToStringObject
    {
        TBase asBase = baseHasStyledToString;

        var callBaseToString = (CustomTypeStyler<TBase>)nonVirtualToString.GetOrAdd(typeof(TBase), bType => CreateInvokeMethod<TBase>());

        callBaseToString(asBase, stsa);
    }

    public static void CallBaseStyledToStringIfSupported<T>(T baseHasStyledToString, IStyledTypeStringAppender stsa)
        where T : IStyledToStringObject
    {
        Type baseType = typeof(T).BaseType!;

        var callBaseToString = (CustomTypeStyler<T>)nonVirtualToString.GetOrAdd(baseType, bType => CreateInvokeMethod(bType));

        callBaseToString(baseHasStyledToString, stsa);
    }

    private record TypeCallCount(Type ForType)
    {
        public int CallCount { get; set; }
    };

    private static ConcurrentDictionary<Type, TypeCallCount> noOpCalls = new();

    private static StyledTypeBuildResult NoOpBaseDoesntSupportStyledToString<T>(T noSupporting, IStyledTypeStringAppender stsa)
    {
        var callCount = noOpCalls.GetOrAdd(typeof(T), new TypeCallCount(typeof(T)));
        callCount.CallCount++;
        return new StyledTypeBuildResult();
    }

    public static CustomTypeStyler<TBase> CreateInvokeMethod<TBase>() where TBase : class, IStyledToStringObject
    {
        var methodToCall = typeof(TBase).GetMethod("ToString", [StyledStringAppenderType]);

        var methodInvoker = GetNonVirtualDispatchStyledToString<TBase>(methodToCall!);
        return methodInvoker;
    }

    public static Delegate CreateInvokeMethod(Type baseType)
    {
        var methodToCall = baseType.GetMethod("ToString", [StyledStringAppenderType]);

        var baseTypeCustomStyler = typeof(CustomTypeStyler<>).MakeGenericType(baseType);
        if (methodToCall == null || methodToCall.IsAbstract)
        {
            var myType         = typeof(TargetToStringInvoker);
            methodToCall = myType.GetMethod("NoOpBaseDoesntSupportStyledToString", [baseType, StyledStringAppenderType]);
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
            new DynamicMethod($"{styledToStringType.Name}_Styled_ToString", typeof(StyledTypeBuildResult),
                              [styledToStringType, typeof(IStyledTypeStringAppender)], styledToStringType.Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(customStyleType);
        return methodInvoker;
    }

    public static CustomTypeStyler<T> GetNonVirtualDispatchStyledToString<T>(MethodInfo methodToCall)
    {
        var methodInvoker = (CustomTypeStyler<T>)GetNonVirtualDispatchStyledToString(typeof(T), methodToCall, typeof(CustomTypeStyler<T>));
        return methodInvoker;
    }
}
