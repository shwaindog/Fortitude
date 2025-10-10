// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IFormatExpectation
{
    Type InputType { get; }
    Type SpanFormattableType { get; }
    
    bool IsNullableStruct { get; }
    
    object? UnknownInput { get; }

    string ExpectedOutput { get; }

    string? FormatString { get; }

    bool HasDefault { get; }
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T Input { get; }
}

public record FmtExpect<T>
    (T Input, string ExpectedOutput, string? FormatString = null, bool HasDefault = false, T? DefaultValue = default)
    : ITypedFormatExpectation<T> where T : ISpanFormattable
{
    public object? UnknownInput => Input;
    public Type InputType => typeof(T);
    public Type SpanFormattableType => typeof(T);
    public bool IsNullableStruct => false;
};

public record NullFmtStructExpect<T>
    (T? Input, string ExpectedOutput, string? FormatString = null, bool HasDefault = false, T? DefaultValue = null) : ITypedFormatExpectation<T?>
    where T : struct, ISpanFormattable
{
    public object? UnknownInput => Input;
    public Type InputType => typeof(T?);
    public Type SpanFormattableType => typeof(T);
    
    public bool IsNullableStruct => true;
}

public static class SpanFormattableTestData
{
    public static readonly FmtExpect<byte> ByteDefault = new(0, "128");
    public static readonly FmtExpect<byte> ByteNoFormatString = new(128, "128");
    public static readonly FmtExpect<byte> ByteNo2DpFormatFormatString = new(128, "128.00", "C2");
    public static readonly FmtExpect<byte> ByteRightPadding20AsStringFormatString =
        new(128, "128.00", "\"{0,-20}\"");
    
    public static readonly NullFmtStructExpect<byte> NullableByteDefault = new(0, "128");
    public static readonly NullFmtStructExpect<byte> NullableByteNull = new(null, "null");
    public static readonly NullFmtStructExpect<byte> NullableByteNoFormatString = new(128, "128");
    public static readonly NullFmtStructExpect<byte> NullableByteNo2DpFormatFormatString = new(128, "128.00", "C2");
    public static readonly NullFmtStructExpect<byte> NullableByteRightPadding20AsStringFormatString =
        new(128, "128.00", "\"{0,20}\"");
    
    private static readonly ConcurrentDictionary<Type, Func<IFormatExpectation, ScaffoldingPartEntry, bool, IStringBearer>> dynStructInvokers = new();


    public static readonly IFormatExpectation[] AllSpanFormattableExpectations =
    [
        ByteDefault
      , ByteNoFormatString
      , ByteNo2DpFormatFormatString
      , ByteRightPadding20AsStringFormatString
      , NullableByteDefault
      , NullableByteNull
      , NullableByteNoFormatString
      , NullableByteNo2DpFormatFormatString
      , NullableByteRightPadding20AsStringFormatString  
    ];


    public static IStringBearer CreateStringBearerWithValueFor<T>(this ITypedFormatExpectation<T> forTestData, ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.CreateStringBearerFunc(forTestData.InputType)();
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
        {
            isObjectMold.Value = forTestData.Input;
        }
        else 
        {
            ((IMoldSupportedValue<T>)createdStringBearer).Value = forTestData.Input;    
        }
        if (forTestData.FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = forTestData.FormatString;
        }
        return createdStringBearer;
    }


    public static IStringBearer CreateStringBearerWithNullableValueFor<T>(this ITypedFormatExpectation<T?> forTestData, ScaffoldingPartEntry scaffoldEntry)
        where T : struct
    {
        var createdStringBearer = scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(forTestData.SpanFormattableType)()
            : scaffoldEntry.CreateStringBearerFunc(forTestData.InputType)();
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
        {
            isObjectMold.Value = forTestData.Input;
        }
        else 
        {
            ((IMoldSupportedValue<T?>)createdStringBearer).Value = forTestData.Input;    
        }
        if (forTestData.FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = forTestData.FormatString;
        }
        return createdStringBearer;
    }

    public static IStringBearer CreateStringBearerWithPossibleNullableStructValueFor<T>(this IFormatExpectation forTestData, ScaffoldingPartEntry scaffoldEntry, bool isNullable)
        where T : struct
    {
        if(isNullable)
        {
            return CreateStringBearerWithNullableValueFor((ITypedFormatExpectation<T?>)forTestData, scaffoldEntry);
        }
        return CreateStringBearerWithValueFor((ITypedFormatExpectation<T>)forTestData, scaffoldEntry);
    }

    public static IStringBearer CreateStringBearerWithValueFor<T>(this IFormatExpectation forTestData, ScaffoldingPartEntry scaffoldEntry) =>
        CreateStringBearerWithValueFor((ITypedFormatExpectation<T>)forTestData, scaffoldEntry);


