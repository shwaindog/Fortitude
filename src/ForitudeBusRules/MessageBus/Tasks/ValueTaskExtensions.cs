﻿#region

using System.Linq.Expressions;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public static class ValueTaskExtensions
{
    public static Task<T> ToTask<T>(this ValueTask<T> valueTask) => TypedValueTaskExtensions<T>.ExtractTask(valueTask);

    public static IReusableMessageResponseSource<T>? ToReusableValueTaskSource<T>(this ValueTask<T> valueTask) =>
        TypedValueTaskExtensions<T>.ExtractReusableValueTaskSource(valueTask);

    public static string ValueTaskInternalsString<T>(this ValueTask<T> valueTask) =>
        TypedValueTaskExtensions<T>.ValueTaskObjAndTokenString(valueTask);

    private static class TypedValueTaskExtensions<T>
    {
        private static readonly Func<ValueTask<T>, object?> ExtractValueTaskObj;
        private static readonly Func<ValueTask<T>, short> ExtractValueTaskToken;

        static TypedValueTaskExtensions()
        {
            var paramValueTask = Expression.Parameter(typeof(ValueTask<T>));
            Expression mObj = Expression.Field(paramValueTask, "_obj");
            ExtractValueTaskObj = Expression.Lambda<Func<ValueTask<T>, object?>>(mObj, paramValueTask).Compile();

            Expression mToken = Expression.Field(paramValueTask, "_token");
            ExtractValueTaskToken = Expression.Lambda<Func<ValueTask<T>, short>>(mToken, paramValueTask).Compile();
        }

        // ReSharper disable once UnusedMember.Global
        public static IReusableMessageResponseSource<T>? ExtractReusableValueTaskSource(ValueTask<T> toConvert)
        {
            var obj = ExtractValueTaskObj(toConvert);
            if (obj is ReusableValueTaskSource<T> reusableObj) return reusableObj;

            return null;
        }

        public static Task<T> ExtractTask(ValueTask<T> toConvert)
        {
            var obj = ExtractValueTaskObj(toConvert);
            if (obj is IReusableMessageResponseSource<T> reusableObj) return reusableObj.AsTask;

            return toConvert.AsTask();
        }

        public static string ValueTaskObjAndTokenString(ValueTask<T> valueTask)
        {
            var obj = ExtractValueTaskObj(valueTask);
            var token = ExtractValueTaskToken(valueTask);
            return $"ValueTask<{typeof(T).Name}>(_obj: {obj}, _token: {token})";
        }
    }
}