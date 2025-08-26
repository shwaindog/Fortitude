using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public interface IFLogMessageBuilder : IReusableObject<IFLogMessageBuilder> { }

public abstract partial class FLogEntryMessageBuilder : ReusableObject<IFLogMessageBuilder>, IFLogMessageBuilder
{
    private static readonly Type myType;

    private static readonly MethodInfo[] MyNonPubStaticMethods;


    protected Action<IStringBuilder?> OnComplete = null!;

    protected FLogEntry LogEntry = null!;

    static FLogEntryMessageBuilder()
    {
        myType = typeof(FLogEntryMessageBuilder);

        MyNonPubStaticMethods = myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
    }


    public virtual FLogEntryMessageBuilder Initialize(FLogEntry flogEntry, Action<IStringBuilder?> onCompleteHandler)
    {
        LogEntry   = flogEntry;
        OnComplete = onCompleteHandler;

        return this;
    }

    protected static void AppendStyled<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender appender)
        where TToStyle : TStylerType
    {
        var (value, structStyler) = valueTuple;
        structStyler(value, appender);
    }

    protected static void AppendFromRange((string?, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((string?, int, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((char[]?, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((char[]?, int, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((ICharSequence?, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((ICharSequence?, int, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromRange((StringBuilder?, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Max(0, value.Length - startIndex);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendFromToRange((StringBuilder?, int, int) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
    {
        var (value, startIndex, count) = valueTuple;
        if (value == null) return;
        var cappedLength = Math.Min(Math.Max(0, value.Length - startIndex), count);
        var sb           = styledTypeStringAppender.WriteBuffer;
        sb.Append(value, startIndex, cappedLength);
    }

    protected static void AppendObject(object? value, IStyledTypeStringAppender styledTypeStringAppender)
    {
        styledTypeStringAppender.WriteBuffer.Append(value);
    }

    protected static void AppendSpanFormattable<TFmtStruct>((TFmtStruct, string) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        styledTypeStringAppender.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendSpanFormattable<TFmtStruct>((TFmtStruct?, string) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        if (value == null) return;
        styledTypeStringAppender.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendStyledObject(IStyledToStringObject? styledObj, IStyledTypeStringAppender styledTypeStringAppender)
    {
        styledObj?.ToString(styledTypeStringAppender);
    }


    public IStyledTypeStringAppender TempStyledTypeAppender
    {
        get
        {
            var tempStsa = Recycler?.Borrow<StyledTypeStringAppender>().Initialize() ?? new StyledTypeStringAppender();
            return tempStsa;
        }
    }

    protected void AppendMatchSelect<T>(T value, IStyledTypeStringAppender toAppendTo)
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
            case Version valueGuid:      stringBuilder.AppendSpanFormattable("{0}", valueGuid); break;
            case IPAddress valueIntPtr:  stringBuilder.AppendSpanFormattable("{0}", valueIntPtr); break;
            case Uri valueUri:           stringBuilder.AppendSpanFormattable("{0}", valueUri); break;

            case DateTimeOffset valueDateTimeOffset:   stringBuilder.Append(valueDateTimeOffset); break;
            case ICharSequence valueCharSeq:           stringBuilder.Append(valueCharSeq); break;
            case StringBuilder valueStringBuilder:     stringBuilder.Append(valueStringBuilder); break;
            case IStyledToStringObject valueStyledObj: AppendStyledObject(valueStyledObj, toAppendTo); break;

            default:
                if (value is ISpanFormattable spanFormattableValue)
                {
                    stringBuilder.AppendSpanFormattable("{0}", spanFormattableValue);
                }
                BuildCacheCallGenericAppend(value, toAppendTo);
                break;
        }
    }

    protected void BuildCacheCallGenericAppend<T>(T value, IStyledTypeStringAppender toAppendTo)
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
            Action<T, IStyledTypeStringAppender>? matchedInvoker;
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
        Action<object?, IStyledTypeStringAppender> unknownAppender = AppendObject;

        CreateWarningMessageAppender()?
            .Append("Created auto serializer call object for type '").Append(type.Name).Append("' at ").FinalAppend(LogEntry.LogLocation);

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

    protected Action<TToAppend, IStyledTypeStringAppender> CreateSingleGenericArgMethodInvoker<TToAppend>
        (TToAppend toAppend, Type appendType, MethodInfo genDefMethodInfo, Type methodGenericArg)
    {
        var tupleParameter           = Expression.Parameter(appendType, "firstParam");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppenderSecondParam");

        var typedInvokeMethod = genDefMethodInfo.MakeGenericMethod(methodGenericArg);

        var callMethodExpression = Expression.Call(typedInvokeMethod, tupleParameter, styledTypeStringAppender);

        var invoke =
            Expression.Lambda<Action<TToAppend, IStyledTypeStringAppender>>(callMethodExpression, tupleParameter, styledTypeStringAppender).Compile();
        return invoke;
    }

    protected Action<TToAppend, IStyledTypeStringAppender> CreateTwoGenericArgsMethodInvoker<TToAppend>
        (TToAppend toAppend, Type appendType, MethodInfo genDefMethodInfo, Type firstGenericArg, Type secondGenericArg)
    {
        var tupleParameter           = Expression.Parameter(appendType, "firstParam");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppenderSecondParam");

        var typedInvokeMethod = genDefMethodInfo.MakeGenericMethod(firstGenericArg, secondGenericArg);

        var callMethodExpression = Expression.Call(typedInvokeMethod, tupleParameter, styledTypeStringAppender);

        var invoke =
            Expression.Lambda<Action<TToAppend, IStyledTypeStringAppender>>(callMethodExpression, tupleParameter, styledTypeStringAppender).Compile();
        return invoke;
    }
}

public abstract partial class FLogEntryMessageBuilderBase<TMessageEntry> : FLogEntryMessageBuilder
    where TMessageEntry : FLogEntryMessageBuilderBase<TMessageEntry>
{
    protected readonly IStringBuilder Warnings = new MutableString();


    protected FLogEntryMessageBuilderBase() { }

    protected FLogEntryMessageBuilderBase(FLogEntryMessageBuilderBase<TMessageEntry> toClone)
    {
        Warnings.Clear();
        Warnings.AppendRange(toClone.Warnings);
        OnComplete = toClone.OnComplete;
    }

    public override TMessageEntry Initialize(FLogEntry flogEntry, Action<IStringBuilder?> onCompleteHandler)
    {
        base.Initialize(flogEntry, onCompleteHandler);

        return (TMessageEntry)this;
    }

    public override void StateReset()
    {
        Warnings.Clear();
        OnComplete = null!;

        base.StateReset();
    }
}

public static class MessageBuilderExtensions
{
    public static bool IsFormatterType(this Type checkIsFormatterType) =>
        checkIsFormatterType == typeof(string) ||
        (checkIsFormatterType.IsGenericType
      && checkIsFormatterType.GetGenericTypeDefinition() == typeof(CustomTypeStyler<>));

    public static bool IsOrderedCollectionFilterPredicate(this Type checkIsFormatterType) =>
        (checkIsFormatterType.IsGenericType
      && checkIsFormatterType.GetGenericTypeDefinition() == typeof(OrderedCollectionPredicate<>));

    public static bool IsKeyValueFilterPredicate(this Type checkIsFormatterType) =>
        (checkIsFormatterType.IsGenericType
      && checkIsFormatterType.GetGenericTypeDefinition() == typeof(KeyValuePredicate<,>));

    public static bool IsValidFormatterForType(this Type baseType, Type formatterType) =>
        formatterType == typeof(string) && (!baseType.IsValueType || baseType.GetInterfaces().Any(i => i == typeof(ISpanFormattable))) ||
        baseType.IsValueType && (formatterType.IsGenericType
                              && formatterType.GetGenericTypeDefinition() == typeof(CustomTypeStyler<>)
                              && formatterType.GenericTypeArguments[0].IsAssignableFrom(baseType));
}
