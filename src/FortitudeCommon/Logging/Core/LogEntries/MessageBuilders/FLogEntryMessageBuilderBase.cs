// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public interface IFLogMessageBuilder : IRecyclableObject { }

public abstract partial class FLogEntryMessageBuilder : RecyclableObject, IFLogMessageBuilder
{
    private static readonly Type myType;

    private static readonly MethodInfo[] MyNonPubStaticMethods;

    protected FLogEntry LogEntry = null!;


    protected Action<IStringBuilder?> OnComplete = null!;

    static FLogEntryMessageBuilder()
    {
        myType = typeof(FLogEntryMessageBuilder);

        MyNonPubStaticMethods = myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
    }


    public ITheOneString Temp
    {
        get
        {
            var tempStsa = Recycler?.Borrow<TheOneString>().Initialize() ?? new TheOneString();
            return tempStsa;
        }
    }

    public virtual FLogEntryMessageBuilder Initialize(FLogEntry flogEntry, Action<IStringBuilder?> onCompleteHandler)
    {
        LogEntry   = flogEntry;
        OnComplete = onCompleteHandler;

        return this;
    }

    protected static void AppendStyled<TToStyle, TStylerType>((TToStyle, StringBearerRevealState<TStylerType>) valueTuple
      , ITheOneString ofPower) where TToStyle : TStylerType
    {
        var (value, structStyler) = valueTuple;
        structStyler(value, ofPower);
    }

