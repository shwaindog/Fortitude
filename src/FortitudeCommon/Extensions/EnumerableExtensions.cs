// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.Types;

namespace FortitudeCommon.Extensions;

public readonly struct StructEnumerable<TEnumbl, T>(TEnumbl wrapped) : IEnumerable<T>
where TEnumbl : IEnumerable<T>
{
    IEnumerator IEnumerable.GetEnumerator() => wrapped.GetEnumerator();
    public IEnumerator<T>   GetEnumerator() => wrapped.GetEnumerator();
}
public readonly struct StructEnumerator<TEnumtr, T>(TEnumtr wrapped) : IEnumerator<T>
where TEnumtr : IEnumerator<T>
{
    public bool MoveNext() => wrapped.MoveNext();

    public void Reset()
    {
        wrapped.MoveNext();
    }
    
    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        wrapped.Dispose();
    }

    public T Current => wrapped.Current;
}

public static class EnumerableExtensions
{
    private static readonly ConcurrentDictionary<Type, MethodInfo?> EnumerableToEnumeratorMethodInfoCache = new();
    
    private static readonly ConcurrentDictionary<Type, Type?>       EnumerableToEnumeratorTypeCache       = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> CompileEnumerableCount = new();

    public static StructEnumerable<TEnumbl, T> ToStructEnumerable<TEnumbl, T>(this TEnumbl wrapped) where TEnumbl : IEnumerable<T> => 
        new (wrapped);
    
    public static StructEnumerable<TEnumbl, T>? ToNullableStructEnumerable<TEnumbl, T>(this TEnumbl? wrapped) where TEnumbl : IEnumerable<T> => 
        wrapped != null ? new StructEnumerable<TEnumbl, T>(wrapped) : null;

    public static StructEnumerator<TEnumtr, T> ToStructEnumerator<TEnumtr, T>(this TEnumtr wrapped) where TEnumtr : IEnumerator<T> => 
        new (wrapped);
    
    public static StructEnumerator<TEnumtr, T>? ToNullableStructEnumerator<TEnumtr, T>(this TEnumtr? wrapped) where TEnumtr : IEnumerator<T> => 
        wrapped != null ? new StructEnumerator<TEnumtr, T>(wrapped) : null;

    public static Type? GetEnumeratorType(this Type enumerableType) =>
        EnumerableToEnumeratorTypeCache
            .GetOrAdd(enumerableType, static (enumblType) => enumblType.GetEnumeratorType());

    public static bool IsClassEnumerator(this Type enumerableType) => !(enumerableType.GetEnumeratorType()?.IsValueType ?? true);

    public static MethodInfo? GetEnumeratorMethodInfo(this Type enumerableType) =>
        EnumerableToEnumeratorMethodInfoCache
            .GetOrAdd(enumerableType, static (enumblType) => enumblType.GetEnumeratorMethod());

    public static MethodInfo? GetEnumeratorMoveNextMethodInfo(this Type enumeratorType) =>
        enumeratorType.GetMethod(nameof(IEnumerator.MoveNext), BindingFlags.Instance | BindingFlags.Public);

    public static bool IsStructEnumeratorWithEntries<TEnumbl>(this TEnumbl enumblToCount, Type? actualEnumblType = null)
        where TEnumbl : IEnumerable?
    {
        if (enumblToCount == null) return false;
        actualEnumblType ??= enumblToCount.GetType();
        var enumeratorType = actualEnumblType.GetEnumeratorType();
        if (!(enumeratorType?.IsValueType ?? false)) return false;
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker = (Func<TEnumbl?, int, int>)
        CompileEnumerableCount
                .GetOrAdd((enumblParamType, actualEnumblType, enumeratorType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              BuildGetStructEnumeratorAndCountUpToMaxLimitFunc<TEnumbl>(key.enumblType, key.enumblParamType, key.enumeratorType)
                        , callAsFactory);
        return invoker(enumblToCount, 1) > 0;
    }

    private static Func<TEnumbl?, int, int> BuildGetStructEnumeratorAndCountUpToMaxLimitFunc<TEnumbl>
        (Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var moveNextMethodInf = enumeratorType.GetEnumeratorMoveNextMethodInfo()!;

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(int),
                 [enumblParamType, typeof(int)]
               , typeof(EnumerableExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        ilGenerator.DeclareLocal(typeof(int));
        ilGenerator.DeclareLocal(typeof(bool));

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);
        
        ilGenerator.Emit(OpCodes.Ldc_I4_0);
        ilGenerator.Emit(OpCodes.Stloc_3);
        
        var loopBody = ilGenerator.DefineLabel();
        var nextIterationCheck = ilGenerator.DefineLabel();
        var nextIterationCompare = ilGenerator.DefineLabel();
        var toEndLoop = ilGenerator.DefineLabel();
        
        // Loop count
        ilGenerator.Emit(OpCodes.Br_S, nextIterationCheck);
        
        ilGenerator.MarkLabel(loopBody);
        // count++
        ilGenerator.Emit(OpCodes.Ldloc_3);
        ilGenerator.Emit(OpCodes.Ldc_I4_1);
        ilGenerator.Emit(OpCodes.Add);
        ilGenerator.Emit(OpCodes.Stloc_3);
        
        // while
        ilGenerator.MarkLabel(nextIterationCheck);
        // enumtr.MoveNext == true
        ilGenerator.Emit(OpCodes.Ldloca_S, 1);
        ilGenerator.Emit(OpCodes.Call, moveNextMethodInf);
        ilGenerator.Emit(OpCodes.Brfalse_S, toEndLoop);
        // count < maxCount
        ilGenerator.Emit(OpCodes.Ldloc_3);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Clt);
        
        ilGenerator.Emit(OpCodes.Br_S, nextIterationCompare);
        ilGenerator.MarkLabel(toEndLoop);
        ilGenerator.Emit(OpCodes.Ldc_I4_0);
        ilGenerator.MarkLabel(nextIterationCompare);
        // save load do next iteration
        ilGenerator.Emit(OpCodes.Stloc_S, 4);
        ilGenerator.Emit(OpCodes.Ldloc_S, 4);
        ilGenerator.Emit(OpCodes.Brtrue_S, loopBody);
        
        // return count
        ilGenerator.Emit(OpCodes.Ldloc_3);
        ilGenerator.Emit(OpCodes.Ret);
        
        var methodInvoker = helperMethod.CreateDelegate(typeof(Func<TEnumbl?, int, int>));
        return (Func<TEnumbl?, int, int>)methodInvoker;
    }
}
