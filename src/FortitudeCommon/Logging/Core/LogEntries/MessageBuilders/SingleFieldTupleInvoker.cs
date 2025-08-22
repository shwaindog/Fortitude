using System.Reflection;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    private static readonly Type TypeOfChar  = typeof(char);
    private static readonly Type TypeOfArray = typeof(Array);


    private static readonly Type[] SupportedRangeIndexTypes =
    [
        typeof(string)
      , typeof(char[])
      , typeof(ICharSequence)
      , typeof(StringBuilder)
    ];

    private bool IsSupportedRangeSingleFieldType(Type checkIsSupported)
    {
        for (int i = 0; i < SupportedRangeIndexTypes.Length; i++)
        {
            if (SupportedRangeIndexTypes[i] == checkIsSupported) return true;
        }
        return false;
    }

    Action<T, IStyledTypeStringAppender>? CheckSingleFieldFor2ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type)
    {
        if (TypeOfArray.IsAssignableFrom(item1Type) && item1Type.GetElementType() != TypeOfChar)
        {
            return null;
        }

        if (item2Type.IsGenericType)
        {
            var item2GenericType = item2Type.GetGenericTypeDefinition();
            if (item1Type.IsValueType && item2GenericType == typeof(CustomTypeStyler<>))
            {
                var structStylerInvoker = TryBuildSingleFieldStructStylerInvoker(tuple, tupleType, item1Type, item2Type);
                return structStylerInvoker;
            }
        }
        else if (item1Type.IsSpanFormattable() && item2Type == typeof(string))
        {
            var spanFormattableInvoker = TryBuildSingleFieldSpanFormattableInvoker(tuple, tupleType, item1Type, item2Type);
            return spanFormattableInvoker;
        }
        else if (IsSupportedRangeSingleFieldType(item1Type) && item2Type == typeof(int))
        {
            var indexRangeInvoker = TryBuildSingleFieldAppendRangeInvoker(tuple, tupleType, item1Type, item2Type);
            return indexRangeInvoker;
        }
        return null;
    }

    Action<T, IStyledTypeStringAppender>? CheckSingleFieldFor3ItemTupleInvoker<T>(T toAppend, Type typeOfT
      , Type item1Type, Type item2Type, Type item3Type)
    {
        if (TypeOfArray.IsAssignableFrom(item1Type) && item1Type.GetElementType() != TypeOfChar)
        {
            return null;
        }

        if (IsSupportedRangeSingleFieldType(item1Type) && item2Type == typeof(int) && item3Type == typeof(int))
        {
            var genericAppendAction = TryBuildSingleFieldAppendRangeFromToInvoker(toAppend, typeOfT, item1Type, item2Type, item3Type);
            return genericAppendAction;
        }
        return null;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildSingleFieldSpanFormattableInvoker<T>(T toAppend, Type tupleType, Type item1Type
      , Type item2Type)
    {
        var         item1IsNullable = item1Type.IsGenericType && item1Type.GetGenericTypeDefinition() == typeof(Nullable<>);
        
        MethodInfo? foundMatch      = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];

            if (mi.Name != nameof(AppendSpanFormattable) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            if (methodParams[1].ParameterType != typeof(IStyledTypeStringAppender)) continue;
            var firstParamType = methodParams[0].ParameterType;
            var fpItem1Type    = firstParamType.GenericTypeArguments[0];
            var fpItem2Type    = firstParamType.GenericTypeArguments[1];
            if (fpItem2Type.IsNotString()) continue;
            if(!firstParamType.IsGenericType || firstParamType.GetGenericTypeDefinition() != typeof(ValueTuple<,>) ) continue;
            var p1Item1IsNullable = fpItem1Type.IsGenericType && fpItem1Type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if(p1Item1IsNullable != item1IsNullable) continue;
            var p1IsSpanFormattable = fpItem1Type.IsSpanFormattable();
            if (p1Item1IsNullable)
            {
                p1IsSpanFormattable = firstParamType.GenericTypeArguments[0].IsSpanFormattable();
            }
            if(!p1IsSpanFormattable) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateSingleGenericArgMethodInvoker(toAppend, tupleType, foundMatch, item1Type);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildSingleFieldAppendRangeFromToInvoker<T>(T toAppend, Type tupleType, Type item1Type
      , Type item2Type
      , Type item3Type)
    {
        MethodInfo? foundMatch      = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];

            if (mi.Name != nameof(AppendFromToRange) || !mi.IsStatic) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            if(methodParams[1].ParameterType == typeof(int)) continue;
            if(methodParams[2].ParameterType == typeof(int)) continue;
            var firstParamType = methodParams[0].ParameterType;
            if(item1Type != firstParamType) continue;
            
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateSingleGenericArgMethodInvoker(toAppend, tupleType, foundMatch, item1Type);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildSingleFieldAppendRangeInvoker<T>(T toAppend, Type tupleType, Type item1Type
      , Type item2Type)
    {
        MethodInfo? foundMatch      = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];

            if (mi.Name != nameof(AppendFromRange) || !mi.IsStatic) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            if(methodParams[1].ParameterType == typeof(int)) continue;
             
            var firstParamType = methodParams[0].ParameterType;
            if(item1Type != firstParamType) continue;
            
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateSingleGenericArgMethodInvoker(toAppend, tupleType, foundMatch, item1Type);
        return invokeAppend;
    }

    protected Action<T, IStyledTypeStringAppender> TryBuildSingleFieldStructStylerInvoker<T>(T toAppend, Type tupleType, Type item1Type
      , Type item2Type)
    {
        var item1IsNullable = item1Type.IsGenericType && item1Type.GetGenericTypeDefinition() == typeof(Nullable<>);

        MethodInfo? foundMatch      = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];

            if (mi.Name != nameof(AppendStruct) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var firstParamType = methodParams[0].ParameterType;
            if(!firstParamType.IsValueType) continue;
            var p1IsNullable   = item1Type.IsGenericType && item1Type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if(p1IsNullable != item1IsNullable) continue;
            var secondParamType = methodParams[0].ParameterType;
            if(!secondParamType.IsGenericType) continue;
            if(!secondParamType.GenericTypeArguments[0].IsAssignableFrom(item1Type)) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateSingleGenericArgMethodInvoker(toAppend, tupleType, foundMatch, item1Type);
        return invokeAppend;
    }
}