    protected static void AppendFromRange((string?, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((string?, int, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((char[]?, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((char[]?, int, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((ICharSequence?, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((ICharSequence?, int, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((StringBuilder?, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((StringBuilder?, int, int) valueTuple, ITheOneString theOneString)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = theOneString.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendObject(object? value, ITheOneString theOneString)
    {
        theOneString.WriteBuffer.Append(value);
    }

    protected static void AppendSpanFormattable<TFmt>((TFmt, string) valueTuple, ITheOneString theOneString)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        theOneString.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendSpanFormattable<TFmtStruct>((TFmtStruct?, string) valueTuple, ITheOneString theOneString)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        if (value == null) return;
        theOneString.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendStyledObject(IStringBearer? styledObj, ITheOneString theOneString)
    {
        styledObj?.RevealState(theOneString);
    }

    protected void AppendMatchSelect<T>(T value, ITheOneString toAppendTo)
    {
        var stringBuilder = toAppendTo.WriteBuffer;
        switch (value)
        {
            case null:                   break;
            case bool valueBool:         stringBuilder.Append(valueBool); break;
            case byte valueByte:         stringBuilder.Append(valueByte); break;
            case sbyte valueSByte:       stringBuilder.Append(valueSByte); break;
            case char valueChar:         stringBuilder.Append(valueChar); break;
            case short valueShort:       stringBuilder.Append(valueShort); break;
            case ushort valueUShort:     stringBuilder.Append(valueUShort); break;
            case Half valueHalfFloat:    stringBuilder.Append(valueHalfFloat); break;
            case int valueInt:           stringBuilder.Append(valueInt); break;
            case uint valueUInt:         stringBuilder.Append(valueUInt); break;
            case nint valueNInt:         stringBuilder.Append(valueNInt); break;
            case float valueFloat:       stringBuilder.Append(valueFloat); break;
            case long valueLong:         stringBuilder.Append(valueLong); break;
            case ulong valueULong:       stringBuilder.Append(valueULong); break;
            case double valueDouble:     stringBuilder.Append(valueDouble); break;
            case decimal valueDecimal:   stringBuilder.Append(valueDecimal); break;
            case Int128 veryLongInt:     stringBuilder.Append(veryLongInt); break;
            case UInt128 veryLongUInt:   stringBuilder.Append(veryLongUInt); break;
            case BigInteger veryLongInt: stringBuilder.Append(veryLongInt); break;
            case Complex veryLongInt:    stringBuilder.Append(veryLongInt); break;
            case DateTime valueDateTime: stringBuilder.Append(valueDateTime); break;
            case DateOnly valueDateOnly: stringBuilder.Append(valueDateOnly); break;
            case TimeSpan valueTimeSpan: stringBuilder.Append(valueTimeSpan); break;
            case TimeOnly valueTimeSpan: stringBuilder.Append(valueTimeSpan); break;
            case Rune valueTimeSpan:     stringBuilder.Append(valueTimeSpan); break;
            case Guid valueGuid:         stringBuilder.Append(valueGuid); break;
            case IPNetwork valueIntPtr:  stringBuilder.Append(valueIntPtr); break;
            case string valueString:     stringBuilder.Append(valueString); break;
            case char[] valueCharArray:  stringBuilder.Append(valueCharArray); break;
            case Version valueGuid:      stringBuilder.AppendFormat("{0}", valueGuid); break;
            case IPAddress valueIntPtr:  stringBuilder.AppendFormat("{0}", valueIntPtr); break;
            case Uri valueUri:           stringBuilder.AppendFormat("{0}", valueUri); break;

            case DateTimeOffset valueDateTimeOffset:   stringBuilder.Append(valueDateTimeOffset); break;
            case ICharSequence valueCharSeq:           stringBuilder.Append(valueCharSeq); break;
            case StringBuilder valueStringBuilder:     stringBuilder.Append(valueStringBuilder); break;
            case IStringBearer valueStyledObj: AppendStyledObject(valueStyledObj, toAppendTo); break;

            default:
                if (value is ISpanFormattable spanFormattableValue) stringBuilder.AppendFormat("{0}", spanFormattableValue);
                BuildCacheCallGenericAppend(value, toAppendTo);
                break;
        }
    }

    protected void BuildCacheCallGenericAppend<T>(T value, ITheOneString toAppendTo)
    {
        if (value == null) return;
        var type = value.GetType();
        if (FLogStringSerializerRegistry.TryGetSerializerFor(type, out var regStringSerializer))
        {
            regStringSerializer.Invoke(value, toAppendTo);
            return;
        }
        if (type.IsCollection())
        {
            Action<T, ITheOneString>? matchedInvoker;
            matchedInvoker = CheckCollectionForInvoker(value, type);
            if (matchedInvoker != null)
            {
                FLogStringSerializerRegistry.AutoRegisterSerializerFor(matchedInvoker, LogEntry.LogLocation);
                matchedInvoker(value, toAppendTo);
                return;
            }
            matchedInvoker = CheckKeyedCollectionForInvoker(value, type);
            if (matchedInvoker != null)
            {
                FLogStringSerializerRegistry.AutoRegisterSerializerFor(matchedInvoker, LogEntry.LogLocation);
                matchedInvoker(value, toAppendTo);
                return;
            }
        }
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(ValueTuple<,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];

                var singleMatchedInvoker = CheckSingleFieldFor2ItemTupleInvoker(value, type, item1Type, item2Type);
                if (singleMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(singleMatchedInvoker, LogEntry.LogLocation);
                    singleMatchedInvoker(value, toAppendTo);
                    return;
                }
                var collectionMatchedInvoker = CheckCollectionFor2ItemTupleInvoker(value, type, item1Type, item2Type);
                if (collectionMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(collectionMatchedInvoker, LogEntry.LogLocation);
                    collectionMatchedInvoker(value, toAppendTo);
                    return;
                }
                var keyedCollectionMatchedInvoker = CheckKeyedCollectionFor2ItemTupleInvoker(value, type, item1Type, item2Type);
                if (keyedCollectionMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(keyedCollectionMatchedInvoker, LogEntry.LogLocation);
                    keyedCollectionMatchedInvoker(value, toAppendTo);
                    return;
                }
            }
            else if (genericType == typeof(ValueTuple<,,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];
                var item3Type = arguments[2];

                var singleMatchedInvoker = CheckSingleFieldFor3ItemTupleInvoker(value, type, item1Type, item2Type, item3Type);
                if (singleMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(singleMatchedInvoker, LogEntry.LogLocation);
                    singleMatchedInvoker(value, toAppendTo);
                    return;
                }
                var collectionMatchedInvoker = CheckCollectionFor3ItemTupleInvoker(value, type, item1Type, item2Type, item3Type);
                if (collectionMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(collectionMatchedInvoker, LogEntry.LogLocation);
                    collectionMatchedInvoker(value, toAppendTo);
                    return;
                }
                var keyedCollectionMatchedInvoker = CheckKeyedCollectionFor3ItemTupleInvoker(value, type, item1Type, item2Type, item3Type);
                if (keyedCollectionMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(keyedCollectionMatchedInvoker, LogEntry.LogLocation);
                    keyedCollectionMatchedInvoker(value, toAppendTo);
                    return;
                }
            }
            else if (genericType == typeof(ValueTuple<,,,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];
                var item3Type = arguments[2];
                var item4Type = arguments[3];

                var singleMatchedInvoker = CheckKeyedCollectionFor4ItemTupleInvoker(value, type, item1Type, item2Type, item3Type, item4Type);
                if (singleMatchedInvoker != null)
                {
                    FLogStringSerializerRegistry.AutoRegisterSerializerFor(singleMatchedInvoker, LogEntry.LogLocation);
                    singleMatchedInvoker(value, toAppendTo);
                    return;
                }
            }
        }
        var unknownAppender = AppendObject;

        CreateWarningMessageAppender()?
            .Append("Created auto serializer call object for type '").Append(type.Name).Append("' at ").FinalAppendObject(LogEntry.LogLocation);

        FLogStringSerializerRegistry.AutoRegisterSerializerFor(unknownAppender, LogEntry.LogLocation);
        unknownAppender(value, toAppendTo);
    }

    protected IFLogStringAppender CreateWarningMessageAppender(FLogLevel warningLevel = FLogLevel.Trace)
    {
        var context                     = FLogContext.Context;
        var contextLogEntryPoolRegistry = context.LogEntryPoolRegistry;
        var globalLogEntryPoolConfig    = contextLogEntryPoolRegistry.LogEntryPoolInitConfig.GlobalLogEntryPool;
        var logCreatedObjEntry          = contextLogEntryPoolRegistry.ResolveFLogEntryPool(globalLogEntryPoolConfig);

        var failAppender = context.AppenderRegistry.FailAppender;

        var objCreatedLogEntry = logCreatedObjEntry.SourceLogEntry();
        objCreatedLogEntry.Initialize(new LoggerEntryContext(LogEntry.Logger, failAppender, LogEntry.LogLocation, warningLevel));
        return objCreatedLogEntry.StringAppender();
    }

    protected Action<TToAppend, ITheOneString> CreateSingleGenericArgMethodInvoker<TToAppend>
        (TToAppend toAppend, Type appendType, MethodInfo genDefMethodInfo, Type methodGenericArg)
    {
        var tupleParameter           = Expression.Parameter(appendType, "firstParam");
        var styledTypeStringAppender = Expression.Parameter(typeof(ITheOneString), "typeStringAppenderSecondParam");

        var typedInvokeMethod = genDefMethodInfo.MakeGenericMethod(methodGenericArg);

        var callMethodExpression = Expression.Call(typedInvokeMethod, tupleParameter, styledTypeStringAppender);

        var invoke =
            Expression.Lambda<Action<TToAppend, ITheOneString>>(callMethodExpression, tupleParameter, styledTypeStringAppender).Compile();
        return invoke;
    }

    protected Action<TToAppend, ITheOneString> CreateTwoGenericArgsMethodInvoker<TToAppend>
        (TToAppend toAppend, Type appendType, MethodInfo genDefMethodInfo, Type firstGenericArg, Type secondGenericArg)
    {
        var tupleParameter           = Expression.Parameter(appendType, "firstParam");
        var styledTypeStringAppender = Expression.Parameter(typeof(ITheOneString), "typeStringAppenderSecondParam");

        var typedInvokeMethod = genDefMethodInfo.MakeGenericMethod(firstGenericArg, secondGenericArg);

        var callMethodExpression = Expression.Call(typedInvokeMethod, tupleParameter, styledTypeStringAppender);

        var invoke =
            Expression.Lambda<Action<TToAppend, ITheOneString>>(callMethodExpression, tupleParameter, styledTypeStringAppender).Compile();
        return invoke;
    }
}

public interface IMessageBuilderAppendChecks<out TMessageEntry>
{
    ITheOneString? ContinueOnReceivingStringAppender<T>(T param, [CallerMemberName] string memberName = "");
    TMessageEntry?             PostAppendContinue<T>(ITheOneString? justAppended, T param, [CallerMemberName] string memberName = "");
}

public abstract partial class FLogEntryMessageBuilderBase<TIMsgBuild, TMsgBuilderImp> : FLogEntryMessageBuilder
  , IMessageBuilderAppendChecks<TIMsgBuild>
    where TMsgBuilderImp : FLogEntryMessageBuilderBase<TIMsgBuild, TMsgBuilderImp>
    where TIMsgBuild : class, IFLogMessageBuilder
{
    protected readonly IStringBuilder Warnings = new MutableString();

    protected bool NextPostAppendIsLast;

    protected FLogEntryMessageBuilderBase() { }

    protected FLogEntryMessageBuilderBase(FLogEntryMessageBuilderBase<TIMsgBuild, TMsgBuilderImp> toClone)
    {
        Warnings.Clear();
        Warnings.AppendRange(toClone.Warnings);
        OnComplete = toClone.OnComplete;
    }

    ITheOneString? IMessageBuilderAppendChecks<TIMsgBuild>.ContinueOnReceivingStringAppender<T>
        (T param, string memberName) =>
        PreappendCheckGetStringAppender(param, memberName);

    TIMsgBuild? IMessageBuilderAppendChecks<TIMsgBuild>.PostAppendContinue<T>(ITheOneString? justAppended, T param,
        string memberName) =>
        PostAppendContinueOnMessageEntry(justAppended, param, memberName);

    public override TMsgBuilderImp Initialize(FLogEntry flogEntry, Action<IStringBuilder?> onCompleteHandler)
    {
        base.Initialize(flogEntry, onCompleteHandler);

        return (TMsgBuilderImp)this;
    }

    public override void StateReset()
    {
        Warnings.Clear();
        OnComplete = null!;

        base.StateReset();
    }

    protected abstract ITheOneString? PreappendCheckGetStringAppender<T>(T param, [CallerMemberName] string memberName = "");

    protected abstract TIMsgBuild? PostAppendContinueOnMessageEntry<T>(ITheOneString? justAppended,
        T param, [CallerMemberName] string memberName = "");
}

public static class MessageBuilderExtensions
{
    public static bool IsFormatterType(this Type checkIsFormatterType) =>
        checkIsFormatterType == typeof(string) || (checkIsFormatterType.IsGenericType
                                                && checkIsFormatterType.GetGenericTypeDefinition() == typeof(StringBearerRevealState<>));

    public static bool IsOrderedCollectionFilterPredicate(this Type checkIsFormatterType) =>
        checkIsFormatterType.IsGenericType && checkIsFormatterType.GetGenericTypeDefinition() == typeof(OrderedCollectionPredicate<>);

    public static bool IsKeyValueFilterPredicate(this Type checkIsFormatterType) =>
        checkIsFormatterType.IsGenericType && checkIsFormatterType.GetGenericTypeDefinition() == typeof(KeyValuePredicate<,>);

    public static bool IsValidFormatterForType(this Type baseType, Type formatterType) =>
        (formatterType == typeof(string) && (!baseType.IsValueType
                                          || baseType.GetInterfaces().Any(i => i == typeof(ISpanFormattable))))
     || (baseType.IsValueType && formatterType.IsGenericType
                              && formatterType.GetGenericTypeDefinition() == typeof(StringBearerRevealState<>)
                              && formatterType.GenericTypeArguments[0].IsAssignableFrom(baseType));
}