    public static IStringBearer CreateStringBearerWithValueFor(this IFormatExpectation forTestData, ScaffoldingPartEntry scaffoldEntry)
    {
        var input = forTestData.InputType.GetDefaultForUnderlyingNullableOrThis();
        var isNullable = forTestData.IsNullableStruct;
        
        switch (input)
        {
            case bool :       return CreateStringBearerWithPossibleNullableStructValueFor<bool>(forTestData, scaffoldEntry, isNullable);        
            case byte :       return CreateStringBearerWithPossibleNullableStructValueFor<byte>(forTestData, scaffoldEntry, isNullable);
            case sbyte :      return CreateStringBearerWithPossibleNullableStructValueFor<sbyte>(forTestData, scaffoldEntry, isNullable);       
            case char :       return CreateStringBearerWithPossibleNullableStructValueFor<char>(forTestData, scaffoldEntry, isNullable);         
            case short :      return CreateStringBearerWithPossibleNullableStructValueFor<short>(forTestData, scaffoldEntry, isNullable);       
            case ushort :     return CreateStringBearerWithPossibleNullableStructValueFor<ushort>(forTestData, scaffoldEntry, isNullable);     
            case Half :       return CreateStringBearerWithPossibleNullableStructValueFor<Half>(forTestData, scaffoldEntry, isNullable);    
            case int :        return CreateStringBearerWithPossibleNullableStructValueFor<int>(forTestData, scaffoldEntry, isNullable);           
            case uint :       return CreateStringBearerWithPossibleNullableStructValueFor<uint>(forTestData, scaffoldEntry, isNullable);         
            case nint :       return CreateStringBearerWithPossibleNullableStructValueFor<nint>(forTestData, scaffoldEntry, isNullable);         
            case float :      return CreateStringBearerWithPossibleNullableStructValueFor<float>(forTestData, scaffoldEntry, isNullable);       
            case long :       return CreateStringBearerWithPossibleNullableStructValueFor<long>(forTestData, scaffoldEntry, isNullable);         
            case ulong :      return CreateStringBearerWithPossibleNullableStructValueFor<ulong>(forTestData, scaffoldEntry, isNullable);       
            case double :     return CreateStringBearerWithPossibleNullableStructValueFor<double>(forTestData, scaffoldEntry, isNullable);     
            case decimal :    return CreateStringBearerWithPossibleNullableStructValueFor<decimal>(forTestData, scaffoldEntry, isNullable);   
            case Int128 :     return CreateStringBearerWithPossibleNullableStructValueFor<Int128>(forTestData, scaffoldEntry, isNullable);     
            case UInt128 :    return CreateStringBearerWithPossibleNullableStructValueFor<UInt128>(forTestData, scaffoldEntry, isNullable);   
            case BigInteger : return CreateStringBearerWithPossibleNullableStructValueFor<BigInteger>(forTestData, scaffoldEntry, isNullable); 
            case Complex :    return CreateStringBearerWithPossibleNullableStructValueFor<Complex>(forTestData, scaffoldEntry, isNullable);    
            case DateTime :   return CreateStringBearerWithPossibleNullableStructValueFor<DateTime>(forTestData, scaffoldEntry, isNullable); 
            case DateOnly :   return CreateStringBearerWithPossibleNullableStructValueFor<DateOnly>(forTestData, scaffoldEntry, isNullable); 
            case TimeSpan :   return CreateStringBearerWithPossibleNullableStructValueFor<TimeSpan>(forTestData, scaffoldEntry, isNullable); 
            case TimeOnly :   return CreateStringBearerWithPossibleNullableStructValueFor<TimeOnly>(forTestData, scaffoldEntry, isNullable); 
            case Rune :       return CreateStringBearerWithPossibleNullableStructValueFor<Rune>(forTestData, scaffoldEntry, isNullable);     
            case Guid :       return CreateStringBearerWithPossibleNullableStructValueFor<Guid>(forTestData, scaffoldEntry, isNullable);         
            case IPNetwork :  return CreateStringBearerWithPossibleNullableStructValueFor<IPNetwork>(forTestData, scaffoldEntry, isNullable);
            case ValueType :
                var type = input.GetType(); 
                if (!dynStructInvokers.TryGetValue(type, out var invokerFunc))
                {
                    invokerFunc = CreateStringBearerStructInvoker(type);
                    dynStructInvokers.TryAdd(type, invokerFunc);
                }
                return invokerFunc(forTestData, scaffoldEntry, isNullable);      
            case Version :    return CreateStringBearerWithValueFor<Version>(forTestData, scaffoldEntry);      
            case IPAddress :  return CreateStringBearerWithValueFor<IPAddress>(forTestData, scaffoldEntry);  
            case Uri :        return CreateStringBearerWithValueFor<Uri>(forTestData, scaffoldEntry);           
        }
        throw new ArgumentException("Unexpected SpanFormattable Type or non SpanFormattable Type - " + input?.GetType().FullName);
    }
    
    public static Func<IFormatExpectation, ScaffoldingPartEntry, bool, IStringBearer> CreateStringBearerStructInvoker(Type valueType)
    {
        var genTypeDefMeth = typeof(SpanFormattableTestData)
            .GetMethod("CreateStringBearerWithPossibleNullableStructValueFor", BindingFlags.Static | BindingFlags.Public
                     , [typeof(IFormatExpectation), typeof(ScaffoldingPartEntry), typeof(bool)]);
        
        var generified = genTypeDefMeth!.MakeGenericMethod(valueType);
        
        var helperMethod =
            new DynamicMethod($"{valueType.Name}_Styled_ToString", typeof(IStringBearer),
                              [typeof(IFormatExpectation), typeof(ScaffoldingPartEntry), typeof(bool)]
                            , typeof(SpanFormattableTestData).Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Call, generified);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(Func<IFormatExpectation, ScaffoldingPartEntry, bool, IStringBearer>));
        return (Func<IFormatExpectation, ScaffoldingPartEntry, bool, IStringBearer>)methodInvoker;
    }
}
