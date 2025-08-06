using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface IFLogMessageBuilder : IReusableObject<IFLogMessageBuilder>
{
    IStyledTypeStringAppender StyledTypeAppender { get; }

    IStringBuilder WriteBuffer { get; }
}

public abstract class FLogEntryMessageBuilderBase<TMessageEntry> : ReusableObject<IFLogMessageBuilder>, IFLogMessageBuilder
    where TMessageEntry : FLogEntryMessageBuilderBase<TMessageEntry>
{
    protected IStyledTypeStringAppender? Stsa;

    protected IStringBuilder Sb = null!;

    protected readonly IStringBuilder Warnings = new MutableString();

    protected Action<IStringBuilder?> OnComplete = null!;

    protected FLogEntry LogEntry = null!;

    private static readonly Type[] SupportedRangeIndexTypes =
    [
        typeof(string)
      , typeof(char[])
      , typeof(ICharSequence)
      , typeof(StringBuilder)
    ];

    private bool IsSupportedRangeType(Type checkIsSupported)
    {
        for (int i = 0; i < SupportedRangeIndexTypes.Length; i++)
        {
            if (SupportedRangeIndexTypes[i] == checkIsSupported) return true;
        }
        return false;
    }

    protected FLogEntryMessageBuilderBase() { }

    protected FLogEntryMessageBuilderBase(FLogEntryMessageBuilderBase<TMessageEntry> toClone)
    {
        Stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender());
        Stsa.CopyFrom(toClone.Stsa!);
        Sb = Stsa.WriteBuffer;
        Warnings.Clear();
        Warnings.AppendRange(toClone.Warnings);
        OnComplete       = toClone.OnComplete;
    }

    public TMessageEntry Initialize
        (FLogEntry flogEntry, Action<IStringBuilder?> onCompleteHandler, IStyledTypeStringAppender? styledTypeStringAppender = null)
    {
        Stsa ??= (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender()).Initialize(flogEntry.Style);
        Sb   =   Stsa.WriteBuffer;

        OnComplete = onCompleteHandler;

        return (TMessageEntry)this;
    }

    public IStringBuilder WriteBuffer => Sb;
    
    public IStyledTypeStringAppender StyledTypeAppender
    {
        get
        {
            Stsa ??= Recycler?.Borrow<StyledTypeStringAppender>().Initialize() ?? new StyledTypeStringAppender();
            Sb   =   Stsa.WriteBuffer;
            return Stsa;
        }
    }

    protected static void AppendStruct<TStruct>((TStruct, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, structStyler) = valueTuple;
        structStyler(value, appender);
    }

    protected static void AppendStruct<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, structStyler) = valueTuple;
        if (value != null) structStyler(value.Value, appender);
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

    protected static void AppendFormattedNumber<TNum>((TNum, string) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
        where TNum : struct, INumber<TNum>
    {
        var (value, formatString) = valueTuple;
        styledTypeStringAppender.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendFormattedNumber<TNum>((TNum?, string) valueTuple, IStyledTypeStringAppender styledTypeStringAppender)
        where TNum : struct, INumber<TNum>
    {
        var (value, formatString) = valueTuple;
        if (value == null) return;
        styledTypeStringAppender.WriteBuffer.AppendFormat(formatString, value);
    }

    protected static void AppendStyledObject(IStyledToStringObject? styledObj, IStyledTypeStringAppender styledTypeStringAppender)
    {
        styledObj?.ToString(styledTypeStringAppender);
    }

    protected void AppendMatchSelect<T>(T value, IStyledTypeStringAppender toAppendTo)
    {
        var stringBuilder = toAppendTo.WriteBuffer;
        switch (value)
        {
            case null:                  break;
            case bool valueBool:        stringBuilder.Append(valueBool); break;
            case byte valueByte:        stringBuilder.Append(valueByte); break;
            case sbyte valueSByte:      stringBuilder.Append(valueSByte); break;
            case char valueChar:        stringBuilder.Append(valueChar); break;
            case short valueShort:      stringBuilder.Append(valueShort); break;
            case ushort valueUShort:    stringBuilder.Append(valueUShort); break;
            case int valueInt:          stringBuilder.Append(valueInt); break;
            case uint valueUInt:        stringBuilder.Append(valueUInt); break;
            case nint valueNInt:        stringBuilder.Append(valueNInt); break;
            case float valueFloat:      stringBuilder.Append(valueFloat); break;
            case long valueLong:        stringBuilder.Append(valueLong); break;
            case ulong valueULong:      stringBuilder.Append(valueULong); break;
            case double valueDouble:    stringBuilder.Append(valueDouble); break;
            case decimal valueDecimal:  stringBuilder.Append(valueDecimal); break;
            case string valueString:    stringBuilder.Append(valueString); break;
            case char[] valueCharArray: stringBuilder.Append(valueCharArray); break;

            case ICharSequence valueCharSeq:           stringBuilder.Append(valueCharSeq); break;
            case StringBuilder valueStringBuilder:     stringBuilder.Append(valueStringBuilder); break;
            case IStyledToStringObject valueStyledObj: AppendStyledObject(valueStyledObj, Stsa!); break;
            default:
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
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(ValueTuple<,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];

                if (item2Type.IsGenericType)
                {
                    var item2GenericType = item2Type.GetGenericTypeDefinition();
                    if (item2GenericType == typeof(StructStyler<>))
                    {
                        var genericAppendAction = TryBuildAppendStructInvoker(value, type, item1Type, item2Type);
                        genericAppendAction(value, toAppendTo);
                        return;
                    }
                }
                else if (item1Type.IsNumericType() && item2Type == typeof(string))
                {
                    var genericAppendAction = TryBuildNumberFormatInvoker(value, type, item1Type, item2Type);
                    genericAppendAction(value, toAppendTo);
                    return;
                }
                else if (IsSupportedRangeType(item1Type) && item2Type == typeof(int))
                {
                    var genericAppendAction = TryBuildAppendRangeFromInvoker(value, type, item1Type, item2Type);
                    genericAppendAction(value, toAppendTo);
                    return;
                }
            }
            else if (genericType == typeof(ValueTuple<,,>))
            {
                var arguments = type.GenericTypeArguments;
                var item1Type = arguments[0];
                var item2Type = arguments[1];
                var item3Type = arguments[2];
                if (IsSupportedRangeType(item1Type) && item2Type == typeof(int) && item3Type == typeof(int))
                {
                    var genericAppendAction = TryBuildAppendRangeFromToInvoker(value, type, item1Type, item2Type, item3Type);
                    genericAppendAction(value, toAppendTo);
                    return;
                }
            }
        }
        Action<object?, IStyledTypeStringAppender> unknownAppender = AppendObject;

        var context                     = FLogContext.Context;
        var contextLogEntryPoolRegistry = context.LogEntryPoolRegistry;
        var globalLogEntryPoolConfig    = contextLogEntryPoolRegistry.LogEntryPoolInitConfig.GlobalLogEntryPool;
        var logCreatedObjEntry          = contextLogEntryPoolRegistry.ResolveFLogEntryPool(globalLogEntryPoolConfig);

        var failAppender = context.AppenderRegistry.FailAppender;

        var objCreatedLogEntry = logCreatedObjEntry.SourceLogEntry();
        objCreatedLogEntry.Initialize(new LoggerEntryContext(LogEntry.Logger, failAppender, LogEntry.LogLocation, FLogLevel.Trace));
        objCreatedLogEntry.StringAppender().Append("Created auto serializer call object for type '")
                       .Append(type.Name).Append("' at ").FinalAppend(LogEntry.LogLocation);

        FLogStringSerializerRegistry.AutoRegisterSerializerFor(unknownAppender, LogEntry.LogLocation);
        unknownAppender(value, toAppendTo);
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildNumberFormatInvoker<T>(T toAppend, Type typeOfT, Type item1Type, Type item2Type)
    {
        var myType = GetType();

        var tupleParameter           = Expression.Parameter(typeOfT, "genericTuple");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppender");

        var invokeMethodDef =
            myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                  .Single(mi =>
                  {
                      var methodParams   = mi.GetParameters();
                      var firstParamType = methodParams[0].ParameterType;
                      return mi is { Name: nameof(AppendFormattedNumber), IsStatic: true }
                          && methodParams.Length == 2
                          && item1Type == firstParamType
                          && methodParams[1].ParameterType.GetGenericTypeDefinition() == typeof(int);
                  }) ??
            throw new InvalidOperationException("Method does not exist");

        var invokeMethod = invokeMethodDef.MakeGenericMethod(item1Type);

        var block = Expression.Block
            (
             [tupleParameter, styledTypeStringAppender],
             Expression.Call(invokeMethod, tupleParameter, styledTypeStringAppender)
            );

        Expression<Action<T, IStyledTypeStringAppender>> invoke =
            Expression.Lambda<Action<T, IStyledTypeStringAppender>>(block, tupleParameter, styledTypeStringAppender);
        var invokeAppend = invoke.Compile();
        FLogStringSerializerRegistry.AutoRegisterSerializerFor(invokeAppend, LogEntry.LogLocation);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildAppendRangeFromToInvoker<T>(T toAppend, Type typeOfT, Type item1Type, Type item2Type, Type item3Type)
    {
        var myType = GetType();

        var tupleParameter           = Expression.Parameter(typeOfT, "genericTuple");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppender");

        var invokeMethodDef =
            myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                  .Single(mi =>
                  {
                      var methodParams   = mi.GetParameters();
                      var firstParamType = methodParams[0].ParameterType;
                      return mi is { Name: nameof(AppendFromToRange), IsStatic: true }
                          && methodParams.Length == 3
                          && item1Type == firstParamType
                          && methodParams[1].ParameterType.GetGenericTypeDefinition() == typeof(int)
                          && methodParams[2].ParameterType.GetGenericTypeDefinition() == typeof(int);
                  }) ??
            throw new InvalidOperationException("Method does not exist");

        var invokeMethod = invokeMethodDef.MakeGenericMethod(item1Type);

        var block = Expression.Block
            (
             [tupleParameter, styledTypeStringAppender],
             Expression.Call(invokeMethod, tupleParameter, styledTypeStringAppender)
            );

        Expression<Action<T, IStyledTypeStringAppender>> invoke =
            Expression.Lambda<Action<T, IStyledTypeStringAppender>>(block, tupleParameter, styledTypeStringAppender);
        var invokeAppend = invoke.Compile();
        FLogStringSerializerRegistry.AutoRegisterSerializerFor(invokeAppend, LogEntry.LogLocation);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildAppendRangeFromInvoker<T>(T toAppend, Type typeOfT, Type item1Type, Type item2Type)
    {
        var myType = GetType();

        var tupleParameter           = Expression.Parameter(typeOfT, "genericTuple");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppender");

        var invokeMethodDef =
            myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                  .Single(mi =>
                  {
                      var methodParams   = mi.GetParameters();
                      var firstParamType = methodParams[0].ParameterType;
                      return mi is { Name: nameof(AppendFromRange), IsStatic: true }
                          && methodParams.Length == 2
                          && item1Type == firstParamType
                          && methodParams[1].ParameterType.GetGenericTypeDefinition() == typeof(int);
                  }) ??
            throw new InvalidOperationException("Method does not exist");

        var invokeMethod = invokeMethodDef.MakeGenericMethod(item1Type);

        var block = Expression.Block
            (
             [tupleParameter, styledTypeStringAppender],
             Expression.Call(invokeMethod, tupleParameter, styledTypeStringAppender)
            );

        Expression<Action<T, IStyledTypeStringAppender>> invoke =
            Expression.Lambda<Action<T, IStyledTypeStringAppender>>(block, tupleParameter, styledTypeStringAppender);
        var invokeAppend = invoke.Compile();
        FLogStringSerializerRegistry.AutoRegisterSerializerFor(invokeAppend, LogEntry.LogLocation);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildAppendStructInvoker<T>(T toAppend, Type typeOfT, Type item1Type, Type item2Type)
    {
        var myType = GetType();

        var tupleParameter           = Expression.Parameter(typeOfT, "genericTuple");
        var styledTypeStringAppender = Expression.Parameter(typeof(IStyledTypeStringAppender), "typeStringAppender");

        var item1IsNullable = item1Type.IsGenericType && item1Type.GetGenericTypeDefinition() == typeof(Nullable<>);

        var invokeMethodDef =
            myType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                  .Single(mi =>
                  {
                      var genericParams  = mi.GetGenericArguments();
                      var methodParams   = mi.GetParameters();
                      var firstParamType = methodParams[0].ParameterType;
                      var firstParamIsNullAbleStruct =
                          firstParamType.IsGenericType
                       && firstParamType.GetGenericTypeDefinition() == typeof(Nullable<>);
                      return mi is { Name: nameof(AppendStruct), IsStatic: true }
                          && methodParams.Length == 2
                          && genericParams[0] == firstParamType
                          && firstParamIsNullAbleStruct == item1IsNullable
                          && methodParams[1].ParameterType.GetGenericTypeDefinition() ==
                             typeof(StructStyler<>);
                  }) ??
            throw new InvalidOperationException("Method does not exist");

        var invokeMethod = invokeMethodDef.MakeGenericMethod(item1Type);

        var block = Expression.Block
            (
             [tupleParameter, styledTypeStringAppender],
             Expression.Call(invokeMethod, tupleParameter, styledTypeStringAppender)
            );

        Expression<Action<T, IStyledTypeStringAppender>> invoke =
            Expression.Lambda<Action<T, IStyledTypeStringAppender>>(block, tupleParameter, styledTypeStringAppender);
        var invokeAppend = invoke.Compile();
        FLogStringSerializerRegistry.AutoRegisterSerializerFor(invokeAppend, LogEntry.LogLocation);
        return invokeAppend;
    }

    public override void StateReset()
    {
        Stsa?.DecrementRefCount();
        Stsa = null;
        Sb   = null!;
        Warnings.Clear();
        OnComplete    = null!;

        base.StateReset();
    }
}